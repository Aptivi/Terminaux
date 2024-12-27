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

using Textify.Figlet;
using Textify.Figlet.Utilities.Lines;

namespace Terminaux.Writer.FancyWriters.Tools
{
    /// <summary>
    /// Tools for text manipulation
    /// </summary>
    public static class FigletTextTools
    {
        internal static string defaultFigletFontName = "speed";

        /// <summary>
        /// Default figlet font name
        /// </summary>
        public static string DefaultFigletFontName
        {
            get => defaultFigletFontName ?? "speed";
            set => defaultFigletFontName = FigletFonts.TryGetByName(defaultFigletFontName) is not null ? value : "speed";
        }

        /// <summary>
        /// Default figlet font
        /// </summary>
        public static FigletFont DefaultFigletFont =>
            FigletFonts.GetByName(DefaultFigletFontName);
    }
}
