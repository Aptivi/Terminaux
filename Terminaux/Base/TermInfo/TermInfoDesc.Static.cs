//
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
using System.IO;
using Textify.General;

namespace Terminaux.Base.TermInfo
{
    /// <summary>
    /// Represents a parsed terminfo description.
    /// </summary>
    public sealed partial class TermInfoDesc
    {
        private static readonly TermInfoDesc fallback = Load(typeof(TermInfoDesc).Assembly.GetManifestResourceStream("Terminaux.Resources.TermFiles.x.xterm-256color"));
        private static readonly TermInfoDesc current = LoadSafe();

        /// <summary>
        /// Current terminal information description
        /// </summary>
        public static TermInfoDesc Current =>
            current;

        /// <summary>
        /// Fallback terminal information description (that is, xterm-256color)
        /// </summary>
        public static TermInfoDesc Fallback =>
            fallback;

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
        /// or an exception if none could be resolved.</returns>
        public static TermInfoDesc Load() =>
            TermInfoLoader.Load();

        /// <summary>
        /// Loads the default terminfo description for the current terminal.
        /// </summary>
        /// <returns>The default terminfo description for the current terminal,
        /// or fallback to XTerm if none could be resolved.</returns>
        public static TermInfoDesc LoadSafe()
        {
            if (!TryLoad(out TermInfoDesc? result))
                return Fallback;
            return result ?? Fallback;
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
        /// or an exception if none could be resolved.</returns>
        public static TermInfoDesc Load(string name) =>
            TermInfoLoader.Load(name);

        /// <summary>
        /// Loads the default terminfo description for the current terminal.
        /// </summary>
        /// <param name="name">The name of the terminfo description to load.</param>
        /// <returns>The default terminfo description for the current terminal,
        /// or fallback to XTerm if none could be resolved.</returns>
        public static TermInfoDesc LoadSafe(string name)
        {
            if (!TryLoad(name, out TermInfoDesc? result))
                return Fallback;
            return result ?? Fallback;
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
        public static TermInfoDesc Load(Stream stream) =>
            TermInfoLoader.Load(stream);

        /// <summary>
        /// Loads the default terminfo description for the current terminal.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The default terminfo description for the current terminal,
        /// or fallback to XTerm if none could be resolved.</returns>
        public static TermInfoDesc LoadSafe(Stream stream)
        {
            if (!TryLoad(stream, out TermInfoDesc? result))
                return Fallback;
            return result ?? Fallback;
        }

        /// <summary>
        /// Gets the built-in terminfo names
        /// </summary>
        /// <returns></returns>
        public static string[] GetBuiltins()
        {
            // Builtins tracked from Debian's NCurses: https://packages.debian.org/sid/all/ncurses-base/download
            // and https://packages.debian.org/sid/all/ncurses-term/download
            string[] builtins = GetBuiltinPaths();
            List<string> names = [];
            foreach (string builtin in builtins)
            {
                string termNamePath = builtin.RemovePrefix("Terminaux.Resources.TermFiles.");
                string termName = termNamePath.Substring(termNamePath.IndexOf('.') + 1);
                names.Add(termName);
            }
            return [.. names];
        }

        internal static string[] GetBuiltinPaths()
        {
            string[] builtins = typeof(TermInfoDesc).Assembly.GetManifestResourceNames();
            return builtins;
        }
    }
}
