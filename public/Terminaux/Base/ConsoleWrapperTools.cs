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
using Terminaux.Base.Wrappers;

namespace Terminaux.Base
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles and Windows-only features.
    /// </summary>
    public static class ConsoleWrapperTools
    {
        private static bool isOnLocal = false;
        private static string currentWrapper = nameof(BaseConsoleWrapper);
        private static string currentWrapperLocal = nameof(BaseConsoleWrapper);
        private readonly static Dictionary<string, BaseConsoleWrapper> wrappers = new()
        {
            { nameof(BaseConsoleWrapper), new BaseConsoleWrapper() },
            { nameof(Wrappers.Buffered), new Wrappers.Buffered() },
            { nameof(FileSequence), new FileSequence() },
            { nameof(FileWrite), new FileWrite() },
            { nameof(Null), new Null() },
        };
        private readonly static Dictionary<string, BaseConsoleWrapper> customWrappers = [];

        /// <summary>
        /// Wrapper instance
        /// </summary>
        public static BaseConsoleWrapper Wrapper =>
            isOnLocal ? GetWrapper(currentWrapperLocal) : GetWrapper(currentWrapper);

        /// <summary>
        /// Wrapper instance (excluding the local wrapper)
        /// </summary>
        public static BaseConsoleWrapper WrapperNoLocal =>
            GetWrapper(currentWrapper);

        /// <summary>
        /// Checks whether the wrapper is built-in
        /// </summary>
        /// <param name="wrapperName">Name of the console wrapper</param>
        /// <returns>True if built-in; false otherwise.</returns>
        public static bool IsBuiltin(string wrapperName) =>
            wrappers.ContainsKey(wrapperName);

        /// <summary>
        /// Checks whether the wrapper is registered
        /// </summary>
        /// <param name="wrapperName">Name of the console wrapper</param>
        /// <returns>True if registered; false otherwise.</returns>
        public static bool IsRegistered(string wrapperName) =>
            IsBuiltin(wrapperName) || customWrappers.ContainsKey(wrapperName);

        /// <summary>
        /// Gets the wrapper instance
        /// </summary>
        /// <param name="wrapperName">Name of the console wrapper</param>
        /// <returns>A console wrapper</returns>
        /// <exception cref="TerminauxException"></exception>
        public static BaseConsoleWrapper GetWrapper(string wrapperName)
        {
            if (!wrappers.TryGetValue(wrapperName, out var wrapper))
                if (!customWrappers.TryGetValue(wrapperName, out wrapper))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NOTFOUND"));
            return wrapper;
        }

        /// <summary>
        /// Registers the console wrapper by type
        /// </summary>
        /// <param name="wrapperName">Name of the console wrapper</param>
        /// <param name="wrapper">Console wrapper</param>
        /// <exception cref="TerminauxException"></exception>
        public static void RegisterWrapper<T>(string wrapperName, T wrapper)
            where T : BaseConsoleWrapper
        {
            if (IsRegistered(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_ALREADYFOUND"));
            if (string.IsNullOrEmpty(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NAMENEEDED"));
            customWrappers.Add(wrapperName, wrapper);
        }

        /// <summary>
        /// Unregisters the console wrapper by type
        /// </summary>
        /// <param name="wrapperName">Name of the console wrapper</param>
        /// <exception cref="TerminauxException"></exception>
        public static void UnregisterWrapper(string wrapperName)
        {
            if (!IsRegistered(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NOTFOUND"));
            if (string.IsNullOrEmpty(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NAMENEEDED"));
            if (IsBuiltin(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_REMOVEBUILTIN"));
            if (!customWrappers.Remove(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_REMOVALFAILED"));
        }

        /// <summary>
        /// Sets the current console wrapper
        /// </summary>
        /// <param name="wrapperName">Name of the console wrapper</param>
        /// <exception cref="TerminauxException"></exception>
        public static void SetWrapper(string wrapperName)
        {
            if (!IsRegistered(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NOTFOUND"));
            if (string.IsNullOrEmpty(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NAMENEEDED"));
            currentWrapper = wrapperName;
        }

        /// <summary>
        /// Sets the local console wrapper and enters the local scope
        /// </summary>
        /// <param name="wrapperName">Name of the console wrapper</param>
        /// <exception cref="TerminauxException"></exception>
        public static void SetWrapperLocal(string wrapperName)
        {
            if (!IsRegistered(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NOTFOUND"));
            if (string.IsNullOrEmpty(wrapperName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BASE_CONSOLEWRAPPER_EXCEPTION_NAMENEEDED"));
            currentWrapperLocal = wrapperName;
            isOnLocal = true;
        }

        /// <summary>
        /// Unsets the local console wrapper and exits the local scope
        /// </summary>
        /// <exception cref="TerminauxException"></exception>
        public static void UnsetWrapperLocal()
        {
            currentWrapperLocal = nameof(BaseConsoleWrapper);
            isOnLocal = false;
        }
    }
}
