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

using Terminaux.Console.Fixtures.Cases.CaseData;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs;
using Terminaux.Base;
using System;

namespace Terminaux.Console.Fixtures.Cases.Tui
{
    internal class CliDoublePaneTest : IFixture
    {
        public string FixtureID =>
            "CliDoublePaneTest";

        public FixtureCategory Category => FixtureCategory.TextualUi;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            // Run the resize listener
            ConsoleResizeHandler.StartResizeListener();
            var tui = new CliDoublePaneTestData();
            tui.Bindings.Add(new InteractiveTuiBinding<string>("Add", ConsoleKey.F1, (_, index, _, index2) => tui.Add(index, index2), true));
            tui.Bindings.Add(new InteractiveTuiBinding<string>("Delete", ConsoleKey.F2, (_, index, _, index2) => tui.Remove(index, index2), true));
            tui.Bindings.Add(new InteractiveTuiBinding<string>("Delete Last", ConsoleKey.F3, (_, _, _, _) => tui.RemoveLast(), true));

            // Start the demo TUI app
            InteractiveTuiTools.OpenInteractiveTui(tui);
            Input.EnableMouse = false;
        }
    }
}
