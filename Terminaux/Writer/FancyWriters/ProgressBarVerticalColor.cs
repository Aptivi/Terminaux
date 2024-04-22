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
                TextWriterRaw.WriteRaw(RenderVerticalProgressPlain(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, DrawBorder));
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
        public static void WriteVerticalProgress(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, bool DrawBorder = true) =>
             WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

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
            WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

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
                TextWriterRaw.WriteRaw(RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, FrameColor, DrawBorder));
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
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, FrameColor, BackgroundColor, DrawBorder));
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
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        public static string RenderVerticalProgressPlain(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true) =>
            RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, false, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="ProgressColor">The progress bar color</param>
        public static string RenderVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, bool DrawBorder = true) =>
            RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        public static string RenderVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        public static string RenderVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, FrameColor, BackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, Color FrameColor, Color BackgroundColor, bool useColor, bool DrawBorder = true)
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
                int MaximumHeight = ConsoleWrapper.WindowHeight - FinalHeightOffset;
                int ProgressFilled = ConsoleMisc.PercentRepeatTargeted((int)Math.Round(Progress), 100, MaximumHeight);

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
                    progBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)}");
                    progBuilder.Append($"{ProgressTools.ProgressUpperLeftCornerChar}{ProgressTools.ProgressUpperFrameChar}{ProgressTools.ProgressUpperRightCornerChar}");
                    for (int i = 0; i < ConsoleWrapper.WindowHeight - FinalHeightOffset; i++)
                    {
                        progBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + i + 2)}");
                        progBuilder.Append(ProgressTools.ProgressLeftFrameChar + " " + ProgressTools.ProgressRightFrameChar);
                    }
                    progBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + MaximumHeight + 2)}");
                    progBuilder.Append(ProgressTools.ProgressLowerLeftCornerChar.ToString() + ProgressTools.ProgressLowerFrameChar + ProgressTools.ProgressLowerRightCornerChar);
                }

                // Draw the progress bar
                for (int i = ProgressFilled; i < MaximumHeight; i++)
                {
                    progBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + MaximumHeight - i + 1)}");
                    progBuilder.Append(' ');
                }
                if (useColor)
                    progBuilder.Append($"{ProgressColor.VTSequenceBackground}");
                for (int i = 0; i < ProgressFilled; i++)
                {
                    progBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + MaximumHeight - i + 1)}");
                    progBuilder.Append(' ');
                }
                if (useColor)
                {
                    progBuilder.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor));
                    progBuilder.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true));
                }

                // Render to the console
                return progBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static ProgressBarVerticalColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
