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

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Graphical cyclic writer (for GUI implementations to the CLI, such as charts)
    /// </summary>
    public abstract class GraphicalCyclicWriter : CyclicWriter
    {
        private int left = 0;
        private int top = 0;
        private int width = 0;
        private int height = 0;

        /// <summary>
        /// Left position
        /// </summary>
        public virtual int Left
        {
            get => left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public virtual int Top
        {
            get => top;
            set => top = value;
        }

        /// <summary>
        /// Element width
        /// </summary>
        public virtual int Width
        {
            get => width;
            set => width = value;
        }

        /// <summary>
        /// Element height
        /// </summary>
        public virtual int Height
        {
            get => height;
            set => height = value;
        }
    }
}
