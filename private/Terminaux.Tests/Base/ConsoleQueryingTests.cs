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
using Terminaux.Base.Structures;
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
        /// Tests getting wrapped sentences
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetWrappedSentencesByWordsEdgeCase2()
        {
            string phrase = "Equal length of a word";
            var sentences = ConsoleMisc.GetWrappedSentencesByWords(phrase, phrase.Length);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(1);
            sentences[0].Length.ShouldBe(phrase.Length);
            sentences[0].ShouldBe(phrase);
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
        /// Tests padding
        /// </summary>
        [TestMethod]
        [DataRow(2, 2, 2, 2)]
        [DataRow(2, 2, 2, 0)]
        [DataRow(2, 2, 0, 2)]
        [DataRow(2, 0, 2, 2)]
        [DataRow(0, 2, 2, 2)]
        [DataRow(2, 0, 2, 0)]
        [DataRow(0, 2, 0, 2)]
        [DataRow(0, 0, 0, 0)]
        [Description("Querying")]
        public void TestPadding(int left, int top, int right, int bottom)
        {
            var padding = new Padding(left, top, right, bottom);
            padding.Left.ShouldBe(left);
            padding.Top.ShouldBe(top);
            padding.Right.ShouldBe(right);
            padding.Bottom.ShouldBe(bottom);
        }

        /// <summary>
        /// Tests margins
        /// </summary>
        [TestMethod]
        [DataRow(2, 2, 2, 2, 10, 10, 6, 6)]
        [DataRow(2, 2, 2, 0, 10, 10, 6, 8)]
        [DataRow(2, 2, 0, 2, 10, 10, 8, 6)]
        [DataRow(2, 0, 2, 2, 10, 10, 6, 8)]
        [DataRow(0, 2, 2, 2, 10, 10, 8, 6)]
        [DataRow(2, 0, 2, 0, 10, 10, 6, 10)]
        [DataRow(0, 2, 0, 2, 10, 10, 10, 6)]
        [DataRow(0, 0, 0, 0, 10, 10, 10, 10)]
        [Description("Querying")]
        public void TestMargins(int left, int top, int right, int bottom, int width, int height, int expectedWidth, int expectedHeight)
        {
            var padding = new Padding(left, top, right, bottom);
            var margin = new Margin(padding, width, height);
            margin.Margins.Left.ShouldBe(left);
            margin.Margins.Top.ShouldBe(top);
            margin.Margins.Right.ShouldBe(right);
            margin.Margins.Bottom.ShouldBe(bottom);
            margin.Width.ShouldBe(expectedWidth);
            margin.Height.ShouldBe(expectedHeight);
        }

        /// <summary>
        /// Tests padding
        /// </summary>
        [TestMethod]
        [DataRow(2, 2)]
        [DataRow(2, 0)]
        [DataRow(0, 2)]
        [DataRow(0, 0)]
        [Description("Querying")]
        public void TestHorizontalPadding(int left, int right)
        {
            var padding = new HorizontalPad(left, right);
            padding.Left.ShouldBe(left);
            padding.Right.ShouldBe(right);
        }

        /// <summary>
        /// Tests margins
        /// </summary>
        [TestMethod]
        [DataRow(2, 2, 10, 6)]
        [DataRow(2, 0, 10, 8)]
        [DataRow(0, 2, 10, 8)]
        [DataRow(0, 0, 10, 10)]
        [Description("Querying")]
        public void TestHorizontalMargins(int left, int right, int width, int expectedWidth)
        {
            var padding = new HorizontalPad(left, right);
            var margin = new HorizontalMargin(padding, width);
            margin.Margins.Left.ShouldBe(left);
            margin.Margins.Right.ShouldBe(right);
            margin.Width.ShouldBe(expectedWidth);
        }

        /// <summary>
        /// Tests padding
        /// </summary>
        [TestMethod]
        [DataRow(2, 2)]
        [DataRow(2, 0)]
        [DataRow(0, 2)]
        [DataRow(0, 0)]
        [Description("Querying")]
        public void TestVerticalPadding(int top, int bottom)
        {
            var padding = new VerticalPad(top, bottom);
            padding.Top.ShouldBe(top);
            padding.Bottom.ShouldBe(bottom);
        }

        /// <summary>
        /// Tests margins
        /// </summary>
        [TestMethod]
        [DataRow(2, 2, 10, 6)]
        [DataRow(2, 0, 10, 8)]
        [DataRow(0, 2, 10, 8)]
        [DataRow(0, 0, 10, 10)]
        [Description("Querying")]
        public void TestVerticalMargins(int top, int bottom, int height, int expectedHeight)
        {
            var padding = new VerticalPad(top, bottom);
            var margin = new VerticalMargin(padding, height);
            margin.Margins.Top.ShouldBe(top);
            margin.Margins.Bottom.ShouldBe(bottom);
            margin.Height.ShouldBe(expectedHeight);
        }

    }
}
