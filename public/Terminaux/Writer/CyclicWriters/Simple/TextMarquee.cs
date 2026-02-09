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

using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Sequences;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Text marquee that scrolls for long text similar to how music players work
    /// </summary>
    public class TextMarquee : SimpleCyclicWriter
    {
        internal string text = "";
        private bool pausing = false;
        private int delayCount = 0;
        private int leftIdx = 0;
        private int rightIdx = 0;
        private int textWidth = 0;
        private bool useColors = true;
        private Color fgColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color bgColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Text to render. All VT sequences and control characters are trimmed away.
        /// </summary>
        public string Text =>
            text;

        /// <summary>
        /// Width of the marquee
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Delay interval. The default is 30 ticks for 100 milliseconds, but you can adjust it, depending on the speed of the loop.
        /// </summary>
        public int Delay { get; set; } = 30;

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => fgColor;
            set => fgColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => bgColor;
            set => bgColor = value;
        }

        /// <summary>
        /// Renders a scrolling text marquee
        /// </summary>
        /// <returns>The result</returns>
        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append(
                (UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "") +
                (UseColors ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "")
            );
            if (textWidth > Width)
            {
                // Process the width
                int processedWidth = 0;
                int processedChars = 0;
                var renderedBuilder = new StringBuilder();
                for (int i = leftIdx; i <= leftIdx + Width && processedWidth < Width; i++)
                {
                    char c = i >= Text.Length || i < 0 ? ' ' : Text[i];
                    int charWidth = TextTools.GetCharWidth(c);
                    processedWidth += charWidth;
                    processedChars++;
                    renderedBuilder.Append(c);
                }
                while (processedWidth > Width)
                {
                    if (leftIdx >= 0)
                        break;
                    renderedBuilder.Remove(0, 1);
                    leftIdx++;
                    if (leftIdx == 0)
                        leftIdx--;
                    processedWidth--;
                }

                // Check the indexes
                if (leftIdx == 0 && rightIdx == 0)
                    rightIdx = processedChars - 1;

                // If pausing, stop for three seconds
                if (pausing)
                {
                    delayCount++;
                    if (delayCount == Delay)
                    {
                        delayCount = 0;
                        pausing = false;
                    }
                }

                // Adjust the marquee
                if (!pausing)
                {
                    leftIdx++;
                    rightIdx++;
                    if (leftIdx == Text.Length + 1)
                    {
                        rightIdx = -1;
                        leftIdx = -1 - processedWidth;
                    }
                    if (leftIdx == 0)
                        pausing = true;
                }
                builder.Append(renderedBuilder.ToString());
            }
            else
                builder.Append(Text);
            builder.Append(
                (UseColors ? ConsoleColoring.RenderResetBackground() : "") +
                (UseColors ? ConsoleColoring.RenderResetForeground() : "")
            );
            return builder.ToString();
        }

        /// <summary>
        /// Makes a new instance of text marquee
        /// </summary>
        /// <param name="text">Text to render. All VT sequences and control characters are trimmed away.</param>
        /// <param name="args">Arguments to format the string with</param>
        public TextMarquee(Mark? text = null, params object?[]? args)
        {
            this.text = VtSequenceTools.FilterVTSequences(((string)(text ?? "")).FormatString(args)).ReplaceAll(CharManager.GetAllControlChars(), "");
            textWidth = ConsoleChar.EstimateCellWidth(this.text);
        }
    }
}
