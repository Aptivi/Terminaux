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
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Animated text renderable
    /// </summary>
    public class AnimatedText : GraphicalCyclicWriter
    {
        private int frame = 0;
        private string[] frames = [];
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private TextSettings settings = new();
        private bool useColors = true;
        private bool rainbow = false;
        private bool rainbowBg = false;

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
        public override string Render()
        {
            int rainbowState = Rainbow ? RainbowBg ? 2 : 1 : 0;
            try
            {
                var animated = new StringBuilder();
                string text = frames[frame];
                ConsoleLogger.Debug("Rendering animated text frame {0} / {1}", frame, frames.Length);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, Width);
                if (UseColors)
                {
                    animated.Append(
                        ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                        ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    StringBuilder finalSentence = new();
                    int consoleInfoX = TextWriterTools.DetermineTextAlignment(sentence, Width, Settings.Alignment, Left);
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
                                ConsoleColoring.RenderSetConsoleColor(rainbowState == 1 ? new Color($"hsl:{hue};100;50") : ForegroundColor) +
                                ConsoleColoring.RenderSetConsoleColor(rainbowState == 2 ? new Color($"hsl:{hue};100;50") : BackgroundColor, true) +
                                $"{filteredString}"
                            );
                        }
                    }
                    else
                        finalSentence.Append(sentence);
                    animated.Append(TextWriterWhereColor.RenderWherePlain(finalSentence.ToString(), consoleInfoX, Top + i, false, Settings.Alignment == TextAlignment.Left ? Left + Width : 0));
                    finalSentence.Clear();
                }

                // Write the resulting buffer
                if (UseColors)
                {
                    animated.Append(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
                    );
                }
                frame++;
                if (frame >= TextFrames.Length)
                    frame = 0;
                return animated.ToString();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
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
