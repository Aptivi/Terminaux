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

using System;
using Textify.General;

namespace Terminaux.Base
{
    /// <summary>
    /// Terminaux exception class
    /// </summary>
    public class TerminauxInternalException : Exception
    {
        /// <summary>
        /// Makes an empty <see cref="TerminauxInternalException"/> exception instance with the default message
        /// </summary>
        public TerminauxInternalException() :
            base(LanguageTools.GetLocalized("T_EXCEPTION_UNKNOWNERROR2"))
        { }

        /// <summary>
        /// Makes an empty <see cref="TerminauxInternalException"/> exception instance with the default message
        /// </summary>
        /// <param name="innerException">An inner exception to specify</param>
        public TerminauxInternalException(Exception innerException) :
            base(LanguageTools.GetLocalized("T_EXCEPTION_UNKNOWNERROR2"), innerException)
        { }

        /// <summary>
        /// Makes a <see cref="TerminauxInternalException"/> exception instance
        /// </summary>
        /// <param name="message">A message to specify</param>
        public TerminauxInternalException(string message) :
            base(message)
        { }

        /// <summary>
        /// Makes a <see cref="TerminauxInternalException"/> exception instance
        /// </summary>
        /// <param name="message">A message to specify</param>
        /// <param name="vars">List of variables to format the message with</param>
        public TerminauxInternalException(string message, params object[] vars) :
            base(TextTools.FormatString(message, vars))
        { }

        /// <summary>
        /// Makes a <see cref="TerminauxInternalException"/> exception instance
        /// </summary>
        /// <param name="message">A message to specify</param>
        /// <param name="innerException">An inner exception to specify</param>
        public TerminauxInternalException(string message, Exception innerException) :
            base(message, innerException)
        { }

        /// <summary>
        /// Makes a <see cref="TerminauxInternalException"/> exception instance
        /// </summary>
        /// <param name="message">A message to specify</param>
        /// <param name="innerException">An inner exception to specify</param>
        /// <param name="vars">List of variables to format the message with</param>
        public TerminauxInternalException(string message, Exception innerException, params object[] vars) :
            base(TextTools.FormatString(message, vars), innerException)
        { }
    }
}
