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
        private int intersectionStop;

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
        /// Whether to invert the rendering direction or not
        /// </summary>
        /// <remarks>
        /// If <see cref="Orientation"/> is set to <see cref="RulerOrientation.Horizontal"/>, the renderer will render a ruler from the right to the left if inversion is enabled, or from the left to the right if inversion is disabled.<br></br>
        /// If <see cref="Orientation"/> is set to <see cref="RulerOrientation.Vertical"/>, the renderer will render a ruler from the bottom to the top if inversion is enabled, or from the top to the bottom if inversion is disabled.
        /// </remarks>
        public bool InvertDirection { get; set; }

        /// <summary>
        /// Defines a stop level when defining intersections (skip n intersections)
        /// </summary>
        /// <remarks>
        /// If set to 0, no stops will occur. Otherwise, the renderer will render intersections a specified number of times.
        /// </remarks>
        public int IntersectionStop
        {
            readonly get => intersectionStop;
            set
            {
                if (intersectionStop >= 0)
                    intersectionStop = value;
            }
        }

        /// <summary>
        /// Makes a new ruler info struct
        /// </summary>
        /// <param name="position">Zero-based position of the ruler relative to the supported cyclic writer or its inside</param>
        /// <param name="orientation">Orientation of the ruler</param>
        /// <param name="intersectionStop">Stop level for intersections</param>
        /// <param name="invertDirection">Whether to invert the rendering direction or not</param>
        public RulerInfo(int position, RulerOrientation orientation, int intersectionStop = 0, bool invertDirection = false)
        {
            this.position = position;
            Orientation = orientation;
            this.intersectionStop = intersectionStop;
            InvertDirection = invertDirection;
        }
    }
}
