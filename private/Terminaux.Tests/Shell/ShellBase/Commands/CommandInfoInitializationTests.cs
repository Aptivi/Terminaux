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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Terminaux.Tests.Shell.ShellBase.Commands
{

    [TestClass]
    public class CommandInfoInitializationTests
    {
        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimple()
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", "Help page",
                    [
                        new CommandArgumentInfo()
                    ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArg()
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", "Help page",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "testarg")
                        ])
                    ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitch()
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", "Help page",
                    [
                        new CommandArgumentInfo(
                        [
                            new SwitchInfo("s", "Simple help")
                        ])
                    ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArg()
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", "Help page",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "testarg")
                        ],
                        [
                            new SwitchInfo("s", "Simple help")
                        ])
                    ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptions()
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", "Help page",
                    [
                        new CommandArgumentInfo(
                        [
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        ])
                    ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsAndArg()
        {
            // Create instance
            var CommandInstance =
                new CommandInfo("help", "Help page",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "testarg")
                        ],
                        [
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        ])
                    ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimpleMultiCommandArgumentInfo()
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", "Help page",
                [
                    new CommandArgumentInfo(),
                    new CommandArgumentInfo()
                ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgMultiCommandArgumentInfo()
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", "Help page",
                [
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "testarg")
                        ]),
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        ]),
                ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchMultiCommandArgumentInfo()
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", "Help page",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("s", "Simple help")
                    ]),
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("c", "Complicated help")
                    ]),
                ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeFalse();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArgMultiCommandArgumentInfo()
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", "Help page",
                [
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "testarg")
                        ],
                        [
                            new SwitchInfo("s", "Simple help")
                        ]),
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        ],
                        [
                            new SwitchInfo("c", "Complicated help")
                        ]),
                ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeTrue();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeFalse();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsMultiCommandArgumentInfo()
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", "Help page",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("s", "Simple help", new SwitchOptions
                        {
                            AcceptsValues = false,
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("c", "Complicated help", new SwitchOptions
                        {
                            AcceptsValues = false,
                            IsRequired = true,
                        })
                    ]),
                ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(0);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeTrue();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchWithOptionsAndArgMultiCommandArgumentInfo()
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", "Help page",
                [
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "testarg")
                        ],
                        [
                            new SwitchInfo("s", "Simple help", new SwitchOptions
                            {
                                AcceptsValues = false,
                            })
                        ]),
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "testme"),
                            new CommandArgumentPart(false, "path"),
                        ],
                        [
                            new SwitchInfo("c", "Complicated help", new SwitchOptions
                            {
                                AcceptsValues = false,
                                IsRequired = true,
                            })
                        ]),
                ], null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo[0].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo[0].MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo[1].Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo[1].ArgumentsRequired.ShouldBeTrue();
            CommandInstance.CommandArgumentInfo[1].MinimumArguments.ShouldBe(1);
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo[0].Switches[0];
            var @switch2 = CommandInstance.CommandArgumentInfo[1].Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch.AcceptsValues.ShouldBeFalse();
            @switch2.SwitchName.ShouldBe("c");
            @switch2.HelpDefinition.ShouldBe("Complicated help");
            @switch2.IsRequired.ShouldBeTrue();
            @switch2.ArgumentsRequired.ShouldBeFalse();
            @switch2.ConflictsWith.ShouldBeEmpty();
            @switch2.OptionalizeLastRequiredArguments.ShouldBe(0);
            @switch2.AcceptsValues.ShouldBeFalse();
        }

    }
}
