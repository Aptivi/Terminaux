
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

using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.ConsoleDemo.Fixtures;
using Terminaux.Reader.Inputs;
using Terminaux.Writer.ConsoleWriters;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class ColorWithResetTest : IFixture
    {
        public string FixtureID => "ColorWithResetTest";
        public void RunFixture()
        {
            string Text = Input.ReadLine("Write a color number ranging from 1 to 255: ");
            if (int.TryParse(Text, out int color))
            {
                var colorInstance = new Color(color);
                ColorTools.LoadBack(ConsoleColors.Blue);
                TextWriterColor.Write("Color {0} [{1}]", true, colorInstance, vars: new object[] { colorInstance.PlainSequence, colorInstance.PlainSequenceTrueColor });
                TextWriterColor.Write("Press any key to reset all.");
                Input.DetectKeypress();
                ConsoleExtensions.ResetAll();
                TextWriterColor.Write("Congrats! You've reset the whole console!");
            }
        }
    }
}
