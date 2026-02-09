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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Sequences;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Bounded text renderable
    /// </summary>
    public class BoundedText : GraphicalCyclicWriter
    {
        private string text = "";
        private bool positionWise = false;
        private int width = 0;
        private int height = 0;
        private int lineIdx = 0;
        private int columnIdx = 0;
        private int rowIdx = 0;
        private int incrementRate = 0;
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private TextSettings settings = new();
        private bool customSize = false;
        private bool useColors = true;

        /// <summary>
        /// A text to render
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Left margin of the aligned figlet text
        /// </summary>
        public override int Width
        {
            get => width;
            set
            {
                width = value;
                customSize = true;
            }
        }

        /// <summary>
        /// Right margin of the aligned figlet text
        /// </summary>
        public override int Height
        {
            get => height;
            set
            {
                height = value;
                customSize = true;
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
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Whether to form a bounded text position-wise or line-wise
        /// </summary>
        public bool PositionWise
        {
            get => positionWise;
            set => positionWise = value;
        }

        /// <summary>
        /// [Position-wise] Specifies a zero-based row index
        /// </summary>
        public int Row
        {
            get => rowIdx;
            set => rowIdx = value;
        }

        /// <summary>
        /// [Position-wise] Specifies a zero-based column index
        /// </summary>
        public int Column
        {
            get => columnIdx;
            set => columnIdx = value;
        }

        /// <summary>
        /// [Line-wise] Specifies a zero-based line index
        /// </summary>
        public int Line
        {
            get => lineIdx;
            set => lineIdx = value;
        }

        /// <summary>
        /// [Line-wise] Specifies an incrementation rate
        /// </summary>
        public int IncrementRate =>
            incrementRate;

        /// <summary>
        /// Renders a bounded text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            if (!customSize)
                UpdateInternalSize();
            string[] lines = TextWriterTools.GetFinalLines(Text, width);
            ConsoleLogger.Debug("Bounded text width x height (poswise: {0}): {1}, {2}", PositionWise, Width, Height);
            if (PositionWise)
                return RenderTextPoswise(
                    lines, Settings, ForegroundColor, BackgroundColor, Width, Height, Left, Top, UseColors, Row, Column);
            else
                return RenderTextLinewise(
                    lines, Settings, ForegroundColor, BackgroundColor, Width, Height, Left, Top, UseColors, Line, ref incrementRate);
        }

        internal void UpdateInternalSize()
        {
            width = ConsoleWrapper.WindowWidth;
            height = ConsoleWrapper.WindowHeight;
        }

        internal static string RenderTextLinewise(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, int currIdx, ref int increment)
        {
            int linesMade = 0;
            var buffer = new StringBuilder();
            if (useColor)
            {
                buffer.Append(
                    $"{ConsoleColoring.RenderSetConsoleColor(textColor)}" +
                    $"{ConsoleColoring.RenderSetConsoleColor(backgroundColor, true)}"
                );
            }
            for (int i = currIdx; i < lines.Length; i++)
            {
                var line = lines[i];
                int posX = TextWriterTools.DetermineTextAlignment(line, width, settings.Alignment, left);
                if (linesMade % height == 0 && linesMade > 0)
                {
                    // Reached the end of the box. Bail.
                    increment = linesMade;
                    break;
                }
                buffer.Append(TextWriterWhereColor.RenderWherePlain(line, posX, top + linesMade % height, false));
                linesMade++;
            }
            if (useColor)
            {
                buffer.Append(
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }
            return buffer.ToString();
        }

        internal static string RenderTextPoswise(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, int currIdxRow, int currIdxColumn)
        {
            // Get the start and the end indexes for lines
            int lineLinesPerPage = height;
            int currentPage = currIdxRow / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage + 1;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (startIndex > lines.Length)
                startIndex = lines.Length;
            if (endIndex > lines.Length)
                endIndex = lines.Length;

            // Get the lines and highlight the selection
            int count = 0;
            var sels = new StringBuilder();
            if (useColor)
            {
                sels.Append(
                    $"{ConsoleColoring.RenderSetConsoleColor(textColor)}" +
                    $"{ConsoleColoring.RenderSetConsoleColor(backgroundColor, true)}"
                );
            }
            for (int i = startIndex; i <= endIndex; i++)
            {
                // Get a line
                string source = lines[i - 1].Replace("\t", new string(' ', ConsoleMisc.TabWidth));
                if (source.Length == 0)
                    source = " ";
                var sequencesCollections = VtSequenceTools.MatchVTSequences(source);
                int vtSeqIdx = 0;

                // Seek through the whole string to find unprintable characters
                var sourceBuilder = new StringBuilder();
                for (int l = 0; l < source.Length; l++)
                {
                    string sequence = ConsolePositioning.BufferChar(source, sequencesCollections, ref l, ref vtSeqIdx, out bool isVtSequence);
                    bool unprintable = ConsoleChar.EstimateCellWidth(sequence) == 0;
                    string rendered = unprintable && !isVtSequence ? "." : sequence;
                    sourceBuilder.Append(rendered);
                }
                source = sourceBuilder.ToString();

                // Now, get the line range
                var lineBuilder = new StringBuilder();
                var absolutes = GetAbsoluteSequences(source, sequencesCollections);
                if (source.Length > 0)
                {
                    int charsPerPage = width;
                    int currentCharPage = currIdxColumn / charsPerPage;
                    int startLineIndex = charsPerPage * currentCharPage;
                    int endLineIndex = charsPerPage * (currentCharPage + 1);
                    if (startLineIndex > absolutes.Length)
                        startLineIndex = absolutes.Length;
                    if (endLineIndex > absolutes.Length)
                        endLineIndex = absolutes.Length;
                    source = "";
                    for (int a = startLineIndex; a < endLineIndex; a++)
                        source += absolutes[a];
                }
                lineBuilder.Append(source);

                // Change the color depending on the highlighted line and column
                int posX = TextWriterTools.DetermineTextAlignment(lineBuilder.ToString(), settings.Alignment, left);
                sels.Append(TextWriterWhereColor.RenderWherePlain(lineBuilder.ToString(), posX, top + count, false));
                count++;
            }
            if (useColor)
            {
                sels.Append(
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }
            return sels.ToString();
        }

        private static string[] GetAbsoluteSequences(string source, ReadOnlyDictionary<VtSequenceType, VtSequenceInfo[]> sequencesCollections)
        {
            int vtSeqIdx = 0;
            List<string> sequences = [];
            string sequence = "";
            for (int l = 0; l < source.Length; l++)
            {
                sequence += ConsolePositioning.BufferChar(source, sequencesCollections, ref l, ref vtSeqIdx, out bool isVtSequence);
                if (isVtSequence)
                    continue;
                sequences.Add(sequence);
                sequence = "";
            }
            return [.. sequences];
        }

        /// <summary>
        /// Makes a new instance of the bounded text renderer
        /// </summary>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public BoundedText(Mark? text = null, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text ?? "", vars);
            UpdateInternalSize();
        }
    }
}
