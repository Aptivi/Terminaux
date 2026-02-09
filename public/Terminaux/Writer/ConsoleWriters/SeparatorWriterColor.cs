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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Separator writer
    /// </summary>
    public static class SeparatorWriterColor
    {
        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorPlain(string Text, bool line = true, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSeparatorPlain(Text, Vars));
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
        public static void WriteSeparator(string Text, bool line = true, params object[] Vars) =>
            WriteSeparator(Text, ThemeColorType.NeutralText, line, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, ThemeColorType Color = ThemeColorType.NeutralText, bool line = true, params object[] Vars) =>
            WriteSeparator(Text, Color, ThemeColorType.Background, line, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, bool line = true, params object[] Vars) =>
            WriteSeparatorColorBack(Text, ThemeColorsTools.GetColor(ForegroundColor), ThemeColorsTools.GetColor(BackgroundColor), line, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="line">Whether to write a new line or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, Color Color, bool line = true, params object[] Vars) =>
            WriteSeparatorColorBack(Text, Color, ThemeColorsTools.GetColor(ThemeColorType.Background), line, Vars);

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
        public static string RenderSeparatorPlain(string Text, params object[] Vars) =>
            RenderSeparator(Text, ThemeColorsTools.GetColor(ThemeColorType.NeutralText), ThemeColorsTools.GetColor(ThemeColorType.Background), false, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, params object[] Vars) =>
            RenderSeparator(Text, ThemeColorType.NeutralText, ThemeColorType.Background, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, params object[] Vars) =>
            RenderSeparator(Text, ForegroundColor, ThemeColorType.Background, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] Vars) =>
            RenderSeparator(Text, ThemeColorsTools.GetColor(ForegroundColor), ThemeColorsTools.GetColor(BackgroundColor), Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, Color ForegroundColor, params object[] Vars) =>
            RenderSeparator(Text, ForegroundColor, ThemeColorsTools.GetColor(ThemeColorType.Background), Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderSeparator(Text, ForegroundColor, BackgroundColor, true, Vars);

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
                    Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 8) : Text;
                    if (useColor)
                    {
                        separator.Append(
                            ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                            ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
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
                            ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                            ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    separator.Append(new string('─', RepeatTimes));
                }

                // Return the resulting separator
                if (useColor)
                {
                    separator.Append(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
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
    }
}
