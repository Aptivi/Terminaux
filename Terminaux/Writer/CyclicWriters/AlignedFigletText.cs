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
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.FancyWriters;
using Textify.Data.Figlet;
using Textify.Data.Figlet.Utilities.Lines;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Aligned figlet text renderable
    /// </summary>
    public class AlignedFigletText : IStaticRenderable
    {
        private FigletFont figletFont = FigletFonts.GetByName("small");
        private int top = 0;
        private string text = "";
        private bool oneLine = false;
        private int leftMargin = 0;
        private int rightMargin = 0;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private TextSettings settings = new();
        private bool customTop = false;
        private bool customColor = false;

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set
            {
                top = value;
                customTop = true;
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
                if (!customTop)
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
                if (!customTop)
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
                if (!customTop)
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
        /// Text settings to use
        /// </summary>
        public TextSettings Settings
        {
            get => settings;
            set => settings = value;
        }

        /// <summary>
        /// Whether to print all lines or only one line
        /// </summary>
        public bool OneLine
        {
            get => oneLine;
            set => oneLine = value;
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            if (!OneLine)
                return RenderAligned(
                    Top, Font, Text, ForegroundColor, BackgroundColor, customColor, Settings.Alignment, LeftMargin, RightMargin);
            else
            {
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(
                    Top, Font, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor, customColor, Settings.Alignment, LeftMargin, RightMargin);
            }
        }

        internal void UpdateInternalTop()
        {
            string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);

            // Install the values
            top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
        }

        internal static string RenderAligned(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
                int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                int consoleMaxY = top + figHeight;
                int textMaxWidth = ConsoleWrapper.WindowWidth - (leftMargin + consoleX + rightMargin);
                if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                {
                    // The figlet won't fit, so use small text
                    consoleX = ConsoleWrapper.WindowWidth / 2 - figWidthFallback;
                    consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                    consoleMaxY = top + figHeightFallback;
                    if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        return AlignedText.RenderAligned(top, Text, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin);
                    }
                    else
                    {
                        // Write the figlet.
                        string renderedFiglet = FigletTools.RenderFiglet(Text, figFontFallback, textMaxWidth);
                        return AlignedText.RenderAligned(consoleY, renderedFiglet, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin);
                    }
                }
                else
                {
                    // Write the figlet.
                    string renderedFiglet = FigletTools.RenderFiglet(Text, FigletFont, textMaxWidth);
                    return AlignedText.RenderAligned(consoleY, renderedFiglet, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the aligned figlet text renderer
        /// </summary>
        /// <param name="figletFont">Figlet font to render with</param>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public AlignedFigletText(FigletFont figletFont, string text, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text, vars);
            this.figletFont = figletFont;
            UpdateInternalTop();
        }
    }
}
