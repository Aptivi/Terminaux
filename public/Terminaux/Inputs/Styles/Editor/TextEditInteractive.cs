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
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using System.Collections.Generic;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Terminaux.Inputs.Interactive;
using Textify.General;
using Textify.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Inputs.Styles.Editor
{
    /// <summary>
    /// Interactive text editor
    /// </summary>
    public static class TextEditInteractive
    {
        private static string status = "";
        private static string cachedFind = "";
        private static bool bail;
        private static bool entering;
        private static int lineIdx = 0;
        private static int lineColIdx = 0;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding("Exit", ConsoleKey.Escape),
            new Keybinding("Keybindings", ConsoleKey.K),
            new Keybinding("Enter...", ConsoleKey.I),
            new Keybinding("Delete here", ConsoleKey.X),
            new Keybinding("Insert", ConsoleKey.F1),
            new Keybinding("Remove Line", ConsoleKey.F2),
            new Keybinding("Insert", ConsoleKey.F1, ConsoleModifiers.Shift),
            new Keybinding("Remove Line", ConsoleKey.F2, ConsoleModifiers.Shift),
            new Keybinding("Find Next", ConsoleKey.Divide),
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
        /// <param name="fullscreen">Whether it's a fullscreen viewer or not</param>
        public static void OpenInteractive(ref List<string> lines, InteractiveTuiSettings? settings = null, bool fullscreen = false)
        {
            // Set status
            status = "Ready";
            bail = false;
            settings ??= InteractiveTuiSettings.GlobalSettings;

            // Check to see if the list of lines is empty
            if (lines.Count == 0)
                lines = [""];

            // Main loop
            lineIdx = 0;
            lineColIdx = 0;
            cachedFind = "";
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
                        RenderTextViewBox(ref screen, settings);

                        // Render the status
                        RenderStatus(ref screen, settings);
                    }

                    // Now, render the visual text with the current selection
                    RenderContentsWithSelection(lineIdx, ref screen, lines, settings, fullscreen);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.ReadKey();
                    HandleKeypress(keypress, ref lines, screen, fullscreen, settings);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal($"The text editor failed: {ex.Message}");
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
                var keybindingsRenderable = new Keybindings()
                {
                    KeybindingList = entering ? bindingsEntering : bindings,
                    BuiltinColor = settings.KeyBindingBuiltinColor,
                    BuiltinForegroundColor = settings.KeyBindingBuiltinForegroundColor,
                    BuiltinBackgroundColor = settings.KeyBindingBuiltinBackgroundColor,
                    OptionColor = settings.KeyBindingOptionColor,
                    OptionForegroundColor = settings.OptionForegroundColor,
                    OptionBackgroundColor = settings.OptionBackgroundColor,
                    BackgroundColor = settings.BackgroundColor,
                    Left = 0,
                    Top = ConsoleWrapper.WindowHeight - 1,
                    Width = ConsoleWrapper.WindowWidth - 1,
                };
                return keybindingsRenderable.Render();
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
            screen.AddBufferedPart("Text editor interactive - Text view box", part);
        }

        private static void RenderContentsWithSelection(int lineIdx, ref Screen screen, List<string> lines, InteractiveTuiSettings settings, bool fullscreen)
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
                int SeparatorConsoleWidthInterior = fullscreen ? ConsoleWrapper.WindowWidth : ConsoleWrapper.WindowWidth - 2;
                int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;

                // Get the colors
                var unhighlightedColorBackground = settings.BackgroundColor;
                var highlightedColorBackground = settings.PaneSelectedItemBackColor;

                // Get the start and the end indexes for lines
                int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
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
                            source += absolutes[a].Item2;
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

        private static void HandleKeypress(ConsoleKeyInfo key, ref List<string> lines, Screen screen, bool fullscreen, InteractiveTuiSettings settings)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveBackward(lines, screen);
                    return;
                case ConsoleKey.RightArrow:
                    MoveForward(lines, screen);
                    return;
                case ConsoleKey.UpArrow:
                    MoveUp(lines, screen);
                    return;
                case ConsoleKey.DownArrow:
                    MoveDown(lines, screen);
                    return;
                case ConsoleKey.PageUp:
                    PreviousPage(lines, screen, fullscreen);
                    return;
                case ConsoleKey.PageDown:
                    NextPage(lines, screen, fullscreen);
                    return;
                case ConsoleKey.Home:
                    Beginning(lines, screen);
                    return;
                case ConsoleKey.End:
                    End(lines, screen);
                    return;
            }
            if (entering)
            {
                // Handle the entering keys apppropriately
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        RuboutChar(ref lines, screen);
                        break;
                    case ConsoleKey.Delete:
                        DeleteChar(ref lines, screen);
                        break;
                    case ConsoleKey.Escape:
                        SwitchEnter(lines, screen);
                        break;
                    case ConsoleKey.Enter:
                        Insert(ref lines, screen);
                        break;
                    default:
                        InsertChar(key.KeyChar, ref lines, screen);
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
                        screen.RequireRefresh();
                        break;
                    case ConsoleKey.I:
                        SwitchEnter(lines, screen);
                        break;
                    case ConsoleKey.X:
                        DeleteChar(ref lines, screen);
                        break;
                    case ConsoleKey.F1:
                        if (key.Modifiers == ConsoleModifiers.Shift)
                            InsertNoMove(ref lines, screen);
                        else
                            Insert(ref lines, screen);
                        break;
                    case ConsoleKey.F2:
                        if (key.Modifiers == ConsoleModifiers.Shift)
                            RemoveLineNoMove(ref lines, screen);
                        else
                            RemoveLine(ref lines, screen);
                        break;
                    case ConsoleKey.F3:
                        if (key.Modifiers == ConsoleModifiers.Shift)
                            ReplaceAll(ref lines, settings);
                        else
                            Replace(ref lines, settings);
                        screen.RequireRefresh();
                        break;
                    case ConsoleKey.Oem2:
                    case ConsoleKey.Divide:
                        FindNext(ref lines, settings, screen);
                        screen.RequireRefresh();
                        break;
                }
            }
        }

        private static void InsertChar(char keyChar, ref List<string> lines, Screen screen)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Insert a character
            lines[lineIdx] = lines[lineIdx].Insert(lines[lineIdx].Length == 0 ? 0 : lineColIdx, $"{keyChar}");
            MoveForward(lines, screen);
        }

        private static void RuboutChar(ref List<string> lines, Screen screen)
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
                MoveBackward(lines, screen);
            }
            else if (lineIdx > 0)
            {
                string substring = lines[lineIdx];
                int oldLen = lines[lineIdx - 1].Length;
                lines[lineIdx - 1] = lines[lineIdx - 1] + substring;
                RemoveLine(ref lines, screen);
                UpdateColumnIndex(oldLen, lines, screen);
            }
        }

        private static void DeleteChar(ref List<string> lines, Screen screen)
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
                UpdateLineIndex(lineIdx, lines, screen);
            }
            else
                RemoveLine(ref lines, screen);
        }

        private static void RenderKeybindingsBox(InteractiveTuiSettings settings)
        {
            // Show the available keys list
            var finalBindings = entering ? bindingsEntering : bindings;
            if (finalBindings.Length == 0)
                return;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingTools.RenderKeybindingHelpText(finalBindings);
            InfoBoxModalColor.WriteInfoBoxModalColorBack("Available keybindings", bindingsHelp, settings.BoxForegroundColor, settings.BoxBackgroundColor);
        }

        private static void MoveBackward(List<string> lines, Screen screen) =>
            UpdateColumnIndex(lineColIdx - 1, lines, screen);

        private static void MoveForward(List<string> lines, Screen screen) =>
            UpdateColumnIndex(lineColIdx + 1, lines, screen);

        private static void MoveUp(List<string> lines, Screen screen) =>
            UpdateLineIndex(lineIdx - 1, lines, screen);

        private static void MoveDown(List<string> lines, Screen screen) =>
            UpdateLineIndex(lineIdx + 1, lines, screen);

        private static void Insert(ref List<string> lines, Screen screen)
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

            MoveDown(lines, screen);
            UpdateColumnIndex(0, lines, screen);
        }

        private static void RemoveLine(ref List<string> lines, Screen screen)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Remove a line
            RemoveLine(ref lines, lineIdx + 1);
            MoveUp(lines, screen);
        }

        private static void InsertNoMove(ref List<string> lines, Screen screen)
        {
            // Insert a line
            if (lines.Count == 0)
                lines.Add("");
            else
                lines.Insert(lineIdx + 1, "");
            UpdateLineIndex(lineIdx, lines, screen);
        }

        private static void RemoveLineNoMove(ref List<string> lines, Screen screen)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Remove a line
            RemoveLine(ref lines, lineIdx + 1);
            UpdateLineIndex(lineIdx, lines, screen);
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

        private static void FindNext(ref List<string> lines, InteractiveTuiSettings settings, Screen screen)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string text = InfoBoxInputColor.WriteInfoBoxInputColorBack("Write a string to find. Supports regular expressions.", settings.BoxForegroundColor, settings.BoxBackgroundColor);

            // See if we have a cached find if the user didn't provide any string to find
            if (string.IsNullOrEmpty(text))
            {
                if (string.IsNullOrEmpty(cachedFind))
                {
                    InfoBoxModalColor.WriteInfoBoxModal("String is required to find, but you haven't provided one.", settings.BorderSettings);
                    return;
                }
                else
                    text = cachedFind;
            }

            // Validate the regex
            if (!RegexTools.IsValidRegex(text))
            {
                InfoBoxModalColor.WriteInfoBoxModal("This string to find is not a valid regular expression.", settings.BorderSettings);
                return;
            }

            // Determine row and column
            (int row, int column) = (lineIdx, lineColIdx + 1);
            if (row >= lines.Count)
                row = 0;
            if (column >= lines[row].Length)
            {
                column = 0;
                row++;
                if (row >= lines.Count)
                    row--;
            }

            // Now, run a loop to find, continuing from top where necessary
            bool found = false;
            bool firstTime = true;
            int r = row, c = column;
            while (!found)
            {
                // Get a line and match it
                string line = lines[r];
                var regex = new Regex(text);
                if (!regex.IsMatch(line, c))
                {
                    // If we came to the same column, stop.
                    if (!firstTime && r == row)
                        break;

                    // Go to the next line and continue from top if necessary
                    c = 0;
                    r++;
                    if (r >= lines.Count)
                        r = 0;
                    firstTime = false;
                    continue;
                }
                firstTime = false;

                // Now, assuming that there is a match, get info
                var match = regex.Match(line, c);
                found = true;
                c = match.Index;
            }

            // Update line index and column index if found. Otherwise, show a message
            if (found)
            {
                UpdateLineIndex(r, lines, screen);
                UpdateColumnIndex(c, lines, screen);
                cachedFind = text;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal("Not found. Check your syntax or broaden your search.", settings.BorderSettings);
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

        private static void PreviousPage(List<string> lines, Screen screen, bool fullscreen)
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage;
            if (startIndex > lines.Count)
                startIndex = lines.Count;
            UpdateLineIndex(startIndex - 1 < 0 ? 0 : startIndex - 1, lines, screen);
        }

        private static void NextPage(List<string> lines, Screen screen, bool fullscreen)
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int currentPage = lineIdx / lineLinesPerPage;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (endIndex > lines.Count - 1)
                endIndex = lines.Count - 1;
            UpdateLineIndex(endIndex, lines, screen);
        }

        private static void Beginning(List<string> lines, Screen screen) =>
            UpdateLineIndex(0, lines, screen);

        private static void End(List<string> lines, Screen screen) =>
            UpdateLineIndex(lines.Count - 1, lines, screen);

        private static void UpdateLineIndex(int lnIdx, List<string> lines, Screen screen)
        {
            lineIdx = lnIdx;
            if (lineIdx > lines.Count - 1)
                lineIdx = lines.Count - 1;
            if (lineIdx < 0)
                lineIdx = 0;
            UpdateColumnIndex(lineColIdx, lines, screen);
        }

        private static void UpdateColumnIndex(int clIdx, List<string> lines, Screen screen)
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
            screen.RequireRefresh();
        }

        private static void SwitchEnter(List<string> lines, Screen screen)
        {
            entering = !entering;
            screen.RequireRefresh();
            UpdateLineIndex(lineIdx, lines, screen);
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
