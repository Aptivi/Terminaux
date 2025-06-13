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
using System.Globalization;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Inputs.Styles.Editor
{
    /// <summary>
    /// Interactive hex viewer
    /// </summary>
    [Obsolete("To avoid duplicate code, we've deprecated this class. Please use HexEditInteractive with \"edit\" set to false.")]
    public static class HexViewInteractive
    {
        private static string status = "";
        private static byte[] cachedFind = [];
        private static bool bail;
        private static int byteIdx = 0;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_EXIT"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_KEYBINDINGS"), ConsoleKey.K),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_KEYBINDING_NUMBERINFO"), ConsoleKey.F1),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_FINDNEXT"), ConsoleKey.Divide),
        ];

        /// <summary>
        /// Opens an interactive hex viewer
        /// </summary>
        /// <param name="bytes">Byte array to showcase</param>
        /// <param name="settings">TUI settings</param>
        /// <param name="fullscreen">Whether it's a fullscreen viewer or not</param>
        public static void OpenInteractive(byte[] bytes, InteractiveTuiSettings? settings = null, bool fullscreen = false)
        {
            // Set status
            status = LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_STATUS_READY");
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
                InfoBoxModalColor.WriteInfoBoxModal($"The hex viewer failed: {ex.Message}");
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
            screen.AddBufferedPart("Hex viewer interactive - Hex view box", part);
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
            screen.AddBufferedPart("Hex viewer interactive - Contents", part);
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
                    RenderKeybindingsBox(bytes, settings);
                    screen.RequireRefresh();
                    break;
                case ConsoleKey.F1:
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

        private static byte[] RenderKeybindingsBox(byte[] bytes, InteractiveTuiSettings settings)
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return bytes;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingTools.RenderKeybindingHelpText(bindings);
            InfoBoxModalColor.WriteInfoBoxModal(bindingsHelp, new InfoBoxSettings(settings.InfoBoxSettings)
            {
                Title = LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_KEYBINDING_AVAILABLE_KEYBINDINGS"),
            });
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
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMSREQUIRED"), settings.InfoBoxSettings);
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
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_NOTFOUND"), settings.InfoBoxSettings);
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
            InfoBoxModalColor.WriteInfoBoxModal(
                $"Position:     0x{byteIdx:X8}" + CharManager.NewLine +
                $"Hexadecimal:  0x{byteNumHex}" + CharManager.NewLine +
                $"Octal:        {byteNumOctal}" + CharManager.NewLine +
                $"Number:       {byteNumNumber}" + CharManager.NewLine +
                $"Binary:       {byteNumBinary}", new InfoBoxSettings(settings.InfoBoxSettings)
                {
                    Title = LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_TITLE"),
                });
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
            if (startByte > bytes.Length)
                startByte = bytes.Length;
            byteIdx = startByte - 1 < 0 ? 0 : startByte - 1;
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
            byteIdx = startByte;
            screen.RequireRefresh();
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
                throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTENUMSMALLER"));

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
                throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_STARTBYTELARGER1"));
            else if (EndByte > FileByte.LongLength)
                throw new ArgumentOutOfRangeException(nameof(EndByte), EndByte, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_ENDBYTELARGER1"));
            else
                throw new ArgumentOutOfRangeException($"The specified byte number is invalid. {StartByte}, {EndByte}");
        }
    }
}
