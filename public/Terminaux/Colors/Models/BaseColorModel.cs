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

using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Models.Parsing;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// Base color model
    /// </summary>
    public abstract class BaseColorModel
    {
        /// <summary>
        /// Does the string specifier represent a valid model-agnostic specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid model-agnostic specifier</param>
        /// <param name="checkParts">Whether to check parts or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierValid(string specifier, bool checkParts = false)
        {
            ConsoleLogger.Debug($"{specifier}, {checkParts}");
            return specifier.Contains(";");
        }

        /// <summary>
        /// Does the string specifier represent a valid RGB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RGB specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierAndValueValid(string specifier)
        {
            if (ParsingTools.IsSpecifierConsoleColors(specifier))
                return true;
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(specifier))
                return true;
            if (!IsSpecifierValid(specifier))
                return false;

            return
                CyanMagentaYellowKey.IsSpecifierAndValueValid(specifier) ||
                CyanMagentaYellow.IsSpecifierAndValueValid(specifier) ||
                HueSaturationLightness.IsSpecifierAndValueValid(specifier) ||
                HueSaturationValue.IsSpecifierAndValueValid(specifier) ||
                HueWhiteBlack.IsSpecifierAndValueValid(specifier) ||
                RedGreenBlue.IsSpecifierAndValueValid(specifier) ||
                RedYellowBlue.IsSpecifierAndValueValid(specifier) ||
                LumaInPhaseQuadrature.IsSpecifierAndValueValid(specifier) ||
                LumaChromaUv.IsSpecifierAndValueValid(specifier) ||
                Xyz.IsSpecifierAndValueValid(specifier) ||
                Yxy.IsSpecifierAndValueValid(specifier) ||
                HunterLab.IsSpecifierAndValueValid(specifier) ||
                CieLab.IsSpecifierAndValueValid(specifier) ||
                CieLch.IsSpecifierAndValueValid(specifier) ||
                CieLuv.IsSpecifierAndValueValid(specifier);
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            // Necessary variables
            bool usesColorId = ParsingTools.IsSpecifierConsoleColors(specifier);

            // Get the RGB
            var finalRgb =
                // Color models
                CyanMagentaYellowKey.IsSpecifierValid(specifier) ? CyanMagentaYellowKey.ParseSpecifierToRgb(specifier, settings) :
                CyanMagentaYellow.IsSpecifierValid(specifier) ? CyanMagentaYellow.ParseSpecifierToRgb(specifier, settings) :
                HueSaturationLightness.IsSpecifierValid(specifier) ? HueSaturationLightness.ParseSpecifierToRgb(specifier, settings) :
                HueSaturationValue.IsSpecifierValid(specifier) ? HueSaturationValue.ParseSpecifierToRgb(specifier, settings) :
                HueWhiteBlack.IsSpecifierValid(specifier) ? HueWhiteBlack.ParseSpecifierToRgb(specifier, settings) :
                RedYellowBlue.IsSpecifierValid(specifier) ? RedYellowBlue.ParseSpecifierToRgb(specifier, settings) :
                LumaInPhaseQuadrature.IsSpecifierValid(specifier) ? LumaInPhaseQuadrature.ParseSpecifierToRgb(specifier, settings) :
                LumaChromaUv.IsSpecifierValid(specifier) ? LumaChromaUv.ParseSpecifierToRgb(specifier, settings) :
                Xyz.IsSpecifierValid(specifier) ? Xyz.ParseSpecifierToRgb(specifier, settings) :
                Yxy.IsSpecifierValid(specifier) ? Yxy.ParseSpecifierToRgb(specifier, settings) :
                HunterLab.IsSpecifierValid(specifier) ? HunterLab.ParseSpecifierToRgb(specifier, settings) :
                CieLab.IsSpecifierValid(specifier) ? CieLab.ParseSpecifierToRgb(specifier, settings) :
                CieLch.IsSpecifierValid(specifier) ? CieLch.ParseSpecifierToRgb(specifier, settings) :
                CieLuv.IsSpecifierValid(specifier) ? CieLuv.ParseSpecifierToRgb(specifier, settings) :

                // Colors and hash
                usesColorId ? ParsingTools.ParseSpecifierRgbName(specifier, settings) :
                ParsingTools.IsSpecifierValidRgbHash(specifier) ? ParsingTools.ParseSpecifierRgbHash(specifier, settings) :

                // Fallback
                RedGreenBlue.ParseSpecifierToRgb(specifier, settings);

            // Finalize the RGB values according to the settings as needed.
            if (settings is not null)
                finalRgb.FinalizeValues(settings);
            return finalRgb;
        }
    }
}
