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
    /// Vertical slider writer with color support
    /// </summary>
    public static class SliderVerticalColor
    {
        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSliderPlain(int currPos, int maxPos, int Left, int Top, bool DrawBorder = true) =>
            WriteVerticalSliderPlain(currPos, maxPos, Left, Top, 2, 0, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="HeightOffset">Height offset</param>
        public static void WriteVerticalSliderPlain(int currPos, int maxPos, int Left, int Top, int HeightOffset, bool DrawBorder = true) =>
            WriteVerticalSliderPlain(currPos, maxPos, Left, Top, HeightOffset, 0, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        public static void WriteVerticalSliderPlain(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderVerticalSliderPlain(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, DrawBorder));
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
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, 2, 0, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int HeightOffset, bool DrawBorder = true) =>
             WriteVerticalSlider(currPos, maxPos, Left, Top, HeightOffset, 0, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, Color SliderColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, 2, 0, SliderColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int HeightOffset, Color SliderColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, HeightOffset, 0, SliderColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color SliderColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, SliderColor, ColorTools.GetGray(), DrawBorder);

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
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, 2, 0, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int HeightOffset, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, HeightOffset, 0, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color SliderColor, Color FrameColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, SliderColor, FrameColor, DrawBorder));
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
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, 2, 0, SliderColor, FrameColor, BackgroundColor, DrawBorder);

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
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int HeightOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, HeightOffset, 0, SliderColor, FrameColor, BackgroundColor, DrawBorder);

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
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, SliderColor, FrameColor, BackgroundColor, DrawBorder));
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
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        public static string RenderVerticalSliderPlain(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true) =>
            RenderVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, false, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="SliderColor">The slider color</param>
        public static string RenderVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color SliderColor, bool DrawBorder = true) =>
            RenderVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, SliderColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        public static string RenderVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            RenderVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, SliderColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static string RenderVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            RenderVerticalSlider(currPos, maxPos, Left, Top, TopHeightOffset, BottomHeightOffset, SliderColor, FrameColor, BackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderVerticalSlider(int currPos, int maxPos, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color SliderColor, Color FrameColor, Color BackgroundColor, bool useColor, bool DrawBorder = true)
        {
            try
            {
                // Get the final height offset
                int FinalHeightOffset = TopHeightOffset + BottomHeightOffset;

                // Check the slider value
                if (maxPos < 0)
                    maxPos = 0;
                if (currPos > maxPos)
                    currPos = maxPos;
                if (currPos < 0)
                    currPos = 0;
                int MaximumHeight = ConsoleWrapper.WindowHeight - FinalHeightOffset;
                if (maxPos <= MaximumHeight)
                    return "";

                // Fill the slider
                int maxPosOffset = maxPos - MaximumHeight;
                double maxPosFraction = (double)MaximumHeight / maxPosOffset;
                int SliderFilled = ConsoleMisc.PercentRepeatTargeted((int)Math.Round(maxPosFraction), maxPos, MaximumHeight);
                if (SliderFilled == 0)
                    SliderFilled = 1;

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
                        BorderColor.RenderBorderPlain(
                            Left, Top, 1, MaximumHeight,
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
                        BoxColor.RenderBox(Left + 1, Top, 1, MaximumHeight, BackgroundColor, useColor)
                    );
                }

                // Draw the slider
                int maxBlanks = ConsoleMisc.PercentRepeatTargeted(currPos, maxPos, MaximumHeight);
                if (maxBlanks == 0)
                    maxBlanks = 1;
                if (useColor)
                    progBuilder.Append($"{SliderColor.VTSequenceBackground}");
                for (int i = 0; i < SliderFilled; i++)
                {
                    int offset = maxBlanks - i;
                    if (offset <= 0)
                        offset = 1 + i;
                    progBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + offset + 1)}");
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

        static SliderVerticalColor()
        {
            ConsoleChecker.CheckConsole();
        }
    }
}
