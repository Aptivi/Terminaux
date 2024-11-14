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

namespace Terminaux.Writer.CyclicWriters.Renderer.Markup
{
    /// <summary>
    /// String with console markup support
    /// </summary>
    public class Mark
    {
        private string markup = "";

        /// <summary>
        /// Markup representation
        /// </summary>
        public string Markup
        {
            get => markup;
            set
            {
                markup = value;
                MarkupTools.ParseMarkup(Markup);
            }
        }

        /// <summary>
        /// Parses the markup representation
        /// </summary>
        /// <returns>A string that can be written to the console with the parsed markup representations</returns>
        public string ParseMarkup() =>
            MarkupTools.ParseMarkup(Markup);

        /// <summary>
        /// Constructor without any argument to create a markup representation class without text
        /// </summary>
        public Mark()
        { }

        /// <summary>
        /// Constructor to create a markup representation class with specified text
        /// </summary>
        /// <param name="markup">Markup representation of a text</param>
        public Mark(string markup)
        {
            Markup = markup;
        }
    }
}
