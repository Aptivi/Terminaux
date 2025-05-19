﻿//
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
using System.Globalization;
using System.Linq;
using System.Text;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Textify.General;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Interactive;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Inputs.Styles.Editor
{
    /// <summary>
    /// Interactive hex editor
    /// </summary>
    public static class HexEditInteractive
    {
        private static string status = "";
        private static byte[] cachedFind = [];
        private static bool bail;
        private static int byteIdx = 0;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding("Exit", ConsoleKey.Escape),
            new Keybinding("Keybindings", ConsoleKey.K),
            new Keybinding("Insert", ConsoleKey.F1),
            new Keybinding("Remove", ConsoleKey.F2),
            new Keybinding("Replace", ConsoleKey.F3),
            new Keybinding("Replace All", ConsoleKey.F3, ConsoleModifiers.Shift),
            new Keybinding("Replace All What", ConsoleKey.F3, ConsoleModifiers.Shift | ConsoleModifiers.Alt),
            new Keybinding("Find Next", ConsoleKey.Divide),
            new Keybinding("Number Info", ConsoleKey.F4),
        ];

        /// <summary>
        /// Opens an interactive hex editor
        /// </summary>
        /// <param name="bytes">Byte array to showcase</param>
        /// <param name="settings">TUI settings</param>
        /// <param name="fullscreen">Whether it's a fullscreen viewer or not</param>
        public static void OpenInteractive(ref byte[] bytes, InteractiveTuiSettings? settings = null, bool fullscreen = false)
        {
            // Set status
            status = "Ready";
            bail = false;
            settings ??= InteractiveTuiSettings.GlobalSettings;

            // Main loop
            byteIdx = 0;
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            ConsoleWrapper.CursorVisible = false;
            try
            {
                while (!bail)
                {
                    // Now, render the keybindings
                    RenderKeybindings(ref screen, settings);

                    // Check to see if we need to render the box and the status
                    if (!fullscreen)
                    {
                        // Render the box
                        RenderHexViewBox(ref screen, settings);

                        // Render the status
                        RenderStatus(ref screen, settings);
                    }

                    // Now, render the visual hex with the current selection
                    RenderContentsInHexWithSelection(byteIdx, ref screen, bytes, settings, fullscreen);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.ReadKey();
                    HandleKeypress(keypress, ref bytes, screen, fullscreen, settings);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal($"The hex editor failed: {ex.Message}");
            }
            bail = false;
            ScreenTools.UnsetCurrent(screen);

            // Clean up
            ColorTools.LoadBack();
        }

        private static void RenderKeybindings(ref Screen screen, InteractiveTuiSettings settings)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var keybindingsRenderable = new Keybindings()
                {
                    KeybindingList = bindings,
                    BuiltinColor = settings.KeyBindingBuiltinColor,
                    BuiltinForegroundColor = settings.KeyBindingBuiltinForegroundColor,
                    BuiltinBackgroundColor = settings.KeyBindingBuiltinBackgroundColor,
                    OptionColor = settings.KeyBindingOptionColor,
                    OptionForegroundColor = settings.OptionForegroundColor,
                    OptionBackgroundColor = settings.OptionBackgroundColor,
                    BackgroundColor = settings.BackgroundColor,
                    Width = ConsoleWrapper.WindowWidth - 1,
                };
                return RendererTools.RenderRenderable(keybindingsRenderable, new(0, ConsoleWrapper.WindowHeight - 1));
            });
            screen.AddBufferedPart("Hex editor interactive - Keybindings", part);
        }

        private static void RenderStatus(ref Screen screen, InteractiveTuiSettings settings)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(
                    $"{ColorTools.RenderSetConsoleColor(settings.ForegroundColor)}" +
                    $"{ColorTools.RenderSetConsoleColor(settings.BackgroundColor, true)}" +
                    $"{TextWriterWhereColor.RenderWhere(status + ConsoleClearing.GetClearLineToRightSequence(), 0, 0)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex editor interactive - Status", part);
        }

        private static void RenderHexViewBox(ref Screen screen, InteractiveTuiSettings settings)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();

                // Get the widths and heights
                int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

                // Render the box
                var border = new Border()
                {
                    Left = 0,
                    Top = SeparatorMinimumHeight,
                    Width = SeparatorConsoleWidthInterior,
                    Height = SeparatorMaximumHeightInterior,
                };
                builder.Append(
                    ColorTools.RenderSetConsoleColor(settings.PaneSeparatorColor) +
                    ColorTools.RenderSetConsoleColor(settings.BackgroundColor, true) +
                    border.Render()
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex editor interactive - Hex view box", part);
        }

        private static void RenderContentsInHexWithSelection(int byteIdx, ref Screen screen, byte[] bytes, InteractiveTuiSettings settings, bool fullscreen)
        {
            // First, update the status
            StatusNumInfo(bytes);

            // Then, render the contents with the selection indicator
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
                var builder = new StringBuilder();
                int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
                int currentSelection = byteIdx / 16;
                int currentPage = currentSelection / byteLinesPerPage;
                int startIndex = byteLinesPerPage * currentPage;
                int endIndex = byteLinesPerPage * (currentPage + 1);
                int startByte = startIndex * 16 + 1;
                int endByte = endIndex * 16;
                if (startByte > bytes.Length)
                    startByte = bytes.Length;
                if (endByte > bytes.Length)
                    endByte = bytes.Length;
                string rendered = RenderContentsInHex(byteIdx + 1, startByte, endByte, bytes, settings);

                // Render the box
                builder.Append(
                    $"{ColorTools.RenderSetConsoleColor(settings.ForegroundColor)}" +
                    $"{ColorTools.RenderSetConsoleColor(settings.BackgroundColor, true)}" +
                    $"{TextWriterWhereColor.RenderWhere(rendered, fullscreen ? 0 : 1, SeparatorMinimumHeightInterior)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex editor interactive - Contents", part);
        }

        private static void HandleKeypress(ConsoleKeyInfo key, ref byte[] bytes, Screen screen, bool fullscreen, InteractiveTuiSettings settings)
        {
            // Check to see if we have this binding
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveBackward();
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.RightArrow:
                    MoveForward(bytes);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.UpArrow:
                    MoveUp();
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown(bytes);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.PageUp:
                    PreviousPage(bytes, screen, fullscreen);
                    break;
                case ConsoleKey.PageDown:
                    NextPage(bytes, screen, fullscreen);
                    break;
                case ConsoleKey.Home:
                    Beginning();
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.End:
                    End(bytes);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    break;
                case ConsoleKey.K:
                    RenderKeybindingsBox(settings);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.F1:
                    Insert(ref bytes, settings);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.F2:
                    Remove(ref bytes);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.F3:
                    if (key.Modifiers == ConsoleModifiers.Shift)
                        ReplaceAll(ref bytes, settings);
                    else if (key.Modifiers == (ConsoleModifiers.Shift | ConsoleModifiers.Alt))
                        ReplaceAllWhat(ref bytes, settings);
                    else
                        Replace(ref bytes, settings);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.F4:
                    NumInfo(bytes, settings);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.Oem2:
                case ConsoleKey.Divide:
                    FindNext(ref bytes, settings);
                    screen.RequireRefresh();
                    break;
            }
        }

        private static void RenderKeybindingsBox(InteractiveTuiSettings settings)
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingTools.RenderKeybindingHelpText(bindings);
            InfoBoxModalColor.WriteInfoBoxModal(bindingsHelp, new InfoBoxSettings(settings.InfoBoxSettings)
            {
                Title = "Available keybindings",
            });
        }

        private static void MoveBackward()
        {
            byteIdx--;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void MoveForward(byte[] bytes)
        {
            byteIdx++;
            if (byteIdx > bytes.Length - 1)
                byteIdx = bytes.Length - 1;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void MoveUp()
        {
            byteIdx -= 16;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void MoveDown(byte[] bytes)
        {
            byteIdx += 16;
            if (byteIdx > bytes.Length - 1)
                byteIdx = bytes.Length - 1;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void Insert(ref byte[] bytes, InteractiveTuiSettings settings)
        {
            // Prompt and parse the number
            byte byteNum = default;
            string byteNumHex = InfoBoxInputColor.WriteInfoBoxInput("Write the byte number with the hexadecimal value. 00 -> FF.", settings.InfoBoxSettings);
            if (byteNumHex.Length != 2 ||
                byteNumHex.Length == 2 && !byte.TryParse(byteNumHex, NumberStyles.AllowHexSpecifier, null, out byteNum))
                InfoBoxModalColor.WriteInfoBoxModal("The byte number specified is not valid.", settings.InfoBoxSettings);
            else
                AddNewByte(ref bytes, byteNum, byteIdx + 1);
        }

        private static void Remove(ref byte[] bytes)
        {
            DeleteByte(ref bytes, byteIdx + 1);
            if (byteIdx + 1 > bytes.Length && bytes.Length > 0)
                MoveBackward();
        }

        private static void Replace(ref byte[] bytes, InteractiveTuiSettings settings)
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Get the current byte number and its hex
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInput("Write the byte number with the hexadecimal value to replace {0} with. 00 -> FF.", settings.InfoBoxSettings, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxModalColor.WriteInfoBoxModal("The byte number specified is not valid.", settings.InfoBoxSettings);

            // Do the replacement!
            Replace(ref bytes, byteNum, byteNumReplaced, byteIdx + 1, byteIdx + 1);
        }

        private static void ReplaceAll(ref byte[] bytes, InteractiveTuiSettings settings)
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Get the current byte number and its hex
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInput("Write the byte number with the hexadecimal value to replace {0} with. 00 -> FF.", settings.InfoBoxSettings, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxModalColor.WriteInfoBoxModal("The byte number specified is not valid.", settings.InfoBoxSettings);

            // Do the replacement!
            Replace(ref bytes, byteNum, byteNumReplaced);
        }

        private static void ReplaceAllWhat(ref byte[] bytes, InteractiveTuiSettings settings)
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Prompt and parse the number
            byte byteNum = default;
            string byteNumHex = InfoBoxInputColor.WriteInfoBoxInput("Write the byte number with the hexadecimal value to be replaced. 00 -> FF.", settings.InfoBoxSettings);
            if (byteNumHex.Length != 2 ||
                byteNumHex.Length == 2 && !byte.TryParse(byteNumHex, NumberStyles.AllowHexSpecifier, null, out byteNum))
                InfoBoxModalColor.WriteInfoBoxModal("The byte number specified is not valid.", settings.InfoBoxSettings);

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInput("Write the byte number with the hexadecimal value to replace {0} with. 00 -> FF.", settings.InfoBoxSettings, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxModalColor.WriteInfoBoxModal("The byte number specified is not valid.", settings.InfoBoxSettings);

            // Do the replacement!
            Replace(ref bytes, byteNum, byteNumReplaced);
        }

        private static void FindNext(ref byte[] bytes, InteractiveTuiSettings settings)
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Now, prompt for the replacement line
            string bytesSpec = InfoBoxInputColor.WriteInfoBoxInput("Write a byte or a group of bytes separated by whitespaces. It can be from 00 to FF.", settings.InfoBoxSettings);
            byte[] refBytes;

            // See if we have a cached find if the user didn't provide any string to find
            if (string.IsNullOrEmpty(bytesSpec))
            {
                if (cachedFind.Length == 0)
                {
                    InfoBoxModalColor.WriteInfoBoxModal("Bytes are required to find, but you haven't provided one.", settings.InfoBoxSettings);
                    return;
                }
                else
                    refBytes = cachedFind;
            }
            else
            {
                // Validate each byte
                string[] splitBytes = bytesSpec.Split(' ');
                List<byte> finalBytes = [];
                foreach (string byteSplit in splitBytes)
                {
                    // Check this individual byte
                    if (!byte.TryParse(byteSplit, NumberStyles.AllowHexSpecifier, null, out byte finalByte))
                    {
                        InfoBoxModalColor.WriteInfoBoxModal($"Invalid byte {byteSplit}.", settings.InfoBoxSettings);
                        return;
                    }

                    // Add this byte
                    finalBytes.Add(finalByte);
                }
                refBytes = [.. finalBytes];
            }

            // Determine row and column
            int byteIndex = byteIdx + 1;
            if (byteIndex >= bytes.Length)
                byteIndex = 0;

            // Now, run a loop to find, continuing from top where necessary
            bool found = false;
            bool firstTime = true;
            int b = byteIndex;
            while (!found)
            {
                byte[] processed = new byte[refBytes.Length];

                // Get a byte and add it to the processed byte array
                for (int i = 0; i < refBytes.Length; i++)
                {
                    byte candidate = bytes[b + i];
                    processed[i] = candidate;
                }

                // Compare between the processed byte and the actual byte
                int foundBytes = 0;
                for (int i = 0; i < processed.Length; i++)
                {
                    byte candidate = processed[i];
                    byte actual = refBytes[i];
                    if (candidate == actual)
                        foundBytes++;
                }

                // Determine whether to exit
                if (foundBytes == refBytes.Length)
                    found = true;
                if (!firstTime && b == byteIndex)
                    break;
                if (!found)
                {
                    firstTime = false;
                    b++;
                    if (b >= bytes.Length)
                        b = 0;
                }
            }

            // Update line index and column index if found. Otherwise, show a message
            if (found)
            {
                byteIdx = b;
                cachedFind = refBytes;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal("Not found. Check your syntax or broaden your search.", settings.InfoBoxSettings);
        }

        private static void NumInfo(byte[] bytes, InteractiveTuiSettings settings)
        {
            // Check the index
            if (byteIdx >= bytes.Length)
                return;

            // Get the hex number in different formats
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");
            string byteNumOctal = Convert.ToString(byteNum, 8);
            string byteNumNumber = Convert.ToString(byteNum);
            string byteNumBinary = Convert.ToString(byteNum, 2);

            // Print the number information
            InfoBoxModalColor.WriteInfoBoxModal(
                $"Position:     0x{byteIdx:X8}" + CharManager.NewLine +
                $"Hexadecimal:  0x{byteNumHex}" + CharManager.NewLine +
                $"Octal:        {byteNumOctal}" + CharManager.NewLine +
                $"Number:       {byteNumNumber}" + CharManager.NewLine +
                $"Binary:       {byteNumBinary}", new InfoBoxSettings(settings.InfoBoxSettings)
                {
                    Title = "Number information",
                });
        }

        private static void StatusNumInfo(byte[] bytes)
        {
            // Check the index
            if (byteIdx >= bytes.Length)
                return;

            // Get the hex number in different formats
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");
            string byteNumOctal = Convert.ToString(byteNum, 8);
            string byteNumNumber = Convert.ToString(byteNum);
            string byteNumBinary = Convert.ToString(byteNum, 2);

            // Change the status to the number information
            status =
                $"Position: 0x{byteIdx:X8} | " +
                $"Hexadecimal: 0x{byteNumHex} | " +
                $"Octal: {byteNumOctal} | " +
                $"Number: {byteNumNumber} | " +
                $"Binary: {byteNumBinary}";
        }

        private static void PreviousPage(byte[] bytes, Screen screen, bool fullscreen)
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int currentSelection = byteIdx / 16;
            int currentPage = currentSelection / byteLinesPerPage;
            int startIndex = byteLinesPerPage * currentPage;
            int startByte = startIndex * 16;
            if (startByte > bytes.Length - 1)
                startByte = bytes.Length - 1;
            if (startByte < 0)
                startByte = 0;
            byteIdx = startByte;
            screen.RequireRefresh();
        }

        private static void NextPage(byte[] bytes, Screen screen, bool fullscreen)
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int currentSelection = byteIdx / 16;
            int currentPage = currentSelection / byteLinesPerPage;
            int endIndex = byteLinesPerPage * (currentPage + 1);
            int startByte = endIndex * 16;
            if (startByte > bytes.Length - 1)
                startByte = bytes.Length - 1;
            if (startByte < 0)
                startByte = 0;
            byteIdx = startByte;
            screen.RequireRefresh();
        }

        private static void Beginning() =>
            byteIdx = 0;

        private static void End(byte[] bytes)
        {
            byteIdx = bytes.Length - 1;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static string RenderContentsInHex(long ByteHighlight, long StartByte, long EndByte, byte[] FileByte, InteractiveTuiSettings settings)
        {
            // Get the un-highlighted and highlighted colors
            var entryColor = settings.PaneItemForeColor;
            var unhighlightedColorBackground = settings.BackgroundColor;
            var highlightedColorBackground = settings.PaneSelectedItemBackColor;

            // Check the index
            if (byteIdx >= FileByte.Length)
                return "Empty content";

            // Now, do the job!
            StartByte.SwapIfSourceLarger(ref EndByte);
            if (StartByte < 1)
                throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, "Byte number must start with 1.");

            if (StartByte <= FileByte.LongLength && EndByte <= FileByte.LongLength)
            {
                // We need to know how to write the bytes and their contents in this shape:
                // -> 0x00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                // ... and so on.
                var builder = new StringBuilder();
                for (long CurrentByteNumber = StartByte; CurrentByteNumber <= EndByte; CurrentByteNumber += 16)
                {
                    builder.Append($"{ColorTools.RenderSetConsoleColor(unhighlightedColorBackground, true)}{entryColor.VTSequenceForeground}0x{CurrentByteNumber - 1L:X8} ");

                    // Iterate these number of bytes for the ASCII codes
                    long byteNum;
                    for (byteNum = 0; byteNum < 16 && CurrentByteNumber + byteNum <= EndByte; byteNum++)
                    {
                        byte CurrentByte = FileByte[(int)(CurrentByteNumber + byteNum - 1)];
                        builder.Append(
                            $"{(CurrentByteNumber + byteNum == ByteHighlight ? unhighlightedColorBackground : highlightedColorBackground).VTSequenceForeground}" +
                            $"{ColorTools.RenderSetConsoleColor((CurrentByteNumber + byteNum == ByteHighlight ? highlightedColorBackground : unhighlightedColorBackground), true)}" +
                            $"{CurrentByte:X2}" +
                            $"{highlightedColorBackground.VTSequenceForeground}" +
                            $"{ColorTools.RenderSetConsoleColor(unhighlightedColorBackground, true)}" +
                            $" "
                        );
                    }

                    // Pad the remaining ASCII byte display
                    int remaining = (int)(16 - byteNum);
                    int padTimes = remaining * 3;
                    string padded = new(' ', padTimes);
                    builder.Append(padded);

                    // Iterate these number of bytes for the actual rendered characters
                    for (byteNum = 0; byteNum < 16 && CurrentByteNumber + byteNum <= EndByte; byteNum++)
                    {
                        byte CurrentByte = FileByte[(int)(CurrentByteNumber + byteNum - 1)];
                        char ProjectedByteChar = Convert.ToChar(CurrentByte);
                        char RenderedByteChar = '.';
                        if (!char.IsWhiteSpace(ProjectedByteChar) & !char.IsControl(ProjectedByteChar) & !char.IsHighSurrogate(ProjectedByteChar) & !char.IsLowSurrogate(ProjectedByteChar))
                        {
                            // The renderer will actually render the character, not as a dot.
                            RenderedByteChar = ProjectedByteChar;
                        }
                        builder.Append(
                            $"{(CurrentByteNumber + byteNum == ByteHighlight ? unhighlightedColorBackground : highlightedColorBackground).VTSequenceForeground}" +
                            $"{ColorTools.RenderSetConsoleColor((CurrentByteNumber + byteNum == ByteHighlight ? highlightedColorBackground : unhighlightedColorBackground), true)}" +
                            $"{RenderedByteChar}"
                        );
                    }
                    builder.AppendLine();
                }
                return builder.ToString();
            }
            else if (StartByte > FileByte.LongLength)
                throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, "The specified start byte number may not be larger than the file size.");
            else if (EndByte > FileByte.LongLength)
                throw new ArgumentOutOfRangeException(nameof(EndByte), EndByte, "The specified end byte number may not be larger than the file size.");
            else
                throw new ArgumentOutOfRangeException($"The specified byte number is invalid. {StartByte}, {EndByte}");
        }

        private static void AddNewByte(ref byte[] bytes, byte Content, long pos)
        {
            if (bytes is not null)
            {
                // If empty, ignore the position and just use the content given
                var FileBytesList = bytes.ToList();
                if (bytes.Length == 0)
                {
                    FileBytesList.Add(Content);
                    bytes = [.. FileBytesList];
                    return;
                }

                // Check the position
                if (pos < 1 || pos > bytes.Length)
                    throw new ArgumentOutOfRangeException(nameof(pos), pos, $"The specified byte number may not be larger than {bytes.LongLength} or smaller than 1.");

                // Actually remove a byte
                long ByteIndex = pos - 1L;
                if (pos <= bytes.LongLength)
                {
                    FileBytesList.Insert((int)ByteIndex, Content);
                    bytes = [.. FileBytesList];
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(pos), pos, $"The specified byte number may not be larger than {bytes.LongLength}.");
            }
            else
                throw new ArgumentNullException("Can't perform this operation on a null array.");
        }

        private static void DeleteByte(ref byte[] bytes, long ByteNumber)
        {
            if (bytes is not null)
            {
                if (ByteNumber < 1)
                    throw new ArgumentOutOfRangeException(nameof(ByteNumber), ByteNumber, "Byte number must start with 1.");
                if (bytes.Length == 0)
                    return;
                var FileBytesList = bytes.ToList();
                long ByteIndex = ByteNumber - 1L;

                // Actually remove a byte
                if (ByteNumber <= bytes.LongLength)
                {
                    FileBytesList.RemoveAt((int)ByteIndex);
                    bytes = [.. FileBytesList];
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(ByteNumber), ByteNumber, $"The specified byte number may not be larger than {bytes.LongLength}.");
            }
            else
                throw new ArgumentNullException("Can't perform this operation on a null array.");
        }

        private static void Replace(ref byte[] bytes, byte FromByte, byte WithByte) =>
            Replace(ref bytes, FromByte, WithByte, 1L, bytes.LongLength);

        private static void Replace(ref byte[] bytes, byte FromByte, byte WithByte, long StartByte, long EndByte)
        {
            if (bytes is not null)
            {
                if (StartByte < 1)
                    throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, "Byte number must start with 1.");
                if (bytes.Length == 0)
                    return;
                if (StartByte <= bytes.LongLength & EndByte <= bytes.LongLength)
                {
                    for (long ByteNumber = StartByte; ByteNumber <= EndByte; ByteNumber++)
                    {
                        if (bytes[(int)(ByteNumber - 1L)] == FromByte)
                            bytes[(int)(ByteNumber - 1L)] = WithByte;
                    }
                }
                else if (StartByte > bytes.LongLength)
                    throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, $"The specified start byte number may not be larger than {bytes.LongLength}.");
                else if (EndByte > bytes.LongLength)
                    throw new ArgumentOutOfRangeException(nameof(EndByte), EndByte, $"The specified end byte number may not be larger than {bytes.LongLength}.");
            }
            else
                throw new ArgumentNullException("Can't perform this operation on a null array.");
        }
    }
}
