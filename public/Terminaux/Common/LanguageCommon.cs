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
using System.Collections.Generic;
using System.Globalization;
using Terminaux.Base;
using Terminaux.Localized;

namespace Terminaux.Common
{
    /// <summary>
    /// Language common tools
    /// </summary>
    public static class LanguageCommon
    {
        // TODO: Once tests prove stability, move them to supplemental package of LocaleStation in v1.2.0.
        private static readonly Dictionary<string, LanguageLocalActions> localActions = new()
        {
            { "Terminaux", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists) },
        };
        private static readonly Dictionary<string, LanguageLocalActions> localCustomActions = [];
        private static string language = "eng";

        /// <summary>
        /// Language to use
        /// </summary>
        public static string Language
        {
            get => language;
            set => language = value;
        }

        /// <summary>
        /// Inferred language according to the current UI culture
        /// </summary>
        /// <param name="localActions">Localization actions</param>
        public static string GetInferredLanguage(string localActions = "") =>
            GetInferredLanguages(localActions)[0];

        /// <summary>
        /// Inferred languages according to the current UI culture
        /// </summary>
        /// <param name="localActions">Localization actions</param>
        public static string[] GetInferredLanguages(string localActions = "")
        {
            var currentCulture = CultureInfo.CurrentUICulture;
            string cultureName = currentCulture.Name;
            var langActions = GetActions(localActions);
            var cultLangs = langActions.ListLanguagesCulture.Invoke(cultureName);

            // Usually only one
            if (cultLangs.Length > 0)
                return cultLangs;
            return ["eng"];
        }

        /// <summary>
        /// Translates the string using a specified localization ID
        /// </summary>
        /// <param name="locId">Localization ID</param>
        /// <returns>Translated string</returns>
        public static string Translate(string locId) =>
            Translate(locId, "Terminaux", Language);

        /// <summary>
        /// Translates the string using a specified localization ID
        /// </summary>
        /// <param name="locId">Localization ID</param>
        /// <param name="actions">Local actions</param>
        /// <returns>Translated string</returns>
        public static string Translate(string locId, string actions) =>
            Translate(locId, actions, Language);

        /// <summary>
        /// Translates the string using a specified localization ID
        /// </summary>
        /// <param name="locId">Localization ID</param>
        /// <param name="actions">Local actions</param>
        /// <param name="language">Language to use</param>
        /// <returns>Translated string</returns>
        public static string Translate(string locId, string actions, string language)
        {
            var langActions = GetActions(actions);
            var translated = langActions.Translate.Invoke(locId, language);
            return translated;
        }

        /// <summary>
        /// Is the custom actions built-in?
        /// </summary>
        /// <param name="name">Name of the actions</param>
        /// <returns>True if built-in; false otherwise</returns>
        public static bool IsCustomActionsBuiltin(string name) =>
            localActions.ContainsKey(name);

        /// <summary>
        /// Is the custom actions defined?
        /// </summary>
        /// <param name="name">Name of the actions</param>
        /// <returns>True if defined; false otherwise</returns>
        public static bool IsCustomActionsDefined(string name) =>
            IsCustomActionsBuiltin(name) || localCustomActions.ContainsKey(name);

        /// <summary>
        /// Adds a custom actions for language translation
        /// </summary>
        /// <param name="name">Name of the actions</param>
        /// <param name="actions">Actions that contains localized strings generated by Localizer</param>
        /// <exception cref="TerminauxException"></exception>
        public static void AddCustomActions(string name, LanguageLocalActions actions)
        {
            if (IsCustomActionsDefined(name))
                throw new TerminauxException("Custom language actions {0} is already defined", name);
            localCustomActions.Add(name, actions);
        }

        /// <summary>
        /// Removes a custom actions for language translation
        /// </summary>
        /// <param name="name">Name of the actions</param>
        /// <exception cref="TerminauxException"></exception>
        public static void RemoveCustomActions(string name)
        {
            if (!IsCustomActionsDefined(name))
                throw new TerminauxException("Custom language actions {0} is not defined", name);
            if (IsCustomActionsBuiltin(name))
                throw new TerminauxException("Custom language actions {0} is builtin and thus cannot be removed", name);
            localCustomActions.Remove(name);
        }

        /// <summary>
        /// Gets the language actions
        /// </summary>
        /// <param name="name">Name of the actions</param>
        /// <returns>A <see cref="LanguageLocalActions"/> object that contains the Translate() function</returns>
        /// <exception cref="TerminauxException"></exception>
        public static LanguageLocalActions GetActions(string name)
        {
            if (!localActions.TryGetValue(name, out var actions))
                if (!localCustomActions.TryGetValue(name, out actions))
                    throw new TerminauxException("Language actions {0} is not defined", name);
            return actions;
        }

        internal static void AddBuiltinActions(string name, LanguageLocalActions actions)
        {
            if (IsCustomActionsDefined(name))
                throw new TerminauxException("Builtin language actions {0} is already defined", name);
            localActions.Add(name, actions);
        }

        internal static void RemoveBuiltinActions(string name)
        {
            if (!IsCustomActionsDefined(name))
                throw new TerminauxException("Custom language actions {0} is not defined", name);
            if (!IsCustomActionsBuiltin(name))
                throw new TerminauxException("Custom language actions {0} is custom and thus cannot be removed", name);
            localActions.Remove(name);
        }
    }
}
