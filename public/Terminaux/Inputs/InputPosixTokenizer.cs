//
// Terminaux  Copyright (C) 2023-2025  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Inputs.Pointer;
using Terminaux.Sequences.Builder;
using static Terminaux.Base.Extensions.Native.NativeMethods;

namespace Terminaux.Inputs
{
    internal class InputPosixTokenizer
    {
        private readonly char[] charRead;

        private readonly Dictionary<char, (ConsoleKey, ConsoleModifiers)> arrowKeys = new()
        {
            { 'A', (ConsoleKey.UpArrow, 0) },
            { 'x', (ConsoleKey.UpArrow, 0) },
            { 'a', (ConsoleKey.UpArrow, ConsoleModifiers.Shift) },
            { 'B', (ConsoleKey.DownArrow, 0) },
            { 'r', (ConsoleKey.DownArrow, 0) },
            { 'b', (ConsoleKey.DownArrow, ConsoleModifiers.Shift) },
            { 'C', (ConsoleKey.RightArrow, 0) },
            { 'v', (ConsoleKey.RightArrow, 0) },
            { 'c', (ConsoleKey.RightArrow, ConsoleModifiers.Shift) },
            { 'D', (ConsoleKey.LeftArrow, 0) },
            { 't', (ConsoleKey.LeftArrow, 0) },
            { 'd', (ConsoleKey.LeftArrow, ConsoleModifiers.Shift) },
        };

        private readonly Dictionary<char, (ConsoleKey, ConsoleModifiers)> functionKeys = new()
        {
            { 'P', (ConsoleKey.F1, 0) },
            { 'Q', (ConsoleKey.F2, 0) },
            { 'R', (ConsoleKey.F3, 0) },
            { 'S', (ConsoleKey.F4, 0) },
            { 'T', (ConsoleKey.F5, 0) },
            { 'U', (ConsoleKey.F6, 0) },
            { 'V', (ConsoleKey.F7, 0) },
            { 'W', (ConsoleKey.F8, 0) },
            { 'X', (ConsoleKey.F9, 0) },
            { 'Y', (ConsoleKey.F10, 0) },
            { 'Z', (ConsoleKey.F11, 0) },
            { '[', (ConsoleKey.F12, 0) },
        };

        private readonly Dictionary<char, (ConsoleKey, ConsoleModifiers)> keypadKeysVt100 = new()
        {
            { 'p', (ConsoleKey.NumPad0, 0) },
            { 'q', (ConsoleKey.NumPad1, 0) },
            { 'r', (ConsoleKey.NumPad2, 0) },
            { 's', (ConsoleKey.NumPad3, 0) },
            { 't', (ConsoleKey.NumPad4, 0) },
            { 'u', (ConsoleKey.NumPad5, 0) },
            { 'v', (ConsoleKey.NumPad6, 0) },
            { 'w', (ConsoleKey.NumPad7, 0) },
            { 'x', (ConsoleKey.NumPad8, 0) },
            { 'y', (ConsoleKey.NumPad9, 0) },
            { 'm', (ConsoleKey.Subtract, 0) },
            { 'l', (ConsoleKey.OemComma, 0) },
            { 'n', (ConsoleKey.OemPeriod, 0) },
            { 'M', (ConsoleKey.Enter, 0) },
        };

        private readonly Dictionary<char, (ConsoleKey, ConsoleModifiers)> otherKeys = new()
        {
            { 'E', (ConsoleKey.NoName, 0) },
            { 'u', (ConsoleKey.NoName, 0) },
            { 'H', (ConsoleKey.Home, 0) },
            { 'F', (ConsoleKey.End, 0) },
            { 'q', (ConsoleKey.End, 0) },
            { 'j', (ConsoleKey.Multiply, 0) },
            { 'k', (ConsoleKey.Add, 0) },
            { 'm', (ConsoleKey.Subtract, 0) },
            { 'M', (ConsoleKey.Enter, 0) },
            { 'n', (ConsoleKey.Delete, 0) },
            { 'o', (ConsoleKey.Divide, 0) },
            { 'p', (ConsoleKey.Insert, 0) },
            { 's', (ConsoleKey.PageDown, 0) },
            { 'y', (ConsoleKey.PageUp, 0) },
            { 'w', (ConsoleKey.Home, 0) },
        };

