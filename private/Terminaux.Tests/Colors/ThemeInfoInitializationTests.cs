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

using System.IO;
using Terminaux.Colors.Themes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using Terminaux.Colors.Themes.Colors;

namespace Terminaux.Tests.Colors
{

    [TestClass]
    public class ThemeInfoInitializationTests
    {

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from resources
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromResources()
        {
            // Create instance
            var ThemeInfoInstance = new ThemeInfo();

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.themeColors[type].ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from all resources
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGetThemeInfoFromAllResources()
        {
            var installedThemes = ThemeTools.GetInstalledThemes();
            foreach (string themeName in ThemeTools.GetInstalledThemes().Keys)
            {
                // Create instance
                var ThemeInfoInstance = installedThemes[themeName];

                // Check for null
                ThemeInfoInstance.themeColors.ShouldNotBeNull();
                for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
                {
                    string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                    ThemeInfoInstance.themeColors[type].ShouldNotBeNull();
                }
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from file
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromFile()
        {
            // Create instance
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            var ThemeInfoStream = new StreamReader(SourcePath);
            var ThemeInfoInstance = new ThemeInfo(ThemeInfoStream);
            ThemeInfoStream.Close();

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.themeColors[type].ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from file
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromFilePath()
        {
            // Create instance
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            var ThemeInfoInstance = new ThemeInfo(SourcePath);

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.themeColors[type].ShouldNotBeNull();
            }
        }

    }
}
