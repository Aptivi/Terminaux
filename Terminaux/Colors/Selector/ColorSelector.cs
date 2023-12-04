
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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors.Accessibility;
using Terminaux.Reader.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Textify.Sequences.Builder.Types;

namespace Terminaux.Colors.Selector
{
    /// <summary>
    /// Color selection application
    /// </summary>
    public static class ColorSelector
    {
        private static int trueColorHue = 0;
        private static int trueColorSaturation = 100;
        private static int trueColorLightness = 50;
        private static ConsoleColors colorValue255 = ConsoleColors.Magenta;
        private static ConsoleColor colorValue16 = ConsoleColor.Magenta;
        private static bool save = true;

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector() =>
            OpenColorSelector(ConsoleColors.White);

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(ConsoleColors initialColor) =>
            OpenColorSelector(new Color(initialColor));

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(Color initialColor)
        {
            // Initial color is selected
            Color selectedColor = initialColor;
            ColorType type = initialColor.Type;

            // Color selector entry
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            try
            {
                // Make a screen part
                var screenPart = new ScreenPart();

                // Set initial colors
                switch (type)
                {
                    case ColorType.TrueColor:
                        trueColorHue = selectedColor.HSL.HueWhole;
                        trueColorSaturation = selectedColor.HSL.SaturationWhole;
                        trueColorLightness = selectedColor.HSL.LightnessWhole;
                        break;
                    case ColorType._255Color:
                        colorValue255 = selectedColor.ColorEnum255;
                        break;
                    case ColorType._16Color:
                        colorValue16 = selectedColor.ColorEnum16;
                        break;
                    default:
                        throw new TerminauxException("Invalid color type in the color selector");
                }
                UpdateColor(ref selectedColor, type);

                // Now, the selector main loop
                bool bail = false;
                while (!bail)
                {
                    // We need to refresh the screen
                    screenPart.AddText(
                        $"{ColorTools.currentBackgroundColor.VTSequenceBackground}" +
                        $"{CsiSequences.GenerateCsiEraseInDisplay(2)}"
                    );

                    // Now, render the selector and handle input
                    switch (type)
                    {
                        case ColorType.TrueColor:
                            screenPart.AddDynamicText(() =>
                            {
                                ConsoleWrapper.CursorVisible = false;
                                return RenderTrueColorSelector(selectedColor);
                            });
                            screen.AddBufferedPart("Color selector", screenPart);
                            ScreenTools.Render();
                            bail = HandleKeypressTrueColor(ref selectedColor, ref type);
                            break;
                        case ColorType._255Color:
                            screenPart.AddDynamicText(() =>
                            {
                                ConsoleWrapper.CursorVisible = false;
                                return Render255ColorsSelector(selectedColor);
                            });
                            screen.AddBufferedPart("Color selector", screenPart);
                            ScreenTools.Render();
                            bail = HandleKeypress255Colors(ref selectedColor, ref type);
                            break;
                        case ColorType._16Color:
                            screenPart.AddDynamicText(() =>
                            {
                                ConsoleWrapper.CursorVisible = false;
                                return Render16ColorsSelector(selectedColor);
                            });
                            screen.AddBufferedPart("Color selector", screenPart);
                            ScreenTools.Render();
                            bail = HandleKeypress16Colors(ref selectedColor, ref type);
                            break;
                        default:
                            throw new TerminauxException("invalid color type in the color selector");
                    }
                    screenPart.Clear();
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox(
                    $"Color selector has failed: {ex.Message}\n\n" +
                     "Check your input and try again. If it still didn't work, contact us."
                );
            }
            finally
            {
                // Return the selected color
                if (!save)
                {
                    save = true;
                    selectedColor = initialColor;
                }
            }
            ScreenTools.UnsetCurrent(screen);
            return selectedColor;
        }

        private static string RenderTrueColorSelector(Color selectedColor)
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the hue, saturation, and lightness bars
            int hueBarX = (ConsoleWrapper.WindowWidth / 2) + 3;
            int hueBarY = 1;
            int saturationBarY = 5;
            int lightnessBarY = 9;
            int rgbRampBarY = 13;
            int grayRampBarY = 19;
            int boxWidth = (ConsoleWrapper.WindowWidth / 2) - 6;
            int boxHeight = 1;
            var initialBackground = new Color(ConsoleColors.Black).VTSequenceBackground;

            // Buffer the hue ramp
            StringBuilder hueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int hue = (int)(360 * width);
                hueRamp.Append($"{new Color($"hsl:{hue};100;50").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Hue: {trueColorHue}/360", hueBarX, hueBarY, boxWidth, boxHeight) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, hueBarY + 2) +
                hueRamp.ToString()
            );

