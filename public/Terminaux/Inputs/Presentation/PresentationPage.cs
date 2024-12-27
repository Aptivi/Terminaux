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

using Terminaux.Inputs.Presentation.Elements;
using Terminaux.Inputs.Presentation.Inputs;

namespace Terminaux.Inputs.Presentation
{
    /// <summary>
    /// Presentation page
    /// </summary>
    public class PresentationPage
    {
        /// <summary>
        /// Presentation page name
        /// </summary>
        public string Name { get; } = "Untitled presentation page";

        /// <summary>
        /// Presentation page elements
        /// </summary>
        public IElement[] Elements { get; }

        /// <summary>
        /// Presentation page inputs
        /// </summary>
        public InputInfo[] Inputs { get; }

        /// <summary>
        /// Makes a new presentation page
        /// </summary>
        /// <param name="name">Page name</param>
        /// <param name="elements">List of elements</param>
        /// <param name="inputs">List of inputs</param>
        public PresentationPage(string name, IElement[] elements, InputInfo[]? inputs = null)
        {
            Name = name;
            Elements = elements;
            Inputs = inputs ?? [];
        }
    }
}
