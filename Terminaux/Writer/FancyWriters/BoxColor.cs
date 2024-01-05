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
using System.Threading;
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Textify.Sequences.Builder.Types;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Box writer with color support
    /// </summary>
    public static class BoxColor
    {
        /// <summary>
        /// Writes the box plainly
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxPlain(int Left, int Top, int InteriorWidth, int InteriorHeight)
        {
            try
            {
                // Fill the box with spaces inside it
                TextWriterWhereColor.WriteWhere(RenderBox(Left, Top, InteriorWidth, InteriorHeight), Left, Top);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box plainly
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBox(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBox(Left, Top, InteriorWidth, InteriorHeight, ColorTools.currentBackgroundColor);

        /// <summary>
        /// Writes the box plainly
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxColor">Box color</param>
        public static void WriteBox(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BoxColor)
        {
            try
            {
                // Fill the box with spaces inside it
                TextWriterWhereColor.WriteWhereColorBack(RenderBox(Left, Top, InteriorWidth, InteriorHeight), Left, Top, false, ColorTools.currentForegroundColor, new Color(BoxColor));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box plainly
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxColor">Box color</param>
        public static void WriteBox(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxColor)
        {
            try
            {
                // Fill the box with spaces inside it
                TextWriterWhereColor.WriteWhereColorBack(RenderBox(Left, Top, InteriorWidth, InteriorHeight), Left, Top, false, ColorTools.currentForegroundColor, BoxColor);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders the box
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <returns>The rendered box</returns>
        public static string RenderBox(int Left, int Top, int InteriorWidth, int InteriorHeight)
        {
            // Fill the box with spaces inside it
            StringBuilder box = new();
            box.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 2));
            for (int y = 1; y <= InteriorHeight; y++)
                box.Append(
                    new string(' ', InteriorWidth) +
                    CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + y + 2)
                );
            return box.ToString();
        }

        /// <summary>
        /// Renders the box
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxColor">Box color</param>
        /// <returns>The rendered box</returns>
        public static string RenderBox(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxColor)
        {
            // Fill the box with spaces inside it
            StringBuilder box = new();
            box.Append(
                BoxColor.VTSequenceBackground +
                CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 2)
            );
            for (int y = 1; y <= InteriorHeight; y++)
                box.Append(
                    new string(' ', InteriorWidth) +
                    CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + y + 2)
                );
            box.Append(ColorTools.currentBackgroundColor.VTSequenceBackground);
            return box.ToString();
        }
    }
}
