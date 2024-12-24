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

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Animated text renderable
    /// </summary>
    public class AnimatedText : IStaticRenderable
    {
        private int top = 0;
        private int left = 0;
        private int width = 0;
        private int frame = 0;
        private string[] frames = [];
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private TextSettings settings = new();
        private bool useColors = true;
        private bool rainbow = false;
        private bool rainbowBg = false;

        /// <summary>
        /// Left position
        /// </summary>
        public int Left
        {
            get => left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set => top = value;
        }

        /// <summary>
        /// Width of the animated text
        /// </summary>
        public int Width
        {
            get => width;
            set => width = value;
        }

        /// <summary>
        /// Text frames
        /// </summary>
        public string[] TextFrames
        {
            get => frames;
            set => frames = value;
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
        /// Renders an animated text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            int rainbowState = Rainbow ? RainbowBg ? 2 : 1 : 0;
            try
            {
                var animated = new StringBuilder();
                string text = frames[frame];
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, width);
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    StringBuilder finalSentence = new();
                    int consoleInfoX = TextWriterTools.DetermineTextAlignment(sentence, width, Settings.Alignment, left);
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
                    animated.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                        TextWriterWhereColor.RenderWhere(finalSentence.ToString(), consoleInfoX, top + i, true, Settings.Alignment == TextAlignment.Left ? left + width : 0)
                    );
                    finalSentence.Clear();
                }

                // Write the resulting buffer
                if (UseColors)
                {
                    animated.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                frame++;
                if (frame >= TextFrames.Length)
                    frame = 0;
                return animated.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the animated text renderer
        /// </summary>
        public AnimatedText()
        { }
    }
}
