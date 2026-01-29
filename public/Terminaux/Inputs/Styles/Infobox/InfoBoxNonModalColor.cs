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
using System.Threading;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Textify.General;
using Terminaux.Base.Extensions;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with color support
    /// </summary>
    public static class InfoBoxNonModalColor
    {
        /// <summary>
        /// Writes the info box that just appears
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, params object[] vars) =>
            WriteInfoBox(text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box that just appears
        /// </summary>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, InfoBoxSettings settings, params object[] vars)
        {
            // Prepare the screen
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxNonModalColor), infoBoxScreenPart);

            // Make a new infobox instance
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = 0;

            // Render it
            try
            {
                // Draw the border and the text
                int currIdx = 0;
                int increment = 0;
                bool bail = false;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    return infoBox.Erase() + infoBox.Render(ref increment, currIdx, true, false);
                });

                // Main loop
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Wait until the user presses any key to close the box
                    var (_, maxHeight, _, _, _, _, linesLength) = infoBox.Dimensions;
                    if (currIdx < linesLength - maxHeight)
                    {
                        Thread.Sleep(5000);
                        bail = currIdx == linesLength - maxHeight;
                        currIdx += increment;
                        if (currIdx > linesLength - maxHeight)
                            currIdx = linesLength - maxHeight;
                    }
                    else
                        bail = true;
                }
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
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
        }
    }
}
