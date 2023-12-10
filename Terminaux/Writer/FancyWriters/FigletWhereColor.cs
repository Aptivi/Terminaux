﻿//
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
using System.Threading;
using Terminaux.Colors;
using Figletize.Utilities;
using Figletize;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Figlet writer (positional)
    /// </summary>
    public static class FigletWhereColor
    {

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWherePlain(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, params object[] Vars)
        {
            try
            {
                TextWriterColor.WritePlain(RenderFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

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
        public static void WriteFigletWhereColor(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, ConsoleColors Color, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(new Color(Color));

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

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
        public static void WriteFigletWhereColorBack(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(new Color(ForegroundColor));
                ColorTools.SetConsoleColor(new Color(BackgroundColor), true);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

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
        public static void WriteFigletWhereColor(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, Color Color, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(Color);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

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
        public static void WriteFigletWhereColorBack(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(ForegroundColor);
                ColorTools.SetConsoleColor(BackgroundColor, true);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderFigletWherePlain(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, params object[] Vars)
        {
            Text = FigletTools.RenderFiglet(Text, FigletFont, Vars);
            var builder = new StringBuilder();
            builder.Append(
                TextWriterWhereColor.RenderWherePlain(Text, Left, Top, Return, Vars)
            );
            return builder.ToString();
        }

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
        public static string RenderFigletWhere(string Text, int Left, int Top, bool Return, FigletizeFont FigletFont, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            Text = FigletTools.RenderFiglet(Text, FigletFont, Vars);
            var builder = new StringBuilder();
            builder.Append(
                ForegroundColor.VTSequenceForeground +
                BackgroundColor.VTSequenceBackground +
                TextWriterWhereColor.RenderWherePlain(Text, Left, Top, Return, Vars) +
                ColorTools.currentForegroundColor.VTSequenceForeground +
                ColorTools.currentBackgroundColor.VTSequenceBackground
            );
            return builder.ToString();
        }

    }
}
