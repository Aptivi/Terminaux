﻿//
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
using Terminaux.Colors;
using Textify.Data.Figlet.Utilities.Lines;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Base.Checks;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Figlet writer
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class FigletColor
    {
        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletPlain(string Text, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteFigletPlain(Text, FigletTextTools.DefaultFigletFont, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletPlain(string Text, FigletFont FigletFont, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(
                    RenderFigletPlain(Text, FigletFont, leftMargin, rightMargin, Vars)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletColor(string Text, Color Color, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteFigletColor(Text, FigletTextTools.DefaultFigletFont, Color, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletColor(string Text, FigletFont FigletFont, Color Color, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(
                    RenderFiglet(Text, FigletFont, Color, ColorTools.currentBackgroundColor, leftMargin, rightMargin, Vars)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletColorBack(string Text, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteFigletColorBack(Text, FigletTextTools.DefaultFigletFont, ForegroundColor, BackgroundColor, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletColorBack(string Text, FigletFont FigletFont, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(
                    RenderFiglet(Text, FigletFont, ForegroundColor, BackgroundColor, leftMargin, rightMargin, Vars)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletPlain(string Text, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            FigletText.RenderFiglet(Text, FigletTextTools.DefaultFigletFont, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletPlain(string Text, FigletFont FigletFont, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            FigletText.RenderFiglet(Text, FigletFont, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFiglet(string Text, Color ForegroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            FigletText.RenderFiglet(Text, FigletTextTools.DefaultFigletFont, ForegroundColor, ColorTools.currentBackgroundColor, true, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFiglet(string Text, FigletFont FigletFont, Color ForegroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            FigletText.RenderFiglet(Text, FigletFont, ForegroundColor, ColorTools.currentBackgroundColor, true, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFiglet(string Text, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            FigletText.RenderFiglet(Text, FigletTextTools.DefaultFigletFont, ForegroundColor, BackgroundColor, true, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFiglet(string Text, FigletFont FigletFont, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            FigletText.RenderFiglet(Text, FigletFont, ForegroundColor, BackgroundColor, true, leftMargin, rightMargin, Vars);

        static FigletColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
