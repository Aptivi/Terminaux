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
using System.IO;
using Terminaux.Base;
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
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandle(Filename, LineNumber, ColumnNumber, new Color(ConsoleColors.Gray));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber) =>
            PrintLineWithHandle(Array, LineNumber, ColumnNumber, new Color(ConsoleColors.Gray));

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Filename">Path to text file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string Filename, int LineNumber, int ColumnNumber, Color ColorType)
        {
            // Read the contents
            Filename = ConsoleExtensions.NeutralizePath(Filename, Environment.CurrentDirectory);
            var FileContents = File.ReadAllLines(Filename);

            // Do the job
            PrintLineWithHandle(FileContents, LineNumber, ColumnNumber, ColorType);
        }

        /// <summary>
        /// Prints the line of a text file with the specified line number and the column number
        /// </summary>
        /// <param name="Array">A string array containing the contents of the file</param>
        /// <param name="LineNumber">Line number (not index)</param>
        /// <param name="ColumnNumber">Column number (not index). This tells the handle where to place itself</param>
        /// <param name="ColorType">The type of color</param>
        public static void PrintLineWithHandle(string[] Array, int LineNumber, int ColumnNumber, Color ColorType)
        {
            // Get the line index from number
            if (LineNumber <= 0)
                LineNumber = 1;
            if (LineNumber > Array.Length)
                LineNumber = Array.Length;
            int LineIndex = LineNumber - 1;

            // Get the line
            string LineContent = Array[LineIndex];
            TextWriterColor.Write(" | " + LineContent, true, ColorType);

            // Place the column handle
            int RepeatBlanks = ColumnNumber - 1;
            if (RepeatBlanks < 0)
                RepeatBlanks = 0;
            TextWriterColor.Write(" | " + new string(' ', RepeatBlanks) + "^", true, ColorType);
        }

    }
}
