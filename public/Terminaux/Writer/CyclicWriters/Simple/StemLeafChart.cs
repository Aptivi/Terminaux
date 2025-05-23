//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Stem and leaf chart renderable
    /// </summary>
    public class StemLeafChart : SimpleCyclicWriter
    {
        private double[] elements = [];
        private Color stemColor = ColorTools.CurrentForegroundColor;
        private Color leafColor = ColorTools.CurrentForegroundColor;
        private Color separatorColor = ColorTools.CurrentForegroundColor;
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
        /// Chart elements (for numbers)
        /// </summary>
        public double[] Elements
        {
            get => elements;
            set => elements = value;
        }

        /// <summary>
        /// Stem color
        /// </summary>
        public Color StemColor
        {
            get => stemColor;
            set => stemColor = value;
        }

        /// <summary>
        /// Leaf color
        /// </summary>
        public Color LeafColor
        {
            get => leafColor;
            set => leafColor = value;
        }

        /// <summary>
        /// Separator color
        /// </summary>
        public Color SeparatorColor
        {
            get => separatorColor;
            set => separatorColor = value;
        }

        /// <summary>
        /// Renders a stem and leaf chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            StringBuilder stemLeafChart = new();
            Dictionary<int, List<int>> stemLeafs = [];

            // First, sort the numeric elements
            Array.Sort(Elements);

            // Get the digits and split it according to the conditions.
            foreach (var element in Elements)
            {
                // If this number describes a decimal, then we need to split the numeric part into the stem and the
                // decimal part into the leaf.
                int floor = (int)Math.Floor(element);
                int ceiling = (int)Math.Ceiling(element);
                bool isDecimal = element != floor && element != ceiling;
                int stem = 0, leaf = 0;
                if (isDecimal)
                {
                    // The stem is a numeric part and the leaf is a fractional part as a whole number.
                    stem = (int)Math.Truncate(element);
                    leaf = (int)(((decimal)element - stem) * 100m);
                }
                else
                {
                    // The stem describes digits of tens and greater and the leaf is the digit of ones.
                    stem = (int)(element / 10);
                    leaf = (int)(element % 10);
                }

                // Add the stem and the leaf
                if (!stemLeafs.ContainsKey(stem))
                    stemLeafs.Add(stem, []);
                stemLeafs[stem].Add(leaf);
            }

            // Render the stems and the leafs
            int maxStemWidth = stemLeafs.Max((kvp) => LineHandle.GetDigits(kvp.Key));
            foreach (var stem in stemLeafs.Keys)
            {
                // Write the stem
                var leafs = stemLeafs[stem];
                int stemWidth = LineHandle.GetDigits(stem);
                int maxSpaces = maxStemWidth - stemWidth;
                stemLeafChart.Append(
                    $"{(UseColors ? ColorTools.RenderSetConsoleColor(StemColor) : "")}" +
                    new string(' ', maxSpaces) + stem +
                    $"{(UseColors ? ColorTools.RenderSetConsoleColor(SeparatorColor) : "")} | "
                );

                // Write the leafs
                foreach (int leaf in leafs)
                {
                    stemLeafChart.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(LeafColor) : "")}" +
                        leaf + " "
                    );
                }

                // Add a new line
                stemLeafChart.AppendLine();
            }

            // Return the result
            return stemLeafChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the stem and leaf chart renderer
        /// </summary>
        public StemLeafChart()
        { }
    }
}
