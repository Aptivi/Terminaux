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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Border renderable
    /// </summary>
    public class Border : GraphicalCyclicWriter
    {
        private int left = 0;
        private int top = 0;
        private string title = "";
        private string text = "";
        private int width = 0;
        private int height = 0;
        private Color borderColor = ThemeColorsTools.GetColor(ThemeColorType.Separator);
        private Color textColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private BorderSettings settings = new();
        private TextSettings textSettings = new();
        private bool customPos = false;
        private bool useColors = true;

        /// <summary>
        /// Left position
        /// </summary>
        public override int Left
        {
            get => left;
            set
            {
                left = value;
                customPos = true;
            }
        }

        /// <summary>
        /// Top position
        /// </summary>
        public override int Top
        {
            get => top;
            set
            {
                top = value;
                customPos = true;
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value;
        }

        /// <summary>
        /// Text to show
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Interior width
        /// </summary>
        public override int Width
        {
            get => width;
            set
            {
                width = value;
                if (!customPos)
                    UpdateInternalTop();
            }
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public override int Height
        {
            get => height;
            set
            {
                height = value;
                if (!customPos)
                    UpdateInternalTop();
            }
        }

        /// <summary>
        /// Border color
        /// </summary>
        public Color Color
        {
            get => borderColor;
            set => borderColor = value;
        }

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get => textColor;
            set => textColor = value;
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
        /// Text settings to use
        /// </summary>
        public TextSettings TextSettings
        {
            get => textSettings;
            set => textSettings = value;
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
        /// Rulers that divide the border
        /// </summary>
        public RulerInfo[] Rulers { get; set; } = [];

        /// <summary>
        /// Renders a border
        /// </summary>
        /// <returns>Rendered border that will be used by the renderer</returns>
        public override string Render()
        {
            if (!customPos)
                UpdateInternalTop();

            StringBuilder border = new();
            try
            {
                // StringBuilder to put out the final rendering text
                var boxFrame = new BoxFrame()
                {
                    Text = title,
                    Left = Left,
                    Top = Top,
                    Width = Width,
                    Height = Height,
                    Settings = settings,
                    TitleSettings = textSettings,
                    UseColors = UseColors,
                    DropShadow = DropShadow,
                    ShadowColor = ShadowColor,
                    Rulers = Rulers,
                };
                var box = new Box()
                {
                    Left = Left + 1,
                    Top = Top + 1,
                    Width = Width,
                    Height = Height,
                    UseColors = UseColors,
                };
                if (UseColors)
                {
                    box.Color = BackgroundColor;
                    boxFrame.BackgroundColor = BackgroundColor;
                    boxFrame.FrameColor = Color;
                    boxFrame.TitleColor = TextColor;
                }
                border.Append(
                    box.Render() +
                    boxFrame.Render()
                );

                // Wrap the sentences to fit the box
                if (!string.IsNullOrWhiteSpace(text))
                {
                    // Get the current foreground color
                    if (UseColors)
                    {
                        border.Append(
                            ConsoleColoring.RenderSetConsoleColor(TextColor) +
                            ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }

                    // Now, split the sentences and count them to fit the box
                    string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(text, Width);
                    for (int i = 0; i < sentences.Length; i++)
                    {
                        string sentence = sentences[i];
                        if (Top + 1 + i > Top + Height)
                            break;
                        int leftPos = TextWriterTools.DetermineTextAlignment(sentence, Width, textSettings.Alignment, Left);
                        border.Append(
                            TextWriterWhereColor.RenderWherePlain(sentence, leftPos + 1, Top + 1 + i)
                        );
                    }
                }

                // Write the resulting buffer
                if (UseColors)
                {
                    border.Append(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
                    );
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            return border.ToString();
        }

        internal void UpdateInternalTop()
        {
            var splitLines = text.SplitNewLines();
            width = splitLines.Max((str) => str.Length);
            height = splitLines.Length;
            if (width >= ConsoleWrapper.WindowWidth)
                width = ConsoleWrapper.WindowWidth - 4;
            if (height >= ConsoleWrapper.WindowHeight)
                height = ConsoleWrapper.WindowHeight - 4;
            left = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            top = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
        }

        /// <summary>
        /// Makes a new instance of the border renderer
        /// </summary>
        public Border()
        {
            UpdateInternalTop();
        }
    }
}
