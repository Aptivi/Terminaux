﻿//
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
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintRtl : IFixture
    {
        public void RunFixture()
        {
            TextWriterColor.WriteColor("Note that RTL rendering on terminals is extremely limited.", true, new Color(ConsoleColors.Yellow));
            TextWriterColor.WriteColor("Our reverse: السلام عليكم! Terminaux رائعٌ!", true, new Color(ConsoleColors.Lime));
            ConsoleMisc.TerminalReversesRtlText = true;
            TextWriterColor.WriteColor("Emu reverse: السلام عليكم! Terminaux رائعٌ!", true, new Color(ConsoleColors.Green));
            ConsoleMisc.TerminalReversesRtlText = false;
        }
    }
}