        internal InputEventInfo[] Parse()
        {
            List<InputEventInfo> eventInfos = [];

            // Run a main loop
            for (int pos = 0; pos < charRead.Length; pos++)
            {
                // Get a character
                char charValue = charRead[pos];

                // Check to see if we've pressed ESC
                int bytesToAdd;
                if (charValue == VtSequenceBasicChars.EscapeChar)
                {
                    // We've just obtained the escape character. We need to perform checks.
                    if (TryParseCtlSeqs(pos, out var evt, out bytesToAdd))
                    {
                        // Add this sequence
                        eventInfos.Add(evt);
                        pos += bytesToAdd - 1;
                        continue;
                    }

                    // Obtain the mouse info
                    if (TryParseMouse(pos, out evt, out bytesToAdd))
                    {
                        pos += bytesToAdd - 1;

                        // Check the event before adding it
                        if (evt.EventType == InputEventType.Mouse)
                            eventInfos.Add(evt);
                        continue;
                    }

                    // Obtain the position report info
                    if (TryParsePositionReporting(pos, out evt, out bytesToAdd))
                    {
                        // Add this sequence
                        eventInfos.Add(evt);
                        pos += bytesToAdd - 1;
                        continue;
                    }

                    // Obtain the ALT sequence info
                    if (TryParseAltSequence(pos, out evt, out bytesToAdd))
                    {
                        // Add this sequence
                        eventInfos.Add(evt);
                        pos += bytesToAdd - 1;
                        continue;
                    }

                    // Fallback to standalone ESC character
                    var cki = new ConsoleKeyInfo('\x1b', ConsoleKey.Escape, false, false, false);
                    evt = new(null, cki, null);
                    eventInfos.Add(evt);
                }
                else
                {
                    // Not an escape sequence, but could be a UTF-8 character. Add it.
                    if (TryParseSingleCharacter(pos, out var evt, out bytesToAdd))
                    {
                        // Add this sequence
                        eventInfos.Add(evt);
                        pos += bytesToAdd - 1;
                        continue;
                    }
                }
            }

            return [.. eventInfos];
        }

