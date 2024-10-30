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

using Terminaux.Colors;
using Terminaux.Writer.FancyWriters;

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
        private bool customColor = false;

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
            set
            {
                boxColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return BoxColor.RenderBox(
                Left, Top, InteriorWidth, InteriorHeight, Color, customColor);
        }

        /// <summary>
        /// Makes a new instance of the box renderer
        /// </summary>
        public Box()
        { }
    }
}
