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
using Colorimetry;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Box renderable
    /// </summary>
    public class Box : GraphicalCyclicWriter
    {
        private Color boxColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private bool useColors = true;

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
        public override string Render()
        {
            // Fill the box with spaces inside it
            StringBuilder box = new();
            box.Append(
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(Color, true) : "")}" +
                CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)
            );
            ConsoleLogger.Debug("Box width: {0}, height: {1}", Width, Height);
            for (int y = 1; y <= Height; y++)
                box.Append(
                    new string(' ', Width) +
                    CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + y + 1)
                );
            if (UseColors)
                box.Append(ConsoleColoring.RenderRevertBackground());
            return box.ToString();
        }

        /// <summary>
        /// Makes a new instance of the box renderer
        /// </summary>
        public Box()
        { }
    }
}
