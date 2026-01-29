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

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
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
    public class HexEditInteractive : TextualUI
    {
        private static Keybinding[] Bindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_EXIT"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_KEYBINDING_NUMBERINFO"), ConsoleKey.F4),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_FINDNEXT"), ConsoleKey.Divide),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOUP1"), ConsoleKey.UpArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GODOWN1"), ConsoleKey.DownArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_MOVELEFT"), ConsoleKey.LeftArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_MOVERIGHT"), ConsoleKey.RightArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOPREVPAGE1"), ConsoleKey.PageUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GONEXTPAGE1"), ConsoleKey.PageDown),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOFIRST1"), ConsoleKey.Home),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOLAST1"), ConsoleKey.End),
        ];

        private static Keybinding[] EditorBindings =>
        [
            .. Bindings,
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_INSERT"), ConsoleKey.F1),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_KEYBINDING_REMOVE"), ConsoleKey.F2),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_REPLACE"), ConsoleKey.F3),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_REPLACEALL"), ConsoleKey.F3, ConsoleModifiers.Shift),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_KEYBINDING_REPLACEALLWHAT"), ConsoleKey.F3, ConsoleModifiers.Shift | ConsoleModifiers.Alt),
        ];

        private string status = "";
        private byte[] bytes = [];
        private InteractiveTuiSettings settings = InteractiveTuiSettings.GlobalSettings;
        private byte[] cachedFind = [];
        private int byteIdx = 0;
        private bool fullscreen;
        private bool editable = true;

        /// <summary>
        /// Opens an interactive hex editor
        /// </summary>
        /// <param name="bytes">Byte array to showcase</param>
        /// <param name="settings">TUI settings</param>
        /// <param name="fullscreen">Whether it's a fullscreen viewer or not</param>
        /// <param name="edit">Whether it's editable or not</param>
        public static void OpenInteractive(ref byte[] bytes, InteractiveTuiSettings? settings = null, bool fullscreen = false, bool edit = true)
        {
            // Make a new TUI
            var finalSettings = settings ?? InteractiveTuiSettings.GlobalSettings;
            var hexEditor = new HexEditInteractive()
            {
                status = LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_STATUS_READY"),
                editable = edit,
                bytes = bytes,
                settings = finalSettings,
                fullscreen = fullscreen,
            };

            // Assign keybindings
            hexEditor.Keybindings.Add((Bindings[0], (ui, _, _) => TextualUITools.ExitTui(ui)));
            hexEditor.Keybindings.Add((Bindings[1], (ui, _, _) => ((HexEditInteractive)ui).NumInfo()));
            hexEditor.Keybindings.Add((Bindings[2], (ui, _, _) => ((HexEditInteractive)ui).FindNext()));
            hexEditor.Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_FINDNEXT"), ConsoleKey.Oem2), (ui, _, _) => ((HexEditInteractive)ui).FindNext()));
            hexEditor.Keybindings.Add((Bindings[3], (ui, _, _) => ((HexEditInteractive)ui).MoveUp()));
            hexEditor.Keybindings.Add((Bindings[4], (ui, _, _) => ((HexEditInteractive)ui).MoveDown()));
            hexEditor.Keybindings.Add((Bindings[5], (ui, _, _) => ((HexEditInteractive)ui).MoveBackward()));
            hexEditor.Keybindings.Add((Bindings[6], (ui, _, _) => ((HexEditInteractive)ui).MoveForward()));
            hexEditor.Keybindings.Add((Bindings[7], (ui, _, _) => ((HexEditInteractive)ui).PreviousPage()));
            hexEditor.Keybindings.Add((Bindings[8], (ui, _, _) => ((HexEditInteractive)ui).NextPage()));
            hexEditor.Keybindings.Add((Bindings[9], (ui, _, _) => ((HexEditInteractive)ui).Beginning()));
            hexEditor.Keybindings.Add((Bindings[10], (ui, _, _) => ((HexEditInteractive)ui).End()));

            // Assign edit keybindings
            if (edit)
            {
                hexEditor.Keybindings.Add((EditorBindings[11], (ui, _, _) => ((HexEditInteractive)ui).Insert()));
                hexEditor.Keybindings.Add((EditorBindings[12], (ui, _, _) => ((HexEditInteractive)ui).Remove()));
                hexEditor.Keybindings.Add((EditorBindings[13], (ui, _, _) => ((HexEditInteractive)ui).Replace()));
                hexEditor.Keybindings.Add((EditorBindings[14], (ui, _, _) => ((HexEditInteractive)ui).ReplaceAll()));
                hexEditor.Keybindings.Add((EditorBindings[15], (ui, _, _) => ((HexEditInteractive)ui).ReplaceAllWhat()));
            }

            // Run the TUI
            TextualUITools.RunTui(hexEditor);
            bytes = hexEditor.bytes;
        }

        /// <inheritdoc/>
        public override string Render()
        {
            var builder = new StringBuilder();

            // Now, render the keybindings
            builder.Append(RenderKeybindings());

            // Check to see if we need to render the box and the status
            if (!fullscreen)
            {
                // Render the box
                builder.Append(RenderHexViewBox());

                // Render the status
                builder.Append(RenderStatus());
            }

            // Now, render the visual hex with the current selection
            builder.Append(RenderContentsInHexWithSelection());
            return builder.ToString();
        }

        private string RenderKeybindings()
        {
            var keybindingsRenderable = new Keybindings()
            {
                KeybindingList = editable ? EditorBindings : Bindings,
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
        }

        private string RenderStatus()
        {
            // First, update the status
            StatusNumInfo();

            // Now, render the status
            var builder = new StringBuilder();
            builder.Append(
                $"{ConsoleColoring.RenderSetConsoleColor(settings.ForegroundColor)}" +
                $"{ConsoleColoring.RenderSetConsoleColor(settings.BackgroundColor, true)}" +
                $"{TextWriterWhereColor.RenderWherePlain(status + ConsoleClearing.GetClearLineToRightSequence(), 0, 0)}"
            );
            return builder.ToString();
        }

        private string RenderHexViewBox()
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
                ConsoleColoring.RenderSetConsoleColor(settings.PaneSeparatorColor) +
                ConsoleColoring.RenderSetConsoleColor(settings.BackgroundColor, true) +
                border.Render()
            );
            return builder.ToString();
        }

        private string RenderContentsInHexWithSelection()
        {
            // Render the contents with the selection indicator
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
                $"{ConsoleColoring.RenderSetConsoleColor(settings.ForegroundColor)}" +
                $"{ConsoleColoring.RenderSetConsoleColor(settings.BackgroundColor, true)}" +
                $"{TextWriterWhereColor.RenderWherePlain(rendered, fullscreen ? 0 : 1, SeparatorMinimumHeightInterior)}"
            );
            return builder.ToString();
        }

        private void MoveBackward()
        {
            byteIdx--;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private void MoveForward()
        {
            byteIdx++;
            if (byteIdx > bytes.Length - 1)
                byteIdx = bytes.Length - 1;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private void MoveUp()
        {
            byteIdx -= 16;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private void MoveDown()
        {
            byteIdx += 16;
            if (byteIdx > bytes.Length - 1)
                byteIdx = bytes.Length - 1;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private void Insert()
        {
            // Prompt and parse the number
            byte byteNum = default;
            string byteNumHex = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMPROMPT") + " 00 -> FF.", settings.InfoBoxSettings);
            if (byteNumHex.Length != 2 ||
                byteNumHex.Length == 2 && !byte.TryParse(byteNumHex, NumberStyles.AllowHexSpecifier, null, out byteNum))
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMINVALID1"), settings.InfoBoxSettings);
            else
                AddNewByte(byteNum, byteIdx + 1);
        }

        private void Remove()
        {
            DeleteByte(byteIdx + 1);
            if (byteIdx + 1 > bytes.Length && bytes.Length > 0)
                MoveBackward();
        }

        private void Replace()
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Get the current byte number and its hex
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMPROMPT_REPLACE") + " 00 -> FF.", settings.InfoBoxSettings, InfoBoxInputType.Text, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMINVALID1"), settings.InfoBoxSettings);

            // Do the replacement!
            Replace(byteNum, byteNumReplaced, byteIdx + 1, byteIdx + 1);
        }

        private void ReplaceAll()
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Get the current byte number and its hex
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMPROMPT_REPLACE") + " 00 -> FF.", settings.InfoBoxSettings, InfoBoxInputType.Text, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMINVALID1"), settings.InfoBoxSettings);

            // Do the replacement!
            Replace(byteNum, byteNumReplaced);
        }

        private void ReplaceAllWhat()
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Prompt and parse the number
            byte byteNum = default;
            string byteNumHex = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMPROMPT_REPLACED") + " 00 -> FF.", settings.InfoBoxSettings);
            if (byteNumHex.Length != 2 ||
                byteNumHex.Length == 2 && !byte.TryParse(byteNumHex, NumberStyles.AllowHexSpecifier, null, out byteNum))
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMINVALID1"), settings.InfoBoxSettings);

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMPROMPT_REPLACE") + " 00 -> FF.", settings.InfoBoxSettings, InfoBoxInputType.Text, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMINVALID1"), settings.InfoBoxSettings);

            // Do the replacement!
            Replace(byteNum, byteNumReplaced);
        }

        private void FindNext()
        {
            // Check the bytes
            if (bytes.Length == 0)
                return;

            // Now, prompt for the replacement line
            string bytesSpec = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMSPROMPT") + " 00 -> FF.", settings.InfoBoxSettings);
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
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_BYTENUMINVALID2"), settings.InfoBoxSettings, byteSplit);
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

        private void NumInfo()
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
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_POSITION") + $": 0x{byteIdx:X8}" + CharManager.NewLine +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_HEXADECIMAL") + $": 0x{byteNumHex}" + CharManager.NewLine +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_OCTAL") + $": {byteNumOctal}" + CharManager.NewLine +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_NUMBER") + $": {byteNumNumber}" + CharManager.NewLine +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_BINARY") + $": {byteNumBinary}", new InfoBoxSettings(settings.InfoBoxSettings)
                {
                    Title = LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_TITLE"),
                });
        }

        private void StatusNumInfo()
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
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_POSITION") + $": 0x{byteIdx:X8} | " +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_HEXADECIMAL") + $": 0x{byteNumHex} | " +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_OCTAL") + $": {byteNumOctal} | " +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_NUMBER") + $": {byteNumNumber} | " +
                LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_NUMINFO_BINARY") + $": {byteNumBinary}";
        }

        private void PreviousPage()
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int startByte = byteIdx - (byteLinesPerPage * 16);
            if (startByte > bytes.Length - 1)
                startByte = bytes.Length - 1;
            if (startByte < 0)
                startByte = 0;
            byteIdx = startByte;
            RequireRefresh();
        }

        private void NextPage()
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int startByte = byteIdx + (byteLinesPerPage * 16);
            if (startByte > bytes.Length - 1)
                startByte = bytes.Length - 1;
            if (startByte < 0)
                startByte = 0;
            byteIdx = startByte;
            RequireRefresh();
        }

        private void Beginning()
        {
            byteIdx = 0;
            RequireRefresh();
        }

        private void End()
        {
            byteIdx = bytes.Length - 1;
            if (byteIdx < 0)
                byteIdx = 0;
            RequireRefresh();
        }

        private string RenderContentsInHex(long ByteHighlight, long StartByte, long EndByte, byte[] FileByte, InteractiveTuiSettings settings)
        {
            // Get the un-highlighted and highlighted colors
            var entryColor = settings.PaneItemForeColor;
            var unhighlightedColorBackground = settings.BackgroundColor;
            var highlightedColorBackground = settings.PaneSelectedItemBackColor;

            // Check the index
            if (byteIdx >= FileByte.Length)
                return LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EMPTYCONTENT");

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
                    builder.Append($"{ConsoleColoring.RenderSetConsoleColor(unhighlightedColorBackground, true)}{entryColor.VTSequenceForeground()}0x{CurrentByteNumber - 1L:X8}  ");

                    // Iterate these number of bytes for the ASCII codes
                    long byteNum;
                    for (byteNum = 0; byteNum < 16 && CurrentByteNumber + byteNum <= EndByte; byteNum++)
                    {
                        byte CurrentByte = FileByte[(int)(CurrentByteNumber + byteNum - 1)];
                        builder.Append(
                            $"{(CurrentByteNumber + byteNum == ByteHighlight ? unhighlightedColorBackground : highlightedColorBackground).VTSequenceForeground()}" +
                            $"{ConsoleColoring.RenderSetConsoleColor((CurrentByteNumber + byteNum == ByteHighlight ? highlightedColorBackground : unhighlightedColorBackground), true)}" +
                            $"{CurrentByte:X2}" +
                            $"{highlightedColorBackground.VTSequenceForeground()}" +
                            $"{ConsoleColoring.RenderSetConsoleColor(unhighlightedColorBackground, true)}" +
                            $" "
                        );
                    }

                    // Pad the remaining ASCII byte display
                    int remaining = (int)(16 - byteNum);
                    int padTimes = remaining * 3 + 1;
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
                            $"{(CurrentByteNumber + byteNum == ByteHighlight ? unhighlightedColorBackground : highlightedColorBackground).VTSequenceForeground()}" +
                            $"{ConsoleColoring.RenderSetConsoleColor((CurrentByteNumber + byteNum == ByteHighlight ? highlightedColorBackground : unhighlightedColorBackground), true)}" +
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
                throw new ArgumentOutOfRangeException(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_INVALIDBYTENUMBER") + $" {StartByte}, {EndByte}");
        }

        private void AddNewByte(byte Content, long pos)
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
                    throw new ArgumentOutOfRangeException(nameof(pos), pos, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTENUMLARGER1").FormatString(bytes.LongLength));

                // Actually remove a byte
                long ByteIndex = pos - 1L;
                if (pos <= bytes.LongLength)
                {
                    FileBytesList.Insert((int)ByteIndex, Content);
                    bytes = [.. FileBytesList];
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(pos), pos, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTENUMLARGER2").FormatString(bytes.LongLength));
            }
            else
                throw new ArgumentNullException(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTEARRAYNULL"));
        }

        private void DeleteByte(long ByteNumber)
        {
            if (bytes is not null)
            {
                if (ByteNumber < 1)
                    throw new ArgumentOutOfRangeException(nameof(ByteNumber), ByteNumber, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTENUMSMALLER"));
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
                    throw new ArgumentOutOfRangeException(nameof(ByteNumber), ByteNumber, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTENUMLARGER2").FormatString(bytes.LongLength));
            }
            else
                throw new ArgumentNullException(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTEARRAYNULL"));
        }

        private void Replace(byte FromByte, byte WithByte) =>
            Replace(FromByte, WithByte, 1L, bytes.LongLength);

        private void Replace(byte FromByte, byte WithByte, long StartByte, long EndByte)
        {
            if (bytes is not null)
            {
                if (StartByte < 1)
                    throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTENUMSMALLER"));
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
                    throw new ArgumentOutOfRangeException(nameof(StartByte), StartByte, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_STARTBYTELARGER2").FormatString(bytes.LongLength));
                else if (EndByte > bytes.LongLength)
                    throw new ArgumentOutOfRangeException(nameof(EndByte), EndByte, LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_ENDBYTELARGER2").FormatString(bytes.LongLength));
            }
            else
                throw new ArgumentNullException(LanguageTools.GetLocalized("T_INPUT_STYLES_HEXEDITOR_EXCEPTION_BYTEARRAYNULL"));
        }
    }
}
