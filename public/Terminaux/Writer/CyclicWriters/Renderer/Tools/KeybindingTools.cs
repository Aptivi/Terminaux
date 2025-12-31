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

using System;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Keybinding tools
    /// </summary>
    public static class KeybindingTools
    {
        /// <summary>
        /// Shows the keybinding information box
        /// </summary>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfobox(Keybinding[] keybindings, params object[] vars) =>
            ShowKeybindingInfobox(keybindings, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Shows the keybinding information box
        /// </summary>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void ShowKeybindingInfobox(Keybinding[] keybindings, InfoBoxSettings settings, params object[] vars)
        {
            var finalSettings = new InfoBoxSettings(settings)
            {
                Title = string.IsNullOrEmpty(settings.Title) ? LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_KEYBINDING_AVAILABLE_KEYBINDINGS") : settings.Title
            };
            string keybindingsText = RenderKeybindingHelpText(keybindings);
            InfoBoxModalColor.WriteInfoBoxModal(keybindingsText, finalSettings, vars);
        }

        /// <summary>
        /// Renders the keybinding help text
        /// </summary>
        /// <param name="keybindings">Keybindings (including the built-in ones)</param>
        /// <returns>Rendered keybindings help text that you can render for infoboxes.</returns>
        public static string RenderKeybindingHelpText(Keybinding[] keybindings)
        {
            // First, check the bindings length
            Keybinding[] finalBindings = [.. keybindings.Where((kb) => !kb.BindingHidden)];
            var nonMouseBindings = finalBindings.Where((bind) => !bind.BindingUsesMouse).ToArray();
            var mouseBindings = finalBindings.Where((bind) => bind.BindingUsesMouse).ToArray();
            if (!Input.EnableMouse)
                mouseBindings = [];

            // Get the maximum length for keyboard and for mouse
            int maxKeyboardBindingLength = nonMouseBindings.Length > 0 ?
                nonMouseBindings.Max((itb) => ConsoleChar.EstimateCellWidth(GetBindingKeyShortcut(itb))) : 0;
            int maxMouseBindingLength = mouseBindings.Length > 0 ?
                mouseBindings.Max((itb) => ConsoleChar.EstimateCellWidth(GetBindingMouseShortcut(itb))) : 0;
            int maxBindingLength = Math.Max(maxKeyboardBindingLength, maxMouseBindingLength);

            // User needs an infobox that shows all available keys
            string[] bindingRepresentations = [.. nonMouseBindings.Select((itb) => $"{GetBindingKeyShortcut(itb) + new string(' ', maxBindingLength - ConsoleChar.EstimateCellWidth(GetBindingKeyShortcut(itb))) + $" | {itb.BindingName}"}")];
            string[] bindingMouseRepresentations = [.. mouseBindings.Select((itb) => $"{GetBindingMouseShortcut(itb) + new string(' ', maxBindingLength - ConsoleChar.EstimateCellWidth(GetBindingMouseShortcut(itb))) + $" | {itb.BindingName}"}")];

            // Build the final help text
            if (bindingRepresentations.Length == 0 && bindingMouseRepresentations.Length == 0)
                return LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_KEYBINDING_NOBINDINGS");
            var helpTextBuilder = new StringBuilder();
            if (bindingRepresentations.Length > 0)
                helpTextBuilder.Append(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_KEYBINDING_KEYBOARDBINDINGS") + $":\n\n{string.Join("\n", bindingRepresentations)}");
            if (bindingMouseRepresentations.Length > 0)
                helpTextBuilder.Append("\n\n" + LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_KEYBINDING_MOUSEBINDINGS") + $":\n\n{string.Join("\n", bindingMouseRepresentations)}");
            return helpTextBuilder.ToString();
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
