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

using Terminaux.Writer.ConsoleWriters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Reader;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;

namespace Terminaux.Inputs.Presentation.Elements
{
    /// <summary>
    /// Multiple choice input element
    /// </summary>
    public class MultipleChoiceInputElement : IElement
    {
        /// <inheritdoc/>
        public bool IsInput => true;

        /// <inheritdoc/>
        public string? WrittenInput { get; set; }

        /// <summary>
        /// The first argument denotes the prompt to be written, and the rest for the parameters to be formatted
        /// </summary>
        public object[]? Arguments { get; set; }

        /// <summary>
        /// Renders the element
        /// </summary>
        public void Render()
        {
            // Populate some variables
            int presentationUpperBorderLeft = 2;
            int presentationUpperBorderTop = 1;
            int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
            int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
            int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
            int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 4;

            // Get the text and the arguments
            object[] finalArgs = Arguments is not null && Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];
            string text = TextTools.FormatString((string)(Arguments is not null && Arguments.Length > 0 ? Arguments[0] : ""), finalArgs);

            // Check the bounds
            string[] splitText = ConsoleMisc.GetWrappedSentencesByWords(text, presentationLowerInnerBorderLeft - presentationUpperBorderLeft + 2);
            int top = ConsoleWrapper.CursorTop;
            int seekTop = ConsoleWrapper.CursorTop;
            var buffer = new StringBuilder();
            foreach (string split in splitText)
            {
                int maxHeight = presentationLowerInnerBorderTop - top + 1;
                if (maxHeight < 0)
                {
                    // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                    TextWriterWhereColor.WriteWhereColor(buffer.ToString(), presentationUpperInnerBorderLeft, seekTop, false, new Color(ConsoleColors.White));
                    TermReader.ReadPointerOrKey();
                    TextWriterRaw.WriteRaw(PresentationTools.ClearPresentation());
                    seekTop = top = presentationUpperInnerBorderTop;
                    buffer.Clear();
                }

                // Write the part
                buffer.Append(split + "\n");
                top++;
            }

            // Write the buffer text
            string bufferText = buffer.ToString();
            int maxHeightFinal = presentationLowerInnerBorderTop - top + 1;
            if (maxHeightFinal <= 0)
            {
                // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                TextWriterWhereColor.WriteWhereColor(bufferText, presentationUpperInnerBorderLeft, seekTop, false, new Color(ConsoleColors.White));
                TermReader.ReadPointerOrKey();
                TextWriterRaw.WriteRaw(PresentationTools.ClearPresentation());
                top = presentationUpperInnerBorderTop;
                buffer.Clear();
            }
            else
                TextWriterWhereColor.WriteWhereColor(bufferText, presentationUpperInnerBorderLeft, seekTop, false, new Color(ConsoleColors.White));

            // Flatten the enumerables to their string value representations
            List<string> choices = [];
            foreach (var finalArg in finalArgs)
            {
                if (finalArg is IEnumerable enumerable && finalArg is not string)
                    foreach (var enumerableValue in enumerable)
                        choices.Add(enumerableValue.ToString());
                else
                    choices.Add(finalArg.ToString());
            }

            // Render the choices (with checking for bounds, again)
            TextWriterWhereColor.WriteWhereColor("\n", presentationUpperInnerBorderLeft, Console.CursorTop, false, presentationUpperInnerBorderLeft, new Color(ConsoleColors.White));
            string[] finalChoices = [.. choices];
            int choiceNum = 1;
            var choiceBuffer = new StringBuilder();
            int choiceSeekTop = ConsoleWrapper.CursorTop;
            int choiceTop = ConsoleWrapper.CursorTop;
            foreach (string choice in finalChoices)
            {
                string finalChoice = $"{choiceNum}) {choice}";
                string[] splitTextChoice = ConsoleMisc.GetWrappedSentencesByWords(finalChoice, presentationLowerInnerBorderLeft - presentationUpperBorderLeft + 2);
                foreach (string split in splitTextChoice)
                {
                    int maxHeight = presentationLowerInnerBorderTop - choiceTop + 1;
                    if (maxHeight < 0)
                    {
                        // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                        TextWriterWhereColor.WriteWhereColor(choiceBuffer.ToString(), presentationUpperInnerBorderLeft, choiceSeekTop, false, new Color(ConsoleColors.White));
                        TermReader.ReadPointerOrKey();
                        TextWriterRaw.WriteRaw(PresentationTools.ClearPresentation());
                        choiceSeekTop = choiceTop = presentationUpperInnerBorderTop;
                        choiceBuffer.Clear();
                    }

                    // Write the part
                    choiceBuffer.Append(split + "\n");
                    choiceTop++;
                }
                choiceNum++;
            }

