
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
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

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
            Text = ConsoleExtensions.FormatString(Text, Vars);
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont);
            int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback);
            int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
            int consoleMaxY = top + figHeight;
            if (consoleX < 0 || consoleMaxY > ConsoleWrappers.ActionWindowHeight())
            {
                // The figlet won't fit, so use small text
                consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - figWidthFallback;
                consoleMaxY = top + figHeightFallback;
                if (consoleX < 0 || consoleMaxY > ConsoleWrappers.ActionWindowHeight())
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - (Text.Length / 2);
                    TextWriterWhereColor.WriteWhereColor(Text, consoleX, top, true, new Color(ConsoleColors.Gray), Vars);
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereColor(Text, consoleX, top, true, figFontFallback, new Color(ConsoleColors.Gray), Vars);
                }
            }
            else
            {
                // Write the figlet.
                FigletWhereColor.WriteFigletWhereColor(Text, consoleX, top, true, FigletFont, new Color(ConsoleColors.Gray), Vars);
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
        public static void WriteCenteredFigletColor(int top, FigletizeFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, new Color(Color), new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(int top, FigletizeFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColor(int top, FigletizeFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, Color, new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(int top, FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            Text = ConsoleExtensions.FormatString(Text, Vars);
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont);
            int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback);
            int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
            int consoleMaxY = top + figHeight;
            if (consoleX < 0 || consoleMaxY > ConsoleWrappers.ActionWindowHeight())
            {
                // The figlet won't fit, so use small text
                consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - figWidthFallback;
                consoleMaxY = top + figHeightFallback;
                if (consoleX < 0 || consoleMaxY > ConsoleWrappers.ActionWindowHeight())
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - (Text.Length / 2);
                    TextWriterWhereColor.WriteWhereColorBack(Text, consoleX, top, true, ForegroundColor, BackgroundColor, Vars);
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, top, true, figFontFallback, ForegroundColor, BackgroundColor, Vars);
                }
            }
            else
            {
                // Write the figlet.
                FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, top, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
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
            Text = ConsoleExtensions.FormatString(Text, Vars);
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
            int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
            int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
            int consoleY = ConsoleWrappers.ActionWindowHeight() / 2 - figHeight;
            if (consoleX < 0 || consoleY < 0)
            {
                // The figlet won't fit, so use small text
                consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - figWidthFallback;
                consoleY = (ConsoleWrappers.ActionWindowHeight() / 2) - figHeightFallback;
                if (consoleX < 0 || consoleY < 0)
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - (Text.Length / 2);
                    consoleY = ConsoleWrappers.ActionWindowHeight() / 2;
                    TextWriterWhereColor.WriteWhereColor(Text, consoleX, consoleY, true, new Color(ConsoleColors.Gray), Vars);
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereColor(Text, consoleX, consoleY, true, figFontFallback, new Color(ConsoleColors.Gray), Vars);
                }
            }
            else
            {
                // Write the figlet.
                FigletWhereColor.WriteFigletWhereColor(Text, consoleX, consoleY, true, FigletFont, new Color(ConsoleColors.Gray), Vars);
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColor(FigletizeFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, new Color(Color), new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(FigletizeFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColor(FigletizeFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, Color, new Color(ConsoleColors.Black), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            Text = ConsoleExtensions.FormatString(Text, Vars);
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
            int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
            int consoleX = ConsoleWrappers.ActionWindowWidth() / 2 - figWidth;
            int consoleY = ConsoleWrappers.ActionWindowHeight() / 2 - figHeight;
            if (consoleX < 0 || consoleY < 0)
            {
                // The figlet won't fit, so use small text
                consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - figWidthFallback;
                consoleY = (ConsoleWrappers.ActionWindowHeight() / 2) - figHeightFallback;
                if (consoleX < 0 || consoleY < 0)
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = (ConsoleWrappers.ActionWindowWidth() / 2) - (Text.Length / 2);
                    consoleY = ConsoleWrappers.ActionWindowHeight() / 2;
                    TextWriterWhereColor.WriteWhereColorBack(Text, consoleX, consoleY, true, ForegroundColor, BackgroundColor, Vars);
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, consoleY, true, figFontFallback, ForegroundColor, BackgroundColor, Vars);
                }
            }
            else
            {
                // Write the figlet.
                FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, consoleY, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
            }
        }

    }
}
