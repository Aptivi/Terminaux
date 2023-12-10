﻿//
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
using System.Threading;
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Textify.Sequences.Builder.Types;
using Textify.General;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// BoxFrame writer with color support
    /// </summary>
    public static class BoxFrameTextColor
    {
        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFramePlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBoxFramePlain(text, Left, Top, InteriorWidth, InteriorHeight,
                               BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                               BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                               BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                               BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        public static void WriteBoxFramePlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                              char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                              char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterWhereColor.WriteWhere(frame, Left, Top, false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Gray), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BoxFrameColor), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BoxFrameColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BoxFrameColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(ConsoleColors.Gray), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color BoxFrameColor, Color BackgroundColor, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, BoxFrameColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(BoxFrameColor), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors BoxFrameColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, BoxFrameColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                           BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                           BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                           BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                           BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            try
            {
                // StringBuilder is here to formulate the whole string consisting of box frame
                StringBuilder frameBuilder = new();

                // Render the initial frame
                frameBuilder.Append(BoxFrameColor.RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar));

                // Text title
                string finalText = $" {TextTools.FormatString(text, vars).Truncate(InteriorWidth - 5)} ";
                frameBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 3, Top + 1)}" +
                    $"{finalText}");
                return frameBuilder.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                            Color FrameColor, Color BackgroundColor, params object[] vars) =>
            RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight,
                           BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                           BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                           BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                           BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                           FrameColor, BackgroundColor, vars);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
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
        /// <param name="FrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                            Color FrameColor, Color BackgroundColor, params object[] vars)
        {
            try
            {
                // StringBuilder is here to formulate the whole string consisting of box frame
                StringBuilder frameBuilder = new();

                // Render the initial frame
                frameBuilder.Append(
                    BoxFrameColor.RenderBoxFrame
                    (
                        Left, Top, InteriorWidth, InteriorHeight,
                        UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                        UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                        FrameColor, BackgroundColor
                    )
                );

                // Text title
                string finalText = $" {TextTools.FormatString(text, vars).Truncate(InteriorWidth - 5)} ";
                frameBuilder.Append(
                    FrameColor.VTSequenceForeground +
                    BackgroundColor.VTSequenceBackground +
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 3, Top + 1)}" +
                    $"{finalText}" +
                    ColorTools.currentForegroundColor.VTSequenceForeground +
                    ColorTools.currentBackgroundColor.VTSequenceBackground
                );
                return frameBuilder.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            return "";
        }
    }
}
