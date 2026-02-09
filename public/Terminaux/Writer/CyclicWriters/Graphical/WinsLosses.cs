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

using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry.Data;
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
            // Showcase variables
            var showcase = new ValueShowcaseDouble()
            {
                Left = Left,
                Top = Top,
                Width = Width / 4,
                Height = Height,
                UseColors = UseColors,
                Elements = Elements,
            };
            int showcaseLength = 0;

            // Some variables
            int wholeLength = Height - 1;
            double maxWinValue = elements.Max((element) => element.win.Value);
            double maxLossValue = elements.Max((element) => element.loss.Value);
            var shownWinElementHeights = elements.Select((ce) => (ce.win, (int)(ce.win.Value * wholeLength / 2 / maxWinValue))).ToArray();
            var shownLossElementHeights = elements.Select((ce) => (ce.loss, (int)(ce.loss.Value * wholeLength / 2 / maxLossValue))).ToArray();

            // Fill the wins/losses chart with the showcase first
            StringBuilder winsLosses = new();
            if (Showcase)
            {
                showcaseLength = showcase.Length;
                winsLosses.Append(showcase.Render());

                // Write a separator between wins and losses
                int nameLength = elements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Item1) + $"  {element.win.Value:0.00}/{element.loss.Value:0.00}".Length);
                nameLength = nameLength > Width ? Width : nameLength;
                Coordinate winLossSeparatorCoord = new(Left + nameLength, Top + (Height / 2));
                winsLosses.Append(
                    ConsolePositioning.RenderChangePosition(winLossSeparatorCoord.X, winLossSeparatorCoord.Y) +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                    " ▐" +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Lime) : "") +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Red, true) : "") +
                    "▀" +
                    new string('▀', Width - showcaseLength - 1)
                );
            }

            // Show the actual win-loss chart
            double stickWidth = (double)(Width - (showcaseLength + 3)) / elements.Length / 2;
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
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Lime, true) : "") +
                            new string(' ', stickWidthInt) +
                            (UseColors ? ConsoleColoring.RenderResetBackground() : "")
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
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Red, true) : "") +
                            new string(' ', stickWidthInt) +
                            (UseColors ? ConsoleColoring.RenderResetBackground() : "")
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
