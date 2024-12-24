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
using Terminaux.Base.TermInfo;
using Terminaux.Base.TermInfo.Parsing;
using Terminaux.Sequences.Builder;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Text formatting tools for the console
    /// </summary>
    public static class ConsoleFormatting
    {
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

        private static Dictionary<ConsoleFormattingType, (int, TermInfoValueDesc<string?>?)> TypeSequenceNumbers => new()
        {
            { ConsoleFormattingType.Default,                (0, TermInfoDesc.Current.ExitAttributeMode) },
            { ConsoleFormattingType.Intense,                (1, TermInfoDesc.Current.EnterBoldMode) },
            { ConsoleFormattingType.Faint,                  (2, null) },
            { ConsoleFormattingType.Italic,                 (3, TermInfoDesc.Current.EnterItalicsMode) },
            { ConsoleFormattingType.Underline,              (4, TermInfoDesc.Current.EnterUnderlineMode) },
            { ConsoleFormattingType.SlowBlink,              (5, TermInfoDesc.Current.EnterBlinkMode) },
            { ConsoleFormattingType.FastBlink,              (6, TermInfoDesc.Current.EnterBlinkMode) },
            { ConsoleFormattingType.Reverse,                (7, TermInfoDesc.Current.EnterReverseMode) },
            { ConsoleFormattingType.Conceal,                (8, null) },
            { ConsoleFormattingType.Strikethrough,          (9, null) },
            { ConsoleFormattingType.NotBold,                (21, null) },
            { ConsoleFormattingType.NotIntense,             (22, null) },
            { ConsoleFormattingType.NotItalic,              (23, TermInfoDesc.Current.ExitItalicsMode) },
            { ConsoleFormattingType.NotUnderlined,          (24, TermInfoDesc.Current.ExitUnderlineMode) },
            { ConsoleFormattingType.NotBlinking,            (25, null) },
            { ConsoleFormattingType.ProportionalSpacing,    (26, null) },
            { ConsoleFormattingType.NotReversed,            (27, null) },
            { ConsoleFormattingType.Reveal,                 (28, null) },
            { ConsoleFormattingType.NotStruckthrough,       (29, null) },
            { ConsoleFormattingType.NoProportionalSpacing,  (50, null) },
            { ConsoleFormattingType.Framed,                 (51, null) },
            { ConsoleFormattingType.Encircled,              (52, null) },
            { ConsoleFormattingType.Overlined,              (53, null) },
            { ConsoleFormattingType.NotFramedEncircled,     (54, null) },
            { ConsoleFormattingType.NotOverlined,           (55, null) },
        };

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
                    var sequenceTuple = TypeSequenceNumbers[type];

                    // First, check for TermInfo value
                    string finalValue = sequenceTuple.Item2 is not null ? sequenceTuple.Item2.ProcessSequence() ?? "" : "";
                    if (string.IsNullOrWhiteSpace(finalValue))
                        finalValue = VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, sequenceTuple.Item1);
                    builder.Append(finalValue);
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
