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
using Terminaux.Colors;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Exception text renderable
    /// </summary>
    public class TextException : IStaticRenderable
    {
        private Exception exception;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private bool useColors = true;

        /// <summary>
        /// Exception to render
        /// </summary>
        public Exception Exception
        {
            get => exception;
            set => exception = value;
        }

        /// <summary>
        /// Foreground color of the exception text (for general text)
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color of the exception text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
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
        /// Renders an exception figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return "";
        }

        /// <summary>
        /// Makes a new instance of the exception text renderer
        /// </summary>
        public TextException(Exception ex)
        {
            exception = ex;
        }
    }
}
