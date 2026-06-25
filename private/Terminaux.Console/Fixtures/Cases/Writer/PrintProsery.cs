//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintProsery : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var proseryRenderer = new Prosery()
            {
                Prose =
                    """


                    Even if obstacles are there,
                    we still go on with prosperity.



                    So we try to get there,
                    to see the world with clarity.


                    """
            };
            TextWriterRaw.WriteRaw(proseryRenderer.Render() + "\n\n");
            var proseryRenderer2 = new Prosery()
            {
                Prose =
                    """


                    Even if obstacles are there,
                    we still go on with prosperity.



                    So we try to get there,
                    to see the world with clarity.


                    """,
                WrapLines = true,
                WrapWidth = 20,
            };
            TextWriterRaw.WriteRaw(proseryRenderer2.Render());
        }
    }
}
