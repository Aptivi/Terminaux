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

using System.Diagnostics;

namespace Terminaux.Base.TermInfo.Parsing.Parameters
{
    /// <summary>
    /// Parameter information
    /// </summary>
    [DebuggerDisplay("@ {Index} [{Type}]: {Representation}")]
    public class ParameterInfo
    {
        private readonly string representation = "";
        private readonly int index = 0;
        private readonly ParameterType type = ParameterType.Unknown;

        /// <summary>
        /// Representation of this parameter
        /// </summary>
        public string Representation =>
            representation;

        /// <summary>
        /// Index of this parameter found within the string
        /// </summary>
        public int Index =>
            index;

        /// <summary>
        /// Index of this parameter found within the string
        /// </summary>
        public ParameterType Type =>
            type;

        internal ParameterInfo(string representation, int index, ParameterType type)
        {
            this.representation = representation;
            this.index = index;
            this.type = type;
            ConsoleLogger.Debug("Parameter: {0}, index: {1}, type: {2}", representation, index, type);
        }
    }
}
