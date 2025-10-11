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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Tests.Shared.Shells;

namespace Terminaux.Tests.Shell.ShellBase.Commands
{

    [TestClass]
    public class CommandManagerQueryingTests
    {
        [ClassInitialize]
        public static void InitializeTests(TestContext testContext)
        {
            ShellManager.RegisterShell("TestShell", new TestShellInfo());
            testContext.WriteLine("Created new test shell " + testContext.FullyQualifiedTestClassName);
        }

        [ClassCleanup]
        public static void CleanTests(TestContext testContext)
        {
            ShellManager.RegisterShell("TestShell", new TestShellInfo());
            testContext.WriteLine("Removed test shell " + testContext.FullyQualifiedTestClassName);
        }

        /// <summary>
        /// Tests getting list of commands
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetCommandList()
        {
            var Commands = CommandManager.GetCommandNames("TestShell");
            Console.WriteLine(format: "Commands from Shell: {0} commands", Commands.Length);
            Console.WriteLine(format: string.Join(", ", Commands));
            Commands.ShouldNotBeNull();
            Commands.ShouldNotBeEmpty();
        }
    }
}
