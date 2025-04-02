//
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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors;
using Terminaux.Reader;
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using System.Diagnostics;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with input and color support
    /// </summary>
    public static class InfoBoxMultiInputColor
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxPlainMultiInput(string text, params object[] vars) =>
            WriteInfoBoxPlainMultiInput(text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxPlainMultiInput(string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxPlainMultiInput("", text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInput(string text, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColor(string text, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(text, BorderSettings.GlobalSettings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColorBack(string text, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(text, BorderSettings.GlobalSettings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInput(string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColor(string text, BorderSettings settings, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(text, settings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColorBack(string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack("", text, settings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxPlainMultiInput(string title, string text, params object[] vars) =>
            WriteInfoBoxPlainMultiInput(title, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxPlainMultiInput(string title, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(title, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInput(string title, string text, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(title, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColor(string title, string text, Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(title, text, BorderSettings.GlobalSettings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColorBack(string title, string text, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(title, text, BorderSettings.GlobalSettings, InfoBoxTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInput(string title, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(title, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColor(string title, string text, BorderSettings settings, Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(title, text, settings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxMultiInputColorBack(string title, string text, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxMultiInputColorBack(title, text, settings, InfoBoxTitledColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        internal static string WriteInfoBoxMultiInputColorBack(string title, string text, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxInputColor), infoBoxScreenPart);
            try
            {
                int rightMargin = 0;
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 5);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(5, title, text, settings, InfoBoxTitledColor, BackgroundColor, useColor, ref increment, currIdx, false, false, vars)
                    );

                    // Write the input bar and set the cursor position
                    int inputPosX = borderX + 3;
                    rightMargin = inputPosX;
                    int inputPosY = borderY + maxHeight - 3;
                    int maxInputWidth = maxWidth - 6;
                    var border = new Border()
                    {
                        Left = inputPosX,
                        Top = inputPosY,
                        Width = maxInputWidth,
                        Height = 1,
                    };
                    if (useColor)
                    {
                        border.Color = InfoBoxTitledColor;
                        border.BackgroundColor = BackgroundColor;
                    }
                    boxBuffer.Append(
                        border.Render() +
                        CsiSequences.GenerateCsiCursorPosition(inputPosX + 2, inputPosY + 2) +
                        (useColor ? ColorTools.RenderSetConsoleColor(InfoBoxTitledColor) : "") +
                        (useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")
                    );
                    return boxBuffer.ToString();
                });

                // Render the screen
                ScreenTools.Render();

                // Wait until the user presses any key to close the box
                var readerSettings = new TermReaderSettings()
                {
                    RightMargin = rightMargin,
                };
                if (useColor)
                {
                    readerSettings.InputForegroundColor = InfoBoxTitledColor;
                    readerSettings.InputBackgroundColor = BackgroundColor;
                }
                string input = TermReader.Read("", "", readerSettings, false, true);
                return input;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                if (useColor)
                {
                    TextWriterRaw.WriteRaw(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
            return "";
        }

        static InfoBoxMultiInputColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
