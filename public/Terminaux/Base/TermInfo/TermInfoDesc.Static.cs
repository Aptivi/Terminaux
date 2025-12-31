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
        /// if the loading succeeded, or <c>null</c> if the loading failed.
        /// </param>
        /// <returns><c>true</c> if the terminfo description was loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad(out TermInfoDesc? result)
        {
            try
            {
                result = Load();
                ConsoleLogger.Debug("Loaded terminfo instance is not null: {0}", result is not null);
                return result != null;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Can't load terminfo instance.");
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
                ConsoleLogger.Debug("Loaded terminfo instance for {0} is not null: {1}", name, result is not null);
                return result != null;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Can't load terminfo instance {0}.", name);
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
                ConsoleLogger.Debug("Loaded terminfo instance in {0} bytes is not null: {1}", stream.Length, result is not null);
                return result != null;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Can't load terminfo instance in {0} bytes.", stream.Length);
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
            // Builtins tracked from https://github.com/ThomasDickey/ncurses-snapshots.git. To learn more, consult
            // TermInfoLoader.cs, function Load(string).
            string[] builtins = GetBuiltinPaths();
            List<string> names = [];
            foreach (string builtin in builtins)
            {
                if (!builtin.VerifyPrefix("Terminaux.Resources.TermFiles."))
                    continue;
                string termNamePath = builtin.RemovePrefix("Terminaux.Resources.TermFiles.");
                string termName = termNamePath.Substring(termNamePath.IndexOf('.') + 1);
                ConsoleLogger.Debug("Adding builtin terminfo: {0} (path: {1}, {2})", termName, termNamePath, builtin);
                names.Add(termName);
            }
            ConsoleLogger.Info("{0} terminfo names", names.Count);
            return [.. names];
        }

        internal static string[] GetBuiltinPaths()
        {
            string[] builtins = typeof(TermInfoDesc).Assembly.GetManifestResourceNames();
            return builtins;
        }
    }
}
