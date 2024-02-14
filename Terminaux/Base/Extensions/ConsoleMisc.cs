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
using Terminaux.Sequences;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Miscellaneous extensions for the console
    /// </summary>
    public static class ConsoleMisc
    {
        /// <summary>
        /// Sets the console title
        /// </summary>
        /// <param name="Text">The text to be set</param>
        public static void SetTitle(string Text)
        {
            char BellChar = Convert.ToChar(7);
            char EscapeChar = Convert.ToChar(27);
            string Sequence = $"{EscapeChar}]0;{Text}{BellChar}";
            TextWriterRaw.WriteRaw(Sequence);
        }

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset) =>
            (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * ((ConsoleWrapper.WindowWidth - WidthOffset) * 0.01d));

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="TargetWidth">The target width</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeatTargeted(int CurrentNumber, int MaximumNumber, int TargetWidth) =>
            (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * (TargetWidth * 0.01d));

        /// <summary>
        /// Filters the VT sequences that matches the regex
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text)
        {
            // Filter all sequences
            Text = VtSequenceTools.FilterVTSequences(Text);
            return Text;
        }
    }
}
