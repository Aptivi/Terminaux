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

using System;
using System.Diagnostics;
using Terminaux.Base;

namespace Terminaux.Inputs.TestFixtures
{
    /// <summary>
    /// Test fixture class (routines that return <see langword="void"/>)
    /// </summary>
    [DebuggerDisplay("[U non-generic: {fixtureDelegate.GetType().Name}] {Name}: {Description}")]
    public class FixtureUnconditional : Fixture
    {
        internal Delegate fixtureDelegate;

        /// <summary>
        /// Makes a new test fixture class
        /// </summary>
        /// <param name="fixtureName">Fixture name</param>
        /// <param name="fixtureDesc">Fixture description</param>
        /// <param name="fixtureDelegate">Delegate that executes this test fixture</param>
        /// <param name="initialParameters">Fixture initial parameters</param>
        public FixtureUnconditional(string fixtureName, string fixtureDesc, Delegate? fixtureDelegate, params object?[]? initialParameters) :
            base(fixtureName, fixtureDesc, initialParameters)
        {
            // Check for delegate type
            if (fixtureDelegate is null)
                throw new TerminauxException(nameof(fixtureDelegate));
            if (fixtureDelegate.Method.ReturnType != typeof(void))
                throw new TerminauxException("Method in this delegate needs to return void");

            // Install values
            this.fixtureDelegate = fixtureDelegate;
        }
    }

    /// <summary>
    /// Test fixture class (routines that return <see langword="void"/>)
    /// </summary>
    [DebuggerDisplay("[U generic: {fixtureDelegate.GetType().Name}] {Name}: {Description}")]
    public class FixtureUnconditional<TDelegate> : FixtureUnconditional
        where TDelegate : Delegate
    {
        /// <summary>
        /// Makes a new test fixture class
        /// </summary>
        /// <param name="fixtureName">Fixture name</param>
        /// <param name="fixtureDesc">Fixture description</param>
        /// <param name="fixtureDelegate">Delegate that executes this test fixture</param>
        /// <param name="initialParameters">Fixture initial parameters</param>
        public FixtureUnconditional(string fixtureName, string fixtureDesc, TDelegate? fixtureDelegate, params object?[]? initialParameters) :
            base(fixtureName, fixtureDesc, fixtureDelegate, initialParameters)
        { }
    }
}
