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
    internal class TestInputInfoBoxMultiInput : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            var modules = new InputModule[]
            {
                new TextBoxModule()
                {
                    Name = "Text Box Integer",
                    Description = "Type any integer to continue",
                },
                new TextBoxModule()
                {
                    Name = "Text Box String",
                    Description = "Type any string to continue",
                },
                new MaskedTextBoxModule()
                {
                    Name = "Text Box Password",
                    Description = "Type any password to continue",
                },
            };
            InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, nameof(TestInputInfoBoxMultiInput), "Select an input module to test...");
            string[] rendered = [.. modules.Select((im) => im.Value?.ToString() ?? "")];
            TextWriterWhereColor.WriteWhere("  - " + string.Join("\n  - ", rendered), 0, 0);
            Input.EnableMouse = false;
        }
    }
}
