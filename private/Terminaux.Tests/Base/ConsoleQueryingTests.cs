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
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder;
using Textify.General;
using Textify.General.Data;

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
            string expected = "Nitrocid is aweso...";
            string Source = "Nitrocid is awesome and is great!";
            int Target = 20;
            Source = Source.Truncate(Target);
            Source.ShouldBe(expected);
        }

        /// <summary>
        /// Tests truncating...
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestTruncateNoEllipsis()
        {
            string expected = "Nitrocid is awesome ";
            string Source = "Nitrocid is awesome and is great!";
            int Target = 20;
            Source = Source.Truncate(Target, false);
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
            int actual = TextTools.GetCharWidth(c);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting the Unicode character widths
        /// </summary>
        [TestMethod]
        [DataRow(null, CharWidthType.NonPrinting)]     // NULL character
        [DataRow('\0', CharWidthType.NonPrinting)]     // NULL character
        [DataRow('\b', CharWidthType.NonPrinting)]     // BEEP character
        [DataRow('\u001A', CharWidthType.NonPrinting)] // SUBSTITUTE character
        [DataRow('A', (CharWidthType)(-1))]
        [DataRow('a', (CharWidthType)(-1))]
        [DataRow('1', (CharWidthType)(-1))]
        [DataRow('?', (CharWidthType)(-1))]
        [DataRow('*', (CharWidthType)(-1))]
        [DataRow('你', CharWidthType.DoubleWidth)]
        [DataRow('！', CharWidthType.DoubleWidth)]
        [DataRow('\u200b', CharWidthType.NonPrinting)] // ZERO-WIDTH SPACE character
        [Description("Querying")]
        public void TestUnicodeCharWidthTypes(char c, CharWidthType expected)
        {
            var actual = TextTools.GetCharWidthType(c);
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

        // Emoji should take two cells, as they can't be expressed by just one cell, and they are surrogate pairs.
        [DataRow("😀", 0, 2)]
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

        // Emoji should take two cells, as they can't be expressed by just one cell, and they are surrogate pairs.
        [DataRow("😀", 1, 2)]
        [Description("Querying")]
        public void TestEstimateFullWidths(string sentence, int expectedWidths, int expectedLength)
        {
            int actualWidths = ConsoleChar.EstimateFullWidths(sentence);
            int actualLength = string.IsNullOrEmpty(sentence) ? 0 : sentence.Length;
            actualWidths.ShouldBe(expectedWidths);
            actualLength.ShouldBe(expectedLength);
        }

        /// <summary>
        /// Tests reversing the right-to-left characters in a string (for terminal printing)
        /// </summary>
        [TestMethod]
        [DataRow(null, "")]
        [DataRow("", "")]
        [DataRow("\u200b", "\u200b")]
        [DataRow("Hello!", "Hello!")]
        [DataRow("H\u200bello!", "H\u200bello!")]
        [DataRow(null, "", true)]
        [DataRow("", "", true)]
        [DataRow("\u200b", "\u200b", true)]
        [DataRow("Hello!", "Hello!", true)]
        [DataRow("H\u200bello!", "H\u200bello!", true)]

        // Chinese and Korean should not be reversed.
        [DataRow("你好！", "你好！")]
        [DataRow("\u200b你好！", "\u200b你好！")]
        [DataRow("你好!", "你好!")]
        [DataRow("\u200b你好!", "\u200b你好!")]
        [DataRow("Terminaux는 최고입니다!", "Terminaux는 최고입니다!")]
        [DataRow("\u200bTerminaux는 최고입니다!", "\u200bTerminaux는 최고입니다!")]
        [DataRow("你好！", "你好！", true)]
        [DataRow("\u200b你好！", "\u200b你好！", true)]
        [DataRow("你好!", "你好!", true)]
        [DataRow("\u200b你好!", "\u200b你好!", true)]
        [DataRow("Terminaux는 최고입니다!", "Terminaux는 최고입니다!", true)]
        [DataRow("\u200bTerminaux는 최고입니다!", "\u200bTerminaux는 최고입니다!", true)]

        // Arabic should be reversed, preserving the order of English characters.
        [DataRow("Terminaux رائع!", "Terminaux عئار!")]
        [DataRow("Terminaux رائع! Terminaux رائع!", "Terminaux عئار! Terminaux عئار!")]
        [DataRow("\u200bTerminaux رائع!", "\u200bTerminaux عئار!")]
        [DataRow("\u200bTerminaux رائع! Terminaux رائع!", "\u200bTerminaux عئار! Terminaux عئار!")]
        [DataRow("Terminaux رائع!", "Terminaux رائع!", true)]
        [DataRow("Terminaux رائع! Terminaux رائع!", "Terminaux رائع! Terminaux رائع!", true)]
        [DataRow("\u200bTerminaux رائع!", "\u200bTerminaux رائع!", true)]
        [DataRow("\u200bTerminaux رائع! Terminaux رائع!", "\u200bTerminaux رائع! Terminaux رائع!", true)]

        // Arabic with formatters. The "Aldammatun (وٌ)" should not be affected.
        [DataRow("Terminaux رائعٌ!", "Terminaux عٌئار!")]
        [DataRow("Terminaux رائعٌ! Terminaux رائعٌ!", "Terminaux عٌئار! Terminaux عٌئار!")]
        [DataRow("\u200bTerminaux رائعٌ!", "\u200bTerminaux عٌئار!")]
        [DataRow("\u200bTerminaux رائعٌ! Terminaux رائعٌ!", "\u200bTerminaux عٌئار! Terminaux عٌئار!")]
        [DataRow("Terminaux رائعٌ!", "Terminaux رائعٌ!", true)]
        [DataRow("Terminaux رائعٌ! Terminaux رائعٌ!", "Terminaux رائعٌ! Terminaux رائعٌ!", true)]
        [DataRow("\u200bTerminaux رائعٌ!", "\u200bTerminaux رائعٌ!", true)]
        [DataRow("\u200bTerminaux رائعٌ! Terminaux رائعٌ!", "\u200bTerminaux رائعٌ! Terminaux رائعٌ!", true)]

        // Emoji should be unaffected.
        [DataRow("😀", "😀")]
        [DataRow("😀", "😀", true)]
        [Description("Querying")]
        public void TestReverseRtl(string sentence, string expectedSentence, bool terminalReverses = false)
        {
            ConsoleMisc.TerminalReversesRtlText = terminalReverses;
            string actualSentence = ConsoleMisc.ReverseRtl(sentence);
            ConsoleMisc.TerminalReversesRtlText = false;
            actualSentence.ShouldBe(expectedSentence);
        }

    }
}
