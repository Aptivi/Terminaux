//
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

using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Input.CJK
{
    internal class TestInputInfoBoxSelectionCjk : IFixture
    {
        public void RunFixture()
        {
            PointerListener.StartListening();

            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            var choices = new InputChoiceInfo[]
            {
                new("focal", "20.04 (Focal Fossa)", "Ubuntu 20.04 LTS，代号 Focal Fossa，是一个长期支持版本，于 2020 年 4 月 23 日发布。"),
                new("jammy", "22.04 (Jammy Jellyfish)", "Ubuntu 22.04 LTS，代号 Jammy Jellyfish，于 2022 年 4 月 21 日发布，是一个长期支持版本，支持期五年，直到 2027 年 4 月。"),
                new("noble", "24.04 (Noble Numbat)", "Ubuntu 24.04 LTS，代号Noble Numbat，计划于2024年4月发布，属于长期支持版本，支持五年，直到2029年4月。", true),
            };
            int selected = InfoBoxSelectionColor.WriteInfoBoxSelection(nameof(TestInputInfoBoxSelectionCjk), choices, "您想运行哪个 Ubuntu 版本？");
            TextWriterWhereColor.WriteWhere($"{selected}", 0, 0);
            PointerListener.StopListening();
        }
    }
}
