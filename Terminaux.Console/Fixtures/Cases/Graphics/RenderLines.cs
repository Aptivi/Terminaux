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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Graphics;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using System;
using Terminaux.Base;
using Terminaux.Reader;

namespace Terminaux.Console.Fixtures.Cases.Graphics
{
    internal class RenderLines : IFixture
    {
        public void RunFixture()
        {
            bool bail = false;
            var rng = new Random();
            while (!bail)
            {
                ColorTools.SetConsoleColorDry(ConsoleColors.Green, true);
                (int, int) startPoint1 = (rng.Next(ConsoleWrapper.WindowWidth), rng.Next(ConsoleWrapper.WindowHeight));
                (int, int) endPoint1 = (rng.Next(ConsoleWrapper.WindowWidth), rng.Next(ConsoleWrapper.WindowHeight));
                (int, int) startPoint2 = (rng.Next(ConsoleWrapper.WindowWidth), rng.Next(ConsoleWrapper.WindowHeight));
                (int, int) endPoint2 = (rng.Next(ConsoleWrapper.WindowWidth), rng.Next(ConsoleWrapper.WindowHeight));
                string line1 = GraphicsTools.RenderLine(startPoint1, endPoint1);
                string line2 = GraphicsTools.RenderLine(startPoint2, endPoint2);
                TextWriterRaw.WriteRaw(line1);
                TextWriterRaw.WriteRaw(line2);
                ColorTools.ResetBackground();
                var cki = TermReader.ReadKey();
                if (cki.Key == ConsoleKey.Escape)
                    bail = true;
            }
        }
    }
}
