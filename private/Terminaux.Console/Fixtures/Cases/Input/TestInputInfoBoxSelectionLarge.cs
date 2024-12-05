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

using System.Collections.Generic;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Input
{
    internal class TestInputInfoBoxSelectionLarge : IFixture
    {
        public void RunFixture()
        {
            PointerListener.StartListening();
            var choices = new List<InputChoiceInfo>();
            for (int i = 0; i < 1000; i++)
                choices.Add(new InputChoiceInfo($"{i + 1}", $"Number #{i + 1}"));
            int selected = InfoBoxSelectionColor.WriteInfoBoxSelection(nameof(TestInputInfoBoxSelectionLarge), [.. choices], "Select a number");
            TextWriterWhereColor.WriteWhere($"{selected}", 0, 0);
            PointerListener.StopListening();
        }
    }
}
