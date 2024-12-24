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

namespace Terminaux.Colors.Models.Parsing
{
    /// <summary>
    /// Illuminant type
    /// </summary>
    public enum IlluminantType
    {
        /// <summary>
        /// 2856 Kelvin, incandescent / tungsten
        /// </summary>
        A,
        /// <summary>
        /// 4874 Kelvin, obsolete, direct sunlight at noon
        /// </summary>
        B,
        /// <summary>
        /// 6774 Kelvin, obsolete, average / North sky daylight, NTSC 1953, PAL-M
        /// </summary>
        C,
        /// <summary>
        /// 5003 Kelvin, horizon light, ICC profile PCS
        /// </summary>
        D50,
        /// <summary>
        /// 5503 Kelvin, mid-morning / mid-afternoon daylight
        /// </summary>
        D55,
        /// <summary>
        /// [Default] 6504 Kelvin, noon daylight: television, sRGB color space
        /// </summary>
        D65,
        /// <summary>
        /// 7504 Kelvin, north sky daylight
        /// </summary>
        D75,
        /// <summary>
        /// 5454 Kelvin, equal energy
        /// </summary>
        E,
        /// <summary>
        /// 6430 Kelvin, daylight fluorescent
        /// </summary>
        F1,
        /// <summary>
        /// 4230 Kelvin, cool white fluorescent
        /// </summary>
        F2,
        /// <summary>
        /// 3450 Kelvin, white fluorescent
        /// </summary>
        F3,
        /// <summary>
        /// 2940 Kelvin, warm white fluorescent
        /// </summary>
        F4,
        /// <summary>
        /// 6350 Kelvin, daylight fluorescent
        /// </summary>
        F5,
        /// <summary>
        /// 4150 Kelvin, light white fluorescent
        /// </summary>
        F6,
        /// <summary>
        /// 6500 Kelvin, D65 simulator, daylight simulator
        /// </summary>
        F7,
        /// <summary>
        /// 5000 Kelvin, D50 simulator, Sylvania F40 Design 50
        /// </summary>
        F8,
        /// <summary>
        /// 4150 Kelvin, cool white deluxe fluorescent
        /// </summary>
        F9,
        /// <summary>
        /// 5000 Kelvin, Philips TL85, Ultralume 50
        /// </summary>
        F10,
        /// <summary>
        /// 4000 Kelvin, Philips TL84, Ultralume 40
        /// </summary>
        F11,
        /// <summary>
        /// 3000 Kelvin, Philips TL83, Ultralume 30
        /// </summary>
        F12,
    }
}
