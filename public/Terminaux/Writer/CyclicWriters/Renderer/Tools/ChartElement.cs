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

using Colorimetry;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Chart element for bar charts, breakdown charts, and other analytical renderers
    /// </summary>
    public class ChartElement
    {
        private Color elementColor = ColorTools.GetRandomColor();
        private string elementName = "";
        private bool elementHidden;
        private double elementValue;

        /// <summary>
        /// Color of the chart element
        /// </summary>
        public Color Color
        {
            get => elementColor;
            set => elementColor = value;
        }

        /// <summary>
        /// Name of the chart element
        /// </summary>
        public string Name
        {
            get => elementName;
            set => elementName = value;
        }

        /// <summary>
        /// Whether this element is hidden from view or not
        /// </summary>
        public bool Hidden
        {
            get => elementHidden;
            set => elementHidden = value;
        }

        /// <summary>
        /// Value of the element
        /// </summary>
        public double Value
        {
            get => elementValue;
            set
            {
                elementValue = value;
                if (elementValue < 0)
                    elementValue = 0;
            }
        }
    }
}