        private bool TryParseCtlSeqs(int idx, out InputEventInfo evt, out int advance)
        {
            // Set initial values
            evt = new();
            advance = 0;

            // Check to see if we have <ESC>[ or <ESC>O sequences, and advance two bytes to indicate that we've seen
            // the two characters before the prefix.
            if (!TryGetChar(idx + 1, out char prefix) || (prefix != '[' && prefix != 'O'))
                return false;
            advance++;

            // Check the parameter character
            if (!TryGetChar(idx + 2, out char param))
                return false;
            advance++;

            // Parse the sequence now
            if (prefix == '[')
            {
                // Normal mode parsing or any other key
                if (arrowKeys.TryGetValue(param, out var key) ||
                    otherKeys.TryGetValue(param, out key))
                {
                    // If it's "M", we could have a mouse event using the X10 mouse protocol.
                    if (param == 'M')
                    {
                        if (TryGetChar(idx + 3, out char mouseParam1) &&
                            TryGetChar(idx + 4, out char mouseParam2) &&
                            TryGetChar(idx + 5, out char mouseParam3))
                        {
                            // Get the button states and change them as necessary
                            PosixButtonState state = (PosixButtonState)(mouseParam1 & 0b11);
                            PosixButtonModifierState modState = (PosixButtonModifierState)(mouseParam1 & 0b11100);
                            if (mouseParam1 >= 64 && mouseParam1 < 96)
                                state = PosixButtonState.Movement;
                            if (mouseParam1 >= 96 && mouseParam1 % 2 == 0)
                                state = PosixButtonState.WheelUp;
                            else if (mouseParam1 >= 97)
                                state = PosixButtonState.WheelDown;

                            // Now, translate them to something Terminaux understands
                            (_, PointerButtonPress press, _) = Input.ProcessPointerEventPosix(state, modState);
                            if ((Input.EnableMovementEvents || press != PointerButtonPress.Moved) &&
                                (byte)mouseParam2 >= 32 && (byte)mouseParam2 <= 127 &&
                                (byte)mouseParam3 >= 32 && (byte)mouseParam3 <= 127)
                                return false;
                        }
                    }

                    // It's one of the arrow keys! Parse it.
                    var cki = new ConsoleKeyInfo('\0', key.Item1, key.Item2.HasFlag(ConsoleModifiers.Shift), false, false);
                    evt = new(null, cki, null);
                    advance++;
                    return true;
                }

                // Not an arrow key, but we can check for CTRL + Arrow keys
                if (param == '1')
                {
                    // It's either F5 to F8, or CTRL + Arrow.
                    if (!TryGetChar(idx + 3, out char secParam))
                        return false;
                    advance++;

                    // Try to parse the second parameter to distinguish between CTRL + Arrow key and F5 to F8
                    if (secParam == ';')
                    {
                        // CTRL + Arrow key has been pressed.
                        if (CheckChars(idx + 4, ['5', 'A']) ||
                            CheckChars(idx + 4, ['5', 'B']) ||
                            CheckChars(idx + 4, ['5', 'C']) ||
                            CheckChars(idx + 4, ['5', 'D']))
                        {
                            advance += 2;
                            char arrowKey = charRead[idx + 5];
                            var arrowKeyInfo = arrowKeys[arrowKey];
                            var cki = new ConsoleKeyInfo('\0', arrowKeyInfo.Item1, false, false, true);
                            evt = new(null, cki, null);
                            return true;
                        }
                    }
                    else if (CheckChar(secParam, ['5', '7', '8', '9']) && CheckChar(idx + 4, ['~', '^', '$', '@']))
                    {
                        // F5 to F8 has been pressed.
                        advance += 2;

                        // Map the result
                        ConsoleKey ck =
                            secParam == '5' ? ConsoleKey.F5 :
                            secParam == '6' ? ConsoleKey.F6 :
                            secParam == '7' ? ConsoleKey.F7 :
                            secParam == '8' ? ConsoleKey.F8 : 0;
                        char modTag = charRead[idx + 4];
                        var (shift, ctrl) = GetRxvtModifiers(modTag);
                        var cki = new ConsoleKeyInfo('\0', ck, shift, false, ctrl);
                        evt = new(null, cki, null);
                        return true;
                    }
                    else if (CheckChar(secParam, ['~', '^', '$', '@']))
                    {
                        var (shift, ctrl) = GetRxvtModifiers(secParam);
                        var cki = new ConsoleKeyInfo('\0', ConsoleKey.Home, shift, false, ctrl);
                        evt = new(null, cki, null);
                        advance++;
                        return true;
                    }
                }
                else if (CheckChar(param, ['2', '3', '5', '6']))
                {
                    // Check for tilde mapped to Insert, Delete, PageUp, or PageDown
                    if (!TryGetChar(idx + 3, out char secParam))
                        return false;
                    advance++;

                    // If this is a tilde, we assume that the key is Insert, Delete, PageUp, or PageDown
                    if (CheckChar(secParam, ['~', '^', '$', '@']))
                    {
                        // User pressed Insert, Delete, PageUp, or PageDown. Add it.
                        ConsoleKey ck =
                            param == '2' ? ConsoleKey.Insert :
                            param == '3' ? ConsoleKey.Delete :
                            param == '5' ? ConsoleKey.PageUp :
                            param == '6' ? ConsoleKey.PageDown : 0;
                        var (shift, ctrl) = GetRxvtModifiers(secParam);
                        var cki = new ConsoleKeyInfo('\0', ck, shift, false, ctrl);
                        evt = new(null, cki, null);
                        advance++;
                        return true;
                    }

                    // Check for F9 to F16
                    if (param == '2' && CheckChar(secParam, ['0', '1', '3', '4', '5', '6', '8', '9']) && CheckChar(idx + 4, ['~', '^', '$', '@']))
                    {
                        // F9 to F16 has been pressed.
                        advance += 2;

                        // Map the result
                        ConsoleKey ck =
                            secParam == '0' ? ConsoleKey.F9 :
                            secParam == '1' ? ConsoleKey.F10 :
                            secParam == '3' ? ConsoleKey.F11 :
                            secParam == '4' ? ConsoleKey.F12 :
                            secParam == '5' ? ConsoleKey.F13 :
                            secParam == '6' ? ConsoleKey.F14 :
                            secParam == '8' ? ConsoleKey.F15 :
                            secParam == '9' ? ConsoleKey.F16 : 0;
                        char modTag = charRead[idx + 4];
                        var (shift, ctrl) = GetRxvtModifiers(modTag);
                        var cki = new ConsoleKeyInfo('\0', ck, shift, false, ctrl);
                        evt = new(null, cki, null);
                        return true;
                    }
                    else if (param == '3' && CheckChar(secParam, ['1', '2', '3', '4']) && CheckChar(idx + 4, ['~', '^', '$', '@']))
                    {
                        // F17 to F20 has been pressed.
                        advance += 2;

                        // Map the result
                        ConsoleKey ck =
                            secParam == '1' ? ConsoleKey.F17 :
                            secParam == '2' ? ConsoleKey.F18 :
                            secParam == '3' ? ConsoleKey.F19 :
                            secParam == '4' ? ConsoleKey.F20 : 0;
                        char modTag = charRead[idx + 4];
                        var (shift, ctrl) = GetRxvtModifiers(modTag);
                        var cki = new ConsoleKeyInfo('\0', ck, shift, false, ctrl);
                        evt = new(null, cki, null);
                        return true;
                    }
                }
                else if (CheckChar(param, ['7', '4', '8']))
                {
                    // Check for tilde mapped to Home or End
                    if (!TryGetChar(idx + 3, out char secParam))
                        return false;
                    advance++;

                    // If this is a tilde, we assume that the key is Home or End
                    if (CheckChar(secParam, ['~', '^', '$', '@']))
                    {
                        // User pressed Home or End. Add it.
                        ConsoleKey ck =
                            param == '7' ? ConsoleKey.Home :
                            param == '4' ? ConsoleKey.End :
                            param == '8' ? ConsoleKey.End : 0;
                        var (shift, ctrl) = GetRxvtModifiers(secParam);
                        var cki = new ConsoleKeyInfo('\0', ck, shift, false, ctrl);
                        evt = new(null, cki, null);
                        advance++;
                        return true;
                    }
                }
            }
            else if (prefix == 'O')
            {
                // Application mode parsing
                if (arrowKeys.TryGetValue(param, out var key) ||
                    functionKeys.TryGetValue(param, out key) ||
                    keypadKeysVt100.TryGetValue(param, out key) ||
                    otherKeys.TryGetValue(param, out key))
                {
                    // It's one of the arrow keys, function keys, or keypad keys! Parse it.
                    var cki = new ConsoleKeyInfo('\0', key.Item1, key.Item2.HasFlag(ConsoleModifiers.Shift), false, false);
                    evt = new(null, cki, null);
                    advance++;
                    return true;
                }
            }
            return false;
        }

