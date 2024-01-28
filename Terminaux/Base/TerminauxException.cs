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

using System;

namespace Terminaux.Base
{
    /// <summary>
    /// Terminaux exception class
    /// </summary>
    public class TerminauxException : Exception
    {
        /// <summary>
        /// Makes an empty <see cref="TerminauxException"/> exception instance with the default message
        /// </summary>
        public TerminauxException() :
            base("General Terminaux error!")
        { }

        /// <summary>
        /// Makes a <see cref="TerminauxException"/> exception instance
        /// </summary>
        /// <param name="message">A message to specify</param>
        public TerminauxException(string message) :
            base(message)
        { }

        /// <summary>
        /// Makes a <see cref="TerminauxException"/> exception instance
        /// </summary>
        /// <param name="message">A message to specify</param>
        /// <param name="innerException">An inner exception to specify</param>
        public TerminauxException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
