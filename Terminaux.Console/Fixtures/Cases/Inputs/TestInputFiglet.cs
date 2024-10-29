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

using Textify.Data.Figlet;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestInputFiglet : IFixture
    {
        public void RunFixture()
        {
            Input.EnableMouse = true;
            string font = FigletSelector.PromptForFiglet();
            var figlet = FigletTools.GetFigletFont(font);
            TextWriterColor.Write($"Got figlet font {font}!");
            var figletText = new FigletText(figlet, "Hello!")
            {
                ForegroundColor = ConsoleColors.Green,
            };
            TextWriterRaw.WriteRaw(figletText.Render());
            Input.EnableMouse = false;
        }
    }
}
