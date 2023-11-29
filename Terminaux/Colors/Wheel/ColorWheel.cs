
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
using Terminaux.Base;
using Terminaux.Colors.Accessibility;
using Terminaux.Colors.Selector;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Colors.Wheel
{
    /// <summary>
    /// Color wheel class
    /// </summary>
    [Obsolete($"The legacy color wheel is obsolete. Use {nameof(ColorSelector)} instead.")]
    public static class ColorWheel
    {
        private static double wheelSeverity = 0.60;
        private static ColorType wheelColorMode = ColorType.TrueColor;
        private static int wheelR = 0, wheelG = 128, wheelB = 0;
        private static ConsoleColors wheelColor255 = ConsoleColors.Green;
        private static ConsoleColor wheelColor16 = ConsoleColor.Green;
        private static Color wheelColor = new(wheelR, wheelG, wheelB);
        private static Color wheelColorProtan, wheelColorDeutan, wheelColorTritan;
        private static ColorWheelRgbMode wheelRgbIndicator = ColorWheelRgbMode.Red;
        private static readonly Color infoBoxColorFg = new(ConsoleColors.White);
        private static readonly Color infoBoxColorBg = new(ConsoleColors.DarkRed);

        /// <summary>
        /// Inputs the user for color selection
        /// </summary>
        /// <returns>The color from the user input</returns>
        [Obsolete($"The legacy color wheel is obsolete. Use {nameof(ColorSelector)}.{nameof(ColorSelector.OpenColorSelector)}() instead.")]
        public static Color InputForColor() =>
            InputForColor(new Color(ConsoleColors.White));

        /// <summary>
        /// Inputs the user for color selection
        /// </summary>
        /// <param name="initialColor">Initial color</param>
        /// <returns>The color from the user input</returns>
        [Obsolete($"The legacy color wheel is obsolete. Use {nameof(ColorSelector)}.{nameof(ColorSelector.OpenColorSelector)}({nameof(Color)} initialColor) instead.")]
        public static Color InputForColor(Color initialColor)
        {
            ResetColors(initialColor);
            ConsoleKeyInfo cki = default;
            while (cki.Key != ConsoleKey.Escape && cki.Key != ConsoleKey.Enter)
            {
                RenderWheel();
                cki = ConsoleWrappers.ActionReadKey(true);
                HandleUserInput(cki);
                UpdateColor();
            }
            if (cki.Key == ConsoleKey.Escape)
                ResetColors(initialColor);
            return wheelColor;
        }

        internal static void RenderWheel()
        {
            ConsoleWrappers.ActionCursorVisible(false);
            ConsoleWrappers.ActionClear();

            // Get the box sizes to fit the console. Four boxes are needed to represent all the modes:
            //   - Normal color
            //   - Protanopia color
            //   - Deuteranopia color
            //   - Tritanopia color
            int topLeftCornerPosLeft = 0;
            int topLeftCornerPosTop = 0;
            int boxWidth = ConsoleWrappers.ActionWindowWidth() / 4 - 2;
            int boxHeight = ConsoleWrappers.ActionWindowHeight() / 3 - 2;
            int boxNum;

            // Render all the colors based on the current wheel color
            wheelColorProtan = ColorTools.RenderColorBlindnessAware(wheelColor, Deficiency.Protan, wheelSeverity);
            wheelColorDeutan = ColorTools.RenderColorBlindnessAware(wheelColor, Deficiency.Deutan, wheelSeverity);
            wheelColorTritan = ColorTools.RenderColorBlindnessAware(wheelColor, Deficiency.Tritan, wheelSeverity);

            // Render all the boxes now
            for (boxNum = 0; boxNum < 4; boxNum++)
            {
                int topLeftCornerPosLeftCurrent = (boxWidth + 2) * boxNum + topLeftCornerPosLeft;
                Color color = boxNum == 1 ? wheelColorProtan :
                              boxNum == 2 ? wheelColorDeutan :
                              boxNum == 3 ? wheelColorTritan :
                              wheelColor;
                string mode = boxNum == 1 ? $"Protanopia [{wheelSeverity:n2}]" :
                              boxNum == 2 ? $"Deuteranopia [{wheelSeverity:n2}]" :
                              boxNum == 3 ? $"Tritanopia [{wheelSeverity:n2}]" :
                              "Normal";
                BorderColor.WriteBorder(topLeftCornerPosLeftCurrent, topLeftCornerPosTop, boxWidth, boxHeight, color);
                BoxColor.WriteBox(topLeftCornerPosLeftCurrent + 1, topLeftCornerPosTop, boxWidth, boxHeight, color);
                TextWriterWhereColor.WriteWhereColor(mode, topLeftCornerPosLeftCurrent + boxWidth / 2 - mode.Length / 2, boxHeight + 2, color);
                TextWriterWhereColor.WriteWhereColor(color.PlainSequence, topLeftCornerPosLeftCurrent + boxWidth / 2 - color.PlainSequence.Length / 2, boxHeight + 4, color);
                TextWriterWhereColor.WriteWhereColor(color.Hex, topLeftCornerPosLeftCurrent + boxWidth / 2 - color.Hex.Length / 2, boxHeight + 5, color);
            }

            // Write the RGB adjuster
            string adjusterTop = "  ^  ";
            string adjusterBottom = "  v  ";
            int adjusterLength = 5;
            int greenAdjusterPos = ConsoleWrappers.ActionWindowWidth() / 2 - (adjusterLength + 2) / 2 + 1;
            int blueAdjusterPos = greenAdjusterPos + 7;
            int redAdjusterPos = greenAdjusterPos - 7;
            int adjusterTopTop = ConsoleWrappers.ActionWindowHeight() - 8;
            int adjusterInfoTop = ConsoleWrappers.ActionWindowHeight() - 6;
            int adjusterBottomTop = ConsoleWrappers.ActionWindowHeight() - 4;
            if (wheelColorMode == ColorType.TrueColor)
            {
                Color redColor = new(255, 0, 0);
                Color greenColor = new(0, 255, 0);
                Color blueColor = new(0, 0, 255);
                Color finalForeR = wheelRgbIndicator == ColorWheelRgbMode.Red ? Color.Empty : redColor;
                Color finalBackR = wheelRgbIndicator == ColorWheelRgbMode.Red ? redColor : Color.Empty;
                Color finalForeG = wheelRgbIndicator == ColorWheelRgbMode.Green ? Color.Empty : greenColor;
                Color finalBackG = wheelRgbIndicator == ColorWheelRgbMode.Green ? greenColor : Color.Empty;
                Color finalForeB = wheelRgbIndicator == ColorWheelRgbMode.Blue ? Color.Empty : blueColor;
                Color finalBackB = wheelRgbIndicator == ColorWheelRgbMode.Blue ? blueColor : Color.Empty;
                TextWriterWhereColor.WriteWhereColorBack(adjusterTop, redAdjusterPos, adjusterTopTop, finalForeR, finalBackR);
                TextWriterWhereColor.WriteWhereColor($"{wheelColor.R}", redAdjusterPos + ($"{wheelColor.R}".Length == 1 ? 2 : $"{wheelColor.R}".Length / 2), adjusterInfoTop, redColor);
                TextWriterWhereColor.WriteWhereColorBack(adjusterBottom, redAdjusterPos, adjusterBottomTop, finalForeR, finalBackR);
                TextWriterWhereColor.WriteWhereColorBack(adjusterTop, greenAdjusterPos, adjusterTopTop, finalForeG, finalBackG);
                TextWriterWhereColor.WriteWhereColor($"{wheelColor.G}", greenAdjusterPos + ($"{wheelColor.G}".Length == 1 ? 2 : $"{wheelColor.G}".Length / 2), adjusterInfoTop, greenColor);
                TextWriterWhereColor.WriteWhereColorBack(adjusterBottom, greenAdjusterPos, adjusterBottomTop, finalForeG, finalBackG);
                TextWriterWhereColor.WriteWhereColorBack(adjusterTop, blueAdjusterPos, adjusterTopTop, finalForeB, finalBackB);
                TextWriterWhereColor.WriteWhereColor($"{wheelColor.B}", blueAdjusterPos + ($"{wheelColor.B}".Length == 1 ? 2 : $"{wheelColor.B}".Length / 2), adjusterInfoTop, blueColor);
                TextWriterWhereColor.WriteWhereColorBack(adjusterBottom, blueAdjusterPos, adjusterBottomTop, finalForeB, finalBackB);
            }
            else if (wheelColorMode == ColorType._255Color)
            {
                TextWriterWhereColor.WriteWhereColorBack(adjusterTop, greenAdjusterPos, adjusterTopTop, Color.Empty, wheelColor);
                TextWriterWhereColor.WriteWhereColor($"{wheelColor255} [{(int)wheelColor255}]", ConsoleWrappers.ActionWindowWidth() / 2 - ($"{wheelColor255} [{(int)wheelColor255}]".Length == 1 ? 2 : $"{wheelColor255} [{(int)wheelColor255}]".Length / 2), adjusterInfoTop, wheelColor);
                TextWriterWhereColor.WriteWhereColorBack(adjusterBottom, greenAdjusterPos, adjusterBottomTop, Color.Empty, wheelColor);
            }
            else
            {
                string renderedName = $"{wheelColor16} [{(int)wheelColor16}]";
                TextWriterWhereColor.WriteWhereColorBack(adjusterTop, greenAdjusterPos, adjusterTopTop, Color.Empty, wheelColor);
                TextWriterWhereColor.WriteWhereColor(renderedName, ConsoleWrappers.ActionWindowWidth() / 2 - (renderedName.Length == 1 ? 2 : renderedName.Length / 2), adjusterInfoTop, wheelColor);
                TextWriterWhereColor.WriteWhereColorBack(adjusterBottom, greenAdjusterPos, adjusterBottomTop, Color.Empty, wheelColor);
            }
            TextWriterColor.Write("\n");

            // Write the bound keys list
            string keysStr = "[ESC] Exit | [ENTER] Accept | [I] Info | [H] Help";
            TextWriterWhereColor.WriteWhereColor(keysStr, ConsoleWrappers.ActionWindowWidth() / 2 - keysStr.Length / 2, ConsoleWrappers.ActionWindowHeight() - 2, new Color(ConsoleColors.White));
        }

        internal static void UpdateColor()
        {
            switch (wheelColorMode)
            {
                case ColorType.TrueColor:
                    wheelColor = new Color(wheelR, wheelG, wheelB);
                    break;
                case ColorType._255Color:
                    wheelColor = new Color(wheelColor255);
                    break;
                case ColorType._16Color:
                    wheelColor = new Color(wheelColor16);
                    break;
            }
        }

        internal static void HandleUserInput(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                // Switch color mode
                case ConsoleKey.Tab:
                    if (wheelColorMode == ColorType._16Color)
                        wheelColorMode = ColorType.TrueColor;
                    else
                        wheelColorMode++;
                    break;
                case ConsoleKey.UpArrow:
                    IncrementColor();
                    break;
                case ConsoleKey.DownArrow:
                    DecrementColor();
                    break;
                case ConsoleKey.RightArrow:
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
                        IncrementSeverity();
                    else
                        IncrementRgbIndicator();
                    break;
                case ConsoleKey.LeftArrow:
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
                        DecrementSeverity();
                    else
                        DecrementRgbIndicator();
                    break;
                case ConsoleKey.H:
                    InfoBoxColor.WriteInfoBoxColorBack(
                        """
                        Controls
                        --------

                        [ESC]       | Exits the color selection
                        [ENTER]     | Accepts the color selection
                        [<-]        | Goes to the previous RGB level
                        [->]        | Goes to the next RGB level
                        [CTRL + <-] | Decreases the color-blind severity
                        [CTRL + ->] | Increases the color-blind severity
                        [TAB]       | Changes the color mode (16, 255, true)
                        [UP]        | Increases the current RGB value of color
                        [DOWN]      | Decreases the current RGB value of color
                        [I]         | Gives you more information about the colors used
                        """,
                        infoBoxColorFg, infoBoxColorBg);
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);
                    break;
                case ConsoleKey.I:
                    InfoBoxColor.WriteInfoBoxColorBack(
                        $$"""
                        Color info
                        ----------

                        RGB level:        {{wheelColor.PlainSequence}}
                        RGB level (true): {{wheelColor.PlainSequenceTrueColor}}
                        RGB hex code:     {{wheelColor.Hex}}
                        Color type:       {{wheelColor.Type}}

                        CMYK information:
                          - Black key:    {{wheelColor.CMYK.KWhole}}
                          - Cyan:         {{wheelColor.CMYK.CMY.CWhole}}
                          - Magenta:      {{wheelColor.CMYK.CMY.MWhole}}
                          - Yellow:       {{wheelColor.CMYK.CMY.YWhole}}

                        HSL information:
                          - Hue (degs):   {{wheelColor.HSL.HueWhole}}'
                          - Reverse Hue   {{wheelColor.HSL.ReverseHueWhole}}'
                          - Saturation:   {{wheelColor.HSL.SaturationWhole}}
                          - Lightness:    {{wheelColor.HSL.LightnessWhole}}
                        """,
                        infoBoxColorFg, infoBoxColorBg);
                    InfoBoxColor.WriteInfoBoxColorBack(
                        $$"""
                        Color info (Protan)
                        -------------------

                        RGB level:        {{wheelColorProtan.PlainSequence}}
                        RGB level (true): {{wheelColorProtan.PlainSequenceTrueColor}}
                        RGB hex code:     {{wheelColorProtan.Hex}}
                        Color type:       {{wheelColorProtan.Type}}

                        CMYK information:
                          - Black key:    {{wheelColorProtan.CMYK.KWhole}}
                          - Cyan:         {{wheelColorProtan.CMYK.CMY.CWhole}}
                          - Magenta:      {{wheelColorProtan.CMYK.CMY.MWhole}}
                          - Yellow:       {{wheelColorProtan.CMYK.CMY.YWhole}}

                        HSL information:
                          - Hue (degs):   {{wheelColorProtan.HSL.HueWhole}}'
                          - Reverse Hue   {{wheelColorProtan.HSL.ReverseHueWhole}}'
                          - Saturation:   {{wheelColorProtan.HSL.SaturationWhole}}
                          - Lightness:    {{wheelColorProtan.HSL.LightnessWhole}}
                        """,
                        infoBoxColorFg, infoBoxColorBg);
                    InfoBoxColor.WriteInfoBoxColorBack(
                        $$"""
                        Color info (Deutan)
                        -------------------

                        RGB level:        {{wheelColorDeutan.PlainSequence}}
                        RGB level (true): {{wheelColorDeutan.PlainSequenceTrueColor}}
                        RGB hex code:     {{wheelColorDeutan.Hex}}
                        Color type:       {{wheelColorDeutan.Type}}

                        CMYK information:
                          - Black key:    {{wheelColorDeutan.CMYK.KWhole}}
                          - Cyan:         {{wheelColorDeutan.CMYK.CMY.CWhole}}
                          - Magenta:      {{wheelColorDeutan.CMYK.CMY.MWhole}}
                          - Yellow:       {{wheelColorDeutan.CMYK.CMY.YWhole}}

                        HSL information:
                          - Hue (degs):   {{wheelColorDeutan.HSL.HueWhole}}'
                          - Reverse Hue   {{wheelColorDeutan.HSL.ReverseHueWhole}}'
                          - Saturation:   {{wheelColorDeutan.HSL.SaturationWhole}}
                          - Lightness:    {{wheelColorDeutan.HSL.LightnessWhole}}
                        """,
                        infoBoxColorFg, infoBoxColorBg);
                    InfoBoxColor.WriteInfoBoxColorBack(
                        $$"""
                        Color info (Tritan)
                        -------------------

                        RGB level:        {{wheelColorTritan.PlainSequence}}
                        RGB level (true): {{wheelColorTritan.PlainSequenceTrueColor}}
                        RGB hex code:     {{wheelColorTritan.Hex}}
                        Color type:       {{wheelColorTritan.Type}}

                        CMYK information:
                          - Black key:    {{wheelColorTritan.CMYK.KWhole}}
                          - Cyan:         {{wheelColorTritan.CMYK.CMY.CWhole}}
                          - Magenta:      {{wheelColorTritan.CMYK.CMY.MWhole}}
                          - Yellow:       {{wheelColorTritan.CMYK.CMY.YWhole}}

                        HSL information:
                          - Hue (degs):   {{wheelColorTritan.HSL.HueWhole}}'
                          - Reverse Hue   {{wheelColorTritan.HSL.ReverseHueWhole}}'
                          - Saturation:   {{wheelColorTritan.HSL.SaturationWhole}}
                          - Lightness:    {{wheelColorTritan.HSL.LightnessWhole}}
                        """,
                        infoBoxColorFg, infoBoxColorBg);
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);
                    break;
            }
        }

        private static void IncrementRgbIndicator()
        {
            wheelRgbIndicator++;
            if (wheelRgbIndicator > ColorWheelRgbMode.Blue)
                wheelRgbIndicator = ColorWheelRgbMode.Red;
        }

        private static void DecrementRgbIndicator()
        {
            wheelRgbIndicator--;
            if (wheelRgbIndicator < ColorWheelRgbMode.Red)
                wheelRgbIndicator = ColorWheelRgbMode.Blue;
        }

        private static void IncrementSeverity()
        {
            wheelSeverity += 0.01;
            if (wheelSeverity > 1)
                wheelSeverity = 0;
        }

        private static void DecrementSeverity()
        {
            wheelSeverity -= 0.01;
            if (wheelSeverity < 0)
                wheelSeverity = 1;
        }

        private static void IncrementColor()
        {
            switch (wheelColorMode)
            {
                case ColorType.TrueColor:
                    switch (wheelRgbIndicator)
                    {
                        case ColorWheelRgbMode.Red:
                            wheelR++;
                            if (wheelR > 255)
                                wheelR = 0;
                            break;
                        case ColorWheelRgbMode.Green:
                            wheelG++;
                            if (wheelG > 255)
                                wheelG = 0;
                            break;
                        case ColorWheelRgbMode.Blue:
                            wheelB++;
                            if (wheelB > 255)
                                wheelB = 0;
                            break;
                    }
                    break;
                case ColorType._255Color:
                    if (wheelColor255 == ConsoleColors.Grey93)
                        wheelColor255 = ConsoleColors.Black;
                    else
                        wheelColor255++;
                    break;
                case ColorType._16Color:
                    if (wheelColor16 == ConsoleColor.White)
                        wheelColor16 = ConsoleColor.Black;
                    else
                        wheelColor16++;
                    break;
            }
        }

        private static void DecrementColor()
        {
            switch (wheelColorMode)
            {
                case ColorType.TrueColor:
                    switch (wheelRgbIndicator)
                    {
                        case ColorWheelRgbMode.Red:
                            wheelR--;
                            if (wheelR < 0)
                                wheelR = 255;
                            break;
                        case ColorWheelRgbMode.Green:
                            wheelG--;
                            if (wheelG < 0)
                                wheelG = 255;
                            break;
                        case ColorWheelRgbMode.Blue:
                            wheelB--;
                            if (wheelB < 0)
                                wheelB = 255;
                            break;
                    }
                    break;
                case ColorType._255Color:
                    if (wheelColor255 == ConsoleColors.Black)
                        wheelColor255 = ConsoleColors.Grey93;
                    else
                        wheelColor255--;
                    break;
                case ColorType._16Color:
                    if (wheelColor16 == ConsoleColor.Black)
                        wheelColor16 = ConsoleColor.White;
                    else
                        wheelColor16--;
                    break;
            }
        }

        private static void ResetColors(Color initialColor = null)
        {
            bool fallback = initialColor is null;
            wheelColorMode = !fallback ? initialColor.Type : ColorType.TrueColor;
            wheelR = !fallback ? initialColor.R : 0;
            wheelG = !fallback ? initialColor.G : 128;
            wheelB = !fallback ? initialColor.B : 0;
            wheelColor255 = !fallback ? initialColor.Type == ColorType._255Color ? initialColor.ColorEnum255 : ConsoleColors.Green : ConsoleColors.Green;
            wheelColor16 = !fallback ? initialColor.Type == ColorType._16Color ? initialColor.ColorEnum16 : ConsoleColor.Green : ConsoleColor.Green;
            wheelColor = !fallback ? initialColor : new(wheelR, wheelG, wheelB);
        }
    }
}
