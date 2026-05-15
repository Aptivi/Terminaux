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
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Aligned cowsay text renderable
    /// </summary>
    public class AlignedCowsayText : GraphicalCyclicWriter
    {
        private CowName cowsayCow = CowName.Default;
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
        /// Cow to render the text with
        /// </summary>
        public CowName Cow
        {
            get => cowsayCow;
            set => cowsayCow = value;
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
        /// Cowsay speak mode
        /// </summary>
        public CowSayMode CowSayMode { get; set; }

        /// <summary>
        /// Renders an aligned cowsay text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            try
            {
                bool think = CowSayMode == CowSayMode.Think;
                int rainbowState = Rainbow ? RainbowBg ? 2 : 1 : 0;
                var cowFallback = CowName.Default;
                var cowLines = CowsayTools.GetCowsayLines(Text, Cow, think, Width);
                int cowWidth = CowsayTools.GetCowsayWidth(Text, Cow, think);
                int cowHeight = CowsayTools.GetCowsayHeight(Text, Cow, think, Width);
                var cowLinesFallback = CowsayTools.GetCowsayLines(Text, cowFallback, think, Width);
                int cowWidthFallback = CowsayTools.GetCowsayWidth(Text, cowFallback, think);
                int cowHeightFallback = CowsayTools.GetCowsayHeight(Text, cowFallback, think, Width);
                string renderedCowsay = string.Join("\n", cowLines);
                string renderedCowsayFallback = string.Join("\n", cowLinesFallback);
                int markedX = Left > 0 ? Left : 0;
                int markedY = Top > 0 ? Top : 0;
                int markedEndX = markedX + cowWidth;
                int markedEndY = markedY + cowHeight;
                int markedEndFallX = markedX + cowWidthFallback;
                int markedEndFallY = markedY + cowHeightFallback;

                // Determine whether to use the selected cow or resort to fallbacks
                if (markedEndFallX >= Width + markedX || markedEndFallY >= Height + markedY)
                {
                    // The fallback cowsay won't fit, so use smaller text
                    string text = Text;
                    ConsoleLogger.Warning("Fallback cowsay exceeds (reason: {0}, {1}) (renderable: {2}x{3})", markedEndFallX, markedEndFallY, Width, Height);
                    return AlignedText.RenderAligned(markedX, markedY, Width, text, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState);
                }
                else if (markedEndX >= Width + markedX || markedEndY >= Height + markedY)
                {
                    // The cowsay won't fit, so use small text
                    ConsoleLogger.Warning("Cowsay exceeds (reason: {0}, {1}) (renderable: {2}x{3})", markedEndX, markedEndY, Width, Height);
                    return AlignedText.RenderAligned(markedX, markedY, Width, renderedCowsayFallback, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState);
                }
                else
                {
                    // Write the cowsay.
                    return AlignedText.RenderAligned(markedX, markedY, Width, renderedCowsay, ForegroundColor, BackgroundColor, UseColors, Settings.Alignment, rainbowState);
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the aligned cowsay text renderer
        /// </summary>
        /// <param name="cowsayCow">Cow to render with</param>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public AlignedCowsayText(CowName cowsayCow, Mark? text = null, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text ?? "", vars);
            this.cowsayCow = cowsayCow;
        }
    }
}
