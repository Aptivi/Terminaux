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
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Tests.Asciinema
{
    [TestClass]
    public class AsciinemaTests
    {
        /// <summary>
        /// Tests parsing an Asciinema v1 representation
        /// </summary>
        [TestMethod]
        [Description("Parsing")]
        public void TestAsciinemaV1Parse()
        {
            // Example taken from the official docs: https://docs.asciinema.org/manual/asciicast/v1/
            string representation =
                """
                {
                  "version": 1,
                  "width": 80,
                  "height": 24,
                  "duration": 1.515658,
                  "command": "/bin/zsh",
                  "title": "",
                  "env": {
                    "TERM": "xterm-256color",
                    "SHELL": "/bin/zsh"
                  },
                  "stdout": [
                    [
                      0.248848,
                      "\u001b[1;31mHello \u001b[32mWorld!\u001b[0m\n"
                    ],
                    [
                      1.001376,
                      "I am \rThis is on the next line."
                    ]
                  ]
                }
                """;

            // Run the representation parser
            var parsed = AsciinemaRepresentation.GetRepresentationFromContents(representation);

            // Test general properties
            parsed.Version.ShouldBe(1);
            parsed.Width.ShouldBe(80);
            parsed.Height.ShouldBe(24);

            // Test type correctness
            parsed.Asciicast.ShouldNotBeNull();
            parsed.Asciicast.ShouldBeOfType<AsciicastV1>();

            // Test individual properties
            var asciiCastV1 = parsed.Asciicast as AsciicastV1;
            asciiCastV1?.Duration.ShouldBe(1.515658f);
            asciiCastV1?.Command.ShouldBe("/bin/zsh");
            asciiCastV1?.Title.ShouldBe("");
            asciiCastV1?.Environment.ShouldNotBeNull();
            asciiCastV1?.Environment?.Terminal.ShouldBe("xterm-256color");
            asciiCastV1?.Environment?.Shell.ShouldBe("/bin/zsh");
            asciiCastV1?.StdOutData.ShouldNotBeEmpty();
            asciiCastV1?.StdOutData.Count.ShouldBe(2);
            asciiCastV1?.StdOutData[0].Item1.ShouldBe(0.248848);
            asciiCastV1?.StdOutData[0].Item2.ShouldBe("o");
            asciiCastV1?.StdOutData[0].Item3.ShouldBe("\u001b[1;31mHello \u001b[32mWorld!\u001b[0m\n");
            asciiCastV1?.StdOutData[1].Item1.ShouldBe(1.001376);
            asciiCastV1?.StdOutData[1].Item2.ShouldBe("o");
            asciiCastV1?.StdOutData[1].Item3.ShouldBe("I am \rThis is on the next line.");
        }

        /// <summary>
        /// Tests parsing an Asciinema v2 representation
        /// </summary>
        [TestMethod]
        [Description("Parsing")]
        public void TestAsciinemaV2Parse()
        {
            // Example taken from the official docs: https://docs.asciinema.org/manual/asciicast/v2/
            string representation =
                """
                {"version": 2, "width": 80, "height": 24, "timestamp": 1504467315, "title": "Demo", "env": {"TERM": "xterm-256color", "SHELL": "/bin/zsh"}}
                [0.248848, "o", "\u001b[1;31mHello \u001b[32mWorld!\u001b[0m\n"]
                [1.001376, "o", "That was ok\rThis is better."]
                [1.500000, "m", ""]
                [2.143733, "o", "Now... "]
                [4.050000, "r", "80x24"]
                [6.541828, "o", "Bye!"]
                """;

            // Run the representation parser
            var parsed = AsciinemaRepresentation.GetRepresentationFromContents(representation);

            // Test general properties
            parsed.Version.ShouldBe(2);
            parsed.Width.ShouldBe(80);
            parsed.Height.ShouldBe(24);

            // Test type correctness
            parsed.Asciicast.ShouldNotBeNull();
            parsed.Asciicast.ShouldBeOfType<AsciicastV2>();

            // Test individual properties
            var asciiCastV2 = parsed.Asciicast as AsciicastV2;
            asciiCastV2?.Duration.ShouldBe(0);
            asciiCastV2?.Timestamp.ShouldBe(1504467315);
            asciiCastV2?.IdleTimeLimit.ShouldBe(0);
            asciiCastV2?.Command.ShouldBe("");
            asciiCastV2?.Title.ShouldBe("Demo");
            asciiCastV2?.Environment.ShouldNotBeNull();
            asciiCastV2?.Environment?.Terminal.ShouldBe("xterm-256color");
            asciiCastV2?.Environment?.Shell.ShouldBe("/bin/zsh");
            asciiCastV2?.Theme.ShouldBeNull();
            asciiCastV2?.StdOutData.ShouldNotBeEmpty();
            asciiCastV2?.StdOutData.Count.ShouldBe(6);
            asciiCastV2?.StdOutData[0].Item1.ShouldBe(0.248848);
            asciiCastV2?.StdOutData[0].Item2.ShouldBe("o");
            asciiCastV2?.StdOutData[0].Item3.ShouldBe("\u001b[1;31mHello \u001b[32mWorld!\u001b[0m\n");
            asciiCastV2?.StdOutData[1].Item1.ShouldBe(1.001376);
            asciiCastV2?.StdOutData[1].Item2.ShouldBe("o");
            asciiCastV2?.StdOutData[1].Item3.ShouldBe("That was ok\rThis is better.");
            asciiCastV2?.StdOutData[2].Item1.ShouldBe(1.500000);
            asciiCastV2?.StdOutData[2].Item2.ShouldBe("m");
            asciiCastV2?.StdOutData[2].Item3.ShouldBe("");
            asciiCastV2?.StdOutData[3].Item1.ShouldBe(2.143733);
            asciiCastV2?.StdOutData[3].Item2.ShouldBe("o");
            asciiCastV2?.StdOutData[3].Item3.ShouldBe("Now... ");
            asciiCastV2?.StdOutData[4].Item1.ShouldBe(4.050000);
            asciiCastV2?.StdOutData[4].Item2.ShouldBe("r");
            asciiCastV2?.StdOutData[4].Item3.ShouldBe("80x24");
            asciiCastV2?.StdOutData[5].Item1.ShouldBe(6.541828);
            asciiCastV2?.StdOutData[5].Item2.ShouldBe("o");
            asciiCastV2?.StdOutData[5].Item3.ShouldBe("Bye!");
        }
    }
}
