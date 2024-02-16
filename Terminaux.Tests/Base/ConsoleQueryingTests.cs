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
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder;

namespace Terminaux.Tests.Base
{

    [TestClass]
    public class ConsoleQueryingTests
    {

        /// <summary>
        /// Tests getting how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestPercentRepeatTargeted() =>
            ConsoleMisc.PercentRepeatTargeted(25, 200, 100).ShouldBe(12);

        /// <summary>
        /// Tests filtering the VT sequences that matches the regex
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestFilterVTSequences()
        {
            char BellChar = VtSequenceBasicChars.BellChar;
            char EscapeChar = VtSequenceBasicChars.EscapeChar;
            ConsoleMisc.FilterVTSequences($"Hello!{EscapeChar}[38;5;43m").ShouldBe("Hello!");
            ConsoleMisc.FilterVTSequences($"{EscapeChar}]0;This is the title{BellChar}Hello!").ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests getting wrapped sentences
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetWrappedSentences()
        {
            var sentences = ConsoleMisc.GetWrappedSentences("Nitrocid", 4);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(2);
            sentences[0].ShouldBe("Nitr");
            sentences[1].ShouldBe("ocid");
        }

        /// <summary>
        /// Tests getting wrapped sentences
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetWrappedSentencesIndented()
        {
            var sentences = ConsoleMisc.GetWrappedSentences("Nitrocid", 4, 2);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(3);
            sentences[0].ShouldBe("Ni");
            sentences[1].ShouldBe("troc");
            sentences[2].ShouldBe("id");
        }

        /// <summary>
        /// Tests getting wrapped sentences
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetWrappedSentencesByWords()
        {
            var sentences = ConsoleMisc.GetWrappedSentencesByWords("Nitrocid KS kernel sim", 4);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(6);
            sentences[0].ShouldBe("Nitr");
            sentences[1].ShouldBe("ocid");
            sentences[2].ShouldBe("KS");
            sentences[3].ShouldBe("kern");
            sentences[4].ShouldBe("el");
            sentences[5].ShouldBe("sim");
        }

        /// <summary>
        /// Tests getting wrapped sentences
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetWrappedSentencesByWordsIndented()
        {
            var sentences = ConsoleMisc.GetWrappedSentencesByWords("Nitrocid KS kernel sim", 4, 2);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(7);
            sentences[0].ShouldBe("Ni");
            sentences[1].ShouldBe("troc");
            sentences[2].ShouldBe("id");
            sentences[3].ShouldBe("KS");
            sentences[4].ShouldBe("kern");
            sentences[5].ShouldBe("el");
            sentences[6].ShouldBe("sim");
        }

        /// <summary>
        /// Tests truncating...
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestTruncate()
        {
            string expected = "Nitrocid is awesome ...";
            string Source = "Nitrocid is awesome and is great!";
            int Target = 20;
            Source = Source.Truncate(Target);
            Source.ShouldBe(expected);
        }

    }
}
