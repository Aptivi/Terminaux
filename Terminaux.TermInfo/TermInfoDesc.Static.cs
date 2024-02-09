//
// Terminaux.TermInfo  Copyright (C) 2023-2024  Aptivi
//
// This file is part of Terminaux.TermInfo
//
// Terminaux.TermInfo is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux.TermInfo is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using Terminaux.TermInfo;

namespace Terminaux.TermInfo
{
    /// <summary>
    /// Represents a parsed terminfo description.
    /// </summary>
    public sealed partial class TermInfoDesc
    {
        /// <summary>
        /// Tries to load the default terminfo description for the current terminal.
        /// </summary>
        /// <param name="result">
        /// When this method returns, contains the terminfo description,
        /// if the loading succeeded, or <c>null</c> if the loading  failed.
        /// </param>
        /// <returns><c>true</c> if the terminfo description was loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad(out TermInfoDesc? result)
        {
            try
            {
                result = Load();
                return result != null;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Loads the default terminfo description for the current terminal.
        /// </summary>
        /// <returns>The default terminfo description for the current terminal,
        /// or <c>null</c> if none could be resolved.</returns>
        public static TermInfoDesc? Load()
        {
            return TermInfoLoader.Load();
        }

        /// <summary>
        /// Tries to load the specified terminfo description for the current terminal.
        /// </summary>
        /// <param name="name">The terminfo name to load.</param>
        /// <param name="result">
        /// When this method returns, contains the terminfo description
        /// if the loading succeeded, or <c>null</c> if the loading failed.
        /// </param>
        /// <returns><c>true</c> if the terminfo description was loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad(string name, out TermInfoDesc? result)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            try
            {
                result = Load(name);
                return result != null;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Loads the specified terminfo description for the current terminal.
        /// </summary>
        /// <param name="name">The name of the terminfo description to load.</param>
        /// <returns>The default terminfo description for the current terminal,
        /// or <c>null</c> if none could be resolved.</returns>
        public static TermInfoDesc? Load(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return TermInfoLoader.Load(name);
        }

        /// <summary>
        /// Tries to read a terminfo description from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="result">
        /// When this method returns, contains the terminfo description
        /// if the loading succeeded, or <c>null</c> if the loading failed.
        /// </param>
        /// <returns><c>true</c> if the terminfo description was loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad(Stream stream, out TermInfoDesc? result)
        {
            try
            {
                result = Load(stream);
                return result != null;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Reads a terminfo description from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed terminfo description.</returns>
        public static TermInfoDesc Load(Stream stream)
        {
            return TermInfoLoader.Load(stream);
        }
    }
}
