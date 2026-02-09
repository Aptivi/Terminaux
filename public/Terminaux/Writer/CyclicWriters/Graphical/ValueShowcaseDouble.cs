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
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Value showcase (for two values) renderable
    /// </summary>
    public class ValueShowcaseDouble : GraphicalCyclicWriter
    {
        private (string, ChartElement element1, ChartElement element2)[] elements = [];
        private bool useColors = true;

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
        public (string, ChartElement element1, ChartElement element2)[] Elements
        {
            get => elements;
            set => elements = value;
        }

        /// <summary>
        /// Shows the separator
        /// </summary>
        public bool ShowSeparator { get; set; } = true;

        /// <summary>
        /// Calculated length of the showcase panel
        /// </summary>
        public int Length
        {
            get
            {
                // Some variables
                int wholeLength = Height - 1;
                double maxWinValue = elements.Max((element) => element.element1.Value);
                double maxLossValue = elements.Max((element) => element.element2.Value);
                var shownWinElementHeights = elements.Select((ce) => (ce.element1, (int)(ce.element1.Value * wholeLength / 2 / maxWinValue))).ToArray();
                var shownLossElementHeights = elements.Select((ce) => (ce.element2, (int)(ce.element2.Value * wholeLength / 2 / maxLossValue))).ToArray();

                // Fill the showcase panel with the elements first
                int nameLength = elements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Item1) + $"  {element.element1.Value:0.##}/{element.element2.Value:0.##}".Length);
                nameLength = nameLength > Width ? Width : nameLength;
                return nameLength + (ShowSeparator ? 2 : 0);
            }
        }

        /// <summary>
        /// Renders a showcase panel
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Some variables
            StringBuilder showcase = new();
            int wholeLength = Height - 1;
            double maxFirstGroupValue = elements.Max((element) => element.element1.Value);
            double maxSecondGroupValue = elements.Max((element) => element.element2.Value);
            var shownFirstGroupElementHeights = elements.Select((ce) => (ce.element1, (int)(ce.element1.Value * wholeLength / 2 / maxFirstGroupValue))).ToArray();
            var shownSecondGroupElementHeights = elements.Select((ce) => (ce.element2, (int)(ce.element2.Value * wholeLength / 2 / maxSecondGroupValue))).ToArray();

            // Fill the showcase panel with the elements first
            int nameLength = elements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Item1) + $"  {element.element1.Value:0.##}/{element.element2.Value:0.##}".Length);
            nameLength = nameLength > Width ? Width : nameLength;
            for (int i = 0; i < elements.Length; i++)
            {
                // Get the element showcase position and write it there
                bool canShow = Height > i;
                if (!canShow)
                    break;
                Coordinate coord = new(Left, Top + i);
                var element = elements[i];

                // Now, write it at the selected position
                showcase.Append(
                    ConsolePositioning.RenderChangePosition(coord.X, coord.Y) +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(element.element1.Color) : "") +
                    " ■ " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                    element.Item1.Truncate(nameLength - 4 - $"{maxFirstGroupValue:0.##}/{maxSecondGroupValue:0.##}".Length) + "  " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Lime) : "") +
                    $"{element.element1.Value:0.##}" +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                    "/" +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Red) : "") +
                    $"{element.element2.Value:0.##}"
                );
            }

            // Show the separator
            if (ShowSeparator)
            {
                showcase.Append(UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "");
                for (int h = 0; h < Height; h++)
                {
                    Coordinate separatorCoord = new(Left + nameLength, Top + h);
                    showcase.Append(
                        ConsolePositioning.RenderChangePosition(separatorCoord.X, separatorCoord.Y) +
                        " ▐"
                    );
                }
            }

            // Return the result
            return showcase.ToString();
        }

        /// <summary>
        /// Makes a new instance of the value showcase (double value) renderer
        /// </summary>
        public ValueShowcaseDouble()
        { }
    }
}
