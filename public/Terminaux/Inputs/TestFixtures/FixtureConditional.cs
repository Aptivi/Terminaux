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

using System;
using System.Diagnostics;
using Terminaux.Base;

namespace Terminaux.Inputs.TestFixtures
{
    /// <summary>
    /// Test fixture class (routines that don't return <see langword="void"/>)
    /// </summary>
    [DebuggerDisplay("[C non-generic: {fixtureDelegate.GetType().Name}] {Name}: {Description}")]
    public class FixtureConditional : Fixture
    {
        internal Delegate fixtureDelegate;
        internal object? expectedValue;

        /// <summary>
        /// Makes a new test fixture class
        /// </summary>
        /// <param name="fixtureName">Fixture name</param>
        /// <param name="fixtureDesc">Fixture description</param>
        /// <param name="fixtureDelegate">Delegate that executes this test fixture</param>
        /// <param name="expectedValue">Expected value</param>
        /// <param name="initialParameters">Fixture initial parameters</param>
        public FixtureConditional(string fixtureName, string fixtureDesc, Delegate? fixtureDelegate, object? expectedValue, params object?[]? initialParameters) :
            base(fixtureName, fixtureDesc, initialParameters)
        {
            // Check for delegate type
            if (fixtureDelegate is null)
                throw new TerminauxException(nameof(fixtureDelegate));
            if (fixtureDelegate.Method.ReturnType == typeof(void))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_CONDITIONALMETHODNOVOID"));

            // Install values
            this.fixtureDelegate = fixtureDelegate;
            this.expectedValue = expectedValue;
        }
    }

    /// <summary>
    /// Test fixture class (routines that don't return <see langword="void"/>)
    /// </summary>
    [DebuggerDisplay("[C generic: {fixtureDelegate.GetType().Name}] {Name}: {Description}")]
    public class FixtureConditional<TDelegate> : FixtureConditional
        where TDelegate : Delegate
    {
        /// <summary>
        /// Makes a new test fixture class
        /// </summary>
        /// <param name="fixtureName">Fixture name</param>
        /// <param name="fixtureDesc">Fixture description</param>
        /// <param name="fixtureDelegate">Delegate that executes this test fixture</param>
        /// <param name="expectedValue">Expected value</param>
        /// <param name="initialParameters">Fixture initial parameters</param>
        public FixtureConditional(string fixtureName, string fixtureDesc, TDelegate? fixtureDelegate, object? expectedValue, params object?[]? initialParameters) :
            base(fixtureName, fixtureDesc, fixtureDelegate, expectedValue, initialParameters)
        { }
    }
}
