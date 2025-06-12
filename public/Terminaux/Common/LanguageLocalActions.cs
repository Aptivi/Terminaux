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

namespace Terminaux.Common
{
    /// <summary>
    /// Language local actions class
    /// </summary>
    public class LanguageLocalActions
    {
        private readonly Func<string[]> getLanguages = () => [];
        private readonly Func<string[]> getLocalizations = () => [];
        private readonly Func<string, string, string> translate = (_, _) => "";
        private readonly Func<string, string, bool> checkCulture = (_, _) => false;
        private readonly Func<string, string[]> listLanguagesCulture = (_) => [];
        private readonly Func<string, string, bool> exists = (_, _) => false;

        /// <summary>
        /// Queries the languages
        /// </summary>
        public Func<string[]> GetLanguages =>
            getLanguages;

        /// <summary>
        /// Queries the localizations
        /// </summary>
        public Func<string[]> GetLocalizations =>
            getLocalizations;

        /// <summary>
        /// Translates the given string using the string ID in a specific language
        /// </summary>
        public Func<string, string, string> Translate =>
            translate;

        /// <summary>
        /// Checks to see if the given culture in a specified language exists
        /// </summary>
        public Func<string, string, bool> CheckCulture =>
            checkCulture;

        /// <summary>
        /// Lists languages in a given culture
        /// </summary>
        public Func<string, string[]> ListLanguagesCulture =>
            listLanguagesCulture;

        /// <summary>
        /// Checks to see if the given string using the string ID in a specific language exists
        /// </summary>
        public Func<string, string, bool> Exists =>
            exists;

        /// <summary>
        /// Language local actions
        /// </summary>
        /// <param name="getLanguages">Queries the languages</param>
        /// <param name="getLocalizations">Queries the localizations</param>
        /// <param name="translate">Translates the given string using the string ID in a specific language</param>
        /// <param name="checkCulture">Checks to see if the given culture in a specified language exists</param>
        /// <param name="listLanguagesCulture">Lists languages in a given culture</param>
        /// <param name="exists">Checks to see if the given string using the string ID in a specific language exists</param>
        public LanguageLocalActions(Func<string[]> getLanguages, Func<string[]> getLocalizations, Func<string, string, string> translate, Func<string, string, bool> checkCulture, Func<string, string[]> listLanguagesCulture, Func<string, string, bool> exists)
        {
            this.getLanguages = getLanguages;
            this.getLocalizations = getLocalizations;
            this.translate = translate;
            this.checkCulture = checkCulture;
            this.listLanguagesCulture = listLanguagesCulture;
            this.exists = exists;
        }
    }
}
