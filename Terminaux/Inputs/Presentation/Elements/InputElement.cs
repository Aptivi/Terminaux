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
using Terminaux.Reader;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Colors;

namespace Terminaux.Inputs.Presentation.Elements
{
    /// <summary>
    /// Input element
    /// </summary>
    public class InputElement : IElement
    {
        /// <inheritdoc/>
        public bool IsInput => true;

        /// <inheritdoc/>
        public string WrittenInput { get; set; }

        /// <summary>
        /// The first argument denotes the prompt to be written, and the rest for the parameters to be formatted
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// Renders the text
        /// </summary>
        public void Render()
        {
            // Get the text and the arguments
            object[] finalArgs = Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];
            string text = TextTools.FormatString((string)(Arguments.Length > 0 ? Arguments[0] : ""), finalArgs);

            // Check the bounds
            string[] splitText = TextTools.GetWrappedSentences(text, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperBorderLeft + 2);
            int top = ConsoleWrapper.CursorTop;
            int seekTop = ConsoleWrapper.CursorTop;
            var buffer = new StringBuilder();
            for (int i = 0; i < splitText.Length; i++)
            {
                string split = splitText[i];
                int maxHeight = PresentationTools.PresentationLowerInnerBorderTop - top + 1;
                if (maxHeight < 0)
                {
                    // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                    TextWriterWhereColor.WriteWhereColor(buffer.ToString(), PresentationTools.PresentationUpperInnerBorderLeft, seekTop, false, new Color(ConsoleColors.White));
                    TermReader.ReadKey();
                    TextWriterRaw.WritePlain(PresentationTools.ClearPresentation(), false);
                    seekTop = top = PresentationTools.PresentationUpperInnerBorderTop;
                    buffer.Clear();
                }

                // Write the part
                buffer.Append(split + (i == splitText.Length - 1 ? "" : "\n"));
                top++;
            }

            // Write the buffer text
            string bufferText = buffer.ToString();
            string[] splitBufferText = TextTools.GetWrappedSentences(bufferText, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperBorderLeft + 2);
            int maxHeightFinal = PresentationTools.PresentationLowerInnerBorderTop - top + 1;
            if (maxHeightFinal <= 0)
            {
                // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                TextWriterWhereColor.WriteWhereColor(bufferText, PresentationTools.PresentationUpperInnerBorderLeft, seekTop, false, new Color(ConsoleColors.White));
                TermReader.ReadKey();
                TextWriterRaw.WritePlain(PresentationTools.ClearPresentation(), false);
                seekTop = top = PresentationTools.PresentationUpperInnerBorderTop;
                buffer.Clear();
            }
            else
                TextWriterWhereColor.WriteWhereColor(bufferText, PresentationTools.PresentationUpperInnerBorderLeft, seekTop, false, new Color(ConsoleColors.White));

            // Populate relevant settings
            var settings = new TermReaderSettings()
            {
                RightMargin = PresentationTools.PresentationUpperInnerBorderLeft
            };

            // Get the input
            ConsoleWrapper.CursorVisible = true;
            WrittenInput = TermReader.Read("", "", settings, false, true);
            ConsoleWrapper.CursorVisible = false;
        }

        /// <summary>
        /// Checks to see if the text is possibly overflowing the slideshow display
        /// </summary>
        public bool IsPossibleOutOfBounds()
        {
            // Get the text and the arguments
            object[] finalArgs = Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];
            string text = TextTools.FormatString((string)(Arguments.Length > 0 ? Arguments[0] : ""), finalArgs);

            // Check the bounds
            string[] splitText = TextTools.GetWrappedSentences(text, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperInnerBorderLeft);
            int maxHeight = PresentationTools.PresentationLowerInnerBorderTop - ConsoleWrapper.CursorTop + 3;
            return splitText.Length > maxHeight;
        }

        /// <inheritdoc/>
        public Action<object[]> InvokeActionInput { get; set; }

        /// <inheritdoc/>
        public Action InvokeAction { get; }
    }
}
