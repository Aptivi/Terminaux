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
using System.IO;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Writer.MiscWriters
{
    /// <summary>
    /// Ranged line handle writer (with start and end positions)
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class LineHandleRangedWriter
    {
        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself as the start.</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandleConditional(Condition, Filename, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Filename, LineNumber, startPos, endPos, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
            {
                PrintLineWithHandle(Array, LineNumber, startPos, endPos, color);
            }
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandle(Filename, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos) =>
            PrintLineWithHandle(Array, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            // Read the contents
            var FileContents = File.ReadAllLines(Filename);

            // Do the job
            PrintLineWithHandle(FileContents, LineNumber, startPos, endPos, color);
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, Color color) =>
            TextWriterColor.WriteColor(RenderLineWithHandle(Array, LineNumber, startPos, endPos, color), true, color);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandleConditional(Condition, Filename, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandleConditional(Condition, Array, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Filename, LineNumber, startPos, endPos, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number if the specified condition is satisfied
        /// </summary>
        /// <param name="Condition">The condition to satisfy</param>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandleConditional(bool Condition, string[] Array, int LineNumber, int startPos, int endPos, Color color)
        {
            if (Condition)
                return RenderLineWithHandle(Array, LineNumber, startPos, endPos, color);
            return "";
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandle(Filename, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos) =>
            RenderLineWithHandle(Array, LineNumber, startPos, endPos, ColorTools.currentForegroundColor);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string Filename, int LineNumber, int startPos, int endPos, Color color)
        {
            // Read the contents
            var FileContents = File.ReadAllLines(Filename);

            // Do the job
            return RenderLineWithHandle(FileContents, LineNumber, startPos, endPos, color);
        }

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        public static string RenderLineWithHandle(string[] Array, int LineNumber, int startPos, int endPos, Color color) =>
            RenderLineHandle(Array, LineNumber, startPos, endPos, color);

        /// <summary>
        /// Renders the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="startPos">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="endPos">Column number (not index). This tells the handle where to place itself as the end. Should be bigger than the start position.</param>
        /// <param name="color">The color</param>
        /// <param name="ranged">Whether it's ranged or not</param>
        internal static string RenderLineHandle(string[] Array, int LineNumber, int startPos, int endPos, Color color, bool ranged = true)
        {
            if (ranged)
            {
                startPos = Math.Min(startPos, endPos);
                endPos = Math.Max(startPos, endPos);
            }
            var handle = new LineHandle(Array)
            {
                Color = color,
                Position = LineNumber,
                SourcePosition = startPos,
                TargetPosition = endPos,
                Ranged = ranged
            };
            return handle.Render();
        }

        static LineHandleRangedWriter()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
