﻿//
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

using System.Collections.Generic;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    /// <summary>
    /// Informational box class
    /// </summary>
    public class InfoBox
    {
        /// <summary>
        /// Positioning settings
        /// </summary>
        public InfoBoxPositioning Positioning { get; set; } = new();

        /// <summary>
        /// Text that will be rendered inside the informational box
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Informational box settings
        /// </summary>
        public InfoBoxSettings Settings { get; set; } = InfoBoxSettings.GlobalSettings;

        /// <summary>
        /// A list of graphical cyclic writers
        /// </summary>
        public List<GraphicalCyclicWriter> Elements { get; set; } = [];

        /// <summary>
        /// Processed dimensions of the infobox
        /// </summary>
        public (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int maxTextHeight, int linesLength) Dimensions
        {
            get
            {
                // Deal with the lines to actually fit text in the infobox
                string[] splitFinalLines = TextWriterTools.GetFinalLines(Text);
                var (maxWidth, maxHeight, maxRenderWidth, borderX, borderY) = !Positioning.Autofit ?
                    InfoBoxTools.GetDimensions(Positioning.Width, Positioning.Height, Positioning.Left, Positioning.Top, Positioning.ExtraHeight) :
                    InfoBoxTools.GetDimensions(splitFinalLines, Positioning.ExtraHeight);
                int maxTextHeight = maxHeight - Positioning.ExtraHeight;
                return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, splitFinalLines.Length);
            }
        }

        /// <summary>
        /// Renders this informational box
        /// </summary>
        /// <param name="increment">Incrementation rate for paged text in the text area (usually passed initialized to 0)</param>
        /// <param name="currIdx">Current index of text line in the text area</param>
        /// <param name="drawBar">Whether to draw the slider bar for the text area or not</param>
        /// <param name="writeBinding">Whether to write the key bindings in the upper right corner of the box or not</param>
        /// <returns></returns>
        public string Render(ref int increment, int currIdx, bool drawBar, bool writeBinding)
        {
            var (maxWidth, maxHeight, _, borderX, borderY, _, _) = Dimensions;

            // Fill the info box with text inside it
            var boxBuffer = new StringBuilder(
                InfoBoxTools.RenderText(maxWidth, maxHeight, borderX, borderY, Positioning.ExtraHeight, Settings.Title, Text, Settings.BorderSettings, Settings.ForegroundColor, Settings.BackgroundColor, Settings.UseColors, ref increment, currIdx, drawBar, writeBinding)
            );

            // Render the elements
            foreach (var element in Elements)
                boxBuffer.Append(element.Render());

            // Reset colors
            if (Settings.UseColors)
            {
                boxBuffer.Append(
                    ColorTools.RenderRevertForeground() +
                    ColorTools.RenderRevertBackground()
                );
            }

            // Return the rendered elements
            return boxBuffer.ToString();
        }
    }
}
