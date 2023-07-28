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

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of DCS sequences and their regular expressions
    /// </summary>
    public static class DcsSequences
    {
        /// <summary>
        /// [DCS Ps ; Ps | Pt ST] Regular expression for user defined keys
        /// </summary>
        public static string DcsUserDefinedKeysSequenceRegex { get => @"(\x90|\x1bP)[0-9]*\;[0-9]*\|(.+?)\x9c"; }

        /// <summary>
        /// [DCS $ q Pt ST] Regular expression for requesting status string
        /// </summary>
        public static string DcsRequestStatusStringSequenceRegex { get => @"(\x90|\x1bP)\$q(.+?)\x9c"; }

        /// <summary>
        /// [DCS Ps $ t Pt ST] Regular expression for restoring presentation status
        /// </summary>
        public static string DcsRestorePresentationStatusSequenceRegex { get => @"(\x90|\x1bP)[0-9]*\$t[0-9]*\x9c"; }

        /// <summary>
        /// [DCS + Q Pt ST] Regular expression for requesting resource values for xterm
        /// </summary>
        public static string DcsRequestResourceValuesSequenceRegex { get => @"(\x90|\x1bP)\+Q(.+?)\x9c"; }
        
        /// <summary>
        /// [DCS + p Pt ST] Regular expression for setting terminfo data for xterm
        /// </summary>
        public static string DcsSetTermInfoDataSequenceRegex { get => @"(\x90|\x1bP)\+p(.+?)\x9c"; }
        
        /// <summary>
        /// [DCS + q Pt ST] Regular expression for requesting terminfo data for xterm
        /// </summary>
        public static string DcsRequestTermInfoDataSequenceRegex { get => @"(\x90|\x1bP)\+q(.+?)\x9c"; }
	
	    /// <summary>
        /// [DCS Ps ; Ps | Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsUserDefinedKeys(int clearUdkDefinitions, int dontLockKeys, string keybindings)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P{clearUdkDefinitions};{dontLockKeys}|{keybindings}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(DcsUserDefinedKeysSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS $ q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRequestStatusString(string status)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P$q{status}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(DcsRequestStatusStringSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS Ps $ t Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRestorePresentationStatus(int controlConvert, string status)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P{controlConvert}$t{status}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(DcsRestorePresentationStatusSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS + Q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRequestResourceValues(string xtermResourceNames)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P+Q{xtermResourceNames}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(DcsRequestResourceValuesSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS + p Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsSetTermInfoData(string termName)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P+p{termName}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(DcsSetTermInfoDataSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [DCS + q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateDcsRequestTermInfoData(string termName)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}P+q{termName}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(DcsRequestTermInfoDataSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
