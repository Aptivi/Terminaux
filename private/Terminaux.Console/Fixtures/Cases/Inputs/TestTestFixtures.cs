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
using Colorimetry.Data;
using Terminaux.Console.Fixtures.Cases.CaseData;
using Terminaux.Inputs.TestFixtures;
using Terminaux.Inputs.TestFixtures.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestTestFixtures : IFixture
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

        public void RunFixture()
        {
            TextWriterColor.Write("Running fixture 1...");
            bool result = FixtureRunner.RunTest((FixtureUnconditional<Action>?)fixtures[0], out var exc);
            TextWriterColor.WriteColor($"Outcome: {result} [{(exc is not null ? exc.Message : "No error")}]", result ? ConsoleColors.Lime : ConsoleColors.Red);

            TextWriterColor.Write("Running fixture 2...");
            bool result2 = FixtureRunner.RunTest((FixtureUnconditional<Action<string>>?)fixtures[1], out var exc2, "John");
            TextWriterColor.WriteColor($"Outcome: {result2} [{(exc2 is not null ? exc2.Message : "No error")}]", result2 ? ConsoleColors.Lime : ConsoleColors.Red);

            TextWriterColor.Write("Running fixture 3...");
            bool result3 = FixtureRunner.RunTest<Func<int>, int>((FixtureConditional<Func<int>>?)fixtures[2], out var exc3);
            TextWriterColor.WriteColor($"Outcome: {result3} [{(exc3 is not null ? exc3.Message : "No error")}]", result3 ? ConsoleColors.Lime : ConsoleColors.Red);

            TextWriterColor.Write("Running fixture 4...");
            bool result4 = FixtureRunner.RunTest<Func<double, double, double>, double>((FixtureConditional<Func<double, double, double>>?)fixtures[3], out var exc4, 8, 4);
            TextWriterColor.WriteColor($"Outcome: {result4} [{(exc4 is not null ? exc4.Message : "No error")}]", result4 ? ConsoleColors.Lime : ConsoleColors.Red);

            TextWriterColor.Write("Running fixture 4 (expected fail)...");
            bool result5 = FixtureRunner.RunTest<Func<double, double, double>, double>((FixtureConditional<Func<double, double, double>>?)fixtures[3], out var exc5, 11, 4);
            TextWriterColor.WriteColor($"Outcome: {result5} [{(exc5 is not null ? exc5.Message : "No error")}]", result5 ? ConsoleColors.Lime : ConsoleColors.Red);
        }
    }
}
