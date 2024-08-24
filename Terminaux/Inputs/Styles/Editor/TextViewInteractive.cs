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
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using System.Collections.Generic;
using Terminaux.Writer.FancyWriters;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Terminaux.Writer.MiscWriters.Tools;
using Terminaux.Writer.MiscWriters;
using Terminaux.Inputs.Interactive;

namespace Terminaux.Inputs.Styles.Editor
{
    /// <summary>
    /// Interactive text viewer
    /// </summary>
    public static class TextViewInteractive
    {
        private static string status = "";
        private static bool bail;
        private static int lineIdx = 0;
        private static int lineColIdx = 0;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding("Exit", ConsoleKey.Escape),
            new Keybinding("Keybindings", ConsoleKey.K),
        ];

        /// <summary>
        /// Opens an interactive text editor
        /// </summary>
        /// <param name="lines">Target number of lines</param>
        /// <param name="settings">TUI settings</param>
        public static void OpenInteractive(List<string> lines, InteractiveTuiSettings? settings = null)
        {
            // Set status
            status = "Ready";
            bail = false;
            settings ??= new();

            // Check to see if the list of lines is empty
            if (lines.Count == 0)
                lines = [""];

            // Main loop
            lineIdx = 0;
            lineColIdx = 0;
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
                    RenderTextViewBox(ref screen, settings);

                    // Now, render the visual hex with the current selection
                    RenderContentsWithSelection(lineIdx, ref screen, lines, settings);

                    // Render the status
                    RenderStatus(ref screen, settings);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.ReadKey();
                    HandleKeypress(keypress, lines, settings);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox($"The text viewer failed: {ex.Message}");
            }
            bail = false;
            ScreenTools.UnsetCurrent(screen);

