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
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs.CJK
{
    internal class TestInputSelectionMultipleDisabledScrollCjk : IFixture
    {
        public FixtureCategory Category => FixtureCategory.InputCjk;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            InputChoiceInfo[] choices =
            [
                new("dapper", "6.04 (Dapper Drake)", "", false, false, true),
                new("hardy", "8.04 (Hardy Heron)", "", false, false, true),
                new("lucid", "10.04 (Lucid Lynx)", "", false, false, true),
                new("precise", "12.04 (Precise Pangolin)", "", false, false, true),
                new("trusty", "14.04 (Trusty Tahr)", "", false, false, true),
                new("xenial", "16.04 (Xenial Xerus)", "", false, false, true),
                new("bionic", "18.04 (Bionic Beaver)", "", false, false, true),
                new("focal", "20.04 (Focal Fossa)"),
                new("jammy", "22.04 (Jammy Jellyfish)"),
                new("noble", "24.04 (Noble Numbat)", "", true),
            ];
            InputChoiceInfo[] altChoices =
            [
                new("warty", "4.10 (Warty Warthog)", "", false, false, true),
                new("hoary", "5.04 (Hoary Hedgehog)", "", false, false, true),
                new("breezy", "5.10 (Breezy Badger)", "", false, false, true),
                new("edgy", "6.10 (Edgy Eft)", "", false, false, true),
                new("feisty", "7.04 (Feisty Fawn)", "", false, false, true),
                new("gutsy", "7.10 (Gutsy Gibbon)", "", false, false, true),
                new("intrepid", "8.10 (Intrepid Ibex)", "", false, false, true),
                new("jaunty", "9.04 (Jaunty Jackalope)", "", false, false, true),
                new("karmic", "9.10 (Karmic Koala)", "", false, false, true),
                new("maverick", "10.10 (Maverick Meerkat)", "", false, false, true),
                new("natty", "11.04 (Natty Narwhal)", "", false, false, true),
                new("oneiric", "11.10 (Oneiric Ocelot)", "", false, false, true),
                new("quantal", "12.10 (Quantal Quetzal)", "", false, false, true),
                new("raring", "13.04 (Raring Ringtail)", "", false, false, true),
                new("saucy", "13.10 (Saucy Salamander)", "", false, false, true),
                new("utopic", "14.10 (Utopic Unicorn)", "", false, false, true),
                new("vivid", "15.04 (Vivid Vervet)", "", false, false, true),
                new("wily", "15.10 (Wily Werewolf)", "", false, false, true),
                new("yakkety", "16.10 (Yakkety Yak)", "", false, false, true),
                new("zesty", "17.04 (Zesty Zapus)", "", false, false, true),
                new("artful", "17.10 (Artful Aardvark)", "", false, false, true),
                new("cosmic", "18.10 (Cosmic Cuttlefish)", "", false, false, true),
                new("disco", "19.04 (Disco Dingo)", "", false, false, true),
                new("eoan", "19.10 (Eoan Ermine)", "", false, false, true),
                new("groovy", "20.10 (Groovy Gorilla)", "", false, false, true),
                new("hirsute", "21.04 (Hirsute Hippo)", "", false, false, true),
                new("impish", "21.10 (Impish Indri)", "", false, false, true),
                new("kinetic", "22.10 (Kinetic Kudu)", "", false, false, true),
                new("lunar", "23.04 (Lunar Lobster)", "", false, false, true),
                new("mantic", "23.10 (Mantic Minotaur)"),
            ];
            var answers = SelectionMultipleStyle.PromptMultipleSelection("您想运行哪个 Ubuntu 版本？", choices, altChoices);
            TextWriterColor.Write(string.Join(", ", answers));
            Input.EnableMouse = false;
        }
    }
}
