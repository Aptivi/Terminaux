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
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Centered writer
    /// </summary>
    public static class CenteredTextColor
    {
        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCentered(top, Text, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColor(int top, string Text, Color Color, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColorBack(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCentered(top, Text, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredOneLine(top, Text, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColor(int top, string Text, Color Color, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColorBack(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredOneLine(top, Text, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCentered(Text, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColor(string Text, Color Color, params object[] Vars) =>
            WriteCenteredColorBack(Text, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCentered(Text, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredOneLine(Text, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColor(string Text, Color Color, params object[] Vars) =>
            WriteCenteredOneLineColorBack(Text, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredOneLine(Text, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(string Text, params object[] Vars) =>
            RenderCentered(Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(string Text, Color ForegroundColor, params object[] Vars) =>
            RenderCentered(Text, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderCentered(Text, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderCentered(string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderCentered(top, Text, ForegroundColor, BackgroundColor, useColor, Vars);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(int top, string Text, params object[] Vars) =>
            RenderCentered(top, Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(int top, string Text, Color ForegroundColor, params object[] Vars) =>
            RenderCentered(top, Text, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderCentered(top, Text, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderCentered(int top, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, params object[] Vars)
        {
            try
            {
                var centered = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - ConsoleMisc.FilterVTSequences(sentence).Length / 2;
                    consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                    centered.Append(
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                        TextWriterWhereColor.RenderWhere(sentence, consoleInfoX, top + i, true, Vars)
                    );
                }

                // Write the resulting buffer
                if (useColor)
                {
                    centered.Append(
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) +
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
                    );
                }
                return centered.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredOneLine(string Text, params object[] Vars)
        {
            try
            {
                var centered = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredOneLine(string Text, Color ForegroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, ColorTools.currentBackgroundColor);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredOneLine(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredOneLine(int top, string Text, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredOneLine(int top, string Text, Color ForegroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, ColorTools.currentBackgroundColor);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredOneLine(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static CenteredTextColor()
        {
            if (GeneralColorTools.CheckConsoleOnCall)
                ConsoleChecker.CheckConsole();
        }
    }
}
