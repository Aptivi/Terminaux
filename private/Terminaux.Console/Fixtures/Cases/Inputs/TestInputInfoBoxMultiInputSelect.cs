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

using System.Linq;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestInputInfoBoxMultiInputSelect : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            var modules = new InputModule[]
            {
                new ComboBoxModule()
                {
                    Name = "EDM Music Genre",
                    Description = "Choose one of the EDM music genres to continue",
                    Choices =
                    [
                        new("Selection infobox", [new("Available options",
                        [
                            new("Acid House", ""),
                            new("Big Room", ""),
                            new("Deep House", ""),
                            new("Drum-n-Bass", ""),
                            new("House", ""),
                            new("Techno", ""),
                            new("Trance", ""),
                        ])])
                    ],
                    Value = 6,
                },
                new ComboBoxModule()
                {
                    Name = "Preferred Operating System",
                    Description = "Choose your preferred operating system to continue",
                    Choices =
                    [
                        new("Selection infobox", [new("Available options",
                        [
                            new("Windows", ""),
                            new("macOS", ""),
                            new("Linux", ""),
                        ])])
                    ],
                },
                new MultiComboBoxModule()
                {
                    Name = "Favorite Colors",
                    Description = "Choose your favorite colors",
                    Choices =
                    [
                        new("Selection infobox", [new("Available options",
                        [
                            new("Green", ""),
                            new("Blue", ""),
                            new("Red", ""),
                            new("Fuchsia", ""),
                            new("Aqua", ""),
                            new("Orange", ""),
                            new("Black", ""),
                            new("White", ""),
                        ])])
                    ],
                },
            };
            InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, nameof(TestInputInfoBoxMultiInput), "Select an input module to test...");
            string[] rendered = [.. modules.Select((im) => im.Value is int[] indexes ? string.Join(", ", indexes.Select((idx) => ((MultiComboBoxModule)modules[2]).Choices[0].Groups[0].Choices[idx].ChoiceName)) : im.Value?.ToString() ?? "")];
            TextWriterWhereColor.WriteWhere("  - " + string.Join("\n  - ", rendered), 0, 0);
            Input.EnableMouse = false;
        }
    }
}