        private bool TryParseMouse(int idx, out InputEventInfo evt, out int advance)
        {
            // Set initial values
            evt = new();
            advance = 0;

            // Check to see if we have <ESC>[Mxxx or <ESC>[<x;x;x;[Mm] sequences, and advance one byte.
            if (!TryGetChar(idx + 1, out char prefix) || prefix != '[')
                return false;
            advance++;

            // Check the parameter character
            if (!TryGetChar(idx + 2, out char param))
                return false;
            advance++;

            // Parse the sequence now
            if (param == 'M')
            {
                // It's an X10 mouse protocol, so decode it.
                advance++;
                if (!TryGetChar(idx + 3, out char buttonChar))
                    return false;
                advance++;
                if (!TryGetChar(idx + 4, out char posX))
                    return false;
                advance++;
                if (!TryGetChar(idx + 5, out char posY))
                    return false;
                advance++;

                // Now, read the button, X, and Y positions
                byte button = (byte)buttonChar;
                byte x = (byte)(posX - 32);
                byte y = (byte)(posY - 32);
                x -= 1;
                y -= 1;

                // Get the button states and change them as necessary
                PosixButtonState state = (PosixButtonState)(button & 0b11);
                PosixButtonModifierState modState = (PosixButtonModifierState)(button & 0b11100);
                if (button >= 64 && button < 96)
                    state = PosixButtonState.Movement;
                if (button >= 96 && button % 2 == 0)
                    state = PosixButtonState.WheelUp;
                else if (button >= 97)
                    state = PosixButtonState.WheelDown;
                ConsoleLogger.Debug($"X10: [{button}: {state} {modState}] X={x} Y={y}");

                // Now, translate them to something Terminaux understands
                (PointerButton buttonPtr, PointerButtonPress press, PointerModifiers mods) = Input.ProcessPointerEventPosix(state, modState);
                if (Input.EnableMovementEvents || press != PointerButtonPress.Moved)
                {
                    var eventContext = Input.GenerateContext(x, y, buttonPtr, press, mods);
                    Input.context = eventContext;
                    evt = new(eventContext, null, null);
                }
                return true;
            }
            else if (param == '<')
            {
                // It's an SGR mouse protocol. Three semicolons and an ending: M (pressed) or m (released).
                // We have this sequence: <ESC>[<x;x;x;[Mm]. We have to parse the below parameters:
                //
                //   - Cb: button parameter (button number [Lo 2 bits] + mods [remaining Hi bits])
                //   - ;: Semicolon
                //   - Cx: X coordinate (one-based, so we need to make it an index)
                //   - ;: Semicolon
                //   - Cy: Y coordinate (one-based, so we need to make it an index)
                //   - ;: Semicolon
                //   - M/m: Press or release
                int digitIdx = idx + 3;
                if (!TryGetChar(digitIdx, out char digit) || !char.IsDigit(digit))
                    return false;
                advance++;

                // Now, run a loop until we reach the semicolon, which separates X from Y.
                List<char> bDigits = [];
                List<char> xDigits = [];
                List<char> yDigits = [];
                int parseMode = 0;
                while (TryGetChar(digitIdx, out digit))
                {
                    // First, check to see if we've reached ';', 'm', or 'M'
                    if (digit == 'm' || digit == 'M')
                    {
                        // Ensure we don't get illegal 'm' or 'M'
                        if (parseMode != 2)
                            return false;

                        // Convert a list of digits to numbers
                        int b = NumberizeArray(bDigits);
                        int x = NumberizeArray(xDigits) - 1;
                        int y = NumberizeArray(yDigits) - 1;

                        // Now, we'll parse the values, so we need to start with the raw button and the
                        // horizontal/vertical wheel press state. We also need to know whether we're
                        // releasing the button.
                        int finalButton = b & 0x3;
                        bool wheelScroll = (b & 0x40) != 0;
                        bool releasing = digit == 'm';
                        bool moving = (b & 0x20) != 0;

                        // What modifiers did we press?
                        PosixButtonModifierState modState = 0;
                        if ((b & (int)PosixButtonModifierState.Shift) != 0)
                            modState |= PosixButtonModifierState.Shift;
                        if ((b & (int)PosixButtonModifierState.Alt) != 0)
                            modState |= PosixButtonModifierState.Alt;
                        if ((b & (int)PosixButtonModifierState.Control) != 0)
                            modState |= PosixButtonModifierState.Control;

                        // Convert the final button value to POSIX button state
                        PosixButtonState state =
                            moving ? PosixButtonState.Movement :
                            releasing ? PosixButtonState.Released :
                            wheelScroll ?
                                (b & 0x1) == 0 ? PosixButtonState.WheelUp : PosixButtonState.WheelDown :
                            finalButton == 0 ? PosixButtonState.Left :
                            finalButton == 1 ? PosixButtonState.Middle :
                            finalButton == 2 ? PosixButtonState.Right : PosixButtonState.Movement;
                        ConsoleLogger.Debug($"SGR: [{finalButton}: {state} {modState}] X={x} Y={y}");
                        (PointerButton buttonPtr, PointerButtonPress press, PointerModifiers mods) = Input.ProcessPointerEventPosix(state, modState);
                        if (Input.EnableMovementEvents || press != PointerButtonPress.Moved)
                        {
                            var eventContext = Input.GenerateContext(x, y, buttonPtr, press, mods);
                            Input.context = eventContext;
                            evt = new(eventContext, null, null);
                            advance += digitIdx - (idx + 2);
                        }
                        return true;
                    }
                    else if (digit == ';')
                    {
                        parseMode++;
                        digitIdx++;
                        continue;
                    }
                    else if (!char.IsDigit(digit))
                        return false;

                    // Now, we have a digit. Add it.
                    if (parseMode == 2)
                        yDigits.Add(digit);
                    else if (parseMode == 1)
                        xDigits.Add(digit);
                    else
                        bDigits.Add(digit);
                    digitIdx++;
                }
            }
            return false;
        }

