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

using Textify.Figlet;
using System;
using System.Diagnostics;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Textify.Figlet.Utilities.Lines;
using System.Linq;

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
        public static void WriteCenteredFiglet(int top, FigletFont FigletFont, string Text, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredFiglet(top, FigletFont, Text, Vars));
            }
            catch (Exception ex)
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
        public static void WriteCenteredFigletColor(int top, FigletFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredFiglet(top, FigletFont, Text, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex)
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
        public static void WriteCenteredFiglet(FigletFont FigletFont, string Text, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredFiglet(FigletFont, Text, Vars));
            }
            catch (Exception ex)
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
        public static void WriteCenteredFigletColor(FigletFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, Color, ColorTools.currentBackgroundColor, Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderCenteredFiglet(FigletFont, Text, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(FigletFont FigletFont, string Text, params object[] Vars) =>
            RenderCenteredFiglet(FigletFont, Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(FigletFont FigletFont, string Text, Color ForegroundColor, params object[] Vars) =>
            RenderCenteredFiglet(FigletFont, Text, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderCenteredFiglet(FigletFont, Text, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderCenteredFiglet(FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            return RenderCenteredFiglet(consoleY, FigletFont, Text, ForegroundColor, BackgroundColor, useColor, Vars);
        }

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(int top, FigletFont FigletFont, string Text, params object[] Vars) =>
            RenderCenteredFiglet(top, FigletFont, Text, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, Vars);

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(int top, FigletFont FigletFont, string Text, Color ForegroundColor, params object[] Vars) =>
            RenderCenteredFiglet(top, FigletFont, Text, ForegroundColor, ColorTools.currentBackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderCenteredFiglet(top, FigletFont, Text, ForegroundColor, BackgroundColor, true, Vars);

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        internal static string RenderCenteredFiglet(int top, FigletFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, bool useColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figBuilder = new StringBuilder();
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont);
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback);
                int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int consoleMaxY = top + figHeight;
                if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                {
                    // The figlet won't fit, so use small text
                    consoleX = ConsoleWrapper.WindowWidth / 2 - figWidthFallback;
                    consoleMaxY = top + figHeightFallback;
                    if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        figBuilder.Append(
                            $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                            $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                            CenteredTextColor.RenderCentered(top, Text, Vars)
                        );
                    }
                    else
                    {
                        // Write the figlet.
                        figBuilder.Append(
                            $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                            $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                            FigletWhereColor.RenderFigletWherePlain(Text, consoleX, top, true, figFontFallback, Vars)
                        );
                    }
                }
                else
                {
                    // Write the figlet.
                    figBuilder.Append(
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "")}" +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                        FigletWhereColor.RenderFigletWherePlain(Text, consoleX, top, true, FigletFont, Vars)
                    );
                }

                // Write the resulting buffer
                if (useColor)
                {
                    figBuilder.Append(
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) +
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
                    );
                }
                return figBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static CenteredFigletTextColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
