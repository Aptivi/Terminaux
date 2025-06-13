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

using LocaleStation.Tools;
using System;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Localized;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Localization
{
    internal class LocalizeManual : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Localization;

        public void RunFixture()
        {
            InputChoiceInfo[] choices = InputChoiceTools.GetInputChoices(LocalStrings.Languages);
            string languageIdxStr = ChoiceStyle.PromptChoice("Choose a language", choices, new()
            {
                PressEnter = true,
                OutputType = ChoiceOutputType.Modern,
            });
            int languageIdx = int.Parse(languageIdxStr);
            string language = LocalStrings.Languages[languageIdx - 1];
            LanguageCommon.Language = language;
            TextWriterColor.Write($"Selected language {language}");
        }
    }
}