        private bool TryParsePositionReporting(int idx, out InputEventInfo evt, out int advance)
        {
            // Set initial values
            evt = new();
            advance = 0;

            // Check to see if we have an <ESC>[<y>;<x>R sequence, and advance one byte.
            if (!TryGetChar(idx + 1, out char prefix) || prefix != '[')
                return false;
            advance++;

            // Check the first parameter for digit
            int digitIdx = idx + 2;
            if (!TryGetChar(digitIdx, out char digit) || !char.IsDigit(digit))
                return false;
            advance++;

            // Now, run a loop until we reach the semicolon, which separates X from Y.
            List<char> xDigits = [];
            List<char> yDigits = [];
            bool parsingY = true;
            while (TryGetChar(digitIdx, out digit))
            {
                // First, check to see if we've reached ';' or 'R'
                if (digit == 'R')
                {
                    // Ensure we don't get illegal 'R'
                    if (parsingY)
                        return false;

                    // Convert a list of digits to numbers and return them
                    int x = NumberizeArray(xDigits) - 1;
                    int y = NumberizeArray(yDigits) - 1;
                    evt = new(null, null, new(x, y));
                    advance += digitIdx - (idx + 1);
                    return true;
                }
                else if (digit == ';')
                {
                    parsingY = false;
                    digitIdx++;
                    continue;
                }
                else if (!char.IsDigit(digit))
                    return false;

                // Now, we have a digit. Add it.
                if (parsingY)
                    yDigits.Add(digit);
                else
                    xDigits.Add(digit);
                digitIdx++;
            }
            return false;
        }

