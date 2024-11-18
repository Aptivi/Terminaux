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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Textify.Data.Figlet;
using Textify.Data.Figlet.Utilities.Lines;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Figlet text renderable
    /// </summary>
    public class FigletText : IStaticRenderable
    {
        private FigletFont figletFont = FigletFonts.GetByName("small");
        private int left = 0;
        private int top = 0;
        private string text = "";
        private int leftMargin = 0;
        private int rightMargin = 0;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private bool customPos = false;
        private bool useColors = true;

        /// <summary>
        /// Left position
        /// </summary>
        [Obsolete("FigletWhereColor is going to be obsolete soon. Use renderable positioning system instead.")]
        public int Left
        {
            get => left;
            set
            {
                left = value;
                customPos = true;
            }
        }

        /// <summary>
        /// Top position
        /// </summary>
        [Obsolete("FigletWhereColor is going to be obsolete soon. Use renderable positioning system instead.")]
        public int Top
        {
            get => top;
            set
            {
                top = value;
                customPos = true;
            }
        }

        /// <summary>
        /// Top position
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                if (!customPos)
                    UpdateInternalTop();
            }
        }

        /// <summary>
        /// Left margin of the aligned figlet text
        /// </summary>
        public int LeftMargin
        {
            get => leftMargin;
            set
            {
                leftMargin = value;
                if (!customPos)
                    UpdateInternalTop();
            }
        }

        /// <summary>
        /// Right margin of the aligned figlet text
        /// </summary>
        public int RightMargin
        {
            get => rightMargin;
            set
            {
                rightMargin = value;
                if (!customPos)
                    UpdateInternalTop();
            }
        }

        /// <summary>
        /// Foreground color of the text
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color of the text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
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
        /// Figlet font to render the text with
        /// </summary>
        public FigletFont Font
        {
            get => figletFont;
            set => figletFont = value;
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return RenderFiglet(
                Text, Font, ForegroundColor, BackgroundColor, UseColors, LeftMargin, RightMargin);
        }

        internal void UpdateInternalTop()
        {
            string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);

            // Install the values
            top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
        }

        internal static string RenderFiglet(string Text, FigletFont FigletFont, Color ForegroundColor, Color BackgroundColor, bool useColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var builder = new StringBuilder();
            builder.Append(
                $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                FigletTools.RenderFiglet(Text, FigletFont, ConsoleWrapper.WindowWidth - (leftMargin + rightMargin), Vars) +
                $"{(useColor ? ColorTools.RenderRevertForeground() : "")}" +
                $"{(useColor ? ColorTools.RenderRevertBackground() : "")}"
            );
            return builder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the aligned figlet text renderer
        /// </summary>
        /// <param name="figletFont">Figlet font to render with</param>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public FigletText(FigletFont figletFont, Mark text, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text, vars);
            this.figletFont = figletFont;
            UpdateInternalTop();
        }
    }
}
