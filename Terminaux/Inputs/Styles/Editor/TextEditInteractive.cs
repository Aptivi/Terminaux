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
using Terminaux.Inputs.Interactive;
using Terminaux.Writer.MiscWriters;
using Textify.General;

namespace Terminaux.Inputs.Styles.Editor
{
    /// <summary>
    /// Interactive text editor
    /// </summary>
    public static class TextEditInteractive
    {
        private static string status = "";
        private static bool bail;
        private static bool entering;
        private static int lineIdx = 0;
        private static int lineColIdx = 0;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding("Exit", ConsoleKey.Escape),
            new Keybinding("Keybindings", ConsoleKey.K),
            new Keybinding("Enter...", ConsoleKey.I),
            new Keybinding("Insert", ConsoleKey.F1),
            new Keybinding("Remove Line", ConsoleKey.F2),
            new Keybinding("Insert", ConsoleKey.F1, ConsoleModifiers.Shift),
            new Keybinding("Remove Line", ConsoleKey.F2, ConsoleModifiers.Shift),
            new Keybinding("Replace", ConsoleKey.F3),
            new Keybinding("Replace All", ConsoleKey.F3, ConsoleModifiers.Shift),
        ];
        private static readonly Keybinding[] bindingsEntering =
        [
            new Keybinding("Stop Entering", ConsoleKey.Escape),
            new Keybinding("New Line", ConsoleKey.Enter),
        ];

