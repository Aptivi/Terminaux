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
using System;

namespace Terminaux.Colors.Tests
{
    [TestFixture]
    public partial class ColorInitializationEqualTests
    {
        [SetUp]
        public void ResetColorDeficiency()
        {
            ColorTools.EnableColorTransformation = false;
            ColorTools.EnableSimpleColorTransformation = false;
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom255ColorsWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color(18);
            var SecondInstance = new Color(18);

            // Check whether they equal
            (ColorInstance == SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom255ColorsWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color(18);
            var SecondInstance = new Color(17);

            // Check whether they equal
            (ColorInstance != SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors and comparing the equality using Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom255ColorsWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color(18);
            var SecondInstance = new Color(18);

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors and comparing the inequality using Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom255ColorsWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color(18);
            var SecondInstance = new Color(17);

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom16ColorsWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColor.Magenta);
            var SecondInstance = new Color(ConsoleColor.Magenta);

            // Check whether they equal
            (ColorInstance == SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom16ColorsWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColor.Magenta);
            var SecondInstance = new Color(ConsoleColor.Red);

            // Check whether they equal
            (ColorInstance != SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors and comparing the equality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom16ColorsWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColor.Magenta);
            var SecondInstance = new Color(ConsoleColor.Magenta);

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors and comparing the inequality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFrom16ColorsWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColor.Magenta);
            var SecondInstance = new Color(ConsoleColor.Red);

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color("94;0;63");
            var SecondInstance = new Color("94;0;63");

            // Check whether they equal
            (ColorInstance == SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color("94;0;63");
            var SecondInstance = new Color("94;0;62");

            // Check whether they equal
            (ColorInstance != SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the equality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color("94;0;63");
            var SecondInstance = new Color("94;0;63");

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the inequality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color("94;0;63");
            var SecondInstance = new Color("94;0;62");

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing color instance from true color numbers and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorNumbersWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color(94, 0, 63);
            var SecondInstance = new Color(94, 0, 63);

            // Check whether they equal
            (ColorInstance == SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color numbers and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorNumbersWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color(94, 0, 63);
            var SecondInstance = new Color(94, 0, 62);

            // Check whether they equal
            (ColorInstance != SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color numbers and comparing the equality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorNumbersWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color(94, 0, 63);
            var SecondInstance = new Color(94, 0, 63);

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color numbers and comparing the inequality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromTrueColorNumbersWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color(94, 0, 63);
            var SecondInstance = new Color(94, 0, 62);

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromHexWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color("#0F0F0F");
            var SecondInstance = new Color("#0F0F0F");

            // Check whether they equal
            (ColorInstance == SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromHexWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color("#0F0F0F");
            var SecondInstance = new Color("#0E0E0E");

            // Check whether they equal
            (ColorInstance != SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the equality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromHexWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color("#0F0F0F");
            var SecondInstance = new Color("#0F0F0F");

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from true color and comparing the inequality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromHexWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color("#0F0F0F");
            var SecondInstance = new Color("#0E0E0E");

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/> and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromNameWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color("Magenta");
            var SecondInstance = new Color("Magenta");

            // Check whether they equal
            (ColorInstance == SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/> and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromNameWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ColorInstance = new Color("Magenta");
            var SecondInstance = new Color("Red");

            // Check whether they equal
            (ColorInstance != SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/> and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromNameWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color("Magenta");
            var SecondInstance = new Color("Magenta");

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/> and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeColorInstanceFromNameWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ColorInstance = new Color("Magenta");
            var SecondInstance = new Color("Red");

            // Check whether they equal
            ColorInstance.Equals(SecondInstance).ShouldBeFalse();
        }
    }
}
