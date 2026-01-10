//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using Terminaux.Base.TermInfo;
using Terminaux.Sequences.Builder;

namespace Terminaux.Colors
{
    /// <summary>
    /// Extension methods for <see cref="Color"/>
    /// </summary>
    public static class ColorExtensions
    {
        // TODO: Move the whole color system on Terminaux 8.2 to a separate library
        /// <summary>
        /// Parsable VT sequence (Foreground)
        /// </summary>
        public static string VTSequenceForeground(this Color color) =>
            color.IsOriginal ? color.VTSequenceForegroundOriginal() : color.VTSequenceForegroundTrueColor();

        /// <summary>
        /// Parsable VT sequence (Foreground)
        /// </summary>
        public static string VTSequenceForegroundOriginal(this Color color)
        {
            if (color.Type == ColorType.TrueColor)
                return color.VTSequenceForegroundTrueColor();
            color.vtSeqForeground ??=
                TermInfoDesc.Current.SetAForeground?.ProcessSequence(color.PlainSequence) ??
                $"{VtSequenceBasicChars.EscapeChar}[38;5;{color.PlainSequence}m";
            return color.vtSeqForeground;
        }

        /// <summary>
        /// Parsable VT sequence (Foreground, true color)
        /// </summary>
        public static string VTSequenceForegroundTrueColor(this Color color)
        {
            color.vtSeqForegroundTrue ??= $"{VtSequenceBasicChars.EscapeChar}[38;2;{color.PlainSequenceTrueColor}m";
            return color.vtSeqForegroundTrue;
        }

        /// <summary>
        /// Parsable VT sequence (Background)
        /// </summary>
        public static string VTSequenceBackground(this Color color) =>
            color.IsOriginal ? color.VTSequenceBackgroundOriginal() : color.VTSequenceBackgroundTrueColor();

        /// <summary>
        /// Parsable VT sequence (Background)
        /// </summary>
        public static string VTSequenceBackgroundOriginal(this Color color)
        {
            if (color.Type == ColorType.TrueColor)
                return color.VTSequenceBackgroundTrueColor();
            color.vtSeqBackground ??=
                TermInfoDesc.Current.SetABackground?.ProcessSequence(color.PlainSequence) ??
                $"{VtSequenceBasicChars.EscapeChar}[38;5;{color.PlainSequence}m";
            return color.vtSeqBackground;
        }

        /// <summary>
        /// Parsable VT sequence (Background, true color)
        /// </summary>
        public static string VTSequenceBackgroundTrueColor(this Color color)
        {
            color.vtSeqBackgroundTrue ??= $"{VtSequenceBasicChars.EscapeChar}[48;2;{color.PlainSequenceTrueColor}m";
            return color.vtSeqBackgroundTrue;
        }
    }
}
