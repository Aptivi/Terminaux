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
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Canvas renderable
    /// </summary>
    public class Canvas : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
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
        /// Pixel representations
        /// </summary>
        public CellOptions[] Pixels { get; set; } = [];

        /// <summary>
        /// Whether this canvas is double-width or single-width
        /// </summary>
        public bool DoubleWidth { get; set; } = true;

        /// <summary>
        /// Whether this canvas is transparent
        /// </summary>
        public bool Transparent { get; set; } = false;

        /// <summary>
        /// Renders a canvas
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return RenderCanvas(
                Pixels, Left, Top, InteriorWidth, InteriorHeight, Color, DoubleWidth, Transparent);
        }

        internal static string RenderCanvas(CellOptions[] pixels, int Left, int Top, int InteriorWidth, int InteriorHeight, Color CanvasColor, bool doubleWidth = true, bool transparent = false)
        {
            // Fill the canvas with spaces inside it
            StringBuilder canvas = new();
            if (!transparent)
            {
                canvas.Append(
                    Box.RenderBox(Left, Top, doubleWidth ? InteriorWidth * 2 : InteriorWidth, InteriorHeight, CanvasColor, true)
                );
            }
            foreach (var pixel in pixels)
            {
                // Check the pixel locations
                int left = pixel.ColumnIndex;
                int top = pixel.RowIndex;
                if (left < 0 || left > InteriorWidth)
                    continue;
                if (top < 0 || top > InteriorHeight)
                    continue;

                // Print this individual pixel
                canvas.Append(
                    TextWriterWhereColor.RenderWhereColorBack(doubleWidth ? "  " : " ", Left + (left * (doubleWidth ? 2 : 1)), Top + top, ColorTools.CurrentForegroundColor, pixel.CellColor)
                );
            }
            canvas.Append(ColorTools.RenderRevertBackground());
            return canvas.ToString();
        }

        /// <summary>
        /// Makes a new instance of the canvas renderer
        /// </summary>
        public Canvas()
        { }
    }
}
