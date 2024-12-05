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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Terminaux.Base;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The RGB class instance
    /// </summary>
    [DebuggerDisplay("RGB = {R};{G};{B}")]
    public class RedGreenBlue : BaseColorModel, IEquatable<RedGreenBlue>
    {
        internal int originalRed = 0;
        internal int originalGreen = 0;
        internal int originalBlue = 0;
        internal int originalAlpha = 255;
        internal RedGreenBlue? originalRgb = null;
        internal object[] parameters = [];

        /// <summary>
        /// The red color value [0 -> 255]
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// The green color value [0 -> 255]
        /// </summary>
        public int G { get; private set; }
        /// <summary>
        /// The blue color value [0 -> 255]
        /// </summary>
        public int B { get; private set; }
        /// <summary>
        /// The red color value [0.0 -> 0.1]
        /// </summary>
        public double RNormalized =>
            R / 255d;
        /// <summary>
        /// The green color value [0.0 -> 0.1]
        /// </summary>
        public double GNormalized =>
            G / 255d;
        /// <summary>
        /// The blue color value [0.0 -> 0.1]
        /// </summary>
        public double BNormalized =>
            B / 255d;
        /// <summary>
        /// The original RGB values (always opaque)
        /// </summary>
        public RedGreenBlue OriginalRgb
        {
            get
            {
                if (R == originalRed && G == originalGreen && B == originalBlue && A == 255)
                    throw new TerminauxException("This color is already an original color.");
                originalRgb ??= new(originalRed, originalGreen, originalBlue);
                return originalRgb;
            }
        }
        /// <summary>
        /// The alpha value indicating transparency applied to the <see cref="OriginalRgb"/> value.
        /// </summary>
        public int A =>
            originalAlpha;

        /// <summary>
        /// Gets the three-dimension vector values from RGB color
        /// </summary>
        public Vector3 Vector =>
            new(R, G, B);

        /// <summary>
        /// &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"{R};{G};{B}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((RedGreenBlue)obj);

        /// <inheritdoc/>
        public bool Equals(RedGreenBlue other) =>
            other is not null &&
            R == other.R &&
            G == other.G &&
            B == other.B;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1520100960;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Gets the RGB order code
        /// </summary>
        /// <returns>RGB order code in decimal RRRGGGBBB format</returns>
        public int GetOrderCode() =>
            (B << 16) | (G << 8) | R;

        /// <summary>
        /// Does the string specifier represent a valid RGB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RGB specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            (!checkParts || (checkParts && specifier.Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid RGB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RGB specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Split(';');
            int r = Convert.ToInt32(specifierArray[0]);
            if (r < 0 || r > 255)
                return false;
            int g = Convert.ToInt32(specifierArray[1]);
            if (g < 0 || g > 255)
                return false;
            int b = Convert.ToInt32(specifierArray[2]);
            if (b < 0 || b > 255)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid color specifier \"{specifier}\". Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the RGB values! First, check to see if we need to filter the color for the color-blind
                int r = Convert.ToInt32(specifierArray[0]);
                if (r < 0 || r > 255)
                    throw new TerminauxException($"The red color level is out of range (0 -> 255). {r}");
                int g = Convert.ToInt32(specifierArray[1]);
                if (g < 0 || g > 255)
                    throw new TerminauxException($"The green color level is out of range (0 -> 255). {g}");
                int b = Convert.ToInt32(specifierArray[2]);
                if (b < 0 || b > 255)
                    throw new TerminauxException($"The blue color level is out of range (0 -> 255). {b}");

                // Now, transform
                settings ??= new(ColorTools.GlobalSettings);
                var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

                // Make a new RGB class
                return new(finalRgb.r, finalRgb.g, finalRgb.b);
            }
            else
                throw new TerminauxException($"Invalid RGB color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: rgb:<C>;<M>;<Y>");
        }

        /// <inheritdoc/>
        public static bool operator ==(RedGreenBlue left, RedGreenBlue right) =>
            EqualityComparer<RedGreenBlue>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(RedGreenBlue left, RedGreenBlue right) =>
            !(left == right);

        internal RedGreenBlue(int r, int g, int b)
        {
            R = originalRed = r;
            G = originalGreen = g;
            B = originalBlue = b;
        }

        internal void FinalizeValues(ColorSettings settings)
        {
            // First step: Apply fake "transparency"
            int alpha = settings.Opacity;
            var alphaColor = settings.OpacityColor;
            ApplyTransparency(alpha, alphaColor);
        }

        #region Color finalizers
        private void ApplyTransparency(int alpha, Color alphaColor)
        {
            if (alpha == 255 || alphaColor is null)
                return;

            // In order to apply "transparency," we need to get both the normalized alpha and the inverse
            // normalized alpha levels in order to apply them to the color RGB levels.
            double normalColorLevel = alpha / 255d;
            double alphaColorLevel = (255 - alpha) / 255d;

            // Now, apply the "transparency."
            int normalR = (int)(R * normalColorLevel);
            int normalG = (int)(G * normalColorLevel);
            int normalB = (int)(B * normalColorLevel);
            int alphaR = (int)(alphaColor.RGB.R * alphaColorLevel);
            int alphaG = (int)(alphaColor.RGB.G * alphaColorLevel);
            int alphaB = (int)(alphaColor.RGB.B * alphaColorLevel);
            int resultR = normalR + alphaR;
            int resultG = normalG + alphaG;
            int resultB = normalB + alphaB;

            // Install the resulting values
            R = resultR;
            G = resultG;
            B = resultB;
            originalAlpha = alpha;
        }
        #endregion
    }
}
