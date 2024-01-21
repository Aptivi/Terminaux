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

using DrawingColor = System.Drawing.Color;
using OurColor = Terminaux.Colors.Color;

namespace Terminaux.Colors.Interop
{
    /// <summary>
    /// Provides you with tools to convert <see cref="DrawingColor">System.Drawing's Color</see> to and from <see cref="OurColor">Terminaux's Color</see>
    /// </summary>
    public static class SystemColorConverter
    {
        /// <summary>
        /// Converts from <see cref="DrawingColor">System.Drawing's Color</see> to <see cref="OurColor">Terminaux's Color</see>
        /// </summary>
        /// <param name="drawingColor">System.Drawing's Color</param>
        /// <returns>Terminaux's Color</returns>
        public static OurColor FromDrawingColor(DrawingColor drawingColor)
        {
            // Check to see if the Color from Drawing is null
            if (drawingColor.IsEmpty)
                return OurColor.Empty;

            // Now, get the Drawing's RGB component and convert that to Terminaux's Color. Terminaux doesn't support
            // Alpha at the moment, so we can't use the Alpha component.
            int r = drawingColor.R;
            int g = drawingColor.G;
            int b = drawingColor.B;
            var color = new OurColor(r, g, b);
            return color;
        }

        /// <summary>
        /// Converts from <see cref="OurColor">Terminaux's Color</see> to <see cref="DrawingColor">System.Drawing's Color</see>
        /// </summary>
        /// <param name="ourColor">Terminaux's Color</param>
        /// <returns>System.Drawing's Color</returns>
        public static DrawingColor ToDrawingColor(OurColor ourColor)
        {
            // Check to see if the Color from Drawing is null
            if (ourColor is null)
                return DrawingColor.Empty;

            // Now, get our color's RGB component and convert that to Drawing's Color. Alpha is assumed to be 100%
            // as Terminaux doesn't support "transparency" yet.
            int r = ourColor.RGB.R;
            int g = ourColor.RGB.G;
            int b = ourColor.RGB.B;
            var color = DrawingColor.FromArgb(r, g, b);
            return color;
        }
    }
}
