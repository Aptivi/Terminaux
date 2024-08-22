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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Consoles
{
    internal class Sizes : IFixture
    {
        public void RunFixture()
        {
            int oldWidth = ConsoleWrapper.WindowWidth;
            int oldHeight = ConsoleWrapper.WindowHeight;
            TextWriterRaw.WritePlain("Console resizing starts now...\n");
            TextWriterRaw.WritePlain($"{oldWidth}, {oldHeight}");
            Input.ReadKey();
            ConsoleWrapper.SetWindowDimensions(80, 24);
            TextWriterRaw.WritePlain("Should be 80x24 below:");
            TextWriterRaw.WritePlain($"{ConsoleWrapper.WindowWidth}, {ConsoleWrapper.WindowHeight}");
            Input.ReadKey();
            ConsoleWrapper.SetWindowDimensions(120, 30);
            TextWriterRaw.WritePlain("Should be 120x30 below:");
            TextWriterRaw.WritePlain($"{ConsoleWrapper.WindowWidth}, {ConsoleWrapper.WindowHeight}");
            Input.ReadKey();
            ConsoleWrapper.SetWindowDimensions(oldWidth, oldHeight);
            ConsoleMisc.ShowMainBuffer();
            TextWriterRaw.WritePlain($"{ConsoleWrapper.WindowWidth}, {ConsoleWrapper.WindowHeight}");
            TextWriterRaw.WritePlain("Testing is complete!");
        }
    }
}
