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

using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Graphics;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Line renderable
    /// </summary>
    public class Line : IStaticRenderable
    {
        private Coordinate startPos = new();
        private Coordinate endPos = new();
        private bool antiAlias = false;
        private Color lineColor = ColorTools.CurrentForegroundColor;

        /// <summary>
        /// Starting position
        /// </summary>
        public Coordinate StartPos
        {
            get => startPos;
            set => startPos = value;
        }

        /// <summary>
        /// Ending position
        /// </summary>
        public Coordinate EndPos
        {
            get => endPos;
            set => endPos = value;
        }

        /// <summary>
        /// Whether to enable anti-aliasing
        /// </summary>
        public bool AntiAlias
        {
            get => antiAlias;
            set => antiAlias = value;
        }

        /// <summary>
        /// Line color
        /// </summary>
        public Color Color
        {
            get => lineColor;
            set => lineColor = value;
        }

        /// <summary>
        /// Renders a line
        /// </summary>
        /// <returns>Rendered line that will be used by the renderer</returns>
        public string Render()
        {
            if (AntiAlias)
                return GraphicsTools.RenderLineSmooth(StartPos, EndPos, Color);
            else
                return GraphicsTools.RenderLine(StartPos, EndPos, Color);
        }

        /// <summary>
        /// Makes a new instance of the line renderer
        /// </summary>
        public Line()
        { }
    }
}
