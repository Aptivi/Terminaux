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

using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Progress bar writer with color support
    /// </summary>
    public static class ProgressBarColor
    {
        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteProgressPlain(Progress, Left, Top, BorderSettings.GlobalSettings, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, int width, bool DrawBorder = true) =>
            WriteProgressPlain(Progress, Left, Top, width, BorderSettings.GlobalSettings, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, BorderSettings settings, bool DrawBorder = true) =>
            WriteProgressPlain(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, int width, BorderSettings settings, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderProgressPlain(Progress, Left, Top, width, settings, DrawBorder));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, Color ProgressColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, ProgressColor, FrameColor, ColorTools.currentBackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, ProgressColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, ProgressColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, BorderSettings settings, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, BorderSettings settings, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, settings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, BorderSettings settings, Color ProgressColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, BorderSettings settings, Color ProgressColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, settings, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, BorderSettings settings, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, BorderSettings settings, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, settings, ProgressColor, FrameColor, ColorTools.currentBackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="settings">Border settings</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, BorderSettings settings, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, ProgressColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, BorderSettings settings, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderProgress(Progress, Left, Top, width, settings, ProgressColor, FrameColor, BackgroundColor, DrawBorder));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        public static string RenderProgressPlain(double Progress, int Left, int Top, int width, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        public static string RenderProgressPlain(double Progress, int Left, int Top, int width, BorderSettings settings, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, settings, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="ProgressColor">The progress bar color</param>
        public static string RenderProgress(double Progress, int Left, int Top, int width, Color ProgressColor, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, ProgressColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        public static string RenderProgress(double Progress, int Left, int Top, int width, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, ProgressColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        public static string RenderProgress(double Progress, int Left, int Top, int width, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, BorderSettings.GlobalSettings, ProgressColor, FrameColor, BackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="ProgressColor">The progress bar color</param>
        public static string RenderProgress(double Progress, int Left, int Top, int width, BorderSettings settings, Color ProgressColor, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, settings, ProgressColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        public static string RenderProgress(double Progress, int Left, int Top, int width, BorderSettings settings, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, settings, ProgressColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        public static string RenderProgress(double Progress, int Left, int Top, int width, BorderSettings settings, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            RenderProgress(Progress, Left, Top, width, settings, ProgressColor, FrameColor, BackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderProgress(double Progress, int Left, int Top, int width, BorderSettings settings, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool useColor, bool DrawBorder = true)
        {
            try
            {
                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Draw the border
                StringBuilder progBuilder = new();
                if (DrawBorder)
                {
                    if (useColor)
                    {
                        progBuilder.Append(
                            ColorTools.RenderSetConsoleColor(FrameColor) +
                            ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }

                    progBuilder.Append(
                        BoxFrameColor.RenderBoxFrame(Left, Top, width, 1, settings)
                    );
                }

                // Draw the progress bar
                int times = ConsoleMisc.PercentRepeatTargeted((int)Math.Round(Progress), 100, width);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1 + times + 1, Top + 2) + new string(' ', width - times));
                if (useColor)
                    progBuilder.Append(ProgressColor.VTSequenceBackground);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 2) + new string(useColor ? ' ' : '*', times));
                if (useColor)
                {
                    progBuilder.Append(ColorTools.RenderRevertForeground());
                    progBuilder.Append(ColorTools.RenderRevertBackground());
                }
                return progBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static ProgressBarColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
