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

using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Box renderable
    /// </summary>
    public class Box : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private Color boxColor = ColorTools.CurrentForegroundColor;
        private bool useColors = true;

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
        /// Box color
        /// </summary>
        public Color Color
        {
            get => boxColor;
            set => boxColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Renders a box
        /// </summary>
        /// <returns>Rendered box that will be used by the renderer</returns>
        public string Render()
        {
            return RenderBox(
                Left, Top, InteriorWidth, InteriorHeight, Color, UseColors);
        }

        internal static string RenderBox(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxColor, bool useColor)
        {
            // Fill the box with spaces inside it
            StringBuilder box = new();
            box.Append(
                $"{(useColor ? ColorTools.RenderSetConsoleColor(BoxColor, true) : "")}" +
                CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 2)
            );
            for (int y = 1; y <= InteriorHeight; y++)
                box.Append(
                    new string(' ', InteriorWidth) +
                    CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + y + 2)
                );
            if (useColor)
                box.Append(ColorTools.RenderRevertBackground());
            return box.ToString();
        }

        /// <summary>
        /// Makes a new instance of the box renderer
        /// </summary>
        public Box()
        { }
    }
}
