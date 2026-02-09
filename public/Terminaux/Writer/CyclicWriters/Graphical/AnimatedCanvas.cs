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

using Terminaux.Base;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Animated canvas renderable
    /// </summary>
    public class AnimatedCanvas : GraphicalCyclicWriter
    {
        private int frame = 0;
        private Color canvasColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Canvas color
        /// </summary>
        public Color Color
        {
            get => canvasColor;
            set => canvasColor = value;
        }

        /// <summary>
        /// Pixel representations for each frame
        /// </summary>
        public CellOptions[][] Frames { get; set; } = [];

        /// <summary>
        /// Whether this canvas is double-width or single-width
        /// </summary>
        public bool DoubleWidth { get; set; } = true;

        /// <summary>
        /// Whether this canvas is transparent
        /// </summary>
        public bool Transparent { get; set; } = false;

        /// <summary>
        /// Renders an animated canvas
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            if (Frames.Length == 0)
                return "";

            // Advance a frame while getting the current frame for the canvas
            var pixelOptionArray = Frames[frame];
            ConsoleLogger.Debug("Rendering animated canvas frame {0} / {1}", frame, Frames.Length);
            frame++;
            if (frame == Frames.Length)
                frame = 0;

            // Create a new canvas to draw the resultant frame
            var canvas = new Canvas()
            {
                Color = Color,
                DoubleWidth = DoubleWidth,
                Transparent = Transparent,
                Pixels = pixelOptionArray,
                Left = Left,
                Top = Top,
                Width = Width,
                Height = Height,
            };
            return canvas.Render();
        }

        /// <summary>
        /// Makes a new instance of the animated canvas renderer
        /// </summary>
        public AnimatedCanvas()
        { }
    }
}
