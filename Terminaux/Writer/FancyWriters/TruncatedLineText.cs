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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Truncated text writer by line number
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class TruncatedLineText
    {
        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, Color backgroundColor, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, Color backgroundColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, textColor, backgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            WriteText(text, settings, textColor, backgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        internal static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, int currIdx, ref int increment, params object[] vars)
        {
            string[] lines = TextWriterTools.GetFinalLines(text, width, vars);
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, height, left, top, useColor, currIdx, ref increment));
        }

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, int height, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, int height, int left, int top, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, int height, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, int height, int left, int top, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, Color backgroundColor, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, Color backgroundColor, int height, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, height, 0, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, Color backgroundColor, int height, int left, int top, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, height, left, top, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, int height, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, int height, int left, int top, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, int height, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, textColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, int height, int left, int top, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, textColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int height, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, height, 0, 0, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int height, int left, int top, int currIdx, ref int increment) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, height, left, top, true, currIdx, ref increment));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, Color backgroundColor, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, Color backgroundColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, width, height, 0, 0, true, currIdx, ref increment, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdx, ref int increment, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, width, height, left, top, true, currIdx, ref increment, vars);

        internal static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, int currIdx, ref int increment, params object[] vars)
        {
            string[] lines = TextWriterTools.GetFinalLines(text, width, vars);
            return BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, height, left, top, useColor, currIdx, ref increment);
        }

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, int height, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, int height, int left, int top, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, int height, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, int height, int left, int top, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, Color backgroundColor, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, Color backgroundColor, int height, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, height, 0, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, Color backgroundColor, int height, int left, int top, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, height, left, top, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, int height, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, int height, int left, int top, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, int height, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, textColor, ColorTools.currentBackgroundColor, height, 0, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, int height, int left, int top, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, textColor, ColorTools.currentBackgroundColor, height, left, top, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int height, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, height, 0, 0, true, currIdx, ref increment);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdx">Current line index for pagination</param>
        /// <param name="increment">[<see langword="out"/>] Lines to increment</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int height, int left, int top, int currIdx, ref int increment) =>
            BoundedText.RenderTextLinewise(lines, settings, textColor, backgroundColor, height, left, top, true, currIdx, ref increment);
    }
}
