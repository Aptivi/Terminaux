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

using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestModalInfoBoxOverflow : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            Input.EnableMouse = true;
            InfoBoxModalColor.WriteInfoBoxModal(
                "This info box should have the below text wrap into the boundaries:\n\n" +
                "TerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminaux\n\n" +
                "TerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminaux\n\n" +
                "TerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminauxTerminaux"
                , new InfoBoxSettings()
                {
                    Title = "Overflowing modal information box"
                }
            );
            Input.EnableMouse = false;
            ConsoleColoring.LoadBack();
        }
    }
}
