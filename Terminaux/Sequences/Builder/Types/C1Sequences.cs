
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
    /// List of C1 sequences and their regular expressions
    /// </summary>
    public static class C1Sequences
    {
        private readonly static Regex c1IndexSequenceRegex = new(@"\x1bD", RegexOptions.Compiled);
        private readonly static Regex c1NextLineSequenceRegex = new(@"\x1bE", RegexOptions.Compiled);
        private readonly static Regex c1TabSetSequenceRegex = new(@"\x1bH", RegexOptions.Compiled);
        private readonly static Regex c1ReverseIndexSequenceRegex = new(@"\x1bM", RegexOptions.Compiled);
        private readonly static Regex c1SingleShiftSelectG2CharacterSetSequenceRegex = new(@"\x1bN", RegexOptions.Compiled);
        private readonly static Regex c1SingleShiftSelectG3CharacterSetSequenceRegex = new(@"\x1bO", RegexOptions.Compiled);
        private readonly static Regex c1DeviceControlStringSequenceRegex = new(@"\x1bP", RegexOptions.Compiled);
        private readonly static Regex c1StartOfGuardedAreaSequenceRegex = new(@"\x1bV", RegexOptions.Compiled);
        private readonly static Regex c1EndOfGuardedAreaSequenceRegex = new(@"\x1bW", RegexOptions.Compiled);
        private readonly static Regex c1StartOfStringSequenceRegex = new(@"\x1bX", RegexOptions.Compiled);
        private readonly static Regex c1ReturnTerminalIdSequenceRegex = new(@"\x1bZ", RegexOptions.Compiled);
        private readonly static Regex c1ControlSequenceIndicatorSequenceRegex = new(@"\x1b\[", RegexOptions.Compiled);
        private readonly static Regex c1StringTerminatorSequenceRegex = new(@"\x1b\\", RegexOptions.Compiled);
        private readonly static Regex c1OperatingSystemCommandSequenceRegex = new(@"\x1b\]", RegexOptions.Compiled);
        private readonly static Regex c1PrivacyMessageSequenceRegex = new(@"\x1b\^", RegexOptions.Compiled);
        private readonly static Regex c1ApplicationProgramCommandSequenceRegex = new(@"\x1b_", RegexOptions.Compiled);

        /// <summary>
        /// [ESC D] Regular expression for index
        /// </summary>
        public static Regex C1IndexSequenceRegex =>
            c1IndexSequenceRegex;

        /// <summary>
        /// [ESC E] Regular expression for next line
        /// </summary>
        public static Regex C1NextLineSequenceRegex =>
            c1NextLineSequenceRegex;

        /// <summary>
        /// [ESC H] Regular expression for tab set
        /// </summary>
        public static Regex C1TabSetSequenceRegex =>
            c1TabSetSequenceRegex;

        /// <summary>
        /// [ESC M] Regular expression for reverse index
        /// </summary>
        public static Regex C1ReverseIndexSequenceRegex =>
            c1ReverseIndexSequenceRegex;

        /// <summary>
        /// [ESC N] Regular expression for single shift select of G2 character set
        /// </summary>
        public static Regex C1SingleShiftSelectG2CharacterSetSequenceRegex =>
            c1SingleShiftSelectG2CharacterSetSequenceRegex;

        /// <summary>
        /// [ESC O] Regular expression for single shift select of G3 character set
        /// </summary>
        public static Regex C1SingleShiftSelectG3CharacterSetSequenceRegex =>
            c1SingleShiftSelectG3CharacterSetSequenceRegex;

        /// <summary>
        /// [ESC P] Regular expression for device control string
        /// </summary>
        public static Regex C1DeviceControlStringSequenceRegex =>
            c1DeviceControlStringSequenceRegex;

        /// <summary>
        /// [ESC V] Regular expression for start of guarded area
        /// </summary>
        public static Regex C1StartOfGuardedAreaSequenceRegex =>
            c1StartOfGuardedAreaSequenceRegex;

        /// <summary>
        /// [ESC W] Regular expression for end of guarded area
        /// </summary>
        public static Regex C1EndOfGuardedAreaSequenceRegex =>
            c1EndOfGuardedAreaSequenceRegex;

        /// <summary>
        /// [ESC X] Regular expression for start of string
        /// </summary>
        public static Regex C1StartOfStringSequenceRegex =>
            c1StartOfStringSequenceRegex;

        /// <summary>
        /// [ESC Z] Regular expression for returning terminal ID
        /// </summary>
        public static Regex C1ReturnTerminalIdSequenceRegex =>
            c1ReturnTerminalIdSequenceRegex;

        /// <summary>
        /// [ESC [] Regular expression for control sequence introducer
        /// </summary>
        public static Regex C1ControlSequenceIndicatorSequenceRegex =>
            c1ControlSequenceIndicatorSequenceRegex;

        /// <summary>
        /// [ESC \] Regular expression for string terminator
        /// </summary>
        public static Regex C1StringTerminatorSequenceRegex =>
            c1StringTerminatorSequenceRegex;

        /// <summary>
        /// [ESC ]] Regular expression for operating system command
        /// </summary>
        public static Regex C1OperatingSystemCommandSequenceRegex =>
            c1OperatingSystemCommandSequenceRegex;

        /// <summary>
        /// [ESC ^] Regular expression for privacy message
        /// </summary>
        public static Regex C1PrivacyMessageSequenceRegex =>
            c1PrivacyMessageSequenceRegex;

        /// <summary>
        /// [ESC _] Regular expression for application program command
        /// </summary>
        public static Regex C1ApplicationProgramCommandSequenceRegex =>
            c1ApplicationProgramCommandSequenceRegex;
        
        /// <summary>
        /// [ESC D] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1Index()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}D";
	        var regexParser = C1IndexSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC E] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1NextLine()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}E";
	        var regexParser = C1NextLineSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC H] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1TabSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}H";
	        var regexParser = C1TabSetSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC M] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ReverseIndex()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}M";
	        var regexParser = C1ReverseIndexSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC N] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1SingleShiftSelectG2CharacterSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}N";
	        var regexParser = C1SingleShiftSelectG2CharacterSetSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC O] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1SingleShiftSelectG3CharacterSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}O";
	        var regexParser = C1SingleShiftSelectG3CharacterSetSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC P] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1DeviceControlString()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P";
	        var regexParser = C1DeviceControlStringSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC V] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1StartOfGuardedArea()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}V";
	        var regexParser = C1StartOfGuardedAreaSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC W] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1EndOfGuardedArea()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}W";
	        var regexParser = C1EndOfGuardedAreaSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC X] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1StartOfString()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}X";
	        var regexParser = C1StartOfStringSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC Z] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ReturnTerminalId()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}Z";
	        var regexParser = C1ReturnTerminalIdSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC [] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ControlSequenceIndicator()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[";
	        var regexParser = C1ControlSequenceIndicatorSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC \] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1StringTerminator()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}\\";
	        var regexParser = C1StringTerminatorSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC ]] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1OperatingSystemCommand()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}]";
	        var regexParser = C1OperatingSystemCommandSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC ^] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1PrivacyMessage()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}^";
	        var regexParser = C1PrivacyMessageSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC _] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ApplicationProgramCommand()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}_";
	        var regexParser = C1ApplicationProgramCommandSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new TerminauxException("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
