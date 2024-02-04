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
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Textify.General;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;

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
            WriteBoxFramePlain(
                Left, Top, InteriorWidth, InteriorHeight,
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
                TextWriterRaw.WritePlain(frame, false);
            }
            catch (Exception ex)
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
            WriteBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                ConsoleColors.Gray);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color BoxFrameColor) =>
            WriteBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color BoxFrameColor, Color BackgroundColor) =>
            WriteBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Terminaux's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Terminaux's <see cref="Color"/></param>
        /// <param name="TextColor">BoxFrame text color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            WriteBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, BackgroundColor, TextColor);

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
            WriteBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                ConsoleColors.Gray);

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
            WriteBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                BoxFrameColor, ColorTools.currentBackgroundColor);

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
            Color BoxFrameColor, Color BackgroundColor) =>
            WriteBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                BoxFrameColor, BackgroundColor, ColorTools.GetGray());

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
        /// <param name="TextColor">BoxFrame text color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color BoxFrameColor, Color BackgroundColor, Color TextColor)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, BackgroundColor, TextColor);
                TextWriterRaw.WritePlain(frame, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar);

        /// <summary>
        /// Renders the box frame
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
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar) =>
            RenderBoxFrame("",
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                                            Color BoxFrameColor) =>
            RenderBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, ColorTools.CurrentBackgroundColor);

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                Color BoxFrameColor, Color BackgroundColor) =>
            RenderBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the box frame
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            RenderBoxFrame(
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, BackgroundColor, TextColor);

        /// <summary>
        /// Renders the box frame
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
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
                char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                Color BoxFrameColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                BoxFrameColor, ColorTools.CurrentBackgroundColor);

        /// <summary>
        /// Renders the box frame
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
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color BoxFrameColor, Color BackgroundColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                BoxFrameColor, BackgroundColor, ColorTools.GetGray());

        /// <summary>
        /// Renders the box frame
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
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered frame</returns>
        public static string RenderBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color BoxFrameColor, Color BackgroundColor, Color TextColor) =>
            RenderBoxFrame("", Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                BoxFrameColor, BackgroundColor, TextColor, true);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFramePlain(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBoxFramePlain(text,
                Left, Top, InteriorWidth, InteriorHeight,
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
        public static void WriteBoxFramePlain(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterRaw.WritePlain(frame, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                new Color(ConsoleColors.Gray), vars);

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
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
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
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color BoxFrameColor, Color BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, BackgroundColor, ColorTools.GetGray(), vars);

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
        /// <param name="TextColor">BoxFrame text color from Terminaux's <see cref="Color"/></param>
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color BoxFrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            WriteBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                BoxFrameColor, BackgroundColor, TextColor, vars);

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
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                new Color(ConsoleColors.Gray), vars);

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
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
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
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color BoxFrameColor, Color BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                BoxFrameColor, BackgroundColor, ColorTools.GetGray(), vars);

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
        /// <param name="TextColor">BoxFrame text color</param>
        public static void WriteBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color BoxFrameColor, Color BackgroundColor, Color TextColor, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, BackgroundColor, TextColor, vars);
                TextWriterRaw.WritePlain(frame, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
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
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, ColorTools.GetGray(), false, vars);

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
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color FrameColor, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                FrameColor, ColorTools.CurrentBackgroundColor, vars);

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
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color FrameColor, Color BackgroundColor, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                FrameColor, BackgroundColor, ColorTools.GetGray(), vars);

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
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            Color FrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                FrameColor, BackgroundColor, TextColor, vars);

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
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color FrameColor, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                FrameColor, ColorTools.CurrentBackgroundColor, vars);

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
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color FrameColor, Color BackgroundColor, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                FrameColor, BackgroundColor, ColorTools.GetGray(), vars);

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
        /// <param name="TextColor">BoxFrame text color</param>
        /// <returns>The rendered box frame</returns>
        public static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color FrameColor, Color BackgroundColor, Color TextColor, params object[] vars) =>
            RenderBoxFrame(text,
                Left, Top, InteriorWidth, InteriorHeight,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                FrameColor, BackgroundColor, TextColor, true, vars);

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
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        /// <param name="TextColor">BoxFrame text color</param>
        /// <param name="useColor">Whether to use the color or not</param>
        /// <returns>The rendered frame</returns>
        internal static string RenderBoxFrame(string text,
            int Left, int Top, int InteriorWidth, int InteriorHeight,
            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
            Color BoxFrameColor, Color BackgroundColor, Color TextColor, bool useColor, params object[] vars)
        {
            try
            {
                // StringBuilder is here to formulate the whole string consisting of box frame
                StringBuilder frameBuilder = new();

                // Colors
                if (useColor)
                {
                    frameBuilder.Append(
                        BoxFrameColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground
                    );
                }

                // Upper frame
                frameBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)}" +
                    $"{UpperLeftCornerChar}{new string(UpperFrameChar, InteriorWidth)}{UpperRightCornerChar}"
                );

                // Left and right edges
                for (int i = 1; i <= InteriorHeight; i++)
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + i + 1)}" +
                        $"{LeftFrameChar}" +
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + InteriorWidth + 2, Top + i + 1)}" +
                        $"{RightFrameChar}"
                    );

                // Lower frame
                frameBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + InteriorHeight + 2)}" +
                    $"{LowerLeftCornerChar}{new string(LowerFrameChar, InteriorWidth)}{LowerRightCornerChar}"
                );

                // Colors
                if (useColor)
                {
                    frameBuilder.Append(
                        TextColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground
                    );
                }

                // Text title
                if (!string.IsNullOrEmpty(text) && InteriorWidth - 7 > 0)
                {
                    string finalText = $" {TextTools.FormatString(text, vars).Truncate(InteriorWidth - 7)} ";
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 3, Top + 1)}" +
                        $"{finalText}"
                    );
                }

                // Write the resulting buffer
                if (useColor)
                {
                    frameBuilder.Append(
                        ColorTools.currentForegroundColor.VTSequenceForeground +
                        ColorTools.currentBackgroundColor.VTSequenceBackground
                    );
                }
                return frameBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }
    }
}
