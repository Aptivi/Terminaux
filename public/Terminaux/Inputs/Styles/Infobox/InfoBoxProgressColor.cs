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
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer;
using Colorimetry.Transformation;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;
using Terminaux.Base.Extensions;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with progress and color support
    /// </summary>
    public static class InfoBoxProgressColor
    {
        /// <summary>
        /// Writes the progress info box
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static InfoBox WriteInfoBoxProgress(double progress, string text, params object[] vars) =>
            WriteInfoBoxProgress(progress, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the progress info box
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static InfoBox WriteInfoBoxProgress(double progress, string text, InfoBoxSettings settings, params object[] vars)
        {
            // Prepare the screen
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxProgressColor), infoBoxScreenPart);

            // Make a new infobox instance
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = 1;

            // Render it
            try
            {
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Fill the info box with text inside it
                    infoBox.Elements.RemoveRenderables();
                    var (maxWidth, maxHeight, _, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;
                    var boxBuffer = new StringBuilder(
                        infoBox.Render(ref increment, currIdx, false, false)
                    );

                    // Render the final result and write the progress bar
                    int progressPosX = borderX + 1;
                    int progressPosY = borderY + maxHeight - 3;
                    int maxProgressWidth = maxWidth - 2;
                    var progressBar = new SimpleProgress((int)progress, 100)
                    {
                        Width = maxProgressWidth,
                    };
                    if (settings.UseColors)
                    {
                        progressBar.ProgressPercentageTextColor = settings.ForegroundColor;
                        progressBar.ProgressActiveForegroundColor = settings.ForegroundColor;
                        progressBar.ProgressForegroundColor = TransformationTools.GetDarkBackground(settings.ForegroundColor);
                        progressBar.ProgressBackgroundColor = settings.BackgroundColor;
                    }
                    boxBuffer.Append(RendererTools.RenderRenderable(progressBar, new(progressPosX + 1, progressPosY + 3)));
                    return boxBuffer.ToString();
                });

                // Render the screen
                ScreenTools.Render();
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
            return infoBox;
        }
    }
}
