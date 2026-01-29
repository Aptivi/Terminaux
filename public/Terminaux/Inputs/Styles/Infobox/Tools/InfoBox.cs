//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
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
        public InfoBoxPositioning Positioning =>
            Settings.Positioning;

        /// <summary>
        /// Text that will be rendered inside the informational box
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Informational box settings
        /// </summary>
        public InfoBoxSettings Settings { get; set; } = InfoBoxSettings.GlobalSettings;

        /// <summary>
        /// A list of renderable elements in a container
        /// </summary>
        public Container Elements { get; set; } = new();

        /// <summary>
        /// Processed dimensions of the infobox
        /// </summary>
        public (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int maxTextHeight, int linesLength) Dimensions
        {
            get
            {
                // Deal with the lines to actually fit text in the infobox
                string[] splitFinalLines = TextWriterTools.GetFinalLines(Text, ConsoleWrapper.WindowWidth - 6);
                var (maxWidth, maxHeight, maxRenderWidth, borderX, borderY) = !Positioning.Autofit ?
                    InfoBoxTools.GetDimensions(Positioning.Width, Positioning.Height, Positioning.Left, Positioning.Top, Positioning.ExtraHeight, Positioning.ExtraWidth) :
                    InfoBoxTools.GetDimensions(splitFinalLines, Positioning.ExtraHeight, Positioning.ExtraWidth);
                if (!Positioning.Autofit)
                    splitFinalLines = TextWriterTools.GetFinalLines(Text, maxWidth + 1);
                int maxTextHeight = maxHeight - Positioning.ExtraHeight;
                return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, splitFinalLines.Length);
            }
        }

        /// <summary>
        /// Incrementation rate for text in this infobox
        /// </summary>
        public int IncrementationRate
        {
            get
            {
                var (maxWidth, maxHeight, _, borderX, borderY, _, _) = Dimensions;
                var bounded = new BoundedText()
                {
                    Left = borderX + 1,
                    Top = borderY + 1,
                    Line = 0,
                    Height = maxHeight - Positioning.ExtraHeight,
                    Width = maxWidth,
                    Text = Text
                };
                bounded.Render();
                int increment = bounded.IncrementRate;
                return increment;
            }
        }

        /// <summary>
        /// Renders this informational box
        /// </summary>
        /// <param name="currIdx">Current index of text line in the text area</param>
        /// <param name="drawBar">Whether to draw the slider bar for the text area or not</param>
        /// <param name="writeBinding">Whether to write the key bindings in the upper right corner of the box or not</param>
        /// <returns></returns>
        public string Render(int currIdx = 0, bool drawBar = false, bool writeBinding = false)
        {
            int increment = 0;
            return Render(ref increment, currIdx, drawBar, writeBinding);
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
            boxBuffer.Append(ContainerTools.RenderContainer(Elements));

            // Reset colors
            if (Settings.UseColors)
            {
                boxBuffer.Append(
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }

            // Return the rendered elements
            return boxBuffer.ToString();
        }

        internal string Erase()
        {
            var (maxWidth, maxHeight, _, borderX, borderY, _, _) = Dimensions;
            var eraser = new Eraser()
            {
                Left = borderX,
                Top = borderY,
                Width = maxWidth + 2,
                Height = maxHeight + 2,
            };
            return eraser.Render();
        }
    }
}