            // Write the choicebuffer text
            string choiceBufferText = choiceBuffer.ToString();
            int maxChoiceHeightFinal = presentationLowerInnerBorderTop - top + 1;
            if (maxChoiceHeightFinal <= 0)
            {
                // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                TextWriterWhereColor.WriteWhereColor(choiceBufferText, presentationUpperInnerBorderLeft, choiceSeekTop, false, new Color(ConsoleColors.White));
                TermReader.ReadPointerOrKey();
                TextWriterRaw.WriteRaw(PresentationTools.ClearPresentation());
                buffer.Clear();
            }
            else
                TextWriterWhereColor.WriteWhereColor(choiceBufferText, presentationUpperInnerBorderLeft, choiceSeekTop, false, new Color(ConsoleColors.White));

            // Populate relevant settings
            var settings = new TermReaderSettings()
            {
                RightMargin = presentationUpperInnerBorderLeft
            };

            // Get the input
            TextWriterWhereColor.WriteWhereColor("\n", presentationUpperInnerBorderLeft, Console.CursorTop, false, presentationUpperInnerBorderLeft, new Color(ConsoleColors.White));
            int cursorLeft = presentationUpperInnerBorderLeft;
            int cursorTop = ConsoleWrapper.CursorTop;
            string[] selected = [];
            while (selected.Length == 0 || !selected.All((selectedChoice) => finalChoices.Contains(selectedChoice)))
            {
                ConsoleWrapper.SetCursorPosition(cursorLeft, cursorTop);
                TextWriterColor.WriteColor("Select your choice separated by semicolons: ", false, new Color(ConsoleColors.Silver));
                ConsoleWrapper.CursorVisible = true;
                WrittenInput = TermReader.Read("", "", settings, false, true);
                selected = WrittenInput.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                ConsoleWrapper.CursorVisible = false;
            }

            // Trim repeated inputs
            List<string> finalSelected = [];
            foreach (string choice in selected)
                if (!finalSelected.Contains(choice))
                    finalSelected.Add(choice);
            WrittenInput = string.Join(";", finalSelected);
        }

        /// <summary>
        /// Checks to see if the text is possibly overflowing the slideshow display
        /// </summary>
        public bool IsPossibleOutOfBounds()
        {
            // Populate some variables
            int presentationUpperBorderLeft = 2;
            int presentationUpperBorderTop = 1;
            int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
            int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
            int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 4;

            // Get the text, the arguments, and the choices
            object[] finalArgs = Arguments is not null && Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];

            // Flatten the enumerables to their string value representations
            List<string> choices = [];
            foreach (var finalArg in finalArgs)
            {
                if (finalArg is IEnumerable enumerable && finalArg is not string)
                    foreach (var enumerableValue in enumerable)
                        choices.Add(enumerableValue.ToString());
                else
                    choices.Add(finalArg.ToString());
            }

            string[] finalChoices = [.. choices];
            string text = TextTools.FormatString((string)(Arguments is not null && Arguments.Length > 0 ? Arguments[0] : ""), finalArgs) + "\n\n";

            // Add the choices to the text
            for (int choice = 0; choice < finalChoices.Length; choice++)
                text += $"\n{choice + 1}) {finalChoices[choice]}";
            text += "\n\n";

            // Check the bounds
            string[] splitText = ConsoleMisc.GetWrappedSentencesByWords(text, presentationLowerInnerBorderLeft - presentationUpperInnerBorderLeft);
            int maxHeight = presentationLowerInnerBorderTop - ConsoleWrapper.CursorTop + 3;
            return splitText.Length > maxHeight;
        }

        /// <inheritdoc/>
        public Action<object[]>? InvokeActionInput { get; set; }

        /// <inheritdoc/>
        public Action? InvokeAction { get; }
    }
}
