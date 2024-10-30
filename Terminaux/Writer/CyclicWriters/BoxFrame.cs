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

using Terminaux.Colors;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Box frame renderable
    /// </summary>
    public class BoxFrame : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private string text = "";
        private Color boxFrameColor = ColorTools.CurrentForegroundColor;
        private Color titleColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private BorderSettings settings = new();
        private TextSettings titleSettings = new();
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
        /// Top position
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
            set => interiorWidth = value;
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set => interiorHeight = value;
        }

        /// <summary>
        /// Box frame color
        /// </summary>
        public Color FrameColor
        {
            get => boxFrameColor;
            set
            {
                boxFrameColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Box frame title color
        /// </summary>
        public Color TitleColor
        {
            get => titleColor;
            set
            {
                titleColor = value;
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
        /// Title settings to use
        /// </summary>
        public TextSettings TitleSettings
        {
            get => titleSettings;
            set => titleSettings = value;
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return BoxFrameColor.RenderBoxFrame(
                Text, Left, Top, InteriorWidth, InteriorHeight, Settings, TitleSettings, FrameColor, BackgroundColor, TitleColor, customColor);
        }

        /// <summary>
        /// Makes a new instance of the box renderer
        /// </summary>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public BoxFrame(string text, params object[] vars)
        {
            this.text = TextTools.FormatString(text, vars);
        }
    }
}
