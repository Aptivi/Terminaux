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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Bounded text renderable
    /// </summary>
    public class BoundedText : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private string text = "";
        private bool positionWise = false;
        private int width = 0;
        private int height = 0;
        private int lineIdx = 0;
        private int columnIdx = 0;
        private int rowIdx = 0;
        private int incrementRate = 0;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private TextSettings settings = new();
        private bool customSize = false;
        private bool customColor = false;

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
        public int Width
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
        public int Height
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
            set
            {
                foregroundColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Background color of the text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                customColor = true;
            }
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
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            if (!customSize)
                UpdateInternalSize();
            if (PositionWise)
                return TruncatedText.RenderText(
                    Text, Settings, ForegroundColor, BackgroundColor, Width, Height, Left, Top, customColor, Row, Column);
            else
                return TruncatedLineText.RenderText(
                    Text, Settings, ForegroundColor, BackgroundColor, Width, Height, Left, Top, customColor, Line, ref incrementRate);
        }

        internal void UpdateInternalSize()
        {
            width = ConsoleWrapper.WindowWidth;
            height = ConsoleWrapper.WindowHeight;
        }

        /// <summary>
        /// Makes a new instance of the bounded text renderer
        /// </summary>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public BoundedText(string text, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text, vars);
            UpdateInternalSize();
        }
    }
}
