
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
using System.Text;
using System.Threading;
using Terminaux.Base;
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
                TextWriterColor.WritePlain(RenderCentered(top, Text, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColor(int top, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, new Color(Color), ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColorBack(int top, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

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
                TextWriterColor.WritePlain(RenderCentered(top, Text, ForegroundColor, BackgroundColor, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
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
                TextWriterColor.WritePlain(RenderCenteredOneLine(top, Text, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColor(int top, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, new Color(Color), ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColorBack(int top, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

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
                TextWriterColor.WritePlain(RenderCenteredOneLine(top, Text, ForegroundColor, BackgroundColor, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
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
                TextWriterColor.WritePlain(RenderCentered(Text, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColor(string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredColorBack(Text, new Color(Color), ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColorBack(string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredColorBack(Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

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
                TextWriterColor.WritePlain(RenderCentered(Text, ForegroundColor, BackgroundColor, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
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
                TextWriterColor.WritePlain(RenderCenteredOneLine(Text, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColor(string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredOneLineColorBack(Text, new Color(Color), ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColorBack(string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredOneLineColorBack(Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

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
                TextWriterColor.WritePlain(RenderCenteredOneLine(Text, ForegroundColor, BackgroundColor, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(string Text, params object[] Vars)
        {
            try
            {
                var centered = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - sentence.Length / 2;
                    consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                    centered.Append(
                        TextWriterWhereColor.RenderWherePlain(sentence + "\n", consoleInfoX, top, Vars)
                    );
                }
                return centered.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                var centered = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - sentence.Length / 2;
                    consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                    centered.Append(
                        ForegroundColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground +
                        TextWriterWhereColor.RenderWherePlain(sentence + "\n", consoleInfoX, top, Vars)
                    );
                }

                // Write the resulting buffer
                centered.Append(
                    ColorTools.currentForegroundColor.VTSequenceForeground +
                    ColorTools.currentBackgroundColor.VTSequenceBackground
                );
                return centered.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(int top, string Text, params object[] Vars)
        {
            try
            {
                var centered = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - sentence.Length / 2;
                    consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                    centered.Append(
                        TextWriterWhereColor.RenderWherePlain(sentence + "\n", consoleInfoX, top, Vars)
                    );
                }
                return centered.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCentered(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                var centered = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - sentence.Length / 2;
                    consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                    centered.Append(
                        ForegroundColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground +
                        TextWriterWhereColor.RenderWherePlain(sentence + "\n", consoleInfoX, top, Vars)
                    );
                }

                // Write the resulting buffer
                centered.Append(
                    ColorTools.currentForegroundColor.VTSequenceForeground +
                    ColorTools.currentBackgroundColor.VTSequenceBackground
                );
                return centered.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
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
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
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
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
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
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
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
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                return RenderCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            return "";
        }

    }
}
