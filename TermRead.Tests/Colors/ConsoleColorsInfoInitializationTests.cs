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

using Shouldly;

namespace TermRead.Colors.Tests
{
    [TestFixture]
    public partial class ConsoleColorsInfoInitializationTests
    {
        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo from a bright color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeConsoleColorsInfoInstanceBright()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);

            // Check for property correctness
            ConsoleColorsInfoInstance.ColorID.ShouldBe(253);
            ConsoleColorsInfoInstance.R.ShouldBe(218);
            ConsoleColorsInfoInstance.G.ShouldBe(218);
            ConsoleColorsInfoInstance.B.ShouldBe(218);
            ConsoleColorsInfoInstance.IsBright.ShouldBeTrue();
            ConsoleColorsInfoInstance.IsDark.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo from a dark color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeConsoleColorsInfoInstanceDark()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey11);

            // Check for property correctness
            ConsoleColorsInfoInstance.ColorID.ShouldBe(234);
            ConsoleColorsInfoInstance.R.ShouldBe(28);
            ConsoleColorsInfoInstance.G.ShouldBe(28);
            ConsoleColorsInfoInstance.B.ShouldBe(28);
            ConsoleColorsInfoInstance.IsBright.ShouldBeFalse();
            ConsoleColorsInfoInstance.IsDark.ShouldBeTrue();
        }
    }
}
