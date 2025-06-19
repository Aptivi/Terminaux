//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
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
using Terminaux.Shell.Shells;
using Terminaux.Tests.Shared.Shells;

namespace Terminaux.Tests.Shell.ShellBase.Scripting
{

    [TestClass]
    public class MESHVariableTests
    {
        [ClassInitialize]
        public static void InitializeTests(TestContext testContext)
        {
            ShellManager.RegisterShell("TestShell", new TestShellInfo());
            testContext.WriteLine("Created new test shell " + testContext.TestName);
        }

        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void CleanTests(TestContext testContext)
        {
            ShellManager.RegisterShell("TestShell", new TestShellInfo());
            testContext.WriteLine("Removed test shell " + testContext.TestName);
        }

        /// <summary>
        /// Tests sanitizing variable name
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestSanitizeVariableNamesWithDollarSign()
        {
            string expected = "$my_var";
            string sanitized = MESHVariables.SanitizeVariableName("$my_var");
            sanitized.ShouldBe(expected);
        }

        /// <summary>
        /// Tests sanitizing variable name
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestSanitizeVariableNamesWithoutDollarSign()
        {
            string expected = "$my_var";
            string sanitized = MESHVariables.SanitizeVariableName("my_var");
            sanitized.ShouldBe(expected);
        }

        /// <summary>
        /// Tests initializing, setting, and getting $variable
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestVariables()
        {
            MESHVariables.InitializeVariable("$test_var");
            MESHVariables.Variables.ShouldNotBeEmpty();
            MESHVariables.SetVariable("$test_var", "test").ShouldBeTrue();
            MESHVariables.GetVariable("$test_var").ShouldBe("test");
            MESHVariables.SetVariables("$test_var_arr", ["Nitrocid", "KS"]).ShouldBeTrue();
            MESHVariables.GetVariable("$test_var_arr[0]").ShouldBe("Nitrocid");
            MESHVariables.GetVariable("$test_var_arr[1]").ShouldBe("KS");
            string ExpectedCommand = "write test";
            string ActualCommand = MESHVariables.GetVariableCommand("$test_var", "write $test_var", "TestShell");
            ActualCommand.ShouldBe(ExpectedCommand);
            string ExpectedCommand2 = "write $notset";
            string ActualCommand2 = MESHVariables.GetVariableCommand("notset", "write $notset", "TestShell");
            ActualCommand2.ShouldBe(ExpectedCommand2);
        }

        /// <summary>
        /// Tests converting the environment variables to MESH's declaration
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestConvertEnvVariables()
        {
            MESHVariables.ConvertSystemEnvironmentVariables();
            MESHVariables.Variables.ShouldNotBeNull();
            MESHVariables.Variables.ShouldNotBeEmpty();
            MESHVariables.Variables.Count.ShouldBeGreaterThan(1);
        }

        /// <summary>
        /// Tests initializing the variables from...
        /// </summary>
        [TestMethod]
        [DataRow("", "", "")]
        [DataRow("$test=bar", "$test", "bar")]
        [DataRow("$test2=bars $test3=\"Nitrocid KS\"", "$test2|$test3", "bars|Nitrocid KS")]
        [Description("Action")]
        public void TestInitializeVariablesFrom(string expression, string expectedKeys, string expectedValues)
        {
            MESHVariables.InitializeVariablesFrom(expression);
            string[] expectedKeysArray = string.IsNullOrEmpty(expectedKeys) ? [] : expectedKeys.Split('|');
            string[] expectedValuesArray = string.IsNullOrEmpty(expectedValues) ? [] : expectedValues.Split('|');
            for (int i = 0; i < expectedKeysArray.Length; i++)
            {
                string key = expectedKeysArray[i];
                string value = expectedValuesArray[i];
                MESHVariables.Variables.ShouldContainKeyAndValue(key, value);
            }
        }

        /// <summary>
        /// Tests getting the variables from...
        /// </summary>
        [TestMethod]
        [DataRow("", "", "")]
        [DataRow("$test=bar", "$test", "bar")]
        [DataRow("$test2=bars $test3=\"Nitrocid KS\"", "$test2|$test3", "bars|\"Nitrocid KS\"")]
        [Description("Action")]
        public void TestGetVariablesFrom(string expression, string expectedKeys, string expectedValues)
        {
            var (varStoreKeys, varStoreValues) = MESHVariables.GetVariablesFrom(expression);
            string[] expectedKeysArray = string.IsNullOrEmpty(expectedKeys) ? [] : expectedKeys.Split('|');
            string[] expectedValuesArray = string.IsNullOrEmpty(expectedValues) ? [] : expectedValues.Split('|');
            string[] actualKeysArray = varStoreKeys;
            string[] actualValuesArray = varStoreValues;
            actualKeysArray.ShouldBe(expectedKeysArray);
            actualValuesArray.ShouldBe(expectedValuesArray);
        }

    }
}
