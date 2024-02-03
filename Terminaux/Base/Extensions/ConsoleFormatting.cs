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
using System.Text;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Textify.Sequences.Builder;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Text formatting tools for the console
    /// </summary>
    public static class ConsoleFormatting
    {
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
            if (types.HasFlag(ConsoleFormattingType.Default))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 0));
            if (types.HasFlag(ConsoleFormattingType.Intense))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 1));
            if (types.HasFlag(ConsoleFormattingType.Faint))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 2));
            if (types.HasFlag(ConsoleFormattingType.Italic))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 3));
            if (types.HasFlag(ConsoleFormattingType.Underline))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 4));
            if (types.HasFlag(ConsoleFormattingType.SlowBlink))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 5));
            if (types.HasFlag(ConsoleFormattingType.FastBlink))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 6));
            if (types.HasFlag(ConsoleFormattingType.Reverse))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 7));
            if (types.HasFlag(ConsoleFormattingType.Conceal))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 8));
            if (types.HasFlag(ConsoleFormattingType.Strikethrough))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 9));
            if (types.HasFlag(ConsoleFormattingType.NotBold))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 21));
            if (types.HasFlag(ConsoleFormattingType.NotIntense))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 22));
            if (types.HasFlag(ConsoleFormattingType.NotItalic))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 23));
            if (types.HasFlag(ConsoleFormattingType.NotUnderlined))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 24));
            if (types.HasFlag(ConsoleFormattingType.NotBlinking))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 25));
            if (types.HasFlag(ConsoleFormattingType.ProportionalSpacing))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 26));
            if (types.HasFlag(ConsoleFormattingType.NotReversed))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 27));
            if (types.HasFlag(ConsoleFormattingType.Reveal))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 28));
            if (types.HasFlag(ConsoleFormattingType.NotStruckthrough))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 29));
            if (types.HasFlag(ConsoleFormattingType.NoProportionalSpacing))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 50));
            if (types.HasFlag(ConsoleFormattingType.Framed))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 51));
            if (types.HasFlag(ConsoleFormattingType.Encircled))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 52));
            if (types.HasFlag(ConsoleFormattingType.Overlined))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 53));
            if (types.HasFlag(ConsoleFormattingType.NotFramedEncircled))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 54));
            if (types.HasFlag(ConsoleFormattingType.NotOverlined))
                builder.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCharacterAttributes, 55));

            // Return the final result
            return builder.ToString();
        }

        /// <summary>
        /// Sets text formatting for future plain writes
        /// </summary>
        /// <param name="formattings">All text formatting to use</param>
        public static void SetFormatting(ConsoleFormattingType formattings)
        {
            TextWriterRaw.WritePlain(GetFormattingSequences(formattings), false);
            formatting = formattings;
        }

        /// <summary>
        /// Resets console text formatting
        /// </summary>
        public static void ResetFormatting()
        {
            TextWriterRaw.WritePlain(GetFormattingSequences(ConsoleFormattingType.Default), false);
            formatting = ConsoleFormattingType.Default;
        }
    }
}
