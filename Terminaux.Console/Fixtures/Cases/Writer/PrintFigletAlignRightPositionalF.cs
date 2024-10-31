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
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.MiscWriters.Tools;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintFigletAlignRightPositionalF : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var font = FigletFonts.TryGetByName("banner3");
            if (font is null)
                return;
            AlignedFigletTextColor.WriteAlignedColor(5, font, "Hello, {0}!", new Color(ConsoleColors.Green), TextAlignment.Right, 20, 10, Vars: "Terminaux");
        }
    }
}
