/*
 * MIT License
 * 
 * Copyright (c) 2022 Aptivi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Newtonsoft.Json.Linq;
using Shouldly;
using System;

namespace TermRead.Colors.Tests
{
    [TestFixture]
    public partial class Color255QueryingTests
    {
        /// <summary>
        /// Tests querying 255-color data from JSON (parses only needed data by KS)
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestQueryColorDataFromJson()
        {
            for (int ColorIndex = 0; ColorIndex <= 255; ColorIndex++)
            {
                JObject ColorData = (JObject)Color255.ColorDataJson[ColorIndex];
                ColorData["colorId"].ToString().ShouldBe(Convert.ToString(ColorIndex));
                int.TryParse(ColorData["rgb"]["r"].ToString(), out int _).ShouldBeTrue();
                int.TryParse(ColorData["rgb"]["g"].ToString(), out int _).ShouldBeTrue();
                int.TryParse(ColorData["rgb"]["b"].ToString(), out int _).ShouldBeTrue();
            }
        }

        /// <summary>
        /// Tests getting an escape character
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetEsc()
        {
            Color255.GetEsc().ShouldBe(Convert.ToChar(0x1B));
        }
    }
}
