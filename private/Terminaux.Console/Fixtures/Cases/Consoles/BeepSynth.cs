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

using Terminaux.Base.Extensions;

namespace Terminaux.Console.Fixtures.Cases.Consoles
{
    internal class BeepSynth : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Console;

        public void RunFixture()
        {
            string synthJson =
                """
                {
                    "name": "Test synth info",
                    "chapters": [
                        {
                            "name": "Chapter 1",
                            "synths": [
                                "128 100",
                                "256 200",
                                "512 300",
                                "256 400",
                                "128 500",
                            ]
                        },
                        {
                            "name": "Chapter 2",
                            "synths": [
                                "256 100",
                                "512 200",
                                "1024 300",
                                "512 400",
                                "256 500",
                            ]
                        },
                    ]
                }
                """;
            var synthInfo = ConsoleAcoustic.GetSynthInfo(synthJson);
            ConsoleAcoustic.PlaySynth(synthInfo);
        }
    }
}
