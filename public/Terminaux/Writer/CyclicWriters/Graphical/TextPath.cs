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

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Path text renderable
    /// </summary>
    public class TextPath : GraphicalCyclicWriter
    {
        private string pathText = "";
        private bool oneLine = false;
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color rootDriveColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color separatorColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color lastPathColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private TextSettings settings = new();
        private bool useColors = true;
        private bool rainbow = false;
        private bool rainbowBg = false;

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
        /// Background color of the text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Text settings to use
        /// </summary>
        public TextSettings Settings
        {
            get => settings;
            set => settings = value;
        }

        /// <summary>
        /// Whether to print all lines or only one line
        /// </summary>
        public bool OneLine
        {
            get => oneLine;
            set => oneLine = value;
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
        /// Whether to write text with rainbow effects or not
        /// </summary>
        public bool Rainbow
        {
            get => rainbow;
            set => rainbow = value;
        }

        /// <summary>
        /// Whether to write text with rainbow effects in the background or in the foreground
        /// </summary>
        public bool RainbowBg
        {
            get => rainbowBg;
            set => rainbowBg = value;
        }

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
                    if (length > Width)
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
                var alignedPath = new AlignedText()
                {
                    ForegroundColor = ForegroundColor,
                    BackgroundColor = BackgroundColor,
                    UseColors = UseColors,
                    Top = Top,
                    Left = Left,
                    Width = Width,
                    Settings = Settings,
                    OneLine = OneLine,
                    Rainbow = Rainbow,
                    RainbowBg = RainbowBg,
                    Text = finalTokenized,
                };
                return alignedPath.Render();
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
