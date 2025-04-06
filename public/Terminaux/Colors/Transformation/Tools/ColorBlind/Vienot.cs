//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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

using System;
using Terminaux.Base;

namespace Terminaux.Colors.Transformation.Tools.ColorBlind
{
    internal static class Vienot
    {
        internal class VienotParameters
        {
            internal double[] TransPlane = [];
        }

        static readonly VienotParameters vn_protan = new()
        {
            TransPlane =
            [
                0.11238,
                0.88762,
                0.00000,
                0.11238,
                0.88762,
                -0.00000,
                0.00401,
                -0.00401,
                1.00000
            ]
        };

        static readonly VienotParameters vn_deutan = new()
        {
            TransPlane =
            [
                0.29275,
                0.70725,
                0.00000,
                0.29275,
                0.70725,
                -0.00000,
                -0.02234,
                0.02234,
                1.00000
            ]
        };

        static readonly VienotParameters vn_tritan = new()
        {
            TransPlane =
            [
                1.00000,
                0.14461,
                -0.14461,
                0.00000,
                0.85924,
                0.14076,
                -0.00000,
                0.85924,
                0.14076
            ]
        };

        internal static (int, int, int) Transform(int r, int g, int b, ColorBlindDeficiency def, double severity)
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
            VienotParameters? vn = def switch
            {
                ColorBlindDeficiency.Protan => vn_protan,
                ColorBlindDeficiency.Deutan => vn_deutan,
                ColorBlindDeficiency.Tritan => vn_tritan,
                _ => throw new ArgumentOutOfRangeException("def"),
            };

            // Get linear RGB from these three RGB values
            double[] linears =
            [
                TransformationTools.SRGBToLinearRGB(r),
                TransformationTools.SRGBToLinearRGB(g),
                TransformationTools.SRGBToLinearRGB(b)
            ];

            var vnt = vn.TransPlane;
            double[] rgbMatrix =
            [
                vnt[0] * linears[0] + vnt[1] * linears[1] + vnt[2] * linears[2],
                vnt[3] * linears[0] + vnt[4] * linears[1] + vnt[5] * linears[2],
                vnt[6] * linears[0] + vnt[7] * linears[1] + vnt[8] * linears[2],
            ];

            // Transform the colors with the severity rate in a linear transform method
            if (severity < 0.999d)
            {
                rgbMatrix[0] = rgbMatrix[0] * severity + linears[0] * (1d - severity);
                rgbMatrix[1] = rgbMatrix[1] * severity + linears[1] * (1d - severity);
                rgbMatrix[2] = rgbMatrix[2] * severity + linears[2] * (1d - severity);
            }

            // Convert these values back to sRGB (domain is [0,255])
            int sRGB_R = TransformationTools.LinearRGBTosRGB(rgbMatrix[0]);
            int sRGB_G = TransformationTools.LinearRGBTosRGB(rgbMatrix[1]);
            int sRGB_B = TransformationTools.LinearRGBTosRGB(rgbMatrix[2]);
            ConsoleLogger.Debug("Transformed to {0} using parameters: def: {1}, severity: {2}", (sRGB_R, sRGB_G, sRGB_B), def, severity);
            return (sRGB_R, sRGB_G, sRGB_B);
        }
    }
}
