
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
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Vertical progress bar writer with color support
    /// </summary>
    public static class ProgressBarVerticalColor
    {

        /// <summary>
        /// Writes the vertical progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgressPlain(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteVerticalProgressPlain(Progress, Left, Top, 2, 0, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="HeightOffset">Height offset</param>
        public static void WriteVerticalProgressPlain(double Progress, int Left, int Top, int HeightOffset, bool DrawBorder = true) =>
            WriteVerticalProgressPlain(Progress, Left, Top, HeightOffset, 0, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        public static void WriteVerticalProgressPlain(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true)
        {
            try
            {
                // Get the final height offset
                int FinalHeightOffset = TopHeightOffset + BottomHeightOffset;

                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Fill the progress
                int MaximumHeight = Console.WindowHeight - FinalHeightOffset;
                int ProgressFilled = ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, MaximumHeight);

                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar.ToString() + ProgressTools.ProgressUpperFrameChar + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true);
                    for (int i = 0; i < Console.WindowHeight - FinalHeightOffset; i++)
                        TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " " + ProgressTools.ProgressRightFrameChar, Left, Top + i + 1, true);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar.ToString() + ProgressTools.ProgressLowerFrameChar + ProgressTools.ProgressLowerRightCornerChar, Left, Top + MaximumHeight + 1, true);
                }

                // Draw the progress bar
                for (int i = ProgressFilled; i < MaximumHeight; i++)
                    TextWriterWhereColor.WriteWhere(" ", Left + 1, Top + MaximumHeight - i, true);
                for (int i = 0; i < ProgressFilled; i++)
                    TextWriterWhereColor.WriteWhere("*", Left + 1, Top + MaximumHeight - i, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        public static void WriteVerticalProgress(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, bool DrawBorder = true) =>
             WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Get the final height offset
                int FinalHeightOffset = TopHeightOffset + BottomHeightOffset;

                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Fill the progress
                int MaximumHeight = Console.WindowHeight - FinalHeightOffset;
                int ProgressFilled = ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, MaximumHeight);

                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar.ToString() + ProgressTools.ProgressUpperFrameChar + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true, FrameColor);
                    for (int i = 0; i < Console.WindowHeight - FinalHeightOffset; i++)
                        TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " " + ProgressTools.ProgressRightFrameChar, Left, Top + i + 1, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar.ToString() + ProgressTools.ProgressLowerFrameChar + ProgressTools.ProgressLowerRightCornerChar, Left, Top + MaximumHeight + 1, true, FrameColor);
                }

                // Draw the progress bar
                for (int i = ProgressFilled; i < MaximumHeight; i++)
                    TextWriterWhereColor.WriteWhere(" ", Left + 1, Top + MaximumHeight - i, true);
                ColorTools.SetConsoleColor(new Color(ProgressColor), true, true);
                for (int i = 0; i < ProgressFilled; i++)
                    TextWriterWhereColor.WriteWhere(" ", Left + 1, Top + MaximumHeight - i, true);
                ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        public static void WriteVerticalProgress(double Progress, int Left, int Top, Color ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, Color ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Get the final height offset
                int FinalHeightOffset = TopHeightOffset + BottomHeightOffset;

                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Fill the progress
                int MaximumHeight = Console.WindowHeight - FinalHeightOffset;
                int ProgressFilled = ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, MaximumHeight);

                // Draw the border
                if (DrawBorder)
                {
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressUpperLeftCornerChar.ToString() + ProgressTools.ProgressUpperFrameChar + ProgressTools.ProgressUpperRightCornerChar, Left, Top, true, FrameColor);
                    for (int i = 0; i < Console.WindowHeight - FinalHeightOffset; i++)
                        TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLeftFrameChar + " " + ProgressTools.ProgressRightFrameChar, Left, Top + i + 1, true, FrameColor);
                    TextWriterWhereColor.WriteWhere(ProgressTools.ProgressLowerLeftCornerChar.ToString() + ProgressTools.ProgressLowerFrameChar + ProgressTools.ProgressLowerRightCornerChar, Left, Top + MaximumHeight + 1, true, FrameColor);
                }

                // Draw the progress bar
                for (int i = ProgressFilled; i < MaximumHeight; i++)
                    TextWriterWhereColor.WriteWhere(" ", Left + 1, Top + MaximumHeight - i, true);
                ColorTools.SetConsoleColor(ProgressColor, true, true);
                for (int i = 0; i < ProgressFilled; i++)
                    TextWriterWhereColor.WriteWhere(" ", Left + 1, Top + MaximumHeight - i, true);
                ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

    }
}
