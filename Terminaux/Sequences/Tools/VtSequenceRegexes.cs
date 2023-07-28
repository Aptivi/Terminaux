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
