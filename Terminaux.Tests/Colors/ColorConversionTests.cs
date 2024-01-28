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
            var cmyk = CmykConversionTools.ConvertFrom(ColorInstance.RGB);

            // Check for property correctness
            cmyk.KWhole.ShouldBe(45);
            cmyk.CMY.CWhole.ShouldBe(0);
            cmyk.CMY.MWhole.ShouldBe(100);
            cmyk.CMY.YWhole.ShouldBe(84);

            // Now, convert back to RGB
            var rgb = RgbConversionTools.ConvertFrom(cmyk);

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
            var hsl = HslConversionTools.ConvertFrom(ColorInstance.RGB);

            // Check for property correctness
            hsl.HueWhole.ShouldBe(350);
            hsl.ReverseHueWhole.ShouldBe(170);
            hsl.SaturationWhole.ShouldBe(100);
            hsl.LightnessWhole.ShouldBe(27);

            // Now, convert back to RGB
            var rgb = RgbConversionTools.ConvertFrom(hsl);

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
            var cmy = CmyConversionTools.ConvertFrom(ColorInstance.RGB);

            // Check for property correctness
            cmy.CWhole.ShouldBe(45);
            cmy.MWhole.ShouldBe(100);
            cmy.YWhole.ShouldBe(91);

            // Now, convert back to RGB
            var rgb = RgbConversionTools.ConvertFrom(cmy);

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
            var hsv = HsvConversionTools.ConvertFrom(ColorInstance.RGB);

            // Check for property correctness
            hsv.HueWhole.ShouldBe(350);
            hsv.ReverseHueWhole.ShouldBe(170);
            hsv.SaturationWhole.ShouldBe(100);
            hsv.ValueWhole.ShouldBe(54);

            // Now, convert back to RGB
            var rgb = RgbConversionTools.ConvertFrom(hsv);

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
            var ryb = RybConversionTools.ConvertFrom(ColorInstance.RGB);

            // Check for property correctness
            ryb.R.ShouldBe(139);
            ryb.Y.ShouldBe(137);
            ryb.B.ShouldBe(22);

            // Now, convert back to RGB
            var rgb = RgbConversionTools.ConvertFrom(ryb);

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
            var yiq = YiqConversionTools.ConvertFrom(ColorInstance.RGB);

            // Check for property correctness
            yiq.Luma.ShouldBe(91);
            yiq.InPhase.ShouldBe(181);
            yiq.Quadrature.ShouldBe(122);

            // Now, convert back to RGB
            var rgb = RgbConversionTools.ConvertFrom(yiq);

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
            var yuv = YuvConversionTools.ConvertFrom(ColorInstance.RGB);

            // Check for property correctness
            yuv.Luma.ShouldBe(91);
            yuv.ChromaU.ShouldBe(89);
            yuv.ChromaV.ShouldBe(162);

            // Now, convert back to RGB
            var rgb = RgbConversionTools.ConvertFrom(yuv);

            // Check for property correctness
            rgb.R.ShouldBe(139);
            rgb.G.ShouldBe(80);
            rgb.B.ShouldBe(54);
        }
    }
}
