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
using Colorimetry;
using Colorimetry.Models;
using Colorimetry.Models.Conversion;
using Colorimetry.Models.Parsing;
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder;

namespace Terminaux.Tests.Colors
{
    [TestClass]
    public partial class ColorConversionVtSeqTests
    {
        /// <summary>
        /// Tests converting an RGB color to CMYK
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCmyk()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CMYK
            var cmyk = ConversionTools.ToCmyk(ColorInstance.RGB);

            // Check for property correctness
            cmyk.KWhole.ShouldBe(45);
            cmyk.CMY.CWhole.ShouldBe(0);
            cmyk.CMY.MWhole.ShouldBe(100);
            cmyk.CMY.YWhole.ShouldBe(84);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cmyk);

            // Check for property correctness
            rgb.R.ShouldBe(140);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to HSL
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToHsl()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HSL
            var hsl = ConversionTools.ToHsl(ColorInstance.RGB);

            // Check for property correctness
            hsl.HueWhole.ShouldBe(350);
            hsl.ReverseHueWhole.ShouldBe(170);
            hsl.SaturationWhole.ShouldBe(100);
            hsl.LightnessWhole.ShouldBe(27);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(hsl);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CMY
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCmy()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CMY
            var cmy = ConversionTools.ToCmy(ColorInstance.RGB);