            // Buffer the saturation ramp
            StringBuilder satRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int sat = (int)(100 * width);
                satRamp.Append($"{new Color($"hsl:{trueColorHue};{sat};50").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Saturation: {trueColorSaturation}/100", hueBarX, saturationBarY, boxWidth, boxHeight) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, saturationBarY + 2) +
                satRamp.ToString()
            );

            // Buffer the lightness ramp
            StringBuilder ligRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int lig = (int)(100 * width);
                ligRamp.Append($"{new Color($"hsl:{trueColorHue};100;{lig}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Lightness: {trueColorLightness}/100", hueBarX, lightnessBarY, boxWidth, boxHeight) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, lightnessBarY + 2) +
                ligRamp.ToString()
            );

            // Buffer the gray ramp
            StringBuilder grayRamp = new();
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Monochromacy, 0.6);
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int gray = (int)(mono.R * width);
                grayRamp.Append($"{new Color($"{gray};{gray};{gray}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Gray: {trueColorLightness}/100", hueBarX, grayRampBarY, boxWidth, boxHeight) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, grayRampBarY + 2) +
                ligRamp.ToString()
            );

            // Buffer the RGB ramp
            StringBuilder redRamp = new();
            StringBuilder greenRamp = new();
            StringBuilder blueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int red = (int)(selectedColor.R * width);
                int green = (int)(selectedColor.G * width);
                int blue = (int)(selectedColor.B * width);
                redRamp.Append($"{new Color($"{red};0;0").VTSequenceBackgroundTrueColor} {initialBackground}");
                greenRamp.Append($"{new Color($"0;{green};0").VTSequenceBackgroundTrueColor} {initialBackground}");
                blueRamp.Append($"{new Color($"0;0;{blue}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame("Red, Green, and Blue: {selectedColor.R};{selectedColor.G};{selectedColor.B}", hueBarX, rgbRampBarY, boxWidth, boxHeight + 2) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 2) +
                redRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 3) +
                greenRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 4) +
                blueRamp.ToString()
            );

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, "[ENTER] Accept color - [H] Help - [ESC] Exit"));
            return selector.ToString();
        }

        private static string Render255ColorsSelector(Color selectedColor)
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the color info
            int infoBoxX = ConsoleWrapper.WindowWidth / 2 + 2;
            int infoBoxY = 1;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6;
            int boxHeight = 9;
            int rgbRampBarY = 13;
            var initialBackground = new Color(ConsoleColors.Black).VTSequenceBackground;

            // Buffer the RGB ramp
            StringBuilder redRamp = new();
            StringBuilder greenRamp = new();
            StringBuilder blueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int red = (int)(selectedColor.R * width);
                int green = (int)(selectedColor.G * width);
                int blue = (int)(selectedColor.B * width);
                redRamp.Append($"{new Color($"{red};0;0").VTSequenceBackgroundTrueColor} {initialBackground}");
                greenRamp.Append($"{new Color($"0;{green};0").VTSequenceBackgroundTrueColor} {initialBackground}");
                blueRamp.Append($"{new Color($"0;0;{blue}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Red, Green, and Blue: {selectedColor.R};{selectedColor.G};{selectedColor.B}", infoBoxX, rgbRampBarY, boxWidth, 3) +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 2) +
                redRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 3) +
                greenRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 4) +
                blueRamp.ToString()
            );

            // then, the boxes
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Monochromacy, 0.6);
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Info for: {colorValue255}", infoBoxX, infoBoxY, boxWidth, boxHeight) +
                BoxColor.RenderBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight) +
                TextWriterWhereColor.RenderWherePlain($"Color ID: {(int)colorValue255}", infoBoxX + 1, infoBoxY + 1) +
                TextWriterWhereColor.RenderWherePlain($"Hex: {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2) +
                TextWriterWhereColor.RenderWherePlain($"RGB sequence: {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3) +
                TextWriterWhereColor.RenderWherePlain($"RGB sequence (real): {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4) +
                TextWriterWhereColor.RenderWherePlain($"CMYK: {selectedColor.CMYK}", infoBoxX + 1, infoBoxY + 5) +
                TextWriterWhereColor.RenderWherePlain($"CMY: {selectedColor.CMY}", infoBoxX + 1, infoBoxY + 6) +
                TextWriterWhereColor.RenderWherePlain($"HSL: {selectedColor.HSL}", infoBoxX + 1, infoBoxY + 7) +
                TextWriterWhereColor.RenderWherePlain($"HSV: {selectedColor.HSV}", infoBoxX + 1, infoBoxY + 8) +
                TextWriterWhereColor.RenderWherePlain($"RYB: {selectedColor.RYB}, Grayscale: {mono}", infoBoxX + 1, infoBoxY + 9)
            );

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, "[ENTER] Accept color - [H] Help - [ESC] Exit"));
            return selector.ToString();
        }

        private static string Render16ColorsSelector(Color selectedColor)
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the color info
            int infoBoxX = ConsoleWrapper.WindowWidth / 2 + 2;
            int infoBoxY = 1;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6;
            int boxHeight = 9;
            int rgbRampBarY = 13;
            var initialBackground = new Color(ConsoleColors.Black).VTSequenceBackground;

            // Buffer the RGB ramp
            StringBuilder redRamp = new();
            StringBuilder greenRamp = new();
            StringBuilder blueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int red = (int)(selectedColor.R * width);
                int green = (int)(selectedColor.G * width);
                int blue = (int)(selectedColor.B * width);
                redRamp.Append($"{new Color($"{red};0;0").VTSequenceBackgroundTrueColor} {initialBackground}");
                greenRamp.Append($"{new Color($"0;{green};0").VTSequenceBackgroundTrueColor} {initialBackground}");
                blueRamp.Append($"{new Color($"0;0;{blue}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Red, Green, and Blue: {selectedColor.R};{selectedColor.G};{selectedColor.B}", infoBoxX, rgbRampBarY, boxWidth, 3) +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 2) +
                redRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 3) +
                greenRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 4) +
                blueRamp.ToString()
            );

            // then, the boxes
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Monochromacy, 0.6);
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame($"Info for: {colorValue16}", infoBoxX, infoBoxY, boxWidth, boxHeight) +
                BoxColor.RenderBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight) +
                TextWriterWhereColor.RenderWherePlain($"Color ID: {(int)colorValue16}", infoBoxX + 1, infoBoxY + 1) +
                TextWriterWhereColor.RenderWherePlain($"Hex: {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2) +
                TextWriterWhereColor.RenderWherePlain($"RGB sequence: {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3) +
                TextWriterWhereColor.RenderWherePlain($"RGB sequence (real): {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4) +
                TextWriterWhereColor.RenderWherePlain($"CMYK: {selectedColor.CMYK}", infoBoxX + 1, infoBoxY + 5) +
                TextWriterWhereColor.RenderWherePlain($"CMY: {selectedColor.CMY}", infoBoxX + 1, infoBoxY + 6) +
                TextWriterWhereColor.RenderWherePlain($"HSL: {selectedColor.HSL}", infoBoxX + 1, infoBoxY + 7) +
                TextWriterWhereColor.RenderWherePlain($"HSV: {selectedColor.HSV}", infoBoxX + 1, infoBoxY + 8) +
                TextWriterWhereColor.RenderWherePlain($"RYB: {selectedColor.RYB}, Grayscale: {mono}", infoBoxX + 1, infoBoxY + 9)
            );

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, "[ENTER] Accept color - [H] Help - [ESC] Exit"));
            return selector.ToString();
        }

        private static bool HandleKeypressTrueColor(ref Color selectedColor, ref ColorType type)
        {
            bool bail = false;
            var keypress = Input.DetectKeypress();
            switch (keypress.Key)
            {
                case ConsoleKey.Tab:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        type--;
                        if (type < ColorType.TrueColor)
                            type = ColorType._16Color;
                    }
                    else
                    {
                        type++;
                        if (type > ColorType._16Color)
                            type = ColorType.TrueColor;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        trueColorLightness--;
                        if (trueColorLightness < 0)
                            trueColorLightness = 100;
                    }
                    else
                    {
                        trueColorHue--;
                        if (trueColorHue < 0)
                            trueColorHue = 360;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        trueColorLightness++;
                        if (trueColorLightness > 100)
                            trueColorLightness = 0;
                    }
                    else
                    {
                        trueColorHue++;
                        if (trueColorHue > 360)
                            trueColorHue = 0;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    trueColorSaturation++;
                    if (trueColorSaturation > 100)
                        trueColorSaturation = 0;
                    break;
                case ConsoleKey.DownArrow:
                    trueColorSaturation--;
                    if (trueColorSaturation < 0)
                        trueColorSaturation = 100;
                    break;
                case ConsoleKey.H:
                    InfoBoxColor.WriteInfoBox(
                        $$"""
                        Available keybindings

                        [ENTER]              | Accept color
                        [ESC]                | Exit
                        [H]                  | Help page
                        [LEFT]               | Reduce hue
                        [CTRL] + [LEFT]      | Reduce lightness
                        [RIGHT]              | Increase hue
                        [CTRL] + [RIGHT]     | Increase lightness
                        [DOWN]               | Reduce saturation
                        [UP]                 | Increase saturation
                        [TAB]                | Change color mode
                        [I]                  | Color information
                        """
                    );
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    break;
                case ConsoleKey.Enter:
                    bail = true;
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    save = false;
                    break;
            }
            UpdateColor(ref selectedColor, type);
            return bail;
        }

        private static bool HandleKeypress255Colors(ref Color selectedColor, ref ColorType type)
        {
            bool bail = false;
            var keypress = Input.DetectKeypress();
            switch (keypress.Key)
            {
                case ConsoleKey.Tab:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        type--;
                        if (type < ColorType.TrueColor)
                            type = ColorType._16Color;
                    }
                    else
                    {
                        type++;
                        if (type > ColorType._16Color)
                            type = ColorType.TrueColor;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    colorValue255--;
                    if (colorValue255 < ConsoleColors.Black)
                        colorValue255 = ConsoleColors.Grey93;
                    break;
                case ConsoleKey.RightArrow:
                    colorValue255++;
                    if (colorValue255 > ConsoleColors.Grey93)
                        colorValue255 = ConsoleColors.Black;
                    break;
                case ConsoleKey.H:
                    InfoBoxColor.WriteInfoBox(
                        $$"""
                        Available keybindings

                        [ENTER]              | Accept color
                        [ESC]                | Exit
                        [H]                  | Help page
                        [LEFT]               | Previous color
                        [RIGHT]              | Next color
                        [TAB]                | Change color mode
                        [I]                  | Color information
                        """
                    );
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    break;
                case ConsoleKey.Enter:
                    bail = true;
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    save = false;
                    break;
            }
            UpdateColor(ref selectedColor, type);
            return bail;
        }

        private static bool HandleKeypress16Colors(ref Color selectedColor, ref ColorType type)
        {
            bool bail = false;
            var keypress = Input.DetectKeypress();
            switch (keypress.Key)
            {
                case ConsoleKey.Tab:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        type--;
                        if (type < ColorType.TrueColor)
                            type = ColorType._16Color;
                    }
                    else
                    {
                        type++;
                        if (type > ColorType._16Color)
                            type = ColorType.TrueColor;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    colorValue16--;
                    if (colorValue16 < ConsoleColor.Black)
                        colorValue16 = ConsoleColor.White;
                    break;
                case ConsoleKey.RightArrow:
                    colorValue16++;
                    if (colorValue16 > ConsoleColor.White)
                        colorValue16 = ConsoleColor.Black;
                    break;
                case ConsoleKey.H:
                    InfoBoxColor.WriteInfoBox(
                        $$"""
                        Available keybindings

                        [ENTER]              | Accept color
                        [ESC]                | Exit
                        [H]                  | Help page
                        [LEFT]               | Previous color
                        [RIGHT]              | Next color
                        [TAB]                | Change color mode
                        [I]                  | Color information
                        """
                    );
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    break;
                case ConsoleKey.Enter:
                    bail = true;
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    save = false;
                    break;
            }
            UpdateColor(ref selectedColor, type);
            return bail;
        }

        private static string RenderPreviewBox(Color selectedColor)
        {
            var builder = new StringBuilder();

            // Draw the box that represents the currently selected color
            int boxX = 2;
            int boxY = 1;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 4;
            int boxHeight = ConsoleWrapper.WindowHeight - 6;

            // First, draw the border
            builder.Append(BoxFrameTextColor.RenderBoxFrame($"{selectedColor.PlainSequence} [{selectedColor.PlainSequenceTrueColor}]", boxX, boxY, boxWidth, boxHeight));

            // then, the box
            builder.Append(
                selectedColor.VTSequenceBackground +
                BoxColor.RenderBox(boxX + 1, boxY, boxWidth, boxHeight) +
                ColorTools.currentBackgroundColor.VTSequenceBackground
            );
            return builder.ToString();
        }

        private static void UpdateColor(ref Color selectedColor, ColorType newType)
        {
            switch (newType)
            {
                case ColorType.TrueColor:
                    selectedColor = new($"hsl:{trueColorHue};{trueColorSaturation};{trueColorLightness}");
                    break;
                case ColorType._255Color:
                    selectedColor = colorValue255;
                    break;
                case ColorType._16Color:
                    selectedColor = colorValue16;
                    break;
            }
        }

        private static void ShowColorInfo(Color selectedColor)
        {
            ShowColorInfoBox(
                "Color info",
                selectedColor
            );
            ShowColorInfoBox(
                "Color info (Protanomaly)",
                selectedColor,
                true
            );
            ShowColorInfoBox(
                "Color info (Protanopia)",
                selectedColor,
                true,
                Deficiency.Protan, 1.0
            );
            ShowColorInfoBox(
                "Color info (Deuteranomaly)",
                selectedColor,
                true,
                Deficiency.Deutan
            );
            ShowColorInfoBox(
                "Color info (Deuteranopia)",
                selectedColor,
                true,
                Deficiency.Deutan, 1.0
            );
            ShowColorInfoBox(
                "Color info (Tritanomaly)",
                selectedColor,
                true,
                Deficiency.Tritan
            );
            ShowColorInfoBox(
                "Color info (Tritanopia)",
                selectedColor,
                true,
                Deficiency.Tritan, 1.0
            );
            ShowColorInfoBox(
                "Color info (Monochromacy)",
                selectedColor,
                true,
                Deficiency.Monochromacy
            );
        }

        private static void ShowColorInfoBox(string localizedTextTitle, Color selectedColor, bool colorBlind = false, Deficiency deficiency = Deficiency.Protan, double severity = 0.6)
        {
            selectedColor =
                colorBlind ?
                ColorTools.RenderColorBlindnessAware(selectedColor, deficiency, severity) :
                selectedColor;
            string separator = new('-', localizedTextTitle.Length);
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{localizedTextTitle}}
                {{separator}}

                RGB level:          {{selectedColor.PlainSequence}}
                RGB level (true):   {{selectedColor.PlainSequenceTrueColor}}
                RGB hex code:       {{selectedColor.Hex}}
                Color type:         {{selectedColor.Type}}
                    
                RYB information:
                    - Red:            {{selectedColor.RYB.R,3}}
                    - Yellow:         {{selectedColor.RYB.Y,3}}
                    - Blue:           {{selectedColor.RYB.B,3}}

                CMYK information:
                    - Black key:      {{selectedColor.CMYK.KWhole,3}}
                    - Cyan:           {{selectedColor.CMYK.CMY.CWhole,3}}
                    - Magenta:        {{selectedColor.CMYK.CMY.MWhole,3}}
                    - Yellow:         {{selectedColor.CMYK.CMY.YWhole,3}}
                    
                CMY information:
                    - Cyan:           {{selectedColor.CMY.CWhole,3}}
                    - Magenta:        {{selectedColor.CMY.CWhole,3}}
                    - Yellow:         {{selectedColor.CMY.CWhole,3}}
                    
                HSL information:
                    - Hue (degs):     {{selectedColor.HSL.HueWhole,3}}'
                    - Reverse Hue:    {{selectedColor.HSL.ReverseHueWhole,3}}'
                    - Saturation:     {{selectedColor.HSL.SaturationWhole,3}}
                    - Lightness:      {{selectedColor.HSL.LightnessWhole,3}}
                    
                HSV information:
                    - Hue (degs):     {{selectedColor.HSV.HueWhole,3}}'
                    - Reverse Hue:    {{selectedColor.HSV.ReverseHueWhole,3}}'
                    - Saturation:     {{selectedColor.HSV.SaturationWhole,3}}
                    - Value:          {{selectedColor.HSV.ValueWhole,3}}
                """
            );
        }
    }
}
