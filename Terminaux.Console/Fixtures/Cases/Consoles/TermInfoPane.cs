//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using Terminaux.Base;
using System;
using Terminaux.Base.TermInfo;

namespace Terminaux.Console.Fixtures.Cases.Consoles
{
    internal class TermInfoPane : IFixture
    {
        public string FixtureID =>
            "TermInfoPane";

        public FixtureCategory Category => FixtureCategory.Console;

        public void RunFixture()
        {
            // Run the resize listener
            ConsoleResizeHandler.StartResizeListener();
            var tui = new TermInfoPaneData();
            tui.Bindings.Add(new InteractiveTuiBinding<TermInfoDesc>("Custom...", ConsoleKey.C, (_, _, _, _) => tui.ShowCustomInfo()));

            // Start the demo TUI app
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
