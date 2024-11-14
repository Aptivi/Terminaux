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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.TermInfo;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Effects;
using Terminaux.Writer.CyclicWriters.Renderer.Effects.Builtins;

namespace Terminaux.Writer.CyclicWriters.Renderer.Markup
{
    /// <summary>
    /// Markup parsing and processing tools
    /// </summary>
    public static class MarkupTools
    {
        internal static IEffect[] effects =
        [
            new BoldEffect(),
            new ConcealEffect(),
            new DimEffect(),
            new InvertEffect(),
            new ItalicEffect(),
            new RapidBlinkEffect(),
            new SlowBlinkEffect(),
            new StandoutEffect(),
            new StrikethroughEffect(),
            new UnderlineEffect(),
        ];

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="mark">Markup instance</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static string ParseMarkup(Mark? mark, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            if (mark is null)
                throw new TerminauxException("Markup is not specified.");
            return ParseMarkup(mark.Markup, foregroundColor, backgroundColor, initialFormat);
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static string ParseMarkup(string markup, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            // Markup is inspired by BBCode and can have escaped starting and ending sequence characters.
            var finalResult = new StringBuilder(markup);
            List<MarkupInfo> sequences = [];
            Queue<MarkupInfo> queuedSequences = [];
            StringBuilder appended = new();
            bool append = false;
            for (int i = 0; i < markup.Length; i++)
            {
                char markupChar = markup[i];
                char nextChar = i + 1 < markup.Length ? markup[i + 1] : '\0';

                // Check to see if we are escaping or not
                if (((markupChar == '[' && nextChar == '[') || (markupChar == ']' && nextChar == ']')) && queuedSequences.Count == 0)
                {
                    sequences.Add(new()
                    {
                        entranceIndex = i,
                        exitIndex = i + 1,
                        represent = $"{markupChar}",
                        isEscape = true,
                    });
                    i++;
                    continue;
                }

                // Now, parse the markups by checking for them while seeking characters to get properties inside them and to apply
                // the changes.
                if (markupChar == '[' && nextChar != '/')
                {
                    queuedSequences.Enqueue(new()
                    {
                        entranceIndex = i,
                    });
                    appended.Append($"{markupChar}");
                    append = true;
                    continue;
                }
                else if (markupChar == '[' && nextChar == '/' && queuedSequences.Count > 0)
                {
                    char nextTwoChar = i + 2 < markup.Length ? markup[i + 2] : '\0';
                    if (nextTwoChar != ']')
                        throw new TerminauxException("Invalid end tag specifier.");
                    if (queuedSequences.Count > 0)
                    {
                        var queued = queuedSequences.Dequeue();
                        queued.exitIndex = i;
                        queued.represent = appended.ToString();
                        appended.Clear();
                        sequences.Add(queued);
                    }
                    else
                        throw new TerminauxException("There are no queued sequences to end the tag with.");
                    i += 2;
                    continue;
                }
                if (queuedSequences.Count > 0 && i == markup.Length - 1)
                    throw new TerminauxException($"There are {queuedSequences.Count} missing end tags.");

                // Add a character
                if (append)
                {
                    appended.Append(markupChar);
                    if (markupChar == ']')
                        append = false;
                }
            }

            // Now that we've got properties, split them to process them
            string[] effectNames = effects.Select((effect) => effect.MarkupTag).ToArray();
            string resetAll = TermInfoDesc.Current?.ExitAttributeMode?.Value ?? "";
            for (int i = finalResult.Length - 1; i >= 0; i--)
            {
                foreach (var sequence in sequences)
                {
                    bool isEntrance = i == sequence.entranceIndex;
                    bool isExit = i == sequence.exitIndex && !sequence.isEscape;
                    if (isEntrance || isExit)
                    {
                        finalResult.Remove(i, sequence.represent.Length);

                        // Process the representation
                        string[] representations = sequence.represent.ToString().Split(' ');
                        break;
                    }
                    else
                        continue;
                }
            }
            return finalResult.ToString();
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="mark">Markup instance</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static bool TryParseMarkup(Mark? mark, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            if (mark is null)
                return false;
            return TryParseMarkup(mark.Markup, foregroundColor, backgroundColor, initialFormat);
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <param name="markup">Markup representation</param>
        /// <param name="foregroundColor">Foreground color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="backgroundColor">Background color of the initial markup color. Overrides the initial format argument.</param>
        /// <param name="initialFormat">Initial format. Overrides the foreground and the background color arguments.</param>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public static bool TryParseMarkup(string markup, Color? foregroundColor = null, Color? backgroundColor = null, string initialFormat = "")
        {
            try
            {
                ParseMarkup(markup, foregroundColor, backgroundColor, initialFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
