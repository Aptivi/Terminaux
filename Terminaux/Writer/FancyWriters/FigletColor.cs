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
using Figletize.Utilities;
using Figletize;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Base.Checks;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Figlet writer
    /// </summary>
    public static class FigletColor
    {
        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletPlain(string Text, params object[] Vars) =>
            WriteFigletPlain(Text, FigletTextTools.DefaultFigletFont, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletPlain(string Text, FigletizeFont FigletFont, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderFigletPlain(Text, FigletFont, Vars)
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
        public static void WriteFigletColor(string Text, Color Color, params object[] Vars) =>
            WriteFigletColor(Text, FigletTextTools.DefaultFigletFont, Color, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletColor(string Text, FigletizeFont FigletFont, Color Color, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderFiglet(Text, FigletFont, Color, ColorTools.currentBackgroundColor, Vars)
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
        public static void WriteFigletColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            WriteFigletColorBack(Text, FigletTextTools.DefaultFigletFont, ForegroundColor, BackgroundColor, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletColorBack(string Text, FigletizeFont FigletFont, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderFiglet(Text, FigletFont, ForegroundColor, BackgroundColor, Vars)
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
        public static string RenderFigletPlain(string Text, params object[] Vars) =>
            RenderFiglet(Text, FigletTextTools.DefaultFigletFont, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderFigletPlain(string Text, FigletizeFont FigletFont, params object[] Vars) =>
            RenderFiglet(Text, FigletFont, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderFiglet(string Text, Color ForegroundColor, params object[] Vars) =>
            RenderFiglet(Text, FigletTextTools.DefaultFigletFont, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderFiglet(string Text, FigletizeFont FigletFont, Color ForegroundColor, params object[] Vars) =>
            RenderFiglet(Text, FigletFont, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderFiglet(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderFiglet(Text, FigletTextTools.DefaultFigletFont, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderFiglet(string Text, FigletizeFont FigletFont, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderFiglet(Text, FigletFont, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderFiglet(string Text, FigletizeFont FigletFont, Color ForegroundColor, Color BackgroundColor, bool useColor, params object[] Vars)
        {
            var builder = new StringBuilder();
            builder.Append(
                $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                FigletTools.RenderFiglet(Text, FigletFont, Vars) +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) : "")}" +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true) : "")}"
            );
            return builder.ToString();
        }

        static FigletColor()
        {
            if (GeneralColorTools.CheckConsoleOnCall)
                ConsoleChecker.CheckConsole();
        }
    }
}