        private bool TryParseAltSequence(int idx, out InputEventInfo evt, out int advance)
        {
            // Set initial values
            evt = new();
            advance = 0;

            // Check to see if we have an <ESC><char> sequence, and advance one byte.
            if (!TryGetChar(idx + 1, out char altChar) || altChar == VtSequenceBasicChars.EscapeChar)
                return false;
            advance += 2;

            // Parse the sequence now
            var cki = TryParseSingleCharacterCki(altChar, false);
            evt = new(null, cki, null);
            return true;
        }

        private bool TryParseSingleCharacter(int idx, out InputEventInfo evt, out int advance)
        {
            // Set initial values
            advance = 0;

            // Check to see if we have a single character.
            char character = charRead[idx];
            advance++;

            // Parse the sequence now
            var cki = TryParseSingleCharacterCki(character, false);
            evt = new(null, cki, null);
            return true;
        }

        private ConsoleKeyInfo TryParseSingleCharacterCki(char character, bool isAlt)
        {
            // Some variables
            bool isCtrl, isShift = false;
            char finalChar = character;

            // Determine the pressed modifiers
            bool ctrlLetterPressed = CharInRange(character, (char)1, (char)26);
            bool ctrlDigitPressed = CharInRange(character, (char)28, (char)31) || character == '\0';

            // Determine the modifiers to pass to the constructor
            if (CharInRange(character, 'A', 'Z'))
                isShift = true;
            isCtrl = ctrlLetterPressed || ctrlDigitPressed;
            if (character == '\b' || character == '\t' || character == '\n' || character == '\r')
                isCtrl = false;

            // Parse the sequence now
            ConsoleKey key = character switch
            {
                // Backspace, tab, and more
                '\b' => ConsoleKey.Backspace,
                '\u007f' => ConsoleKey.Backspace,
                '\t' => ConsoleKey.Tab,
                '\r' or '\n' => ConsoleKey.Enter,
                ' ' => ConsoleKey.Spacebar,

                // Escape
                VtSequenceBasicChars.EscapeChar => ConsoleKey.Escape,

                // Math operators and numeric digits
                '*' => ConsoleKey.Multiply,
                '/' => ConsoleKey.Divide,
                '-' => ConsoleKey.Subtract,
                '+' => ConsoleKey.Add,
                ',' => ConsoleKey.OemComma,
                '.' => ConsoleKey.OemPeriod,

                // Letters
                _ when CharInRange(character, 'a', 'z') => ConsoleKey.A + character - 'a',
                _ when CharInRange(character, 'A', 'Z') => ConsoleKey.A + character - 'A',

                // Digits
                _ when CharInRange(character, '0', '9') => ConsoleKey.D0 + character - '0',

                // Control characters
                _ when CharInRange(character, (char)1, (char)26) => ConsoleKey.A + character - 1,
                _ when CharInRange(character, (char)28, (char)31) => ConsoleKey.D4 + character - 28,
                '\0' => ConsoleKey.D2,

                // They default to 0
                '!' or '@' or '#' or '$' or '%' or '^' or '&' or '&' or '*' or '(' or ')' => default,
                '=' => default,
                _ => default
            };

            // Finalize the modifiers
            if (isAlt)
                isAlt = character != default;

            // Get the final character
            finalChar =
                ctrlLetterPressed && isAlt ? default :
                ctrlDigitPressed ? default :
                character;
            var cki = new ConsoleKeyInfo(finalChar, key, isShift, isAlt, isCtrl);
            return cki;
        }

