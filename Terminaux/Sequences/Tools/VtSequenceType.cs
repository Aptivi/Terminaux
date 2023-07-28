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
    /// Denotes the VT sequence type
    /// </summary>
    public enum VtSequenceType
    {
        /// <summary>
        /// No VT sequence
        /// </summary>
        None = 0,
        /// <summary>
        /// VT sequence is one of the CSI sequences
        /// </summary>
        Csi = 1,
        /// <summary>
        /// VT sequence is one of the OSC sequences
        /// </summary>
        Osc = 2,
        /// <summary>
        /// VT sequence is one of the ESC sequences
        /// </summary>
        Esc = 4,
        /// <summary>
        /// VT sequence is one of the APC sequences
        /// </summary>
        Apc = 8,
        /// <summary>
        /// VT sequence is one of the DCS sequences
        /// </summary>
        Dcs = 16,
        /// <summary>
        /// VT sequence is one of the PM sequences
        /// </summary>
        Pm = 32,
        /// <summary>
        /// VT sequence is one of the C1 sequences
        /// </summary>
        C1 = 64,
        /// <summary>
        /// All VT sequences
        /// </summary>
        All = Csi + Osc + Esc + Apc + Dcs + Pm + C1,
    }
}
