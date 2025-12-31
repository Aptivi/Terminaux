//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

namespace Terminaux.Sequences
{
    /// <summary>
    /// Denotes the VT sequence type
    /// </summary>
    public enum VtSequenceType
    {
        /// <summary>
        /// No VT sequences
        /// </summary>
        None = 0,
        /// <summary>
        /// CSI VT sequences
        /// </summary>
        Csi = 1,
        /// <summary>
        /// OSC VT sequences
        /// </summary>
        Osc = 2,
        /// <summary>
        /// ESC VT sequences
        /// </summary>
        Esc = 4,
        /// <summary>
        /// APC VT sequences
        /// </summary>
        Apc = 8,
        /// <summary>
        /// DCS VT sequences
        /// </summary>
        Dcs = 16,
        /// <summary>
        /// PM VT sequences
        /// </summary>
        Pm = 32,
        /// <summary>
        /// C1 VT sequences
        /// </summary>
        C1 = 64,
        /// <summary>
        /// All VT sequences
        /// </summary>
        All = Csi + Osc + Esc + Apc + Dcs + Pm + C1,
    }
}
