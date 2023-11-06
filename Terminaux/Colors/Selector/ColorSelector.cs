﻿
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
using Terminaux.Colors.Accessibility;
using Terminaux.Reader.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

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
        private static bool refresh = true;
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
            try
            {
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
                        throw new Exception("Invalid color type in the color selector");
                }
                UpdateColor(ref selectedColor, type);

                // Now, the selector main loop
                bool bail = false;
                while (!bail)
                {
                    // We need to refresh the screen if it's required
                    if (refresh)
                    {
                        ConsoleWrappers.ActionCursorVisible(false);
                        refresh = false;
                        ColorTools.LoadBack();
                    }

                    // Now, render the selector and handle input
                    switch (type)
                    {
                        case ColorType.TrueColor:
                            RenderTrueColorSelector(selectedColor);
                            bail = HandleKeypressTrueColor(ref selectedColor, ref type);
                            break;
                        case ColorType._255Color:
                            Render255ColorsSelector(selectedColor);
                            bail = HandleKeypress255Colors(ref selectedColor, ref type);
                            break;
                        case ColorType._16Color:
                            Render16ColorsSelector(selectedColor);
                            bail = HandleKeypress16Colors(ref selectedColor, ref type);
                            break;
                        default:
                            throw new Exception("Invalid color type in the color selector");
                    }
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
                refresh = true;
                if (!save)
                {
                    save = true;
                    selectedColor = initialColor;
                }
            }
            return selectedColor;
        }

        private static void RenderTrueColorSelector(Color selectedColor)
        {
            // First, render the preview box
            RenderPreviewBox(selectedColor);

            // Then, render the hue, saturation, and lightness bars
            int hueBarX = ConsoleWrappers.ActionWindowWidth() / 2 + 2;
            int hueBarY = 1;
            int saturationBarY = 5;
            int lightnessBarY = 9;
            int rgbRampBarY = 13;
            int grayRampBarY = 19;
            int boxWidth = ConsoleWrappers.ActionWindowWidth() / 2 - 6;
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

            // Buffer the saturation ramp
            StringBuilder satRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int sat = (int)(100 * width);
                satRamp.Append($"{new Color($"hsl:{trueColorHue};{sat};50").VTSequenceBackgroundTrueColor} {initialBackground}");
            }

            // Buffer the lightness ramp
            StringBuilder ligRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int lig = (int)(100 * width);
                ligRamp.Append($"{new Color($"hsl:{trueColorHue};100;{lig}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }

            // Buffer the gray ramp
            StringBuilder grayRamp = new();
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Monochromacy, 0.6);
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int gray = (int)(mono.R * width);
                grayRamp.Append($"{new Color($"{gray};{gray};{gray}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }

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

            // then, the boxes
            BoxFrameTextColor.WriteBoxFrame($"Hue: {trueColorHue}/360", hueBarX, hueBarY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(hueRamp.ToString(), hueBarX + 1, hueBarY + 1);
            BoxFrameTextColor.WriteBoxFrame($"Saturation: {trueColorSaturation}/100", hueBarX, saturationBarY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(satRamp.ToString(), hueBarX + 1, saturationBarY + 1);
            BoxFrameTextColor.WriteBoxFrame($"Lightness: {trueColorLightness}/100", hueBarX, lightnessBarY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(ligRamp.ToString(), hueBarX + 1, lightnessBarY + 1);
            BoxFrameTextColor.WriteBoxFrame($"Red, Green, and Blue: {selectedColor}", hueBarX, rgbRampBarY, boxWidth, boxHeight + 2);
            TextWriterWhereColor.WriteWhere(redRamp.ToString(), hueBarX + 1, rgbRampBarY + 1);
            TextWriterWhereColor.WriteWhere(greenRamp.ToString(), hueBarX + 1, rgbRampBarY + 2);
            TextWriterWhereColor.WriteWhere(blueRamp.ToString(), hueBarX + 1, rgbRampBarY + 3);
            BoxFrameTextColor.WriteBoxFrame($"Grayscale: {mono}", hueBarX, grayRampBarY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(grayRamp.ToString(), hueBarX + 1, grayRampBarY + 1);

            // Finally, the keybindings
            int bindingsPos = ConsoleWrappers.ActionWindowHeight() - 2;
            CenteredTextColor.WriteCentered(bindingsPos, $"[ENTER] Accept color - [H] Help - [ESC] Exit");
        }

        private static void Render255ColorsSelector(Color selectedColor)
        {
            // First, render the preview box
            RenderPreviewBox(selectedColor);

            // Then, render the color info
            int infoBoxX = ConsoleWrappers.ActionWindowWidth() / 2 + 2;
            int infoBoxY = 1;
            int boxWidth = ConsoleWrappers.ActionWindowWidth() / 2 - 6;
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

            // then, the boxes
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Monochromacy, 0.6);
            BoxFrameTextColor.WriteBoxFrame($"Info for: {colorValue255}", infoBoxX, infoBoxY, boxWidth, boxHeight);
            BoxColor.WriteBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere($"Color ID: {(int)colorValue255}", infoBoxX + 1, infoBoxY + 1);
            TextWriterWhereColor.WriteWhere($"Hex: {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2);
            TextWriterWhereColor.WriteWhere($"RGB sequence: {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3);
            TextWriterWhereColor.WriteWhere($"RGB sequence (real): {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4);
            TextWriterWhereColor.WriteWhere($"CMYK: {selectedColor.CMYK.CMY}", infoBoxX + 1, infoBoxY + 5);
            TextWriterWhereColor.WriteWhere($"CMY: {selectedColor.CMY}", infoBoxX + 1, infoBoxY + 6);
            TextWriterWhereColor.WriteWhere($"HSL: {selectedColor.HSL}", infoBoxX + 1, infoBoxY + 7);
            TextWriterWhereColor.WriteWhere($"HSV: {selectedColor.HSV}", infoBoxX + 1, infoBoxY + 8);
            TextWriterWhereColor.WriteWhere($"Grayscale: {mono}", infoBoxX + 1, infoBoxY + 9);
            BoxFrameTextColor.WriteBoxFrame($"Red, Green, and Blue: {selectedColor}", infoBoxX, rgbRampBarY, boxWidth, 3);
            TextWriterWhereColor.WriteWhere(redRamp.ToString(), infoBoxX + 1, rgbRampBarY + 1);
            TextWriterWhereColor.WriteWhere(greenRamp.ToString(), infoBoxX + 1, rgbRampBarY + 2);
            TextWriterWhereColor.WriteWhere(blueRamp.ToString(), infoBoxX + 1, rgbRampBarY + 3);

            // Finally, the keybindings
            int bindingsPos = ConsoleWrappers.ActionWindowHeight() - 2;
            CenteredTextColor.WriteCentered(bindingsPos, $"[ENTER] Accept color - [H] Help - [ESC] Exit");
        }

        private static void Render16ColorsSelector(Color selectedColor)
        {
            // First, render the preview box
            RenderPreviewBox(selectedColor);

            // Then, render the color info
            int infoBoxX = ConsoleWrappers.ActionWindowWidth() / 2 + 2;
            int infoBoxY = 1;
            int boxWidth = ConsoleWrappers.ActionWindowWidth() / 2 - 6;
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

            // then, the boxes
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Monochromacy, 0.6);
            BoxFrameTextColor.WriteBoxFrame($"Info for: {colorValue16}", infoBoxX, infoBoxY, boxWidth, boxHeight);
            BoxColor.WriteBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere($"Color ID: {(int)colorValue16}", infoBoxX + 1, infoBoxY + 1);
            TextWriterWhereColor.WriteWhere($"Hex: {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2);
            TextWriterWhereColor.WriteWhere($"RGB sequence: {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3);
            TextWriterWhereColor.WriteWhere($"RGB sequence (real): {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4);
            TextWriterWhereColor.WriteWhere($"CMYK: {selectedColor.CMYK}", infoBoxX + 1, infoBoxY + 5);
            TextWriterWhereColor.WriteWhere($"CMY: {selectedColor.CMY}", infoBoxX + 1, infoBoxY + 6);
            TextWriterWhereColor.WriteWhere($"HSL: {selectedColor.HSL}", infoBoxX + 1, infoBoxY + 7);
            TextWriterWhereColor.WriteWhere($"HSV: {selectedColor.HSV}", infoBoxX + 1, infoBoxY + 8);
            TextWriterWhereColor.WriteWhere($"Grayscale: {mono}", infoBoxX + 1, infoBoxY + 9);
            BoxFrameTextColor.WriteBoxFrame($"Red, Green, and Blue: {selectedColor}", infoBoxX, rgbRampBarY, boxWidth, 3);
            TextWriterWhereColor.WriteWhere(redRamp.ToString(), infoBoxX + 1, rgbRampBarY + 1);
            TextWriterWhereColor.WriteWhere(greenRamp.ToString(), infoBoxX + 1, rgbRampBarY + 2);
            TextWriterWhereColor.WriteWhere(blueRamp.ToString(), infoBoxX + 1, rgbRampBarY + 3);

            // Finally, the keybindings
            int bindingsPos = ConsoleWrappers.ActionWindowHeight() - 2;
            CenteredTextColor.WriteCentered(bindingsPos, $"[ENTER] Accept color - [H] Help - [ESC] Exit");
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
                    refresh = true;
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
                    refresh = true;
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    refresh = true;
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
                    refresh = true;
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
                    refresh = true;
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    refresh = true;
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
                    refresh = true;
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
                    refresh = true;
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    refresh = true;
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

        private static void RenderPreviewBox(Color selectedColor)
        {
            // Draw the box that represents the currently selected color
            int boxX = 2;
            int boxY = 1;
            int boxWidth = ConsoleWrappers.ActionWindowWidth() / 2 - 4;
            int boxHeight = ConsoleWrappers.ActionWindowHeight() - 6;

            // First, draw the border
            BoxFrameTextColor.WriteBoxFrame($"{selectedColor.PlainSequence} [{selectedColor.PlainSequenceTrueColor}, orig: {selectedColor.PlainSequenceOriginal}]", boxX, boxY, boxWidth, boxHeight);

            // then, the box
            BoxColor.WriteBox(boxX + 1, boxY, boxWidth, boxHeight, selectedColor);
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
            var selectedColorProtan = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Protan, 0.6);
            var selectedColorDeutan = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Deutan, 0.6);
            var selectedColorTritan = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Tritan, 0.6);
            var selectedColorMonochromacy = ColorTools.RenderColorBlindnessAware(selectedColor, Deficiency.Monochromacy, 0.6);
            InfoBoxColor.WriteInfoBox(
                    $$"""
                    Color info
                    ----------

                    RGB level:        {{selectedColor.PlainSequence}}
                    RGB level (true): {{selectedColor.PlainSequenceTrueColor}}
                    RGB hex code:     {{selectedColor.Hex}}
                    Color type:       {{selectedColor.Type}}

                    CMYK information:      | CMY information:
                      - Black key:    {{selectedColor.CMYK.KWhole,3}}  | - Cyan:        {{selectedColor.CMY.CWhole,3}}
                      - Cyan:         {{selectedColor.CMYK.CMY.CWhole,3}}  | - Magenta:     {{selectedColor.CMY.CWhole,3}}
                      - Magenta:      {{selectedColor.CMYK.CMY.MWhole,3}}  | - Yellow:      {{selectedColor.CMY.CWhole,3}}
                      - Yellow:       {{selectedColor.CMYK.CMY.YWhole,3}}  |
                    -----------------------+---------------------
                    HSL information:       | HSV information:
                      - Hue (degs):   {{selectedColor.HSL.HueWhole,3}}' | - Hue (degs):  {{selectedColor.HSV.HueWhole,3}}' 
                      - Reverse Hue:  {{selectedColor.HSL.ReverseHueWhole,3}}' | - Reverse Hue: {{selectedColor.HSV.ReverseHueWhole,3}}' 
                      - Saturation:   {{selectedColor.HSL.SaturationWhole,3}}  | - Saturation:  {{selectedColor.HSV.SaturationWhole,3}}  
                      - Lightness:    {{selectedColor.HSL.LightnessWhole,3}}  | - Value:       {{selectedColor.HSV.ValueWhole,3}}  
                    """
            );
            InfoBoxColor.WriteInfoBox(
                    $$"""
                    Color info (Protan)
                    -------------------

                    RGB level:        {{selectedColorProtan.PlainSequence}}
                    RGB level (true): {{selectedColorProtan.PlainSequenceTrueColor}}
                    RGB hex code:     {{selectedColorProtan.Hex}}
                    Color type:       {{selectedColorProtan.Type}}

                    CMYK information:      | CMY information:
                      - Black key:    {{selectedColorProtan.CMYK.KWhole,3}}  | - Cyan:        {{selectedColorProtan.CMY.CWhole,3}}
                      - Cyan:         {{selectedColorProtan.CMYK.CMY.CWhole,3}}  | - Magenta:     {{selectedColorProtan.CMY.CWhole,3}}
                      - Magenta:      {{selectedColorProtan.CMYK.CMY.MWhole,3}}  | - Yellow:      {{selectedColorProtan.CMY.CWhole,3}}
                      - Yellow:       {{selectedColorProtan.CMYK.CMY.YWhole,3}}  |
                    -----------------------+---------------------
                    HSL information:       | HSV information:
                      - Hue (degs):   {{selectedColorProtan.HSL.HueWhole,3}}' | - Hue (degs):  {{selectedColorProtan.HSV.HueWhole,3}}' 
                      - Reverse Hue:  {{selectedColorProtan.HSL.ReverseHueWhole,3}}' | - Reverse Hue: {{selectedColorProtan.HSV.ReverseHueWhole,3}}' 
                      - Saturation:   {{selectedColorProtan.HSL.SaturationWhole,3}}  | - Saturation:  {{selectedColorProtan.HSV.SaturationWhole,3}}  
                      - Lightness:    {{selectedColorProtan.HSL.LightnessWhole,3}}  | - Value:       {{selectedColorProtan.HSV.ValueWhole,3}}  
                    """
            );
            InfoBoxColor.WriteInfoBox(
                    $$"""
                    Color info (Deutan)
                    -------------------

                    RGB level:        {{selectedColorDeutan.PlainSequence}}
                    RGB level (true): {{selectedColorDeutan.PlainSequenceTrueColor}}
                    RGB hex code:     {{selectedColorDeutan.Hex}}
                    Color type:       {{selectedColorDeutan.Type}}

                    CMYK information:      | CMY information:
                      - Black key:    {{selectedColorDeutan.CMYK.KWhole,3}}  | - Cyan:        {{selectedColorDeutan.CMY.CWhole,3}}
                      - Cyan:         {{selectedColorDeutan.CMYK.CMY.CWhole,3}}  | - Magenta:     {{selectedColorDeutan.CMY.CWhole,3}}
                      - Magenta:      {{selectedColorDeutan.CMYK.CMY.MWhole,3}}  | - Yellow:      {{selectedColorDeutan.CMY.CWhole,3}}
                      - Yellow:       {{selectedColorDeutan.CMYK.CMY.YWhole,3}}  |
                    -----------------------+---------------------
                    HSL information:       | HSV information:
                      - Hue (degs):   {{selectedColorDeutan.HSL.HueWhole,3}}' | - Hue (degs):  {{selectedColorDeutan.HSV.HueWhole,3}}' 
                      - Reverse Hue:  {{selectedColorDeutan.HSL.ReverseHueWhole,3}}' | - Reverse Hue: {{selectedColorDeutan.HSV.ReverseHueWhole,3}}' 
                      - Saturation:   {{selectedColorDeutan.HSL.SaturationWhole,3}}  | - Saturation:  {{selectedColorDeutan.HSV.SaturationWhole,3}}  
                      - Lightness:    {{selectedColorDeutan.HSL.LightnessWhole,3}}  | - Value:       {{selectedColorDeutan.HSV.ValueWhole,3}}  
                    """
            );
            InfoBoxColor.WriteInfoBox(
                    $$"""
                    Color info (Tritan)
                    -------------------

                    RGB level:        {{selectedColorTritan.PlainSequence}}
                    RGB level (true): {{selectedColorTritan.PlainSequenceTrueColor}}
                    RGB hex code:     {{selectedColorTritan.Hex}}
                    Color type:       {{selectedColorTritan.Type}}

                    CMYK information:      | CMY information:
                      - Black key:    {{selectedColorTritan.CMYK.KWhole,3}}  | - Cyan:        {{selectedColorTritan.CMY.CWhole,3}}
                      - Cyan:         {{selectedColorTritan.CMYK.CMY.CWhole,3}}  | - Magenta:     {{selectedColorTritan.CMY.CWhole,3}}
                      - Magenta:      {{selectedColorTritan.CMYK.CMY.MWhole,3}}  | - Yellow:      {{selectedColorTritan.CMY.CWhole,3}}
                      - Yellow:       {{selectedColorTritan.CMYK.CMY.YWhole,3}}  |
                    -----------------------+---------------------
                    HSL information:       | HSV information:
                      - Hue (degs):   {{selectedColorTritan.HSL.HueWhole,3}}' | - Hue (degs):  {{selectedColorTritan.HSV.HueWhole,3}}' 
                      - Reverse Hue:  {{selectedColorTritan.HSL.ReverseHueWhole,3}}' | - Reverse Hue: {{selectedColorTritan.HSV.ReverseHueWhole,3}}' 
                      - Saturation:   {{selectedColorTritan.HSL.SaturationWhole,3}}  | - Saturation:  {{selectedColorTritan.HSV.SaturationWhole,3}}  
                      - Lightness:    {{selectedColorTritan.HSL.LightnessWhole,3}}  | - Value:       {{selectedColorTritan.HSV.ValueWhole,3}}  
                    """
            );
            InfoBoxColor.WriteInfoBox(
                    $$"""
                    Color info (Monochromacy)
                    -------------------------

                    RGB level:        {{selectedColorMonochromacy.PlainSequence}}
                    RGB level (true): {{selectedColorMonochromacy.PlainSequenceTrueColor}}
                    RGB hex code:     {{selectedColorMonochromacy.Hex}}
                    Color type:       {{selectedColorMonochromacy.Type}}

                    CMYK information:      | CMY information:
                      - Black key:    {{selectedColorMonochromacy.CMYK.KWhole,3}}  | - Cyan:        {{selectedColorMonochromacy.CMY.CWhole,3}}
                      - Cyan:         {{selectedColorMonochromacy.CMYK.CMY.CWhole,3}}  | - Magenta:     {{selectedColorMonochromacy.CMY.CWhole,3}}
                      - Magenta:      {{selectedColorMonochromacy.CMYK.CMY.MWhole,3}}  | - Yellow:      {{selectedColorMonochromacy.CMY.CWhole,3}}
                      - Yellow:       {{selectedColorMonochromacy.CMYK.CMY.YWhole,3}}  |
                    -----------------------+---------------------
                    HSL information:       | HSV information:
                      - Hue (degs):   {{selectedColorMonochromacy.HSL.HueWhole,3}}' | - Hue (degs):  {{selectedColorMonochromacy.HSV.HueWhole,3}}' 
                      - Reverse Hue:  {{selectedColorMonochromacy.HSL.ReverseHueWhole,3}}' | - Reverse Hue: {{selectedColorMonochromacy.HSV.ReverseHueWhole,3}}' 
                      - Saturation:   {{selectedColorMonochromacy.HSL.SaturationWhole,3}}  | - Saturation:  {{selectedColorMonochromacy.HSV.SaturationWhole,3}}  
                      - Lightness:    {{selectedColorMonochromacy.HSL.LightnessWhole,3}}  | - Value:       {{selectedColorMonochromacy.HSV.ValueWhole,3}}  
                    """
            );
        }
    }
}
