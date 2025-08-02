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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Shells;
using Shouldly;
using Terminaux.Tests.Shared.Shells;

namespace Terminaux.Tests.Shell.ShellBase.Aliases
{

    [TestClass]
    public class AliasTests
    {
        [ClassInitialize]
        public static void InitializeTests(TestContext testContext)
        {
            ShellManager.RegisterShell("TestShell", new TestShellInfo());
            testContext.WriteLine("Created new test shell " + testContext.FullyQualifiedTestClassName);
        }

        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void CleanTests(TestContext testContext)
        {
            ShellManager.RegisterShell("TestShell", new TestShellInfo());
            testContext.WriteLine("Removed test shell " + testContext.FullyQualifiedTestClassName);
        }

        /// <summary>
        /// Tests adding alias
        /// </summary>
        [TestMethod]
        [DataRow("TestShell", "write", "w")]
        [Description("Action")]
        public void TestAddAlias(string type, string source, string target)
        {
            AliasManager.AddAlias(source, target, type).ShouldBeTrue();
            AliasManager.SaveAliases(type);
            AliasManager.DoesAliasExist(target, type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests adding alias
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestAddAliasForUnifiedCommand(string type)
        {
            AliasManager.AddAlias("presets", "p", type).ShouldBeTrue();
            AliasManager.SaveAliases(type);
            AliasManager.DoesAliasExist("p", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests removing alias
        /// </summary>
        [TestMethod]
        [DataRow("TestShell", "w")]
        [Description("Action")]
        public void TestRemoveAlias(string type, string target)
        {
            AliasManager.InitAliases(type);
            AliasManager.RemoveAlias(target, type).ShouldBeTrue();
            AliasManager.SaveAliases(type);
            AliasManager.DoesAliasExist(target, type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests removing alias
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestRemoveAliasForUnifiedCommand(string type)
        {
            AliasManager.InitAliases(type);
            AliasManager.RemoveAlias("p", type).ShouldBeTrue();
            AliasManager.SaveAliases(type);
            AliasManager.DoesAliasExist("p", type).ShouldBeFalse();
        }
    }
}
