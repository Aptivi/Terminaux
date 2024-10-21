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

namespace Terminaux.Writer.FancyWriters.Tools
{
    /// <summary>
    /// Pre-defined border list class
    /// </summary>
    public static class PredefinedBorders
    {
        /// <summary>
        /// Default border settings (a curved rectangle)
        /// </summary>
        public static BorderSettings Default =>
            new();

        /// <summary>
        /// A rectangle with two lines as edges and corners
        /// </summary>
        public static BorderSettings RectangleTwoLines =>
            new()
            {
                BorderUpperLeftCornerChar = '╔',
                BorderUpperRightCornerChar = '╗',
                BorderLowerLeftCornerChar = '╚',
                BorderLowerRightCornerChar = '╝',
                BorderUpperFrameChar = '═',
                BorderLowerFrameChar = '═',
                BorderLeftFrameChar = '║',
                BorderRightFrameChar = '║',
                BorderLeftHorizontalIntersectionChar = '╠',
                BorderRightHorizontalIntersectionChar = '╣',
                BorderHorizontalIntersectionChar = '═',
                BorderTopVerticalIntersectionChar = '╦',
                BorderBottomVerticalIntersectionChar = '╩',
                BorderVerticalIntersectionChar = '║',
                BorderWholeIntersectionChar = '╬',
            };

        /// <summary>
        /// A rectangle with a thin line as edges and corners
        /// </summary>
        public static BorderSettings RectangleThin =>
            new()
            {
                BorderUpperLeftCornerChar = '┌',
                BorderUpperRightCornerChar = '┐',
                BorderLowerLeftCornerChar = '└',
                BorderLowerRightCornerChar = '┘',
                BorderUpperFrameChar = '─',
                BorderLowerFrameChar = '─',
                BorderLeftFrameChar = '│',
                BorderRightFrameChar = '│',
                BorderLeftHorizontalIntersectionChar = '├',
                BorderRightHorizontalIntersectionChar = '┤',
                BorderHorizontalIntersectionChar = '─',
                BorderTopVerticalIntersectionChar = '┬',
                BorderBottomVerticalIntersectionChar = '┴',
                BorderVerticalIntersectionChar = '│',
                BorderWholeIntersectionChar = '┼',
            };

        /// <summary>
        /// A rectangle with a thick line as edges and corners
        /// </summary>
        public static BorderSettings RectangleThick =>
            new()
            {
                BorderUpperLeftCornerChar = '┏',
                BorderUpperRightCornerChar = '┓',
                BorderLowerLeftCornerChar = '┗',
                BorderLowerRightCornerChar = '┛',
                BorderUpperFrameChar = '━',
                BorderLowerFrameChar = '━',
                BorderLeftFrameChar = '┃',
                BorderRightFrameChar = '┃',
                BorderLeftHorizontalIntersectionChar = '┣',
                BorderRightHorizontalIntersectionChar = '┫',
                BorderHorizontalIntersectionChar = '━',
                BorderTopVerticalIntersectionChar = '┳',
                BorderBottomVerticalIntersectionChar = '┻',
                BorderVerticalIntersectionChar = '┃',
                BorderWholeIntersectionChar = '╋',
            };

        /// <summary>
        /// A rectangle using dashes, pipe, and plus signs as edges and borders
        /// </summary>
        public static BorderSettings RectangleSimple =>
            new()
            {
                BorderUpperLeftCornerChar = '+',
                BorderUpperRightCornerChar = '+',
                BorderLowerLeftCornerChar = '+',
                BorderLowerRightCornerChar = '+',
                BorderUpperFrameChar = '-',
                BorderLowerFrameChar = '-',
                BorderLeftFrameChar = '|',
                BorderRightFrameChar = '|',
                BorderLeftHorizontalIntersectionChar = '+',
                BorderRightHorizontalIntersectionChar = '+',
                BorderHorizontalIntersectionChar = '-',
                BorderTopVerticalIntersectionChar = '+',
                BorderBottomVerticalIntersectionChar = '+',
                BorderVerticalIntersectionChar = '|',
                BorderWholeIntersectionChar = '+',
            };
    }
}
