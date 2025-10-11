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

using Terminaux.Shell.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;
using Terminaux.Base;
using Terminaux.Shell.Shells;
using Terminaux.Tests.Shared.Shells;

namespace Terminaux.Tests.Shell.ShellBase.Scripting
{

    [TestClass]
    public class MESHScriptingTests
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
        /// Tests linting a valid MESH script
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestLintMESHScriptValid() =>
            Should.NotThrow(() => MESHParse.Execute(Path.GetFullPath("TestData/ScriptValid.mesh"), "", "TestShell", true));

        /// <summary>
        /// Tests linting a invalid MESH script
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestLintMESHScriptInvalid() =>
            Should.Throw(() => MESHParse.Execute(Path.GetFullPath("TestData/ScriptInvalid.mesh"), "", "TestShell", true), typeof(TerminauxException));

        /// <summary>
        /// Tests linting an empty MESH script
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestLintMESHScriptEmpty() =>
            Should.NotThrow(() => MESHParse.Execute(Path.GetFullPath("TestData/ScriptEmpty.mesh"), "", "TestShell", true));
    }
}
