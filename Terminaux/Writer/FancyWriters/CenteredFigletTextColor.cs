
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

using Figletize;
using Figletize.Utilities;
using System;
using System.Diagnostics;
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Centered Figlet writer
    /// </summary>
    public static class CenteredFigletTextColor
    {

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, params object[] Vars)
        {
            try
            {
                Text = ConsoleExtensions.FormatString(Text, Vars);
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
                FigletWhereColor.WriteFigletWhere(Text, consoleX, top, true, FigletFont, new Color(ConsoleColors.Gray), Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, new Color(Color), new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, Color, new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = ConsoleExtensions.FormatString(Text, Vars);
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
                FigletWhereColor.WriteFigletWhere(Text, consoleX, top, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, params object[] Vars)
        {
            try
            {
                Text = ConsoleExtensions.FormatString(Text, Vars);
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
                int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
                int consoleY = ConsoleWrappers.ActionWindowHeight() / 2 - figHeight;
                FigletWhereColor.WriteFigletWhere(Text, consoleX, consoleY, true, FigletFont, new Color(ConsoleColors.Gray), Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, new Color(Color), new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, Color, new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = ConsoleExtensions.FormatString(Text, Vars);
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
                int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
                int consoleY = ConsoleWrappers.ActionWindowHeight() / 2 - figHeight;
                FigletWhereColor.WriteFigletWhere(Text, consoleX, consoleY, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

    }
}
