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
        /// Tests getting wrapped sentences
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetWrappedSentencesByWordsEdgeCase()
        {
            var sentences = ConsoleMisc.GetWrappedSentencesByWords("-------------------------------------------------------------------\r\n\r\nTest text\n    \n\n  Test text 2.", 30);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(8);
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

        /// <summary>
        /// Tests getting the Unicode character widths
        /// </summary>
        [TestMethod]
        [DataRow(null, 0)]     // NULL character
        [DataRow('\0', 0)]     // NULL character
        [DataRow('\b', 0)]     // BEEP character
        [DataRow('\u001A', 0)] // SUBSTITUTE character
        [DataRow('A', 1)]
        [DataRow('a', 1)]
        [DataRow('1', 1)]
        [DataRow('?', 1)]
        [DataRow('*', 1)]
        [DataRow('你', 2)]
        [DataRow('！', 2)]
        [DataRow('\u200b', 0)] // ZERO-WIDTH SPACE character
        [Description("Querying")]
        public void TestUnicodeCharWidths(char c, int expected)
        {
            int actual = ConsoleChar.GetCharWidth(c);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests estimating the Unicode character widths for a sentence
        /// </summary>
        [TestMethod]
        [DataRow(null, 0, 0)]
        [DataRow("", 0, 0)]
        [DataRow("\u200b", 0, 1)]
        [DataRow("Hello!", 6, 6)]
        [DataRow("H\u200bello!", 6, 7)]

        // Chinese and Korean should occupy two cells.
        [DataRow("你好！", 6, 3)]
        [DataRow("\u200b你好！", 6, 4)]
        [DataRow("你好!", 5, 3)]
        [DataRow("\u200b你好!", 5, 4)]
        [DataRow("Terminaux는 최고입니다!", 23, 17)]
        [DataRow("\u200bTerminaux는 최고입니다!", 23, 18)]

        // Arabic should only occupy one cell.
        [DataRow("Terminaux رائع!", 15, 15)]
        [DataRow("\u200bTerminaux رائع!", 15, 16)]

        // Arabic with formatters. The "Aldammatun (وٌ)" should not occupy any cell.
        [DataRow("Terminaux رائعٌ!", 15, 16)]
        [DataRow("\u200bTerminaux رائعٌ!", 15, 17)]

        // Emoji should take two cells, as they can't be expressed by just one cell, and they are surrogate pairs.
        [DataRow("😀", 2, 2)]
        [Description("Querying")]
        public void TestEstimateWidths(string sentence, int expectedCells, int expectedLength)
        {
            int actualCells = ConsoleChar.EstimateCellWidth(sentence);
            int actualLength = string.IsNullOrEmpty(sentence) ? 0 : sentence.Length;
            actualCells.ShouldBe(expectedCells);
            actualLength.ShouldBe(expectedLength);
        }

        /// <summary>
        /// Tests estimating the number of Unicode zero-width characters in a sentence
        /// </summary>
        [TestMethod]
        [DataRow(null, 0, 0)]
        [DataRow("", 0, 0)]
        [DataRow("\u200b", 1, 1)]
        [DataRow("Hello!", 0, 6)]
        [DataRow("H\u200bello!", 1, 7)]

        // Chinese and Korean should occupy two cells.
        [DataRow("你好！", 0, 3)]
        [DataRow("\u200b你好！", 1, 4)]
        [DataRow("你好!", 0, 3)]
        [DataRow("\u200b你好!", 1, 4)]
        [DataRow("Terminaux는 최고입니다!", 0, 17)]
        [DataRow("\u200bTerminaux는 최고입니다!", 1, 18)]

        // Arabic should only occupy one cell.
        [DataRow("Terminaux رائع!", 0, 15)]
        [DataRow("\u200bTerminaux رائع!", 1, 16)]

        // Arabic with formatters. The "Aldammatun (وٌ)" should not occupy any cell.
        [DataRow("Terminaux رائعٌ!", 1, 16)]
        [DataRow("\u200bTerminaux رائعٌ!", 2, 17)]
        [Description("Querying")]
        public void TestEstimateZeroWidths(string sentence, int expectedWidths, int expectedLength)
        {
            int actualWidths = ConsoleChar.EstimateZeroWidths(sentence);
            int actualLength = string.IsNullOrEmpty(sentence) ? 0 : sentence.Length;
            actualWidths.ShouldBe(expectedWidths);
            actualLength.ShouldBe(expectedLength);
        }

        /// <summary>
        /// Tests estimating the number of Unicode full-width characters in a sentence
        /// </summary>
        [TestMethod]
        [DataRow(null, 0, 0)]
        [DataRow("", 0, 0)]
        [DataRow("\u200b", 0, 1)]
        [DataRow("Hello!", 0, 6)]
        [DataRow("H\u200bello!", 0, 7)]

        // Chinese and Korean should occupy two cells.
        [DataRow("你好！", 3, 3)]
        [DataRow("\u200b你好！", 3, 4)]
        [DataRow("你好!", 2, 3)]
        [DataRow("\u200b你好!", 2, 4)]
        [DataRow("Terminaux는 최고입니다!", 6, 17)]
        [DataRow("\u200bTerminaux는 최고입니다!", 6, 18)]

        // Arabic should only occupy one cell.
        [DataRow("Terminaux رائع!", 0, 15)]
        [DataRow("\u200bTerminaux رائع!", 0, 16)]

        // Arabic with formatters. The "Aldammatun (وٌ)" should not occupy any cell.
        [DataRow("Terminaux رائعٌ!", 0, 16)]
        [DataRow("\u200bTerminaux رائعٌ!", 0, 17)]
        [Description("Querying")]
        public void TestEstimateFullWidths(string sentence, int expectedWidths, int expectedLength)
        {
            int actualWidths = ConsoleChar.EstimateFullWidths(sentence);
            int actualLength = string.IsNullOrEmpty(sentence) ? 0 : sentence.Length;
            actualWidths.ShouldBe(expectedWidths);
            actualLength.ShouldBe(expectedLength);
        }

    }
}
