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

using System.Threading;
using Terminaux.Base.Extensions;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Consoles
{
    internal class ProgressWarnFailTest : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Console;

        public void RunFixture()
        {
            TextWriterRaw.WritePlain("Progress bar is running from 0 to 75, increasing 10 notches every 1 second.");
            for (int progress = 0; progress < 75; progress++)
            {
                var progressState = progress >= 50 ? ConsoleTaskbarProgressEnum.Warning : ConsoleTaskbarProgressEnum.Normal;
                ConsoleTaskbarProgress.SetProgress(progressState, progress);
                Thread.Sleep(100);
            }
            ConsoleTaskbarProgress.SetProgress(ConsoleTaskbarProgressEnum.Error, 75);
            TextWriterRaw.WritePlain("Failed!");
            Thread.Sleep(3000);
            ConsoleTaskbarProgress.SetProgress(ConsoleTaskbarProgressEnum.NoProgress);
        }
    }
}