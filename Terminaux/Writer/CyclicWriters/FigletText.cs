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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
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
        private bool customColor = false;

        /// <summary>
        /// Left position
        /// </summary>
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
            set
            {
                foregroundColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Background color of the text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                customColor = true;
            }
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
            if (!customPos)
                return FigletColor.RenderFiglet(
                    Text, Font, ForegroundColor, BackgroundColor, customColor, LeftMargin, RightMargin);
            else
                return FigletWhereColor.RenderFigletWhere(
                    Text, Left, Top, false, Font, ForegroundColor, BackgroundColor, customColor, LeftMargin, RightMargin);
        }

        internal void UpdateInternalTop()
        {
            string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);

            // Install the values
            top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
        }

        /// <summary>
        /// Makes a new instance of the aligned figlet text renderer
        /// </summary>
        /// <param name="figletFont">Figlet font to render with</param>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public FigletText(FigletFont figletFont, string text, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text, vars);
            this.figletFont = figletFont;
            UpdateInternalTop();
        }
    }
}
