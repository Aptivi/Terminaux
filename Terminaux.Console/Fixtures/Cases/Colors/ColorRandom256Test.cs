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
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases.Colors
{
    internal class ColorRandom256Test : IFixture
    {
        public string FixtureID => "ColorRandom256Test";
        public void RunFixture()
        {
            TextWriterColor.Write("Press any key to exit.");
            while (!ConsoleWrapper.KeyAvailable)
            {
                if (!SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, 250))
                {
                    var colorInstance = ColorTools.GetRandomColor(ColorType._255Color);
                    var colorInstanceNoBlack = ColorTools.GetRandomColor(ColorType._255Color, false);
                    TextWriterColor.WriteColor("WB Color {0} [{1}] ", false, colorInstance, colorInstance.PlainSequence, colorInstance.PlainSequenceTrueColor);
                    TextWriterColor.WriteColor("NB Color {0} [{1}]", true, colorInstanceNoBlack, colorInstanceNoBlack.PlainSequence, colorInstanceNoBlack.PlainSequenceTrueColor);
                }
            }
            if (ConsoleWrapper.KeyAvailable)
                ConsoleWrapper.ReadKey(true);
        }
    }
}
