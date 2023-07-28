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

using System;

namespace Terminaux.Colors.Accessibility
{
    // Refer to Viénot, F., Brettel, H., & Mollon, J. D. (1999). Digital video colourmaps for checking the legibility of displays by dichromats. Color Research & Application, 24(4), 243–252.
    // for more information.
    internal static class Vienot1999
    {
        static readonly VienotParameters vn_protan = new()
        {
            TransPlane = new double[9]
            {
                0.11238, 0.88762, 0.00000,
                0.11238, 0.88762, -0.00000,
                0.00401, -0.00401, 1.00000
            }
        };

        static readonly VienotParameters vn_deutan = new()
        {
            TransPlane = new double[9]
            {
                0.29275, 0.70725, 0.00000,
                0.29275, 0.70725, -0.00000,
                -0.02234, 0.02234, 1.00000
            }
        };

        static readonly VienotParameters vn_tritan = new()
        {
            TransPlane = new double[9]
            {
                1.00000, 0.14461, -0.14461,
                0.00000, 0.85924, 0.14076,
                -0.00000, 0.85924, 0.14076
            }
        };

        public static (int, int, int) Transform(int r, int g, int b, Deficiency def, double severity)
        {
            // Check values
            if (r < 0 || r > 255)
                throw new ArgumentOutOfRangeException("r");
            if (g < 0 || g > 255)
                throw new ArgumentOutOfRangeException("g");
            if (b < 0 || b > 255)
                throw new ArgumentOutOfRangeException("b");
            if (severity < 0.0d || severity > 1.0d)
                throw new ArgumentOutOfRangeException("severity");

            // Select what Vienot deficiency profile to choose how to transform the three RGB values
            VienotParameters vn = null;
            switch (def)
            {
                case Deficiency.Protan:
                    vn = vn_protan;
                    break;
                case Deficiency.Deutan:
                    vn = vn_deutan;
                    break;
                case Deficiency.Tritan:
                    vn = vn_tritan;
                    break;
            }

            // Get linear RGB from these three RGB values
            double[] linears = new double[3]
            {
                ColorTools.SRGBToLinearRGB(r),
                ColorTools.SRGBToLinearRGB(g),
                ColorTools.SRGBToLinearRGB(b)
            };

            var vnt = vn.TransPlane;
            double[] rgbMatrix = new double[3]
            {
                vnt[0]*linears[0] + vnt[1]*linears[1] + vnt[2]*linears[2],
                vnt[3]*linears[0] + vnt[4]*linears[1] + vnt[5]*linears[2],
                vnt[6]*linears[0] + vnt[7]*linears[1] + vnt[8]*linears[2],
            };

            // Transform the colors with the severity rate in a linear transform method
            if (severity < 0.999d)
            {
                rgbMatrix[0] = rgbMatrix[0] * severity + linears[0] * (1d - severity);
                rgbMatrix[1] = rgbMatrix[1] * severity + linears[1] * (1d - severity);
                rgbMatrix[2] = rgbMatrix[2] * severity + linears[2] * (1d - severity);
            }

            // Convert these values back to sRGB (domain is [0,255])
            int sRGB_R = ColorTools.LinearRGBTosRGB(rgbMatrix[0]);
            int sRGB_G = ColorTools.LinearRGBTosRGB(rgbMatrix[1]);
            int sRGB_B = ColorTools.LinearRGBTosRGB(rgbMatrix[2]);
            return (sRGB_R, sRGB_G, sRGB_B);
        }
    }
}
