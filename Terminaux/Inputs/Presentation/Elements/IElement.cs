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


//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;

namespace Terminaux.Inputs.Presentation.Elements
{
    /// <summary>
    /// A presentation element
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Is the element an input?
        /// </summary>
        bool IsInput { get; }

        /// <summary>
        /// Written input
        /// </summary>
        string WrittenInput { get; set; }

        /// <summary>
        /// Arguments
        /// </summary>
        object[] Arguments { get; set; }

        /// <summary>
        /// Renders an element
        /// </summary>
        void Render();

        /// <summary>
        /// Pre-print, check for possible out of bounds when rendering
        /// </summary>
        bool IsPossibleOutOfBounds();

        /// <summary>
        /// Invokes the action after rendering the element (input)
        /// </summary>
        Action<object[]> InvokeActionInput { get; }

        /// <summary>
        /// Invokes the action after rendering the element (normal)
        /// </summary>
        Action InvokeAction { get; }
    }
}
