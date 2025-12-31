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
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Truncated text writer by column and row number
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class TruncatedText
    {
        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

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
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

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
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

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
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            WriteText(text, settings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        internal static void WriteText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, int currIdxRow, int currIdxColumn, params object[] vars)
        {
            string[] lines = TextWriterTools.GetFinalLines(text, width, vars);
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, width, height, left, top, useColor, currIdxRow, currIdxColumn));
        }

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, int width, int height, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, int width, int height, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static void WriteText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            TextWriterRaw.WriteRaw(BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn));

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

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
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

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
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn, vars);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn, vars);

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
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <param name="vars">Variables to format the text with</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn, vars);

        internal static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, int currIdxRow, int currIdxColumn, params object[] vars)
        {
            string[] lines = TextWriterTools.GetFinalLines(text, width, vars);
            return BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, width, height, left, top, useColor, currIdxRow, currIdxColumn);
        }

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, int width, int height, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, int width, int height, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="width">Truncated text width (think of it as a rectangle width)</param>
        /// <param name="height">Truncated text height (think of it as a rectangle height)</param>
        /// <param name="left">Left position of the truncated text "rectangle"</param>
        /// <param name="top">Top position of the truncated text "rectangle"</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, int currIdxRow, int currIdxColumn) =>
            BoundedText.RenderTextPoswise(lines, settings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);
    }
}
