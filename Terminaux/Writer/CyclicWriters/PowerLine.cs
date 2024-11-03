﻿//
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
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Graphics;
using Terminaux.Writer.FancyWriters.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// PowerLine renderable
    /// </summary>
    public class PowerLine : IStaticRenderable
    {
        private List<PowerLineSegment> segments = [];
        private Color endingColor = ColorTools.CurrentBackgroundColor;

        /// <summary>
        /// PowerLine segments
        /// </summary>
        public List<PowerLineSegment> Segments
        {
            get => segments;
            set => segments = value;
        }

        /// <summary>
        /// Ending color
        /// </summary>
        public Color EndingColor
        {
            get => endingColor;
            set => endingColor = value;
        }

        /// <summary>
        /// Renders a PowerLine segment group
        /// </summary>
        /// <returns>Rendered PowerLine text that will be used by the renderer</returns>
        public string Render() =>
            PowerLineTools.RenderSegments(Segments, EndingColor);

        /// <summary>
        /// Makes a new instance of the PowerLine renderer
        /// </summary>
        public PowerLine()
        { }
    }
}
