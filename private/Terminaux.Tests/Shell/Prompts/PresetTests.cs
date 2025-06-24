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

using Terminaux.Shell.Prompts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Terminaux.Tests.Shell.Prompts
{
    [TestClass]
    public class PresetTests
    {
        /// <summary>
        /// Tests setting preset
        /// </summary>
        [TestMethod]
        [DataRow("TestPreset", "TestShell", "TestPreset")]
        [Description("Action")]
        public void TestSetPresetDry(string presetName, string type, string expected)
        {
            PromptPresetManager.SetPreset(presetName, type);
            string baseName = PromptPresetManager.GetCurrentPresetBaseFromShell(type).PresetName;
            baseName.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting preset list from shell
        /// </summary>
        [TestMethod]
        [DataRow("TestShell")]
        [Description("Action")]
        public void TestGetPresetsFromShell(string type)
        {
            var presets = PromptPresetManager.GetPresetsFromShell(type);
            presets.ShouldNotBeNull();
            presets.ShouldNotBeEmpty();
            presets.ShouldContainKey("TestPreset");
        }
    }
}
