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

using System.Globalization;
using System.Linq;
using Terminaux.Localized;

namespace Terminaux.Common
{
    /// <summary>
    /// Language common tools
    /// </summary>
    public static class LanguageCommon
    {
        private static string language = "eng";

        /// <summary>
        /// Language to use
        /// </summary>
        public static string Language
        {
            get => language;
            set
            {
                // This only checks the main language list. Individual extras may not have this language.
                if (LocalStrings.Languages.Contains(value))
                    language = value;
            }
        }

        /// <summary>
        /// Inferred language according to the current UI culture
        /// </summary>
        public static string InferredLanguage
        {
            get
            {
                var currentCulture = CultureInfo.CurrentUICulture;
                string cultureName = currentCulture.Name;
                var cultLangs = LocalStrings.ListLanguagesCulture(cultureName);

                // Usually only one
                if (cultLangs.Length > 0)
                    return cultLangs[0];
                return "eng";
            }
        }
    }
}
