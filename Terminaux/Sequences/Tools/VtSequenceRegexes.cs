
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

namespace Terminaux.Sequences.Tools
{
    /// <summary>
    /// VT sequence regular expressions
    /// </summary>
    public static class VtSequenceRegexes
    {
        /// <summary>
        /// CSI sequences
        /// </summary>
        public static string CSISequences { get => @"(\x9B|\x1B\[)[0-?]*[ -\/]*[@-~]"; }

        /// <summary>
        /// OSC sequences
        /// </summary>
        public static string OSCSequences { get => @"(\x9D|\x1B\]).+(\x07|\x9c)"; }

        /// <summary>
        /// ESC sequences
        /// </summary>
        public static string ESCSequences { get => @"\x1b [F-Nf-n]|\x1b#[3-8]|\x1b%[@Gg]|\x1b[()*+][A-Za-z0-9=`<>]|\x1b[()*+]""[>4?]|\x1b[()*+]%[0-6=]|\x1b[()*+]&[4-5]|\x1b[-.\/][ABFHLM]|\x1b[6-9Fcl-o=>\|\}~]"; }

        /// <summary>
        /// APC sequences
        /// </summary>
        public static string APCSequences { get => @"(\x9f|\x1b_).+\x9c"; }

        /// <summary>
        /// DCS sequences
        /// </summary>
        public static string DCSSequences { get => @"(\x90|\x1bP).+\x9c"; }

        /// <summary>
        /// PM sequences
        /// </summary>
        public static string PMSequences { get => @"(\x9e|\x1b\^).+\x9c"; }

        /// <summary>
        /// C1 sequences
        /// </summary>
        public static string C1Sequences { get => @"\x1b[DEHMNOVWXYZ78]"; }

        /// <summary>
        /// All VT sequences
        /// </summary>
        public static string AllVTSequences { get => CSISequences + "|" + OSCSequences + "|" + ESCSequences + "|" + APCSequences + "|" + DCSSequences + "|" + PMSequences + "|" + C1Sequences; }
    }
}
