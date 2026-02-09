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
using System.IO;
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Line handle renderable
    /// </summary>
    public class LineHandle : SimpleCyclicWriter
    {
        private string[] fileLines = [];
        private Color color = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private bool useColors = true;

        /// <summary>
        /// Text color
        /// </summary>
        public Color Color
        {
            get => color;
            set => color = value;
        }

        /// <summary>
        /// Whether the line handle is ranged or not (targeting a specific start column to a specific end column)
        /// </summary>
        public bool Ranged { get; set; }

        /// <summary>
        /// A specific line number
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// A specific position to either highlight or to start a range from
        /// </summary>
        public int SourcePosition { get; set; }

        /// <summary>
        /// A specific position to end a range to in ranged mode
        /// </summary>
        public int TargetPosition { get; set; }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Renders a line handle
        /// </summary>
        /// <returns>Rendered line handle text that will be used by the renderer</returns>
        public override string Render()
        {
            if (Ranged)
            {
                SourcePosition = Math.Min(SourcePosition, TargetPosition);
                TargetPosition = Math.Max(SourcePosition, TargetPosition);
                return RenderLineHandleRanged(fileLines, Position, SourcePosition, TargetPosition, Color, UseColors);
            }
            return RenderLineHandleRanged(fileLines, Position, SourcePosition, 0, Color, UseColors);
        }

        internal static string RenderLineHandleRanged(string[] Array, int LineNumber, int startPos, int endPos, Color color, bool useColor = true)
        {
            // Get the builder
            StringBuilder builder = new();

            // Get the line index from number
            if (LineNumber <= 0)
                LineNumber = 1;
            if (LineNumber > Array.Length)
                LineNumber = Array.Length;
            int LineIndex = LineNumber - 1;

            // Get the line
            string LineContent = Array[LineIndex];

            // Now, check the column numbers
            if (startPos < 0 || startPos > LineContent.Length)
                startPos = LineContent.Length;
            if (endPos < 0 || endPos > LineContent.Length)
                endPos = LineContent.Length;

            // Place the line and the column handle
            int RepeatBlanks = startPos - 1;
            int RepeatMarkers = endPos - startPos;
            int digits = ConsoleMisc.GetDigits(LineNumber);
            if (RepeatBlanks < 0)
                RepeatBlanks = 0;
            if (RepeatMarkers < 0)
                RepeatMarkers = 0;
            builder.AppendLine($"{(useColor ? ConsoleColoring.RenderSetConsoleColor(color) : "")} {LineNumber} | {LineContent}");
            builder.AppendLine($"{(useColor ? ConsoleColoring.RenderSetConsoleColor(color) : "")} {new string(' ', digits)} | {new string(' ', RepeatBlanks)}^{new string('~', RepeatMarkers)}");

            // Write the resulting buffer
            if (useColor)
            {
                builder.Append(
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }
            return builder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the line handle renderer
        /// </summary>
        /// <param name="filePath">Path to file to render</param>
        public LineHandle(string filePath)
        {
            fileLines = File.ReadAllLines(filePath);
        }

        /// <summary>
        /// Makes a new instance of the line handle renderer
        /// </summary>
        /// <param name="fileLines">File lines to render</param>
        public LineHandle(string[] fileLines)
        {
            this.fileLines = fileLines;
        }
    }
}
