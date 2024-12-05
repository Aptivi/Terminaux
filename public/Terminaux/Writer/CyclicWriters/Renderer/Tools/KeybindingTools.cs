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

using System;
using System.Linq;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Keybinding tools
    /// </summary>
    public static class KeybindingTools
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxPlain(Keybinding[] keybindings, params object[] vars) =>
            ShowKeybindingInfoboxPlain(keybindings, BorderSettings.GlobalSettings, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxPlain(Keybinding[] keybindings, BorderSettings settings, params object[] vars) =>
            ShowKeybindingInfoboxPlain("Available keybindings", keybindings, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfobox(Keybinding[] keybindings, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(keybindings, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColor(Keybinding[] keybindings, Color InfoBoxColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(keybindings, BorderSettings.GlobalSettings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColorBack(Keybinding[] keybindings, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(keybindings, BorderSettings.GlobalSettings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfobox(Keybinding[] keybindings, BorderSettings settings, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(keybindings, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColor(Keybinding[] keybindings, BorderSettings settings, Color InfoBoxColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(keybindings, settings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColorBack(Keybinding[] keybindings, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack("Available keybindings", keybindings, settings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxPlain(string title, Keybinding[] keybindings, params object[] vars) =>
            ShowKeybindingInfoboxPlain(title, keybindings, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="title">Title to be written</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxPlain(string title, Keybinding[] keybindings, BorderSettings settings, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(title, keybindings, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfobox(string title, Keybinding[] keybindings, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(title, keybindings, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColor(string title, Keybinding[] keybindings, Color InfoBoxTitledColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(title, keybindings, BorderSettings.GlobalSettings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColorBack(string title, Keybinding[] keybindings, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(title, keybindings, BorderSettings.GlobalSettings, InfoBoxTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfobox(string title, Keybinding[] keybindings, BorderSettings settings, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(title, keybindings, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColor(string title, Keybinding[] keybindings, BorderSettings settings, Color InfoBoxTitledColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(title, keybindings, settings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfoboxColorBack(string title, Keybinding[] keybindings, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            ShowKeybindingInfoboxColorBack(title, keybindings, settings, InfoBoxTitledColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        internal static void ShowKeybindingInfoboxColorBack(string title, Keybinding[] keybindings, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            string keybindingsText = RenderKeybindingHelpText(keybindings);
            InfoBoxModalColor.WriteInfoBoxModalColorBack(title, keybindingsText, settings, InfoBoxTitledColor, BackgroundColor, useColor, vars);
        }

        /// <summary>
        /// Renders the keybinding help text
        /// </summary>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <returns>Rendered keybindings help text that you can render for infoboxes.</returns>
        public static string RenderKeybindingHelpText(Keybinding[] keybindings)
        {
            // First, check the bindings length
            var nonMouseBindings = keybindings.Where((bind) => !bind.BindingUsesMouse).ToArray();
            var mouseBindings = keybindings.Where((bind) => bind.BindingUsesMouse).ToArray();
            if (keybindings is null || keybindings.Length == 0)
                return "No keybindings available";

            // User needs an infobox that shows all available keys
            int maxBindingLength = nonMouseBindings
                .Max((itb) => ConsoleChar.EstimateCellWidth(GetBindingKeyShortcut(itb)));
            string[] bindingRepresentations = nonMouseBindings
                .Select((itb) => $"{GetBindingKeyShortcut(itb) + new string(' ', maxBindingLength - ConsoleChar.EstimateCellWidth(GetBindingKeyShortcut(itb))) + $" | {itb.BindingName}"}")
                .ToArray();
            string[] bindingMouseRepresentations = [];
            if (mouseBindings is not null && mouseBindings.Length > 0)
            {
                int maxMouseBindingLength = mouseBindings
                    .Max((itb) => ConsoleChar.EstimateCellWidth(GetBindingMouseShortcut(itb)));
                bindingMouseRepresentations = mouseBindings
                    .Select((itb) => $"{GetBindingMouseShortcut(itb) + new string(' ', maxMouseBindingLength - ConsoleChar.EstimateCellWidth(GetBindingMouseShortcut(itb))) + $" | {itb.BindingName}"}")
                    .ToArray();
            }
            return
                $"{string.Join("\n", bindingRepresentations)}" +
                "\n\nMouse bindings:\n\n" +
                $"{(bindingMouseRepresentations.Length > 0 ? string.Join("\n", bindingMouseRepresentations) : "No mouse bindings")}";
        }

        internal static string GetBindingKeyShortcut(Keybinding bind, bool mark = true) =>
            GetBindingKeyShortcut(bind.BindingUsesMouse, bind.BindingKeyModifiers, bind.BindingKeyName, mark);

        internal static string GetBindingKeyShortcut(ConsoleKeyInfo? bind, bool mark = true)
        {
            if (bind is ConsoleKeyInfo keyInfo)
                return GetBindingKeyShortcut(false, keyInfo.Modifiers, keyInfo.Key, mark);
            return "";
        }

        internal static string GetBindingKeyShortcut(bool usesMouse, ConsoleModifiers mods, ConsoleKey key, bool mark = true)
        {
            if (usesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(mods != 0 ? $"{mods} + " : "")}{key}{markEnd}";
        }

        internal static string GetBindingMouseShortcut(Keybinding bind, bool mark = true)
        {
            if (!bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingPointerModifiers != 0 ? $"{bind.BindingPointerModifiers} + " : "")}{bind.BindingPointerButton}{(bind.BindingPointerButtonPress != 0 ? $" {bind.BindingPointerButtonPress}" : "")}{markEnd}";
        }
    }
}