        /// <summary>
        /// Opens an interactive text editor
        /// </summary>
        /// <param name="lines">Target number of lines</param>
        /// <param name="settings">TUI settings</param>
        public static void OpenInteractive(ref List<string> lines, InteractiveTuiSettings? settings = null)
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
                    HandleKeypress(keypress, ref lines, screen, settings);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox($"The text editor failed: {ex.Message}");
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
                return KeybindingsWriter.RenderKeybindings(entering ? bindingsEntering : bindings,
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

                    // Now, get the line range
                    var lineBuilder = new StringBuilder();
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
                    lineBuilder.Append(source);

                    // Highlight the selection
                    if (i == lineIdx + 1)
                    {
                        bool overflown = lineColIdx >= lines[i - 1].Length;
                        int adjustedIdx = lineColIdx % SeparatorConsoleWidthInterior;
                        int finalPos = 1;
                        for (int a = adjustedIdx; a > 0 && !overflown; a--)
                            finalPos += TextTools.GetCharWidth(lines[i - 1][lineColIdx - a]);
                        lineBuilder.Append(CsiSequences.GenerateCsiCursorPosition(finalPos + 1, SeparatorMinimumHeightInterior + count + 1));
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(unhighlightedColorBackground));
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(highlightedColorBackground, true, true));
                        lineBuilder.Append(overflown ? ' ' : lines[i - 1][lineColIdx]);
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(unhighlightedColorBackground, true));
                        lineBuilder.Append(ColorTools.RenderSetConsoleColor(highlightedColorBackground));
                        lineBuilder.Append(CsiSequences.GenerateCsiCursorPosition(SeparatorConsoleWidthInterior + 3 - (SeparatorConsoleWidthInterior - ConsoleChar.EstimateCellWidth(source)), SeparatorMinimumHeightInterior + count + 1));
                    }
                    lineBuilder.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );

                    // Change the color depending on the highlighted line and column
                    sels.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(2, SeparatorMinimumHeightInterior + count + 1)}" +
                        $"{ColorTools.RenderSetConsoleColor(highlightedColorBackground)}" +
                        $"{ColorTools.RenderSetConsoleColor(unhighlightedColorBackground, true)}" +
                        lineBuilder
                    );
                    count++;
                }
                return sels.ToString();
            });
            screen.AddBufferedPart("Text editor interactive - Contents", part);
        }

        private static void HandleKeypress(ConsoleKeyInfo key, ref List<string> lines, Screen screen, InteractiveTuiSettings settings)
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
            }
            if (entering)
            {
                // Handle the entering keys apppropriately
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        RuboutChar(ref lines);
                        break;
                    case ConsoleKey.Delete:
                        DeleteChar(ref lines);
                        break;
                    case ConsoleKey.Escape:
                        SwitchEnter(lines, screen);
                        break;
                    case ConsoleKey.Enter:
                        Insert(ref lines);
                        break;
                    default:
                        InsertChar(key.KeyChar, ref lines);
                        break;
                }
            }
            else
            {
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        bail = true;
                        break;
                    case ConsoleKey.K:
                        RenderKeybindingsBox(settings);
                        break;
                    case ConsoleKey.I:
                        SwitchEnter(lines, screen);
                        break;
                    case ConsoleKey.F1:
                        if (key.Modifiers == ConsoleModifiers.Shift)
                            InsertNoMove(ref lines);
                        else
                            Insert(ref lines);
                        break;
                    case ConsoleKey.F2:
                        if (key.Modifiers == ConsoleModifiers.Shift)
                            RemoveLineNoMove(ref lines);
                        else
                            RemoveLine(ref lines);
                        break;
                    case ConsoleKey.F3:
                        if (key.Modifiers == ConsoleModifiers.Shift)
                            ReplaceAll(ref lines, settings);
                        else
                            Replace(ref lines, settings);
                        break;
                    default:
                        InsertChar(key.KeyChar, ref lines);
                        break;
                }
            }
        }

        private static void InsertChar(char keyChar, ref List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Insert a character
            lines[lineIdx] = lines[lineIdx].Insert(lines[lineIdx].Length == 0 ? 0 : lineColIdx, $"{keyChar}");
            MoveForward(lines);
        }

        private static void RuboutChar(ref List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Delete a character
            if (lineColIdx > 0)
            {
                var sequencesCollections = VtSequenceTools.MatchVTSequences(lines[lineIdx]);
                var absolutes = GetAbsoluteSequences(lines[lineIdx], sequencesCollections);
                int colIdx = absolutes[lineColIdx - 1].Item1;
                int seqLength = absolutes[lineColIdx - 1].Item2.Length;
                lines[lineIdx] = lines[lineIdx].Remove(colIdx, seqLength);
                MoveBackward(lines);
            }
            else if (lineIdx > 0)
            {
                string substring = lines[lineIdx];
                int oldLen = lines[lineIdx - 1].Length;
                lines[lineIdx - 1] = lines[lineIdx - 1] + substring;
                RemoveLine(ref lines);
                UpdateColumnIndex(oldLen, lines);
            }
        }

        private static void DeleteChar(ref List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return;
            if (lines[lineIdx].Length == lineColIdx && lineColIdx > 0)
                return;

            // Delete a character
            if (lines[lineIdx].Length > 0)
            {
                var sequencesCollections = VtSequenceTools.MatchVTSequences(lines[lineIdx]);
                var absolutes = GetAbsoluteSequences(lines[lineIdx], sequencesCollections);
                int colIdx = absolutes[lineColIdx].Item1;
                int seqLength = absolutes[lineColIdx].Item2.Length;
                lines[lineIdx] = lines[lineIdx].Remove(colIdx, seqLength);
                UpdateLineIndex(lineIdx, lines);
            }
            else
                RemoveLine(ref lines);
        }

        private static void RenderKeybindingsBox(InteractiveTuiSettings settings)
        {
            // Show the available keys list
            var finalBindings = entering ? bindingsEntering : bindings;
            if (finalBindings.Length == 0)
                return;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingsWriter.RenderKeybindingHelpText(finalBindings);
            InfoBoxColor.WriteInfoBoxColorBack(bindingsHelp, settings.BoxForegroundColor, settings.BoxBackgroundColor);
        }

        private static void MoveBackward(List<string> lines) =>
            UpdateColumnIndex(lineColIdx - 1, lines);

        private static void MoveForward(List<string> lines) =>
            UpdateColumnIndex(lineColIdx + 1, lines);

        private static void MoveUp(List<string> lines) =>
            UpdateLineIndex(lineIdx - 1, lines);

        private static void MoveDown(List<string> lines) =>
            UpdateLineIndex(lineIdx + 1, lines);

        private static void Insert(ref List<string> lines)
        {
            // Insert a line
            if (lines.Count == 0)
                lines.Add("");
            else
            {
                // Check to see if the current position is not at the end of the line
                var sequencesCollections = VtSequenceTools.MatchVTSequences(lines[lineIdx]);
                var absolutes = GetAbsoluteSequences(lines[lineIdx], sequencesCollections);
                int colIdx = absolutes[lineColIdx].Item1;
                string substringNewLine = lines[lineIdx].Substring(colIdx);
                string substringOldLine = lines[lineIdx].Substring(0, colIdx + 1);
                lines[lineIdx] = substringOldLine;
                lines.Insert(lineIdx + 1, substringNewLine);
            }

            MoveDown(lines);
            UpdateColumnIndex(0, lines);
        }

        private static void RemoveLine(ref List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Remove a line
            RemoveLine(ref lines, lineIdx + 1);
            MoveUp(lines);
        }

        private static void InsertNoMove(ref List<string> lines)
        {
            // Insert a line
            if (lines.Count == 0)
                lines.Add("");
            else
                lines.Insert(lineIdx + 1, "");
            UpdateLineIndex(lineIdx, lines);
        }

        private static void RemoveLineNoMove(ref List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Remove a line
            RemoveLine(ref lines, lineIdx + 1);
            UpdateLineIndex(lineIdx, lines);
        }

        private static void Replace(ref List<string> lines, InteractiveTuiSettings settings)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInputColorBack("Write the string to find", settings.BoxForegroundColor, settings.BoxBackgroundColor);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInputColorBack("Write the replacement string", settings.BoxForegroundColor, settings.BoxBackgroundColor);

            // Do the replacement!
            Replace(ref lines, replacementText, replacedText, lineIdx + 1);
        }

        private static void ReplaceAll(ref List<string> lines, InteractiveTuiSettings settings)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInputColorBack("Write the string to find", settings.BoxForegroundColor, settings.BoxBackgroundColor);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInputColorBack("Write the replacement string", settings.BoxForegroundColor, settings.BoxBackgroundColor);

            // Do the replacement!
            Replace(ref lines, replacementText, replacedText);
        }

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
            if (entering)
                return;
            var sequencesCollections = VtSequenceTools.MatchVTSequences(lines[lineIdx]);
            var absolutes = GetAbsoluteSequences(lines[lineIdx], sequencesCollections);
            var currChar = absolutes[lineColIdx].Item2;
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
            int maxLen = lines[lineIdx].Length;
            maxLen -= entering ? 0 : 1;
            if (lineColIdx > maxLen)
                lineColIdx = maxLen;
            if (lineColIdx < 0)
                lineColIdx = 0;
        }

        private static void SwitchEnter(List<string> lines, Screen screen)
        {
            entering = !entering;
            screen.RequireRefresh();
            UpdateLineIndex(lineIdx, lines);
        }

        private static (int, string)[] GetAbsoluteSequences(string source, (VtSequenceType type, Match[] sequences)[] sequencesCollections)
        {
            int vtSeqIdx = 0;
            List<(int, string)> sequences = [];
            string sequence = "";
            for (int l = 0; l < source.Length; l++)
            {
                sequence += ConsolePositioning.BufferChar(source, sequencesCollections, ref l, ref vtSeqIdx, out bool isVtSequence);
                if (isVtSequence)
                    continue;
                sequences.Add((l, sequence));
                sequence = "";
            }
            return [.. sequences];
        }

        private static void RemoveLine(ref List<string> lines, int LineNumber)
        {
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                if (LineNumber <= lines.Count)
                    lines.RemoveAt(LineIndex);
                else
                    throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, $"The specified line number may not be larger than {lines.Count}.");
            }
            else
                throw new ArgumentNullException("Can't perform this operation on null lines list.");
        }

        private static void Replace(ref List<string> lines, string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new ArgumentNullException(nameof(From));
            if (lines is not null)
            {
                for (int LineIndex = 0; LineIndex <= lines.Count - 1; LineIndex++)
                    lines[LineIndex] = lines[LineIndex].Replace(From, With);
            }
            else
                throw new ArgumentNullException("Can't perform this operation on null lines list.");
        }

        private static void Replace(ref List<string> lines, string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new ArgumentNullException(nameof(From));
            if (lines is not null)
            {
                long LineIndex = LineNumber - 1;
                if (LineNumber <= lines.Count)
                    lines[(int)LineIndex] = lines[(int)LineIndex].Replace(From, With);
                else
                    throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, $"The specified line number may not be larger than {lines.Count}.");
            }
            else
                throw new ArgumentNullException("Can't perform this operation on null lines list.");
        }
    }
}
