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
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Base.Checks;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Canvas writer with color support
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class CanvasColor
    {
        /// <summary>
        /// Writes the canvas plainly
        /// </summary>
        /// <param name="pixels">Individual pixels to render</param>
        /// <param name="Left">Where to place the canvas horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the canvas vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="doubleWidth">Whether this canvas is a double width or a single width canvas</param>
        /// <param name="transparent">Whether this canvas is transparent</param>
        public static void WriteCanvasPlain(CellOptions[] pixels, int Left, int Top, int InteriorWidth, int InteriorHeight, bool doubleWidth = true, bool transparent = false)
        {
            try
            {
                // Fill the canvas with spaces inside it
                TextWriterWhereColor.WriteWhere(RenderCanvas(pixels, Left, Top, InteriorWidth, InteriorHeight, doubleWidth, transparent), Left, Top);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the canvas plainly
        /// </summary>
        /// <param name="pixels">Individual pixels to render</param>
        /// <param name="Left">Where to place the canvas horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the canvas vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="doubleWidth">Whether this canvas is a double width or a single width canvas</param>
        /// <param name="transparent">Whether this canvas is transparent</param>
        public static void WriteCanvas(CellOptions[] pixels, int Left, int Top, int InteriorWidth, int InteriorHeight, bool doubleWidth = true, bool transparent = false) =>
            WriteCanvas(pixels, Left, Top, InteriorWidth, InteriorHeight, ColorTools.currentBackgroundColor, doubleWidth, transparent);

        /// <summary>
        /// Writes the canvas plainly
        /// </summary>
        /// <param name="pixels">Individual pixels to render</param>
        /// <param name="Left">Where to place the canvas horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the canvas vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="doubleWidth">Whether this canvas is a double width or a single width canvas</param>
        /// <param name="transparent">Whether this canvas is transparent</param>
        /// <param name="CanvasColor">Canvas color</param>
        public static void WriteCanvas(CellOptions[] pixels, int Left, int Top, int InteriorWidth, int InteriorHeight, Color CanvasColor, bool doubleWidth = true, bool transparent = false)
        {
            try
            {
                // Fill the canvas with spaces inside it
                TextWriterWhereColor.WriteWhere(RenderCanvas(pixels, Left, Top, InteriorWidth, InteriorHeight, CanvasColor, doubleWidth, transparent), Left, Top);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the canvas
        /// </summary>
        /// <param name="pixels">Individual pixels to render</param>
        /// <param name="Left">Where to place the canvas horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the canvas vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="doubleWidth">Whether this canvas is a double width or a single width canvas</param>
        /// <param name="transparent">Whether this canvas is transparent</param>
        /// <returns>The rendered canvas</returns>
        public static string RenderCanvas(CellOptions[] pixels, int Left, int Top, int InteriorWidth, int InteriorHeight, bool doubleWidth = true, bool transparent = false) =>
            RenderCanvas(pixels, Left, Top, InteriorWidth, InteriorHeight, ColorTools.currentBackgroundColor, doubleWidth, transparent);

        /// <summary>
        /// Renders the canvas
        /// </summary>
        /// <param name="pixels">Individual pixels to render</param>
        /// <param name="Left">Where to place the canvas horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the canvas vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="doubleWidth">Whether this canvas is a double width or a single width canvas</param>
        /// <param name="transparent">Whether this canvas is transparent</param>
        /// <param name="CanvasColor">Canvas color</param>
        /// <returns>The rendered canvas</returns>
        public static string RenderCanvas(CellOptions[] pixels, int Left, int Top, int InteriorWidth, int InteriorHeight, Color CanvasColor, bool doubleWidth = true, bool transparent = false) =>
            Canvas.RenderCanvas(pixels, Left, Top, InteriorWidth, InteriorHeight, CanvasColor, doubleWidth, transparent);

        static CanvasColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
