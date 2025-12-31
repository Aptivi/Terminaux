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

using System.Diagnostics;

namespace Terminaux.Inputs.TestFixtures
{
    /// <summary>
    /// Test fixture class (general)
    /// </summary>
    [DebuggerDisplay("{Name}: {Description}")]
    public abstract class Fixture
    {
        internal string fixtureName = "";
        internal string fixtureDesc = "";
        internal object?[]? initialParameters = null;

        /// <summary>
        /// Fixture name
        /// </summary>
        public string Name =>
            fixtureName;

        /// <summary>
        /// Fixture description
        /// </summary>
        public string Description =>
            fixtureDesc;

        /// <summary>
        /// Makes a new test fixture class
        /// </summary>
        /// <param name="fixtureName">Fixture name</param>
        /// <param name="fixtureDesc">Fixture description</param>
        /// <param name="initialParameters">Fixture initial parameters</param>
        public Fixture(string fixtureName, string fixtureDesc, params object?[]? initialParameters)
        {
            // Install values
            this.fixtureName = fixtureName ?? "Untitled fixture";
            this.fixtureDesc = fixtureDesc ?? "";
            this.initialParameters = initialParameters;
        }
    }
}
