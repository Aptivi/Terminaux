//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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

using System.Text.RegularExpressions;

namespace Terminaux.Sequences
{
    /// <summary>
    /// VT sequence regular expressions
    /// </summary>
    public static class VtSequenceRegexes
    {
        private static readonly Regex csiRegex =
            new(@"(\x9B|\x1B\[)[0-?]*[ -\/]*[@-~]", RegexOptions.Compiled);
        private static readonly Regex oscRegex =
            new(@"(\x9D|\x1B\]).+(\x07|\x9c)", RegexOptions.Compiled);
        private static readonly Regex escRegex =
            new(@"\x1b [F-Nf-n]|\x1b#[3-8]|\x1b%[@Gg]|\x1b[()*+][A-Za-z0-9=`<>]|\x1b[()*+]""[>4?]|\x1b[()*+]%[0-6=]|\x1b[()*+]&[4-5]|\x1b[-.\/][ABFHLM]|\x1b[6-9Fcl-o=>\|\}~]", RegexOptions.Compiled);
        private static readonly Regex apcRegex =
            new(@"(\x9f|\x1b_).+\x9c", RegexOptions.Compiled);
        private static readonly Regex dcsRegex =
            new(@"(\x90|\x1bP).+\x9c", RegexOptions.Compiled);
        private static readonly Regex pmRegex =
            new(@"(\x9e|\x1b\^).+\x9c", RegexOptions.Compiled);
        private static readonly Regex c1Regex =
            new(@"\x1b[DEHMNOVWXYZ78]", RegexOptions.Compiled);
        private static readonly Regex allRegex =
            new(CSISequences + "|" + OSCSequences + "|" + ESCSequences + "|" + APCSequences + "|" + DCSSequences + "|" + PMSequences + "|" + C1Sequences, RegexOptions.Compiled);

        /// <summary>
        /// CSI sequences
        /// </summary>
        public static Regex CSISequences =>
            csiRegex;

        /// <summary>
        /// OSC sequences
        /// </summary>
        public static Regex OSCSequences =>
            oscRegex;

        /// <summary>
        /// ESC sequences
        /// </summary>
        public static Regex ESCSequences =>
            escRegex;

        /// <summary>
        /// APC sequences
        /// </summary>
        public static Regex APCSequences =>
            apcRegex;

        /// <summary>
        /// DCS sequences
        /// </summary>
        public static Regex DCSSequences =>
            dcsRegex;

        /// <summary>
        /// PM sequences
        /// </summary>
        public static Regex PMSequences =>
            pmRegex;

        /// <summary>
        /// C1 sequences
        /// </summary>
        public static Regex C1Sequences =>
            c1Regex;

        /// <summary>
        /// All VT sequences
        /// </summary>
        public static Regex AllVTSequences =>
            allRegex;
    }
}