            // Check for property correctness
            cmy.CWhole.ShouldBe(45);
            cmy.MWhole.ShouldBe(100);
            cmy.YWhole.ShouldBe(91);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cmy);

            // Check for property correctness
            rgb.R.ShouldBe(140);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(23);
        }

        /// <summary>
        /// Tests converting an RGB color to HSV
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToHsv()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HSV
            var hsv = ConversionTools.ToHsv(ColorInstance.RGB);

            // Check for property correctness
            hsv.HueWhole.ShouldBe(350);
            hsv.ReverseHueWhole.ShouldBe(170);
            hsv.SaturationWhole.ShouldBe(100);
            hsv.ValueWhole.ShouldBe(54);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(hsv);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to RYB
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToRyb()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to RYB
            var ryb = ConversionTools.ToRyb(ColorInstance.RGB);

            // Check for property correctness
            ryb.R.ShouldBe(139);
            ryb.Y.ShouldBe(137);
            ryb.B.ShouldBe(22);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(ryb);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(79);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YIQ
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToYiq()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YIQ
            var yiq = ConversionTools.ToYiq(ColorInstance.RGB);

            // Check for property correctness
            yiq.Luma.ShouldBe(91);
            yiq.InPhase.ShouldBe(181);
            yiq.Quadrature.ShouldBe(122);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(yiq);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YUV
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToYuv()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YUV
            var yuv = ConversionTools.ToYuv(ColorInstance.RGB);

            // Check for property correctness
            yuv.Luma.ShouldBe(91);
            yuv.ChromaU.ShouldBe(89);
            yuv.ChromaV.ShouldBe(162);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(yuv);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(54);
        }

        /// <summary>
        /// Tests converting an RGB color to XYZ
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToXyz()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to XYZ
            var xyz = ConversionTools.ToXyz(ColorInstance.RGB);

            // Check for property correctness
            xyz.X.ShouldBe(13.660940262318197);
            xyz.Y.ShouldBe(11.284216455358383);
            xyz.Z.ShouldBe(2.2171176575479863);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(xyz);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YXY
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToYxy()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YXY
            var yxy = ConversionTools.ToYxy(ColorInstance.RGB);

            // Check for property correctness
            yxy.Y2.ShouldBe(11.284216455358383);
            yxy.X.ShouldBe(0.50293801150829631);
            yxy.Y1.ShouldBe(0.41543709850935812);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(yxy);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to HunterLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToHunterLab()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HunterLab
            var hunterLab = ConversionTools.ToHunterLab(ColorInstance.RGB);

            // Check for property correctness
            hunterLab.L.ShouldBe(33.591987817570995);
            hunterLab.A.ShouldBe(13.805076366856513);
            hunterLab.B.ShouldBe(19.601169467400634);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(hunterLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLab()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLab
            var cieLab = ConversionTools.ToCieLab(ColorInstance.RGB);

            // Check for property correctness
            cieLab.L.ShouldBe(40.055099179556059);
            cieLab.A.ShouldBe(20.292379028766018);
            cieLab.B.ShouldBe(42.032442228242864);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLabFull()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLab
            var cieLab = ConversionTools.ToCieLab(ColorInstance.RGB);

            // Check for property correctness
            cieLab.L.ShouldBe(40.055099179556059);
            cieLab.A.ShouldBe(20.292379028766018);
            cieLab.B.ShouldBe(42.032442228242864);
            cieLab.Observer.ShouldBe(2);
            cieLab.Illuminant.ShouldBe(IlluminantType.D65);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLabEqual()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLab
            var cieLab = ConversionTools.ToCieLab(ColorInstance.RGB, 10, IlluminantType.E);

            // Check for property correctness
            cieLab.L.ShouldBe(40.055099179556059);
            cieLab.A.ShouldBe(15.894835373068517);
            cieLab.B.ShouldBe(40.460964943396348);
            cieLab.Observer.ShouldBe(10);
            cieLab.Illuminant.ShouldBe(IlluminantType.E);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLuv
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLuv()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLuv
            var cieLuv = ConversionTools.ToCieLuv(ColorInstance.RGB);

            // Check for property correctness
            cieLuv.L.ShouldBe(40.055099179556059);
            cieLuv.U.ShouldBe(47.074237421611734);
            cieLuv.V.ShouldBe(35.083777826558624);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLuv);

            // Check for property correctness
            rgb.R.ShouldBe(133);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(66);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLuv
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLuvFull()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLuv
            var cieLuv = ConversionTools.ToCieLuv(ColorInstance.RGB);

            // Check for property correctness
            cieLuv.L.ShouldBe(40.055099179556059);
            cieLuv.U.ShouldBe(47.074237421611734);
            cieLuv.V.ShouldBe(35.083777826558624);
            cieLuv.Observer.ShouldBe(2);
            cieLuv.Illuminant.ShouldBe(IlluminantType.D65);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLuv);

            // Check for property correctness
            rgb.R.ShouldBe(133);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(66);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLuv with equal energy (observer is 10 degs)
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLuvEqual()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLuv
            var cieLuv = ConversionTools.ToCieLuv(ColorInstance.RGB, 10, IlluminantType.E);

            // Check for property correctness
            cieLuv.L.ShouldBe(40.055099179556059);
            cieLuv.U.ShouldBe(40.468174920048256);
            cieLuv.V.ShouldBe(32.299035228557536);
            cieLuv.Observer.ShouldBe(10);
            cieLuv.Illuminant.ShouldBe(IlluminantType.E);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLuv);

            // Check for property correctness
            rgb.R.ShouldBe(133);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(66);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLch
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLch()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLch
            var cieLch = ConversionTools.ToCieLch(ColorInstance.RGB);

            // Check for property correctness
            cieLch.L.ShouldBe(40.055099179556059);
            cieLch.C.ShouldBe(46.674477461645743);
            cieLch.H.ShouldBe(64.229726973355483);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLch);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLch
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLchFull()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLch
            var cieLch = ConversionTools.ToCieLch(ColorInstance.RGB);

            // Check for property correctness
            cieLch.L.ShouldBe(40.055099179556059);
            cieLch.C.ShouldBe(46.674477461645743);
            cieLch.H.ShouldBe(64.229726973355483);
            cieLch.Observer.ShouldBe(2);
            cieLch.Illuminant.ShouldBe(IlluminantType.D65);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLch);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLch
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToCieLchEqual()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLch
            var cieLch = ConversionTools.ToCieLch(ColorInstance.RGB, 10, IlluminantType.E);

            // Check for property correctness
            cieLch.L.ShouldBe(40.055099179556059);
            cieLch.C.ShouldBe(43.471087813484708);
            cieLch.H.ShouldBe(68.552930671082933);
            cieLch.Observer.ShouldBe(10);
            cieLch.Illuminant.ShouldBe(IlluminantType.E);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(cieLch);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to HWB
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestConvertRgbToHwb()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HWB
            var hwb = ConversionTools.ToHwb(ColorInstance.RGB);

            // Check for property correctness
            hwb.HueWhole.ShouldBe(350);
            hwb.ReverseHueWhole.ShouldBe(170);
            hwb.WhitenessWhole.ShouldBe(0);
            hwb.BlacknessWhole.ShouldBe(45);

            // Now, convert back to RGB
            var rgb = ConversionTools.ToRgb(hwb);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(21);
        }

        /// <summary>
        /// Tests converting an RGB color to CMYK
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToCmyk()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CMYK
            var cmyk = ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(ColorInstance.RGB);

            // Check for property correctness
            cmyk.KWhole.ShouldBe(45);
            cmyk.CMY.CWhole.ShouldBe(0);
            cmyk.CMY.MWhole.ShouldBe(100);
            cmyk.CMY.YWhole.ShouldBe(84);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(cmyk);

            // Check for property correctness
            rgb.R.ShouldBe(140);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to HSL
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToHsl()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HSL
            var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(ColorInstance.RGB);

            // Check for property correctness
            hsl.HueWhole.ShouldBe(350);
            hsl.ReverseHueWhole.ShouldBe(170);
            hsl.SaturationWhole.ShouldBe(100);
            hsl.LightnessWhole.ShouldBe(27);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(hsl);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CMY
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToCmy()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CMY
            var cmy = ConversionTools.ConvertFromRgb<CyanMagentaYellow>(ColorInstance.RGB);

            // Check for property correctness
            cmy.CWhole.ShouldBe(45);
            cmy.MWhole.ShouldBe(100);
            cmy.YWhole.ShouldBe(91);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(cmy);

            // Check for property correctness
            rgb.R.ShouldBe(140);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(23);
        }

        /// <summary>
        /// Tests converting an RGB color to HSV
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToHsv()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HSV
            var hsv = ConversionTools.ConvertFromRgb<HueSaturationValue>(ColorInstance.RGB);

            // Check for property correctness
            hsv.HueWhole.ShouldBe(350);
            hsv.ReverseHueWhole.ShouldBe(170);
            hsv.SaturationWhole.ShouldBe(100);
            hsv.ValueWhole.ShouldBe(54);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(hsv);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to RYB
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToRyb()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to RYB
            var ryb = ConversionTools.ConvertFromRgb<RedYellowBlue>(ColorInstance.RGB);

            // Check for property correctness
            ryb.R.ShouldBe(139);
            ryb.Y.ShouldBe(137);
            ryb.B.ShouldBe(22);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(ryb);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(79);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YIQ
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToYiq()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YIQ
            var yiq = ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(ColorInstance.RGB);

            // Check for property correctness
            yiq.Luma.ShouldBe(91);
            yiq.InPhase.ShouldBe(181);
            yiq.Quadrature.ShouldBe(122);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(yiq);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YUV
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToYuv()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YUV
            var yuv = ConversionTools.ConvertFromRgb<LumaChromaUv>(ColorInstance.RGB);

            // Check for property correctness
            yuv.Luma.ShouldBe(91);
            yuv.ChromaU.ShouldBe(89);
            yuv.ChromaV.ShouldBe(162);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(yuv);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(54);
        }

        /// <summary>
        /// Tests converting an RGB color to XYZ
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToXyz()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to XYZ
            var xyz = ConversionTools.ConvertFromRgb<Xyz>(ColorInstance.RGB);

            // Check for property correctness
            xyz.X.ShouldBe(13.660940262318197);
            xyz.Y.ShouldBe(11.284216455358383);
            xyz.Z.ShouldBe(2.2171176575479863);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(xyz);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YXY
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToYxy()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YXY
            var yxy = ConversionTools.ConvertFromRgb<Yxy>(ColorInstance.RGB);

            // Check for property correctness
            yxy.Y2.ShouldBe(11.284216455358383);
            yxy.X.ShouldBe(0.50293801150829631);
            yxy.Y1.ShouldBe(0.41543709850935812);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(yxy);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to HunterLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToHunterLab()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HunterLab
            var hunterLab = ConversionTools.ConvertFromRgb<HunterLab>(ColorInstance.RGB);

            // Check for property correctness
            hunterLab.L.ShouldBe(33.591987817570995);
            hunterLab.A.ShouldBe(13.805076366856513);
            hunterLab.B.ShouldBe(19.601169467400634);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(hunterLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToCieLab()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLab
            var cieLab = ConversionTools.ConvertFromRgb<CieLab>(ColorInstance.RGB);

            // Check for property correctness
            cieLab.L.ShouldBe(40.055099179556059);
            cieLab.A.ShouldBe(20.292379028766018);
            cieLab.B.ShouldBe(42.032442228242864);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(cieLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLuv
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToCieLuv()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLuv
            var cieLuv = ConversionTools.ConvertFromRgb<CieLuv>(ColorInstance.RGB);

            // Check for property correctness
            cieLuv.L.ShouldBe(40.055099179556059);
            cieLuv.U.ShouldBe(47.074237421611734);
            cieLuv.V.ShouldBe(35.083777826558624);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(cieLuv);

            // Check for property correctness
            rgb.R.ShouldBe(133);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(66);
        }

        /// <summary>
        /// Tests converting an RGB color to HWB
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericConvertRgbToHwb()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HWB
            var hwb = ConversionTools.ConvertFromRgb<HueWhiteBlack>(ColorInstance.RGB);

            // Check for property correctness
            hwb.HueWhole.ShouldBe(350);
            hwb.ReverseHueWhole.ShouldBe(170);
            hwb.WhitenessWhole.ShouldBe(0);
            hwb.BlacknessWhole.ShouldBe(45);

            // Now, convert back to RGB
            var rgb = ConversionTools.ConvertToRgb(hwb);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(21);
        }

        /// <summary>
        /// Tests converting an RGB color to CMYK
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToCmyk()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CMYK
            var cmyk = ConversionTools.GetConvertedColorModel<RedGreenBlue, CyanMagentaYellowKey>(ColorInstance.RGB);

            // Check for property correctness
            cmyk.KWhole.ShouldBe(45);
            cmyk.CMY.CWhole.ShouldBe(0);
            cmyk.CMY.MWhole.ShouldBe(100);
            cmyk.CMY.YWhole.ShouldBe(84);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<CyanMagentaYellowKey, RedGreenBlue>(cmyk);

            // Check for property correctness
            rgb.R.ShouldBe(140);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to HSL
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToHsl()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HSL
            var hsl = ConversionTools.GetConvertedColorModel<RedGreenBlue, HueSaturationLightness>(ColorInstance.RGB);

            // Check for property correctness
            hsl.HueWhole.ShouldBe(350);
            hsl.ReverseHueWhole.ShouldBe(170);
            hsl.SaturationWhole.ShouldBe(100);
            hsl.LightnessWhole.ShouldBe(27);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<HueSaturationLightness, RedGreenBlue>(hsl);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CMY
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToCmy()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CMY
            var cmy = ConversionTools.GetConvertedColorModel<RedGreenBlue, CyanMagentaYellow>(ColorInstance.RGB);

            // Check for property correctness
            cmy.CWhole.ShouldBe(45);
            cmy.MWhole.ShouldBe(100);
            cmy.YWhole.ShouldBe(91);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<CyanMagentaYellow, RedGreenBlue>(cmy);

            // Check for property correctness
            rgb.R.ShouldBe(140);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(23);
        }

        /// <summary>
        /// Tests converting an RGB color to HSV
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToHsv()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HSV
            var hsv = ConversionTools.GetConvertedColorModel<RedGreenBlue, HueSaturationValue>(ColorInstance.RGB);

            // Check for property correctness
            hsv.HueWhole.ShouldBe(350);
            hsv.ReverseHueWhole.ShouldBe(170);
            hsv.SaturationWhole.ShouldBe(100);
            hsv.ValueWhole.ShouldBe(54);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<HueSaturationValue, RedGreenBlue>(hsv);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to RYB
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToRyb()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to RYB
            var ryb = ConversionTools.GetConvertedColorModel<RedGreenBlue, RedYellowBlue>(ColorInstance.RGB);

            // Check for property correctness
            ryb.R.ShouldBe(139);
            ryb.Y.ShouldBe(137);
            ryb.B.ShouldBe(22);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<RedYellowBlue, RedGreenBlue>(ryb);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(79);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YIQ
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToYiq()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YIQ
            var yiq = ConversionTools.GetConvertedColorModel<RedGreenBlue, LumaInPhaseQuadrature>(ColorInstance.RGB);

            // Check for property correctness
            yiq.Luma.ShouldBe(91);
            yiq.InPhase.ShouldBe(181);
            yiq.Quadrature.ShouldBe(122);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<LumaInPhaseQuadrature, RedGreenBlue>(yiq);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YUV
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToYuv()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YUV
            var yuv = ConversionTools.GetConvertedColorModel<RedGreenBlue, LumaChromaUv>(ColorInstance.RGB);

            // Check for property correctness
            yuv.Luma.ShouldBe(91);
            yuv.ChromaU.ShouldBe(89);
            yuv.ChromaV.ShouldBe(162);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<LumaChromaUv, RedGreenBlue>(yuv);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(54);
        }

        /// <summary>
        /// Tests converting an RGB color to XYZ
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToXyz()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to XYZ
            var xyz = ConversionTools.GetConvertedColorModel<RedGreenBlue, Xyz>(ColorInstance.RGB);

            // Check for property correctness
            xyz.X.ShouldBe(13.660940262318197);
            xyz.Y.ShouldBe(11.284216455358383);
            xyz.Z.ShouldBe(2.2171176575479863);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<Xyz, RedGreenBlue>(xyz);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to YXY
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToYxy()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to YXY
            var yxy = ConversionTools.GetConvertedColorModel<RedGreenBlue, Yxy>(ColorInstance.RGB);

            // Check for property correctness
            yxy.Y2.ShouldBe(11.284216455358383);
            yxy.X.ShouldBe(0.50293801150829631);
            yxy.Y1.ShouldBe(0.41543709850935812);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<Yxy, RedGreenBlue>(yxy);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to HunterLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToHunterLab()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HunterLab
            var hunterLab = ConversionTools.GetConvertedColorModel<RedGreenBlue, HunterLab>(ColorInstance.RGB);

            // Check for property correctness
            hunterLab.L.ShouldBe(33.591987817570995);
            hunterLab.A.ShouldBe(13.805076366856513);
            hunterLab.B.ShouldBe(19.601169467400634);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<HunterLab, RedGreenBlue>(hunterLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLab
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToCieLab()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLab
            var cieLab = ConversionTools.GetConvertedColorModel<RedGreenBlue, CieLab>(ColorInstance.RGB);

            // Check for property correctness
            cieLab.L.ShouldBe(40.055099179556059);
            cieLab.A.ShouldBe(20.292379028766018);
            cieLab.B.ShouldBe(42.032442228242864);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<CieLab, RedGreenBlue>(cieLab);

            // Check for property correctness
            rgb.R.ShouldBe(138);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(22);
        }

        /// <summary>
        /// Tests converting an RGB color to CieLuv
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToCieLuv()
        {
            // Create instance
            var ColorInstance = new Color(139, 80, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;80;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(80);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to CieLuv
            var cieLuv = ConversionTools.GetConvertedColorModel<RedGreenBlue, CieLuv>(ColorInstance.RGB);

            // Check for property correctness
            cieLuv.L.ShouldBe(40.055099179556059);
            cieLuv.U.ShouldBe(47.074237421611734);
            cieLuv.V.ShouldBe(35.083777826558624);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<CieLuv, RedGreenBlue>(cieLuv);

            // Check for property correctness
            rgb.R.ShouldBe(133);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(66);
        }

        /// <summary>
        /// Tests converting an RGB color to HWB
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGenericBidirectionalConvertRgbToHwb()
        {
            // Create instance
            var ColorInstance = new Color(139, 0, 22);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground().ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground().ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground().ShouldBe($"{VtSequenceBasicChars.EscapeChar}[38;2;139;0;22m");
            ColorInstance.RGB.R.ShouldBe(139);
            ColorInstance.RGB.G.ShouldBe(0);
            ColorInstance.RGB.B.ShouldBe(22);

            // Now, convert to HWB
            var hwb = ConversionTools.GetConvertedColorModel<RedGreenBlue, HueWhiteBlack>(ColorInstance.RGB);

            // Check for property correctness
            hwb.HueWhole.ShouldBe(350);
            hwb.ReverseHueWhole.ShouldBe(170);
            hwb.WhitenessWhole.ShouldBe(0);
            hwb.BlacknessWhole.ShouldBe(45);

            // Now, convert back to RGB
            var rgb = ConversionTools.GetConvertedColorModel<HueWhiteBlack, RedGreenBlue>(hwb);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(0);
            rgb.B.ShouldBe(21);
        }
    }
}
