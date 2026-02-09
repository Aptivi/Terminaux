//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using SpecProbe.Software.Platform;
using System;
using System.Threading;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry;
using Terminaux.Inputs;
using Terminaux.Sequences.Builder;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Shell.Scripting;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Textify.Tools.Placeholder;

namespace Terminaux.Base.Wrappers
{
    /// <summary>
    /// Base console wrapper class for the <see cref="Console"/> class to ensure safety against dumb consoles and Windows-only features.
    /// </summary>
    public class BaseConsoleWrapper
    {
        /// <summary>
        /// Checks to see if the console has moved. Only set this to true if the console has really moved, for example, each call to
        /// setting cursor position, key reading, writing text, etc.
        /// </summary>
        protected bool _moved = false;

        /// <summary>
        /// Checks to see if the console is dumb
        /// </summary>
        public virtual bool IsDumb =>
            ConsoleChecker.IsDumb;

        /// <summary>
        /// Has the console moved? Should be set by Write*, Set*, and all console functions that have to do with moving the console.
        /// </summary>
        public virtual bool MovementDetected
        {
            get
            {
                bool moved = _moved;
                _moved = false;
                return moved;
            }
        }

        /// <summary>
        /// The cursor left position
        /// </summary>
        public virtual int CursorLeft
        {
            get
            {
                if (IsDumb)
                    return 0;
                if (ConsoleMode.IsRaw)
                {
                    Write("\x1b[6n");
                    while (true)
                    {
                        var data = Input.ReadPointerOrKeyNoBlock(InputEventType.Position);
                        if (data.ReportedPos is Coordinate coord)
                            return coord.X;
                    }
                }
                return Console.CursorLeft;
            }
            set => SetCursorLeft(value);
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public virtual int CursorTop
        {
            get
            {
                if (IsDumb)
                    return 0;
                if (ConsoleMode.IsRaw)
                {
                    Write("\x1b[6n");
                    while (true)
                    {
                        var data = Input.ReadPointerOrKeyNoBlock(InputEventType.Position);
                        if (data.ReportedPos is Coordinate coord)
                            return coord.Y;
                    }
                }
                return Console.CursorTop;
            }
            set => SetCursorTop(value);
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public virtual Coordinate GetCursorPosition =>
            new(CursorLeft, CursorTop);

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public virtual int WindowWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return ConsoleResizeHandler.GetCurrentConsoleSize().Width;
            }
        }

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public virtual int WindowHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return ConsoleResizeHandler.GetCurrentConsoleSize().Height;
            }
        }

        /// <summary>
        /// The console buffer width (columns)
        /// </summary>
        public virtual int BufferWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.BufferWidth;
            }
        }

        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public virtual int BufferHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.BufferHeight;
            }
        }

        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public virtual bool CursorVisible
        {
            get => ConsoleCursor.CursorVisible;
            set => ConsoleCursor.CursorVisible = value;
        }

        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public virtual bool KeyAvailable
        {
            get
            {
                if (IsDumb)
                    return false;
                return !ConsoleMode.IsRaw && Console.KeyAvailable;
            }
        }

        /// <summary>
        /// Whether to treat Ctrl + C as input or not
        /// </summary>
        public virtual bool TreatCtrlCAsInput
        {
            get
            {
                if (IsDumb)
                    return false;
                return Console.TreatControlCAsInput;
            }
            set
            {
                if (!IsDumb)
                    Console.TreatControlCAsInput = value;
            }
        }

        /// <summary>
        /// Sets the cursor position
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        /// <param name="top">The top to be set (from 0)</param>
        public virtual void SetCursorPosition(int left, int top)
        {
            if (!IsDumb)
            {
                Console.SetCursorPosition(left, top);
                _moved = true;
            }
        }

        /// <summary>
        /// Sets the window dimensions
        /// </summary>
        /// <param name="width">The window width to be set (from 0)</param>
        /// <param name="height">The window height to be set (from 0)</param>
        public virtual void SetWindowDimensions(int width, int height)
        {
            if (!IsDumb)
            {
                if (PlatformHelper.IsOnWindows())
                {
                    Console.WindowWidth = width;
                    Console.WindowHeight = height;
                }
                else
                {
                    TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[8;{height};{width}t");
                    Thread.Sleep(35);
                }
                _moved = true;
            }
        }

        /// <summary>
        /// Sets the buffer dimensions
        /// </summary>
        /// <param name="width">The buffer width to be set (from 0)</param>
        /// <param name="height">The buffer height to be set (from 0)</param>
        public virtual void SetBufferDimensions(int width, int height)
        {
            if (!IsDumb)
            {
                if (PlatformHelper.IsOnWindows())
                {
                    Console.BufferWidth = width;
                    Console.BufferHeight = height;
                }
                else
                {
                    TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[8;{height};{width}t");
                    Thread.Sleep(35);
                }
                _moved = true;
            }
        }

        /// <summary>
        /// Sets the cursor left
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        public virtual void SetCursorLeft(int left)
        {
            if (IsDumb)
                return;
            if (ConsoleMode.IsRaw)
            {
                int top = CursorTop;
                Write(CsiSequences.GenerateCsiCursorPosition(left + 1, top + 1));
            }
            else
                Console.CursorLeft = left;
            _moved = true;
        }

        /// <summary>
        /// Sets the cursor top
        /// </summary>
        /// <param name="top">The top to be set (from 0)</param>
        public virtual void SetCursorTop(int top)
        {
            if (IsDumb)
                return;
            if (ConsoleMode.IsRaw)
            {
                int left = CursorLeft;
                Write(CsiSequences.GenerateCsiCursorPosition(left + 1, top + 1));
            }
            else
                Console.CursorTop = top;
            _moved = true;
        }

        /// <summary>
        /// Sets the window width
        /// </summary>
        /// <param name="width">The window width to be set (from 0)</param>
        public virtual void SetWindowWidth(int width)
        {
            if (!IsDumb)
            {
                if (PlatformHelper.IsOnWindows())
                    Console.WindowWidth = width;
                else
                {
                    TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[8;{Console.WindowHeight};{width}t");
                    Thread.Sleep(35);
                }
                _moved = true;
            }
        }

        /// <summary>
        /// Sets the window height
        /// </summary>
        /// <param name="height">The window height to be set (from 0)</param>
        public virtual void SetWindowHeight(int height)
        {
            if (!IsDumb)
            {
                if (PlatformHelper.IsOnWindows())
                    Console.WindowHeight = height;
                else
                {
                    TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[8;{height};{Console.WindowWidth}t");
                    Thread.Sleep(35);
                }
                _moved = true;
            }
        }

        /// <summary>
        /// Sets the buffer width
        /// </summary>
        /// <param name="width">The buffer width to be set (from 0)</param>
        public virtual void SetBufferWidth(int width)
        {
            if (!IsDumb)
            {
                if (PlatformHelper.IsOnWindows())
                    Console.BufferWidth = width;
                else
                {
                    TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[8;{Console.WindowHeight};{width}t");
                    Thread.Sleep(35);
                }
                _moved = true;
            }
        }

        /// <summary>
        /// Sets the buffer height
        /// </summary>
        /// <param name="height">The buffer height to be set (from 0)</param>
        public virtual void SetBufferHeight(int height)
        {
            if (!IsDumb)
            {
                if (PlatformHelper.IsOnWindows())
                    Console.BufferHeight = height;
                else
                {
                    TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[8;{height};{Console.WindowWidth}t");
                    Thread.Sleep(35);
                }
                _moved = true;
            }
        }

        /// <summary>
        /// Beeps the console
        /// </summary>
        public virtual void Beep() =>
            Console.Beep();

        /// <summary>
        /// Beeps the console
        /// </summary>
        /// <param name="freq">Frequency in hertz</param>
        /// <param name="ms">Duration in milliseconds</param>
        public virtual void BeepCustom(int freq, int ms)
        {
            if (PlatformHelper.IsOnWindows())
                Console.Beep(freq, ms);
            else
            {
                Write($"{VtSequenceBasicChars.EscapeChar}[10;{freq}]{VtSequenceBasicChars.EscapeChar}[11;{ms}]\a");
                Thread.Sleep(ms);
            }
        }

        /// <summary>
        /// Beeps the console (VT Sequence method)
        /// </summary>
        public virtual void BeepSeq() =>
            Write(VtSequenceBasicChars.BellChar);

        /// <summary>
        /// Clears the console screen.
        /// </summary>
        public virtual void Clear() =>
            Write(ConsoleClearing.GetClearWholeScreenSequence());

        /// <summary>
        /// Clears the console screen while loading the background.
        /// </summary>
        public virtual void ClearLoadBack()
        {
            Write(ConsoleColoring.RenderRevertBackground());
            Clear();
        }

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        public virtual ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            ConsoleKeyInfo keyInfo;
            if (!ConsoleMode.IsRaw)
                keyInfo = Console.ReadKey(intercept);
            else
                keyInfo = Input.ReadKey();
            _moved = true;
            return keyInfo;
        }

        /// <summary>
        /// Writes a character to console (stdout)
        /// </summary>
        /// <param name="value">A character</param>
        public virtual void Write(char value)
        {
            lock (TextWriterRaw.WriteLock)
            {
                Console.Write(value);
                _moved = true;
            }
        }

        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        public virtual void Write(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                Console.Write(text);
                _moved = true;
            }
        }

        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public virtual void Write(string text, params object[] args)
        {
            string formatted = text.FormatString(args);
            Write(formatted);
        }

        /// <summary>
        /// Writes new line to console (stdout)
        /// </summary>
        public virtual void WriteLine()
        {
            lock (TextWriterRaw.WriteLock)
            {
                Console.WriteLine();
                _moved = true;
            }
        }

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public virtual void WriteLine(string text)
        {
            Write(text);
            WriteLine();
        }

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public virtual void WriteLine(string text, params object[] args)
        {
            Write(text, args);
            WriteLine();
        }

        /// <summary>
        /// Writes a character to console (stderr)
        /// </summary>
        /// <param name="value">A character</param>
        public virtual void WriteError(char value)
        {
            lock (TextWriterRaw.WriteLock)
            {
                Console.Error.Write(value);
                _moved = true;
            }
        }

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        public virtual void WriteError(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                Console.Error.Write(text);
                _moved = true;
            }
        }

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public virtual void WriteError(string text, params object[] args)
        {
            string formatted = text.FormatString(args);
            WriteError(formatted);
        }

        /// <summary>
        /// Writes new line to console (stderr)
        /// </summary>
        public virtual void WriteErrorLine()
        {
            lock (TextWriterRaw.WriteLock)
            {
                Console.Error.WriteLine();
                _moved = true;
            }
        }

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public virtual void WriteErrorLine(string text)
        {
            WriteError(text);
            WriteErrorLine();
        }

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public virtual void WriteErrorLine(string text, params object[] args)
        {
            WriteError(text, args);
            WriteErrorLine();
        }

        static BaseConsoleWrapper()
        {
            ConsoleMisc.PrepareCodepage();

            // Load the placeholder for an MESH variable, unless it's already defined
            PlaceParse.RegisterCustomPlaceholder("$", MESHVariables.GetVariable);

            // Now, load the color placeholders
            PlaceParse.RegisterCustomPlaceholder("f", (c) => new Color(c).VTSequenceForeground());
            PlaceParse.RegisterCustomPlaceholder("b", (c) => new Color(c).VTSequenceBackground());
            PlaceParse.RegisterCustomPlaceholder("fgreset", (_) => ThemeColorsTools.GetColor(ThemeColorType.NeutralText).VTSequenceForeground());
            PlaceParse.RegisterCustomPlaceholder("bgreset", (_) => ThemeColorsTools.GetColor(ThemeColorType.Background).VTSequenceBackground());

            // Load the platform placeholders
            PlaceParse.RegisterCustomPlaceholder("ridgeneric", (_) => PlatformHelper.GetCurrentGenericRid());
            PlaceParse.RegisterCustomPlaceholder("termemu", (_) => PlatformHelper.GetTerminalEmulator());
            PlaceParse.RegisterCustomPlaceholder("termtype", (_) => PlatformHelper.GetTerminalType());
        }
    }
}
