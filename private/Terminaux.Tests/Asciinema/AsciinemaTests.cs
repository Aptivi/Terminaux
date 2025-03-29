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
            parsed.Version.ShouldBe(1);
            parsed.Width.ShouldBe(80);
            parsed.Height.ShouldBe(24);
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
            parsed.Version.ShouldBe(2);
            parsed.Width.ShouldBe(80);
            parsed.Height.ShouldBe(24);
        }
    }
}
