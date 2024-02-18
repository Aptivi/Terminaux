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
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgressPlain(Progress, Left, Top, 10, 0, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, int WidthOffset, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgressPlain(Progress, Left, Top, WidthOffset, 0, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgressPlain(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, bool DrawBorder = true, bool Targeted = false)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderProgressPlain(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, DrawBorder, Targeted));
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
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, 10, 0, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, WidthOffset, 0, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, 10, 0, ProgressColor, ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, Color ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, WidthOffset, 0, ProgressColor, ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, 10, 0, ProgressColor, FrameColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, WidthOffset, 0, ProgressColor, FrameColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, FrameColor, ColorTools.currentBackgroundColor, DrawBorder, Targeted);

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
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, 10, 0, ProgressColor, FrameColor, BackgroundColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, WidthOffset, 0, ProgressColor, FrameColor, BackgroundColor, DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true, bool Targeted = false)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, FrameColor, BackgroundColor, DrawBorder, Targeted));
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
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static string RenderProgressPlain(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, bool DrawBorder = true, bool Targeted = false) =>
            RenderProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder, Targeted);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static string RenderProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            RenderProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder, Targeted);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static string RenderProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            RenderProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder, Targeted);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static string RenderProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true, bool Targeted = false) =>
            RenderProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, FrameColor, BackgroundColor, true, DrawBorder, Targeted);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        internal static string RenderProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool useColor, bool DrawBorder = true, bool Targeted = false)
        {
            try
            {
                // Get the final width offset
                int FinalWidthOffset = LeftWidthOffset + RightWidthOffset;

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
                        BoxFrameColor.RenderBoxFrame(Left, Top, ConsoleWrapper.WindowWidth - FinalWidthOffset, 1,
                            ProgressTools.ProgressUpperLeftCornerChar, ProgressTools.ProgressLowerLeftCornerChar,
                            ProgressTools.ProgressUpperRightCornerChar, ProgressTools.ProgressLowerRightCornerChar,
                            ProgressTools.ProgressUpperFrameChar, ProgressTools.ProgressLowerFrameChar,
                            ProgressTools.ProgressLeftFrameChar, ProgressTools.ProgressRightFrameChar)
                    );
                }

                // Draw the progress bar
                int times = Targeted ?
                    ConsoleMisc.PercentRepeatTargeted((int)Math.Round(Progress), 100, FinalWidthOffset) :
                    ConsoleMisc.PercentRepeat((int)Math.Round(Progress), 100, FinalWidthOffset);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1 + times + 1, Top + 2) + new string(' ', ConsoleWrapper.WindowWidth - FinalWidthOffset - times));
                if (useColor)
                    progBuilder.Append(ProgressColor.VTSequenceBackground);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 2) + new string(useColor ? ' ' : '*', times));
                if (useColor)
                {
                    progBuilder.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor));
                    progBuilder.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true));
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
            ConsoleChecker.CheckConsole();
        }
    }
}
