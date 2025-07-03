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

namespace Terminaux.Inputs.TestFixtures.Tools
{
    /// <summary>
    /// Fixture runner class to start test fixtures
    /// </summary>
    public static class FixtureRunner
    {
        /// <summary>
        /// Runs the test fixture for functions that return void
        /// </summary>
        /// <typeparam name="TDelegate">Delegate type that points to functions that return void, such as <see cref="Action"/> delegates</typeparam>
        /// <param name="fixture">Test fixture to test against. It must be of type <see cref="FixtureUnconditional{TDelegate}"/></param>
        /// <param name="ex">Output parameter for any possible exception being thrown, only populated if the test succeeded</param>
        /// <param name="args">List of arguments</param>
        /// <returns><see langword="true"/> if test has succeeded; <see langword="false"/> otherwise</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool RunTest<TDelegate>(FixtureUnconditional<TDelegate>? fixture, out Exception? ex, params object?[]? args)
            where TDelegate : Delegate
        {
            ex = null;

            // Check for fixture
            if (fixture is null)
                throw new TerminauxException(nameof(fixture));
            if (fixture.GetType() == typeof(Fixture))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_MAYNOTBEBASE_UNCONDITIONAL"));
            args ??= fixture.initialParameters;

            // Run the fixture
            bool success = true;
            try
            {
                fixture.fixtureDelegate.DynamicInvoke(args);
            }
            catch (Exception exc)
            {
                ex = exc;
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Runs the test fixture for functions that return void
        /// </summary>
        /// <param name="fixture">Test fixture to test against. It must be of type <see cref="FixtureUnconditional"/></param>
        /// <param name="ex">Output parameter for any possible exception being thrown, only populated if the test succeeded</param>
        /// <param name="args">List of arguments</param>
        /// <returns><see langword="true"/> if test has succeeded; <see langword="false"/> otherwise</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool RunUnconditionalTest(FixtureUnconditional? fixture, out Exception? ex, params object?[]? args)
        {
            ex = null;

            // Check for fixture
            if (fixture is null)
                throw new TerminauxException(nameof(fixture));
            if (fixture.GetType() == typeof(Fixture))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_MAYNOTBEBASE_UNCONDITIONAL"));
            args ??= fixture.initialParameters;

            // Run the fixture
            bool success = true;
            try
            {
                fixture.fixtureDelegate.DynamicInvoke(args);
            }
            catch (Exception exc)
            {
                ex = exc;
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Runs the test fixture for functions that don't return void and compares against the returned value
        /// </summary>
        /// <typeparam name="TDelegate">Delegate type that points to functions that don't return void, such as <see cref="Func{TResult}"/> delegates</typeparam>
        /// <typeparam name="TValue">Value type that must match the expected type value</typeparam>
        /// <param name="fixture">Test fixture to test against. It must be of type <see cref="FixtureConditional{TDelegate}"/></param>
        /// <param name="ex">Output parameter for any possible exception being thrown, only populated if the test succeeded</param>
        /// <param name="args">List of arguments</param>
        /// <returns><see langword="true"/> if test has succeeded and the two values match; <see langword="false"/> otherwise</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool RunTest<TDelegate, TValue>(FixtureConditional<TDelegate>? fixture, out Exception? ex, params object?[]? args)
            where TDelegate : Delegate
        {
            ex = null;

            // Check for fixture
            if (fixture is null)
                throw new TerminauxException(nameof(fixture));
            if (fixture.GetType() == typeof(Fixture))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_MAYNOTBEBASE_CONDITIONAL"));
            args ??= fixture.initialParameters;

            // Run the fixture
            bool success = true;
            try
            {
                var returned = (TValue)fixture.fixtureDelegate.DynamicInvoke(args);
                var expected = (TValue?)fixture.expectedValue;
                if (!returned.Equals(expected))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_VALUEMISMATCH_CONDITIONAL"), returned, fixture.expectedValue ?? "");
            }
            catch (Exception exc)
            {
                ex = exc;
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Runs the test fixture for functions that don't return void and compares against the returned value
        /// </summary>
        /// <typeparam name="TValue">Value type that must match the expected type value</typeparam>
        /// <param name="fixture">Test fixture to test against. It must be of type <see cref="FixtureConditional"/></param>
        /// <param name="ex">Output parameter for any possible exception being thrown, only populated if the test succeeded</param>
        /// <param name="args">List of arguments</param>
        /// <returns><see langword="true"/> if test has succeeded and the two values match; <see langword="false"/> otherwise</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool RunConditionalTest<TValue>(FixtureConditional? fixture, out Exception? ex, params object?[]? args)
        {
            ex = null;

            // Check for fixture
            if (fixture is null)
                throw new TerminauxException(nameof(fixture));
            if (fixture.GetType() == typeof(Fixture))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_MAYNOTBEBASE_CONDITIONAL"));
            args ??= fixture.initialParameters;

            // Run the fixture
            bool success = true;
            try
            {
                var returned = (TValue)fixture.fixtureDelegate.DynamicInvoke(args);
                var expected = (TValue?)fixture.expectedValue;
                if (!returned.Equals(expected))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_VALUEMISMATCH_CONDITIONAL"), returned, fixture.expectedValue ?? "");
            }
            catch (Exception exc)
            {
                ex = exc;
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Runs the general-purpose test fixture
        /// </summary>
        /// <param name="fixture">Test fixture to test against. It must not be of type <see cref="Fixture"/>, but one of its derivatives</param>
        /// <param name="ex">Output parameter for any possible exception being thrown, only populated if the test succeeded</param>
        /// <param name="args">List of arguments</param>
        /// <returns><see langword="true"/> if test has succeeded and the two values match; <see langword="false"/> otherwise</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool RunGeneralTest(Fixture? fixture, out Exception? ex, params object?[]? args)
        {
            if (fixture is null)
                throw new TerminauxException(nameof(fixture));
            if (fixture.GetType() == typeof(Fixture))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_UNCONDITIONAL"));
            args ??= fixture.initialParameters;
            if (fixture.GetType() == typeof(FixtureConditional) || fixture.GetType().BaseType == typeof(FixtureConditional))
                return RunConditionalTest<object>((FixtureConditional?)fixture, out ex, args);
            else if (fixture.GetType() == typeof(FixtureUnconditional) || fixture.GetType().BaseType == typeof(FixtureUnconditional))
                return RunUnconditionalTest((FixtureUnconditional)fixture, out ex, args);
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_TESTFIXTURES_EXCEPTION_INVALIDTYPE"));
        }
    }
}
