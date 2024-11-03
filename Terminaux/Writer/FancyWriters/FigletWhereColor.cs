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
using Terminaux.Colors;
using Textify.Data.Figlet.Utilities.Lines;
using Textify.Data.Figlet;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Base.Checks;
using Terminaux.Base;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Figlet writer (positional)
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class FigletWhereColor
    {
        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletWherePlain(string Text, int Left, int Top, bool Return, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteFigletWherePlain(Text, Left, Top, Return, FigletTextTools.DefaultFigletFont, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletWherePlain(string Text, int Left, int Top, bool Return, FigletFont FigletFont, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderFigletWherePlain(Text, Left, Top, Return, FigletFont, leftMargin, rightMargin, Vars)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletWhereColor(string Text, int Left, int Top, bool Return, Color Color, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteFigletWhereColor(Text, Left, Top, Return, FigletTextTools.DefaultFigletFont, Color, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletWhereColor(string Text, int Left, int Top, bool Return, FigletFont FigletFont, Color Color, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderFigletWhere(Text, Left, Top, Return, FigletFont, Color, leftMargin, rightMargin, Vars)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletWhereColorBack(string Text, int Left, int Top, bool Return, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            WriteFigletWhereColorBack(Text, Left, Top, Return, FigletTextTools.DefaultFigletFont, ForegroundColor, BackgroundColor, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static void WriteFigletWhereColorBack(string Text, int Left, int Top, bool Return, FigletFont FigletFont, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderFigletWhere(Text, Left, Top, Return, FigletFont, ForegroundColor, BackgroundColor, leftMargin, rightMargin, Vars)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletWherePlain(string Text, int Left, int Top, bool Return, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderFigletWhere(Text, Left, Top, Return, FigletTextTools.DefaultFigletFont, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletWherePlain(string Text, int Left, int Top, bool Return, FigletFont FigletFont, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderFigletWhere(Text, Left, Top, Return, FigletFont, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletWhere(string Text, int Left, int Top, bool Return, Color ForegroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderFigletWhere(Text, Left, Top, Return, FigletTextTools.DefaultFigletFont, ForegroundColor, ColorTools.currentBackgroundColor, true, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletWhere(string Text, int Left, int Top, bool Return, FigletFont FigletFont, Color ForegroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderFigletWhere(Text, Left, Top, Return, FigletFont, ForegroundColor, ColorTools.currentBackgroundColor, true, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletWhere(string Text, int Left, int Top, bool Return, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderFigletWhere(Text, Left, Top, Return, FigletTextTools.DefaultFigletFont, ForegroundColor, BackgroundColor, true, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        public static string RenderFigletWhere(string Text, int Left, int Top, bool Return, FigletFont FigletFont, Color ForegroundColor, Color BackgroundColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            RenderFigletWhere(Text, Left, Top, Return, FigletFont, ForegroundColor, BackgroundColor, true, leftMargin, rightMargin, Vars);

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        internal static string RenderFigletWhere(string Text, int Left, int Top, bool Return, FigletFont FigletFont, Color ForegroundColor, Color BackgroundColor, bool useColor, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            Text = FigletTools.RenderFiglet(Text, FigletFont, ConsoleWrapper.WindowWidth - (leftMargin + Left + rightMargin), Vars);
            var builder = new StringBuilder();
            builder.Append(
                $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                TextWriterWhereColor.RenderWhere(Text, Left + leftMargin, Top, Return, rightMargin, Vars) +
                $"{(useColor ? ColorTools.RenderRevertForeground() : "")}" +
                $"{(useColor ? ColorTools.RenderRevertBackground() : "")}"
            );
            return builder.ToString();
        }

        static FigletWhereColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
