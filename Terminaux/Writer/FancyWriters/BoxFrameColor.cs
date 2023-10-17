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
using Terminaux.Colors;
using Terminaux.Sequences.Builder;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// BoxFrame writer with color support
    /// </summary>
    public static class BoxFrameColor
    {
        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFramePlain(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight,
                               BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                               BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                               BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                               BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        public static void WriteBoxFramePlain(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                              char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                              char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                TextWriterWhereColor.WriteWhere(frame, Left, Top, false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BoxFrameColor), new Color(ConsoleColors.Black));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BoxFrameColor, ConsoleColors BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BoxFrameColor), new Color(BackgroundColor));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, new Color(ConsoleColors.Black));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, BackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, new Color(ConsoleColors.Black));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color BoxFrameColor, Color BackgroundColor)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, BoxFrameColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(BoxFrameColor), new Color(ConsoleColors.Black));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors BoxFrameColor, ConsoleColors BackgroundColor)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, BoxFrameColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        internal static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                              char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                              char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar)
        {
            try
            {
                // StringBuilder is here to formulate the whole string consisting of box frame
                StringBuilder frameBuilder = new();

                // Upper frame
                frameBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)}" +
                    $"{UpperLeftCornerChar}{new string(UpperFrameChar, InteriorWidth)}{UpperRightCornerChar}");

                // Left and right edges
                for (int i = 1; i <= InteriorHeight; i++)
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + i + 1)}" +
                        $"{LeftFrameChar}" +
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + InteriorWidth + 2, Top + i + 1)}" +
                        $"{RightFrameChar}");

                // Lower frame
                frameBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + InteriorHeight + 2)}" +
                    $"{LowerLeftCornerChar}{new string(LowerFrameChar, InteriorWidth)}{LowerRightCornerChar}");
                return frameBuilder.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }
    }
}