            // Close the file and clean up
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
            screen.AddBufferedPart("Text editor interactive - Keybindings", part);
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
            screen.AddBufferedPart("Text editor interactive - Status", part);
        }

        private static void RenderTextViewBox(ref Screen screen, InteractiveTuiSettings settings)
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
            screen.AddBufferedPart("Text editor interactive - Text view box", part);
        }

        private static void RenderContentsWithSelection(int lineIdx, ref Screen screen, List<string> lines, InteractiveTuiSettings settings)
        {
            // First, update the status
            StatusTextInfo(lines);

            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, make a dynamic text
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                // Get the widths and heights
                int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
                int SeparatorMinimumHeightInterior = 2;

                // Get the colors
                var unhighlightedColorBackground = settings.BackgroundColor;
                var highlightedColorBackground = settings.PaneSelectedItemBackColor;

                // Get the start and the end indexes for lines
                int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
                int currentPage = lineIdx / lineLinesPerPage;
                int startIndex = lineLinesPerPage * currentPage + 1;
                int endIndex = lineLinesPerPage * (currentPage + 1);
                if (startIndex > lines.Count)
                    startIndex = lines.Count;
                if (endIndex > lines.Count)
                    endIndex = lines.Count;

                // Get the lines and highlight the selection
                int count = 0;
                var sels = new StringBuilder();
                for (int i = startIndex; i <= endIndex; i++)
                {
                    // Get a line
                    string source = lines[i - 1].Replace("\t", ">");
                    if (source.Length == 0)
                        source = " ";
                    var sequencesCollections = VtSequenceTools.MatchVTSequences(source);
                    int vtSeqIdx = 0;

                    // Seek through the whole string to find unprintable characters
                    var sourceBuilder = new StringBuilder();
                    int width = ConsoleChar.EstimateCellWidth(source);
                    for (int l = 0; l < source.Length; l++)
                    {
                        string sequence = ConsolePositioning.BufferChar(source, sequencesCollections, ref l, ref vtSeqIdx, out bool isVtSequence);
                        bool unprintable = ConsoleChar.EstimateCellWidth(sequence) == 0;
                        string rendered = unprintable && !isVtSequence ? "." : sequence;
                        sourceBuilder.Append(rendered);
                    }
                    source = sourceBuilder.ToString();

                    // Highlight the selection
                    var lineBuilder = new StringBuilder();
                    if (i == lineIdx + 1)
                    {
                        lineBuilder.Append(CsiSequences.GenerateCsiCursorPosition(lineColIdx % SeparatorConsoleWidthInterior + 2, SeparatorMinimumHeightInterior + count + 1));
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(unhighlightedColorBackground));
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(highlightedColorBackground, true, true));
                        lineBuilder.Append(' ');
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(unhighlightedColorBackground, true));
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(highlightedColorBackground));
                        lineBuilder.Append(CsiSequences.GenerateCsiCursorPosition(SeparatorConsoleWidthInterior + 3 - (SeparatorConsoleWidthInterior - ConsoleChar.EstimateCellWidth(source)), SeparatorMinimumHeightInterior + count + 1));
                    }

                    // Now, get the line range
                    string line = lineBuilder.ToString();
                    var absolutes = GetAbsoluteSequences(source, sequencesCollections);
                    if (source.Length > 0)
                    {
                        int charsPerPage = SeparatorConsoleWidthInterior;
                        int currentCharPage = lineColIdx / charsPerPage;
                        int startLineIndex = charsPerPage * currentCharPage;
                        int endLineIndex = charsPerPage * (currentCharPage + 1);
                        if (startLineIndex > absolutes.Length)
                            startLineIndex = absolutes.Length;
                        if (endLineIndex > absolutes.Length)
                            endLineIndex = absolutes.Length;
                        source = "";
                        for (int a = startLineIndex; a < endLineIndex; a++)
                            source += absolutes[a];
                    }
                    line = source + line + ColorTools.RenderRevertForeground() + ColorTools.RenderRevertBackground();

                    // Change the color depending on the highlighted line and column
                    sels.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(2, SeparatorMinimumHeightInterior + count + 1)}" +
                        $"{ColorTools.RenderSetConsoleColor(highlightedColorBackground)}" +
                        $"{ColorTools.RenderSetConsoleColor(unhighlightedColorBackground, true)}" +
                        line
                    );
                    count++;
                }
                return sels.ToString();
            });
            screen.AddBufferedPart("Text editor interactive - Contents", part);
        }

        private static void HandleKeypress(ConsoleKeyInfo key, List<string> lines, InteractiveTuiSettings settings)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveBackward(lines);
                    return;
                case ConsoleKey.RightArrow:
                    MoveForward(lines);
                    return;
                case ConsoleKey.UpArrow:
                    MoveUp(lines);
                    return;
                case ConsoleKey.DownArrow:
                    MoveDown(lines);
                    return;
                case ConsoleKey.PageUp:
                    PreviousPage(lines);
                    return;
                case ConsoleKey.PageDown:
                    NextPage(lines);
                    return;
                case ConsoleKey.Home:
                    Beginning(lines);
                    return;
                case ConsoleKey.End:
                    End(lines);
                    return;
                case ConsoleKey.Escape:
                    bail = true;
                    return;
                case ConsoleKey.K:
                    RenderKeybindingsBox(lines, settings);
                    return;
            }
        }

        private static List<string> RenderKeybindingsBox(List<string> lines, InteractiveTuiSettings settings)
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return lines;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingsWriter.RenderKeybindingHelpText(bindings);
            InfoBoxColor.WriteInfoBoxColorBack(bindingsHelp, settings.BoxForegroundColor, settings.BoxBackgroundColor);
            return lines;
        }

        private static void MoveBackward(List<string> lines) =>
            UpdateColumnIndex(lineColIdx - 1, lines);

        private static void MoveForward(List<string> lines) =>
            UpdateColumnIndex(lineColIdx + 1, lines);

        private static void MoveUp(List<string> lines) =>
            UpdateLineIndex(lineIdx - 1, lines);

        private static void MoveDown(List<string> lines) =>
            UpdateLineIndex(lineIdx + 1, lines);

        private static void StatusTextInfo(List<string> lines)
        {
            // Get the status
            status =
                $"Lines: {lines.Count} | " +
                $"Column: {lineColIdx + 1} | " +
                $"Row: {lineIdx + 1}";

            // Check to see if the current character is unprintable
            if (lines.Count == 0)
                return;
            if (lines[lineIdx].Length == 0)
                return;
            var sequencesCollections = VtSequenceTools.MatchVTSequences(lines[lineIdx]);
            var absolutes = GetAbsoluteSequences(lines[lineIdx], sequencesCollections);
            var currChar = absolutes[lineColIdx];
            if (ConsoleChar.EstimateCellWidth(currChar) == 0)
                status += " | Bin";
            if (currChar == "\t")
                status += $" | Tab: {(int)currChar[0]}";
        }

        private static void PreviousPage(List<string> lines)
        {
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage;
            if (startIndex > lines.Count)
                startIndex = lines.Count;
            UpdateLineIndex(startIndex - 1 < 0 ? 0 : startIndex - 1, lines);
        }

        private static void NextPage(List<string> lines)
        {
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentPage = lineIdx / lineLinesPerPage;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (endIndex > lines.Count - 1)
                endIndex = lines.Count - 1;
            UpdateLineIndex(endIndex, lines);
        }

        private static void Beginning(List<string> lines) =>
            UpdateLineIndex(0, lines);

        private static void End(List<string> lines) =>
            UpdateLineIndex(lines.Count - 1, lines);

        private static void UpdateLineIndex(int lnIdx, List<string> lines)
        {
            lineIdx = lnIdx;
            if (lineIdx > lines.Count - 1)
                lineIdx = lines.Count - 1;
            if (lineIdx < 0)
                lineIdx = 0;
            UpdateColumnIndex(lineColIdx, lines);
        }

        private static void UpdateColumnIndex(int clIdx, List<string> lines)
        {
            lineColIdx = clIdx;
            if (lines.Count == 0)
            {
                lineColIdx = 0;
                return;
            }
            int maxLen = ConsoleChar.EstimateCellWidth(lines[lineIdx]);
            maxLen--;
            if (lineColIdx > maxLen)
                lineColIdx = maxLen;
            if (lineColIdx < 0)
                lineColIdx = 0;
        }

        private static string[] GetAbsoluteSequences(string source, (VtSequenceType type, Match[] sequences)[] sequencesCollections)
        {
            int vtSeqIdx = 0;
            List<string> sequences = [];
            string sequence = "";
            for (int l = 0; l < source.Length; l++)
            {
                sequence += ConsolePositioning.BufferChar(source, sequencesCollections, ref l, ref vtSeqIdx, out bool isVtSequence);
                if (isVtSequence)
                    continue;
                sequences.Add(sequence);
                sequence = "";
            }
            return [.. sequences];
        }
    }
}
