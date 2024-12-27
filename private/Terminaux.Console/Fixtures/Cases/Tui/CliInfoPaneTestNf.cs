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
using Terminaux.Inputs.Pointer;
using Terminaux.ResizeListener;

namespace Terminaux.Console.Fixtures.Cases.Tui
{
    internal class CliInfoPaneTestNf : IFixture
    {
        public void RunFixture()
        {
            PointerListener.StartListening();

            // Run the resize listener
            ConsoleResizeListener.StartResizeListener();

            // Start the demo TUI app
            InteractiveTuiTools.OpenInteractiveTui(new CliInfoPaneNfTestData());
            PointerListener.StopListening();
        }
    }
}
