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
using Terminaux.Base;
using Terminaux.Console.Fixtures.Cases.CaseData;
using Terminaux.Inputs;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Console.Fixtures.Cases.Tui
{
    internal class CliDoublePaneSlowTest : IFixture
    {
        public string FixtureID =>
            "CliDoublePaneSlowTest";

        public FixtureCategory Category => FixtureCategory.TextualUi;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            // Run the resize listener
            ConsoleResizeHandler.StartResizeListener();

            // Assign keybindings
            var tui = new CliDoublePaneSlowTestData();
            tui.Bindings.Add(new InteractiveTuiBinding<string, string>("Add", ConsoleKey.F1, (_, index, _, index2) => tui.Add(index, index2), true));
            tui.Bindings.Add(new InteractiveTuiBinding<string, string>("Delete", ConsoleKey.F2, (_, index, _, index2) => tui.Remove(index, index2), true));
            tui.Bindings.Add(new InteractiveTuiBinding<string, string>("Delete Last", ConsoleKey.F3, (_, _, _, _) => tui.RemoveLast(), true));
            tui.BindingsFirstPane.Add(new InteractiveTuiBinding<string>("Information", ConsoleKey.F4, (str, _, _, _) => InfoBoxModalColor.WriteInfoBoxModal(str ?? ""), true));
            tui.BindingsSecondPane.Add(new InteractiveTuiBinding<string>("Length", ConsoleKey.F4, (_, _, str, _) => InfoBoxModalColor.WriteInfoBoxModal($"len={(str ?? "").Length}"), true));

            // Start the demo TUI app
            InteractiveTuiTools.OpenInteractiveTui(tui);
            Input.EnableMouse = false;
        }
    }
}
