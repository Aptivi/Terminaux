
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
