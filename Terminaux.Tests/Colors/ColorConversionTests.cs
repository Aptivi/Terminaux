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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Colors;
using Terminaux.Colors.Models;
using Terminaux.Colors.Models.Conversion;

namespace Terminaux.Tests.Colors
{
    [TestClass]
    public partial class ColorConversionTests
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;0;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;0;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;0;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("139;80;22");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe("\u001b[48;2;139;80;22m");
            ColorInstance.VTSequenceForeground.ShouldBe("\u001b[38;2;139;80;22m");
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
    }
}
