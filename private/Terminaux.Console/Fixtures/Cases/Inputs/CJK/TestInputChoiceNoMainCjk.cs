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

using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Choice;

namespace Terminaux.Console.Fixtures.Cases.Inputs.CJK
{
    internal class TestInputChoiceNoMainCjk : IFixture
    {
        public FixtureCategory Category => FixtureCategory.InputCjk;

        public void RunFixture()
        {
            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            InputChoiceInfo[] altChoices =
            [
                new("k", "22.10 (Kinetic Kudu)", "Ubuntu 22.10，代号 Kinetic Kudu，是一个临时版本，于 2022 年 10 月 20 日发布。"),
                new("l", "23.04 (Lunar Lobster)", "Ubuntu 23.04 Lunar Lobster 是一个临时版本，计划于 2023 年 4 月 20 日发布。"),
            ];
            var settings = new ChoiceStyleSettings()
            {
                OutputType = ChoiceOutputType.Modern,
            };
            ChoiceStyle.PromptChoice("您想运行哪个 Ubuntu 版本？", [], altChoices, settings);
        }
    }
}
