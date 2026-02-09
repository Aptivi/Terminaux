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
using System.Globalization;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Aligned text renderable
    /// </summary>
    public class AlignedText : GraphicalCyclicWriter
    {
        private string text = "";
        private bool oneLine = false;
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private TextSettings settings = new();
        private bool useColors = true;
        private bool rainbow = false;
        private bool rainbowBg = false;
        private Decoration? decoration = null;

        /// <summary>
        /// Text to render
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
        /// Decorations to add an appetizing prefix and suffix to the text
        /// </summary>
        public Decoration? Decoration
        {
            get => decoration;
            set => decoration = value;
        }

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            int rainbowState = Rainbow ? RainbowBg ? 2 : 1 : 0;
            if (!OneLine)
                return RenderAligned(
                    Left, Top, Width, Text, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState, Decoration);
            else
            {
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, Width);
                return RenderAligned(
                    Left, Top, Width, sentences[0].Truncate(Width - 4), ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState, Decoration);
            }
        }

        internal static string RenderAligned(int left, int top, int width, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, TextAlignment alignment = TextAlignment.Left, int rainbowState = 0, Decoration? decoration = null)
        {
            try
            {
                // Decorate the text if needed
                var decoratedText = new StringBuilder(Text);
                if (decoration is not null)
                {
                    decoratedText.Insert(0,
                        decoration.RenderStart() +
                        $"{(useColor ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        $"{(useColor ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "")}"
                    );
                    decoratedText.Append(decoration.RenderEnd());
                }
                Text = decoratedText.ToString();

                // Process the sentences
                var aligned = new StringBuilder();
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, width);
                if (useColor)
                {
                    aligned.Append(
                        ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                        ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    StringBuilder finalSentence = new();
                    int consoleInfoX = TextWriterTools.DetermineTextAlignment(sentence, width, alignment, left);
                    if (rainbowState != 0)
                    {
                        var stringInfo = new StringInfo(sentence);
                        int length = stringInfo.LengthInTextElements;
                        for (int l = 0; l < length; l++)
                        {
                            string filteredString = stringInfo.SubstringByTextElements(l, 1);
                            double hueWidth = (double)l / length;
                            int hue = (int)(360 * hueWidth);
                            var color = new Color($"hsl:{hue};100;50", new()
                            {
                                UseTerminalPalette = false,
                            });
                            finalSentence.Append(
                                ConsoleColoring.RenderSetConsoleColor(rainbowState == 1 ? color : ForegroundColor) +
                                ConsoleColoring.RenderSetConsoleColor(rainbowState == 2 ? color : BackgroundColor, true) +
                                $"{filteredString}"
                            );
                        }
                    }
                    else
                        finalSentence.Append(sentence);
                    aligned.Append(TextWriterWhereColor.RenderWherePlain(finalSentence.ToString(), consoleInfoX, top + i, false));
                    finalSentence.Clear();
                }

                // Write the resulting buffer
                if (useColor)
                {
                    aligned.Append(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
                    );
                }
                return aligned.ToString();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the aligned text renderer
        /// </summary>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public AlignedText(Mark? text = null, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text ?? "", vars);
        }
    }
}
