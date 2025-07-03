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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Styles.Selection;
using Textify.General;
using Terminaux.Shell.Shells;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Prompts
{
    /// <summary>
    /// Prompt preset management module
    /// </summary>
    public static class PromptPresetManager
    {
        // Current presets
        internal static Dictionary<string, string> CurrentPresets = new()
        {
            { "Shell", "Default" },
        };

        /// <summary>
        /// Sets the shell preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="ThrowOnNotFound">If the preset is not found, throw an exception. Otherwise, use the default preset.</param>
        public static void SetPreset(string PresetName, string ShellType, bool ThrowOnNotFound = true)
        {
            var Presets = GetPresetsFromShell(ShellType);
            var CustomPresets = GetCustomPresetsFromShell(ShellType);

            // Check to see if we have the preset
            if (Presets.ContainsKey(PresetName) || CustomPresets.ContainsKey(PresetName))
                CurrentPresets[ShellType] = PresetName;
            else if (ThrowOnNotFound)
            {
                ConsoleLogger.Error("Preset {0} for {1} doesn't exist. Throwing...", PresetName, ShellType.ToString());
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_PROMPTS_PRESETS_EXCEPTION_NOTFOUND"), PresetName);
            }
            else
            {
                ConsoleLogger.Warning("Preset {0} for {1} doesn't exist. Setting dryly to default...", PresetName, ShellType.ToString());
                CurrentPresets[ShellType] = "Default";
            }
        }

        /// <summary>
        /// Gets the current preset base from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static PromptPresetBase GetCurrentPresetBaseFromShell(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).CurrentPreset;

        /// <summary>
        /// Gets the predefined presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetPresetsFromShell(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).ShellPresets;

        /// <summary>
        /// Gets the custom presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetCustomPresetsFromShell(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).CustomShellPresets;

        /// <summary>
        /// Gets all presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetAllPresetsFromShell(string ShellType)
        {
            var presets = new Dictionary<string, PromptPresetBase>();
            foreach (var preset in GetPresetsFromShell(ShellType))
                presets.Add(preset.Key, preset.Value);
            foreach (var preset in GetCustomPresetsFromShell(ShellType))
                presets.Add(preset.Key, preset.Value);
            return presets;
        }

        /// <summary>
        /// Writes the shell prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellPrompt(string ShellType)
        {
            var CurrentPresetBase = GetCurrentPresetBaseFromShell(ShellType);
            TextWriterRaw.WritePlain(CurrentPresetBase.PresetPrompt, false);
        }

        /// <summary>
        /// Writes the shell completion prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellCompletionPrompt(string ShellType)
        {
            var CurrentPresetBase = GetCurrentPresetBaseFromShell(ShellType);
            TextWriterRaw.WritePlain(CurrentPresetBase.PresetPromptCompletion, false);
        }

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        public static string PromptForPresets() =>
            PromptForPresets(ShellManager.CurrentShellType);

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        /// <param name="shellType">Sets preset in shell type</param>
        public static string PromptForPresets(string shellType)
        {
            var Presets = GetPresetsFromShell(shellType);

            // Add the custom presets to the local dictionary
            foreach (string PresetName in GetCustomPresetsFromShell(shellType).Keys)
                Presets.Add(PresetName, Presets[PresetName]);

            // Now, prompt the user
            var PresetNames = Presets.Select((kvp) => (kvp.Key, kvp.Value.PresetPromptShowcase)).ToArray();
            int SelectedPreset = SelectionStyle.PromptSelection(TextTools.FormatString(LanguageTools.GetLocalized("T_SHELL_PROMPTS_PRESETS_SELECTPRESET"), shellType), PresetNames);
            if (SelectedPreset == -1)
                return "Default";
            string SelectedPresetName = Presets.Keys.ElementAt(SelectedPreset - 1);
            SetPreset(SelectedPresetName, shellType);
            return SelectedPresetName;
        }

    }
}
