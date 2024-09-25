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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Writer.MiscWriters.Tools;

namespace Terminaux.Writer.MiscWriters
{
    /// <summary>
    /// Keybindings writer class
    /// </summary>
    public static class KeybindingsWriter
    {
        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            WriteKeybindings(bindings, [], new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            WriteKeybindings(bindings, builtinKeybindings, new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            WriteKeybindings(bindings, [], new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), backgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            WriteKeybindings(bindings, builtinKeybindings, new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), backgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            WriteKeybindings(bindings, [], builtinColor, builtinForegroundColor, builtinBackgroundColor, optionColor, optionForegroundColor, optionBackgroundColor, ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            WriteKeybindings(bindings, builtinKeybindings, builtinColor, builtinForegroundColor, builtinBackgroundColor, optionColor, optionForegroundColor, optionBackgroundColor, ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            WriteKeybindings(bindings, [], builtinColor, builtinForegroundColor, builtinBackgroundColor, optionColor, optionForegroundColor, optionBackgroundColor, backgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Writes the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null)
        {
            string rendered = RenderKeybindings(bindings, builtinKeybindings, builtinColor, builtinForegroundColor, builtinBackgroundColor, optionColor, optionForegroundColor, optionBackgroundColor, backgroundColor, left, top, rightMargin, helpKeyInfo);
            TextWriterRaw.WriteRaw(rendered);
        }

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            RenderKeybindings(bindings, [], new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            RenderKeybindings(bindings, builtinKeybindings, new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            RenderKeybindings(bindings, [], new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), backgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            RenderKeybindings(bindings, builtinKeybindings, new(ConsoleColors.Black), new(ConsoleColors.Lime), new(ConsoleColors.Green), new(ConsoleColors.Black), new(ConsoleColors.Yellow), new(ConsoleColors.Olive), backgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            RenderKeybindings(bindings, [], builtinColor, builtinForegroundColor, builtinBackgroundColor, optionColor, optionForegroundColor, optionBackgroundColor, ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            RenderKeybindings(bindings, builtinKeybindings, builtinColor, builtinForegroundColor, builtinBackgroundColor, optionColor, optionForegroundColor, optionBackgroundColor, ColorTools.CurrentBackgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null) =>
            RenderKeybindings(bindings, [], builtinColor, builtinForegroundColor, builtinBackgroundColor, optionColor, optionForegroundColor, optionBackgroundColor, backgroundColor, left, top, rightMargin, helpKeyInfo);

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <param name="bindings">List of keybindings (without the builtin ones)</param>
        /// <param name="builtinKeybindings">List of builtin keybindings</param>
        /// <param name="left">Left position of the keybinding group</param>
        /// <param name="top">Top position of the keybinding group</param>
        /// <param name="rightMargin">Right margin of the keybinding group</param>
        /// <param name="helpKeyInfo">Key information that opens the help page</param>
        /// <param name="builtinColor">Built-in keybinding foreground color in the background color</param>
        /// <param name="builtinForegroundColor">Built-in keybinding foreground color out of the background color</param>
        /// <param name="builtinBackgroundColor">Built-in keybinding background color</param>
        /// <param name="optionColor">Option keybinding foreground color in the background color</param>
        /// <param name="optionForegroundColor">Option keybinding foreground color out of the background color</param>
        /// <param name="optionBackgroundColor">Option keybinding background color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <returns>Keybindings sequence that you can use with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, Color backgroundColor, int left = 0, int top = 0, int rightMargin = 0, ConsoleKeyInfo? helpKeyInfo = null)
        {
            var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(left + 1, top + 1));
            Keybinding[] finalBindings = [.. builtinKeybindings, .. bindings];
            foreach (Keybinding binding in finalBindings)
            {
                // Check the binding mode
                if (binding.BindingUsesMouse)
                    continue;

                // First, check to see if the rendered binding info is going to exceed the console window width
                string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {binding.BindingName}  ";
                string renderedExtraBinding = GetBindingKeyShortcut(helpKeyInfo, false);
                int bindingLength = ConsoleChar.EstimateCellWidth(renderedBinding);
                int bindingExtraLength = ConsoleChar.EstimateCellWidth(renderedExtraBinding);
                int actualLength = ConsoleChar.EstimateCellWidth(VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()));
                int maxLength = ConsoleWrapper.WindowWidth - left - rightMargin - bindingExtraLength;
                bool canDraw = bindingLength + actualLength < maxLength;
                bool isBuiltin = builtinKeybindings.Contains(binding);
                if (canDraw)
                {
                    bindingsBuilder.Append(
                        $"{ColorTools.RenderSetConsoleColor(isBuiltin ? builtinColor : optionColor, false, true)}" +
                        $"{ColorTools.RenderSetConsoleColor(isBuiltin ? builtinBackgroundColor : optionBackgroundColor, true)}" +
                        GetBindingKeyShortcut(binding, false) +
                        $"{ColorTools.RenderSetConsoleColor(isBuiltin ? builtinForegroundColor : optionForegroundColor)}" +
                        $"{ColorTools.RenderSetConsoleColor(backgroundColor, true)}" +
                        $" {binding.BindingName}  "
                    );
                }
                else
                {
                    // We can't render anymore, so just break and write a binding to show more if it's provided
                    int extraKeyLeft = ConsoleWrapper.WindowWidth - rightMargin - bindingExtraLength;
                    bindingsBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(extraKeyLeft + 1, top + 1)}" +
                        $"{ColorTools.RenderSetConsoleColor(builtinColor, false, true)}" +
                        $"{ColorTools.RenderSetConsoleColor(builtinBackgroundColor, true)}" +
                        renderedExtraBinding
                    );
                    break;
                }
            }
            bindingsBuilder.Append(
                ColorTools.RenderRevertForeground() +
                ColorTools.RenderRevertBackground()
            );
            return bindingsBuilder.ToString();
        }

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

        private static string GetBindingKeyShortcut(Keybinding bind, bool mark = true) =>
            GetBindingKeyShortcut(bind.BindingUsesMouse, bind.BindingKeyModifiers, bind.BindingKeyName, mark);

        private static string GetBindingKeyShortcut(ConsoleKeyInfo? bind, bool mark = true)
        {
            if (bind is ConsoleKeyInfo keyInfo)
                return GetBindingKeyShortcut(false, keyInfo.Modifiers, keyInfo.Key, mark);
            return "";
        }

        private static string GetBindingKeyShortcut(bool usesMouse, ConsoleModifiers mods, ConsoleKey key, bool mark = true)
        {
            if (usesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(mods != 0 ? $"{mods} + " : "")}{key}{markEnd}";
        }

        private static string GetBindingMouseShortcut(Keybinding bind, bool mark = true)
        {
            if (!bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingPointerModifiers != 0 ? $"{bind.BindingPointerModifiers} + " : "")}{bind.BindingPointerButton}{(bind.BindingPointerButtonPress != 0 ? $" {bind.BindingPointerButtonPress}" : "")}{markEnd}";
        }
    }
}
