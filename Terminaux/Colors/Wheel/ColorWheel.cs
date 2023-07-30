﻿/*
 * MIT License
 * 
 * Copyright (c) 2023 Aptivi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using Terminaux.Colors.Accessibility;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Colors.Wheel
{
    /// <summary>
    /// Color wheel class
    /// </summary>
    public static class ColorWheel
    {
        private static double wheelSeverity = 0.60;
        private static ColorType wheelColorMode = ColorType.TrueColor;
        private static int wheelR = 0, wheelG = 128, wheelB = 0;
        private static ConsoleColors wheelColor255 = ConsoleColors.Green;
        private static ConsoleColor wheelColor16 = ConsoleColor.Green;
        private static Color wheelColor = new(wheelR, wheelG, wheelB);
        private static int wheelRgbIndicator = 0; // R = 0, G = 1, B = 2
        private static readonly Color infoBoxColorFg = new(ConsoleColors.White);
        private static readonly Color infoBoxColorBg = new(ConsoleColors.DarkRed);

        /// <summary>
        /// Inputs the user for color selection
        /// </summary>
        /// <returns>The color from the user input</returns>
        public static Color InputForColor() =>
            InputForColor(new Color(ConsoleColors.White));

        /// <summary>
        /// Inputs the user for color selection
        /// </summary>
        /// <param name="initialColor">Initial color</param>
        /// <returns>The color from the user input</returns>
        public static Color InputForColor(Color initialColor)
        {
            ResetColors(initialColor);
            ConsoleKeyInfo cki = default;
            while (cki.Key != ConsoleKey.Escape && cki.Key != ConsoleKey.Enter)
            {
                RenderWheel();
                cki = Console.ReadKey(true);
                HandleUserInput(cki);
                UpdateColor();
            }
            if (cki.Key == ConsoleKey.Escape)
                ResetColors(initialColor);
            return wheelColor;
        }

        internal static void RenderWheel()
        {
            Console.CursorVisible = false;
            Console.Clear();

            // Get the box sizes to fit the console. Four boxes are needed to represent all the modes:
            //   - Normal color
            //   - Protanopia color
            //   - Deuteranopia color
            //   - Tritanopia color
            int topLeftCornerPosLeft = 0;
            int topLeftCornerPosTop = 0;
            int boxWidth = Console.WindowWidth / 4 - 2;
            int boxHeight = Console.WindowHeight / 3 - 2;
            int boxNum;

            // Render all the colors based on the current wheel color
            Color wheelColorProtan, wheelColorDeutan, wheelColorTritan;
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiencySeverity = wheelSeverity;
            ColorTools.ColorDeficiency = Deficiency.Protan;
            wheelColorProtan = new Color(wheelColor.PlainSequence);
            ColorTools.ColorDeficiency = Deficiency.Deutan;
            wheelColorDeutan = new Color(wheelColor.PlainSequence);
            ColorTools.ColorDeficiency = Deficiency.Tritan;
            wheelColorTritan = new Color(wheelColor.PlainSequence);
            ColorTools.ColorDeficiency = Deficiency.Protan;
            ColorTools.EnableColorTransformation = false;

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
                string bright = color.IsBright ? "Bright" : "Dark";
                BorderColor.WriteBorder(topLeftCornerPosLeftCurrent, topLeftCornerPosTop, boxWidth, boxHeight, color);
                BoxColor.WriteBox(topLeftCornerPosLeftCurrent + 1, topLeftCornerPosTop, boxWidth, boxHeight, color);
                TextWriterWhereColor.WriteWhere(mode, topLeftCornerPosLeftCurrent + boxWidth / 2 - mode.Length / 2, boxHeight + 2, color);
                TextWriterWhereColor.WriteWhere(color.PlainSequence, topLeftCornerPosLeftCurrent + boxWidth / 2 - color.PlainSequence.Length / 2, boxHeight + 4, color);
                TextWriterWhereColor.WriteWhere(color.Hex, topLeftCornerPosLeftCurrent + boxWidth / 2 - color.Hex.Length / 2, boxHeight + 5, color);
                TextWriterWhereColor.WriteWhere(color.Type.ToString(), topLeftCornerPosLeftCurrent + boxWidth / 2 - color.Type.ToString().Length / 2, boxHeight + 6, color);
                TextWriterWhereColor.WriteWhere(bright, topLeftCornerPosLeftCurrent + boxWidth / 2 - bright.Length / 2, boxHeight + 7, color);
            }

            // Write the RGB adjuster
            string adjusterTop = "  ^  ";
            string adjusterBottom = "  v  ";
            int adjusterLength = 5;
            int greenAdjusterPos = Console.WindowWidth / 2 - (adjusterLength + 2) / 2 + 1;
            int blueAdjusterPos = greenAdjusterPos + 7;
            int redAdjusterPos = greenAdjusterPos - 7;
            int adjusterTopTop = Console.WindowHeight - 8;
            int adjusterInfoTop = Console.WindowHeight - 6;
            int adjusterBottomTop = Console.WindowHeight - 4;
            if (wheelColorMode == ColorType.TrueColor)
            {
                Color redColor = new(255, 0, 0);
                Color greenColor = new(0, 255, 0);
                Color blueColor = new(0, 0, 255);
                TextWriterWhereColor.WriteWhere(adjusterTop, redAdjusterPos, adjusterTopTop, wheelRgbIndicator == 0 ? Color.Empty : redColor, wheelRgbIndicator == 0 ? redColor : Color.Empty);
                TextWriterWhereColor.WriteWhere($"{wheelColor.R}", redAdjusterPos + ($"{wheelColor.R}".Length == 1 ? 2 : $"{wheelColor.R}".Length / 2), adjusterInfoTop, redColor);
                TextWriterWhereColor.WriteWhere(adjusterBottom, redAdjusterPos, adjusterBottomTop, wheelRgbIndicator == 0 ? Color.Empty : redColor, wheelRgbIndicator == 0 ? redColor : Color.Empty);
                TextWriterWhereColor.WriteWhere(adjusterTop, greenAdjusterPos, adjusterTopTop, wheelRgbIndicator == 1 ? Color.Empty : greenColor, wheelRgbIndicator == 1 ? greenColor : Color.Empty);
                TextWriterWhereColor.WriteWhere($"{wheelColor.G}", greenAdjusterPos + ($"{wheelColor.G}".Length == 1 ? 2 : $"{wheelColor.G}".Length / 2), adjusterInfoTop, greenColor);
                TextWriterWhereColor.WriteWhere(adjusterBottom, greenAdjusterPos, adjusterBottomTop, wheelRgbIndicator == 1 ? Color.Empty : greenColor, wheelRgbIndicator == 1 ? greenColor : Color.Empty);
                TextWriterWhereColor.WriteWhere(adjusterTop, blueAdjusterPos, adjusterTopTop, wheelRgbIndicator == 2 ? Color.Empty : blueColor, wheelRgbIndicator == 2 ? blueColor : Color.Empty);
                TextWriterWhereColor.WriteWhere($"{wheelColor.B}", blueAdjusterPos + ($"{wheelColor.B}".Length == 1 ? 2 : $"{wheelColor.B}".Length / 2), adjusterInfoTop, blueColor);
                TextWriterWhereColor.WriteWhere(adjusterBottom, blueAdjusterPos, adjusterBottomTop, wheelRgbIndicator == 2 ? Color.Empty : blueColor, wheelRgbIndicator == 2 ? blueColor : Color.Empty);
            }
            else if (wheelColorMode == ColorType._255Color)
            {
                TextWriterWhereColor.WriteWhere(adjusterTop, greenAdjusterPos, adjusterTopTop, Color.Empty, wheelColor);
                TextWriterWhereColor.WriteWhere($"{wheelColor255} [{(int)wheelColor255}]", Console.WindowWidth / 2 - ($"{wheelColor255} [{(int)wheelColor255}]".Length == 1 ? 2 : $"{wheelColor255} [{(int)wheelColor255}]".Length / 2), adjusterInfoTop, wheelColor);
                TextWriterWhereColor.WriteWhere(adjusterBottom, greenAdjusterPos, adjusterBottomTop, Color.Empty, wheelColor);
            }
            else
            {
                TextWriterWhereColor.WriteWhere(adjusterTop, greenAdjusterPos, adjusterTopTop, Color.Empty, wheelColor);
                TextWriterWhereColor.WriteWhere($"{wheelColor16}  [{(int)wheelColor16}]", Console.WindowWidth / 2 - ($"{wheelColor16} [{(int)wheelColor16}]".Length == 1 ? 2 : $"{wheelColor16} [{(int)wheelColor16}]".Length / 2), adjusterInfoTop, wheelColor);
                TextWriterWhereColor.WriteWhere(adjusterBottom, greenAdjusterPos, adjusterBottomTop, Color.Empty, wheelColor);
            }
            TextWriterColor.Write("\n");

            // Write the bound keys list
            string keysStr = "[ESC] Exit | [ENTER] Accept | [H] Help";
            TextWriterWhereColor.WriteWhere(keysStr, Console.WindowWidth / 2 - keysStr.Length / 2, Console.CursorTop, new Color(ConsoleColors.White));
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
                    wheelColor = new Color((ConsoleColors)wheelColor16);
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
                    InfoBoxColor.WriteInfoBox( 
                        "Controls\n" +
                        "--------\n" +
                        "\n" +
                        "[ESC]       | Exits the color selection\n" +
                        "[ENTER]     | Accepts the color selection\n" +
                        "[<-]        | Goes to the previous RGB level\n" +
                        "[->]        | Goes to the next RGB level\n" +
                        "[CTRL + <-] | Decreases the color-blind severity\n" +
                        "[CTRL + ->] | Increases the color-blind severity\n" +
                        "[TAB]       | Changes the color mode (16, 255, true)\n" +
                        "[UP]        | Increases the current RGB value of color\n" +
                        "[DOWN]      | Decreases the current RGB value of color",
                        infoBoxColorFg, infoBoxColorBg);
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);
                    break;
            }
        }

        private static void IncrementRgbIndicator()
        {
            wheelRgbIndicator++;
            if (wheelRgbIndicator > 2)
                wheelRgbIndicator = 0;
        }

        private static void DecrementRgbIndicator()
        {
            wheelRgbIndicator--;
            if (wheelRgbIndicator < 0)
                wheelRgbIndicator = 2;
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
                        case 0:
                            wheelR++;
                            if (wheelR > 255)
                                wheelR = 0;
                            break;
                        case 1:
                            wheelG++;
                            if (wheelG > 255)
                                wheelG = 0;
                            break;
                        case 2:
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
                        case 0:
                            wheelR--;
                            if (wheelR < 0)
                                wheelR = 255;
                            break;
                        case 1:
                            wheelG--;
                            if (wheelG < 0)
                                wheelG = 255;
                            break;
                        case 2:
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
