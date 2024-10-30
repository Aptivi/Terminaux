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

using System;
using Terminaux.Writer.FancyWriters.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Asciinema renderable
    /// </summary>
    public class Asciinema : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private int frame = 0;
        private AsciinemaRepresentation representation;

        /// <summary>
        /// Left position
        /// </summary>
        public int Left
        {
            get => left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set => top = value;
        }

        /// <summary>
        /// Asciinema representation
        /// </summary>
        public AsciinemaRepresentation Representation
        {
            get => representation;
            set => representation = value;
        }

        /// <summary>
        /// Renders the current frame
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render() =>
            throw new NotImplementedException();

        /// <summary>
        /// Makes a new instance of the Asciinema player
        /// </summary>
        public Asciinema() =>
            throw new NotImplementedException();
    }
}
