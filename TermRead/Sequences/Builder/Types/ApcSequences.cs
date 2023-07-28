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
    /// List of APC sequences and their regular expressions
    /// </summary>
    public static class ApcSequences
    {
        /// <summary>
        /// [APC Pt ST] Regular expression for application program command
        /// </summary>
        public static string ApcApplicationProgramCommandSequenceRegex { get => @"(\x9f|\x1b_).+\x9c"; }
        
        /// <summary>
        /// [APC Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateApcApplicationProgramCommand(string proprietaryCommands)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}_{proprietaryCommands}{VtSequenceBasicChars.StChar}";
	        var regexParser = new Regex(ApcApplicationProgramCommandSequenceRegex);
		    if (!regexParser.IsMatch(result))
		        throw new Exception("TermRead failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
    }
}
