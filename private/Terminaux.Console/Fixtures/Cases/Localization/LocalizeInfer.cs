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

using LocaleStation.Tools;
using System.Globalization;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Localization
{
    internal class LocalizeInfer : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Localization;

        public void RunFixture()
        {
            TextWriterColor.Write($"Selecting inferred language according to culture {CultureInfo.CurrentUICulture.Name}...");
            string language = LanguageCommon.GetInferredLanguage("Terminaux");
            LanguageCommon.Language = language;
            TextWriterColor.Write($"Selected language {language}");
        }
    }
}
