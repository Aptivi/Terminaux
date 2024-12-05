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
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Aligned writer
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class AlignedTextColor
    {
        /// <summary>
        /// Draws an aligned text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAligned(int top, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(top, Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColor(int top, string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedColorBack(top, Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColorBack(int top, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(top, Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLine(int top, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(top, Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColor(int top, string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedOneLineColorBack(top, Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColorBack(int top, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(top, Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAligned(string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColor(string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedColorBack(Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColorBack(string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLine(string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColor(string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedOneLineColorBack(Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColorBack(string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(Text, ForegroundColor, ColorTools.currentBackgroundColor, true, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(Text, ForegroundColor, BackgroundColor, true, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderAligned(string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, leftMargin, rightMargin, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return AlignedText.RenderAligned(top, Text, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, 0, null, Vars);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(int top, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            AlignedText.RenderAligned(top, Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, alignment, leftMargin, rightMargin, 0, null, Vars);

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(int top, string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            AlignedText.RenderAligned(top, Text, ForegroundColor, ColorTools.currentBackgroundColor, true, alignment, leftMargin, rightMargin, 0, null, Vars);

        /// <summary>
        /// Renders an aligned text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(int top, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            AlignedText.RenderAligned(top, Text, ForegroundColor, BackgroundColor, true, alignment, leftMargin, rightMargin, 0, null, Vars);

        /// <summary>
        /// Renders an aligned text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                var aligned = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderAligned(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderAligned(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderAligned(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(int top, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(int top, string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(int top, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static AlignedTextColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
