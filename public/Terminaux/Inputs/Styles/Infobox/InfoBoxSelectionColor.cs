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
    public static class InfoBoxSelectionColor
    {
        private static readonly Keybinding[] keybindings =
        [
            new Keybinding("Goes one choice up (next choice)", ConsoleKey.UpArrow),
            new Keybinding("Goes one choice down (previous choice)", ConsoleKey.DownArrow),
            new Keybinding("Goes to the first choice", ConsoleKey.Home),
            new Keybinding("Goes to the last choice", ConsoleKey.End),
            new Keybinding("Goes to the previous page of choices", ConsoleKey.PageUp),
            new Keybinding("Goes to the next page of choices", ConsoleKey.PageDown),
            new Keybinding("Shows more info in an informational box", ConsoleKey.Tab),
            new Keybinding("Searches for a choice", ConsoleKey.F),
            new Keybinding("Goes one line up", ConsoleKey.W),
            new Keybinding("Goes one line down", ConsoleKey.S),
            new Keybinding("Goes to the previous page of text", ConsoleKey.E),
            new Keybinding("Goes to the next page of text", ConsoleKey.D),
            new Keybinding("Submits the value", ConsoleKey.Enter),
            new Keybinding("Changes the selected value (radio buttons)", ConsoleKey.Spacebar),
            new Keybinding("Closes without submitting the value", ConsoleKey.Escape),
            new Keybinding("Performs an action or selects a choice", PointerButton.Left),
            new Keybinding("Shows more info in an infobox", PointerButton.Right),
            new Keybinding("Previous choice or previous three lines of text", PointerButton.WheelUp),
            new Keybinding("Next choice or next three lines of text", PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelection(selections, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, InfoBoxSettings settings, params object[] vars)
        {
            var category = new InputChoiceCategoryInfo[]
            {
                new("Selection infobox", [new("Available options", selections)])
            };
            return WriteInfoBoxSelectionInternal(
                settings.Title, category, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, settings.RadioButtons, vars);
        }

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelection(selections, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceCategoryInfo[] selections, string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionInternal(settings.Title, selections, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, settings.RadioButtons, vars);
        
        #region To be removed
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionPlain("", selections, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColor(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="BackgroundColor">InfoBoxSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="BackgroundColor">InfoBoxSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                "", selections, text, settings, InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(title, selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                title, selections, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelection(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColor(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelection(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars)
        {
            var category = new InputChoiceCategoryInfo[]
            {
                new("Selection infobox", [new("Available options", selections)])
            };
            return WriteInfoBoxSelectionInternal(
                title, category, text, settings, InfoBoxTitledSelectionColor, BackgroundColor, true, false, vars);
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionPlain("", selections, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColor(InputChoiceCategoryInfo[] selections, string text, Color InfoBoxSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="BackgroundColor">InfoBoxSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(InputChoiceCategoryInfo[] selections, string text, Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelection(InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="BackgroundColor">InfoBoxSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                "", selections, text, settings, InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(title, selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionInternal(
                title, selections, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelection(string title, InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColor(string title, InputChoiceCategoryInfo[] selections, string text, Color InfoBoxTitledSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceCategoryInfo[] selections, string text, Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelection(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionInternal(
                title, selections, text, settings, InfoBoxTitledSelectionColor, BackgroundColor, true, false, vars);
        #endregion

        internal static int WriteInfoBoxSelectionInternal(string title, InputChoiceCategoryInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionColor, Color BackgroundColor, bool useColor, bool radioButtons, params object[] vars)
        {
            int selectedChoice = -1;
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(selections)];

            // First, verify that we have selections
            if (choices is null || choices.Length == 0)
                return selectedChoice;

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
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSelectionColor), infoBoxScreenPart);
            try
            {
                // Modify the current selection according to the default
                int currentSelection = choices.Any((ici) => ici.ChoiceDefault) ? choices.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
                int currentSelected = choices.Any((ici) => ici.ChoiceDefault) ? choices.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
                var selectionsRendered = new Selections(selections)
                {
                    ShowRadioButtons = radioButtons,
                    CurrentSelection = currentSelection,
                    SelectedChoice = currentSelected,
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
                        InfoBoxTools.RenderText(selections, title, text, settings, InfoBoxTitledSelectionColor, BackgroundColor, useColor, ref increment, currIdx, true, vars)
                    );

                    // Buffer the selection box
                    var border = new Border()
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY - 1,
                        Width = maxSelectionWidth,
                        Height = selectionChoices,
                        Settings = settings,
                        Color = InfoBoxTitledSelectionColor,
                    };
                    boxBuffer.Append(border.Render());

                    // Now, render the selections
                    var selectionsRendered = new Selections(selections)
                    {
                        ShowRadioButtons = radioButtons,
                        Left = selectionBoxPosX + 1,
                        Top = selectionBoxPosY,
                        CurrentSelection = currentSelection,
                        SelectedChoice = currentSelected,
                        Height = selectionChoices,
                        Width = maxSelectionWidth,
                        SwapSelectedColors = true,
                        Settings = new()
                        {
                            OptionColor = InfoBoxTitledSelectionColor,
                            SelectedOptionColor = InfoBoxTitledSelectionColor,
                            BackgroundColor = BackgroundColor,
                        }
                    };
                    boxBuffer.Append(
                        selectionsRendered.Render()
                    );

                    // Return the buffer
                    return boxBuffer.ToString();
                });

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
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
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
                                else if (radioButtons)
                                {
                                    if (!InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection))
                                        break;
                                    if (hitboxType != ChoiceHitboxType.Choice)
                                        break;
                                    currentSelected = currentSelection;
                                }
                                else
                                    bail = InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection) && hitboxType == ChoiceHitboxType.Choice;
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
                                        Title = $"[{choiceName}] {choiceTitle}"
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (selectionChoices <= 0)
                                    break;
                                var entriesString = choices.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput("Write a search term (supports regular expressions)");
                                if (!RegexTools.IsValidRegex(keyword))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal("Your query is not a valid regular expression.");
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
                                    int answer = WriteInfoBoxSelection(resultChoices, "Select one of the entries:");
                                    if (answer < 0)
                                        break;
                                    currentSelection = resultEntries[answer].idx;
                                }
                                else if (resultEntries.Length == 1)
                                    currentSelection = resultEntries[0].idx;
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal("No item found.");
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
                            case ConsoleKey.Spacebar:
                                if (radioButtons)
                                    currentSelected = currentSelection;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                            case ConsoleKey.K:
                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(keybindings);
                                break;

                        }
                    }

                    // Verify that the current position is not a disabled choice
                    InfoBoxTools.VerifyDisabled(ref currentSelection, choices, goingUp);
                }
                if (!cancel)
                {
                    if (radioButtons)
                        selectedChoice = currentSelected;
                    else
                        selectedChoice = currentSelection;
                }
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

            // Return the selected choice, or -1
            return selectedChoice;
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

        static InfoBoxSelectionColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
