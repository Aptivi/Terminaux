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
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Box frame renderable
    /// </summary>
    public class BoxFrame : GraphicalCyclicWriter
    {
        private string text = "";
        private Color boxFrameColor = ColorTools.CurrentForegroundColor;
        private Color titleColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private BorderSettings settings = new();
        private TextSettings titleSettings = new();
        private bool useColors = true;

        /// <summary>
        /// Top position
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Box frame color
        /// </summary>
        public Color FrameColor
        {
            get => boxFrameColor;
            set => boxFrameColor = value;
        }

        /// <summary>
        /// Box frame title color
        /// </summary>
        public Color TitleColor
        {
            get => titleColor;
            set => titleColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
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
        /// Border settings to use
        /// </summary>
        public BorderSettings Settings
        {
            get => settings;
            set => settings = value;
        }

        /// <summary>
        /// Title settings to use
        /// </summary>
        public TextSettings TitleSettings
        {
            get => titleSettings;
            set => titleSettings = value;
        }

        /// <summary>
        /// Whether to enable drop shadow or not
        /// </summary>
        public bool DropShadow { get; set; }

        /// <summary>
        /// Drop shadow color
        /// </summary>
        public Color ShadowColor { get; set; } = ConsoleColors.Grey;

        /// <summary>
        /// Renders a box frame
        /// </summary>
        /// <returns>Rendered box frame that will be used by the renderer</returns>
        public override string Render()
        {
            try
            {
                // StringBuilder is here to formulate the whole string consisting of box frame
                StringBuilder frameBuilder = new();

                // Colors
                if (UseColors)
                {
                    frameBuilder.Append(
                        ColorTools.RenderSetConsoleColor(FrameColor) +
                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }

                // Corners
                if (settings.BorderUpperLeftCornerEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)}" +
                        $"{settings.BorderUpperLeftCornerChar}"
                    );
                }
                if (settings.BorderUpperRightCornerEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + Width + 2, Top + 1)}" +
                        $"{settings.BorderUpperRightCornerChar}"
                    );
                }
                if (settings.BorderLowerLeftCornerEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + Height + 2)}" +
                        $"{settings.BorderLowerLeftCornerChar}"
                    );
                }
                if (settings.BorderLowerRightCornerEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + Width + 2, Top + Height + 2)}" +
                        $"{settings.BorderLowerRightCornerChar}"
                    );
                }

                // Upper frame
                if (settings.BorderUpperFrameEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 1)}" +
                        $"{new string(settings.BorderUpperFrameChar, Width)}"
                    );
                }
                if (settings.BorderLowerFrameEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + Height + 2)}" +
                        $"{new string(settings.BorderLowerFrameChar, Width)}"
                    );
                }

                // Left and right edges
                for (int i = 1; i <= Height; i++)
                {
                    if (settings.BorderLeftFrameEnabled)
                    {
                        frameBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + i + 1)}" +
                            $"{settings.BorderLeftFrameChar}"
                        );
                    }
                    if (settings.BorderRightFrameEnabled)
                    {
                        frameBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(Left + Width + 2, Top + i + 1)}" +
                            $"{settings.BorderRightFrameChar}"
                        );
                    }
                }

                // Drop shadow (if any)
                if (DropShadow)
                {
                    for (int i = 1; i <= Height + 1; i++)
                    {
                        if (settings.BorderRightFrameEnabled)
                        {
                            frameBuilder.Append(
                                (UseColors ? ColorTools.RenderSetConsoleColor(ShadowColor, true) : "") +
                                $"{CsiSequences.GenerateCsiCursorPosition(Left + Width + 3, Top + i + 1)}" +
                                " "
                            );
                        }
                    }
                    if (settings.BorderLowerFrameEnabled)
                    {
                        frameBuilder.Append(
                            (UseColors ? ColorTools.RenderSetConsoleColor(ShadowColor, true) : "") +
                            $"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + Height + 3)}" +
                            $"{new string(' ', Width + 2)}"
                        );
                    }
                }

                // Colors
                if (UseColors)
                {
                    frameBuilder.Append(
                        ColorTools.RenderSetConsoleColor(TitleColor) +
                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }

                // Text title
                if (!string.IsNullOrEmpty(text) && Width - 8 > 0 && ConsoleChar.EstimateCellWidth(text) <= Width - 8)
                {
                    string finalText =
                        $"{(settings.BorderRightHorizontalIntersectionEnabled ? $"{settings.BorderRightHorizontalIntersectionChar} " : "")}" +
                        text.Truncate(Width - 8) +
                        $"{(settings.BorderLeftHorizontalIntersectionEnabled ? $" {settings.BorderLeftHorizontalIntersectionChar}" : "")}";
                    int leftPos = TextWriterTools.DetermineTextAlignment(finalText, Width - 8, TitleSettings.TitleAlignment, Left + 2);
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(leftPos + 1, Top + 1)}" +
                        $"{finalText}"
                    );
                }

                // Write the resulting buffer
                if (UseColors)
                {
                    frameBuilder.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
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

        /// <summary>
        /// Makes a new instance of the box frame renderer
        /// </summary>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public BoxFrame(Mark? text = null, params object[] vars)
        {
            this.text = TextTools.FormatString(text ?? "", vars);
        }
    }
}
