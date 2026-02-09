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

using System;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;
using Textify.Data.Figlet.Utilities.Lines;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Aligned figlet text renderable
    /// </summary>
    public class AlignedFigletText : GraphicalCyclicWriter
    {
        private FigletFont figletFont = FigletFonts.GetByName("small");
        private string text = "";
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private TextSettings settings = new();
        private bool useColors = true;
        private bool rainbow = false;
        private bool rainbowBg = false;

        /// <summary>
        /// Top position
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
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
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Whether to write text with rainbow effects or not
        /// </summary>
        public bool Rainbow
        {
            get => rainbow;
            set => rainbow = value;
        }

        /// <summary>
        /// Whether to write text with rainbow effects in the background or in the foreground
        /// </summary>
        public bool RainbowBg
        {
            get => rainbowBg;
            set => rainbowBg = value;
        }

        /// <summary>
        /// Whether to render the figlet text in one line or not
        /// </summary>
        public bool OneLine { get; set; } = true;

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            try
            {
                int rainbowState = Rainbow ? RainbowBg ? 2 : 1 : 0;
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, Font);
                int figHeight = FigletTools.GetFigletHeight(Text, Font, Width);
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback);
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback, Width);
                int markedX = Left;
                int markedY = Top;
                int markedEndX = markedX + figWidth;
                int markedEndY = markedY + figHeight;
                int markedEndFallX = markedX + figWidthFallback;
                int markedEndFallY = markedY + figHeightFallback;

                // Determine whether to use the selected figlet font or resort to fallbacks
                if (markedEndFallX >= Width + markedX && (OneLine || (!OneLine && markedEndFallY >= Height + markedX)))
                {
                    // The fallback figlet won't fit, so use smaller text
                    string text = Text;
                    if (OneLine)
                    {
                        string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, Width);
                        text = sentences[0].Truncate(Width - 4);
                    }
                    ConsoleLogger.Warning("Fallback figlet exceeds (reason: {0}, {1}) (renderable: {2}x{3})", markedEndFallX, markedEndFallY, Width, Height);
                    return AlignedText.RenderAligned(markedX, markedY, Width, text, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState);
                }
                else if (markedEndX >= Width + markedX && (OneLine || (!OneLine && markedEndY >= Height + markedX)))
                {
                    // The figlet won't fit, so use small text
                    ConsoleLogger.Warning("Figlet exceeds (reason: {0}, {1}) (renderable: {2}x{3})", markedEndX, markedEndY, Width, Height);
                    string renderedFiglet = FigletTools.RenderFiglet(Text, figFontFallback, Width);
                    return AlignedText.RenderAligned(markedX, markedY, Width, renderedFiglet, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState);
                }
                else
                {
                    // Write the figlet.
                    string renderedFiglet = FigletTools.RenderFiglet(Text, Font, Width);
                    return AlignedText.RenderAligned(markedX, markedY, Width, renderedFiglet, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState);
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the aligned figlet text renderer
        /// </summary>
        /// <param name="figletFont">Figlet font to render with</param>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public AlignedFigletText(FigletFont figletFont, Mark? text = null, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text ?? "", vars);
            this.figletFont = figletFont;
        }
    }
}
