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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Base.TermInfo;
using Terminaux.Tests.TermInfo.Utilities;

namespace Terminaux.Tests.TermInfo
{
    [TestClass]
    public class TermInfoDataTests
    {
        [TestMethod]
        public void Should_Read_Standard_Format()
        {
            // Given
            var stream = EmbeddedResourceReader.LoadResourceStream("Terminaux.Tests/TermInfo/Data/windows-ansi");

            // When
            var info = TermInfoDesc.Load(stream);

            // Then
            info.Names.Length.ShouldBe(2);
            info.Names[0].ShouldBe("ansi");
            info.Names[1].ShouldBe("ansi/pc-term compatible with color");
            info.ClearScreen.ShouldBe("[H[J");
        }

        [TestMethod]
        [DataRow("xterm+256color", 256)]
        [DataRow("xterm+88color", 88)]
        public void Should_Read_MaxColors(string terminfo, int expected)
        {
            // Given
            var stream = EmbeddedResourceReader.LoadResourceStream($"Terminaux.Tests/TermInfo/Data/{terminfo}");

            // When
            var info = TermInfoDesc.Load(stream);

            // Then
            info.MaxColors.ShouldBe(expected);
        }

        [TestMethod]
        public void Should_Read_Extended_Capabilities()
        {
            // Given
            var stream = EmbeddedResourceReader.LoadResourceStream("Terminaux.Tests/TermInfo/Data/eterm-256color");

            // When
            var info = TermInfoDesc.Load(stream);

            // Then
            info.Names.Length.ShouldBe(2);
            info.Names[0].ShouldBe("Eterm-256color");
            info.Names[1].ShouldBe("Eterm with xterm 256-colors");
            info.Extended.Count.ShouldBe(26);
            info.Extended.GetBoolean("AX").ShouldBe(true);
            info.Extended.GetBoolean("XT").ShouldBe(true);
            info.Extended.GetString("kUP").ShouldBe("\u001b[a");
        }

        [TestMethod]
        public void Should_Read_Extended__Capabilities_Without_String_Values()
        {
            // Given
            var stream = EmbeddedResourceReader.LoadResourceStream("Terminaux.Tests/TermInfo/Data/linux");

            // When
            var info = TermInfoDesc.Load(stream);

            // Then
            info.Names.Length.ShouldBe(2);
            info.Names[0].ShouldBe("linux");
            info.Names[1].ShouldBe("linux console");
            info.Extended.Count.ShouldBe(10);
            info.Extended.GetBoolean("AX").ShouldBe(true);
            info.Extended.GetBoolean("G0").ShouldBe(false);
            info.Extended.GetBoolean("XT").ShouldBe(false);
            info.Extended.GetNum("U8").ShouldBe(1);
        }

        [TestMethod]
        [DataRow("AX", true)]
        [DataRow("ax", null)]
        public void Should_Consider_Extended_Caps_Case_Sensitive(string key, bool? expected)
        {
            // Given
            var stream = EmbeddedResourceReader.LoadResourceStream("Terminaux.Tests/TermInfo/Data/linux");

            // When
            var info = TermInfoDesc.Load(stream);

            // Then
            info.Extended.GetBoolean(key).ShouldBe(expected);
        }
    }
}
