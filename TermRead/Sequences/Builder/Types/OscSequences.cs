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
    /// List of OSC sequences and their regular expressions
    /// </summary>
    public static class OscSequences
    {
        /// <summary>
        /// [OSC Ps ; Pt BEL] Regular expression for operating system command
        /// </summary>
        public static string OscOperatingSystemCommandSequenceRegex { get => @"(\x9D|\x1B\]).+[\x07]"; }

        /// <summary>
        /// [OSC Ps ; Pt ST] Regular expression for operating system command
        /// </summary>
        public static string OscOperatingSystemCommandAltSequenceRegex { get => @"(\x9D|\x1B\]).+[\x9c]"; }
        
        /// <summary>
        /// [OSC Ps ; Pt BEL] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateOscOperatingSystemCommand(string proprietaryCommands)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}]{proprietaryCommands}{VtSequenceBasicChars.BellChar}";
	        var regexParser = new Regex(OscOperatingSystemCommandSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
        
        /// <summary>
        /// [OSC Ps ; Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateOscOperatingSystemCommandAlt(string proprietaryCommands)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}]{proprietaryCommands}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(OscOperatingSystemCommandAltSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
