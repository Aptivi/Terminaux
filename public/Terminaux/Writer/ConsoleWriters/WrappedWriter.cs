//
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
using System.Diagnostics;
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Editor;
using Textify.General;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Wrapped writer similar to <c>less</c> and <c>more</c> commands on Unix
    /// </summary>
    public static class WrappedWriter
    {
        /// <summary>
        /// Writes the text in a pager similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="args">Arguments to format the text</param>
        public static void WriteWrapped(string text, bool force = false, params object?[]? args) =>
            WriteWrapped(text, ColorTools.currentForegroundColor, force, args);

        /// <summary>
        /// Writes the text in a pager similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="args">Arguments to format the text</param>
        public static void WriteWrapped(string text, Color color, bool force = false, params object?[]? args) =>
            WriteWrapped(text, color, ColorTools.currentBackgroundColor, force, args);

        /// <summary>
        /// Writes the text in a pager similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="args">Arguments to format the text</param>
        public static void WriteWrapped(string text, Color foregroundColor, Color backgroundColor, bool force = false, params object?[]? args)
        {
            try
            {
                // Use the text viewer to avoid code repetition
                text = text.FormatString(args);
                var lines = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth).ToList();
                if (force || lines.Count >= ConsoleWrapper.WindowHeight)
                {
                    TextEditInteractive.OpenInteractive(ref lines, new()
                    {
                        PaneSelectedItemBackColor = foregroundColor,
                        BackgroundColor = backgroundColor,
                    }, true, false);
                }
                else
                    TextWriterColor.WriteColorBack(text, foregroundColor, backgroundColor);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }
    }
}
