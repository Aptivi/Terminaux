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

using Terminaux.Inputs;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestInputInfoBoxSelectionDisabled : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            var choices = new InputChoiceCategoryInfo[]
            {
                new("End of life", [
                    new("Versions", [
                        new("eoan", "19.10 (Eoan Ermine)", "Ubuntu 19.10 Eoan Ermine is an interim release, released 17 October 2019.", false, false, true),
                        new("groovy", "20.10 (Groovy Gorilla)", "", false, false, true),
                        new("hirsute", "21.04 (Hirsute Hippo)", "", false, false, true),
                    ])
                ]),
                new("Under support", [
                    new("Old releases", [
                        new("focal", "20.04 (Focal Fossa)", "Ubuntu 20.04 LTS, codenamed Focal Fossa, is a long-term support release and was released on 23 April 2020."),
                        new("jammy", "22.04 (Jammy Jellyfish)", "Ubuntu 22.04 LTS, codenamed Jammy Jellyfish, was released on 21 April 2022, and is a long-term support release, supported for five years, until April 2027."),
                    ]),
                    new("Latest release", [
                        new("noble", "24.04 (Noble Numbat)", "Ubuntu 24.04 LTS, codenamed Noble Numbat, is planned to be released on April 2024, and is a long-term support release, supported for five years, until April 2029.", true),
                    ]),
                    new("Latest interim releases", [
                        new("oracular", "24.10 (Oracular Oriole)"),
                        new("plucky", "25.04 (Plucky Puffin)"),
                    ])
                ]),
            };
            int selected = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, "Which Ubuntu version would you like to run?", new InfoBoxSettings()
            {
                Title = nameof(TestInputInfoBoxSelectionDisabled)
            });
            TextWriterWhereColor.WriteWhere($"{selected}", 0, 0);
            Input.EnableMouse = false;
        }
    }
}
