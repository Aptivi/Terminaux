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
using System.Linq;
using System.Text;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Reader;
using Terminaux.Base.Extensions;

namespace Terminaux.Inputs.Presentation.Elements
{
    /// <summary>
    /// Dynamic text element
    /// </summary>
    public class DynamicTextElement : IElement
    {
        /// <inheritdoc/>
        public bool IsInput => false;

        /// <inheritdoc/>
        public string? WrittenInput { get; set; }

        /// <summary>
        /// The first argument denotes the action which invokes the text, and the rest for the parameters to be formatted
        /// </summary>
        public object[]? Arguments { get; set; }

        /// <summary>
        /// Renders the text
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
            string text = TextTools.FormatString(Arguments is not null && Arguments.Length > 0 ? ((Func<string>)Arguments[0])() : "", finalArgs);

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
            TextWriterWhereColor.WriteWhereColor(buffer.ToString(), presentationUpperInnerBorderLeft, seekTop, false, new Color(ConsoleColors.White));
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

            // Get the text and the arguments
            object[] finalArgs = Arguments is not null && Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];
            string text = TextTools.FormatString(Arguments is not null && Arguments.Length > 0 ? ((Func<string>)Arguments[0])() : "", finalArgs);

            // Check the bounds
            string[] splitText = ConsoleMisc.GetWrappedSentencesByWords(text, presentationLowerInnerBorderLeft - presentationUpperInnerBorderLeft);
            int maxHeight = presentationLowerInnerBorderTop - ConsoleWrapper.CursorTop + 3;
            return splitText.Length > maxHeight;
        }

        /// <inheritdoc/>
        public Action<object[]>? InvokeActionInput { get; }

        /// <inheritdoc/>
        public Action? InvokeAction { get; set; }
    }
}
