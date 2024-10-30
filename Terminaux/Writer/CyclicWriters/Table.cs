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

using System.Collections.Generic;
using Terminaux.Colors;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Table renderable
    /// </summary>
    public class Table : IStaticRenderable
    {
        private string[,]? rows;
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private bool header = false;
        private Color separatorColor = ColorTools.CurrentForegroundColor;
        private Color headerColor = ColorTools.CurrentForegroundColor;
        private Color valueColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private List<CellOptions> settings = [];
        private BorderSettings borderSettings = new();
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
        public string[,] Rows
        {
            get => rows ?? new string[,] {{}};
            set => rows = value;
        }

        /// <summary>
        /// Whether to enable table headers
        /// </summary>
        public bool Header
        {
            get => header;
            set => header = value;
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
        /// Table separator color
        /// </summary>
        public Color SeparatorColor
        {
            get => separatorColor;
            set
            {
                separatorColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Table header color
        /// </summary>
        public Color HeaderColor
        {
            get => headerColor;
            set
            {
                headerColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Table value color
        /// </summary>
        public Color ValueColor
        {
            get => valueColor;
            set
            {
                valueColor = value;
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
        /// Cell settings to use
        /// </summary>
        public List<CellOptions> Settings
        {
            get => settings;
            set
            {
                settings = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Border settings to use
        /// </summary>
        public BorderSettings BorderSettings
        {
            get => borderSettings;
            set => borderSettings = value;
        }

        /// <summary>
        /// Renders an table text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return TableColor.RenderTable(
                Rows, Left, Top, InteriorWidth, InteriorHeight, Header, SeparatorColor, HeaderColor, ValueColor, BackgroundColor, customColor, Settings, BorderSettings);
        }

        /// <summary>
        /// Makes a new instance of the table renderer
        /// </summary>
        public Table()
        { }
    }
}
