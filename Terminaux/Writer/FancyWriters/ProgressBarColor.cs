﻿
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

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters.Tools;

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
                // Get the final width offset
                int FinalWidthOffset = LeftWidthOffset + RightWidthOffset;

                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Draw the border
                if (DrawBorder)
                {
                    BoxFrameColor.WriteBoxFramePlain(Left, Top, ConsoleWrappers.ActionWindowWidth() - FinalWidthOffset, 1,
                        ProgressTools.ProgressUpperLeftCornerChar, ProgressTools.ProgressLowerLeftCornerChar,
                        ProgressTools.ProgressUpperRightCornerChar, ProgressTools.ProgressLowerRightCornerChar,
                        ProgressTools.ProgressUpperFrameChar, ProgressTools.ProgressLowerFrameChar,
                        ProgressTools.ProgressLeftFrameChar, ProgressTools.ProgressRightFrameChar);
                }

                // Draw the progress bar
                int times = Targeted ?
                    ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, FinalWidthOffset) :
                    ConsoleExtensions.PercentRepeat((int)Math.Round(Progress), 100, FinalWidthOffset);
                StringBuilder progBuilder = new();
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1 + times + 1, Top + 2) + new string(' ', ConsoleWrappers.ActionWindowWidth() - FinalWidthOffset - times));
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 2) + new string('*', times));
                TextWriterColor.WritePlain(progBuilder.ToString());
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
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
            WriteProgress(Progress, Left, Top, 10, 0, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder, Targeted);

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
            WriteProgress(Progress, Left, Top, WidthOffset, 0, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder, Targeted);

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
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder, Targeted);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="Targeted">Targeted percentage?</param>
        public static void WriteProgress(double Progress, int Left, int Top, ConsoleColors ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, 10, 0, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder, Targeted);

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
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, ConsoleColors ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, WidthOffset, 0, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder, Targeted);

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
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, ConsoleColors ProgressColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder, Targeted);

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
        public static void WriteProgress(double Progress, int Left, int Top, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true, bool Targeted = false) =>
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
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true, bool Targeted = false) =>
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
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true, bool Targeted = false) =>
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, FrameColor, ConsoleColors.Black, DrawBorder, Targeted);

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
        public static void WriteProgress(double Progress, int Left, int Top, ConsoleColors ProgressColor, ConsoleColors FrameColor, ConsoleColors BackgroundColor, bool DrawBorder = true, bool Targeted = false) =>
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
        public static void WriteProgress(double Progress, int Left, int Top, int WidthOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, ConsoleColors BackgroundColor, bool DrawBorder = true, bool Targeted = false) =>
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
        public static void WriteProgress(double Progress, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, ConsoleColors BackgroundColor, bool DrawBorder = true, bool Targeted = false)
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
                if (DrawBorder)
                {
                    BoxFrameColor.WriteBoxFrame(Left, Top, ConsoleWrappers.ActionWindowWidth() - FinalWidthOffset, 1,
                        ProgressTools.ProgressUpperLeftCornerChar, ProgressTools.ProgressLowerLeftCornerChar,
                        ProgressTools.ProgressUpperRightCornerChar, ProgressTools.ProgressLowerRightCornerChar,
                        ProgressTools.ProgressUpperFrameChar, ProgressTools.ProgressLowerFrameChar,
                        ProgressTools.ProgressLeftFrameChar, ProgressTools.ProgressRightFrameChar,
                        FrameColor, BackgroundColor);
                }

                // Draw the progress bar
                int times = Targeted ?
                            ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, FinalWidthOffset) :
                            ConsoleExtensions.PercentRepeat((int)Math.Round(Progress), 100, FinalWidthOffset);
                StringBuilder progBuilder = new();
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1 + times + 1, Top + 2) + new string(' ', ConsoleWrappers.ActionWindowWidth() - FinalWidthOffset - times));
                progBuilder.Append(new Color(ProgressColor).VTSequenceBackground);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 2) + new string(' ', times));
                progBuilder.Append(new Color(ConsoleColors.Black).VTSequenceBackground);
                TextWriterColor.WritePlain(progBuilder.ToString());
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
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
            WriteProgress(Progress, Left, Top, LeftWidthOffset, RightWidthOffset, ProgressColor, FrameColor, new Color(ConsoleColors.Black), DrawBorder, Targeted);

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
                // Get the final width offset
                int FinalWidthOffset = LeftWidthOffset + RightWidthOffset;

                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Draw the border
                if (DrawBorder)
                {
                    BoxFrameColor.WriteBoxFrame(Left, Top, ConsoleWrappers.ActionWindowWidth() - FinalWidthOffset, 1,
                        ProgressTools.ProgressUpperLeftCornerChar, ProgressTools.ProgressLowerLeftCornerChar,
                        ProgressTools.ProgressUpperRightCornerChar, ProgressTools.ProgressLowerRightCornerChar,
                        ProgressTools.ProgressUpperFrameChar, ProgressTools.ProgressLowerFrameChar,
                        ProgressTools.ProgressLeftFrameChar, ProgressTools.ProgressRightFrameChar,
                        FrameColor, BackgroundColor);
                }

                // Draw the progress bar
                int times = Targeted ?
                            ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, FinalWidthOffset) :
                            ConsoleExtensions.PercentRepeat((int)Math.Round(Progress), 100, FinalWidthOffset);
                StringBuilder progBuilder = new();
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1 + times + 1, Top + 2) + new string(' ', ConsoleWrappers.ActionWindowWidth() - FinalWidthOffset - times));
                progBuilder.Append(ProgressColor.VTSequenceBackground);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 2) + new string(' ', times));
                progBuilder.Append(new Color(ConsoleColors.Black).VTSequenceBackground);
                TextWriterColor.WritePlain(progBuilder.ToString());
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

    }
}
