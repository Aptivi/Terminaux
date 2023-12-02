
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

using System.Text.RegularExpressions;
using Terminaux.Base;

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of DCS sequences and their regular expressions
    /// </summary>
    public static class DcsSequences
    {
        private static readonly Regex dcsUserDefinedKeysSequenceRegex = new(@"(\x90|\x1bP)[0-9]*\;[0-9]*\|(.+?)\x9c", RegexOptions.Compiled);
        private static readonly Regex dcsRequestStatusStringSequenceRegex = new(@"(\x90|\x1bP)\$q(.+?)\x9c", RegexOptions.Compiled);
        private static readonly Regex dcsRestorePresentationStatusSequenceRegex = new(@"(\x90|\x1bP)[0-9]*\$t[0-9]*\x9c", RegexOptions.Compiled);
        private static readonly Regex dcsRequestResourceValuesSequenceRegex = new(@"(\x90|\x1bP)\+Q(.+?)\x9c", RegexOptions.Compiled);
        private static readonly Regex dcsSetTermInfoDataSequenceRegex = new(@"(\x90|\x1bP)\+p(.+?)\x9c", RegexOptions.Compiled);
        private static readonly Regex dcsRequestTermInfoDataSequenceRegex = new(@"(\x90|\x1bP)\+q(.+?)\x9c", RegexOptions.Compiled);

        /// <summary>
        /// [DCS Ps ; Ps | Pt ST] Regular expression for user defined keys
        /// </summary>
        public static Regex DcsUserDefinedKeysSequenceRegex =>
            dcsUserDefinedKeysSequenceRegex;

        /// <summary>
        /// [DCS $ q Pt ST] Regular expression for requesting status string
        /// </summary>
        public static Regex DcsRequestStatusStringSequenceRegex =>
            dcsRequestStatusStringSequenceRegex;

        /// <summary>
        /// [DCS Ps $ t Pt ST] Regular expression for restoring presentation status
        /// </summary>
        public static Regex DcsRestorePresentationStatusSequenceRegex =>
            dcsRestorePresentationStatusSequenceRegex;

        /// <summary>
        /// [DCS + Q Pt ST] Regular expression for requesting resource values for xterm
        /// </summary>
        public static Regex DcsRequestResourceValuesSequenceRegex =>
            dcsRequestResourceValuesSequenceRegex;
        
        /// <summary>
        /// [DCS + p Pt ST] Regular expression for setting terminfo data for xterm
        /// </summary>
        public static Regex DcsSetTermInfoDataSequenceRegex =>
            dcsSetTermInfoDataSequenceRegex;
        
        /// <summary>
        /// [DCS + q Pt ST] Regular expression for requesting terminfo data for xterm
        /// </summary>
        public static Regex DcsRequestTermInfoDataSequenceRegex =>
            dcsRequestTermInfoDataSequenceRegex;
	
	    /// <summary>
        /// [DCS Ps ; Ps | Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsUserDefinedKeys(int clearUdkDefinitions, int dontLockKeys, string keybindings)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P{clearUdkDefinitions};{dontLockKeys}|{keybindings}{VtSequenceBasicChars.StChar}";
	        var regexParser = DcsUserDefinedKeysSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS $ q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRequestStatusString(string status)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P$q{status}{VtSequenceBasicChars.StChar}";
	        var regexParser = DcsRequestStatusStringSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS Ps $ t Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRestorePresentationStatus(int controlConvert, string status)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P{controlConvert}$t{status}{VtSequenceBasicChars.StChar}";
	        var regexParser = DcsRestorePresentationStatusSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS + Q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRequestResourceValues(string xtermResourceNames)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P+Q{xtermResourceNames}{VtSequenceBasicChars.StChar}";
	        var regexParser = DcsRequestResourceValuesSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS + p Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsSetTermInfoData(string termName)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P+p{termName}{VtSequenceBasicChars.StChar}";
	        var regexParser = DcsSetTermInfoDataSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS + q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRequestTermInfoData(string termName)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P+q{termName}{VtSequenceBasicChars.StChar}";
	        var regexParser = DcsRequestTermInfoDataSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
