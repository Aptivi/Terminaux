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
using Terminaux.Colors;
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.CyclicWriters.Simple;

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
        public static void WriteInfoBoxProgress(double progress, string text, params object[] vars) =>
            WriteInfoBoxProgress(progress, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the progress info box
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxProgress(double progress, string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxProgressInternal(settings.Title, progress, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, vars);

        #region To be removed
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressPlain(double progress, string text, params object[] vars) =>
            WriteInfoBoxProgressPlain(progress, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressPlain(double progress, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxProgressPlain("", progress, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColor(double progress, string text, Color InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, BorderSettings.GlobalSettings, InfoBoxProgressColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="BackgroundColor">InfoBoxProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColorBack(double progress, string text, Color InfoBoxProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, BorderSettings.GlobalSettings, InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgress(double progress, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, settings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColor(double progress, string text, BorderSettings settings, Color InfoBoxProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(progress, text, settings, InfoBoxProgressColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxProgressColor">InfoBoxProgress color</param>
        /// <param name="BackgroundColor">InfoBoxProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColorBack(double progress, string text, BorderSettings settings, Color InfoBoxProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack("", progress, text, settings, InfoBoxProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressPlain(string title, double progress, string text, params object[] vars) =>
            WriteInfoBoxProgressPlain(title, progress, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgress(string title, double progress, string text, params object[] vars) =>
            WriteInfoBoxProgressColorBack(title, progress, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColor(string title, double progress, string text, Color InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(title, progress, text, BorderSettings.GlobalSettings, InfoBoxTitledProgressColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColorBack(string title, double progress, string text, Color InfoBoxTitledProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(title, progress, text, BorderSettings.GlobalSettings, InfoBoxTitledProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgress(string title, double progress, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxProgressColorBack(title, progress, text, settings, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColor(string title, double progress, string text, BorderSettings settings, Color InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxProgressColorBack(title, progress, text, settings, InfoBoxTitledProgressColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxProgressColorBack(string title, double progress, string text, BorderSettings settings, Color InfoBoxTitledProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxProgressInternal(title, progress, text, settings, InfoBoxTitledProgressColor, BackgroundColor, true, vars);
        #endregion

        internal static void WriteInfoBoxProgressInternal(string title, double progress, string text, BorderSettings settings, Color InfoBoxTitledProgressColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxProgressColor), infoBoxScreenPart);
            try
            {
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 1);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(1, title, text, settings, InfoBoxTitledProgressColor, BackgroundColor, useColor, ref increment, currIdx, false, false, vars)
                    );

                    // Render the final result and write the progress bar
                    int progressPosX = borderX + 1;
                    int progressPosY = borderY + maxHeight - 3;
                    int maxProgressWidth = maxWidth - 2;
                    var progressBar = new SimpleProgress((int)progress, 100)
                    {
                        Width = maxProgressWidth,
                    };
                    if (useColor)
                    {
                        progressBar.ProgressPercentageTextColor = InfoBoxTitledProgressColor;
                        progressBar.ProgressActiveForegroundColor = InfoBoxTitledProgressColor;
                        progressBar.ProgressForegroundColor = TransformationTools.GetDarkBackground(InfoBoxTitledProgressColor);
                        progressBar.ProgressBackgroundColor = BackgroundColor;
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
        }

        static InfoBoxProgressColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
