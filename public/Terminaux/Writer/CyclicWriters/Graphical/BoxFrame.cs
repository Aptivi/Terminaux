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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
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
        private Color boxFrameColor = ThemeColorsTools.GetColor(ThemeColorType.Separator);
        private Color titleColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
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
        /// Rulers that divide the box frame
        /// </summary>
        public RulerInfo[] Rulers { get; set; } = [];

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
                ConsoleLogger.Debug("Box width: {0}, height: {1}", Width, Height);

                // Text title
                int finalWidth = Width - 6 - (titleSettings.TitleOffset.Left + titleSettings.TitleOffset.Right);
                string finalText = "";
                if (!string.IsNullOrEmpty(text) && finalWidth > 3)
                {
                    var textBuilder = new StringBuilder();
                    textBuilder.Append(settings.BorderRightHorizontalIntersectionEnabled ? $"{settings.BorderRightHorizontalIntersectionChar} " : "");
                    if (UseColors)
                    {
                        textBuilder.Append(
                            ConsoleColoring.RenderSetConsoleColor(TitleColor) +
                            ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    textBuilder.Append(text.Truncate(finalWidth));
                    if (UseColors)
                    {
                        textBuilder.Append(
                            ConsoleColoring.RenderSetConsoleColor(FrameColor) +
                            ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    textBuilder.Append(settings.BorderLeftHorizontalIntersectionEnabled ? $" {settings.BorderLeftHorizontalIntersectionChar}" : "");
                    finalText = textBuilder.ToString();
                }
                int textWidth = ConsoleChar.EstimateCellWidth(finalText);

                // Colors
                if (UseColors)
                {
                    frameBuilder.Append(
                        ConsoleColoring.RenderSetConsoleColor(FrameColor) +
                        ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
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

                // Upper frame (and optionally a title)
                if (settings.BorderUpperFrameEnabled)
                {
                    frameBuilder.Append(CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + 1));
                    int alignmentStart = TextWriterTools.DetermineTextAlignment(finalText, Width - titleSettings.TitleOffset.Right - 2, TitleSettings.Alignment, 1);
                    for (int w = 0; w < Width; w++)
                    {
                        if (w == alignmentStart && textWidth > 3)
                        {
                            frameBuilder.Append(finalText);
                            w += textWidth - 1;
                        }
                        else
                            frameBuilder.Append(settings.BorderUpperFrameChar);
                    }
                }

                // Lower frame
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

                // Any ruler that can be rendered
                List<Coordinate> processedPositions = [];
                foreach (var ruler in Rulers.Distinct())
                {
                    int intersections = 0;
                    switch (ruler.Orientation)
                    {
                        case RulerOrientation.Horizontal:
                            int top = Top + ruler.Position + 1;
                            if (top > Top && top <= Top + Height)
                            {
                                // Render the ruler
                                int i = ruler.InvertDirection ? Width : 1;
                                int length = 0;
                                while ((ruler.InvertDirection && i >= 1) || (!ruler.InvertDirection && i <= Width))
                                {
                                    Coordinate coord = new(Left + i, top);
                                    bool intersected = processedPositions.Contains(coord);
                                    if (intersected)
                                        intersections++;
                                    if (ruler.IntersectionStop > 0 && intersections >= ruler.IntersectionStop)
                                        break;
                                    length++;
                                    char finalIntersectionChar =
                                        intersected ?
                                        (settings.BorderWholeIntersectionEnabled ? settings.BorderWholeIntersectionChar : ' ') :
                                        (settings.BorderHorizontalIntersectionEnabled ? settings.BorderHorizontalIntersectionChar : ' ');
                                    processedPositions.Add(coord);
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(Left + i, top) +
                                        finalIntersectionChar
                                    );
                                    i += ruler.InvertDirection ? -1 : 1;
                                }

                                // Render the final intersection characters
                                if (ruler.InvertDirection)
                                {
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(Left + Width - length, top) +
                                        (settings.BorderLeftHorizontalIntersectionEnabled ? $"{settings.BorderLeftHorizontalIntersectionChar}" : " ")
                                    );
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(Left + Width + 1, top) +
                                        (settings.BorderRightHorizontalIntersectionEnabled ? $"{settings.BorderRightHorizontalIntersectionChar}" : " ")
                                    );
                                }
                                else
                                {
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(Left, top) +
                                        (settings.BorderLeftHorizontalIntersectionEnabled ? $"{settings.BorderLeftHorizontalIntersectionChar}" : " ")
                                    );
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(Left + length + 1, top) +
                                        (settings.BorderRightHorizontalIntersectionEnabled ? $"{settings.BorderRightHorizontalIntersectionChar}" : " ")
                                    );
                                }
                            }
                            break;
                        case RulerOrientation.Vertical:
                            int left = Left + ruler.Position + 1;
                            if (left > Left && left <= Left + Width)
                            {
                                // Render the ruler
                                int i = ruler.InvertDirection ? Height : 1;
                                int length = 0;
                                while ((ruler.InvertDirection && i >= 1) || (!ruler.InvertDirection && i <= Height))
                                {
                                    Coordinate coord = new(left, Top + i);
                                    bool intersected = processedPositions.Contains(coord);
                                    if (intersected)
                                        intersections++;
                                    if (ruler.IntersectionStop > 0 && intersections >= ruler.IntersectionStop)
                                        break;
                                    length++;
                                    char finalIntersectionChar =
                                        processedPositions.Contains(coord) ?
                                        (settings.BorderWholeIntersectionEnabled ? settings.BorderWholeIntersectionChar : ' ') :
                                        (settings.BorderVerticalIntersectionEnabled ? settings.BorderVerticalIntersectionChar : ' ');
                                    processedPositions.Add(coord);
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(left, Top + i) +
                                        finalIntersectionChar
                                    );
                                    i += ruler.InvertDirection ? -1 : 1;
                                }

                                // Render the final intersection characters
                                if (ruler.InvertDirection)
                                {
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(left, Top + Height - length) +
                                        (settings.BorderTopVerticalIntersectionEnabled ? $"{settings.BorderTopVerticalIntersectionChar}" : " ")
                                    );
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(left, Top + Height + 1) +
                                        (settings.BorderBottomVerticalIntersectionEnabled ? $"{settings.BorderBottomVerticalIntersectionChar}" : " ")
                                    );
                                }
                                else
                                {
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(left, Top) +
                                        (settings.BorderTopVerticalIntersectionEnabled ? $"{settings.BorderTopVerticalIntersectionChar}" : " ")
                                    );
                                    frameBuilder.Append(
                                        ConsolePositioning.RenderChangePosition(left, Top + length + 1) +
                                        (settings.BorderBottomVerticalIntersectionEnabled ? $"{settings.BorderBottomVerticalIntersectionChar}" : " ")
                                    );
                                }
                            }
                            break;
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
                                (UseColors ? ConsoleColoring.RenderSetConsoleColor(ShadowColor, true) : "") +
                                $"{CsiSequences.GenerateCsiCursorPosition(Left + Width + 3, Top + i + 1)}" +
                                " "
                            );
                        }
                    }
                    if (settings.BorderLowerFrameEnabled)
                    {
                        frameBuilder.Append(
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(ShadowColor, true) : "") +
                            $"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + Height + 3)}" +
                            $"{new string(' ', Width + 2)}"
                        );
                    }
                }

                // Write the resulting buffer
                if (UseColors)
                {
                    frameBuilder.Append(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
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