        private int NumberizeArray(List<char> numbers)
        {
            int num = 0;
            for (int i = 0; i < numbers.Count; i++)
                num += (int)(MapDigitNum(numbers[i]) * Math.Pow(10, numbers.Count - (i + 1)));
            return num;
        }

        private bool CheckChar(int idx, char[] expected)
        {
            if (idx > charRead.Length)
                return false;
            char actual = charRead[idx];
            return CheckChar(actual, expected);
        }

        private bool CheckChar(char character, char[] expected)
        {
            // Check character one by one
            for (int i = 0; i < expected.Length; i++)
            {
                char exp = expected[i];
                if (character == exp)
                    return true;
            }
            return false;
        }

        private bool CheckChars(int idx, char[] expected)
        {
            if (idx + expected.Length > charRead.Length)
                return false;

            // Check character one by one
            for (int i = 0; i < expected.Length; i++)
            {
                char exp = expected[i];
                char actual = charRead[idx + i];
                if (actual != exp)
                    return false;
            }

            return true;
        }

        private (bool shift, bool ctrl) GetRxvtModifiers(char mod) =>
            mod switch
            {
                '^' => (false, true),
                '$' => (true, false),
                '@' => (true, true),
                _ => default,
            };

        private int MapDigitNum(char digit) =>
            digit switch
            {
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                _ => 0,
            };

        private bool TryGetChar(int idx, out char character)
        {
            character = '\0';
            if (idx < 0 || idx >= charRead.Length)
                return false;
            character = charRead[idx];
            return true;
        }

        private bool CharInRange(char ch, char start, char end) =>
            (uint)(ch - start) <= (uint)(end - start);

        internal InputPosixTokenizer(char[] charRead)
        {
            this.charRead = charRead;
        }
    }
}
