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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintWrappedChar : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            string[] sentences = ConsoleMisc.GetWrappedSentences($"A long text with {new Color(ConsoleColors.Green).VTSequenceForeground}{new Color(ConsoleColors.Green).VTSequenceForeground}the green foreground color {ColorTools.RenderResetForeground()}that is now reset to the {new Color(ConsoleColors.Green).VTSequenceForeground}current {ColorTools.RenderResetForeground()}foreground color as specified by the console. This is a very long text intended to test an edge-case involving the {new Color(ConsoleColors.Green).VTSequenceForeground}wrapped writer {ColorTools.RenderResetForeground()}character-wise.", ConsoleWrapper.WindowWidth);
            foreach (string sentence in sentences)
                TextWriterRaw.WritePlain(sentence);
        }
    }
}
