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
using System;
using DrawingColor = System.Drawing.Color;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Transformation.Contrast;
using Colorimetry.Transformation.Formulas;
using Colorimetry.Transformation.Tools.ColorBlind;
using Terminaux.Sequences.Builder;

namespace Terminaux.Tests.Colors
{
    [TestClass]
    public partial class ColorInitializationVtSeqTests
    {
        /// <summary>
        /// Tests initializing color instance from 255 colors
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255Colors()
        {
            // Create instance
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("18");
            ColorInstance.Type.ShouldBe(ColorType.EightBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;5;18m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;5;18m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#000087");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors using an implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsImplicit()
        {
            // Create instance
            Color ColorInstance = 18;

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("18");
            ColorInstance.Type.ShouldBe(ColorType.EightBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;5;18m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;5;18m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#000087");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopia()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Frequency = 1.0, Deficiency = ColorBlindDeficiency.Protan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;24;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;24;135m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;24;135m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(24);
            ColorInstance.RGB.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#001887");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomaly()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Frequency = 0.6, Deficiency = ColorBlindDeficiency.Protan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;17;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;17;135m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;17;135m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(17);
            ColorInstance.RGB.B.ShouldBe(135);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#001187");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopia()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Frequency = 1.0, Deficiency = ColorBlindDeficiency.Deutan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;41;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;41;134m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;41;134m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(41);
            ColorInstance.RGB.B.ShouldBe(134);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#002986");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomaly()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Frequency = 0.6, Deficiency = ColorBlindDeficiency.Deutan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;31;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;31;134m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;31;134m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(31);
            ColorInstance.RGB.B.ShouldBe(134);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#001F86");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopia()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Frequency = 1.0, Deficiency = ColorBlindDeficiency.Tritan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;48;69");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;48;69m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;48;69m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(48);
            ColorInstance.RGB.B.ShouldBe(69);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#003045");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey15);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomaly()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Frequency = 0.6, Deficiency = ColorBlindDeficiency.Tritan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;36;102");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;36;102m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;36;102m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(36);
            ColorInstance.RGB.B.ShouldBe(102);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#002466");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.NavyBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia, Vienot)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopiaSimple()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Simple = true, Frequency = 1.0, Deficiency = ColorBlindDeficiency.Protan }],
            };
            var ColorInstance = new Color(10, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("241;241;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;241;241;0m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;241;241;0m");
            ColorInstance.RGB.R.ShouldBe(241);
            ColorInstance.RGB.G.ShouldBe(241);
            ColorInstance.RGB.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#F1F100");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Yellow);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Yellow);
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly, Vienot)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomalySimple()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Simple = true, Frequency = 0.6, Deficiency = ColorBlindDeficiency.Protan }],
            };
            var ColorInstance = new Color(10, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("192;247;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;192;247;0m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;192;247;0m");
            ColorInstance.RGB.R.ShouldBe(192);
            ColorInstance.RGB.G.ShouldBe(247);
            ColorInstance.RGB.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#C0F700");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.GreenYellow);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia, Vienot)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopiaSimple()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Simple = true, Frequency = 1.0, Deficiency = ColorBlindDeficiency.Deutan }],
            };
            var ColorInstance = new Color(10, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("218;218;41");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;218;218;41m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;218;218;41m");
            ColorInstance.RGB.R.ShouldBe(218);
            ColorInstance.RGB.G.ShouldBe(218);
            ColorInstance.RGB.B.ShouldBe(41);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#DADA29");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Yellow3Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly, Vienot)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomalySimple()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Simple = true, Frequency = 0.6, Deficiency = ColorBlindDeficiency.Deutan }],
            };
            var ColorInstance = new Color(10, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("174;234;30");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;174;234;30m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;174;234;30m");
            ColorInstance.RGB.R.ShouldBe(174);
            ColorInstance.RGB.G.ShouldBe(234);
            ColorInstance.RGB.B.ShouldBe(30);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#AEEA1E");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Yellow3);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia, Vienot)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopiaSimple()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Simple = true, Frequency = 1.0, Deficiency = ColorBlindDeficiency.Tritan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;51;51");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;51;51m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;51;51m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(51);
            ColorInstance.RGB.B.ShouldBe(51);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#003333");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey15);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly, Vienot)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomalySimple()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new ColorBlind() { Simple = true, Frequency = 0.6, Deficiency = ColorBlindDeficiency.Tritan }],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;39;96");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;0;39;96m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;0;39;96m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(39);
            ColorInstance.RGB.B.ShouldBe(96);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#002760");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.NavyBlue);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Monochromacy)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsMonochromacy()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new Monochromacy()],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("35;35;35");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;35;35;35m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;35;35;35m");
            ColorInstance.RGB.R.ShouldBe(35);
            ColorInstance.RGB.G.ShouldBe(35);
            ColorInstance.RGB.B.ShouldBe(35);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#232323");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey15);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Sepia)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsSepia()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new Sepia()],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("25;22;17");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;25;22;17m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;25;22;17m");
            ColorInstance.RGB.R.ShouldBe(25);
            ColorInstance.RGB.G.ShouldBe(22);
            ColorInstance.RGB.B.ShouldBe(17);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#191611");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey7);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Cyanotype)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsCyanotype()
        {
            // Create instance
            var settings = new ColorSettings()
            {
                Transformations = [new Cyanotype()],
            };
            var ColorInstance = new Color(18, settings);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("22;45;83");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;22;45;83m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;22;45;83m");
            ColorInstance.RGB.R.ShouldBe(22);
            ColorInstance.RGB.G.ShouldBe(45);
            ColorInstance.RGB.B.ShouldBe(83);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#162D53");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey19);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom16Colors()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Fuchsia);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[105m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[95m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Fuchsia);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from true color
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColor()
        {
            // Create instance
            var ColorInstance = new Color("94;0;63");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;94;0;63m");
            ColorInstance.RGB.R.ShouldBe(94);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DeepPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorImplicit()
        {
            // Create instance
            Color ColorInstance = "94;0;63";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;94;0;63m");
            ColorInstance.RGB.R.ShouldBe(94);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DeepPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color numbers
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorNumbers()
        {
            // Create instance
            var ColorInstance = new Color(94, 0, 63);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;94;0;63m");
            ColorInstance.RGB.R.ShouldBe(94);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DeepPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color order code
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorOrderCode()
        {
            // Create instance
            var ColorInstance = new Color(4128862);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;94;0;63m");
            ColorInstance.RGB.R.ShouldBe(94);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DeepPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromHex()
        {
            // Create instance
            var ColorInstance = new Color("#0F0F0F");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("15;15;15");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;15;15;15m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;15;15;15m");
            ColorInstance.RGB.R.ShouldBe(15);
            ColorInstance.RGB.G.ShouldBe(15);
            ColorInstance.RGB.B.ShouldBe(15);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#0F0F0F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey7);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromHexImplicit()
        {
            // Create instance
            Color ColorInstance = "#0F0F0F";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("15;15;15");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;15;15;15m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;15;15;15m");
            ColorInstance.RGB.R.ShouldBe(15);
            ColorInstance.RGB.G.ShouldBe(15);
            ColorInstance.RGB.B.ShouldBe(15);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#0F0F0F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey7);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromIncompleteHex()
        {
            // Create instance
            var ColorInstance = new Color("#FFF");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("15");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[107m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[97m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(255);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FFFFFF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.White);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.White);
        }

        /// <summary>
        /// Tests initializing color instance from true color using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromIncompleteHexImplicit()
        {
            // Create instance
            Color ColorInstance = "#FFF";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("15");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[107m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[97m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(255);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FFFFFF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.White);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.White);
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/>
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromName()
        {
            // Create instance
            var ColorInstance = new Color("Fuchsia");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[105m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[95m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Fuchsia);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/> using an implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromNameImplicit()
        {
            // Create instance
            Color ColorInstance = "Fuchsia";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[105m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[95m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Fuchsia);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Fuchsia);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[105m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[95m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Fuchsia);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum using an implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnumImplicit()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Fuchsia);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[105m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[95m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Fuchsia);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum (16 colors)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum16()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColor.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[105m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[95m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Fuchsia);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum (16 colors) using an implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum16Implicit()
        {
            // Create instance
            Color ColorInstance = ConsoleColor.Magenta;

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[105m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[95m");
            ColorInstance.RGB.R.ShouldBe(255);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(255);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Fuchsia);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests getting empty color
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGetEmptyColor()
        {
            // Create instance
            var ColorInstance = Color.Empty;

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0");
            ColorInstance.Type.ShouldBe(ColorType.FourBitColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[40m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[30m");
            ColorInstance.RGB.R.ShouldBe(0);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(0);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Hex.ShouldBe("#000000");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Black);
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Black);
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMYK)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmyk()
        {
            // Create instance
            var ColorInstance = new Color("cmyk:0;100;84;45");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;140;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;140;0;22m");
            ColorInstance.RGB.R.ShouldBe(140);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8C0016");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMYK) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmykImplicit()
        {
            // Create instance
            Color ColorInstance = "cmyk:0;100;84;45";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;140;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;140;0;22m");
            ColorInstance.RGB.R.ShouldBe(140);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8C0016");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMY)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmy()
        {
            // Create instance
            var ColorInstance = new Color("cmy:45;100;91");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;23");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;140;0;23m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;140;0;23m");
            ColorInstance.RGB.R.ShouldBe(140);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(23);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8C0017");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CMY) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCmyImplicit()
        {
            // Create instance
            Color ColorInstance = "cmy:45;100;91";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;23");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;140;0;23m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;140;0;23m");
            ColorInstance.RGB.R.ShouldBe(140);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(23);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8C0017");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSL)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHsl()
        {
            // Create instance
            var ColorInstance = new Color("hsl:351;100;27");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;0;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSL) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHslImplicit()
        {
            // Create instance
            Color ColorInstance = "hsl:351;100;27";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;0;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSV)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHsv()
        {
            // Create instance
            var ColorInstance = new Color("hsv:351;100;54");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;0;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HSV) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHsvImplicit()
        {
            // Create instance
            Color ColorInstance = "hsv:351;100;54";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;0;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;0;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;0;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A0015");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (RYB)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorRyb()
        {
            // Create instance
            var ColorInstance = new Color("ryb:139;137;22");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;79;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;79;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;79;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8B4F16");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (RYB) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorRybImplicit()
        {
            // Create instance
            Color ColorInstance = "ryb:139;137;22";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;79;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;79;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;79;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8B4F16");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (YIQ)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorYiq()
        {
            // Create instance
            var ColorInstance = new Color("yiq:94;171;132");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;80;53");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;80;53m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;80;53m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(53);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A5035");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (YIQ) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorYiqImplicit()
        {
            // Create instance
            Color ColorInstance = "yiq:94;171;132";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;80;53");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;80;53m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;80;53m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(53);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A5035");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (YUV)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorYuv()
        {
            // Create instance
            var ColorInstance = new Color("yuv:91;89;162");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;54");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;54m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;54m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(54);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8B5036");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (YUV) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorYuvImplicit()
        {
            // Create instance
            Color ColorInstance = "yuv:91;89;162";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;54");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;54m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;54m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(54);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8B5036");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (XYZ)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorXyz()
        {
            // Create instance
            var ColorInstance = new Color("xyz:13.6;11.2;2.2");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;79;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;79;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;79;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A4F15");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (XYZ) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorXyzImplicit()
        {
            // Create instance
            Color ColorInstance = "xyz:13.6;11.2;2.2";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;79;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;79;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;79;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A4F15");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (YXY)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorYxy()
        {
            // Create instance
            var ColorInstance = new Color("yxy:11.2;0.5;0.4");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("141;77;32");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;141;77;32m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;141;77;32m");
            ColorInstance.RGB.R.ShouldBe(141);
            ColorInstance.RGB.G.ShouldBe(77);
            ColorInstance.RGB.B.ShouldBe(32);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8D4D20");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (YXY) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorYxyImplicit()
        {
            // Create instance
            Color ColorInstance = "yxy:11.2;0.5;0.4";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("141;77;32");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;141;77;32m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;141;77;32m");
            ColorInstance.RGB.R.ShouldBe(141);
            ColorInstance.RGB.G.ShouldBe(77);
            ColorInstance.RGB.B.ShouldBe(32);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8D4D20");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HunterLab)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHunterLab()
        {
            // Create instance
            var ColorInstance = new Color("hunterlab:33.59;13.8;19.6");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;80;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;80;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;80;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A5015");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HunterLab) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHunterLabImplicit()
        {
            // Create instance
            Color ColorInstance = "hunterlab:33.59;13.8;19.6";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;80;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;80;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;80;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A5015");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELAB)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLab()
        {
            // Create instance
            var ColorInstance = new Color("cielab:40;20.2;42");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;79;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;79;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;79;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A4F15");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELAB) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLabImplicit()
        {
            // Create instance
            Color ColorInstance = "cielab:40;20.2;42";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;79;21");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;79;21m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;79;21m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(21);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A4F15");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELAB)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLabE()
        {
            // Create instance
            var ColorInstance = new Color("cielab:40;20.2;42;10;7");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("144;76;18");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;144;76;18m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;144;76;18m");
            ColorInstance.RGB.R.ShouldBe(144);
            ColorInstance.RGB.G.ShouldBe(76);
            ColorInstance.RGB.B.ShouldBe(18);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#904C12");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELAB) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLabImplicitE()
        {
            // Create instance
            Color ColorInstance = "cielab:40;20.2;42;10;7";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("144;76;18");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;144;76;18m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;144;76;18m");
            ColorInstance.RGB.R.ShouldBe(144);
            ColorInstance.RGB.G.ShouldBe(76);
            ColorInstance.RGB.B.ShouldBe(18);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#904C12");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELUV)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLuv()
        {
            // Create instance
            var ColorInstance = new Color("cieluv:40;47;35");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("133;80;66");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;133;80;66m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;133;80;66m");
            ColorInstance.RGB.R.ShouldBe(133);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(66);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#855042");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELUV) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLuvImplicit()
        {
            // Create instance
            Color ColorInstance = "cieluv:40;47;35";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("133;80;66");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;133;80;66m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;133;80;66m");
            ColorInstance.RGB.R.ShouldBe(133);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(66);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#855042");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELUV)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLuvE()
        {
            // Create instance
            var ColorInstance = new Color("cieluv:40;47;35;10;7");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("137;78;62");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;137;78;62m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;137;78;62m");
            ColorInstance.RGB.R.ShouldBe(137);
            ColorInstance.RGB.G.ShouldBe(78);
            ColorInstance.RGB.B.ShouldBe(62);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#894E3E");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELUV) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLuvImplicitE()
        {
            // Create instance
            Color ColorInstance = "cieluv:40;47;35;10;7";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("137;78;62");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;137;78;62m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;137;78;62m");
            ColorInstance.RGB.R.ShouldBe(137);
            ColorInstance.RGB.G.ShouldBe(78);
            ColorInstance.RGB.B.ShouldBe(62);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#894E3E");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.LightPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELCH)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLch()
        {
            // Create instance
            var ColorInstance = new Color("cielch:40;46.6;64.2");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;79;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;79;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;79;22m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A4F16");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELCH) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLchImplicit()
        {
            // Create instance
            Color ColorInstance = "cielch:40;46.6;64.2";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("138;79;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;138;79;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;138;79;22m");
            ColorInstance.RGB.R.ShouldBe(138);
            ColorInstance.RGB.G.ShouldBe(79);
            ColorInstance.RGB.B.ShouldBe(22);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8A4F16");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELCH)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLchE()
        {
            // Create instance
            var ColorInstance = new Color("cielch:40;46.6;64.2;10;7");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("144;76;19");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;144;76;19m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;144;76;19m");
            ColorInstance.RGB.R.ShouldBe(144);
            ColorInstance.RGB.G.ShouldBe(76);
            ColorInstance.RGB.B.ShouldBe(19);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#904C13");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (CIELCH) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorCieLchImplicitE()
        {
            // Create instance
            Color ColorInstance = "cielch:40;46.6;64.2;10;7";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("144;76;19");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;144;76;19m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;144;76;19m");
            ColorInstance.RGB.R.ShouldBe(144);
            ColorInstance.RGB.G.ShouldBe(76);
            ColorInstance.RGB.B.ShouldBe(19);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#904C13");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Orange4Alt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HWB)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHwb()
        {
            // Create instance
            var ColorInstance = new Color("hwb:350;0;45");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;23");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;140;0;23m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;140;0;23m");
            ColorInstance.RGB.R.ShouldBe(140);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(23);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8C0017");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color (HWB) using the implicit operator
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorHwbImplicit()
        {
            // Create instance
            Color ColorInstance = "hwb:350;0;45";

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("140;0;23");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;140;0;23m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;140;0;23m");
            ColorInstance.RGB.R.ShouldBe(140);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(23);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#8C0017");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkRedAlt);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from Drawing's color
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromDrawingColor()
        {
            // Create instance
            var sourceDrawingBrushColor = DrawingColor.FromArgb(94, 0, 63);
            var ColorInstance = new Color(sourceDrawingBrushColor);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;94;0;63m");
            ColorInstance.RGB.R.ShouldBe(94);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(63);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DeepPink4);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from Drawing's color (transparent)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromDrawingColorTransparent()
        {
            // Create instance
            var sourceDrawingBrushColor = DrawingColor.FromArgb(128, 94, 0, 63);
            var ColorInstance = new Color(sourceDrawingBrushColor);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("47;0;31");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;47;0;31m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;47;0;31m");
            ColorInstance.RGB.R.ShouldBe(47);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(31);
            ColorInstance.RGB.OriginalRgb.R.ShouldBe(94);
            ColorInstance.RGB.OriginalRgb.G.ShouldBe(0);
            ColorInstance.RGB.OriginalRgb.B.ShouldBe(63);
            ColorInstance.RGB.A.ShouldBe(128);
            ColorInstance.Brightness.ShouldBe(ColorBrightness.Dark);
            ColorInstance.Brightness.ShouldNotBe(ColorBrightness.Light);
            ColorInstance.Hex.ShouldBe("#2F001F");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.Grey11);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));

            // Try to get the original of the original, and it'll fail.
            Should.Throw(() => Console.WriteLine(ColorInstance.RGB.OriginalRgb.OriginalRgb.ToString()), typeof(ColorException));
        }
    }
}
