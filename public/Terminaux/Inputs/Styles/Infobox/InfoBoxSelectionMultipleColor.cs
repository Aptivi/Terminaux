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

using System;
using System.Collections.Generic;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors;
using System.Text;
using System.Linq;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Textify.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Selections = Terminaux.Writer.CyclicWriters.Graphical.Selection;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Structures;
using Terminaux.Inputs.Styles.Selection;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with selection and color support
    /// </summary>
    public static class InfoBoxSelectionMultipleColor
    {
        private static Keybinding[] Keybindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_GOUP"), ConsoleKey.UpArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_GODOWN"), ConsoleKey.DownArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_FIRST"), ConsoleKey.Home),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_LAST"), ConsoleKey.End),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_PREVPAGECHOICES"), ConsoleKey.PageUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_NEXTPAGECHOICES"), ConsoleKey.PageDown),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), ConsoleKey.Tab),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_SEARCHCHOICES"), ConsoleKey.F),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEUP"), ConsoleKey.W),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEDOWN"), ConsoleKey.S),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVPAGETEXT"), ConsoleKey.E),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTPAGETEXT"), ConsoleKey.D),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_MULTIPLE_KEYBINDING_SELECTALLCHOICES"), ConsoleKey.A),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SELECTONECHOICE"), ConsoleKey.Spacebar),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_SUBMIT_PLURAL"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_CANCEL_PLURAL"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PERFORMORSELECT"), PointerButton.Left),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), PointerButton.Right),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELUPCHOICE"), PointerButton.WheelUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELDOWNCHOICE"), PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the multiple-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiple(selections, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the multiple-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceInfo[] selections, string text, InfoBoxSettings settings, params object[] vars)
        {
            var category = new InputChoiceCategoryInfo[]
            {
                new("Selection infobox", [new("Available options", selections)])
            };
            return WriteInfoBoxSelectionMultipleInternal(
                settings.Title, category, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, vars);
        }

        /// <summary>
        /// Writes the multiple-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiple(selections, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the multiple-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceCategoryInfo[] selections, string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleInternal(settings.Title, selections, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, vars);

        // TODO: Remove in the final release
        #region To be removed
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain(selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain("", selections, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColor(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionMultipleColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionMultipleColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(
                "", selections, text, settings, InfoBoxSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain(title, selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(
                title, selections, text, settings, ColorTools.CurrentForegroundColor, ColorTools.CurrentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiple(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColor(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionMultipleColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionMultipleColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiple(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, params object[] vars)
        {
            var category = new InputChoiceCategoryInfo[]
            {
                new("Selection infobox", [new("Available options", selections)])
            };
            return WriteInfoBoxSelectionMultipleInternal(
                title, category, text, settings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, true, vars);
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain(selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain("", selections, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColor(InputChoiceCategoryInfo[] selections, string text, Color InfoBoxSelectionMultipleColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionMultipleColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(InputChoiceCategoryInfo[] selections, string text, Color InfoBoxSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, Color InfoBoxSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(
                "", selections, text, settings, InfoBoxSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(string title, InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain(title, selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiplePlain(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleInternal(
                title, selections, text, settings, ColorTools.CurrentForegroundColor, ColorTools.CurrentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiple(string title, InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColor(string title, InputChoiceCategoryInfo[] selections, string text, Color InfoBoxTitledSelectionMultipleColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionMultipleColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(string title, InputChoiceCategoryInfo[] selections, string text, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultiple(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choice categories</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int[] WriteInfoBoxSelectionMultipleColorBack(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleInternal(
                title, selections, text, settings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, true, vars);
        #endregion

        internal static int[] WriteInfoBoxSelectionMultipleInternal(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            List<int> selectedChoices = [];
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(selections)];

            // Make selected choices from the ChoiceDefaultSelected value.
            selectedChoices = choices.Any((ici) => ici.ChoiceDefaultSelected) ? choices.Select((ici, idx) => (idx, ici.ChoiceDefaultSelected)).Where((tuple) => tuple.ChoiceDefaultSelected).Select((tuple) => tuple.idx).ToList() : [];

            // Verify that we have selections
            if (choices is null || choices.Length == 0)
                return [.. selectedChoices];

            // We need not to run the selection style when everything is disabled
            bool allDisabled = choices.All((ici) => ici.ChoiceDisabled);
            if (allDisabled)
                throw new TerminauxException("The infobox selection style requires that there is at least one choice enabled.");

            // Now, some logic to get the informational box ready
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSelectionMultipleColor), infoBoxScreenPart);
            try
            {
                // Modify the current selection according to the default
                int currentSelection = choices.Any((ici) => ici.ChoiceDefault) ? choices.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
                var selectionsRendered = new Selections(selections)
                {
                    CurrentSelection = currentSelection,
                    Width = 42,
                };
                var related = selectionsRendered.GetRelatedHeights();
                int selectionChoices = related.Count > 10 ? 10 : related.Count;

                // Edge case: We need to check to see if the current highlight is disabled
                InfoBoxTools.VerifyDisabled(ref currentSelection, choices);

                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY, selectionBoxPosX, selectionBoxPosY, _, maxSelectionWidth, _, _) = InfoBoxTools.GetDimensions(selections, splitFinalLines);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(selections, title, text, settings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, useColor, ref increment, currIdx, true, vars)
                    );

                    // Buffer the selection box
                    var border = new Border()
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY - 1,
                        Width = maxSelectionWidth,
                        Height = selectionChoices,
                        Settings = settings,
                        UseColors = useColor,
                        Color = InfoBoxTitledSelectionMultipleColor,
                        BackgroundColor = BackgroundColor,
                    };
                    boxBuffer.Append(border.Render());

                    // Now, render the selections
                    var selectionsRendered = new Selections(selections)
                    {
                        Left = selectionBoxPosX + 1,
                        Top = selectionBoxPosY,
                        CurrentSelection = currentSelection,
                        CurrentSelections = [.. selectedChoices],
                        Height = selectionChoices,
                        Width = maxSelectionWidth,
                        SwapSelectedColors = true,
                        UseColors = useColor,
                        Settings = new()
                        {
                            OptionColor = InfoBoxTitledSelectionMultipleColor,
                            SelectedOptionColor = InfoBoxTitledSelectionMultipleColor,
                            BackgroundColor = BackgroundColor,
                        }
                    };
                    boxBuffer.Append(
                        selectionsRendered.Render()
                    );

                    // Return the buffer
                    return boxBuffer.ToString();
                });

                // Query the enabled answers
                var enabledAnswers = choices.Select((ici, idx) => (ici, idx)).Where((ici) => !ici.ici.ChoiceDisabled).Select((tuple) => tuple.idx).ToArray();

                // Wait for input
                bool bail = false;
                bool cancel = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Handle keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    bool goingUp = false;
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY, _, selectionBoxPosY, leftPos, maxSelectionWidth, arrowSelectLeft, selectionReservedHeight) = InfoBoxTools.GetDimensions(selections, splitFinalLines);
                    maxHeight -= selectionReservedHeight;

                    // Get positions for arrows
                    int arrowLeft = maxWidth + borderX + 1;
                    int arrowTop = 2;
                    int arrowBottom = maxHeight + 1;

                    // Get positions for infobox buttons
                    string infoboxButtons = InfoBoxTools.GetButtons(settings);
                    int infoboxButtonsWidth = ConsoleChar.EstimateCellWidth(infoboxButtons);
                    int infoboxButtonLeftHelpMin = maxWidth + borderX - infoboxButtonsWidth;
                    int infoboxButtonLeftHelpMax = infoboxButtonLeftHelpMin + 2;
                    int infoboxButtonLeftCloseMin = infoboxButtonLeftHelpMin + 3;
                    int infoboxButtonLeftCloseMax = infoboxButtonLeftHelpMin + infoboxButtonsWidth;
                    int infoboxButtonsTop = borderY;

                    // Make hitboxes for arrow and button presses
                    var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), new Action<PointerEventContext>((_) => GoUp(ref currIdx))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), new Action<PointerEventContext>((_) => GoDown(ref currIdx, text, vars, selections))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSelectUpHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY), new Action<PointerEventContext>((_) => SelectionGoUp(ref currentSelection, choices))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSelectDownHitbox = new PointerHitbox(new(arrowSelectLeft, ConsoleWrapper.WindowHeight - selectionChoices), new Action<PointerEventContext>((_) => SelectionGoDown(ref currentSelection, choices))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(Keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                    // Handle input
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Mouse input received.
                        ChoiceHitboxType hitboxType = ChoiceHitboxType.Choice;
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (IsMouseWithinText(text, vars, selections, mouse))
                                    GoUp(ref currIdx, 3);
                                else if (IsMouseWithinInputBox(text, vars, selections, mouse))
                                {
                                    goingUp = true;
                                    SelectionGoUp(ref currentSelection, choices);
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (IsMouseWithinText(text, vars, selections, mouse))
                                    GoDown(ref currIdx, text, vars, selections, 3);
                                else if (IsMouseWithinInputBox(text, vars, selections, mouse))
                                    SelectionGoDown(ref currentSelection, choices);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && splitFinalLines.Length > maxHeight)
                                {
                                    arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowSelectUpHitbox.IsPointerWithin(mouse) || arrowSelectDownHitbox.IsPointerWithin(mouse)) && related.Count > selectionChoices)
                                {
                                    arrowSelectUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowSelectDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if (infoboxButtonHelpHitbox.IsPointerWithin(mouse))
                                    infoboxButtonHelpHitbox.ProcessPointer(mouse, out _);
                                else if (infoboxButtonCloseHitbox.IsPointerWithin(mouse))
                                    infoboxButtonCloseHitbox.ProcessPointer(mouse, out _);
                                else
                                {
                                    int oldIndex = currentSelection;
                                    if (InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection, false))
                                    {
                                        switch (hitboxType)
                                        {
                                            case ChoiceHitboxType.Category:
                                                InfoBoxTools.ProcessSelectionRequest(2, currentSelection + 1, selections, ref selectedChoices);
                                                currentSelection = oldIndex;
                                                break;
                                            case ChoiceHitboxType.Group:
                                                InfoBoxTools.ProcessSelectionRequest(1, currentSelection + 1, selections, ref selectedChoices);
                                                currentSelection = oldIndex;
                                                break;
                                            case ChoiceHitboxType.Choice:
                                                InfoBoxTools.VerifyDisabled(ref currentSelection, choices, goingUp);
                                                if (!selectedChoices.Remove(currentSelection))
                                                    selectedChoices.Add(currentSelection);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (!InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection))
                                    break;
                                if (hitboxType != ChoiceHitboxType.Choice)
                                    break;
                                var selectedInstance = choices[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(choiceDesc, new InfoBoxSettings()
                                    {
                                        Title = $"[{choiceName}] {choiceTitle}",
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection);
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Spacebar:
                                if (!selectedChoices.Remove(currentSelection))
                                    selectedChoices.Add(currentSelection);
                                break;
                            case ConsoleKey.UpArrow:
                                goingUp = true;
                                SelectionGoUp(ref currentSelection, choices);
                                break;
                            case ConsoleKey.DownArrow:
                                SelectionGoDown(ref currentSelection, choices);
                                break;
                            case ConsoleKey.Home:
                                goingUp = true;
                                SelectionSet(ref currentSelection, choices, 0);
                                break;
                            case ConsoleKey.End:
                                SelectionSet(ref currentSelection, choices, choices.Length - 1);
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                {
                                    int currentPageMove = (currentSelection - 1) / selectionChoices;
                                    int startIndexMove = selectionChoices * currentPageMove;
                                    SelectionSet(ref currentSelection, choices, startIndexMove);
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    int currentPageMove = currentSelection / selectionChoices;
                                    int startIndexMove = selectionChoices * (currentPageMove + 1);
                                    SelectionSet(ref currentSelection, choices, startIndexMove);
                                }
                                break;
                            case ConsoleKey.Tab:
                                var selectedInstance = choices[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(choiceDesc, new InfoBoxSettings()
                                    {
                                        Title = $"[{choiceName}] {choiceTitle}",
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.A:
                                bool unselect = selectedChoices.Count == enabledAnswers.Count();
                                if (unselect)
                                    selectedChoices.Clear();
                                else if (selectedChoices.Count == 0)
                                    selectedChoices.AddRange(enabledAnswers);
                                else
                                {
                                    // We need to use Except here to avoid wasting CPU cycles, since we could be dealing with huge data.
                                    var unselected = enabledAnswers.Except(selectedChoices);
                                    selectedChoices.AddRange(unselected);
                                }
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (selectionChoices <= 0)
                                    break;
                                var entriesString = choices.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_COMMON_SEARCHPROMPT"));
                                if (!RegexTools.IsValidRegex(keyword))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_INVALIDQUERY"));
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                    break;
                                }
                                var regex = new Regex(keyword);
                                var resultEntries = entriesString
                                    .Select((entry, idx) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled, idx))
                                    .Where((entry) => (regex.IsMatch(entry.ChoiceName) || regex.IsMatch(entry.ChoiceTitle)) && !entry.ChoiceDisabled).ToArray();
                                if (resultEntries.Length > 1)
                                {
                                    var resultChoices = resultEntries.Select((tuple) => new InputChoiceInfo(tuple.ChoiceName, tuple.ChoiceTitle)).ToArray();
                                    int answer = InfoBoxSelectionColor.WriteInfoBoxSelection(resultChoices, LanguageTools.GetLocalized("T_INPUT_COMMON_ENTRYPROMPT"));
                                    if (answer < 0)
                                        break;
                                    currentSelection = resultEntries[answer].idx;
                                }
                                else if (resultEntries.Length == 1)
                                    currentSelection = resultEntries[0].idx;
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_NOITEMS"));
                                ScreenTools.CurrentScreen?.RequireRefresh();
                                break;
                            case ConsoleKey.E:
                                GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.D:
                                GoDown(ref currIdx, text, vars, selections, increment);
                                break;
                            case ConsoleKey.W:
                                GoUp(ref currIdx);
                                break;
                            case ConsoleKey.S:
                                GoDown(ref currIdx, text, vars, selections);
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                            case ConsoleKey.K:
                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(Keybindings);
                                break;

                        }
                    }

                    // Verify that the current position is not a disabled choice
                    InfoBoxTools.VerifyDisabled(ref currentSelection, choices, goingUp);
                }
                if (cancel)
                    selectedChoices.Clear();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                if (useColor)
                {
                    TextWriterRaw.WriteRaw(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }

            // Return the selected choices
            selectedChoices.Sort();
            return [.. selectedChoices];
        }

        private static bool IsMouseWithinText(string text, object[] vars, InputChoiceCategoryInfo[] choices, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY, _, _, _, _, _, reservedHeight) = InfoBoxTools.GetDimensions(choices, splitFinalLines);
            maxHeight -= reservedHeight;

            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (borderX + 1, borderY + 1), (borderX + maxWidth, borderY + maxHeight));
        }

        private static bool IsMouseWithinInputBox(string text, object[] vars, InputChoiceCategoryInfo[] choices, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (_, _, _, _, _, selectionBoxPosX, selectionBoxPosY, _, maxSelectionWidth, _, reservedHeight) = InfoBoxTools.GetDimensions(choices, splitFinalLines);

            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (selectionBoxPosX + 1, selectionBoxPosY), (selectionBoxPosX + maxSelectionWidth, selectionBoxPosY + reservedHeight - 3));
        }

        private static void GoUp(ref int currIdx, int level = 1)
        {
            currIdx -= level;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void GoDown(ref int currIdx, string text, object[] vars, InputChoiceCategoryInfo[] choices, int level = 1)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (_, maxHeight, _, _, _, _, _, _, _, _, reservedHeight) = InfoBoxTools.GetDimensions(choices, splitFinalLines);
            maxHeight -= reservedHeight;
            currIdx += level;
            if (currIdx > splitFinalLines.Length - maxHeight)
                currIdx = splitFinalLines.Length - maxHeight;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void SelectionGoUp(ref int currentSelection, InputChoiceInfo[] selections)
        {
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = selections.Length - 1;
        }

        private static void SelectionGoDown(ref int currentSelection, InputChoiceInfo[] selections)
        {
            currentSelection++;
            if (currentSelection > selections.Length - 1)
                currentSelection = 0;
        }

        private static void SelectionSet(ref int currentSelection, InputChoiceInfo[] selections, int value)
        {
            currentSelection = value;
            if (currentSelection > selections.Length - 1)
                currentSelection = selections.Length - 1;
            if (currentSelection < 0)
                currentSelection = 0;
        }

        static InfoBoxSelectionMultipleColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
