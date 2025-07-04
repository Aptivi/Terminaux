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

using System;
using Terminaux.Base;

namespace Terminaux.Colors.Transformation
{
    /// <summary>
    /// Color transformation formula
    /// </summary>
    public abstract class BaseTransformationFormula : ITransformationFormula
    {
        /// <summary>
        /// Frequency of the transformation formula
        /// </summary>
        public virtual double Frequency { get; set; } = 1.0;

        /// <inheritdoc/>
        public virtual (int r, int g, int b) Transform(int r, int g, int b) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"), new NotImplementedException());
    }
}
