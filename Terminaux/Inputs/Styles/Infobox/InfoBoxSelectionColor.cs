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
using System.Collections.Generic;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Colors;
using System.Text;
using System.Linq;
using Terminaux.Writer.FancyWriters;
using Terminaux.Base.Buffered;
using System.Diagnostics;
using Terminaux.Base;
using Textify.General;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Reader;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using System.Threading;
using Terminaux.Inputs.Pointer;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with selection and color support
    /// </summary>
    public static class InfoBoxSelectionColor
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(selections, text,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(InputChoiceInfo[] selections, string text,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxSelectionPlain("", selections, text,
                             UpperLeftCornerChar, LowerLeftCornerChar,
                             UpperRightCornerChar, LowerRightCornerChar,
                             UpperFrameChar, LowerFrameChar,
                             LeftFrameChar, RightFrameChar, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColor(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxSelection background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="BackgroundColor">InfoBoxSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(InputChoiceInfo[] selections, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                "", selections, text,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(title, selections, text,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceInfo[] selections, string text,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                title, selections, text,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColor(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(string title, InputChoiceInfo[] selections, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                title, selections, text,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                InfoBoxTitledSelectionColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        internal static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledSelectionColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            int selectedChoice = -1;
            int AnswerTitleLeft = selections.Max(x => $"  {x.ChoiceName}) ".Length);

            // First, verify that we have selections
            if (selections is null || selections.Length == 0)
                return selectedChoice;

            // We need not to run the selection style when everything is disabled
            bool allDisabled = selections.All((ici) => ici.ChoiceDisabled);
            if (allDisabled)
                throw new TerminauxException("The infobox selection style requires that there is at least one choice enabled.");

            // Now, some logic to get the informational box ready
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSelectionColor), infoBoxScreenPart);
            try
            {
                // Modify the current selection according to the default
                int currentSelection = selections.Any((ici) => ici.ChoiceDefault) ? selections.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
                int selectionChoices = selections.Length > 10 ? 10 : selections.Length;

                // Edge case: We need to check to see if the current highlight is disabled
                while (selections[currentSelection].ChoiceDisabled)
                {
                    currentSelection++;
                    if (currentSelection > selections.Length - 1)
                        currentSelection = 0;
                }

                infoBoxScreenPart.AddDynamicText(() =>
                {
                    ColorTools.AllowForeground = true;

                    // Deal with the lines to actually fit text in the infobox
                    string finalInfoRendered = TextTools.FormatString(text, vars);
                    string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                    List<string> splitFinalLines = [];
                    foreach (var line in splitLines)
                    {
                        var lineSentences = ConsoleMisc.GetWrappedSentencesByWords(line, ConsoleWrapper.WindowWidth - 4);
                        foreach (var lineSentence in lineSentences)
                            splitFinalLines.Add(lineSentence);
                    }

                    // Trim the new lines until we reach a full line
                    for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                    {
                        string line = splitFinalLines[i];
                        if (!string.IsNullOrWhiteSpace(line))
                            break;
                        splitFinalLines.RemoveAt(i);
                    }

                    // Fill the info box with text inside it
                    int selectionReservedHeight = 4 + selectionChoices;
                    int maxWidth = ConsoleWrapper.WindowWidth - 4;
                    int maxHeight = splitFinalLines.Count + selectionReservedHeight;
                    if (maxHeight >= ConsoleWrapper.WindowHeight)
                        maxHeight = ConsoleWrapper.WindowHeight - 4;
                    int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
                    int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
                    int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;

                    // Fill in some selection properties
                    int selectionBoxPosX = borderX + 4;
                    int selectionBoxPosY = borderY + maxHeight - selectionReservedHeight + 3;
                    int maxSelectionWidth = maxWidth - selectionBoxPosX * 2 + 2;

                    // Buffer the box
                    var boxBuffer = new StringBuilder();
                    string border =
                        !string.IsNullOrEmpty(title) ?
                        BorderColor.RenderBorderPlain(title, borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar) :
                        BorderColor.RenderBorderPlain(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                    string borderSelection = BorderColor.RenderBorderPlain(selectionBoxPosX, selectionBoxPosY - 1, maxSelectionWidth, selectionChoices, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                    boxBuffer.Append(
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(InfoBoxTitledSelectionColor) : "")}" +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                        $"{border}" +
                        $"{borderSelection}"
                    );

                    // Render text inside it
                    ConsoleWrapper.CursorVisible = false;
                    for (int i = 0; i < splitFinalLines.Count; i++)
                    {
                        var line = splitFinalLines[i];
                        if (i % (maxHeight - selectionReservedHeight) == 0 && i > 0)
                        {
                            // Reached the end of the box. Bail, because we need to print the selection box.
                            break;
                        }
                        boxBuffer.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + i % maxHeight + 1)}" +
                            $"{line}"
                        );
                    }

                    // Now, render the selections
                    int currentPage = currentSelection / selectionChoices;
                    int startIndex = selectionChoices * currentPage;
                    for (int i = 0; i <= selectionChoices - 1; i++)
                    {
                        // Populate the selection box
                        int finalIndex = i + startIndex;
                        if (finalIndex >= selections.Length)
                            break;
                        bool selected = finalIndex == currentSelection;
                        var choice = selections[finalIndex];
                        string AnswerTitle = choice.ChoiceTitle ?? "";
                        bool disabled = choice.ChoiceDisabled;

                        // Get the option
                        string AnswerOption = $"{(selected ? ">" : disabled ? "X" : " ")} {choice}) {AnswerTitle}";
                        int answerTitleMaxLeft = ConsoleWrapper.WindowWidth;
                        if (AnswerTitleLeft < answerTitleMaxLeft)
                        {
                            string renderedChoice = $"{(selected ? ">" : disabled ? "X" : " ")} {choice.ChoiceName}) ";
                            int blankRepeats = AnswerTitleLeft - renderedChoice.Length;
                            AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{AnswerTitle}";
                        }
                        AnswerOption = AnswerOption.Truncate(maxSelectionWidth - 4);

                        // Render an entry
                        var finalForeColor = selected ? BackgroundColor : InfoBoxTitledSelectionColor;
                        var finalBackColor = selected ? InfoBoxTitledSelectionColor : BackgroundColor;
                        int leftPos = selectionBoxPosX + 1;
                        int top = selectionBoxPosY + finalIndex - startIndex;
                        if (useColor)
                        {
                            boxBuffer.Append(
                                TextWriterWhereColor.RenderWhereColorBack(AnswerOption + new string(' ', maxSelectionWidth - AnswerOption.Length - (ConsoleWrapper.WindowWidth % 2 != 0 ? 0 : 1)), leftPos, top, finalForeColor, finalBackColor)
                            );
                        }
                        else
                        {
                            boxBuffer.Append(
                                TextWriterWhereColor.RenderWhere(AnswerOption + new string(' ', maxSelectionWidth - AnswerOption.Length - (ConsoleWrapper.WindowWidth % 2 != 0 ? 0 : 1)), leftPos, top)
                            );
                        }
                    }

                    // Render the vertical bar
                    int left = maxWidth - 3;
                    if (useColor)
                    {
                        boxBuffer.Append(
                            SliderVerticalColor.RenderVerticalSlider(currentSelection + 1, selections.Length, left - 1, selectionBoxPosY - 1, ConsoleWrapper.WindowHeight - selectionChoices, 0, InfoBoxTitledSelectionColor, BackgroundColor, false)
                        );
                    }
                    else
                    {
                        boxBuffer.Append(
                            SliderVerticalColor.RenderVerticalSliderPlain(currentSelection + 1, selections.Length, left - 1, selectionBoxPosY - 1, ConsoleWrapper.WindowHeight - selectionChoices, 0, false)
                        );
                    }

                    // Render the final result
                    if (useColor)
                    {
                        boxBuffer.Append(
                            ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) +
                            ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
                        );
                    }
                    ColorTools.AllowForeground = false;
                    return boxBuffer.ToString();
                });

                // Wait for input
                bool bail = false;
                bool cancel = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Handle keypress
                    SpinWait.SpinUntil(() => PointerListener.InputAvailable);
                    bool goingUp = false;
                    if (PointerListener.PointerAvailable)
                    {
                        void UpdatePositionBasedOnMouse(PointerEventContext mouse)
                        {
                            // Make pages based on console window height
                            int currentPage = currentSelection / selectionChoices;
                            int startIndex = selectionChoices * currentPage;
                            int endIndex = selectionChoices * currentPage + 10;
                            endIndex = endIndex > selectionChoices ? endIndex : selectionChoices;

                            // Now, translate coordinates to the selected index
                            string finalInfoRendered = TextTools.FormatString(text, vars);
                            string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                            List<string> splitFinalLines = [];
                            foreach (var line in splitLines)
                            {
                                var lineSentences = ConsoleMisc.GetWrappedSentencesByWords(line, ConsoleWrapper.WindowWidth - 4);
                                foreach (var lineSentence in lineSentences)
                                    splitFinalLines.Add(lineSentence);
                            }
                            for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                            {
                                string line = splitFinalLines[i];
                                if (!string.IsNullOrWhiteSpace(line))
                                    break;
                                splitFinalLines.RemoveAt(i);
                            }
                            int selectionReservedHeight = 4 + selectionChoices;
                            int maxWidth = ConsoleWrapper.WindowWidth - 4;
                            int maxHeight = splitFinalLines.Count + selectionReservedHeight;
                            if (maxHeight >= ConsoleWrapper.WindowHeight)
                                maxHeight = ConsoleWrapper.WindowHeight - 4;
                            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
                            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
                            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
                            int selectionBoxPosX = borderX + 4;
                            int selectionBoxPosY = borderY + maxHeight - selectionReservedHeight + 2;
                            int leftPos = selectionBoxPosX + 1;
                            int maxSelectionWidth = maxWidth - selectionBoxPosX * 2 + 2;
                            if (mouse.Coordinates.x <= leftPos || mouse.Coordinates.x >= maxSelectionWidth ||
                                mouse.Coordinates.y <= selectionBoxPosY || mouse.Coordinates.y >= selectionBoxPosY + selectionChoices + 1)
                                return;
                            int listIndex = mouse.Coordinates.y - selectionBoxPosY - 1;
                            listIndex = startIndex + listIndex;
                            listIndex = listIndex >= selections.Length ? selections.Length - 1 : listIndex;
                            currentSelection = listIndex;
                        }

                        // Mouse input received.
                        var mouse = TermReader.ReadPointer();
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                goingUp = true;
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = selections.Length - 1;
                                break;
                            case PointerButton.WheelDown:
                                currentSelection++;
                                if (currentSelection > selections.Length - 1)
                                    currentSelection = 0;
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                UpdatePositionBasedOnMouse(mouse);
                                bail = true;
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                var selectedInstance = selections[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxColor.WriteInfoBox($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                UpdatePositionBasedOnMouse(mouse);
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !PointerListener.PointerActive)
                    {
                        var key = TermReader.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                goingUp = true;
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = selections.Length - 1;
                                break;
                            case ConsoleKey.DownArrow:
                                currentSelection++;
                                if (currentSelection > selections.Length - 1)
                                    currentSelection = 0;
                                break;
                            case ConsoleKey.Home:
                                currentSelection = 0;
                                break;
                            case ConsoleKey.End:
                                currentSelection = selections.Length - 1;
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                {
                                    int currentPageMove = (currentSelection - 1) / selectionChoices;
                                    int startIndexMove = selectionChoices * currentPageMove;
                                    currentSelection = startIndexMove;
                                    if (currentSelection < 0)
                                        currentSelection = 0;
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    int currentPageMove = currentSelection / selectionChoices;
                                    int startIndexMove = selectionChoices * (currentPageMove + 1);
                                    currentSelection = startIndexMove;
                                    if (currentSelection > selections.Length - 1)
                                        currentSelection = selections.Length - 1;
                                }
                                break;
                            case ConsoleKey.Tab:
                                var selectedInstance = selections[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxColor.WriteInfoBox($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (selectionChoices <= 0)
                                    break;
                                var entriesString = selections.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput("Write a search term (case insensitive)").ToLower();
                                var resultEntries = entriesString.Select((entry, idx) => ($"{idx + 1}", entry)).Where((tuple) => tuple.entry.ChoiceName.ToLower().Contains(keyword) || tuple.entry.ChoiceTitle.ToLower().Contains(keyword)).Select((tuple) => (tuple.Item1, $"{tuple.entry.ChoiceName}) {tuple.entry.ChoiceTitle}")).ToArray();
                                if (resultEntries.Length > 0)
                                {
                                    var choices = InputChoiceTools.GetInputChoices(resultEntries);
                                    int answer = WriteInfoBoxSelection(choices, "Select one of the entries:");
                                    if (answer < 0)
                                        break;
                                    var resultIdx = int.Parse(resultEntries[answer].Item1);
                                    currentSelection = resultIdx;
                                }
                                else
                                    InfoBoxColor.WriteInfoBox("No item found.");
                                ScreenTools.CurrentScreen?.RequireRefresh();
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;

                        }
                    }

                    // Verify that the current position is not a disabled choice
                    if (currentSelection >= 0)
                    {
                        while (selections[currentSelection].ChoiceDisabled)
                        {
                            if (goingUp)
                            {
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = selections.Length - 1;
                            }
                            else
                            {
                                currentSelection++;
                                if (currentSelection > selections.Length - 1)
                                    currentSelection = 0;
                            }
                        }
                    }
                }
                if (!cancel)
                    selectedChoice = currentSelection;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                if (useColor)
                {
                    TextWriterRaw.WriteRaw(
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) +
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
                    );
                }
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(nameof(InfoBoxSelectionColor));
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }

            // Return the selected choice, or -1
            return selectedChoice;
        }

        static InfoBoxSelectionColor()
        {
            if (GeneralColorTools.CheckConsoleOnCall && !ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
