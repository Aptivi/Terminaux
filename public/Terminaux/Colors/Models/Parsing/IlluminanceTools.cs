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

using System.Collections.Generic;
using Terminaux.Base;

namespace Terminaux.Colors.Models.Parsing
{
    /// <summary>
    /// Standard illuminance tools
    /// </summary>
    public static class IlluminanceTools
    {
        private static Dictionary<(int observer, IlluminantType illuminant), (double refX, double refY, double refZ)> illuminanceTable = new()
        {
            { (2, IlluminantType.A), (109.850, 100, 35.585) },
            { (10, IlluminantType.A), (111.144, 100, 35.200) },
            { (2, IlluminantType.B), (99.0927, 100, 85.313) },
            { (10, IlluminantType.B), (99.178, 100, 84.3493) },
            { (2, IlluminantType.C), (98.074, 100, 118.232) },
            { (10, IlluminantType.C), (97.285, 100, 116.145) },
            { (2, IlluminantType.D50), (96.422, 100, 82.521) },
            { (10, IlluminantType.D50), (96.720, 100, 81.427) },
            { (2, IlluminantType.D55), (95.682, 100, 92.149) },
            { (10, IlluminantType.D55), (95.799, 100, 90.926) },
            { (2, IlluminantType.D65), (95.047, 100, 108.883) },
            { (10, IlluminantType.D65), (94.811, 100, 107.304) },
            { (2, IlluminantType.D75), (94.972, 100, 122.638) },
            { (10, IlluminantType.D75), (94.416, 100, 120.641) },
            { (2, IlluminantType.E), (100, 100, 100) },
            { (10, IlluminantType.E), (100, 100, 100) },
            { (2, IlluminantType.F1), (92.834, 100, 103.665) },
            { (10, IlluminantType.F1), (94.791, 100, 103.191) },
            { (2, IlluminantType.F2), (99.187, 100, 67.395) },
            { (10, IlluminantType.F2), (103.280, 100, 69.026) },
            { (2, IlluminantType.F3), (103.754, 100, 49.861) },
            { (10, IlluminantType.F3), (108.968, 100, 51.965) },
            { (2, IlluminantType.F4), (109.147, 100, 38.813) },
            { (10, IlluminantType.F4), (114.961, 100, 40.963) },
            { (2, IlluminantType.F5), (90.872, 100, 98.723) },
            { (10, IlluminantType.F5), (93.369, 100, 98.636) },
            { (2, IlluminantType.F6), (97.309, 100, 60.191) },
            { (10, IlluminantType.F6), (102.148, 100, 62.074) },
            { (2, IlluminantType.F7), (95.044, 100, 108.755) },
            { (10, IlluminantType.F7), (95.792, 100, 107.687) },
            { (2, IlluminantType.F8), (96.413, 100, 82.333) },
            { (10, IlluminantType.F8), (97.115, 100, 81.135) },
            { (2, IlluminantType.F9), (100.365, 100, 67.868) },
            { (10, IlluminantType.F9), (102.116, 100, 67.826) },
            { (2, IlluminantType.F10), (96.174, 100, 81.712) },
            { (10, IlluminantType.F10), (99.001, 100, 83.134) },
            { (2, IlluminantType.F11), (100.966, 100, 64.370) },
            { (10, IlluminantType.F11), (103.866, 100, 65.627) },
            { (2, IlluminantType.F12), (108.046, 100, 39.228) },
            { (10, IlluminantType.F12), (111.428, 100, 40.353) },
        };

        /// <summary>
        /// Gets the illuminant references
        /// </summary>
        /// <param name="observer">Observer (2 degs or 10 degs)</param>
        /// <param name="illuminant">Illuminant type</param>
        /// <returns>Illuminant references for calculation</returns>
        /// <exception cref="TerminauxException"></exception>
        public static (double refX, double refY, double refZ) GetIlluminantReferences(int observer, IlluminantType illuminant)
        {
            // Check the observer
            if (observer != 2 && observer != 10)
                throw new TerminauxException($"Observer must be either 2 or 10: {observer}");

            // Check the illuminant
            if (illuminant < 0 || illuminant > IlluminantType.F12)
                throw new TerminauxException($"Illuminant is invalid: {(int)illuminant}");

            // Now, get the references from the table
            if (!illuminanceTable.TryGetValue((observer, illuminant), out var references))
                throw new TerminauxException($"Can't get references from the table: {observer} degs, {(int)illuminant}");
            return references;
        }
    }
}
