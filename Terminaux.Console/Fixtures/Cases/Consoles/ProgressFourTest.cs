﻿//
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

using System.Threading;
using Terminaux.Base.Extensions;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Consoles
{
    internal class ProgressFourTest : IFixture
    {
        public void RunFixture()
        {
            TextWriterRaw.WritePlain("Progress bar is running from 0 to 4, increasing 1 notch every 1 second.");
            for (int progress = 0; progress <= 4; progress++)
            {
                ConsoleTaskbarProgress.SetProgress(ConsoleTaskbarProgressEnum.Normal, progress, 4);
                Thread.Sleep(1000);
            }
            ConsoleTaskbarProgress.SetProgress(ConsoleTaskbarProgressEnum.NoProgress);
        }
    }
}