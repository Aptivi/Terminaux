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

using Terminaux.Base.Structures;

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
            get => left + SetMargins.Margins.Left + Padding.Left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public virtual int Top
        {
            get => top + SetMargins.Margins.Top + Padding.Top;
            set => top = value;
        }

        /// <summary>
        /// Element width
        /// </summary>
        public virtual int Width
        {
            get => SetMargins.Width;
            set => width = value;
        }

        /// <summary>
        /// Element height
        /// </summary>
        public virtual int Height
        {
            get => SetMargins.Height;
            set => height = value;
        }

        /// <summary>
        /// Renderable padding
        /// </summary>
        public Padding Padding { get; set; }

        /// <summary>
        /// Renderable margins
        /// </summary>
        public Padding Margins { get; set; }

        /// <summary>
        /// Final margins to use (set <see cref="Margins"/> to set this property to adjust to the required width and height)
        /// </summary>
        public Margin SetMargins =>
            new(Margins, width, height);
    }
}
