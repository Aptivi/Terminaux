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

using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Wins and losses chart renderable
    /// </summary>
    public class WinsLosses : IStaticRenderable
    {
        private (string, ChartElement win, ChartElement loss)[] elements = [];
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private bool showcase = false;
        private bool useColors = true;

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
        /// Interior width
        /// </summary>
        public int InteriorWidth
        {
            get => interiorWidth;
            set => interiorWidth = value;
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set => interiorHeight = value;
        }

        /// <summary>
        /// Show the element list
        /// </summary>
        public bool Showcase
        {
            get => showcase;
            set => showcase = value;
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
        /// Chart elements
        /// </summary>
        public (string, ChartElement win, ChartElement loss)[] Elements
        {
            get => elements;
            set => elements = value;
        }

        /// <summary>
        /// Renders the wins and losses chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return TextWriterWhereColor.RenderWhere(
                RenderWinsLosses(
                    elements, InteriorWidth, InteriorHeight, Showcase, UseColors), Left, Top);
        }

        internal static string RenderWinsLosses((string, ChartElement win, ChartElement loss)[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false, bool useColor = true)
        {
            // Some variables
            int maxNameLength = InteriorWidth / 4;
            int wholeLength = InteriorHeight - 1;
            double maxWinValue = elements.Max((element) => element.win.Value);
            double maxLossValue = elements.Max((element) => element.loss.Value);
            int nameLength = elements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Item1) + $"  {element.win.Value}/{element.loss.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            var shownWinElementHeights = elements.Select((ce) => (ce.win, (int)(ce.win.Value * wholeLength / 2 / maxWinValue))).ToArray();
            var shownLossElementHeights = elements.Select((ce) => (ce.loss, (int)(ce.loss.Value * wholeLength / 2 / maxWinValue))).ToArray();
            int showcaseLength = showcase ? nameLength + 3 : 0;
            double stickWidth = (double)(InteriorWidth - (showcaseLength + 3)) / elements.Length / 2;

            // Fill the stick chart with the showcase first
            StringBuilder winsLosses = new();
            for (int i = 0; i < InteriorHeight; i++)
            {
                // If showcase is on, show names and values.
                int processedWidth = 0;
                if (showcase && i < elements.Length)
                {
                    var element = elements[i];
                    int nameWidth = ConsoleChar.EstimateCellWidth(element.Item1);
                    int spaces = showcaseLength - (" ■ ".Length + nameWidth + 2 + $"{element.win.Value}/{element.loss.Value}".Length);
                    spaces = spaces < 0 ? 0 : spaces;
                    winsLosses.Append(
                        (useColor ? ColorTools.RenderSetConsoleColor(element.win.Color) : "") +
                        " ■ " +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                        element.Item1.Truncate(nameLength - 4 - $"{maxWinValue}/{maxLossValue}".Length) + "  " +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Lime) : "") +
                        $"{element.win.Value}" +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        "/" +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Red) : "") +
                        $"{element.loss.Value}" +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        new string(' ', spaces) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else if (showcase)
                {
                    winsLosses.Append(
                        new string(' ', showcaseLength) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else
                {
                    winsLosses.Append(
                        " ┃ "
                    );
                    processedWidth += 3;
                }

                // Render all elements
                int inverse = InteriorHeight / 2 - i;
                bool lossMode = inverse < 0;
                int e = 0;
                if (inverse == 0)
                {
                    // Write a separator between wins and losses
                    while (processedWidth < InteriorWidth - 2)
                    {
                        winsLosses.Append(
                            (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver, true) : "") +
                            "  " +
                            (useColor ? ColorTools.RenderResetBackground() : "")
                        );
                        processedWidth += 2;
                    }
                }
                else
                {
                    while (processedWidth < InteriorWidth)
                    {
                        var elementTuple = lossMode ? shownLossElementHeights[e] : shownWinElementHeights[e];
                        ChartElement? element = elementTuple.Item1;
                        int height = elementTuple.Item2;

                        for (int w = 0; w < (int)stickWidth; w++)
                        {
                            bool colorElement = lossMode ? -inverse <= height : inverse <= height;
                            var color =
                                colorElement ? lossMode ? ConsoleColors.Red : ConsoleColors.Lime :
                                (useColor ? ColorTools.CurrentBackgroundColor : "");
                            double value = element.Value;

                            // Render the element and its value
                            winsLosses.Append(
                                (useColor ? ColorTools.RenderSetConsoleColor(color, true) : "") +
                                "  " +
                                (useColor ? ColorTools.RenderResetBackground() : "")
                            );
                            processedWidth += 2;
                        }
                        e++;
                        if (e >= elements.Length)
                            break;
                    }
                }

                if (i < InteriorHeight - 1)
                    winsLosses.AppendLine();
            }

            // Return the result
            return winsLosses.ToString();
        }

        /// <summary>
        /// Makes a new instance of the wins and losses chart renderer
        /// </summary>
        public WinsLosses()
        { }
    }
}
