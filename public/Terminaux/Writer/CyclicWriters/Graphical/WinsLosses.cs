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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Wins and losses chart renderable
    /// </summary>
    public class WinsLosses : GraphicalCyclicWriter
    {
        private (string, ChartElement win, ChartElement loss)[] elements = [];
        private bool showcase = false;
        private bool useColors = true;

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
        public override string Render()
        {
            // Some variables
            int maxNameLength = Width / 4;
            int wholeLength = Height - 1;
            double maxWinValue = elements.Max((element) => element.win.Value);
            double maxLossValue = elements.Max((element) => element.loss.Value);
            int nameLength = elements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Item1) + $"  {element.win.Value}/{element.loss.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            var shownWinElementHeights = elements.Select((ce) => (ce.win, (int)(ce.win.Value * wholeLength / 2 / maxWinValue))).ToArray();
            var shownLossElementHeights = elements.Select((ce) => (ce.loss, (int)(ce.loss.Value * wholeLength / 2 / maxLossValue))).ToArray();
            int showcaseLength = showcase ? nameLength + 3 : 0;
            double stickWidth = (double)(Width - (showcaseLength + 3)) / elements.Length / 2;

            // Fill the stick chart with the showcase first
            StringBuilder winsLosses = new();
            if (Showcase)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    // Get the element showcase position and write it there
                    bool canShow = Height > i;
                    if (!canShow)
                        break;
                    Coordinate coord = new(Left, Top + i);
                    var element = elements[i];

                    // Now, write it at the selected position
                    winsLosses.Append(
                        ConsolePositioning.RenderChangePosition(coord.X, coord.Y) +
                        (UseColors ? ColorTools.RenderSetConsoleColor(element.win.Color) : "") +
                        " ■ " +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                        element.Item1.Truncate(nameLength - 4 - $"{maxWinValue}/{maxLossValue}".Length) + "  " +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Lime) : "") +
                        $"{element.win.Value}" +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        "/" +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Red) : "") +
                        $"{element.loss.Value}" +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "")
                    );
                }

                // Show the separator
                for (int h = 0; h < Height; h++)
                {
                    Coordinate separatorCoord = new(Left + nameLength, Top + h);
                    winsLosses.Append(
                        ConsolePositioning.RenderChangePosition(separatorCoord.X, separatorCoord.Y) +
                        " ┃ "
                    );
                }
            }

            // Write a separator between wins and losses
            Coordinate winLossSeparatorCoord = new(Left + nameLength, Top + (Height / 2));
            winsLosses.Append(
                ConsolePositioning.RenderChangePosition(winLossSeparatorCoord.X, winLossSeparatorCoord.Y) +
                (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                " ┣━" +
                new string('━', Width - showcaseLength - 1)
            );

            // Show the actual win-loss chart
            for (int e = 0; e < elements.Length; e++)
            {
                // Win element
                var elementWinTuple = shownWinElementHeights[e];
                ChartElement? elementWin = elementWinTuple.win;
                int heightWin = elementWinTuple.Item2;
                
                // Loss element
                var elementLossTuple = shownLossElementHeights[e];
                ChartElement? elementLoss = elementLossTuple.loss;
                int heightLoss = elementLossTuple.Item2;

                // Decide whether to draw this area or not
                for (int h = 0; h < Height / 2; h++)
                {
                    // Decide whether to draw this area or not (for wins)
                    int stickWidthInt = (int)stickWidth * 2;
                    Coordinate winCoord = new(Left + showcaseLength + (stickWidthInt * e), Top + h);
                    ConsoleLogger.Debug("Win: rendering win-loss chart element {0}: ({1}, {2})", e, winCoord.X, winCoord.Y);
                    if ((Height / 2) - h < heightWin)
                    {
                        ConsoleLogger.Debug("Win: rendering this element with width {0}", stickWidthInt);
                        winsLosses.Append(
                            ConsolePositioning.RenderChangePosition(winCoord.X, winCoord.Y) +
                            (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Lime, true) : "") +
                            new string(' ', stickWidthInt) +
                            (UseColors ? ColorTools.RenderResetBackground() : "")
                        );
                    }

                    // Decide whether to draw this area or not (for losses)
                    Coordinate lossCoord = new(Left + showcaseLength + (stickWidthInt * e), Top + (Height / 2) + h + 1);
                    ConsoleLogger.Debug("Loss: rendering win-loss chart element {0}: ({1}, {2})", e, winCoord.X, winCoord.Y);
                    if (h < heightLoss)
                    {
                        ConsoleLogger.Debug("Loss: rendering this element with width {0}", stickWidthInt);
                        winsLosses.Append(
                            ConsolePositioning.RenderChangePosition(lossCoord.X, lossCoord.Y) +
                            (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Red, true) : "") +
                            new string(' ', stickWidthInt) +
                            (UseColors ? ColorTools.RenderResetBackground() : "")
                        );
                    }
                }
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
