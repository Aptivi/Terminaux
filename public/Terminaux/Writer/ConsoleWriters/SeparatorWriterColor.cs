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
using System.Diagnostics;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Textify.General;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Separator writer
    /// </summary>
    public static class SeparatorWriterColor
    {
        /// <summary>
        /// Draw a separator with text plainly
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorPlain(string Text, bool line = true, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSeparator(Text, Vars));
                if (line)
                    TextWriterRaw.WriteRaw("\n");
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool line = true, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSeparator(Text, Vars));
                if (line)
                    TextWriterRaw.WriteRaw("\n");
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, Color Color, bool line = true, params object[] Vars) =>
            WriteSeparatorColorBack(Text, Color, ColorTools.currentBackgroundColor, line, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColorBack(string Text, Color ForegroundColor, Color BackgroundColor, bool line = true, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSeparator(Text, ForegroundColor, BackgroundColor, Vars));
                if (line)
                    TextWriterRaw.WriteRaw("\n");
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, params object[] Vars) =>
            RenderSeparator(Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, Color ForegroundColor, params object[] Vars) =>
            RenderSeparator(Text, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderSeparator(Text, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderSeparator(string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, params object[] Vars)
        {
            try
            {
                var separator = new StringBuilder();
                bool canPosition = !ConsoleWrapper.IsDumb;
                Text = Text.FormatString(Vars);

                // Print the suffix and the text
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    // Render the text accordingly
                    Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 10) : Text;
                    if (useColor)
                    {
                        separator.Append(
                            ColorTools.RenderSetConsoleColor(ForegroundColor) +
                            ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    separator.Append($"──╢ {Text} ");
                }

                // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
                int RepeatTimes = 0;
                if (canPosition)
                {
                    int width = ConsoleChar.EstimateCellWidth(separator.ToString());
                    RepeatTimes = ConsoleWrapper.WindowWidth - width - 1;
                }

                // Write the closing minus sign.
                if (RepeatTimes > 0)
                {
                    separator.Append('╟');
                    if (useColor)
                    {
                        separator.Append(
                            ColorTools.RenderSetConsoleColor(ForegroundColor) +
                            ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    separator.Append(new string('─', RepeatTimes));
                }

                // Return the resulting separator
                if (useColor)
                {
                    separator.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                return separator.ToString();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static SeparatorWriterColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
