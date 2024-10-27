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

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Spinner cyclic renderer
    /// </summary>
    public class Spinner : ICyclicRenderer
    {
        private int step = 0;
        private readonly string[] spinners = [];

        /// <summary>
        /// Renders a spinner
        /// </summary>
        /// <returns>A string representation of the spinner</returns>
        public string Render()
        {
            // Get the spinner, increase a step, and check
            string spinner = spinners[step];
            step++;
            if (step >= spinners.Length)
                step = 0;

            // Return the spinner
            return spinner;
        }

        /// <summary>
        /// Makes a new spinner instance
        /// </summary>
        /// <param name="spinners">A list of spinners</param>
        public Spinner(string[] spinners)
        {
            // Check the spinners
            int lastWidth = 0;
            bool first = true;
            foreach (string spinner in spinners)
            {
                int width = ConsoleChar.EstimateCellWidth(spinner);
                if (width != lastWidth && !first)
                    throw new TerminauxException("All spinners should have the same width.");
                lastWidth = width;
                first = false;
            }

            // Install them
            this.spinners = spinners;
        }
    }
}
