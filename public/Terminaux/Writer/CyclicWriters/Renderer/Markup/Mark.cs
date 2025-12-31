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

using Textify.General.Structures;

namespace Terminaux.Writer.CyclicWriters.Renderer.Markup
{
    /// <summary>
    /// String with console markup support
    /// </summary>
    public class Mark
    {
        private WideString markup = (WideString)"";

        /// <summary>
        /// Markup representation
        /// </summary>
        public WideString Markup
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
        /// Makes a <see cref="Mark"/> instance from a string implicitly
        /// </summary>
        /// <param name="markup">Markup representation</param>
        public static implicit operator Mark(string markup) =>
            new(markup);

        /// <summary>
        /// Makes a <see cref="Mark"/> instance from a wide string implicitly
        /// </summary>
        /// <param name="markup">Markup representation</param>
        public static implicit operator Mark(WideString markup) =>
            new(markup);

        /// <summary>
        /// Makes a string from the markup
        /// </summary>
        /// <param name="mark">Markup instance</param>
        public static implicit operator string(Mark mark) =>
            mark.ParseMarkup();

        /// <summary>
        /// Makes a wide string from the markup
        /// </summary>
        /// <param name="mark">Markup instance</param>
        public static implicit operator WideString(Mark mark) =>
            (WideString)mark.ParseMarkup();

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
            Markup = (WideString)markup;
        }
    }
}
