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

namespace Terminaux.Writer.CyclicWriters.Renderer.Markup
{
    /// <summary>
    /// Markup parsing information
    /// </summary>
    public class MarkupInfo
    {
        internal int entranceIndex = -1;
        internal int exitIndex = -1;
        internal int nestLevel = -1;
        internal string represent = "";
        internal bool isEscape = false;

        /// <summary>
        /// Entrance index in the whole string
        /// </summary>
        public int EntranceIndex =>
            entranceIndex;

        /// <summary>
        /// Exit index in the whole string
        /// </summary>
        public int ExitIndex =>
            exitIndex;

        /// <summary>
        /// Nesting level
        /// </summary>
        public int NestLevel =>
            nestLevel;

        /// <summary>
        /// Representation delimited by spaces
        /// </summary>
        public string Representation =>
            represent;

        /// <summary>
        /// Whether this is an escape markup indicator or not
        /// </summary>
        public bool Escape =>
            isEscape;
    }
}
