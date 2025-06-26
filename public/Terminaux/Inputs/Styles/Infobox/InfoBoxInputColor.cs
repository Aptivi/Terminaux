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
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Textify.General.Structures;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with input and color support
    /// </summary>
    public static class InfoBoxInputColor
    {
        /// <summary>
        /// Writes the input info box
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, params object[] vars) =>
            WriteInfoBoxInput(text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the input info box
        /// </summary>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxInputInternal(settings.Title, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, false, false, vars);

        /// <summary>
        /// Writes the masked input info box
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInputPassword(string text, params object[] vars) =>
            WriteInfoBoxInputPassword(text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the masked input info box
        /// </summary>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInputPassword(string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxInputInternal(settings.Title, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, true, false, vars);

        /// <summary>
        /// Writes the single-character input info box
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInputChar(string text, params object[] vars) =>
            WriteInfoBoxInputChar(text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the single-character input info box
        /// </summary>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInputChar(string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxInputInternal(settings.Title, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, false, true, vars);

        internal static string WriteInfoBoxInputInternal(string title, string text, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, bool useColor, bool password, bool character, params object[] vars)
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
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 3);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(3, title, text, settings, InfoBoxTitledColor, BackgroundColor, useColor, ref increment, currIdx, false, false, vars)
                    );

                    // Write the input bar and set the cursor position
                    int maxInputWidth = maxWidth - 4;
                    int inputPosX = borderX + 2;
                    int inputEndPosX = inputPosX + maxInputWidth;
                    int inputPosY = borderY + maxHeight - 2;
                    rightMargin = Console.WindowWidth - inputEndPosX - 2;
                    var border = new Border()
                    {
                        Left = inputPosX,
                        Top = inputPosY,
                        Width = maxInputWidth,
                        Height = 1,
                        UseColors = useColor,
                        Color = InfoBoxTitledColor,
                        BackgroundColor = BackgroundColor,
                    };
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
                string input = TermReader.Read("", "", readerSettings, password, true);
                if (character)
                {
                    WideString wideInput = (WideString)input;
                    if (wideInput.Length > 0)
                        return $"{wideInput[0]}";
                    return input;
                }
                else
                    return input;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
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

        static InfoBoxInputColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
