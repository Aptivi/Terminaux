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

using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Decoration renderer
    /// </summary>
    public class Decoration : SimpleCyclicWriter
    {
        private string decorationStart = "";
        private string decorationEnd = "";
        private bool useColors = true;
        private Color fgColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color bgColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Start of the decoration
        /// </summary>
        public string Start
        {
            get => decorationStart;
            set => decorationStart = value;
        }

        /// <summary>
        /// End of the decoration
        /// </summary>
        public string End
        {
            get => decorationEnd;
            set => decorationEnd = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => fgColor;
            set => fgColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => bgColor;
            set => bgColor = value;
        }

        /// <summary>
        /// Renders a decoration (both start and end)
        /// </summary>
        /// <returns>A string representation of the decoration</returns>
        public override string Render() =>
            RenderStart() + RenderEnd();

        /// <summary>
        /// Renders a decoration (start)
        /// </summary>
        /// <returns>A string representation of the decoration</returns>
        public string RenderStart()
            => (UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "") +
               (UseColors ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "") +
               Start +
               (UseColors ? ConsoleColoring.RenderResetBackground() : "") +
               (UseColors ? ConsoleColoring.RenderResetForeground() : "");

        /// <summary>
        /// Renders a decoration (end)
        /// </summary>
        /// <returns>A string representation of the decoration</returns>
        public string RenderEnd()
            => (UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "") +
               (UseColors ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "") +
               End +
               (UseColors ? ConsoleColoring.RenderResetBackground() : "") +
               (UseColors ? ConsoleColoring.RenderResetForeground() : "");

        /// <summary>
        /// Makes a new decoration instance
        /// </summary>
        public Decoration()
        { }
    }
}
