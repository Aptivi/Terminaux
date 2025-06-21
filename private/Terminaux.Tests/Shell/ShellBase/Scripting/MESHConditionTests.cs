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
using Terminaux.Shell.Scripting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Tests.Shell.ShellBase.Scripting.CustomConditions;
using Terminaux.Base;

namespace Terminaux.Tests.Shell.ShellBase.Scripting
{

    [TestClass]
    public class MESHConditionTests
    {

        /// <summary>
        /// Tests querying available conditions
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestQueryAvailableConditions()
        {
            MESHConditional.AvailableConditions.ShouldNotBeNull();
            MESHConditional.AvailableConditions.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests querying available conditions
        /// </summary>
        [TestMethod]
        [DataRow(false, "1 eq 1", "", "", "", "", true)]
        [DataRow(false, "1 eq 2", "", "", "", "", false)]
        [DataRow(true, "$firstVar eq 1", "firstVar", "1", "", "", true)]
        [DataRow(true, "$firstVar eq 2", "firstVar", "1", "", "", false)]
        [DataRow(true, "$firstVar eq $secondVar", "firstVar", "1", "secondVar", "1", true)]
        [DataRow(true, "$firstVar eq $secondVar", "firstVar", "1", "secondVar", "2", false)]
        [DataRow(false, "ter.txt isfname", "", "", "", "", true)]
        [DataRow(false, "?><\"\0.zfu isfname", "", "", "", "", false)]
        [DataRow(true, "$firstVar isfname", "firstVar", "ter.txt", "", "", true)]
        [DataRow(true, "$firstVar isfname", "firstVar", "?><\"\0.zfu", "", "", false)]
        [Description("Action")]
        public void TestConditionSatisfied(bool varMode, string condition, string variable, string variableValue, string variable2, string variableValue2, bool expected)
        {
            if (varMode)
            {
                if (string.IsNullOrEmpty(variable))
                    Assert.Fail("First variable must be filled in");

                MESHVariables.InitializeVariable(variable);
                MESHVariables.SetVariable(variable, variableValue).ShouldBeTrue();
                MESHVariables.InitializeVariable(variable2);
                MESHVariables.SetVariable(variable2, variableValue2).ShouldBeTrue();
            }
            bool actual = MESHConditional.ConditionSatisfied(condition);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests registering the condition and testing it
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestRegisterConditionAndTestSatisfaction()
        {
            Should.NotThrow(() => MESHConditional.RegisterCondition("haslen", new MyCondition()));
            Should.NotThrow(() => MESHConditional.ConditionSatisfied("Hello haslen")).ShouldBeTrue();
            Should.NotThrow(() => MESHConditional.ConditionSatisfied("\"\" haslen")).ShouldBeFalse();
        }

        /// <summary>
        /// Tests registering the condition and testing it
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestUnregisterConditionAndTestSatisfaction()
        {
            Should.NotThrow(() => MESHConditional.UnregisterCondition("haslen"));
            Should.Throw(() => MESHConditional.ConditionSatisfied("Hello haslen"), typeof(TerminauxException));
            Should.Throw(() => MESHConditional.ConditionSatisfied("\"\" haslen"), typeof(TerminauxException));
        }

    }
}
