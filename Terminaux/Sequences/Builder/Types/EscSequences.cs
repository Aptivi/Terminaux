
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

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of ESC sequences and their regular expressions
    /// </summary>
    public static class EscSequences
    {
        /// <summary>
        /// [ESC SP F] Regular expression for 7-bit controls
        /// </summary>
        public static string Esc7BitControlsSequenceRegex { get => @"\x1b F"; }

        /// <summary>
        /// [ESC SP G] Regular expression for 8-bit controls
        /// </summary>
        public static string Esc8BitControlsSequenceRegex { get => @"\x1b G"; }

        /// <summary>
        /// [ESC SP L] Regular expression for setting ANSI conformance level 1
        /// </summary>
        public static string EscAnsiConformanceLevel1SequenceRegex { get => @"\x1b L"; }

        /// <summary>
        /// [ESC SP M] Regular expression for setting ANSI conformance level 2
        /// </summary>
        public static string EscAnsiConformanceLevel2SequenceRegex { get => @"\x1b M"; }

        /// <summary>
        /// [ESC SP N] Regular expression for setting ANSI conformance level 3
        /// </summary>
        public static string EscAnsiConformanceLevel3SequenceRegex { get => @"\x1b N"; }

        /// <summary>
        /// [ESC # 3] Regular expression for DEC double-height line top half
        /// </summary>
        public static string EscDecDoubleHeightLineTopHalfSequenceRegex { get => @"\x1b#3"; }

        /// <summary>
        /// [ESC # 4] Regular expression for DEC double-height line bottom half
        /// </summary>
        public static string EscDecDoubleHeightLineBottomHalfSequenceRegex { get => @"\x1b#4"; }

        /// <summary>
        /// [ESC # 5] Regular expression for DEC single-width line
        /// </summary>
        public static string EscDecSingleWidthLineSequenceRegex { get => @"\x1b#5"; }

        /// <summary>
        /// [ESC # 6] Regular expression for DEC double-width line
        /// </summary>
        public static string EscDecDoubleWidthLineSequenceRegex { get => @"\x1b#6"; }

        /// <summary>
        /// [ESC # 8] Regular expression for DEC screen alignment test
        /// </summary>
        public static string EscDecScreenAlignmentTestSequenceRegex { get => @"\x1b#8"; }

        /// <summary>
        /// [ESC % @] Regular expression for selecting default character set
        /// </summary>
        public static string EscSelectDefaultCharacterSetSequenceRegex { get => @"\x1b%@"; }

        /// <summary>
        /// [ESC % G] Regular expression for selecting UTF-8 character set
        /// </summary>
        public static string EscSelectUtf8CharacterSetSequenceRegex { get => @"\x1b%G"; }

        /// <summary>
        /// [ESC ( Pc] Regular expression for designating the G0 character set
        /// </summary>
        public static string EscDesignateG0CharacterSetSequenceRegex { get => @"\x1b\(.+"; }

        /// <summary>
        /// [ESC ) Pc] Regular expression for designating the G1 character set
        /// </summary>
        public static string EscDesignateG1CharacterSetSequenceRegex { get => @"\x1b\).+"; }

        /// <summary>
        /// [ESC * Pc] Regular expression for designating the G2 character set
        /// </summary>
        public static string EscDesignateG2CharacterSetSequenceRegex { get => @"\x1b\*.+"; }

        /// <summary>
        /// [ESC + Pc] Regular expression for designating the G3 character set
        /// </summary>
        public static string EscDesignateG3CharacterSetSequenceRegex { get => @"\x1b\+.+"; }

        /// <summary>
        /// [ESC - Pc] Regular expression for designating the G1 character set
        /// </summary>
        public static string EscDesignateG1CharacterSetAltSequenceRegex { get => @"\x1b\-.+"; }

        /// <summary>
        /// [ESC , Pc] Regular expression for designating the G2 character set
        /// </summary>
        public static string EscDesignateG2CharacterSetAltSequenceRegex { get => @"\x1b,.+"; }

        /// <summary>
        /// [ESC / Pc] Regular expression for designating the G3 character set
        /// </summary>
        public static string EscDesignateG3CharacterSetAltSequenceRegex { get => @"\x1b/.+"; }

        /// <summary>
        /// [ESC 6] Regular expression for back index
        /// </summary>
        public static string EscBackIndexSequenceRegex { get => @"\x1b6"; }

        /// <summary>
        /// [ESC 7] Regular expression for saving cursor
        /// </summary>
        public static string EscSaveCursorSequenceRegex { get => @"\x1b7"; }

        /// <summary>
        /// [ESC 8] Regular expression for restoring cursor
        /// </summary>
        public static string EscRestoreCursorSequenceRegex { get => @"\x1b8"; }

        /// <summary>
        /// [ESC 9] Regular expression for forward index
        /// </summary>
        public static string EscForwardIndexSequenceRegex { get => @"\x1b9"; }

        /// <summary>
        /// [ESC =] Regular expression for application keypad
        /// </summary>
        public static string EscApplicationKeypadSequenceRegex { get => @"\x1b="; }

        /// <summary>
        /// [ESC >] Regular expression for normal keypad
        /// </summary>
        public static string EscNormalKeypadSequenceRegex { get => @"\x1b\>"; }

        /// <summary>
        /// [ESC F] Regular expression for cursor to lower left corner
        /// </summary>
        public static string EscCursorToLowerLeftCornerSequenceRegex { get => @"\x1bF"; }

        /// <summary>
        /// [ESC c] Regular expression for full reset
        /// </summary>
        public static string EscFullResetSequenceRegex { get => @"\x1bc"; }

        /// <summary>
        /// [ESC l] Regular expression for memory lock
        /// </summary>
        public static string EscMemoryLockSequenceRegex { get => @"\x1bl"; }

        /// <summary>
        /// [ESC m] Regular expression for memory unlock
        /// </summary>
        public static string EscMemoryUnlockSequenceRegex { get => @"\x1bm"; }

        /// <summary>
        /// [ESC n] Regular expression for invoking the G2 character set as GL
        /// </summary>
        public static string EscInvokeG2CharacterSetGlSequenceRegex { get => @"\x1bn"; }

        /// <summary>
        /// [ESC o] Regular expression for invoking the G3 character set as GL
        /// </summary>
        public static string EscInvokeG3CharacterSetGlSequenceRegex { get => @"\x1bo"; }

        /// <summary>
        /// [ESC |] Regular expression for invoking the G3 character set as GR
        /// </summary>
        public static string EscInvokeG3CharacterSetGrSequenceRegex { get => @"\x1b\|"; }

        /// <summary>
        /// [ESC }] Regular expression for invoking the G2 character set as GR
        /// </summary>
        public static string EscInvokeG2CharacterSetGrSequenceRegex { get => @"\x1b\}"; }

        /// <summary>
        /// [ESC ~] Regular expression for invoking the G1 character set as GR
        /// </summary>
        public static string EscInvokeG1CharacterSetGrSequenceRegex { get => @"\x1b~"; }
	
	    /// <summary>
        /// [ESC SP F] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEsc7BitControls()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar} F";
	        var regexParser = new Regex(Esc7BitControlsSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC SP G] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEsc8BitControls()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar} G";
	        var regexParser = new Regex(Esc8BitControlsSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC SP L] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscAnsiConformanceLevel1()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar} L";
	        var regexParser = new Regex(EscAnsiConformanceLevel1SequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC SP M] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscAnsiConformanceLevel2()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar} M";
	        var regexParser = new Regex(EscAnsiConformanceLevel2SequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC SP N] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscAnsiConformanceLevel3()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar} N";
	        var regexParser = new Regex(EscAnsiConformanceLevel3SequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC # 3] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecDoubleHeightLineTopHalf()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}#3";
	        var regexParser = new Regex(EscDecDoubleHeightLineTopHalfSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC # 4] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecDoubleHeightLineBottomHalf()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}#4";
	        var regexParser = new Regex(EscDecDoubleHeightLineBottomHalfSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC # 5] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecSingleWidthLine()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}#5";
	        var regexParser = new Regex(EscDecSingleWidthLineSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC # 6] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecDoubleWidthLine()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}#6";
	        var regexParser = new Regex(EscDecDoubleWidthLineSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC # 8] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecScreenAlignmentTest()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}#8";
	        var regexParser = new Regex(EscDecScreenAlignmentTestSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC % @] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscSelectDefaultCharacterSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}%@";
	        var regexParser = new Regex(EscSelectDefaultCharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC % G] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscSelectUtf8CharacterSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}%G";
	        var regexParser = new Regex(EscSelectUtf8CharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC ( Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG0CharacterSet(string charSet)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}({charSet}";
	        var regexParser = new Regex(EscDesignateG0CharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC ) Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG1CharacterSet(string charSet)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}){charSet}";
	        var regexParser = new Regex(EscDesignateG1CharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC * Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG2CharacterSet(string charSet)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}*{charSet}";
	        var regexParser = new Regex(EscDesignateG2CharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC + Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG3CharacterSet(string charSet)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}+{charSet}";
	        var regexParser = new Regex(EscDesignateG3CharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC - Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG1CharacterSetAlt(string charSet)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}-{charSet}";
	        var regexParser = new Regex(EscDesignateG1CharacterSetAltSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC , Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG2CharacterSetAlt(string charSet)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar},{charSet}";
	        var regexParser = new Regex(EscDesignateG2CharacterSetAltSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC / Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG3CharacterSetAlt(string charSet)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}/{charSet}";
	        var regexParser = new Regex(EscDesignateG3CharacterSetAltSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC 6] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscBackIndex()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}6";
	        var regexParser = new Regex(EscBackIndexSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC 7] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscSaveCursor()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}7";
	        var regexParser = new Regex(EscSaveCursorSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC 8] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscRestoreCursor()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}8";
	        var regexParser = new Regex(EscRestoreCursorSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC 9] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscForwardIndex()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}9";
	        var regexParser = new Regex(EscForwardIndexSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC =] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscApplicationKeypad()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}=";
	        var regexParser = new Regex(EscApplicationKeypadSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC >] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscNormalKeypad()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}>";
	        var regexParser = new Regex(EscNormalKeypadSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC F] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscCursorToLowerLeftCorner()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}F";
	        var regexParser = new Regex(EscCursorToLowerLeftCornerSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC c] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscFullReset()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}c";
	        var regexParser = new Regex(EscFullResetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC l] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscMemoryLock()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}l";
	        var regexParser = new Regex(EscMemoryLockSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC m] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscMemoryUnlock()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}m";
	        var regexParser = new Regex(EscMemoryUnlockSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC n] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG2CharacterSetGl()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}n";
	        var regexParser = new Regex(EscInvokeG2CharacterSetGlSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC o] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG3CharacterSetGl()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}o";
	        var regexParser = new Regex(EscInvokeG3CharacterSetGlSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC |] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG3CharacterSetGr()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}|";
	        var regexParser = new Regex(EscInvokeG3CharacterSetGrSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC }] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG2CharacterSetGr()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}}}";
	        var regexParser = new Regex(EscInvokeG2CharacterSetGrSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC ~] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG1CharacterSetGr()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}~";
	        var regexParser = new Regex(EscInvokeG1CharacterSetGrSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
