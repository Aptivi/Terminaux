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

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Truncated text writer
    /// </summary>
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
            string[] lines = text.FormatString(vars).SplitNewLines();
            return RenderText(lines, settings, textColor, backgroundColor, width, height, left, top, useColor, currIdxRow, currIdxColumn);
        }

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, int currIdxRow, int currIdxColumn) =>
            RenderText(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="textColor">Text foreground color</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, Color textColor, int currIdxRow, int currIdxColumn) =>
            RenderText(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, TextSettings.GlobalSettings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, TextSettings.GlobalSettings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, TextSettings.GlobalSettings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        /// <summary>
        /// Renders a truncated text block according to the width and the height
        /// </summary>
        /// <param name="lines">Lines of text to render</param>
        /// <param name="settings">Text settings to use</param>
        /// <param name="currIdxRow">Current row index for pagination</param>
        /// <param name="currIdxColumn">Current height index for pagination</param>
        /// <returns>A string that you can write to the console with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderText(string[] lines, TextSettings settings, int currIdxRow, int currIdxColumn) =>
            RenderText(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, textColor, ColorTools.currentBackgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, textColor, ColorTools.currentBackgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, textColor, ColorTools.currentBackgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, textColor, backgroundColor, width, height, 0, 0, true, currIdxRow, currIdxColumn);

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
            RenderText(lines, settings, textColor, backgroundColor, width, height, left, top, true, currIdxRow, currIdxColumn);

        internal static string RenderText(string[] lines, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, int currIdxRow, int currIdxColumn)
        {
            // Get the start and the end indexes for lines
            int lineLinesPerPage = height;
            int currentPage = currIdxRow / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage + 1;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (startIndex > lines.Length)
                startIndex = lines.Length;
            if (endIndex > lines.Length)
                endIndex = lines.Length;

            // Get the lines and highlight the selection
            int count = 0;
            var sels = new StringBuilder();
            for (int i = startIndex; i <= endIndex; i++)
            {
                // Get a line
                string source = lines[i - 1].Replace("\t", "    ");
                if (source.Length == 0)
                    source = " ";
                var sequencesCollections = VtSequenceTools.MatchVTSequences(source);
                int vtSeqIdx = 0;

                // Seek through the whole string to find unprintable characters
                var sourceBuilder = new StringBuilder();
                for (int l = 0; l < source.Length; l++)
                {
                    string sequence = ConsolePositioning.BufferChar(source, sequencesCollections, ref l, ref vtSeqIdx, out bool isVtSequence);
                    bool unprintable = ConsoleChar.EstimateCellWidth(sequence) == 0;
                    string rendered = unprintable && !isVtSequence ? "." : sequence;
                    sourceBuilder.Append(rendered);
                }
                source = sourceBuilder.ToString();

                // Now, get the line range
                var lineBuilder = new StringBuilder();
                var absolutes = GetAbsoluteSequences(source, sequencesCollections);
                if (source.Length > 0)
                {
                    int charsPerPage = width;
                    int currentCharPage = currIdxColumn / charsPerPage;
                    int startLineIndex = charsPerPage * currentCharPage;
                    int endLineIndex = charsPerPage * (currentCharPage + 1);
                    if (startLineIndex > absolutes.Length)
                        startLineIndex = absolutes.Length;
                    if (endLineIndex > absolutes.Length)
                        endLineIndex = absolutes.Length;
                    source = "";
                    for (int a = startLineIndex; a < endLineIndex; a++)
                        source += absolutes[a];
                }
                lineBuilder.Append(source);

                // Change the color depending on the highlighted line and column
                int posX = TextWriterTools.DetermineTextAlignment(lineBuilder.ToString(), settings.Alignment, left);
                if (useColor)
                {
                    sels.Append(
                        $"{ColorTools.RenderSetConsoleColor(textColor)}" +
                        $"{ColorTools.RenderSetConsoleColor(backgroundColor, true)}"
                    );
                }
                sels.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(posX + 1, top + count + 1)}" +
                    lineBuilder
                );
                if (useColor)
                {
                    sels.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                count++;
            }
            return sels.ToString();
        }

        private static string[] GetAbsoluteSequences(string source, (VtSequenceType type, Match[] sequences)[] sequencesCollections)
        {
            int vtSeqIdx = 0;
            List<string> sequences = [];
            string sequence = "";
            for (int l = 0; l < source.Length; l++)
            {
                sequence += ConsolePositioning.BufferChar(source, sequencesCollections, ref l, ref vtSeqIdx, out bool isVtSequence);
                if (isVtSequence)
                    continue;
                sequences.Add(sequence);
                sequence = "";
            }
            return [.. sequences];
        }
    }
}
