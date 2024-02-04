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

using System.Text.RegularExpressions;
using Textify.Tools;

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of PM sequences and their regular expressions
    /// </summary>
    public static class PmSequences
    {
        private static readonly Regex pmPrivacyMessageSequenceRegex = new(@"(\x9e|\x1b\^).+\x9c", RegexOptions.Compiled);

        /// <summary>
        /// [PM Pt ST] Regular expression for privacy message
        /// </summary>
        public static Regex PmPrivacyMessageSequenceRegex =>
            pmPrivacyMessageSequenceRegex;

        /// <summary>
        /// [PM Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GeneratePmPrivacyMessage(string proprietaryCommands)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}^{proprietaryCommands}{VtSequenceBasicChars.StChar}";
            var regexParser = PmPrivacyMessageSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }
    }
}
