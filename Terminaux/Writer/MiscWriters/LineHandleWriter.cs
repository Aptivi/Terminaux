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
using System.IO;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Writer.MiscWriters
{
    /// <summary>
    /// Line handle writer
    /// </summary>
    public static class LineHandleWriter
    {

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandleConditional(Condition, Filename, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Filename, LineNumber, ColumnNumber, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Array, LineNumber, ColumnNumber, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandle(Filename, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandle(Array, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            // Read the contents
            Filename = ConsolePositioning.NeutralizePath(Filename, Environment.CurrentDirectory);
            var FileContents = File.ReadAllLines(Filename);

            // Do the job
            PrintLineWithHandle(FileContents, LineNumber, ColumnNumber, color);
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, Color color) =>
            TextWriterColor.WriteColor(RenderLineWithHandle(Array, LineNumber, ColumnNumber, color), true, color);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandleConditional(Condition, Filename, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandleConditional(Condition, Array, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Filename, LineNumber, ColumnNumber, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int ColumnNumber, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Array, LineNumber, ColumnNumber, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandle(Filename, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int ColumnNumber) =>
            RenderLineWithHandle(Array, LineNumber, ColumnNumber, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int ColumnNumber, Color color)
        {
            // Read the contents
            Filename = ConsolePositioning.NeutralizePath(Filename, Environment.CurrentDirectory);
            var FileContents = File.ReadAllLines(Filename);

            // Do the job
            return RenderLineWithHandle(FileContents, LineNumber, ColumnNumber, color);
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, Color color) =>
            LineHandleRangedWriter.RenderLineHandle(Array, LineNumber, ColumnNumber, 0, color, false);

    }
}
