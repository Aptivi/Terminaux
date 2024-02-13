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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Inputs;

namespace Terminaux.Tests.Inputs
{
    [TestClass]
    public class InputChoiceToolsTests
    {
        [TestMethod]
        public void TestGetInputChoicesAnswersStr()
        {
            var choices = InputChoiceTools.GetInputChoices("Y/N/C", ["Yes", "No", "Cancel"]);
            choices.Length.ShouldBe(3);
            choices[0].ChoiceName.ShouldBe("Y");
            choices[1].ChoiceName.ShouldBe("N");
            choices[2].ChoiceName.ShouldBe("C");
            choices[0].ChoiceTitle.ShouldBe("Yes");
            choices[1].ChoiceTitle.ShouldBe("No");
            choices[2].ChoiceTitle.ShouldBe("Cancel");
        }

        [TestMethod]
        public void TestGetInputChoicesAnswersArray()
        {
            var choices = InputChoiceTools.GetInputChoices(["Y", "N", "C"], ["Yes", "No", "Cancel"]);
            choices.Length.ShouldBe(3);
            choices[0].ChoiceName.ShouldBe("Y");
            choices[1].ChoiceName.ShouldBe("N");
            choices[2].ChoiceName.ShouldBe("C");
            choices[0].ChoiceTitle.ShouldBe("Yes");
            choices[1].ChoiceTitle.ShouldBe("No");
            choices[2].ChoiceTitle.ShouldBe("Cancel");
        }

        [TestMethod]
        public void TestGetInputChoicesAnswersTitleNoMatch()
        {
            var choices = InputChoiceTools.GetInputChoices(["Y", "N", "C"], ["Yes", "No"]);
            choices.Length.ShouldBe(3);
            choices[0].ChoiceName.ShouldBe("Y");
            choices[1].ChoiceName.ShouldBe("N");
            choices[2].ChoiceName.ShouldBe("C");
            choices[0].ChoiceTitle.ShouldBe("Yes");
            choices[1].ChoiceTitle.ShouldBe("No");
            choices[2].ChoiceTitle.ShouldBe("[3]");
        }

        [TestMethod]
        public void TestGetInputChoicesAnswersAnswerNoMatch()
        {
            var choices = InputChoiceTools.GetInputChoices(["Y", "N"], ["Yes", "No", "Cancel"]);
            choices.Length.ShouldBe(3);
            choices[0].ChoiceName.ShouldBe("Y");
            choices[1].ChoiceName.ShouldBe("N");
            choices[2].ChoiceName.ShouldBe("[3]");
            choices[0].ChoiceTitle.ShouldBe("Yes");
            choices[1].ChoiceTitle.ShouldBe("No");
            choices[2].ChoiceTitle.ShouldBe("Cancel");
        }
    }
}
