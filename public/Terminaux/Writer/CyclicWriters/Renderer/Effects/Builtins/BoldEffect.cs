﻿//
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

using Terminaux.Base.TermInfo;

namespace Terminaux.Writer.CyclicWriters.Renderer.Effects.Builtins
{
    /// <summary>
    /// Bold text effect
    /// </summary>
    public class BoldEffect : IEffect
    {
        /// <inheritdoc/>
        public string MarkupTag =>
            "bold";

        /// <inheritdoc/>
        public string EffectStart =>
            TermInfoDesc.Current?.EnterBoldMode?.Value ?? "";
    }
}
