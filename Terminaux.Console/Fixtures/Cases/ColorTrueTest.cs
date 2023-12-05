
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Terminaux.Colors;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class ColorTrueTest : IFixture
    {
        public string FixtureID => "ColorTrueTest";
        public void RunFixture()
        {
            string TextR = Input.ReadLine("R - " + "Write a color number ranging from 1 to 255: ");
            string TextG = Input.ReadLine("G - " + "Write a color number ranging from 1 to 255: ");
            string TextB = Input.ReadLine("B - " + "Write a color number ranging from 1 to 255: ");
            if (int.TryParse(TextR, out int r) && int.TryParse(TextG, out int g) && int.TryParse(TextB, out int b))
            {
                var color = new Color(r, g, b);
                TextWriterColor.WriteColor("Color {0}", true, color, vars: [color.PlainSequence]);
            }
        }
    }
}
