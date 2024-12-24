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
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;

namespace Terminaux.Console.Fixtures.Cases.Inputs.CJK
{
    internal class TestInputSelectionScrollCjk : IFixture
    {
        public FixtureCategory Category => FixtureCategory.InputCjk;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            InputChoiceInfo[] choices =
            [
                new("dapper", "6.04 (Dapper Drake)"),
                new("hardy", "8.04 (Hardy Heron)"),
                new("lucid", "10.04 (Lucid Lynx)"),
                new("precise", "12.04 (Precise Pangolin)"),
                new("trusty", "14.04 (Trusty Tahr)"),
                new("xenial", "16.04 (Xenial Xerus)"),
                new("bionic", "18.04 (Bionic Beaver)"),
                new("focal", "20.04 (Focal Fossa)"),
                new("jammy", "22.04 (Jammy Jellyfish)"),
                new("noble", "24.04 (Noble Numbat)", "", true),
            ];
            InputChoiceInfo[] altChoices =
            [
                new("warty", "4.10 (Warty Warthog)"),
                new("hoary", "5.04 (Hoary Hedgehog)"),
                new("breezy", "5.10 (Breezy Badger)"),
                new("edgy", "6.10 (Edgy Eft)"),
                new("feisty", "7.04 (Feisty Fawn)"),
                new("gutsy", "7.10 (Gutsy Gibbon)"),
                new("intrepid", "8.10 (Intrepid Ibex)"),
                new("jaunty", "9.04 (Jaunty Jackalope)"),
                new("karmic", "9.10 (Karmic Koala)"),
                new("maverick", "10.10 (Maverick Meerkat)"),
                new("natty", "11.04 (Natty Narwhal)"),
                new("oneiric", "11.10 (Oneiric Ocelot)"),
                new("quantal", "12.10 (Quantal Quetzal)"),
                new("raring", "13.04 (Raring Ringtail)"),
                new("saucy", "13.10 (Saucy Salamander)"),
                new("utopic", "14.10 (Utopic Unicorn)"),
                new("vivid", "15.04 (Vivid Vervet)"),
                new("wily", "15.10 (Wily Werewolf)"),
                new("yakkety", "16.10 (Yakkety Yak)"),
                new("zesty", "17.04 (Zesty Zapus)"),
                new("artful", "17.10 (Artful Aardvark)"),
                new("cosmic", "18.10 (Cosmic Cuttlefish)"),
                new("disco", "19.04 (Disco Dingo)"),
                new("eoan", "19.10 (Eoan Ermine)"),
                new("groovy", "20.10 (Groovy Gorilla)"),
                new("hirsute", "21.04 (Hirsute Hippo)"),
                new("impish", "21.10 (Impish Indri)"),
                new("kinetic", "22.10 (Kinetic Kudu)"),
                new("lunar", "23.04 (Lunar Lobster)"),
                new("mantic", "23.10 (Mantic Minotaur)"),
            ];
            SelectionStyle.PromptSelection("您想运行哪个 Ubuntu 版本？", choices, altChoices);
            Input.EnableMouse = false;
        }
    }
}
