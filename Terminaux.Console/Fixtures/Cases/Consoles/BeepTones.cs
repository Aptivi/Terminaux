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
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Consoles
{
    internal class BeepTones : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Console;

        public void RunFixture()
        {
            TextWriterRaw.WritePlain("Beep tones are as follows (freq, ms): [(128, 200), (256, 400), (512, 600)]");
            TextWriterRaw.WritePlain("128, 200...");
            ConsoleWrapper.Beep(128, 200);
            TextWriterRaw.WritePlain("256, 400...");
            ConsoleWrapper.Beep(256, 400);
            TextWriterRaw.WritePlain("512, 600...");
            ConsoleWrapper.Beep(512, 600);
            TextWriterRaw.WritePlain("Testing is complete!");
        }
    }
}
