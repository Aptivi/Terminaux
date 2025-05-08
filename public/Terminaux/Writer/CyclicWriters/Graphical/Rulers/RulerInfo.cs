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

using System.Diagnostics;

namespace Terminaux.Writer.CyclicWriters.Graphical.Rulers
{
    /// <summary>
    /// Ruler information
    /// </summary>
    [DebuggerDisplay("({Position}, {Orientation})")]
    public struct RulerInfo
    {
        private int position;

        /// <summary>
        /// Zero-based position of the ruler relative to the supported cyclic writer or its inside
        /// </summary>
        /// <remarks>
        /// On <see cref="RulerOrientation.Horizontal"/>, the position refers to the horizontal position that determines a top position of a ruler relative to the enclosed cyclic writer or its inside<br></br>
        /// On <see cref="RulerOrientation.Vertical"/>, the position refers to the vertical position that determines a left position of a ruler relative to the enclosed cyclic writer or its inside
        /// </remarks>
        public int Position
        {
            readonly get => position;
            set
            {
                if (position >= 0)
                    position = value;
            }
        }

        /// <summary>
        /// Orientation of the ruler
        /// </summary>
        public RulerOrientation Orientation { get; set; }

        /// <summary>
        /// Makes a new ruler info struct
        /// </summary>
        /// <param name="position">Zero-based position of the ruler relative to the supported cyclic writer or its inside</param>
        /// <param name="orientation">Orientation of the ruler</param>
        public RulerInfo(int position, RulerOrientation orientation)
        {
            this.position = position;
            Orientation = orientation;
        }
    }
}
