﻿//
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
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Textify.General;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Base;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// BoxFrame writer with color support
    /// </summary>
    public static class BoxFrameColor
    {
        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFramePlain(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static void WriteBoxFramePlain(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings) =>
            WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static void WriteBoxFramePlain(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings);
                TextWriterRaw.WriteRaw(frame);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, ConsoleColors.Silver);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        /// <param name="TextColor">BoxFrame text color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, TextColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, ConsoleColors.Silver);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor, Color BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, BackgroundColor, TextColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, ConsoleColors.Silver);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor, Color TextColor)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, BackgroundColor, TextColor);
                TextWriterRaw.WriteRaw(frame);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor) =>
            RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, ColorTools.CurrentBackgroundColor);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor) =>
            RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, TextColor);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, ColorTools.CurrentBackgroundColor);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor, Color BackgroundColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, BackgroundColor, TextColor, true);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, ColorTools.CurrentBackgroundColor);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, BackgroundColor, TextColor, true);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFramePlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBoxFramePlain(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static void WriteBoxFramePlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, params object[] vars) =>
            WriteBoxFramePlain(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static void WriteBoxFramePlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, vars);
                TextWriterRaw.WriteRaw(frame);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        /// <param name="TextColor">BoxFrame text color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, TextColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, new Color(ConsoleColors.Silver), vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor, Color BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color BoxFrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, BoxFrameColor, BackgroundColor, TextColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, new Color(ConsoleColors.Silver), vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor, Color TextColor, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, BoxFrameColor, BackgroundColor, TextColor, vars);
                TextWriterRaw.WriteRaw(frame);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ColorTools.GetGray(), false, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ColorTools.GetGray(), false, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color FrameColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, FrameColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color FrameColor, Color BackgroundColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, FrameColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color FrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, FrameColor, BackgroundColor, TextColor, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color FrameColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, FrameColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color FrameColor, Color BackgroundColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, FrameColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, Color FrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, TextSettings.GlobalSettings, FrameColor, BackgroundColor, TextColor, true, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color FrameColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, FrameColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color FrameColor, Color BackgroundColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, FrameColor, BackgroundColor, ColorTools.GetGray(), vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color FrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, textSettings, FrameColor, BackgroundColor, TextColor, true, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="textSettings">Text settings to use</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <returns>The rendered frame</returns>
        internal static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor, Color TextColor, bool useColor, params object[] vars)
        {
            try
            {
                // StringBuilder is here to formulate the whole string consisting of box frame
                StringBuilder frameBuilder = new();

                // Colors
                if (useColor)
                {
                    frameBuilder.Append(
                        ColorTools.RenderSetConsoleColor(BoxFrameColor) +
                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }

                // Upper frame
                frameBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)}" +
                    $"{settings.BorderUpperLeftCornerChar}{new string(settings.BorderUpperFrameChar, InteriorWidth)}{settings.BorderUpperRightCornerChar}"
                );

                // Left and right edges
                for (int i = 1; i <= InteriorHeight; i++)
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + i + 1)}" +
                        $"{settings.BorderLeftFrameChar}" +
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + InteriorWidth + 2, Top + i + 1)}" +
                        $"{settings.BorderRightFrameChar}"
                    );

                // Lower frame
                frameBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + InteriorHeight + 2)}" +
                    $"{settings.BorderLowerLeftCornerChar}{new string(settings.BorderLowerFrameChar, InteriorWidth)}{settings.BorderLowerRightCornerChar}"
                );

                // Colors
                if (useColor)
                {
                    frameBuilder.Append(
                        ColorTools.RenderSetConsoleColor(TextColor) +
                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }

                // Text title
                if (!string.IsNullOrEmpty(text) && InteriorWidth - 8 > 0)
                {
                    string finalText = $"{settings.BorderRightHorizontalIntersectionChar} {TextTools.FormatString(text, vars).Truncate(InteriorWidth - 8)} {settings.BorderLeftHorizontalIntersectionChar}";
                    int leftPos = TextWriterTools.DetermineTextAlignment(finalText, InteriorWidth - 8, textSettings.TitleAlignment, Left + 2);
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(leftPos + 1, Top + 1)}" +
                        $"{finalText}"
                    );
                }

                // Write the resulting buffer
                if (useColor)
                {
                    frameBuilder.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                return frameBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static BoxFrameColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
