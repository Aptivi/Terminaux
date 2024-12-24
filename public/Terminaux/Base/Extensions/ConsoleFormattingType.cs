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

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console formatting type
    /// </summary>
    public enum ConsoleFormattingType
    {
        /// <summary>
        /// [0] Default formatting
        /// </summary>
        Default = 0,
        /// <summary>
        /// [1] Intense (either bold or bright) text
        /// </summary>
        Intense = 1,
        /// <summary>
        /// [2] Reduced intensity (either light or dim) text
        /// </summary>
        Faint = 2,
        /// <summary>
        /// [3] Italic text
        /// </summary>
        Italic = 4,
        /// <summary>
        /// [4] Underlined text
        /// </summary>
        Underline = 8,
        /// <summary>
        /// [5] Blinks the cursor slowly
        /// </summary>
        SlowBlink = 16,
        /// <summary>
        /// [6] Blinks the cursor quickly
        /// </summary>
        FastBlink = 32,
        /// <summary>
        /// [7] Swaps background and foreground colors
        /// </summary>
        Reverse = 64,
        /// <summary>
        /// [8] Conceals text
        /// </summary>
        Conceal = 128,
        /// <summary>
        /// [9] Strikes text through
        /// </summary>
        Strikethrough = 256,
        /// <summary>
        /// [21] Not bold
        /// </summary>
        NotBold = 512,
        /// <summary>
        /// [22] Not intense
        /// </summary>
        NotIntense = 1024,
        /// <summary>
        /// [23] Not italic
        /// </summary>
        NotItalic = 2048,
        /// <summary>
        /// [24] Not underlined
        /// </summary>
        NotUnderlined = 4096,
        /// <summary>
        /// [25] Not blinking
        /// </summary>
        NotBlinking = 8192,
        /// <summary>
        /// [26] Proportional spacing
        /// </summary>
        ProportionalSpacing = 16384,
        /// <summary>
        /// [27] Not reversed
        /// </summary>
        NotReversed = 32768,
        /// <summary>
        /// [28] Reveal
        /// </summary>
        Reveal = 65536,
        /// <summary>
        /// [29] Not struckthrough
        /// </summary>
        NotStruckthrough = 131072,
        /// <summary>
        /// [50] No proportional spacing
        /// </summary>
        NoProportionalSpacing = 262144,
        /// <summary>
        /// [51] Framed
        /// </summary>
        Framed = 524288,
        /// <summary>
        /// [52] Encircled
        /// </summary>
        Encircled = 1048576,
        /// <summary>
        /// [53] Overlined
        /// </summary>
        Overlined = 2097152,
        /// <summary>
        /// [54] Not framed/encircled
        /// </summary>
        NotFramedEncircled = 4194304,
        /// <summary>
        /// [55] Not overlined
        /// </summary>
        NotOverlined = 8388608,
    }
}
