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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Textify.General.Structures;
using Terminaux.Shell.Shells.Unified;
using Textify.General;
using System.Collections.Generic;
using System.Text;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Prosery cyclic renderer
    /// </summary>
    public class Prosery : SimpleCyclicWriter
    {
        private WideChar padChar = (WideChar)"▒";
        private bool useColors = true;
        private Color fgColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color padColor = ThemeColorsTools.GetColor(ThemeColorType.Separator);
        private Color bgColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Prosery padding character at the beginning of each line
        /// </summary>
        public WideChar PadChar
        {
            get => padChar;
            set => padChar = value;
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
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => fgColor;
            set => fgColor = value;
        }

        /// <summary>
        /// Prose pad color
        /// </summary>
        public Color PadColor
        {
            get => padColor;
            set => padColor = value;
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
        /// Prose to render to the console
        /// </summary>
        public string Prose { get; set; } = "";

        /// <summary>
        /// Whether to wrap lines or not
        /// </summary>
        public bool WrapLines { get; set; } = true;

        /// <summary>
        /// Line wrap width for each line
        /// </summary>
        public int WrapWidth { get; set; } = 75;

        /// <summary>
        /// Renders a spinner
        /// </summary>
        /// <returns>A string representation of the spinner</returns>
        public override string Render()
        {
            // First, process the prose
            string[] lines = ProcessProse();

            // Now, append prose padding
            var proseBuilder = new StringBuilder();
            string prosePadding = $"{PadChar}  ";
            foreach (var line in lines)
            {
                // Add padding
                proseBuilder.Append(
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(PadColor) : "") +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "") +
                    prosePadding
                );

                // Add actual line
                proseBuilder.Append(
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "") +
                    line
                );

                // Reset colors
                proseBuilder.AppendLine(
                    (UseColors ? ConsoleColoring.RenderResetBackground() : "") +
                    (UseColors ? ConsoleColoring.RenderResetForeground() : "")
                );
            }

            // Return the spinner
            return proseBuilder.ToString();
        }

        private string[] ProcessProse()
        {
            // Find the new lines and split them, eliminating blank lines in the process
            string[] paragraphs = Prose.SplitNewLines();

            // Check to see if wrapping is required
            List<string> proseLines = [];
            bool emptyLineAdded = false;
            for (int i = 0; i < paragraphs.Length; i++)
            {
                string line = paragraphs[i];

                // If wrapping is required, wrap by words, taking VT escape sequences, CJK, and emojis into account
                if (!string.IsNullOrWhiteSpace(line))
                {
                    emptyLineAdded = false;
                    if (WrapLines)
                    {
                        string[] splitLines = ConsoleMisc.GetWrappedSentencesByWords(line, WrapWidth);
                        proseLines.AddRange(splitLines);
                    }
                    else
                        proseLines.Add(line);
                }

                // Add empty line if this line is not the last one
                if (i + 1 < paragraphs.Length && string.IsNullOrWhiteSpace(line) && !emptyLineAdded)
                {
                    emptyLineAdded = true;
                    proseLines.Add("");
                }
            }

            // Trim the lines from the top and the bottom
            for (int i = proseLines.Count - 1; i >= 0; i--)
            {
                string line = proseLines[i];

                // If this line is empty, remove it
                if (string.IsNullOrWhiteSpace(line))
                    proseLines.RemoveAt(i);
                else
                    break;
            }
            while (proseLines.Count > 0)
            {
                string line = proseLines[0];

                // If this line is empty, remove it
                if (string.IsNullOrWhiteSpace(line))
                    proseLines.RemoveAt(0);
                else
                    break;
            }

            // Return the result
            return [.. proseLines];
        }
    }
}
