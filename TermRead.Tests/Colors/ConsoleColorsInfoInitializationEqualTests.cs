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
    public partial class ConsoleColorsInfoInitializationEqualTests
    {
        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);

            // Check for equality
            (ConsoleColorsInfoInstance == SecondConsoleColorsInfoInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey15);

            // Check for equality
            (ConsoleColorsInfoInstance != SecondConsoleColorsInfoInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the equality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);

            // Check for equality
            ConsoleColorsInfoInstance.Equals(SecondConsoleColorsInfoInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the inequality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey15);

            // Check for equality
            ConsoleColorsInfoInstance.Equals(SecondConsoleColorsInfoInstance).ShouldBeFalse();
        }
    }
}
