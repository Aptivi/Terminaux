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

using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Textify.General;

namespace Terminaux.Spectre
{
    /// <summary>
    /// Translation tools
    /// </summary>
	public static class TranslationTools
	{
        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="Markup"/> from Terminaux's <see cref="Mark"/>
        /// </summary>
        /// <param name="mark">Terminaux's markup instance</param>
        /// <returns>Spectre.Console's markup instance</returns>
        public static Markup GetMarkup(Mark mark)
        {
            StringBuilder originalMarkup = new(mark.Markup);

            // Check to see if there are any properties we need to replace
            var markupInfos = MarkupTools.GetMarkupInfo(mark);
            List<string> effectNames = [.. MarkupTools.effects.Select((effect) => effect.MarkupTag)];
            for (int i = markupInfos.Length - 1; i >= 0; i--)
            {
                // Split the representations
                MarkupInfo markupInfo = markupInfos[i];
                if (markupInfo.Escape)
                    continue;
                string representationGroup = markupInfo.Representation.RemovePrefix("[").RemoveSuffix("]");
                List<string> representations = [.. representationGroup.Split(' ')];

                // Parse the effect or the color
                for (int r = representations.Count - 1; r >= 0; r--)
                {
                    string representation = representations[r];
                    if (effectNames.Contains(representation))
                    {
                        // Spectre.Console doesn't have "standout"
                        if (representation == "standout")
                            representations.RemoveAt(r);
                    }
                    else if (ColorTools.TryParseColor(representation))
                    {
                        // Convert to RGB sequence
                        var parsedColor = new Colors.Color(representation);
                        representations[r] = parsedColor.Hex;
                    }
                }

                // Apply the changes
                originalMarkup.Remove(markupInfo.EntranceIndex + 1, representationGroup.Length);
                originalMarkup.Insert(markupInfo.EntranceIndex + 1, string.Join(" ", representations));
            }
            return new(originalMarkup.ToString());
        }

        /// <summary>
        /// Returns a compatible Spectre.Console <see cref="Panel"/> from Terminaux's <see cref="BoxFrame"/>
        /// </summary>
        /// <param name="boxFrame">Terminaux's box frame instance</param>
        /// <returns>Spectre.Console's panel instance</returns>
        public static Panel GetPanel(BoxFrame boxFrame)
        {
            // Get all properties
            var width = boxFrame.Width;
            var height = boxFrame.Height;

            // Return the new panel
            string panelTitle = boxFrame.Text;
            panelTitle = VtSequenceTools.FilterVTSequences(panelTitle);
            var spectrePanel = new Panel("")
            {
                Header = new(panelTitle),
                Width = width + 2,
                Height = height + 2,
                Padding = new(boxFrame.Padding.Left, boxFrame.Padding.Top, boxFrame.Padding.Right, boxFrame.Padding.Bottom),
                Border = BoxBorder.Rounded,
            };
            return spectrePanel;
        }
    }
}
