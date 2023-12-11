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

using Shouldly;
using System;
using Terminaux.Colors;
using Terminaux.Colors.Accessibility;

namespace Terminaux.Tests.Colors
{
    [TestFixture]
    public partial class ColorInitializationTests
    {
        [SetUp]
        public void ResetColorDeficiency()
        {
            ColorTools.EnableColorTransformation = false;
            ColorTools.ColorTransformationMethod = TransformationMethod.Brettel1997;
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255Colors()
        {
            // Create instance
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("18");
            ColorInstance.Type.ShouldBe(ColorType._255Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;18m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;18m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#000087");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue_000087);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors using an implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsImplicit()
        {
            // Create instance
            Color ColorInstance = 18;

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("18");
            ColorInstance.Type.ShouldBe(ColorType._255Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;18m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;18m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#000087");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue_000087);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopia()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;24;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;24;135m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;24;135m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(24);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#001887");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomaly()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;17;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;17;135m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;17;135m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(17);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#001187");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopia()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;41;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;41;134m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;41;134m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(41);
            ColorInstance.B.ShouldBe(134);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#002986");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomaly()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;31;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;31;134m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;31;134m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(31);
            ColorInstance.B.ShouldBe(134);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#001F86");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopia()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;48;69");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;48;69m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;48;69m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(48);
            ColorInstance.B.ShouldBe(69);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#003045");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomaly()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;36;102");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;36;102m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;36;102m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(36);
            ColorInstance.B.ShouldBe(102);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#002466");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopiaSimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorTransformationMethod = TransformationMethod.Vienot1999;
            ColorTools.ColorDeficiency = Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("241;241;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;241;241;0m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;241;241;0m");
            ColorInstance.R.ShouldBe(241);
            ColorInstance.G.ShouldBe(241);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#F1F100");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomalySimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorTransformationMethod = TransformationMethod.Vienot1999;
            ColorTools.ColorDeficiency = Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("192;247;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;192;247;0m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;192;247;0m");
            ColorInstance.R.ShouldBe(192);
            ColorInstance.G.ShouldBe(247);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#C0F700");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopiaSimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorTransformationMethod = TransformationMethod.Vienot1999;
            ColorTools.ColorDeficiency = Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("218;218;41");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;218;218;41m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;218;218;41m");
            ColorInstance.R.ShouldBe(218);
            ColorInstance.G.ShouldBe(218);
            ColorInstance.B.ShouldBe(41);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#DADA29");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomalySimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorTransformationMethod = TransformationMethod.Vienot1999;
            ColorTools.ColorDeficiency = Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("174;234;30");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;174;234;30m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;174;234;30m");
            ColorInstance.R.ShouldBe(174);
            ColorInstance.G.ShouldBe(234);
            ColorInstance.B.ShouldBe(30);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#AEEA1E");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopiaSimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorTransformationMethod = TransformationMethod.Vienot1999;
            ColorTools.ColorDeficiency = Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;51;51");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;51;51m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;51;51m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(51);
            ColorInstance.B.ShouldBe(51);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#003333");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomalySimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorTransformationMethod = TransformationMethod.Vienot1999;
            ColorTools.ColorDeficiency = Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;39;96");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;39;96m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;39;96m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(39);
            ColorInstance.B.ShouldBe(96);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#002760");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Monochromacy)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsMonochromacy()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Deficiency.Monochromacy;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("35;35;35");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;35;35;35m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;35;35;35m");
            ColorInstance.R.ShouldBe(35);
            ColorInstance.G.ShouldBe(35);
            ColorInstance.B.ShouldBe(35);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#232323");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopiaRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Protan, 1.0);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;24;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;24;135m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;24;135m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(24);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#001887");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomalyRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Protan, 0.6);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;17;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;17;135m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;17;135m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(17);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#001187");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopiaRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Deutan, 1.0);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;41;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;41;134m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;41;134m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(41);
            ColorInstance.B.ShouldBe(134);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#002986");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomalyRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Deutan, 0.6);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;31;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;31;134m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;31;134m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(31);
            ColorInstance.B.ShouldBe(134);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#001F86");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopiaRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Tritan, 1.0);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;48;69");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;48;69m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;48;69m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(48);
            ColorInstance.B.ShouldBe(69);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#003045");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomalyRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Tritan, 0.6);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;36;102");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;36;102m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;36;102m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(36);
            ColorInstance.B.ShouldBe(102);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#002466");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopiaSimpleRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(10), Deficiency.Protan, 1.0, TransformationMethod.Vienot1999);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("241;241;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;241;241;0m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;241;241;0m");
            ColorInstance.R.ShouldBe(241);
            ColorInstance.G.ShouldBe(241);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#F1F100");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomalySimpleRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(10), Deficiency.Protan, 0.6, TransformationMethod.Vienot1999);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("192;247;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;192;247;0m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;192;247;0m");
            ColorInstance.R.ShouldBe(192);
            ColorInstance.G.ShouldBe(247);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#C0F700");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopiaSimpleRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(10), Deficiency.Deutan, 1.0, TransformationMethod.Vienot1999);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("218;218;41");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;218;218;41m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;218;218;41m");
            ColorInstance.R.ShouldBe(218);
            ColorInstance.G.ShouldBe(218);
            ColorInstance.B.ShouldBe(41);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#DADA29");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomalySimpleRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(10), Deficiency.Deutan, 0.6, TransformationMethod.Vienot1999);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("174;234;30");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;174;234;30m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;174;234;30m");
            ColorInstance.R.ShouldBe(174);
            ColorInstance.G.ShouldBe(234);
            ColorInstance.B.ShouldBe(30);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#AEEA1E");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopiaSimpleRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Tritan, 1.0, TransformationMethod.Vienot1999);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;51;51");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;51;51m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;51;51m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(51);
            ColorInstance.B.ShouldBe(51);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#003333");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomalySimpleRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Tritan, 0.6, TransformationMethod.Vienot1999);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;39;96");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;0;39;96m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;0;39;96m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(39);
            ColorInstance.B.ShouldBe(96);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#002760");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Monochromacy)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsMonochromacyRenderColorBlindnessAware()
        {
            // Create instance
            var ColorInstance = ColorTools.RenderColorBlindnessAware(new Color(18), Deficiency.Monochromacy, 0.6);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("35;35;35");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;35;35;35m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;35;35;35m");
            ColorInstance.R.ShouldBe(35);
            ColorInstance.G.ShouldBe(35);
            ColorInstance.B.ShouldBe(35);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#232323");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom16Colors()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from true color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColor()
        {
            // Create instance
            var ColorInstance = new Color("94;0;63");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;94;0;63m");
            ColorInstance.R.ShouldBe(94);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color using the implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorImplicit()
        {
            // Create instance
            Color ColorInstance = "94;0;63";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;94;0;63m");
            ColorInstance.R.ShouldBe(94);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color numbers
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorNumbers()
        {
            // Create instance
            var ColorInstance = new Color(94, 0, 63);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;94;0;63m");
            ColorInstance.R.ShouldBe(94);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromHex()
        {
            // Create instance
            var ColorInstance = new Color("#0F0F0F");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("15;15;15");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;15;15;15m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;15;15;15m");
            ColorInstance.R.ShouldBe(15);
            ColorInstance.G.ShouldBe(15);
            ColorInstance.B.ShouldBe(15);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#0F0F0F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color using the implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromHexImplicit()
        {
            // Create instance
            Color ColorInstance = "#0F0F0F";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("15;15;15");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;15;15;15m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;15;15;15m");
            ColorInstance.R.ShouldBe(15);
            ColorInstance.G.ShouldBe(15);
            ColorInstance.B.ShouldBe(15);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#0F0F0F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/>
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromName()
        {
            // Create instance
            var ColorInstance = new Color("Magenta");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/> using an implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromNameImplicit()
        {
            // Create instance
            Color ColorInstance = "Magenta";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum using an implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnumImplicit()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum (16 colors)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum16()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColor.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum (16 colors) using an implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum16Implicit()
        {
            // Create instance
            Color ColorInstance = ConsoleColor.Magenta;

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests getting empty color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestGetEmptyColor()
        {
            // Create instance
            var ColorInstance = Color.Empty;

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;5;0m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;5;0m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#000000");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Black);
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMYK)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmyk()
        {
            // Create instance
            var ColorInstance = new Color("cmyk:0;100;84;45");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;140;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;140;0;22m");
            ColorInstance.R.ShouldBe(140);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8C0016");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMYK) using the implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmykImplicit()
        {
            // Create instance
            Color ColorInstance = "cmyk:0;100;84;45";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;140;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;140;0;22m");
            ColorInstance.R.ShouldBe(140);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8C0016");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMY)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmy()
        {
            // Create instance
            var ColorInstance = new Color("cmy:45;100;91");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;23");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;140;0;23m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;140;0;23m");
            ColorInstance.R.ShouldBe(140);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(23);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8C0017");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMY) using the implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmyImplicit()
        {
            // Create instance
            Color ColorInstance = "cmy:45;100;91";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;23");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;140;0;23m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;140;0;23m");
            ColorInstance.R.ShouldBe(140);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(23);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8C0017");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSL)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHsl()
        {
            // Create instance
            var ColorInstance = new Color("hsl:351;100;27");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;138;0;21m");
            ColorInstance.R.ShouldBe(138);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSL) using the implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHslImplicit()
        {
            // Create instance
            Color ColorInstance = "hsl:351;100;27";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;138;0;21m");
            ColorInstance.R.ShouldBe(138);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSV)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHsv()
        {
            // Create instance
            var ColorInstance = new Color("hsv:351;100;54");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;138;0;21m");
            ColorInstance.R.ShouldBe(138);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSV) using the implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHsvImplicit()
        {
            // Create instance
            Color ColorInstance = "hsv:351;100;54";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;138;0;21m");
            ColorInstance.R.ShouldBe(138);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (RYB)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorRyb()
        {
            // Create instance
            var ColorInstance = new Color("ryb:139;137;22");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;79;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;79;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;79;22m");
            ColorInstance.R.ShouldBe(139);
            ColorInstance.G.ShouldBe(79);
            ColorInstance.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8B4F16");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (RYB) using the implicit operator
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorRybImplicit()
        {
            // Create instance
            Color ColorInstance = "ryb:139;137;22";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;79;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;79;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;79;22m");
            ColorInstance.R.ShouldBe(139);
            ColorInstance.G.ShouldBe(79);
            ColorInstance.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#8B4F16");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }
    }
}
