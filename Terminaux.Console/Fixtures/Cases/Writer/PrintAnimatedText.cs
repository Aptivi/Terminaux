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

using System.Threading;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintAnimatedText : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var animatedText = new AnimatedText()
            {
                TextFrames =
                [
                    "H",
                    "He",
                    "Hel",
                    "Hell",
                    "Hello",
                    "Hello ",
                    "Hello W",
                    "Hello Wo",
                    "Hello Wor",
                    "Hello Worl",
                    "Hello World",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World!",
                    "Hello World ",
                    "Hello Worl  ",
                    "Hello Wor   ",
                    "Hello Wo    ",
                    "Hello W     ",
                    "Hello       ",
                    "Hello       ",
                    "Hell        ",
                    "Hel         ",
                    "He          ",
                    "H           ",
                    "            ",
                ],
                Left = 4,
                Top = 2,
                Width = 18,
            };
            for (int i = 0; i < animatedText.TextFrames.Length; i++)
            {
                TextWriterRaw.WriteRaw(animatedText.Render());
                Thread.Sleep(100);
            }
        }
    }
}
