//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Inputs.Styles
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
                var hsl = HslConversionTools.ConvertFrom(selectedColor.RGB);
                switch (type)
                {
                    case ColorType.TrueColor:
                        trueColorHue = hsl.HueWhole;
                        trueColorSaturation = hsl.SaturationWhole;
                        trueColorLightness = hsl.LightnessWhole;
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
                bool refresh = true;
                while (!bail)
                {
                    // Now, render the selector
                    screenPart.AddDynamicText(() =>
                    {
                        ConsoleWrapper.CursorVisible = false;
                        return RenderColorSelector(selectedColor, type);
                    });
                    screen.AddBufferedPart("Color selector", screenPart);
                    ScreenTools.Render();

                    // Handle input
                    bail =
                        type == ColorType.TrueColor || type == ColorType._255Color || type == ColorType._16Color ?
                        HandleKeypress(ref selectedColor, ref type, out refresh) :
                        throw new TerminauxException("Invalid color type in the color selector");
                    if (refresh)
                        screen.RequireRefresh();

                    // Clean up after ourselves
                    screenPart.Clear();
                    screen.RemoveBufferedPart("Color selector");
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

        private static string RenderColorSelector(Color selectedColor, ColorType type)
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the hue, saturation, and lightness bars
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6 + (ConsoleWrapper.WindowWidth % 2 == 0 ? 0 : 1);
            int boxHeight = 2;
            int hueBarX = ConsoleWrapper.WindowWidth / 2 + 2;
            int hueBarY = 1;
            int saturationBarY = hueBarY + boxHeight + 3;
            int lightnessBarY = saturationBarY + boxHeight + 3;
            int grayRampBarY = lightnessBarY + boxHeight + 3;
            int transparencyRampBarY = grayRampBarY + boxHeight + 2;
            int rgbRampBarY = transparencyRampBarY + boxHeight + 2;
            var initialBackground = ColorTools.CurrentBackgroundColor;

            // Buffer the hue ramp
            if (ConsoleWrapper.WindowHeight - 3 > hueBarY + 2)
            {
                int final = type == ColorType.TrueColor ? trueColorHue : HslConversionTools.ConvertFrom(selectedColor.RGB).HueWhole;
                StringBuilder hueRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int hue = (int)(360 * width);
                    hueRamp.Append($"{new Color($"hsl:{hue};100;50").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"Hue: {final}/360", hueBarX, hueBarY, boxWidth, boxHeight) +
                    SliderColor.RenderSlider(final, 360, hueBarX, hueBarY + 1, hueBarX, ConsoleWrapper.WindowWidth - (hueBarX + boxWidth), ColorTools.CurrentForegroundColor, false) +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, hueBarY + 2) +
                    hueRamp.ToString()
                );
            }

            // Buffer the saturation ramp
            if (ConsoleWrapper.WindowHeight - 3 > saturationBarY + 2)
            {
                int final = type == ColorType.TrueColor ? trueColorSaturation : HslConversionTools.ConvertFrom(selectedColor.RGB).SaturationWhole;
                StringBuilder satRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int sat = (int)(100 * width);
                    satRamp.Append($"{new Color($"hsl:{(type == ColorType.TrueColor ? trueColorHue : HslConversionTools.ConvertFrom(selectedColor.RGB).HueWhole)};{sat};50").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"Saturation: {final}/100", hueBarX, saturationBarY, boxWidth, boxHeight) +
                    SliderColor.RenderSlider(final, 100, hueBarX, saturationBarY + 1, hueBarX, ConsoleWrapper.WindowWidth - (hueBarX + boxWidth), ColorTools.CurrentForegroundColor, false) +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, saturationBarY + 2) +
                    satRamp.ToString()
                );
            }

            // Buffer the lightness ramp
            if (ConsoleWrapper.WindowHeight - 3 > lightnessBarY + 2)
            {
                int final = type == ColorType.TrueColor ? trueColorLightness : HslConversionTools.ConvertFrom(selectedColor.RGB).LightnessWhole;
                StringBuilder ligRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int lig = (int)(100 * width);
                    ligRamp.Append($"{new Color($"hsl:{(type == ColorType.TrueColor ? trueColorHue : HslConversionTools.ConvertFrom(selectedColor.RGB).HueWhole)};100;{lig}").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"Lightness: {final}/100", hueBarX, lightnessBarY, boxWidth, boxHeight) +
                    SliderColor.RenderSlider(final, 100, hueBarX, lightnessBarY + 1, hueBarX, ConsoleWrapper.WindowWidth - (hueBarX + boxWidth), ColorTools.CurrentForegroundColor, false) +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, lightnessBarY + 2) +
                    ligRamp.ToString()
                );
            }

            // Buffer the gray ramp
            if (ConsoleWrapper.WindowHeight - 3 > grayRampBarY + 2)
            {
                StringBuilder grayRamp = new();
                var mono = TransformationTools.RenderColorBlindnessAware(selectedColor, TransformationFormula.Monochromacy, 0.6);
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int gray = (int)(mono.RGB.R * width);
                    grayRamp.Append($"{new Color($"{gray};{gray};{gray}").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"Gray: {mono.RGB.R}/255", hueBarX, grayRampBarY, boxWidth, boxHeight - 1) +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, grayRampBarY + 2) +
                    grayRamp.ToString()
                );
            }

            // Buffer the transparency ramp
            if (ConsoleWrapper.WindowHeight - 3 > transparencyRampBarY + 2)
            {
                StringBuilder transparencyRamp = new();
                var mono = TransformationTools.RenderColorBlindnessAware(selectedColor, TransformationFormula.Monochromacy, 0.6);
                for (int i = 0; i < boxWidth - 6; i++)
                {
                    double width = (double)i / boxWidth;
                    int transparency = (int)(mono.RGB.originalAlpha * width);
                    transparencyRamp.Append($"{new Color($"{transparency};{transparency};{transparency}").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"Transparency: {ColorTools.GlobalSettings.Opacity}/255", hueBarX, transparencyRampBarY, boxWidth - 6, boxHeight - 1) +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, transparencyRampBarY + 2) +
                    transparencyRamp.ToString()
                );
            }

            // Buffer the dark/light indicator
            if (ConsoleWrapper.WindowHeight - 3 > transparencyRampBarY + 2)
            {
                StringBuilder darkLightIndicator = new();
                var indicator = selectedColor.Brightness == ColorBrightness.Light ? ConsoleColors.White : ConsoleColors.Black;
                darkLightIndicator.Append($"{new Color(indicator).VTSequenceBackgroundTrueColor}   {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                selector.Append(
                    BoxFrameColor.RenderBoxFrame(ConsoleWrapper.WindowWidth - 7, transparencyRampBarY, 3, 1) +
                    CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 6 + 1, transparencyRampBarY + 2) +
                    darkLightIndicator.ToString()
                );
            }

            // Buffer the RGB ramp
            if (ConsoleWrapper.WindowHeight - 3 > rgbRampBarY + 4)
            {
                StringBuilder redRamp = new();
                StringBuilder greenRamp = new();
                StringBuilder blueRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int red = (int)(selectedColor.RGB.R * width);
                    int green = (int)(selectedColor.RGB.G * width);
                    int blue = (int)(selectedColor.RGB.B * width);
                    redRamp.Append($"{new Color($"{red};0;0").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                    greenRamp.Append($"{new Color($"0;{green};0").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                    blueRamp.Append($"{new Color($"0;0;{blue}").VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"Red, Green, and Blue: {selectedColor.RGB.R};{selectedColor.RGB.G};{selectedColor.RGB.B}", hueBarX, rgbRampBarY, boxWidth, boxHeight + 1) +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 2) +
                    redRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 3) +
                    greenRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 4) +
                    blueRamp.ToString()
                );
            }

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, "[ENTER] Accept - [H] Help - [ESC] Exit"));
            return selector.ToString();
        }

        private static bool HandleKeypress(ref Color selectedColor, ref ColorType type, out bool refresh)
        {
            bool bail = false;
            refresh = false;
            var keypress = TermReader.ReadKey();
            switch (keypress.Key)
            {
                // Unified
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    refresh = true;
                    break;
                case ConsoleKey.V:
                    ShowColorInfoVisually(selectedColor);
                    refresh = true;
                    break;
                case ConsoleKey.Enter:
                    bail = true;
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    save = false;
                    break;
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
                case ConsoleKey.O:
                    ColorTools.GlobalSettings.Opacity++;
                    break;
                case ConsoleKey.P:
                    ColorTools.GlobalSettings.Opacity--;
                    break;

                // Non-unified
                case ConsoleKey.LeftArrow:
                    switch (type)
                    {
                        case ColorType.TrueColor:
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
                        case ColorType._255Color:
                            colorValue255--;
                            if (colorValue255 < ConsoleColors.Black)
                                colorValue255 = ConsoleColors.Grey93;
                            break;
                        case ColorType._16Color:
                            colorValue16--;
                            if (colorValue16 < ConsoleColor.Black)
                                colorValue16 = ConsoleColor.White;
                            break;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    switch (type)
                    {
                        case ColorType.TrueColor:
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
                        case ColorType._255Color:
                            colorValue255++;
                            if (colorValue255 > ConsoleColors.Grey93)
                                colorValue255 = ConsoleColors.Black;
                            break;
                        case ColorType._16Color:
                            colorValue16++;
                            if (colorValue16 > ConsoleColor.White)
                                colorValue16 = ConsoleColor.Black;
                            break;
                    }
                    break;
                case ConsoleKey.H:
                    switch (type)
                    {
                        case ColorType.TrueColor:
                            InfoBoxColor.WriteInfoBox("Available keybindings",
                                $$"""
                                [ENTER]              | Accept color
                                [ESC]                | Exit
                                [H]                  | Help page
                                [LEFT]               | Reduce hue
                                [CTRL] + [LEFT]      | Reduce lightness
                                [RIGHT]              | Increase hue
                                [CTRL] + [RIGHT]     | Increase lightness
                                [DOWN]               | Reduce saturation
                                [UP]                 | Increase saturation
                                [O]                  | Increase opaqueness
                                [P]                  | Increase transparency
                                [TAB]                | Change color mode
                                [I]                  | Color information
                                [V]                  | Color information (visual)
                                """
                            );
                            refresh = true;
                            break;
                        case ColorType._255Color:
                        case ColorType._16Color:
                            InfoBoxColor.WriteInfoBox("Available keybindings",
                                $$"""
                                [ENTER]              | Accept color
                                [ESC]                | Exit
                                [H]                  | Help page
                                [LEFT]               | Previous color
                                [RIGHT]              | Next color
                                [O]                  | Increase opaqueness
                                [P]                  | Increase transparency
                                [TAB]                | Change color mode
                                [I]                  | Color information
                                [V]                  | Color information (visual)
                                """
                            );
                            refresh = true;
                            break;
                    }
                    break;

                // Only for true color
                case ConsoleKey.UpArrow:
                    if (type != ColorType.TrueColor)
                        break;
                    trueColorSaturation++;
                    if (trueColorSaturation > 100)
                        trueColorSaturation = 0;
                    break;
                case ConsoleKey.DownArrow:
                    if (type != ColorType.TrueColor)
                        break;
                    trueColorSaturation--;
                    if (trueColorSaturation < 0)
                        trueColorSaturation = 100;
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
            builder.Append(BoxFrameColor.RenderBoxFrame($"{selectedColor.PlainSequence} [{selectedColor.PlainSequenceTrueColor}{(selectedColor.ColorId is not null ? $" | {selectedColor.ColorId.Name}" : "")}]", boxX, boxY, boxWidth, boxHeight));

            // then, the box
            builder.Append(
                selectedColor.VTSequenceBackground +
                BoxColor.RenderBox(boxX + 1, boxY, boxWidth, boxHeight) +
                ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
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
                TransformationFormula.Protan, 1.0
            );
            ShowColorInfoBox(
                "Color info (Deuteranomaly)",
                selectedColor,
                true,
                TransformationFormula.Deutan
            );
            ShowColorInfoBox(
                "Color info (Deuteranopia)",
                selectedColor,
                true,
                TransformationFormula.Deutan, 1.0
            );
            ShowColorInfoBox(
                "Color info (Tritanomaly)",
                selectedColor,
                true,
                TransformationFormula.Tritan
            );
            ShowColorInfoBox(
                "Color info (Tritanopia)",
                selectedColor,
                true,
                TransformationFormula.Tritan, 1.0
            );
            ShowColorInfoBox(
                "Color info (Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.Monochromacy
            );
            ShowColorInfoBox(
                "Color info (Inverse)",
                selectedColor,
                true,
                TransformationFormula.Inverse
            );
            ShowColorInfoBox(
                "Color info (Blue Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.BlueScale
            );
            ShowColorInfoBox(
                "Color info (Green Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.GreenScale
            );
            ShowColorInfoBox(
                "Color info (Red Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.RedScale
            );
        }

        private static void ShowColorInfoBox(string localizedTextTitle, Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            selectedColor =
                colorBlind ?
                TransformationTools.RenderColorBlindnessAware(selectedColor, formula, severity) :
                selectedColor;
            var ryb = RybConversionTools.ConvertFrom(selectedColor.RGB);
            var hsl = HslConversionTools.ConvertFrom(selectedColor.RGB);
            var hsv = HsvConversionTools.ConvertFrom(selectedColor.RGB);
            var cmyk = CmykConversionTools.ConvertFrom(selectedColor.RGB);
            var cmy = CmyConversionTools.ConvertFrom(selectedColor.RGB);
            var yiq = YiqConversionTools.ConvertFrom(selectedColor.RGB);
            var yuv = YuvConversionTools.ConvertFrom(selectedColor.RGB);
            InfoBoxColor.WriteInfoBox(localizedTextTitle,
                $$"""
                RGB level:          {{selectedColor.PlainSequence}}
                RGB level (true):   {{selectedColor.PlainSequenceTrueColor}}
                RGB hex code:       {{selectedColor.Hex}}
                Color type:         {{selectedColor.Type}}
                    
                RYB information:
                    - Red:            {{ryb.R,3}}
                    - Yellow:         {{ryb.Y,3}}
                    - Blue:           {{ryb.B,3}}

                CMYK information:
                    - Black key:      {{cmyk.KWhole,3}}
                    - Cyan:           {{cmyk.CMY.CWhole,3}}
                    - Magenta:        {{cmyk.CMY.MWhole,3}}
                    - Yellow:         {{cmyk.CMY.YWhole,3}}
                    
                CMY information:
                    - Cyan:           {{cmy.CWhole,3}}
                    - Magenta:        {{cmy.MWhole,3}}
                    - Yellow:         {{cmy.YWhole,3}}
                    
                HSL information:
                    - Hue (degs):     {{hsl.HueWhole,3}}'
                    - Reverse Hue:    {{hsl.ReverseHueWhole,3}}'
                    - Saturation:     {{hsl.SaturationWhole,3}}
                    - Lightness:      {{hsl.LightnessWhole,3}}
                    
                HSV information:
                    - Hue (degs):     {{hsv.HueWhole,3}}'
                    - Reverse Hue:    {{hsv.ReverseHueWhole,3}}'
                    - Saturation:     {{hsv.SaturationWhole,3}}
                    - Value:          {{hsv.ValueWhole,3}}
                    
                YIQ information:
                    - Luma:           {{yiq.Luma,3}}
                    - In-Phase:       {{yiq.InPhase,3}}
                    - Quadrature:     {{yiq.Quadrature,3}}
                    
                YUV information:
                    - Luma:           {{yuv.Luma,3}}
                    - Chroma (U):     {{yuv.ChromaU,3}}
                    - Chroma (V):     {{yuv.ChromaV,3}}
                """
            );
        }

        private static void ShowColorInfoVisually(Color selectedColor)
        {
            ShowColorUsingBackground(
                "Color info",
                selectedColor
            );
            ShowColorUsingBackground(
                "Color info (Protanomaly)",
                selectedColor,
                true
            );
            ShowColorUsingBackground(
                "Color info (Protanopia)",
                selectedColor,
                true,
                TransformationFormula.Protan, 1.0
            );
            ShowColorUsingBackground(
                "Color info (Deuteranomaly)",
                selectedColor,
                true,
                TransformationFormula.Deutan
            );
            ShowColorUsingBackground(
                "Color info (Deuteranopia)",
                selectedColor,
                true,
                TransformationFormula.Deutan, 1.0
            );
            ShowColorUsingBackground(
                "Color info (Tritanomaly)",
                selectedColor,
                true,
                TransformationFormula.Tritan
            );
            ShowColorUsingBackground(
                "Color info (Tritanopia)",
                selectedColor,
                true,
                TransformationFormula.Tritan, 1.0
            );
            ShowColorUsingBackground(
                "Color info (Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.Monochromacy
            );
            ShowColorUsingBackground(
                "Color info (Inverse)",
                selectedColor,
                true,
                TransformationFormula.Inverse
            );
            ShowColorUsingBackground(
                "Color info (Blue Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.BlueScale
            );
            ShowColorUsingBackground(
                "Color info (Green Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.GreenScale
            );
            ShowColorUsingBackground(
                "Color info (Red Monochromacy)",
                selectedColor,
                true,
                TransformationFormula.RedScale
            );
        }

        private static void ShowColorUsingBackground(string localizedTextTitle, Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            selectedColor =
                colorBlind ?
                TransformationTools.RenderColorBlindnessAware(selectedColor, formula, severity) :
                selectedColor;
            InfoBoxColor.WriteInfoBoxColorBack(localizedTextTitle, ColorTools.GetGray(selectedColor), selectedColor);
        }
    }
}
