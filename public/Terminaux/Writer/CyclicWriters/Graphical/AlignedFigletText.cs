﻿//
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

using System;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
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
        private int top = 0;
        private string text = "";
        private bool oneLine = false;
        private int leftMargin = 0;
        private int rightMargin = 0;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private TextSettings settings = new();
        private bool customTop = false;
        private bool useColors = true;
        private bool rainbow = false;
        private bool rainbowBg = false;

        /// <summary>
        /// Top position
        /// </summary>
        public override int Top
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
        /// Whether to print all lines or only one line
        /// </summary>
        public bool OneLine
        {
            get => oneLine;
            set => oneLine = value;
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
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            int rainbowState = Rainbow ? RainbowBg ? 2 : 1 : 0;
            if (!OneLine)
                return RenderAligned(
                    Top, Font, Text, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, LeftMargin, RightMargin, rainbowState);
            else
            {
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(
                    Top, Font, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, LeftMargin, RightMargin, rainbowState);
            }
        }

        internal void UpdateInternalTop()
        {
            string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);

            // Install the values
            top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
        }

        internal static string RenderAligned(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, int rainbowState = 0, params object[] Vars)
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
                        ConsoleLogger.Warning("Fallback figlet exceeds ({0}, {1}) (max: {2}) (window: {3})", consoleX, consoleY, consoleMaxY, ConsoleWrapper.WindowHeight);
                        return AlignedText.RenderAligned(top, Text, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, rainbowState);
                    }
                    else
                    {
                        // Write the figlet.
                        ConsoleLogger.Warning("Figlet exceeds ({0}, {1}) (max: {2}) (window: {3})", consoleX, consoleY, consoleMaxY, ConsoleWrapper.WindowHeight);
                        string renderedFiglet = FigletTools.RenderFiglet(Text, figFontFallback, textMaxWidth);
                        return AlignedText.RenderAligned(consoleY, renderedFiglet, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, rainbowState);
                    }
                }
                else
                {
                    // Write the figlet.
                    string renderedFiglet = FigletTools.RenderFiglet(Text, FigletFont, textMaxWidth);
                    return AlignedText.RenderAligned(consoleY, renderedFiglet, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, rainbowState);
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
            UpdateInternalTop();
        }
    }
}
