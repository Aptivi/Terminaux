//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using Terminaux.Reader;
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Textify.General.Structures;
using Textify.General;
using Terminaux.Base.Extensions;

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
        /// <param name="inputType">Input type</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, InfoBoxInputType inputType = InfoBoxInputType.Text, params object[] vars) =>
            WriteInfoBoxInput("", text, InfoBoxSettings.GlobalSettings, inputType, vars);

        /// <summary>
        /// Writes the input info box
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="inputType">Input type</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, InfoBoxSettings settings, InfoBoxInputType inputType = InfoBoxInputType.Text, params object[] vars) =>
            WriteInfoBoxInput("", text, settings, inputType, vars);

        /// <summary>
        /// Writes the input info box
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="inputType">Input type</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string initialValue, string text, InfoBoxInputType inputType = InfoBoxInputType.Text, params object[] vars) =>
            WriteInfoBoxInput(initialValue, text, InfoBoxSettings.GlobalSettings, inputType, vars);

        /// <summary>
        /// Writes the input info box
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="inputType">Input type</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string initialValue, string text, InfoBoxSettings settings, InfoBoxInputType inputType = InfoBoxInputType.Text, params object[] vars)
        {
            // Prepare the screen
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxInputColor), infoBoxScreenPart);

            // Make a new infobox instance
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = 3;

            // Render it
            bool password = inputType == InfoBoxInputType.Password;
            bool character = inputType == InfoBoxInputType.Character;
            try
            {
                int width = 0;
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Fill the info box with text inside it
                    infoBox.Elements.RemoveRenderables();
                    var (maxWidth, maxHeight, _, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;

                    // Prepare the input bar
                    int maxInputWidth = maxWidth - 4;
                    int inputPosX = borderX + 2;
                    int inputEndPosX = inputPosX + maxInputWidth;
                    int inputPosY = borderY + maxHeight - 2;
                    width = maxInputWidth;
                    var border = new Border()
                    {
                        Left = inputPosX,
                        Top = inputPosY,
                        Width = maxInputWidth,
                        Height = 1,
                        UseColors = settings.UseColors,
                        Color = settings.ForegroundColor,
                        BackgroundColor = settings.BackgroundColor,
                    };
                    infoBox.Elements.AddRenderable("Input box", border);

                    // Write the input bar and set the cursor position
                    var boxBuffer = new StringBuilder(infoBox.Render(ref increment, currIdx, false, false));
                    boxBuffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(inputPosX + 2, inputPosY + 2) +
                        (settings.UseColors ? ConsoleColoring.RenderSetConsoleColor(settings.ForegroundColor) : "") +
                        (settings.UseColors ? ConsoleColoring.RenderSetConsoleColor(settings.BackgroundColor, true) : "")
                    );
                    return boxBuffer.ToString();
                });

                // Render the screen
                ScreenTools.Render();

                // Wait until the user presses any key to close the box
                var readerSettings = new TermReaderSettings()
                {
                    Width = width,
                    WriteDefaultValue = true,
                    PasswordMaskChar = settings.PasswordMaskChar,
                };
                if (settings.UseColors)
                {
                    readerSettings.InputForegroundColor = settings.ForegroundColor;
                    readerSettings.InputBackgroundColor = settings.BackgroundColor;
                }
                string input = TermReader.Read("", initialValue, readerSettings, password, true);
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
                if (settings.UseColors)
                {
                    TextWriterRaw.WriteRaw(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
                    );
                }
                TextWriterRaw.WriteRaw(infoBox.Erase());
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
            return "";
        }
    }
}
