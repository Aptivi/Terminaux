
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Shouldly;
using System;
using Terminaux.Colors;

namespace Terminaux.Tests.Colors
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
