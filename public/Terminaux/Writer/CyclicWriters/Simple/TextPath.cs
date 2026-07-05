//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Path text renderable
    /// </summary>
    public class TextPath : SimpleCyclicWriter
    {
        private string pathText = "";
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color rootDriveColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color separatorColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color lastPathColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private TextSettings settings = new();
        private bool useColors = true;

        /// <summary>
        /// Path text to render
        /// </summary>
        public string PathText
        {
            get => pathText;
            set => pathText = value;
        }

        /// <summary>
        /// Foreground color of the path text (usually a path color)
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Root drive color in the path text
        /// </summary>
        public Color RootDriveColor
        {
            get => rootDriveColor;
            set => rootDriveColor = value;
        }

        /// <summary>
        /// Separator color in the path text
        /// </summary>
        public Color SeparatorColor
        {
            get => separatorColor;
            set => separatorColor = value;
        }

        /// <summary>
        /// Last path color in the path text
        /// </summary>
        public Color LastPathColor
        {
            get => lastPathColor;
            set => lastPathColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Width of the text path (0 to disable alignment)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Alignment of the path text
        /// </summary>
        public TextAlignment Alignment { get; set; }

        /// <summary>
        /// Renders a path text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            try
            {
                // Tokenize the path
                string finalPath = PathText.Trim().Replace('\\', '/');
                bool isUnc = finalPath.StartsWith("//");
                string[] paths = finalPath.Split(['/'], StringSplitOptions.RemoveEmptyEntries);

                // Check to see if the path is rooted
                string separator = "/";
                bool isRootedUnix = finalPath.StartsWith(separator);
                bool isRooted = isRootedUnix || paths.Length > 0 && paths[0].EndsWith(":");
                string root = isRooted ? !isRootedUnix ? isUnc ? "//" + paths[0] : paths[0] : separator : "";
                int length = ConsoleChar.EstimateCellWidth(finalPath) - ConsoleChar.EstimateCellWidth(root) - 1;
                paths = finalPath.RemovePrefix(root).Split(['/'], StringSplitOptions.RemoveEmptyEntries);

                // Now, tokenize the path, applying styling in the process.
                StringBuilder tokenized = new();
                if (isRooted)
                {
                    tokenized.Append(
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(RootDriveColor) : "") +
                        root +
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(SeparatorColor) : "") +
                        (root == "/" ? "" : separator)
                    );
                }
                bool addedEllipsis = false;
                for (int i = 0; i < paths.Length; i++)
                {
                    // Add an ellipsis and subtract the length if required
                    string path = paths[i];
                    int pathWidth = ConsoleChar.EstimateCellWidth(path);
                    if (Width > 0 && length > Width)
                    {
                        if (!addedEllipsis)
                        {
                            addedEllipsis = true;
                            tokenized.Append(
                                (UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "") +
                                "..." +
                                (UseColors ? ConsoleColoring.RenderSetConsoleColor(SeparatorColor) : "") +
                                separator
                            );
                        }
                        length -= pathWidth;
                        continue;
                    }

                    // Stylize the path
                    Color final = i == paths.Length - 1 ? LastPathColor : ForegroundColor;
                    tokenized.Append(
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(final) : "") +
                        path
                    );
                    if (i < paths.Length - 1)
                    {
                        tokenized.Append(
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(SeparatorColor) : "") +
                            separator
                        );
                    }
                }

                // Return a rendered AlignedText
                string finalTokenized = tokenized.ToString();
                finalTokenized = ConsoleMisc.Pad(finalTokenized, Width, Alignment);
                return finalTokenized;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the path text renderer
        /// </summary>
        public TextPath()
        { }
    }
}
