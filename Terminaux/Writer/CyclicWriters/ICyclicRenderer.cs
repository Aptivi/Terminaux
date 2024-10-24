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

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Cyclic renderer
    /// </summary>
    public interface ICyclicRenderer
    {
        /// <summary>
        /// Renders an element
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="args">Arguments to format text</param>
        /// <returns>Rendered text that will be used by the cyclic renderer</returns>
        public string Render(string text, params object?[]? args);
    }
}
