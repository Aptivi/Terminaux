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
using System.Text;
using Terminaux.Base.Checks;
using Terminaux.Sequences.Builder;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Text formatting tools for the console
    /// </summary>
    public static class ConsoleFormatting
    {
        private static readonly Dictionary<ConsoleFormattingType, int> typeSequenceNumbers = new()
        {
            { ConsoleFormattingType.Default,                0 },
            { ConsoleFormattingType.Intense,                1 },
            { ConsoleFormattingType.Faint,                  2 },
            { ConsoleFormattingType.Italic,                 3 },
            { ConsoleFormattingType.Underline,              4 },
            { ConsoleFormattingType.SlowBlink,              5 },
            { ConsoleFormattingType.FastBlink,              6 },
            { ConsoleFormattingType.Reverse,                7 },
            { ConsoleFormattingType.Conceal,                8 },
            { ConsoleFormattingType.Strikethrough,          9 },
            { ConsoleFormattingType.NotBold,                21 },
            { ConsoleFormattingType.NotIntense,             22 },
            { ConsoleFormattingType.NotItalic,              23 },
            { ConsoleFormattingType.NotUnderlined,          24 },
            { ConsoleFormattingType.NotBlinking,            25 },
            { ConsoleFormattingType.ProportionalSpacing,    26 },
            { ConsoleFormattingType.NotReversed,            27 },
            { ConsoleFormattingType.Reveal,                 28 },
            { ConsoleFormattingType.NotStruckthrough,       29 },
            { ConsoleFormattingType.NoProportionalSpacing,  50 },
            { ConsoleFormattingType.Framed,                 51 },
            { ConsoleFormattingType.Encircled,              52 },
            { ConsoleFormattingType.Overlined,              53 },
            { ConsoleFormattingType.NotFramedEncircled,     54 },
            { ConsoleFormattingType.NotOverlined,           55 },
        };
        private static readonly ConsoleFormattingType[] formattings = (ConsoleFormattingType[])Enum.GetValues(typeof(ConsoleFormattingType));
        private static ConsoleFormattingType formatting = ConsoleFormattingType.Default;

        /// <summary>
        /// Current text formatting
        /// </summary>
        public static ConsoleFormattingType CurrentFormatting =>
            formatting;

        /// <summary>
        /// Has the text been formatted?
        /// </summary>
        public static bool TextFormatted =>
            formatting != ConsoleFormattingType.Default;

        /// <summary>
        /// Gets the formatting sequences for the selected formatting types
        /// </summary>
        /// <param name="types">Selected formatting types</param>
        /// <returns>A string containing necessary VT sequences for all selected text formatting</returns>
        public static string GetFormattingSequences(ConsoleFormattingType types)
        {
            var builder = new StringBuilder();

            // Work on all the possible types
            foreach (var type in formattings)
            {
                if (types.HasFlag(type))
                {
                    var sequence = typeSequenceNumbers[type];
                    builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, sequence));
                }
            }

            // Return the final result
            return builder.ToString();
        }

        /// <summary>
        /// Sets text formatting for future plain writes
        /// </summary>
        /// <param name="formattings">All text formatting to use</param>
        public static void SetFormatting(ConsoleFormattingType formattings)
        {
            TextWriterRaw.WriteRaw(GetFormattingSequences(formattings));
            formatting = formattings;
        }

        /// <summary>
        /// Resets console text formatting
        /// </summary>
        public static void ResetFormatting()
        {
            TextWriterRaw.WriteRaw(GetFormattingSequences(ConsoleFormattingType.Default));
            formatting = ConsoleFormattingType.Default;
        }

        static ConsoleFormatting()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
