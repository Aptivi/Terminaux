//
// Terminaux  Copyright (C) 2023-2025  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

namespace Terminaux.Sequences.Builder
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

        /// <summary>
        /// Gets the CSI character
        /// </summary>
        public const char CsiChar = '\x9B';

        /// <summary>
        /// Gets the CSI prefix character
        /// </summary>
        public const char CsiPrefixChar = '[';

        /// <summary>
        /// Gets the CSI escape sequence prefix
        /// </summary>
        public static readonly string CsiSequencePrefix = $"{EscapeChar}{CsiPrefixChar}";

        /// <summary>
        /// Gets the OSC character
        /// </summary>
        public const char OSCChar = '\x9D';

        /// <summary>
        /// Gets the OSC prefix character
        /// </summary>
        public const char OSCPrefixChar = ']';

        /// <summary>
        /// Gets the OSC escape sequence prefix
        /// </summary>
        public static readonly string OSCSequencePrefix = $"{EscapeChar}{OSCPrefixChar}";

        /// <summary>
        /// Gets the APC character
        /// </summary>
        public const char APCChar = '\x9F';

        /// <summary>
        /// Gets the APC prefix character
        /// </summary>
        public const char APCPrefixChar = '_';

        /// <summary>
        /// Gets the APC escape sequence prefix
        /// </summary>
        public static readonly string APCSequencePrefix = $"{EscapeChar}{APCPrefixChar}";

        /// <summary>
        /// Gets the DCS character
        /// </summary>
        public const char DCSChar = '\x90';

        /// <summary>
        /// Gets the DCS prefix character
        /// </summary>
        public const char DCSPrefixChar = 'P';

        /// <summary>
        /// Gets the DCS escape sequence prefix
        /// </summary>
        public static readonly string DCSSequencePrefix = $"{EscapeChar}{DCSPrefixChar}";

        /// <summary>
        /// Gets the PM character
        /// </summary>
        public const char PMChar = '\x9E';

        /// <summary>
        /// Gets the PM prefix character
        /// </summary>
        public const char PMPrefixChar = '^';

        /// <summary>
        /// Gets the PM escape sequence prefix
        /// </summary>
        public static readonly string PMSequencePrefix = $"{EscapeChar}{PMPrefixChar}";
    }
}
