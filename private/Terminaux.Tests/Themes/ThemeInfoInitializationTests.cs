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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using Terminaux.Themes;
using Terminaux.Themes.Colors;
using Colorimetry.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Textify.General;

namespace Terminaux.Tests.Themes
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

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from resources and setting its colors
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromResourcesAndSetColors()
        {
            // Create instance
            var ThemeInfoInstance = new ThemeInfo();

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.SetColor(type, ConsoleColors.Aqua);
            }
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.themeColors[type].ShouldNotBeNull();
                ThemeInfoInstance.themeColors[type].ShouldBe(ConsoleColors.Aqua);
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from resources and setting its colors permanently by editing the theme
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromResourcesAndSetColorsPermanently()
        {
            // Create instance
            var ThemeInfoInstance = new ThemeInfo();

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.SetColor(type, ConsoleColors.Aqua);
                ThemeInfoInstance.themeColors[type].ShouldNotBeNull();
                ThemeInfoInstance.themeColors[type].ShouldBe(ConsoleColors.Aqua);
            }
            ThemeTools.EditTheme("Default", ThemeInfoInstance);
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeTools.GetThemeInfo("Default").GetColor(type).ShouldBe(ConsoleColors.Aqua);
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from resources and setting its colors permanently by editing the theme. Then, resets the theme
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromResourcesAndSetColorsPermanentlyWithReset()
        {
            // Create instance
            var ThemeInfoInstance = new ThemeInfo();

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.SetColor(type, ConsoleColors.MediumPurple);
                ThemeInfoInstance.themeColors[type].ShouldNotBeNull();
                ThemeInfoInstance.themeColors[type].ShouldBe(ConsoleColors.MediumPurple);
            }
            ThemeTools.EditTheme("Default", ThemeInfoInstance);
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeTools.GetThemeInfo("Default").GetColor(type).ShouldBe(ConsoleColors.MediumPurple);
            }
            ThemeTools.ResetTheme("Default");
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length - 1; typeIndex++)
            {
                string type = ThemeInfoInstance.themeColors.Keys.ElementAt(typeIndex);
                ThemeTools.GetThemeInfo("Default").GetColor(type).ShouldNotBe(ConsoleColors.MediumPurple);
            }
        }

        /// <summary>
        /// Tests exporting theme info to JSON object
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestExportThemeInfoJson()
        {
            // Create instance
            var ThemeInfoInstance = new ThemeInfo();

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();

            // Export to JSON
            JObject jsonObject = ThemeInfoInstance.ExportToJson();
            string jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented).UnixifyNewLines();

            // Verify that the JSON object contains all colors
            string jsonStringExpected =
            """
            {
              "Metadata": {
                "Name": "Default"
              },
              "InputColor": "15",
              "LicenseColor": "15",
              "BackgroundColor": "0",
              "NeutralTextColor": "7",
              "ListEntryColor": "3",
              "ListValueColor": "8",
              "StageColor": "10",
              "ErrorColor": "9",
              "WarningColor": "11",
              "OptionColor": "3",
              "BannerColor": "10",
              "QuestionColor": "11",
              "SuccessColor": "10",
              "UserDollarColor": "7",
              "TipColor": "11",
              "SeparatorTextColor": "15",
              "SeparatorColor": "7",
              "ListTitleColor": "15",
              "ProgressColor": "10",
              "BackOptionColor": "1",
              "TableSeparatorColor": "8",
              "TableHeaderColor": "15",
              "TableValueColor": "7",
              "SelectedOptionColor": "14",
              "AlternativeOptionColor": "11",
              "WeekendDayColor": "14",
              "EventDayColor": "14",
              "TableTitleColor": "14",
              "TodayDayColor": "10",
              "TuiBackgroundColor": "0",
              "TuiForegroundColor": "11",
              "TuiPaneBackgroundColor": "0",
              "TuiPaneSeparatorColor": "2",
              "TuiPaneSelectedSeparatorColor": "10",
              "TuiPaneSelectedItemForeColor": "0",
              "TuiPaneSelectedItemBackColor": "3",
              "TuiPaneItemForeColor": "3",
              "TuiPaneItemBackColor": "0",
              "TuiOptionBackgroundColor": "3",
              "TuiKeyBindingOptionColor": "0",
              "TuiOptionForegroundColor": "11",
              "TuiBoxBackgroundColor": "9",
              "TuiBoxForegroundColor": "15",
              "DisabledOptionColor": "8",
              "TuiKeyBindingBuiltinBackgroundColor": "3",
              "TuiKeyBindingBuiltinColor": "0",
              "TuiKeyBindingBuiltinForegroundColor": "10",
              "ProgressFailedColor": "9",
              "ProgressPausedColor": "11",
              "ProgressWarningColor": "11"
            }
            """.UnixifyNewLines();
            jsonString.ShouldBe(jsonStringExpected);
        }

        /// <summary>
        /// Tests exporting theme info to JSON object with custom colors
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestExportThemeInfoJsonWithCustomColors()
        {
            // Create instance
            var ThemeInfoInstance = new ThemeInfo();

            // Check for null
            ThemeInfoInstance.themeColors.ShouldNotBeNull();

            // Add custom colors
            ThemeInfoInstance.SetColor("Custom", new(ConsoleColors.Red));
            ThemeInfoInstance.SetColor("Custom2", new(ConsoleColors.Green));
            ThemeInfoInstance.SetColor("Custom3", new(ConsoleColors.Blue));

            // Export to JSON
            JObject jsonObject = ThemeInfoInstance.ExportToJson();
            string jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented).UnixifyNewLines();

            // Verify that the JSON object contains all colors
            string jsonStringExpected =
            """
            {
              "Metadata": {
                "Name": "Default"
              },
              "InputColor": "15",
              "LicenseColor": "15",
              "BackgroundColor": "0",
              "NeutralTextColor": "7",
              "ListEntryColor": "3",
              "ListValueColor": "8",
              "StageColor": "10",
              "ErrorColor": "9",
              "WarningColor": "11",
              "OptionColor": "3",
              "BannerColor": "10",
              "QuestionColor": "11",
              "SuccessColor": "10",
              "UserDollarColor": "7",
              "TipColor": "11",
              "SeparatorTextColor": "15",
              "SeparatorColor": "7",
              "ListTitleColor": "15",
              "ProgressColor": "10",
              "BackOptionColor": "1",
              "TableSeparatorColor": "8",
              "TableHeaderColor": "15",
              "TableValueColor": "7",
              "SelectedOptionColor": "14",
              "AlternativeOptionColor": "11",
              "WeekendDayColor": "14",
              "EventDayColor": "14",
              "TableTitleColor": "14",
              "TodayDayColor": "10",
              "TuiBackgroundColor": "0",
              "TuiForegroundColor": "11",
              "TuiPaneBackgroundColor": "0",
              "TuiPaneSeparatorColor": "2",
              "TuiPaneSelectedSeparatorColor": "10",
              "TuiPaneSelectedItemForeColor": "0",
              "TuiPaneSelectedItemBackColor": "3",
              "TuiPaneItemForeColor": "3",
              "TuiPaneItemBackColor": "0",
              "TuiOptionBackgroundColor": "3",
              "TuiKeyBindingOptionColor": "0",
              "TuiOptionForegroundColor": "11",
              "TuiBoxBackgroundColor": "9",
              "TuiBoxForegroundColor": "15",
              "DisabledOptionColor": "8",
              "TuiKeyBindingBuiltinBackgroundColor": "3",
              "TuiKeyBindingBuiltinColor": "0",
              "TuiKeyBindingBuiltinForegroundColor": "10",
              "ProgressFailedColor": "9",
              "ProgressPausedColor": "11",
              "ProgressWarningColor": "11",
              "Custom": "9",
              "Custom2": "2",
              "Custom3": "12"
            }
            """.UnixifyNewLines();
            jsonString.ShouldBe(jsonStringExpected);
        }
    }
}
