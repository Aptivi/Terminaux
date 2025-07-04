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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;
using Textify.General;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The Luma, In-phase, and Quadrature (YIQ, NTSC 1953) model
    /// </summary>
    [DebuggerDisplay("YIQ = {Luma};{InPhase};{Quadrature}")]
    public class LumaInPhaseQuadrature : BaseColorModel, IEquatable<LumaInPhaseQuadrature>
    {
        /// <summary>
        /// The Y component, known as luma information [0 -> 255]
        /// </summary>
        public int Luma { get; private set; }
        /// <summary>
        /// The I component, known as chrominance information [0 -> 255]
        /// </summary>
        public int InPhase { get; private set; }
        /// <summary>
        /// The Q component, known as chrominance information [0 -> 255]
        /// </summary>
        public int Quadrature { get; private set; }
        /// <summary>
        /// The Y component, known as luma information [0.0 -> 1.0]
        /// </summary>
        public double LumaNormalized =>
            Luma / 255d;
        /// <summary>
        /// The I component, known as chrominance information [-0.5957 -> 0.5957]
        /// </summary>
        public double InPhaseNormalized =>
            (InPhase - 128) / 255d;
        /// <summary>
        /// The Q component, known as chrominance information [-0.5226 -> 0.5226]
        /// </summary>
        public double QuadratureNormalized =>
            (Quadrature - 128) / 255d;

        /// <summary>
        /// yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;
        /// </summary>
        public override string ToString() =>
            $"yiq:{Luma};{InPhase};{Quadrature}";

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="LumaInPhaseQuadrature"/>
        /// </summary>
        /// <param name="specifier">Specifier of YIQ</param>
        /// <returns>An instance of <see cref="LumaInPhaseQuadrature"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDYIQSPECIFIER").FormatString(specifier) + ": yiq:<Y>;<I>;<Q>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the YIQ whole values! First, check to see if we need to filter the color for the color-blind
                int y = Convert.ToInt32(specifierArray[0]);
                if (y < 0 || y > 255)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEYUVYLEVEL") + $" {y}");
                int i = Convert.ToInt32(specifierArray[1]);
                if (i < 0 || i > 255)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEYIQILEVEL") + $" {i}");
                int q = Convert.ToInt32(specifierArray[2]);
                if (q < 0 || q > 255)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEYIQQLEVEL") + $" {q}");

                // First, we need to convert from YIQ to RGB
                var yiq = new LumaInPhaseQuadrature(y, i, q);
                return yiq;
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDYIQSPECIFIEREXCEED").FormatString(specifier) + ": yiq:<Y>;<I>;<Q>");
        }

        /// <summary>
        /// Does the string specifier represent a valid YIQ specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YIQ specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("yiq:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid YIQ specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YIQ specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int y = Convert.ToInt32(specifierArray[0]);
            if (y < 0 || y > 255)
                return false;
            int i = Convert.ToInt32(specifierArray[1]);
            if (i < 0 || i > 255)
                return false;
            int q = Convert.ToInt32(specifierArray[2]);
            if (q < 0 || q > 255)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="LumaInPhaseQuadrature"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var yiq = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(yiq);
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
            Equals((LumaInPhaseQuadrature)obj);

        /// <inheritdoc/>
        public bool Equals(LumaInPhaseQuadrature other) =>
            other is not null &&
            Luma == other.Luma &&
            InPhase == other.InPhase &&
            Quadrature == other.Quadrature;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1746433734;
            hashCode = hashCode * -1521134295 + Luma.GetHashCode();
            hashCode = hashCode * -1521134295 + InPhase.GetHashCode();
            hashCode = hashCode * -1521134295 + Quadrature.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(LumaInPhaseQuadrature left, LumaInPhaseQuadrature right) =>
            EqualityComparer<LumaInPhaseQuadrature>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(LumaInPhaseQuadrature left, LumaInPhaseQuadrature right) =>
            !(left == right);

        internal LumaInPhaseQuadrature(int luma, int inphase, int quadrature)
        {
            Luma = luma;
            InPhase = inphase;
            Quadrature = quadrature;
        }
    }
}
