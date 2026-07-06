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
using Colorimetry.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Value showcase renderable
    /// </summary>
    public class ValueShowcase : SimpleCyclicWriter
    {
        private ChartElement[] elements = [];
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
        public ChartElement[] Elements
        {
            get => elements;
            set => elements = value;
        }

        /// <summary>
        /// Shows the separator
        /// </summary>
        public bool ShowSeparator { get; set; } = true;

        /// <summary>
        /// Maximum width of the showcase (0 to automatically determine based on content)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Maximum height of the showcase (0 to automatically determine based on content)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Calculated length of the showcase panel
        /// </summary>
        public int Length
        {
            get
            {
                // Some variables
                var shownElements = elements.Where((ce) => !ce.Hidden);
                if (Height > 0)
                    shownElements = shownElements.Take(Height);
                double maxValue = shownElements.Max((element) => element.Value);

                // Get the showcase length
                StringBuilder showcase = new();
                int nameLength = shownElements.Max((element) => ConsoleChar.EstimateCellWidth($" ■ {element.Name}  {element.Value:0.##}"));
                nameLength = nameLength > Width && Width > 0 ? Width : nameLength;
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
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            if (Height > 0)
                shownElements = [.. shownElements.Take(Height)];
            double maxValue = shownElements.Max((element) => element.Value);

            // Fill the showcase panel with the elements first
            StringBuilder showcase = new();
            int nameLength = shownElements.Max((element) => ConsoleChar.EstimateCellWidth($" ■ {element.Name}  {element.Value:0.##}"));
            nameLength = nameLength > Width && Width > 0 ? Width : nameLength;
            int processedHeight = 0;
            for (int i = 0; i < shownElements.Length; i++)
            {
                StringBuilder elementBuilder = new();

                // Get the element showcase position and write it there
                bool canShow = Height > i;
                if (!canShow)
                    break;
                var element = shownElements[i];

                // Now, write it at the selected position
                elementBuilder.Append(
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(element.Color) : "") +
                    " ■ " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                    element.Name.Truncate(nameLength - 4 - $"{maxValue:0.##}".Length) + "  " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                    $"{element.Value:0.##}"
                );
                string builtElement = elementBuilder.ToString();
                showcase.Append(builtElement);

                // Write the separator
                if (ShowSeparator)
                {
                    int currentWidth = ConsoleChar.EstimateCellWidth(builtElement);
                    int spaces = nameLength - currentWidth;
                    showcase.Append(
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        new string(' ', spaces) +
                        " ▐"
                    );
                }
                if (i < shownElements.Length - 1)
                    showcase.AppendLine();
                processedHeight++;
            }

            // In case we've specified height, we need to write the separator appropriately
            int remainingHeight = Height - processedHeight;
            if (remainingHeight > 0)
            {
                showcase.AppendLine();
                for (int i = 0; i < remainingHeight; i++)
                {
                    showcase.Append(
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        new string(' ', nameLength) +
                        " ▐"
                    );
                    if (i < remainingHeight - 1)
                        showcase.AppendLine();
                }
            }

            // Return the result
            return showcase.ToString();
        }

        /// <summary>
        /// Makes a new instance of the value showcase renderer
        /// </summary>
        public ValueShowcase()
        { }
    }
}
