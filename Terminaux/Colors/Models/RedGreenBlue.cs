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

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The RGB class instance
    /// </summary>
    [DebuggerDisplay("RGB = {R};{G};{B}")]
    public class RedGreenBlue : IEquatable<RedGreenBlue>
    {
        internal int originalRed = 0;
        internal int originalGreen = 0;
        internal int originalBlue = 0;
        internal int originalAlpha = 255;
        internal RedGreenBlue? originalRgb = null;

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
            if (alpha == 255 || alphaColor is null || alphaColor.RGB is null)
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
