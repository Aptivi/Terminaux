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

using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Consoles
{
    internal class AlternateBuffers : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Console;

        public void RunFixture()
        {
            TextWriterRaw.WritePlain("Alternate buffers are not supported on Windows, so you'll see five lines in this screen.\n");
            TextWriterRaw.WritePlain("This is a sample text written in the main buffer.");
            Input.ReadKey();
            ConsoleMisc.ShowAltBuffer();
            TextWriterRaw.WritePlain("This is a sample text written in the alternate buffer.");
            Input.ReadKey();
            ConsoleMisc.ShowMainBuffer();
            TextWriterRaw.WritePlain("This is the second line in the main buffer.");
            Input.ReadKey();
            ConsoleMisc.ShowAltBuffer();
            TextWriterRaw.WritePlain("This is the second line in the alternate buffer written in the first line.");
            Input.ReadKey();
            ConsoleMisc.ShowMainBuffer();
            TextWriterRaw.WritePlain("Testing is complete!");
        }
    }
}
