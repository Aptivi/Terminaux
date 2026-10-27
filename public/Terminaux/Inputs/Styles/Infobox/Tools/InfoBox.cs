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
using Colorimetry.Transformation;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    /// <summary>
    /// Informational box class
    /// </summary>
    public class InfoBox
    {
        private bool updateDimensions = true;
        private bool updated = true;
        private (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int maxTextHeight, int linesLength) dimensions;
        private string[] splitFinalLines = [];
        private InfoBoxPositioning? cachedPositioning = null;
        private int cachedConsoleWidth = 0;
        private int cachedConsoleHeight = 0;
        private string text = "";

        /// <summary>
        /// Positioning settings
        /// </summary>
        public InfoBoxPositioning Positioning =>
            Settings.Positioning;

        /// <summary>
        /// Text that will be rendered inside the informational box
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                updateDimensions = true;
            }
        }

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
                int linesWidth = ConsoleWrapper.WindowWidth - 6;
                int height = ConsoleWrapper.WindowHeight;
                if (cachedConsoleWidth != linesWidth)
                    updateDimensions = true;
                if (cachedConsoleHeight != height)
                    updateDimensions = true;
                if (cachedPositioning is null)
                    updateDimensions = true;
                if (cachedPositioning?.Width != Positioning.Width)
                    updateDimensions = true;
                if (cachedPositioning?.Height != Positioning.Height)
                    updateDimensions = true;
                if (cachedPositioning?.Left != Positioning.Left)
                    updateDimensions = true;
                if (cachedPositioning?.Top != Positioning.Top)
                    updateDimensions = true;
                if (cachedPositioning?.ExtraHeight != Positioning.ExtraHeight)
                    updateDimensions = true;
                if (cachedPositioning?.ExtraWidth != Positioning.ExtraWidth)
                    updateDimensions = true;

                updated = updateDimensions;
                if (updateDimensions)
                {
                    updateDimensions = false;

                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(Text, linesWidth);
                    var (maxWidth, maxHeight, maxRenderWidth, borderX, borderY) = !Positioning.Autofit ?
                        InfoBoxTools.GetDimensions(Positioning.Width, Positioning.Height, Positioning.Left, Positioning.Top, Positioning.ExtraHeight, Positioning.ExtraWidth) :
                        InfoBoxTools.GetDimensions(splitFinalLines, Positioning.ExtraHeight, Positioning.ExtraWidth);
                    if (!Positioning.Autofit)
                        splitFinalLines = TextWriterTools.GetFinalLines(Text, maxWidth + 1);
                    int maxTextHeight = maxHeight - Positioning.ExtraHeight;

                    dimensions = (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, splitFinalLines.Length);
                    cachedPositioning = new(Positioning);
                    cachedConsoleWidth = linesWidth;
                    cachedConsoleHeight = height;
                }
                return dimensions;
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
        /// <returns>A buffered string you can render to the console</returns>
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
        /// <returns>A buffered string you can render to the console</returns>
        public string Render(ref int increment, int currIdx, bool drawBar, bool writeBinding)
        {
            var (maxWidth, maxHeight, _, borderX, borderY, _, _) = Dimensions;

            // Deal with the lines to actually fit text in the infobox
            string buttons = InfoBoxTools.GetButtons(Settings.BorderSettings);
            int buttonsWidth = writeBinding ? ConsoleChar.EstimateCellWidth(buttons) : -2;
            splitFinalLines = updated ? TextWriterTools.GetFinalLines(Text, maxWidth) : splitFinalLines;

            // Finalize the offsets
            var titleSettings = new TextSettings()
            {
                TitleOffset = new(0, buttonsWidth + 2)
            };

            // Fill the info box with text inside it
            var boxBuffer = new StringBuilder();
            var border = new Border()
            {
                Left = borderX,
                Top = borderY,
                Width = maxWidth,
                Height = maxHeight,
                Color = Settings.ForegroundColor,
                TextColor = Settings.ForegroundColor,
                BackgroundColor = Settings.BackgroundColor,
                Settings = Settings.BorderSettings,
                TextSettings = titleSettings,
            };
            if (!string.IsNullOrEmpty(Settings.Title))
                border.Title = (writeBinding && maxWidth >= buttonsWidth + 2 ? Settings.Title.Truncate(maxWidth - buttonsWidth) : Settings.Title);
            boxBuffer.Append(
                (Settings.UseColors ? ConsoleColoring.RenderSetConsoleColor(Settings.ForegroundColor) : "") +
                (Settings.UseColors ? ConsoleColoring.RenderSetConsoleColor(Settings.BackgroundColor, true) : "") +
                border.Render()
            );

            // Render text inside it
            ConsoleWrapper.CursorVisible = false;
            var bounded = new BoundedText()
            {
                Left = borderX + 1,
                Top = borderY + 1,
                Line = currIdx,
                Height = maxHeight - Positioning.ExtraHeight,
                Width = maxWidth,
                ForegroundColor = Settings.ForegroundColor,
                BackgroundColor = Settings.BackgroundColor,
                Text = Text
            };
            boxBuffer.Append(
                bounded.Render()
            );
            increment = bounded.IncrementRate;

            // Render the vertical bar
            int left = maxWidth + borderX + 1;
            var slider = new Slider((int)((double)currIdx / (splitFinalLines.Length - (maxHeight - Positioning.ExtraHeight)) * splitFinalLines.Length), 0, splitFinalLines.Length)
            {
                Vertical = true,
                Height = maxHeight - Positioning.ExtraHeight - 2,
                SliderActiveForegroundColor = Settings.ForegroundColor,
                SliderForegroundColor = TransformationTools.GetDarkBackground(Settings.ForegroundColor),
                SliderBackgroundColor = Settings.BackgroundColor,
                SliderVerticalActiveTrackChar = Settings.BorderSettings.BorderRightFrameChar,
                SliderVerticalInactiveTrackChar = Settings.BorderSettings.BorderRightFrameChar,
            };
            if (splitFinalLines.Length > maxHeight - Positioning.ExtraHeight && drawBar)
            {
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▲", left, borderY + 1, Settings.ForegroundColor, Settings.BackgroundColor));
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▼", left, borderY + maxHeight - Positioning.ExtraHeight, Settings.ForegroundColor, Settings.BackgroundColor));
                boxBuffer.Append(RendererTools.RenderRenderable(slider, new(left, borderY + 2)));
            }

            // Render a keybinding that points to the help page
            if (writeBinding && maxWidth >= buttonsWidth + 2)
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(buttons, left - buttonsWidth - 1, borderY, Settings.ForegroundColor, Settings.BackgroundColor));

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

        /// <summary>
        /// Erases this informational box
        /// </summary>
        /// <returns>A buffered string you can render to the console</returns>
        public string Erase()
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
