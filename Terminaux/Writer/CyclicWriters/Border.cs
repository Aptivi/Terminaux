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
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Aligned text renderable
    /// </summary>
    public class Border : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private string title = "";
        private string text = "";
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private Color borderColor = ColorTools.CurrentForegroundColor;
        private Color textColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private BorderSettings settings = new();
        private TextSettings textSettings = new();
        private bool customPos = false;
        private bool customColor = false;

        /// <summary>
        /// Left position
        /// </summary>
        public int Left
        {
            get => left;
            set
            {
                left = value;
                customPos = true;
            }
        }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set
            {
                top = value;
                customPos = true;
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value;
        }

        /// <summary>
        /// Text to show
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Interior width
        /// </summary>
        public int InteriorWidth
        {
            get => interiorWidth;
            set
            {
                interiorWidth = value;
                if (!customPos)
                    UpdateInternalTop();
            }
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set
            {
                interiorHeight = value;
                if (!customPos)
                    UpdateInternalTop();
            }
        }

        /// <summary>
        /// Border color
        /// </summary>
        public Color Color
        {
            get => borderColor;
            set
            {
                borderColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get => textColor;
            set
            {
                textColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Background color
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
        /// Border settings to use
        /// </summary>
        public BorderSettings Settings
        {
            get => settings;
            set => settings = value;
        }

        /// <summary>
        /// Text settings to use
        /// </summary>
        public TextSettings TextSettings
        {
            get => textSettings;
            set => textSettings = value;
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            if (!customPos)
                UpdateInternalTop();
            return BorderTextColor.RenderBorder(
                Title, text, Left, Top, InteriorWidth, InteriorHeight, Settings, TextSettings, Color, BackgroundColor, TextColor, customColor);
        }

        internal void UpdateInternalTop()
        {
            var splitLines = text.SplitNewLines();
            interiorWidth = splitLines.Max((str) => str.Length);
            interiorHeight = splitLines.Length;
            if (interiorWidth >= ConsoleWrapper.WindowWidth)
                interiorWidth = ConsoleWrapper.WindowWidth - 4;
            if (interiorHeight >= ConsoleWrapper.WindowHeight)
                interiorHeight = ConsoleWrapper.WindowHeight - 4;
            left = ConsoleWrapper.WindowWidth / 2 - interiorWidth / 2 - 1;
            top = ConsoleWrapper.WindowHeight / 2 - interiorHeight / 2 - 1;
        }

        /// <summary>
        /// Makes a new instance of the border renderer
        /// </summary>
        public Border()
        {
            UpdateInternalTop();
        }
    }
}
