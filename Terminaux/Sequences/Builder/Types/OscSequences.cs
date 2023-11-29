
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Text.RegularExpressions;
using Terminaux.Base;

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of OSC sequences and their regular expressions
    /// </summary>
    public static class OscSequences
    {
        private static readonly Regex oscOperatingSystemCommandSequenceRegex = new(@"(\x9D|\x1B\]).+[\x07]", RegexOptions.Compiled);
        private static readonly Regex oscOperatingSystemCommandAltSequenceRegex = new(@"(\x9D|\x1B\]).+[\x9c]", RegexOptions.Compiled);

        /// <summary>
        /// [OSC Ps ; Pt BEL] Regular expression for operating system command
        /// </summary>
        public static Regex OscOperatingSystemCommandSequenceRegex =>
            oscOperatingSystemCommandSequenceRegex;

        /// <summary>
        /// [OSC Ps ; Pt ST] Regular expression for operating system command
        /// </summary>
        public static Regex OscOperatingSystemCommandAltSequenceRegex =>
            oscOperatingSystemCommandAltSequenceRegex;

        /// <summary>
        /// [OSC Ps ; Pt BEL] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateOscOperatingSystemCommand(string proprietaryCommands)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}]{proprietaryCommands}{VtSequenceBasicChars.BellChar}";
	        var regexParser = OscOperatingSystemCommandSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
        
        /// <summary>
        /// [OSC Ps ; Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateOscOperatingSystemCommandAlt(string proprietaryCommands)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}]{proprietaryCommands}{VtSequenceBasicChars.StChar}";
	        var regexParser = OscOperatingSystemCommandAltSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
