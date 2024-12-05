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
using Terminaux.Console.Fixtures.Cases.CaseData;
using Terminaux.Inputs.TestFixtures;
using Terminaux.Inputs.TestFixtures.Tools;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestTestFixturesInteractive : IFixture
    {
        private static readonly Fixture[] fixtures =
        [
            new FixtureUnconditional<Action>(nameof(UnconditionalFunctions.TestWrite), "Tests writing to console", UnconditionalFunctions.TestWrite),
            new FixtureUnconditional<Action<string>>(nameof(UnconditionalFunctions.TestWriteArgs), "Tests writing to console with arguments", UnconditionalFunctions.TestWriteArgs, "John"),
            new FixtureConditional<Func<int>>(nameof(ConditionalFunctions.TestRead), "Tests reading from console", ConditionalFunctions.TestRead, (int)'A'),
            new FixtureConditional<Func<double, double, double>>(nameof(ConditionalFunctions.TestReadArgs), "Tests reading from console with arguments", ConditionalFunctions.TestReadArgs, 0d, 4, 2),
            new FixtureConditional<Func<double, double, double>>(nameof(ConditionalFunctions.TestReadArgs) + "Fail", "Tests reading from console with arguments (failure expected)", ConditionalFunctions.TestReadArgs, 0d, 5, 2),
        ];

        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture() =>
            FixtureSelector.OpenFixtureSelector(fixtures);
    }
}
