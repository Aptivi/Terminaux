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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Base.Extensions;

namespace Terminaux.Tests.Base
{
    [TestClass]
    public class ConsoleExtensionTests
    {
        /// <summary>
        /// Tests getting synth info from the JSON representation
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetSynthInfo()
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
            synthInfo.ShouldNotBeNull();
            synthInfo.Name.ShouldBe("Test synth info");
            synthInfo.Chapters.Length.ShouldBe(2);
            synthInfo.Chapters[0].Name.ShouldBe("Chapter 1");
            synthInfo.Chapters[0].Synths.Length.ShouldBe(5);
            synthInfo.Chapters[0].Synths[0].ShouldBe("128 100");
            synthInfo.Chapters[1].Name.ShouldBe("Chapter 2");
            synthInfo.Chapters[1].Synths.Length.ShouldBe(5);
            synthInfo.Chapters[1].Synths[0].ShouldBe("256 100");
        }
    }
}
