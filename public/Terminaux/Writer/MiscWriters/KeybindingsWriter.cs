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
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.MiscWriters
{
    /// <summary>
    /// Keybindings writer class
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
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
            int maxLength = ConsoleWrapper.WindowWidth - left - rightMargin;
            return new Keybindings()
            {
                Left = left,
                Top = top,
                BackgroundColor = backgroundColor,
                BuiltinColor = builtinColor,
                BuiltinBackgroundColor = builtinBackgroundColor,
                BuiltinForegroundColor = builtinForegroundColor,
                OptionColor = optionColor,
                OptionBackgroundColor = optionBackgroundColor,
                OptionForegroundColor = optionForegroundColor,
                Width = maxLength,
                BuiltinKeybindings = builtinKeybindings,
                KeybindingList = bindings,
                HelpKeyInfo = helpKeyInfo,
            }.Render();
        }
    }
}
