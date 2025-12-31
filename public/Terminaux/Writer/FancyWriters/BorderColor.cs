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
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Base;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Border writer with color support
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class BorderColor
    {
        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBorderPlain(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static void WriteBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings) =>
            WriteBorderPlain(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static void WriteBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = RenderBorderPlain(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings);
                TextWriterWhereColor.WriteWhere(rendered, Left, Top);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Terminaux's <see cref="Color"/></param>
        /// <param name="TextColor">Border text color from Terminaux's <see cref="Color"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor, Color TextColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, ConsoleColors.Silver);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor, Color TextColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, ConsoleColors.Silver);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor, Color TextColor)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = RenderBorder(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, TextColor);
                TextWriterRaw.WriteRaw(rendered);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static string RenderBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            RenderBorderPlain(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static string RenderBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static string RenderBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static string RenderBorderPlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            RenderBorderPlain(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static string RenderBorderPlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, ColorTools.GetGray(), false, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static string RenderBorderPlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, ColorTools.GetGray(), false, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor) =>
            RenderBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor) =>
            RenderBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor, Color TextColor) =>
            RenderBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, ColorTools.CurrentBackgroundColor);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor, Color TextColor) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor, true);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, ColorTools.CurrentBackgroundColor);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static string RenderBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor, Color TextColor) =>
            RenderBorder("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, TextColor, true);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Terminaux's <see cref="Color"/></param>
        /// <param name="TextColor">Border text color from Terminaux's <see cref="Color"/></param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, new Color(ConsoleColors.Silver), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, new Color(ConsoleColors.Silver), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor, params object[] vars) =>
            WriteBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static void WriteBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor, Color TextColor, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, TextColor, vars);
                TextWriterRaw.WriteRaw(rendered);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, ColorTools.CurrentBackgroundColor, ColorTools.GetGray(), true, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, ColorTools.GetGray(), true, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BorderColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BorderColor, BackgroundColor, TextColor, true, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, ColorTools.CurrentBackgroundColor, ColorTools.GetGray(), true, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, ColorTools.GetGray(), true, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        public static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBorder(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, TextColor, true, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        /// <param name="TextColor">Border text color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderBorder(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BorderColor, Color BackgroundColor, Color TextColor, bool useColor, params object[] vars) =>
            BorderTextColor.RenderBorder(
                text, "", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BorderColor, BackgroundColor, TextColor, useColor, vars
            );

        static BorderColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
