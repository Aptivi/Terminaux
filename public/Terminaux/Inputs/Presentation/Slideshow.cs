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

using System.Collections.Generic;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Presentation
{
    /// <summary>
    /// The presentation containing all the pages
    /// </summary>
    public class Slideshow
    {
        /// <summary>
        /// Presentation name
        /// </summary>
        public string Name { get; } = "Untitled presentation";

        /// <summary>
        /// Presentation pages
        /// </summary>
        public List<PresentationPage> Pages { get; }

        /// <summary>
        /// Slideshow frame border settings
        /// </summary>
        public BorderSettings BorderSettings { get; set; } = new();

        /// <summary>
        /// Slideshow frame color
        /// </summary>
        public Color FrameColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Separator);

        /// <summary>
        /// Slideshow background color
        /// </summary>
        public Color BackgroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Makes a new presentation
        /// </summary>
        /// <param name="name">Presentation name</param>
        /// <param name="pages">Presentation pages</param>
        public Slideshow(string name, List<PresentationPage> pages)
        {
            Name = name;
            Pages = pages;
        }
    }
}
