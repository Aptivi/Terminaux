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
using System.Diagnostics;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Box frame renderable
    /// </summary>
    public class BoxFrame : IStaticRenderable
    {
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private string text = "";
        private Color boxFrameColor = ColorTools.CurrentForegroundColor;
        private Color titleColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private BorderSettings settings = new();
        private TextSettings titleSettings = new();
        private bool useColors = true;

        /// <summary>
        /// Left position
        /// </summary>
        public int Left
        {
            get => left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set => top = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Interior width
        /// </summary>
        public int InteriorWidth
        {
            get => interiorWidth;
            set => interiorWidth = value;
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set => interiorHeight = value;
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
        public string Render()
        {
            return RenderBoxFrame(
                Text, Left, Top, InteriorWidth, InteriorHeight, Settings, TitleSettings, FrameColor, BackgroundColor, TitleColor, UseColors, DropShadow, ShadowColor);
        }

        internal static string RenderBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, TextSettings textSettings, Color BoxFrameColor, Color BackgroundColor, Color TextColor, bool useColor, bool dropShadow, Color shadowColor, params object[] vars)
        {
            try
            {
                // StringBuilder is here to formulate the whole string consisting of box frame
                StringBuilder frameBuilder = new();

                // Colors
                if (useColor)
                {
                    frameBuilder.Append(
                        ColorTools.RenderSetConsoleColor(BoxFrameColor) +
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
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + InteriorWidth + 2, Top + 1)}" +
                        $"{settings.BorderUpperRightCornerChar}"
                    );
                }
                if (settings.BorderLowerLeftCornerEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + InteriorHeight + 2)}" +
                        $"{settings.BorderLowerLeftCornerChar}"
                    );
                }
                if (settings.BorderLowerRightCornerEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + InteriorWidth + 2, Top + InteriorHeight + 2)}" +
                        $"{settings.BorderLowerRightCornerChar}"
                    );
                }

                // Upper frame
                if (settings.BorderUpperFrameEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 1)}" +
                        $"{new string(settings.BorderUpperFrameChar, InteriorWidth)}"
                    );
                }
                if (settings.BorderLowerFrameEnabled)
                {
                    frameBuilder.Append(
                        $"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + InteriorHeight + 2)}" +
                        $"{new string(settings.BorderLowerFrameChar, InteriorWidth)}"
                    );
                }

                // Left and right edges
                for (int i = 1; i <= InteriorHeight; i++)
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
                            $"{CsiSequences.GenerateCsiCursorPosition(Left + InteriorWidth + 2, Top + i + 1)}" +
                            $"{settings.BorderRightFrameChar}"
                        );
                    }
                }

                // Drop shadow (if any)
                if (dropShadow)
                {
                    for (int i = 1; i <= InteriorHeight + 1; i++)
                    {
                        if (settings.BorderRightFrameEnabled)
                        {
                            frameBuilder.Append(
                                (useColor ? ColorTools.RenderSetConsoleColor(shadowColor, true) : "") +
                                $"{CsiSequences.GenerateCsiCursorPosition(Left + InteriorWidth + 3, Top + i + 1)}" +
                                " "
                            );
                        }
                    }
                    if (settings.BorderLowerFrameEnabled)
                    {
                        frameBuilder.Append(
                            (useColor ? ColorTools.RenderSetConsoleColor(shadowColor, true) : "") +
                            $"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + InteriorHeight + 3)}" +
                            $"{new string(' ', InteriorWidth + 2)}"
                        );
                    }
                }

                // Text title
                int finalWidth = InteriorWidth - 7 - (textSettings.TitleOffset.Left + textSettings.TitleOffset.Right);
                text = TextTools.FormatString(text, vars);
                if (!string.IsNullOrEmpty(text) && finalWidth > 3)
                {
                    string finalText =
                        $"{(settings.BorderRightHorizontalIntersectionEnabled ? $"{settings.BorderRightHorizontalIntersectionChar} " : "")}" +
                        text.Truncate(finalWidth) +
                        $"{(settings.BorderLeftHorizontalIntersectionEnabled ? $" {settings.BorderLeftHorizontalIntersectionChar}" : "")}";
                    var titleWriter = new AlignedText()
                    {
                        Text = finalText,
                        LeftMargin = Left + 2,
                        RightMargin = ConsoleWrapper.WindowWidth - (Left + finalWidth + 7),
                        Top = Top,
                        ForegroundColor = TextColor,
                        BackgroundColor = BackgroundColor,
                        Settings = textSettings,
                    };
                    frameBuilder.Append(titleWriter.Render());
                }

                // Write the resulting buffer
                if (useColor)
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
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        /// <summary>
        /// Makes a new instance of the box renderer
        /// </summary>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public BoxFrame(Mark? text = null, params object[] vars)
        {
            this.text = TextTools.FormatString(text ?? "", vars);
        }
    }
}
