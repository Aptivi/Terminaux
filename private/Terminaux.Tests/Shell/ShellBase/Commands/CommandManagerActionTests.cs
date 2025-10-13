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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Tests.Shell.ShellBase.Commands.TestCommands;
using Terminaux.Base;
using Terminaux.Tests.Shared.Shells;
using Terminaux.Shell.Shells;

namespace Terminaux.Tests.Shell.ShellBase.Commands
{
    [TestClass]
    public class CommandManagerActionTests
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
        /// Tests seeing if the command is found in specific shell (test case: lsr command)
        /// </summary>
        [TestMethod]
        [DataRow("TestShell", false)]
        [Description("Action")]
        public void TestIsCommandFoundInSpecificShell(string type, bool expected)
        {
            bool actual = CommandManager.IsCommandFound("lsr", type);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests seeing if the command is found in all the shells (test case: wrap command)
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestIsCommandFoundInAllTheShells() =>
            CommandManager.IsCommandFound("wrap").ShouldBeTrue();

        /// <summary>
        /// Tests registering the command
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestRegisterCommand(string type)
        {
            Should.NotThrow(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("mycmd2", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ));
            CommandManager.IsCommandFound("mycmd2", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestRegisterEmptyCommandName(string type)
        {
            Should.Throw(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ), typeof(TerminauxException));
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestRegisterCommandConflicting(string type)
        {
            Should.Throw(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ), typeof(TerminauxException));
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestRegisterNullCommand(string type) =>
            Should.Throw(() => CommandManager.RegisterCustomCommand(type, null), typeof(TerminauxException));

        /// <summary>
        /// Tests unregistering the command
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestUnregisterCommand(string type)
        {
            Should.NotThrow(() => CommandManager.UnregisterCustomCommand(type, "mycmd2"));
            CommandManager.IsCommandFound("mycmd2", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestUnregisterNonexistentCommand(string type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, "mycmd4"), typeof(TerminauxException));

        /// <summary>
        /// Tests registering the commands
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestRegisterCommands(string type)
        {
            var commandInfos = new CommandInfo[]
            {
                new("cmdgroup3", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("cmdgroup4", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("cmdgroup5", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),
            };
            Should.NotThrow(() => CommandManager.RegisterCustomCommands(type, commandInfos));
            CommandManager.IsCommandFound("cmdgroup3", type).ShouldBeTrue();
            CommandManager.IsCommandFound("cmdgroup4", type).ShouldBeTrue();
            CommandManager.IsCommandFound("cmdgroup5", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the commands (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestRegisterCommandsWithErrors(string type)
        {
            var commandInfos = new CommandInfo[]
            {
                new("command2", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),
            };
            Should.Throw(() => CommandManager.RegisterCustomCommands(type, commandInfos), typeof(TerminauxException));
            CommandManager.IsCommandFound("command2", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests unregistering the commands
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestUnregisterCommands(string type)
        {
            var commandInfos = new string[]
            {
                "cmdgroup3",
                "cmdgroup4",
                "cmdgroup5"
            };
            Should.NotThrow(() => CommandManager.UnregisterCustomCommands(type, commandInfos));
            CommandManager.IsCommandFound("cmdgroup3", type).ShouldBeFalse();
            CommandManager.IsCommandFound("cmdgroup4", type).ShouldBeFalse();
            CommandManager.IsCommandFound("cmdgroup5", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the commands (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestUnregisterCommandsWithErrors(string type)
        {
            var commandInfos = new string[]
            {
                "command2",
                "exit",
                ""
            };
            Should.Throw(() => CommandManager.UnregisterCustomCommands(type, commandInfos), typeof(TerminauxException));
            CommandManager.IsCommandFound("command2", type).ShouldBeFalse();
        }
    }
}
