﻿//
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
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Slider writer with color support
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class SliderColor
    {
        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderPlainAbsolute(int currPos, int maxPos, int Left, int Top, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderPlainAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderPlainAbsolute(int currPos, int maxPos, int Left, int Top, int width, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderPlainAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderPlainAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderPlainAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderPlainAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, int minPos = 0, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSliderPlainAbsolute(currPos, maxPos, Left, Top, width, settings, minPos, DrawBorder));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, Color SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, SliderColor, ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, Color SliderColor, Color FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, SliderColor, FrameColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, Color SliderColor, Color FrameColor, Color BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, Color BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, settings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, Color SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, SliderColor, ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, settings, SliderColor, ColorTools.GetGray(), minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, Color SliderColor, Color FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, SliderColor, FrameColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, SliderColor, FrameColor, BackgroundColor, minPos, DrawBorder);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, int minPos = 0, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSliderAbsolute(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, BackgroundColor, minPos, DrawBorder));
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
        public static void WriteSliderPlain(int currPos, int maxPos, int Left, int Top, bool DrawBorder = true) =>
            WriteSliderPlain(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSliderPlain(int currPos, int maxPos, int Left, int Top, int width, bool DrawBorder = true) =>
            WriteSliderPlain(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderPlain(int currPos, int maxPos, int Left, int Top, BorderSettings settings, bool DrawBorder = true) =>
            WriteSliderPlain(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSliderPlain(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSliderPlain(currPos, maxPos, Left, Top, width, settings, DrawBorder));
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
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

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
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, SliderColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, ColorTools.GetGray(), DrawBorder);

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
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, DrawBorder);

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
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, DrawBorder);

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
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, settings, new Color(ConsoleColors.Olive), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, Color SliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, SliderColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, settings, SliderColor, ColorTools.GetGray(), DrawBorder);

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
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, SliderColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, DrawBorder);

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
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, settings, SliderColor, FrameColor, BackgroundColor, DrawBorder);

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
        /// <param name="width">Slider width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="settings">Border settings</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderSlider(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, BackgroundColor, DrawBorder));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        public static string RenderSliderPlainAbsolute(int currPos, int maxPos, int Left, int Top, int width, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        public static string RenderSliderPlainAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, settings, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="SliderColor">The slider color</param>
        public static string RenderSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        public static string RenderSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static string RenderSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, Color BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        public static string RenderSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, settings, SliderColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        public static string RenderSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static string RenderSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, int minPos = 0, bool DrawBorder = true) =>
            RenderSliderAbsolute(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, BackgroundColor, true, minPos, DrawBorder);

        /// <summary>
        /// Renders the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, bool useColor, int minPos = 0, bool DrawBorder = true) =>
            RenderSlider((int)(width * ((double)(currPos - minPos) / (maxPos - minPos))), width + 1, Left, Top, width, settings, SliderColor, FrameColor, BackgroundColor, useColor, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        public static string RenderSliderPlain(int currPos, int maxPos, int Left, int Top, int width, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        public static string RenderSliderPlain(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, settings, ColorTools.currentForegroundColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="SliderColor">The slider color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int width, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, BorderSettings.GlobalSettings, SliderColor, FrameColor, BackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, settings, SliderColor, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, ColorTools.currentBackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static string RenderSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, bool DrawBorder = true) =>
            RenderSlider(currPos, maxPos, Left, Top, width, settings, SliderColor, FrameColor, BackgroundColor, true, DrawBorder);

        /// <summary>
        /// Renders the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="width">Slider width</param>
        /// <param name="settings">Border settings</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="FrameColor">The slider frame color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        internal static string RenderSlider(int currPos, int maxPos, int Left, int Top, int width, BorderSettings settings, Color SliderColor, Color FrameColor, Color BackgroundColor, bool useColor, bool DrawBorder = true)
        {
            try
            {
                // Check the slider value
                if (maxPos < 0)
                    maxPos = 0;
                if (currPos > maxPos)
                    currPos = maxPos;
                if (currPos < 0)
                    currPos = 0;
                if (maxPos <= width)
                    return "";

                // Fill the slider
                int maxPosOffset = maxPos - width;
                double maxPosFraction = (double)width / maxPosOffset;
                int times = ConsoleMisc.PercentRepeat((int)Math.Round(maxPosFraction), maxPos, width);
                if (times == 0)
                    times = 1;

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
                            Left, Top, width, 1, settings
                        )
                    );
                }
                else
                {
                    progBuilder.Append(
                        Box.RenderBox(Left + 1, Top, width, 1, BackgroundColor, useColor)
                    );
                }

                // Draw the slider
                int maxBlanks = ConsoleMisc.PercentRepeatTargeted(currPos, maxPos, width - times + 1);
                if (maxBlanks == width - times + 1)
                    maxBlanks--;
                if (useColor)
                    progBuilder.Append(SliderColor.VTSequenceBackground);
                progBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + maxBlanks + 2, Top + 2) + new string(useColor ? ' ' : '*', times));
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

        static SliderColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
