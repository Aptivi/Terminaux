//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using System.Text;
using Terminaux.Writer.FancyWriters;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Textify.General;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.MiscWriters.Tools;
using Terminaux.Writer.MiscWriters;
using Terminaux.Inputs.Interactive;

namespace Terminaux.Inputs.Styles.Editor
{
    /// <summary>
    /// Interactive hex viewer
    /// </summary>
    public static class HexViewInteractive
    {
        private static string status = "";
        private static bool bail;
        private static int byteIdx = 0;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding("Exit", ConsoleKey.Escape),
            new Keybinding("Keybindings", ConsoleKey.K),
            new Keybinding("Number Info", ConsoleKey.F1),
        ];

        /// <summary>
        /// Opens an interactive hex viewer
        /// </summary>
        /// <param name="bytes">Byte array to showcase</param>
        /// <param name="settings">TUI settings</param>
        public static void OpenInteractive(byte[] bytes, InteractiveTuiSettings? settings = null)
        {
            // Set status
            status = "Ready";
            bail = false;
            settings ??= new();

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

                    // Render the box
                    RenderHexViewBox(ref screen, settings);

                    // Now, render the visual hex with the current selection
                    RenderContentsInHexWithSelection(byteIdx, ref screen, bytes, settings);

                    // Render the status
                    RenderStatus(ref screen, settings);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.ReadKey();
                    HandleKeypress(keypress, ref bytes, settings);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox($"The hex viewer failed: {ex.Message}");
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
                return KeybindingsWriter.RenderKeybindings(bindings,
                    settings.KeyBindingBuiltinColor, settings.KeyBindingBuiltinForegroundColor, settings.KeyBindingBuiltinBackgroundColor,
                    settings.KeyBindingOptionColor, settings.OptionForegroundColor, settings.OptionBackgroundColor,
                    0, ConsoleWrapper.WindowHeight - 1);
            });
            screen.AddBufferedPart("Hex viewer interactive - Keybindings", part);
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
            screen.AddBufferedPart("Hex viewer interactive - Status", part);
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
                builder.Append(
                    $"{ColorTools.RenderSetConsoleColor(settings.PaneSeparatorColor)}" +
                    $"{ColorTools.RenderSetConsoleColor(settings.BackgroundColor, true)}" +
                    $"{BorderColor.RenderBorderPlain(0, SeparatorMinimumHeight, SeparatorConsoleWidthInterior, SeparatorMaximumHeightInterior)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex viewer interactive - Hex view box", part);
        }

        private static void RenderContentsInHexWithSelection(int byteIdx, ref Screen screen, byte[] bytes, InteractiveTuiSettings settings)
        {
            // First, update the status
            StatusNumInfo(bytes);

            // Then, render the contents with the selection indicator
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4;
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
                    $"{TextWriterWhereColor.RenderWhere(rendered, 1, 2)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex viewer interactive - Contents", part);
        }

        private static void HandleKeypress(ConsoleKeyInfo key, ref byte[] bytes, InteractiveTuiSettings settings)
        {
            // Check to see if we have this binding
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveBackward();
                    break;
                case ConsoleKey.RightArrow:
                    MoveForward(bytes);
                    break;
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown(bytes);
                    break;
                case ConsoleKey.PageUp:
                    PreviousPage(bytes);
                    break;
                case ConsoleKey.PageDown:
                    NextPage(bytes);
                    break;
                case ConsoleKey.Home:
                    Beginning();
                    break;
                case ConsoleKey.End:
                    End(bytes);
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    break;
                case ConsoleKey.K:
                    RenderKeybindingsBox(bytes, settings);
                    break;
                case ConsoleKey.F1:
                    NumInfo(bytes, settings);
                    break;
            }
        }

        private static byte[] RenderKeybindingsBox(byte[] bytes, InteractiveTuiSettings settings)
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return bytes;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingsWriter.RenderKeybindingHelpText(bindings);
            InfoBoxColor.WriteInfoBoxColorBack(bindingsHelp, settings.BoxForegroundColor, settings.BoxBackgroundColor);
            return bytes;
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
        }

        private static byte[] NumInfo(byte[] bytes, InteractiveTuiSettings settings)
        {
            // Get the hex number in different formats
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");
            string byteNumOctal = Convert.ToString(byteNum, 8);
            string byteNumNumber = Convert.ToString(byteNum);
            string byteNumBinary = Convert.ToString(byteNum, 2);

            // Print the number information
            string header = "Number information:";
            int maxLength = header.Length > ConsoleWrapper.WindowWidth - 4 ? ConsoleWrapper.WindowWidth - 4 : header.Length;
            InfoBoxColor.WriteInfoBoxColorBack(
                header + CharManager.NewLine +
                new string('=', maxLength) + CharManager.NewLine + CharManager.NewLine +
                $"Hexadecimal:  {byteNumHex}" + CharManager.NewLine +
                $"Octal:        {byteNumOctal}" + CharManager.NewLine +
                $"Number:       {byteNumNumber}" + CharManager.NewLine +
                $"Binary:       {byteNumBinary}"
                , settings.BoxForegroundColor, settings.BoxBackgroundColor);
            return bytes;
        }

        private static void StatusNumInfo(byte[] bytes)
        {
            // Get the hex number in different formats
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");
            string byteNumOctal = Convert.ToString(byteNum, 8);
            string byteNumNumber = Convert.ToString(byteNum);
            string byteNumBinary = Convert.ToString(byteNum, 2);

            // Change the status to the number information
            status =
                $"Hexadecimal: {byteNumHex} | " +
                $"Octal: {byteNumOctal} | " +
                $"Number: {byteNumNumber} | " +
                $"Binary: {byteNumBinary}";
        }

        private static void PreviousPage(byte[] bytes)
        {
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentSelection = byteIdx / 16;
            int currentPage = currentSelection / byteLinesPerPage;
            int startIndex = byteLinesPerPage * currentPage;
            int startByte = startIndex * 16;
            if (startByte > bytes.Length)
                startByte = bytes.Length;
            byteIdx = startByte - 1 < 0 ? 0 : startByte - 1;
        }

        private static void NextPage(byte[] bytes)
        {
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentSelection = byteIdx / 16;
            int currentPage = currentSelection / byteLinesPerPage;
            int endIndex = byteLinesPerPage * (currentPage + 1);
            int startByte = endIndex * 16;
            if (startByte > bytes.Length - 1)
                startByte = bytes.Length - 1;
            byteIdx = startByte;
        }

        private static void Beginning() =>
            byteIdx = 0;

        private static void End(byte[] bytes) =>
            byteIdx = bytes.Length - 1;

        private static string RenderContentsInHex(long ByteHighlight, long StartByte, long EndByte, byte[] FileByte, InteractiveTuiSettings settings)
        {
            // Get the un-highlighted and highlighted colors
            var entryColor = settings.PaneItemForeColor;
            var unhighlightedColorBackground = settings.BackgroundColor;
            var highlightedColorBackground = settings.PaneSelectedItemBackColor;

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
    }
}
