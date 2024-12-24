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

using System.Linq;
using Textify.General;

namespace Terminaux.Inputs.Presentation.Elements
{
    /// <summary>
    /// Text element
    /// </summary>
    public class TextElement : IElement
    {
        /// <summary>
        /// The first argument denotes the text to be written, and the rest for the parameters to be formatted
        /// </summary>
        public object[]? Arguments { get; set; }

        /// <summary>
        /// Renders the text
        /// </summary>
        public string RenderToString()
        {
            // Get the text and the arguments
            object[] finalArgs = Arguments is not null && Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];
            string text = TextTools.FormatString((string)(Arguments is not null && Arguments.Length > 0 ? Arguments[0] : ""), finalArgs);
            return text;
        }
    }
}
