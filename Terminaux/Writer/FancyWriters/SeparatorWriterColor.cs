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
using System.Diagnostics;
using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Writer.FancyWriters
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
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorPlain(string Text, params object[] Vars) =>
            WriteSeparatorPlain(Text, true, Vars);

        /// <summary>
        /// Draw a separator with text plainly
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorPlain(string Text, bool PrintSuffix, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSeparator(Text, PrintSuffix, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, params object[] Vars) =>
            WriteSeparator(Text, true, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, Color Color, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, ForegroundColor, BackgroundColor, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSeparator(Text, PrintSuffix, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, bool PrintSuffix, Color Color, params object[] Vars) =>
            WriteSeparatorColorBack(Text, PrintSuffix, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColorBack(string Text, bool PrintSuffix, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSeparator(Text, PrintSuffix, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, params object[] Vars) =>
            RenderSeparator(Text, true, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, bool PrintSuffix, params object[] Vars) =>
            RenderSeparator(Text, PrintSuffix, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, Color ForegroundColor, params object[] Vars) =>
            RenderSeparator(Text, true, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, bool PrintSuffix, Color ForegroundColor, params object[] Vars) =>
            RenderSeparator(Text, PrintSuffix, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderSeparator(Text, true, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, bool PrintSuffix, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderSeparator(Text, PrintSuffix, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderSeparator(string Text, bool PrintSuffix, Color ForegroundColor, Color BackgroundColor, bool useColor, params object[] Vars)
        {
            try
            {
                var separator = new StringBuilder();
                bool canPosition = !ConsoleWrapper.IsDumb;
                Text = TextTools.FormatString(Text, Vars);

                // Print the suffix and the text
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    if (PrintSuffix)
                    {
                        if (useColor)
                        {
                            separator.Append(
                                ForegroundColor.VTSequenceForeground +
                                ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                            );
                        }
                        separator.Append("- ");
                    }

                    if (!Text.EndsWith("-"))
                        Text += " ";

                    // We need to set an appropriate color for the suffix in the text.
                    if (Text.StartsWith("-"))
                    {
                        for (int CharIndex = 0; CharIndex <= Text.Length - 1; CharIndex++)
                        {
                            if (Text[CharIndex] == '-')
                            {
                                if (useColor)
                                {
                                    separator.Append(
                                        ForegroundColor.VTSequenceForeground +
                                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                                    );
                                }
                                separator.Append(Text[CharIndex]);
                            }
                            else
                            {
                                // We're (mostly) done
                                Text = Text.Substring(CharIndex);
                                break;
                            }
                        }
                    }

                    // Render the text accordingly
                    Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 6) : Text;
                    if (useColor)
                    {
                        separator.Append(
                            ForegroundColor.VTSequenceForeground +
                            ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    separator.Append(Text);
                }

                // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
                int RepeatTimes = 0;
                if (canPosition)
                    RepeatTimes = ConsoleWrapper.WindowWidth - (Text + " ").Length - 1;

                // Write the closing minus sign.
                if (useColor)
                {
                    separator.Append(
                        ForegroundColor.VTSequenceForeground +
                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                separator.Append(new string('-', RepeatTimes));
                if (useColor)
                {
                    separator.Append(
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) +
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
                    );
                }
                return separator.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

    }
}
