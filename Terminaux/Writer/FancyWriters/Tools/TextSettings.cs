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

using Terminaux.Writer.MiscWriters.Tools;

namespace Terminaux.Writer.FancyWriters.Tools
{
    /// <summary>
    /// Text settings to manipulate writing
    /// </summary>
    public class TextSettings
    {
        private static readonly TextSettings globalSettings = new();

        /// <summary>
        /// Global text settings
        /// </summary>
        public static TextSettings GlobalSettings =>
            globalSettings;

        /// <summary>
        /// Specifies a text alignment
        /// </summary>
        public TextAlignment Alignment { get; set; } = TextAlignment.Left;
    }
}
