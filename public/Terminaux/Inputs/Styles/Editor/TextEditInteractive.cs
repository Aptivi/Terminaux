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
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Terminaux.Inputs.Interactive;
using Textify.General;
using Textify.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using System.Collections.ObjectModel;

namespace Terminaux.Inputs.Styles.Editor
{
    /// <summary>
    /// Interactive text editor
    /// </summary>
    public class TextEditInteractive : TextualUI
    {
        private string status = "";
        private List<string> lines = [];
        private InteractiveTuiSettings settings = InteractiveTuiSettings.GlobalSettings;
        private string cachedFind = "";
        private bool entering;
        private int lineIdx = 0;
        private int lineColIdx = 0;
        private bool fullscreen;
        private bool editable = true;

        private static Keybinding[] Bindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_EXIT"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_KEYBINDINGS"), ConsoleKey.K),
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
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_ENTER"), ConsoleKey.I),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_DELETEHERE"), ConsoleKey.X),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_INSERT"), ConsoleKey.F1),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_REMOVELINE"), ConsoleKey.F2),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_INSERT"), ConsoleKey.F1, ConsoleModifiers.Shift),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_REMOVELINE"), ConsoleKey.F2, ConsoleModifiers.Shift),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_REPLACE"), ConsoleKey.F3),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_KEYBINDING_REPLACEALL"), ConsoleKey.F3, ConsoleModifiers.Shift),
        ];

        private static Keybinding[] BindingsEntering =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_STOPENTERING"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_NEWLINE"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_BACKSPACE"), ConsoleKey.Backspace),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_KEYBINDING_DELETE"), ConsoleKey.Delete),
        ];

        /// <summary>
        /// Opens an interactive text editor
        /// </summary>
        /// <param name="lines">Target number of lines</param>
        /// <param name="settings">TUI settings</param>
        /// <param name="fullscreen">Whether it's a fullscreen viewer or not</param>
        /// <param name="edit">Whether it's editable or not</param>
        public static void OpenInteractive(ref List<string> lines, InteractiveTuiSettings? settings = null, bool fullscreen = false, bool edit = true)
        {
            // Make a new TUI
            var finalSettings = settings ?? InteractiveTuiSettings.GlobalSettings;
            var textEditor = new TextEditInteractive()
            {
                status = LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_STATUS_READY"),
                editable = edit,
                lines = lines,
                settings = finalSettings,
                fullscreen = fullscreen,
            };

            // Assign keybindings
            textEditor.UpdateKeybindings();

            // Run the TUI
            TextualUITools.RunTui(textEditor);
            lines = textEditor.lines;
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
                builder.Append(RenderTextViewBox());

                // Render the status
                builder.Append(RenderStatus());
            }

            // Now, render the text with the current selection
            builder.Append(RenderContentsWithSelection());
            return builder.ToString();
        }

        private string RenderKeybindings()
        {
            var keybindingsRenderable = new Keybindings()
            {
                KeybindingList = editable ? entering ? BindingsEntering : EditorBindings : Bindings,
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
            StatusTextInfo();

            // Now, render the status
            var builder = new StringBuilder();
            builder.Append(
                $"{ColorTools.RenderSetConsoleColor(settings.ForegroundColor)}" +
                $"{ColorTools.RenderSetConsoleColor(settings.BackgroundColor, true)}" +
                $"{TextWriterWhereColor.RenderWherePlain(status + ConsoleClearing.GetClearLineToRightSequence(), 0, 0)}"
            );
            return builder.ToString();
        }

        private string RenderTextViewBox()
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
        }

        private string RenderContentsWithSelection()
        {
            // Check the lines
            if (lines.Count == 0)
                return "";

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
                    int finalPos = overflown ? adjustedIdx + 1 : 1;
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
        }

        private void InsertChar(char keyChar)
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Insert a character
            lines[lineIdx] = lines[lineIdx].Insert(lines[lineIdx].Length == 0 ? 0 : lineColIdx, $"{keyChar}");
            MoveForward();
        }

        private void RuboutChar()
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
                MoveBackward();
            }
            else if (lineIdx > 0)
            {
                string substring = lines[lineIdx];
                int oldLen = lines[lineIdx - 1].Length;
                lines[lineIdx - 1] = lines[lineIdx - 1] + substring;
                RemoveLine();
                UpdateColumnIndex(oldLen);
            }
        }

        private void DeleteChar()
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
                UpdateLineIndex(lineIdx);
            }
            else
                RemoveLine();
        }

        private void RenderKeybindingsBox()
        {
            // Show the available keys list
            var finalBindings = editable ? entering ? BindingsEntering : EditorBindings : Bindings;
            if (finalBindings.Length == 0)
                return;

            // User needs an infobox that shows all available keys
            string bindingsHelp = KeybindingTools.RenderKeybindingHelpText(finalBindings);
            InfoBoxModalColor.WriteInfoBoxModal(bindingsHelp, new InfoBoxSettings(settings.InfoBoxSettings)
            {
                Title = LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_KEYBINDING_AVAILABLE_KEYBINDINGS")
            });
            RequireRefresh();
        }

        private void MoveBackward() =>
            UpdateColumnIndex(lineColIdx - 1);

        private void MoveForward() =>
            UpdateColumnIndex(lineColIdx + 1);

        private void MoveUp() =>
            UpdateLineIndex(lineIdx - 1);

        private void MoveDown() =>
            UpdateLineIndex(lineIdx + 1);

        private void Insert()
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

            MoveDown();
            UpdateColumnIndex(0);
        }

        private void RemoveLine()
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Remove a line
            RemoveLine(lineIdx + 1);
            MoveUp();
        }

        private void InsertNoMove()
        {
            // Insert a line
            if (lines.Count == 0)
                lines.Add("");
            else
                lines.Insert(lineIdx + 1, "");
            UpdateLineIndex(lineIdx);
        }

        private void RemoveLineNoMove()
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Remove a line
            RemoveLine(lineIdx + 1);
            UpdateLineIndex(lineIdx);
        }

        private void Replace()
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_FINDSTRINGPROMPT"), settings.InfoBoxSettings);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_REPLACESTRINGPROMPT"), settings.InfoBoxSettings);

            // Do the replacement!
            Replace(replacementText, replacedText, lineIdx + 1);
            RequireRefresh();
        }

        private void ReplaceAll()
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_FINDSTRINGPROMPT"), settings.InfoBoxSettings);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_REPLACESTRINGPROMPT"), settings.InfoBoxSettings);

            // Do the replacement!
            Replace(replacementText, replacedText);
            RequireRefresh();
        }

        private void FindNext()
        {
            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string text = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_FINDSTRINGREGEXPROMPT"), settings.InfoBoxSettings);

            // See if we have a cached find if the user didn't provide any string to find
            if (string.IsNullOrEmpty(text))
            {
                if (string.IsNullOrEmpty(cachedFind))
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_STRINGREGEXREQUIRED"), settings.InfoBoxSettings);
                    return;
                }
                else
                    text = cachedFind;
            }

            // Validate the regex
            if (!RegexTools.IsValidRegex(text))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_STRINGREGEXINVALID"), settings.InfoBoxSettings);
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
                UpdateLineIndex(r);
                UpdateColumnIndex(c);
                cachedFind = text;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_EDITORS_NOTFOUND"), settings.InfoBoxSettings);
            RequireRefresh();
        }

        private void StatusTextInfo()
        {
            // Get the status
            status =
                LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_TEXTINFO_LINES") + $": {lines.Count} | " +
                LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_TEXTINFO_COLUMN") + $": {lineColIdx + 1} | " +
                LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_TEXTINFO_ROW") + $": {lineIdx + 1}";

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

        private void PreviousPage()
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage;
            if (startIndex > lines.Count)
                startIndex = lines.Count;
            UpdateLineIndex(startIndex - 1 < 0 ? 0 : startIndex - 1);
        }

        private void NextPage()
        {
            int SeparatorMinimumHeightInterior = fullscreen ? 0 : 2;
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4 + (2 - SeparatorMinimumHeightInterior) + (fullscreen ? 1 : 0);
            int currentPage = lineIdx / lineLinesPerPage;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (endIndex > lines.Count - 1)
                endIndex = lines.Count - 1;
            UpdateLineIndex(endIndex);
        }

        private void Beginning()
        {
            UpdateLineIndex(0);
            UpdateColumnIndex(0);
        }

        private void End()
        {
            UpdateLineIndex(lines.Count - 1);
            int maxLen = lines[lineIdx].Length;
            maxLen -= entering ? 0 : 1;
            UpdateColumnIndex(maxLen);
        }

        private void UpdateLineIndex(int lnIdx)
        {
            lineIdx = lnIdx;
            if (lineIdx > lines.Count - 1)
                lineIdx = lines.Count - 1;
            if (lineIdx < 0)
                lineIdx = 0;
            UpdateColumnIndex(lineColIdx);
        }

        private void UpdateColumnIndex(int clIdx)
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
            RequireRefresh();
        }

        private void SwitchEnter()
        {
            entering = !entering;
            RequireRefresh();
            UpdateLineIndex(lineIdx);
            UpdateKeybindings();
        }

        private (int, string)[] GetAbsoluteSequences(string source, ReadOnlyDictionary<VtSequenceType, VtSequenceInfo[]> sequencesCollections)
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

        private void RemoveLine(int LineNumber)
        {
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                if (LineNumber <= lines.Count)
                    lines.RemoveAt(LineIndex);
                else
                    throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_EXCEPTION_LINENUMLARGER").FormatString(lines.Count));
            }
            else
                throw new ArgumentNullException(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_EXCEPTION_LINESARRAYNULL"));
        }

        private void Replace(string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new ArgumentNullException(nameof(From));
            if (lines is not null)
            {
                for (int LineIndex = 0; LineIndex <= lines.Count - 1; LineIndex++)
                    lines[LineIndex] = lines[LineIndex].Replace(From, With);
            }
            else
                throw new ArgumentNullException(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_EXCEPTION_LINESARRAYNULL"));
        }

        private void Replace(string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new ArgumentNullException(nameof(From));
            if (lines is not null)
            {
                long LineIndex = LineNumber - 1;
                if (LineNumber <= lines.Count)
                    lines[(int)LineIndex] = lines[(int)LineIndex].Replace(From, With);
                else
                    throw new ArgumentOutOfRangeException(nameof(LineNumber), LineNumber, LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_EXCEPTION_LINENUMLARGER").FormatString(lines.Count));
            }
            else
                throw new ArgumentNullException(LanguageTools.GetLocalized("T_INPUT_STYLES_TEXTEDITOR_EXCEPTION_LINESARRAYNULL"));
        }

        private void UpdateKeybindings()
        {
            Keybindings.Clear();

            // Add some common keybindings
            Keybindings.Add((Bindings[3], (ui, _, _) => ((TextEditInteractive)ui).MoveUp()));
            Keybindings.Add((Bindings[4], (ui, _, _) => ((TextEditInteractive)ui).MoveDown()));
            Keybindings.Add((Bindings[5], (ui, _, _) => ((TextEditInteractive)ui).MoveBackward()));
            Keybindings.Add((Bindings[6], (ui, _, _) => ((TextEditInteractive)ui).MoveForward()));
            Keybindings.Add((Bindings[7], (ui, _, _) => ((TextEditInteractive)ui).PreviousPage()));
            Keybindings.Add((Bindings[8], (ui, _, _) => ((TextEditInteractive)ui).NextPage()));
            Keybindings.Add((Bindings[9], (ui, _, _) => ((TextEditInteractive)ui).Beginning()));
            Keybindings.Add((Bindings[10], (ui, _, _) => ((TextEditInteractive)ui).End()));

            // Add mode-specific keybindings
            if (entering)
            {
                Keybindings.Add((BindingsEntering[0], (ui, _, _) => ((TextEditInteractive)ui).SwitchEnter()));
                Keybindings.Add((BindingsEntering[1], (ui, _, _) => ((TextEditInteractive)ui).Insert()));
                Keybindings.Add((BindingsEntering[2], (ui, _, _) => ((TextEditInteractive)ui).RuboutChar()));
                Keybindings.Add((BindingsEntering[3], (ui, _, _) => ((TextEditInteractive)ui).DeleteChar()));
                Fallback = (ui, cki, _) => ((TextEditInteractive)ui).InsertChar(cki.KeyChar);
            }
            else
            {
                Fallback = null;

                // Assign keybindings
                Keybindings.Add((Bindings[0], (ui, _, _) => TextualUITools.ExitTui(ui)));
                Keybindings.Add((Bindings[1], (ui, _, _) => ((TextEditInteractive)ui).RenderKeybindingsBox()));
                Keybindings.Add((Bindings[2], (ui, _, _) => ((TextEditInteractive)ui).FindNext()));
                Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_FINDNEXT"), ConsoleKey.Oem2), (ui, _, _) => ((TextEditInteractive)ui).FindNext()));

                // Assign edit keybindings
                if (editable)
                {
                    Keybindings.Add((EditorBindings[11], (ui, _, _) => ((TextEditInteractive)ui).SwitchEnter()));
                    Keybindings.Add((EditorBindings[12], (ui, _, _) => ((TextEditInteractive)ui).DeleteChar()));
                    Keybindings.Add((EditorBindings[13], (ui, _, _) => ((TextEditInteractive)ui).Insert()));
                    Keybindings.Add((EditorBindings[14], (ui, _, _) => ((TextEditInteractive)ui).RemoveLine()));
                    Keybindings.Add((EditorBindings[15], (ui, _, _) => ((TextEditInteractive)ui).InsertNoMove()));
                    Keybindings.Add((EditorBindings[16], (ui, _, _) => ((TextEditInteractive)ui).RemoveLineNoMove()));
                    Keybindings.Add((EditorBindings[17], (ui, _, _) => ((TextEditInteractive)ui).Replace()));
                    Keybindings.Add((EditorBindings[18], (ui, _, _) => ((TextEditInteractive)ui).ReplaceAll()));
                }
            }
        }
    }
}
