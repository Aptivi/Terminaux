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
using System.Globalization;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Aligned text renderable
    /// </summary>
    public class AlignedText : IStaticRenderable
    {
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
        private Decoration? decoration = null;

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
        /// Text to render
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
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            int rainbowState = Rainbow ? RainbowBg ? 2 : 1 : 0;
            if (!OneLine)
                return RenderAligned(
                    Top, Text, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, LeftMargin, RightMargin, rainbowState, Decoration);
            else
            {
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(
                    Top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, LeftMargin, RightMargin, rainbowState, Decoration);
            }
        }

        internal void UpdateInternalTop()
        {
            string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);

            // Install the values
            top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
        }

        internal static string RenderAligned(int top, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, int rainbowState = 0, Decoration? decoration = null, params object[] Vars)
        {
            try
            {
                // Decorate the text if needed
                var decoratedText = new StringBuilder(Text.FormatString(Text, Vars));
                if (decoration is not null)
                {
                    decoratedText.Insert(0,
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}"
                    );
                    decoratedText.Insert(0, decoration.RenderStart());
                    decoratedText.Append(decoration.RenderEnd());
                }
                Text = decoratedText.ToString();

                // Process the sentences
                var aligned = new StringBuilder();
                int width = ConsoleWrapper.WindowWidth - rightMargin - leftMargin;
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, width);
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    StringBuilder finalSentence = new();
                    int consoleInfoX = TextWriterTools.DetermineTextAlignment(sentence, width, alignment, leftMargin);
                    if (rainbowState != 0)
                    {
                        var stringInfo = new StringInfo(sentence);
                        int length = stringInfo.LengthInTextElements;
                        for (int l = 0; l < length; l++)
                        {
                            string filteredString = stringInfo.SubstringByTextElements(l, 1);
                            double hueWidth = (double)l / length;
                            int hue = (int)(360 * hueWidth);
                            finalSentence.Append(
                                ColorTools.RenderSetConsoleColor(rainbowState == 1 ? new Color($"hsl:{hue};100;50") : ForegroundColor) +
                                ColorTools.RenderSetConsoleColor(rainbowState == 2 ? new Color($"hsl:{hue};100;50") : BackgroundColor, true) +
                                $"{filteredString}"
                            );
                        }
                    }
                    else
                        finalSentence.Append(sentence);
                    aligned.Append(
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                        TextWriterWhereColor.RenderWhere(finalSentence.ToString(), consoleInfoX, top + i, true, alignment == TextAlignment.Left ? rightMargin : 0)
                    );
                    finalSentence.Clear();
                }

                // Write the resulting buffer
                if (useColor)
                {
                    aligned.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                return aligned.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
            UpdateInternalTop();
        }
    }
}
