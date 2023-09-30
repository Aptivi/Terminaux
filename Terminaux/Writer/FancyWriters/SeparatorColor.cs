
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Writer.FancyWriters
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
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, params object[] Vars)
        {
            try
            {
                bool canPosition = !ConsoleChecker.IsDumb;
                Text = ConsoleExtensions.FormatString(Text, Vars);

                // Print the suffix and the text
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    if (PrintSuffix)
                        TextWriterColor.Write("- ", false, new Color(ConsoleColors.Gray));
                    if (!Text.EndsWith("-"))
                        Text += " ";

                    // We need to set an appropriate color for the suffix in the text.
                    if (Text.StartsWith("-"))
                    {
                        for (int CharIndex = 0; CharIndex <= Text.Length - 1; CharIndex++)
                        {
                            if (Convert.ToString(Text[CharIndex]) == "-")
                            {
                                TextWriterColor.Write(Convert.ToString(Text[CharIndex]), false, new Color(ConsoleColors.Gray));
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
                    Text = canPosition ? Text.Truncate(ConsoleWrappers.ActionWindowWidth() - 6) : Text;
                    TextWriterColor.Write(Text, false, new Color(ConsoleColors.Gray));
                }

                // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
                int RepeatTimes = 0;
                if (canPosition)
                    RepeatTimes = ConsoleWrappers.ActionWindowWidth() - (Text + " ").Length - 1;

                // Write the closing minus sign.
                TextWriterColor.Write(new string('-', RepeatTimes), true, new Color(ConsoleColors.Gray));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
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
        public static void WriteSeparator(string Text, bool PrintSuffix, ConsoleColors Color, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, new Color(Color), new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, Color Color, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, Color, new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                bool canPosition = !ConsoleChecker.IsDumb;
                Text = ConsoleExtensions.FormatString(Text, Vars);

                // Print the suffix and the text
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    if (PrintSuffix)
                        Text = "- " + Text;
                    if (!Text.EndsWith("-"))
                        Text += " ";

                    // Render the text accordingly
                    Text = canPosition ? Text.Truncate(ConsoleWrappers.ActionWindowWidth() - 6) : Text;
                    TextWriterColor.Write(Text, false, ForegroundColor, BackgroundColor);
                }

                // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
                int RepeatTimes = 0;
                if (canPosition)
                    RepeatTimes = ConsoleWrappers.ActionWindowWidth() - (Text + " ").Length + 1;

                // Write the closing minus sign.
                TextWriterColor.Write(new string('-', RepeatTimes), true, ForegroundColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

    }
}
