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

using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Animated canvas renderable
    /// </summary>
    public class AnimatedCanvas : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private int frame = 0;
        private Color canvasColor = ColorTools.CurrentBackgroundColor;

        /// <summary>
        /// Left position
        /// </summary>
        public int Left
        {
            get => left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set => top = value;
        }

        /// <summary>
        /// Interior width
        /// </summary>
        public int InteriorWidth
        {
            get => interiorWidth;
            set => interiorWidth = value;
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set => interiorHeight = value;
        }

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
        public string Render()
        {
            if (Frames.Length == 0)
                return "";

            // Advance a frame while getting the current frame for the canvas
            var pixelOptionArray = Frames[frame];
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
                InteriorWidth = InteriorWidth,
                InteriorHeight = InteriorHeight,
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
