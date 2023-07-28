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
    /// List of PM sequences and their regular expressions
    /// </summary>
    public static class PmSequences
    {
        /// <summary>
        /// [PM Pt ST] Regular expression for privacy message
        /// </summary>
        public static string PmPrivacyMessageSequenceRegex { get => @"(\x9e|\x1b\^).+\x9c"; }
        
        /// <summary>
        /// [PM Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GeneratePmPrivacyMessage(string proprietaryCommands)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}^{proprietaryCommands}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(PmPrivacyMessageSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
