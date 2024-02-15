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
    /// Slider writer with color support
    /// </summary>
    public static class SliderColor
    {
        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderPlain(int currPos, int maxPos, int Left, int Top, bool DrawBorder = true) =>
            WriteSliderPlain(currPos, maxPos, Left, Top, 10, 0, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="WidthOffset">Width offset</param>
        public static void WriteSliderPlain(int currPos, int maxPos, int Left, int Top, int WidthOffset, bool DrawBorder = true) =>
            WriteSliderPlain(currPos, maxPos, Left, Top, WidthOffset, 0, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        public static void WriteSliderPlain(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSliderPlain(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, DrawBorder));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, 10, 0, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int WidthOffset, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, WidthOffset, 0, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, new Color(ConsoleColors.DarkYellow), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, Color SliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, 10, 0, SliderColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int WidthOffset, Color SliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, WidthOffset, 0, SliderColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color SliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, SliderColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, 10, 0, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int WidthOffset, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, WidthOffset, 0, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, SliderColor, FrameColor, ColorTools.currentBackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, 10, 0, SliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="WidthOffset">Width offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int WidthOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, WidthOffset, 0, SliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, SliderColor, FrameColor, BackgroundColor, DrawBorder));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        public static string RenderSliderPlain(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="SliderColor">The slider color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color SliderColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, SliderColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, SliderColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, LeftWidthOffset, RightWidthOffset, SliderColor, FrameColor, BackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="LeftWidthOffset">Width offset from the left</param>
        /// <param name="RightWidthOffset">Width offset from the right</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderSlider(int currPos, int maxPos, int Left, int Top, int LeftWidthOffset, int RightWidthOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool useColor, bool DrawBorder = true)
        {
            try
            {
                // Get the final width offset
                int FinalWidthOffset = LeftWidthOffset + RightWidthOffset;

                // Check the slider value
                if (maxPos < 0)
                    maxPos = 0;
                if (currPos > maxPos)
                    currPos = maxPos;
                if (currPos < 0)
                    currPos = 0;
                int MaximumWidth = ConsoleWrapper.WindowWidth - FinalWidthOffset;
                if (maxPos <= MaximumWidth)
                    return "";

                // Fill the slider
                int maxPosOffset = maxPos - MaximumWidth;
                double maxPosFraction = (double)MaximumWidth / maxPosOffset;
                int times = ConsoleMisc.PercentRepeat((int)Math.Round(maxPosFraction), maxPos, FinalWidthOffset);
                if (times == 0)
                    times = 1;

                // Draw the border
                StringBuilder progBuilder = new();
                if (DrawBorder)
                {
                    if (useColor)
                    {
                        progBuilder.Append(
                            FrameColor.VTSequenceForeground +
                            ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    progBuilder.Append(
                        BorderColor.RenderBorderPlain(
                            Left, Top, ConsoleWrapper.WindowWidth - FinalWidthOffset, 1,
                            ProgressTools.ProgressUpperLeftCornerChar, ProgressTools.ProgressLowerLeftCornerChar,
                            ProgressTools.ProgressUpperRightCornerChar, ProgressTools.ProgressLowerRightCornerChar,
                            ProgressTools.ProgressUpperFrameChar, ProgressTools.ProgressLowerFrameChar,
                            ProgressTools.ProgressLeftFrameChar, ProgressTools.ProgressRightFrameChar
                        )
                    );
                }
                else
                {
                    progBuilder.Append(
                        BoxColor.RenderBox(Left + 1, Top, ConsoleWrapper.WindowWidth - FinalWidthOffset, 1, BackgroundColor, useColor)
                    );
                }

                // Draw the slider
                int maxBlanks = ConsoleMisc.PercentRepeatTargeted(currPos, maxPos, MaximumWidth - times + 1);
                if (maxBlanks == MaximumWidth - times + 1)
                    maxBlanks--;
                if (useColor)
                    progBuilder.Append(SliderColor.VTSequenceBackground);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + maxBlanks + 2, Top + 2) + new string(useColor ? ' ' : '*', times));
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

        static SliderColor()
        {
            ConsoleChecker.CheckConsole();
        }
    }
}
