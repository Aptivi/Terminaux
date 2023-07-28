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

namespace TermRead.Sequences.Builder
{
    /// <summary>
    /// VT sequence basic characters for forming VT sequences
    /// </summary>
    public static class VtSequenceBasicChars
    {
        /*
         *  Taken from https://invisible-island.net/xterm/ctlseqs/ctlseqs.html
         *  
         *  General definitions:
         *
         *  C    A single (required) character.
         *
         *  Ps   A single (usually optional) numeric parameter, composed of one or
         *       more digits.
         *
         *  Pm   Any number of single numeric parameters, separated by ;
         *       character(s). Individual values for the parameters are listed with
         *       Ps.
         *
         *  Pt   A text parameter composed of printable characters.
         *  
         *  Single-character functions:
         *  
         *  BEL       Bell (BEL is Ctrl-G).
         *  
         *  BS        Backspace (BS is Ctrl-H).
         *  
         *  CR        Carriage Return (CR is Ctrl-M).
         *  
         *  ENQ       Return Terminal Status (ENQ is Ctrl-E). Default response is
         *            an empty string, but may be overridden by a resource
         *            answerbackString.
         *  
         *  FF        Form Feed or New Page (NP). (FF is Ctrl-L).  FF is treated
         *            the same as LF.
         *  
         *  LF        Line Feed or New Line (NL). (LF is Ctrl-J).
         *  
         *  SI        Switch to Standard Character Set (Ctrl-O is Shift In or LS0).
         *            This invokes the G0 character set (the default) as GL.
         *            VT200 and up implement LS0.
         *  
         *  SO        Switch to Alternate Character Set (Ctrl-N is Shift Out or
         *            LS1). This invokes the G1 character set as GL.
         *            VT200 and up implement LS1.
         *  
         *  SP        Space.
         *  
         *  TAB       Horizontal Tab (HTS is Ctrl-I).
         *  
         *  VT        Vertical Tab (VT is Ctrl-K). This is treated the same as LF.
         */

        /// <summary>
        /// Gets the bell character
        /// </summary>
        public const char BellChar = '\x07';

        /// <summary>
        /// Gets the backspace character
        /// </summary>
        public const char BackspaceChar = '\x08';

        /// <summary>
        /// Gets the carriage return character
        /// </summary>
        public const char CarriageReturnChar = '\x0D';

        /// <summary>
        /// Gets the return terminal status character
        /// </summary>
        public const char ReturnTerminalStatusChar = '\x05';

        /// <summary>
        /// Gets the form feed character
        /// </summary>
        public const char FormFeedChar = '\x0C';

        /// <summary>
        /// Gets the line feed character
        /// </summary>
        public const char LineFeedChar = '\x0A';

        /// <summary>
        /// Gets the standard character set character
        /// </summary>
        public const char StandardCharacterSetChar = '\x0F';

        /// <summary>
        /// Gets the alternate character set character
        /// </summary>
        public const char AlternateCharacterSetChar = '\x0E';

        /// <summary>
        /// Gets the space character
        /// </summary>
        public const char SpaceChar = ' ';

        /// <summary>
        /// Gets the horizontal tab character
        /// </summary>
        public const char HorizontalTabChar = '\x09';

        /// <summary>
        /// Gets the vertical tab character
        /// </summary>
        public const char VerticalTabChar = '\x0B';

        /// <summary>
        /// Gets the escape character
        /// </summary>
        public const char EscapeChar = '\x1B';

        /// <summary>
        /// Gets the St character
        /// </summary>
        public const char StChar = '\x9C';
    }
}
