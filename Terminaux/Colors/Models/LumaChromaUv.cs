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
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The Luma chroma (YUV, PAL, SDTV with BT.470) model
    /// </summary>
    [DebuggerDisplay("YUV = {Luma};{ChromaU};{ChromaV}")]
    public class LumaChromaUv : BaseColorModel, IEquatable<LumaChromaUv>
    {
        /// <summary>
        /// The Y component, known as luma information [0 -> 255]
        /// </summary>
        public int Luma { get; private set; }
        /// <summary>
        /// The U component, known as chroma [0 -> 255]
        /// </summary>
        public int ChromaU { get; private set; }
        /// <summary>
        /// The V component, known as chroma [0 -> 255]
        /// </summary>
        public int ChromaV { get; private set; }
        /// <summary>
        /// The Y component, known as luma information [0.0 -> 1.0]
        /// </summary>
        public double LumaNormalized =>
            Luma / 255d;
        /// <summary>
        /// The U component, known as chrominance information [-0.5019 -> 0.5019]
        /// </summary>
        public double ChromaUNormalized =>
            (ChromaU - 128) / 255d;
        /// <summary>
        /// The V component, known as chrominance information [-0.5019 -> 0.5019]
        /// </summary>
        public double ChromaVNormalized =>
            (ChromaV - 128) / 255d;

        /// <summary>
        /// yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;
        /// </summary>
        public override string ToString() =>
            $"yuv:{Luma};{ChromaU};{ChromaV}";

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="LumaChromaUv"/>
        /// </summary>
        /// <param name="specifier">Specifier of YUV</param>
        /// <returns>An instance of <see cref="LumaChromaUv"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid YUV color specifier \"{specifier}\". Ensure that it's on the correct format: yuv:<Y>;<I>;<Q>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the YUV whole values! First, check to see if we need to filter the color for the color-blind
                int y = Convert.ToInt32(specifierArray[0]);
                if (y < 0 || y > 255)
                    throw new TerminauxException($"The luma level is out of range (0 -> 255). {y}");
                int u = Convert.ToInt32(specifierArray[1]);
                if (u < 0 || u > 255)
                    throw new TerminauxException($"The chroma (U) level is out of range (0 -> 255). {u}");
                int v = Convert.ToInt32(specifierArray[2]);
                if (v < 0 || v > 255)
                    throw new TerminauxException($"The chroma (V) level is out of range (0 -> 255). {v}");

                // First, we need to convert from YUV to RGB
                var yuv = new LumaChromaUv(y, u, v);
                return yuv;
            }
            else
                throw new TerminauxException($"Invalid YUV color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: yuv:<Y>;<I>;<Q>");
        }

        /// <summary>
        /// Does the string specifier represent a valid YUV specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YUV specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("yuv:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid YUV specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YUV specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int y = Convert.ToInt32(specifierArray[0]);
            if (y < 0 || y > 255)
                return false;
            int u = Convert.ToInt32(specifierArray[1]);
            if (u < 0 || u > 255)
                return false;
            int v = Convert.ToInt32(specifierArray[2]);
            if (v < 0 || v > 255)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="LumaChromaUv"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var yuv = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(yuv);
            int r = rgb.R;
            int g = rgb.G;
            int b = rgb.B;

            // Now, transform
            settings ??= new(ColorTools.GlobalSettings);
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((LumaChromaUv)obj);

        /// <inheritdoc/>
        public bool Equals(LumaChromaUv other) =>
            other is not null &&
            Luma == other.Luma &&
            ChromaU == other.ChromaU &&
            ChromaV == other.ChromaV;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -485171435;
            hashCode = hashCode * -1521134295 + Luma.GetHashCode();
            hashCode = hashCode * -1521134295 + ChromaU.GetHashCode();
            hashCode = hashCode * -1521134295 + ChromaV.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(LumaChromaUv left, LumaChromaUv right) =>
            EqualityComparer<LumaChromaUv>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(LumaChromaUv left, LumaChromaUv right) =>
            !(left == right);

        internal LumaChromaUv(int luma, int chromaU, int chromaV)
        {
            Luma = luma;
            ChromaU = chromaU;
            ChromaV = chromaV;
        }
    }
}
