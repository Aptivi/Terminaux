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
        /// <param name="settings">Settings to use</param>
        /// <returns>Terminaux's Color</returns>
        public static OurColor FromDrawingColor(DrawingColor drawingColor, ColorSettings? settings = null)
        {
            // Select appropriate settings
            var finalSettings = settings ?? new(ColorTools.GlobalSettings);

            // Check to see if the Color from Drawing is null
            if (drawingColor.IsEmpty)
                return OurColor.Empty;

            // Now, get the Drawing's RGB component and convert that to Terminaux's Color.
            int r = drawingColor.R;
            int g = drawingColor.G;
            int b = drawingColor.B;
            int a = drawingColor.A;
            finalSettings.Opacity = a;
            var color = new OurColor(r, g, b, finalSettings);
            return color;
        }

        /// <summary>
        /// Converts from <see cref="OurColor">Terminaux's Color</see> to <see cref="DrawingColor">System.Drawing's Color</see>
        /// </summary>
        /// <param name="ourColor">Terminaux's Color</param>
        /// <returns>System.Drawing's Color</returns>
        public static DrawingColor ToDrawingColor(OurColor? ourColor)
        {
            // Check to see if the Color from Drawing is null
            if (ourColor is null || ourColor.RGB is null)
                return DrawingColor.Empty;

            // Now, get our color's RGB component and convert that to Drawing's Color.
            int r = ourColor.RGB.originalRed;
            int g = ourColor.RGB.originalGreen;
            int b = ourColor.RGB.originalBlue;
            int a = ourColor.RGB.originalAlpha;
            var color = DrawingColor.FromArgb(a, r, g, b);
            return color;
        }
    }
}
