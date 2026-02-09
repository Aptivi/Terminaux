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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Spinner cyclic renderer
    /// </summary>
    public class Spinner : SimpleCyclicWriter
    {
        private int step = 0;
        private readonly string[] spinners = [];
        private bool useColors = true;
        private Color fgColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color bgColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

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
        /// Renders a spinner
        /// </summary>
        /// <returns>A string representation of the spinner</returns>
        public override string Render()
        {
            // Get the spinner, increase a step, and check
            string spinner = Peek();
            step++;
            if (step >= spinners.Length)
                step = 0;

            // Return the spinner
            return
                (UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "") +
                (UseColors ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "") +
                spinner +
                (UseColors ? ConsoleColoring.RenderResetBackground() : "") +
                (UseColors ? ConsoleColoring.RenderResetForeground() : "");
        }

        internal string Peek() =>
            spinners[step];

        /// <summary>
        /// Makes a new spinner instance
        /// </summary>
        /// <param name="spinners">A list of spinners</param>
        public Spinner(string[] spinners)
        {
            // Check the spinners
            int lastWidth = 0;
            bool first = true;
            foreach (string spinner in spinners)
            {
                int width = ConsoleChar.EstimateCellWidth(spinner);
                if (width != lastWidth && !first)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_SIMPLE_SPINNER_EXCEPTION_NOTSAMEWIDTH"));
                lastWidth = width;
                first = false;
            }

            // Install them
            this.spinners = spinners;
        }
    }
}
