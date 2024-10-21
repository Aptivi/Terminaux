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
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Writer.MiscWriters.Tools;
using Textify.Data.Figlet;
using Textify.Data.Figlet.Utilities.Lines;
using Textify.General;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Aligned figlet writer
    /// </summary>
    public static class AlignedFigletTextColor
    {
        /// <summary>
        /// Draws an aligned figlet text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAligned(int top, FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(top, FigletFont, Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned figlet text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColor(int top, FigletFont FigletFont, string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedColorBack(top, FigletFont, Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned figlet text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColorBack(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(top, FigletFont, Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLine(int top, FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(top, FigletFont, Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColor(int top, FigletFont FigletFont, string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedOneLineColorBack(top, FigletFont, Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColorBack(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(top, FigletFont, Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned figlet text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAligned(FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(FigletFont, Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned figlet text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColor(FigletFont FigletFont, string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedColorBack(FigletFont, Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned figlet text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedColorBack(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAligned(FigletFont, Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLine(FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(FigletFont, Text, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draws an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColor(FigletFont FigletFont, string Text, Color Color, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteAlignedOneLineColorBack(FigletFont, Text, Color, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteAlignedOneLineColorBack(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderAlignedOneLine(FigletFont, Text, ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(FigletFont, Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(FigletFont FigletFont, string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(FigletFont, Text, ForegroundColor, ColorTools.currentBackgroundColor, true, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(FigletFont, Text, ForegroundColor, BackgroundColor, true, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderAligned(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, leftMargin, rightMargin, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderAligned(top, FigletFont, Text, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, Vars);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(int top, FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(top, FigletFont, Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(int top, FigletFont FigletFont, string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(top, FigletFont, Text, ForegroundColor, ColorTools.currentBackgroundColor, true, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAligned(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderAligned(top, FigletFont, Text, ForegroundColor, BackgroundColor, true, alignment, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderAligned(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
                int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                int consoleMaxY = top + figHeight;
                int textMaxWidth = ConsoleWrapper.WindowWidth - (leftMargin + consoleX + rightMargin);
                if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                {
                    // The figlet won't fit, so use small text
                    consoleX = ConsoleWrapper.WindowWidth / 2 - figWidthFallback;
                    consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                    consoleMaxY = top + figHeightFallback;
                    if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        return AlignedTextColor.RenderAligned(top, Text, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, Vars);
                    }
                    else
                    {
                        // Write the figlet.
                        string renderedFiglet = FigletTools.RenderFiglet(Text, figFontFallback, textMaxWidth, Vars);
                        return AlignedTextColor.RenderAligned(consoleY, renderedFiglet, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, Vars);
                    }
                }
                else
                {
                    // Write the figlet.
                    string renderedFiglet = FigletTools.RenderFiglet(Text, FigletFont, textMaxWidth, Vars);
                    return AlignedTextColor.RenderAligned(consoleY, renderedFiglet, ForegroundColor, BackgroundColor, useColor, alignment, leftMargin, rightMargin, Vars);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                var aligned = new StringBuilder();
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderAligned(top, FigletFont, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(FigletFont FigletFont, string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderAligned(top, FigletFont, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                int top = ConsoleWrapper.WindowHeight / 2 - sentences.Length / 2;
                return RenderAligned(top, FigletFont, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(int top, FigletFont FigletFont, string Text, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(top, FigletFont, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(int top, FigletFont FigletFont, string Text, Color ForegroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(top, FigletFont, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, ColorTools.currentBackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Renders an aligned figlet text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write aligned text to</param>
        /// <param name="Text">Text to be written.</param>
        /// <param name="FigletFont">Figlet font to use</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderAlignedOneLine(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, TextAlignment alignment = TextAlignment.Left, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth - rightMargin - leftMargin);
                return RenderAligned(top, FigletFont, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4), ForegroundColor, BackgroundColor, alignment, leftMargin, rightMargin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static AlignedFigletTextColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
