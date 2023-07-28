/*
 * MIT License
 * 
 * Copyright (c) 2022 Aptivi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Text.RegularExpressions;

namespace TermRead.Sequences.Builder.Types
{
    /// <summary>
    /// List of C1 sequences and their regular expressions
    /// </summary>
    public static class C1Sequences
    {
        /// <summary>
        /// [ESC D] Regular expression for index
        /// </summary>
        public static string C1IndexSequenceRegex { get => @"\x1bD"; }

        /// <summary>
        /// [ESC E] Regular expression for next line
        /// </summary>
        public static string C1NextLineSequenceRegex { get => @"\x1bE"; }

        /// <summary>
        /// [ESC H] Regular expression for tab set
        /// </summary>
        public static string C1TabSetSequenceRegex { get => @"\x1bH"; }

        /// <summary>
        /// [ESC M] Regular expression for reverse index
        /// </summary>
        public static string C1ReverseIndexSequenceRegex { get => @"\x1bM"; }

        /// <summary>
        /// [ESC N] Regular expression for single shift select of G2 character set
        /// </summary>
        public static string C1SingleShiftSelectG2CharacterSetSequenceRegex { get => @"\x1bN"; }

        /// <summary>
        /// [ESC O] Regular expression for single shift select of G3 character set
        /// </summary>
        public static string C1SingleShiftSelectG3CharacterSetSequenceRegex { get => @"\x1bO"; }

        /// <summary>
        /// [ESC P] Regular expression for device control string
        /// </summary>
        public static string C1DeviceControlStringSequenceRegex { get => @"\x1bP"; }

        /// <summary>
        /// [ESC V] Regular expression for start of guarded area
        /// </summary>
        public static string C1StartOfGuardedAreaSequenceRegex { get => @"\x1bV"; }

        /// <summary>
        /// [ESC W] Regular expression for end of guarded area
        /// </summary>
        public static string C1EndOfGuardedAreaSequenceRegex { get => @"\x1bW"; }

        /// <summary>
        /// [ESC X] Regular expression for start of string
        /// </summary>
        public static string C1StartOfStringSequenceRegex { get => @"\x1bX"; }

        /// <summary>
        /// [ESC Z] Regular expression for returning terminal ID
        /// </summary>
        public static string C1ReturnTerminalIdSequenceRegex { get => @"\x1bZ"; }

        /// <summary>
        /// [ESC [] Regular expression for control sequence introducer
        /// </summary>
        public static string C1ControlSequenceIndicatorSequenceRegex { get => @"\x1b\["; }

        /// <summary>
        /// [ESC \] Regular expression for string terminator
        /// </summary>
        public static string C1StringTerminatorSequenceRegex { get => @"\x1b\\"; }

        /// <summary>
        /// [ESC ]] Regular expression for operating system command
        /// </summary>
        public static string C1OperatingSystemCommandSequenceRegex { get => @"\x1b\]"; }

        /// <summary>
        /// [ESC ^] Regular expression for privacy message
        /// </summary>
        public static string C1PrivacyMessageSequenceRegex { get => @"\x1b\^"; }

        /// <summary>
        /// [ESC _] Regular expression for application program command
        /// </summary>
        public static string C1ApplicationProgramCommandSequenceRegex { get => @"\x1b_"; }
        
        /// <summary>
        /// [ESC D] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1Index()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}D";
	        var regexParser = new Regex(C1IndexSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC E] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1NextLine()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}E";
	        var regexParser = new Regex(C1NextLineSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC H] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1TabSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}H";
	        var regexParser = new Regex(C1TabSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC M] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ReverseIndex()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}M";
	        var regexParser = new Regex(C1ReverseIndexSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC N] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1SingleShiftSelectG2CharacterSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}N";
	        var regexParser = new Regex(C1SingleShiftSelectG2CharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC O] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1SingleShiftSelectG3CharacterSet()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}O";
	        var regexParser = new Regex(C1SingleShiftSelectG3CharacterSetSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC P] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1DeviceControlString()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P";
	        var regexParser = new Regex(C1DeviceControlStringSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC V] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1StartOfGuardedArea()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}V";
	        var regexParser = new Regex(C1StartOfGuardedAreaSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC W] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1EndOfGuardedArea()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}W";
	        var regexParser = new Regex(C1EndOfGuardedAreaSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC X] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1StartOfString()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}X";
	        var regexParser = new Regex(C1StartOfStringSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC Z] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ReturnTerminalId()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}Z";
	        var regexParser = new Regex(C1ReturnTerminalIdSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC [] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ControlSequenceIndicator()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[";
	        var regexParser = new Regex(C1ControlSequenceIndicatorSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC \] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1StringTerminator()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}\\";
	        var regexParser = new Regex(C1StringTerminatorSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC ]] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1OperatingSystemCommand()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}]";
	        var regexParser = new Regex(C1OperatingSystemCommandSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC ^] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1PrivacyMessage()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}^";
	        var regexParser = new Regex(C1PrivacyMessageSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [ESC _] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateC1ApplicationProgramCommand()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}_";
	        var regexParser = new Regex(C1ApplicationProgramCommandSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
