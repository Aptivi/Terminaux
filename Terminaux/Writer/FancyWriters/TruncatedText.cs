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

using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Truncated text writer
    /// </summary>
    public static class TruncatedText
    {
        public static string RenderText(string text, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, int width, int height, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, int width, int height, int left, int top, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, Color textColor, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, Color textColor, int width, int height, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, Color textColor, int width, int height, int left, int top, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, Color textColor, Color backgroundColor, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, Color textColor, Color backgroundColor, int width, int height, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, Color textColor, Color backgroundColor, int width, int height, int left, int top, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, TextSettings settings, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, TextSettings settings, int width, int height, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, TextSettings settings, int width, int height, int left, int top, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, TextSettings settings, Color textColor, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, TextSettings settings, Color textColor, int width, int height, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, TextSettings settings, Color textColor, int width, int height, int left, int top, ref int increment, int currIdx, params object[] vars)
        {

        }

        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, ref int increment, int currIdx, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, 0, 0, true, ref increment, currIdx, vars);

        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, ref int increment, int currIdx, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, width, height, 0, 0, true, ref increment, currIdx, vars);

        public static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, ref int increment, int currIdx, params object[] vars) =>
            RenderText(text, settings, textColor, backgroundColor, width, height, left, top, true, ref increment, currIdx, vars);

        internal static string RenderText(string text, TextSettings settings, Color textColor, Color backgroundColor, int width, int height, int left, int top, bool useColor, ref int increment, int currIdx, params object[] vars)
        {
            string[] splitFinalLines = InfoBoxTools.GetFinalLines(text, vars);

            // Render text inside it
            var builder = new StringBuilder();
            int linesMade = 0;
            for (int i = currIdx; i < splitFinalLines.Length; i++)
            {
                var line = splitFinalLines[i];
                if (linesMade % height == 0 && linesMade > 0)
                {
                    // Reached the end of the box. Bail.
                    increment = linesMade;
                    break;
                }
                builder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(left + 1, top + 1 + linesMade % height + 1)}" +
                    $"{line}"
                );
                linesMade++;
            }

            return builder.ToString();
        }
    }
}
