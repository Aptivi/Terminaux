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
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Inputs.Pointer;
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
        private static ConsoleColors colorValue255 = ConsoleColors.Fuchsia;
        private static ConsoleColor colorValue16 = ConsoleColor.Magenta;
        private static bool save = true;

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="settings">Settings to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(ColorSettings? settings = null) =>
            OpenColorSelector(ConsoleColors.White, settings);

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <param name="settings">Settings to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(Color initialColor, ColorSettings? settings = null)
        {
            // Select appropriate settings
            var finalSettings = settings ?? new(ColorTools.GlobalSettings);

            // Initial color is selected
            if (initialColor.RGB is null)
                return initialColor;
            Color selectedColor = new(initialColor.RGB.R, initialColor.RGB.G, initialColor.RGB.B, finalSettings);
            if (selectedColor.RGB is null)
                return selectedColor;
            ColorType type = initialColor.Type;

            // Color selector entry
            var screen = new Screen();
            var screenPart = new ScreenPart();
            ScreenTools.SetCurrent(screen);
            try
            {
                // Now, render the selector
                screenPart.AddDynamicText(() =>
                {
                    ConsoleWrapper.CursorVisible = false;
                    return RenderColorSelector(selectedColor, type, finalSettings);
                });
                screen.AddBufferedPart("Color selector", screenPart);

                // Set initial colors
                var hsl = ConversionTools.ToHsl(selectedColor.RGB);
                switch (type)
                {
                    case ColorType.TrueColor:
                        trueColorHue = hsl.HueWhole;
                        trueColorSaturation = hsl.SaturationWhole;
                        trueColorLightness = hsl.LightnessWhole;
                        break;
                    case ColorType.EightBitColor:
                        colorValue255 = selectedColor.ColorEnum255;
                        break;
                    case ColorType.FourBitColor:
                        colorValue16 = selectedColor.ColorEnum16;
                        break;
                    default:
                        throw new TerminauxException("Invalid color type in the color selector");
                }
                UpdateColor(ref selectedColor, type, finalSettings);

                // Now, the selector main loop
                bool bail = false;
                bool refresh = true;
                while (!bail)
                {
                    ScreenTools.Render();

                    // Handle input
                    bail =
                        type == ColorType.TrueColor || type == ColorType.EightBitColor || type == ColorType.FourBitColor ?
                        HandleKeypress(ref selectedColor, ref type, out refresh, finalSettings, screen) :
                        throw new TerminauxException("Invalid color type in the color selector");
                    if (refresh)
                        screen.RequireRefresh();
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
                screen.RemoveBufferedPart(screenPart.Id);
            }
            ScreenTools.UnsetCurrent(screen);
            return selectedColor;
        }

        private static string RenderColorSelector(Color selectedColor, ColorType type, ColorSettings finalSettings)
        {
            if (selectedColor.RGB is null)
                throw new TerminauxInternalException("Selected color RGB instance is null.");
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the hue, saturation, and lightness bars
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6 + (ConsoleWrapper.WindowWidth % 2 == 0 ? 0 : 1);
            int boxHeight = 2;
            int hslBarX = ConsoleWrapper.WindowWidth / 2 + 2;
            int hslBarY = 1;
            int grayRampBarY = hslBarY + (boxHeight * 3) + 3;
            int rgbRampBarY = grayRampBarY + boxHeight + 3;
            var initialBackground = ColorTools.CurrentBackgroundColor;

            // Buffer the hue ramp
            if (ConsoleWrapper.WindowHeight - 3 > hslBarY + 2)
            {
                int finalHue = type == ColorType.TrueColor ? trueColorHue : ConversionTools.ToHsl(selectedColor.RGB).HueWhole;
                int finalSaturation = type == ColorType.TrueColor ? trueColorSaturation : ConversionTools.ToHsl(selectedColor.RGB).SaturationWhole;
                int finalLightness = type == ColorType.TrueColor ? trueColorLightness : ConversionTools.ToHsl(selectedColor.RGB).LightnessWhole;

                // Make a box frame for the HSL indicator
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"H: {finalHue}/360 | S: {finalSaturation}/100 | L: {finalLightness}/100", hslBarX, hslBarY, boxWidth, boxHeight * 3)
                );

                // Deal with the hue
                StringBuilder hueRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int hue = (int)(360 * width);
                    hueRamp.Append($"{new Color($"hsl:{hue};100;50", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    SliderColor.RenderSlider(finalHue, 360, hslBarX, hslBarY + 1, boxWidth, ColorTools.GetGray(), false) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 2) +
                    hueRamp.ToString()
                );

                // Deal with the saturation
                StringBuilder satRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int sat = (int)(100 * width);
                    satRamp.Append($"{new Color($"hsl:{finalHue};{sat};50", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    SliderColor.RenderSlider(finalSaturation, 100, hslBarX, hslBarY + 3, boxWidth, ColorTools.GetGray(), false) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 4) +
                    satRamp.ToString()
                );

                // Deal with the lightness
                StringBuilder ligRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int lig = (int)(100 * width);
                    ligRamp.Append($"{new Color($"hsl:{finalHue};100;{lig}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    SliderColor.RenderSlider(finalLightness, 100, hslBarX, hslBarY + 5, boxWidth, ColorTools.GetGray(), false) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 6) +
                    ligRamp.ToString()
                );
            }

            // Buffer the gray ramp
            if (ConsoleWrapper.WindowHeight - 3 > grayRampBarY + 2)
            {
                StringBuilder grayRamp = new();
                StringBuilder transparencyRamp = new();
                var mono = TransformationTools.RenderColorBlindnessAware(selectedColor, TransformationFormula.Monochromacy, 0.6);
                if (mono.RGB is null)
                    throw new TerminauxInternalException("Gray ramp RGB instance is null.");
                for (int i = 0; i < boxWidth - 7; i++)
                {
                    double width = (double)i / boxWidth;
                    int gray = (int)(mono.RGB.R * width);
                    grayRamp.Append($"{new Color($"{gray};{gray};{gray}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                    int transparency = (int)(mono.RGB.originalAlpha * width);
                    transparencyRamp.Append($"{new Color($"{transparency};{transparency};{transparency}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"Gray: {mono.RGB.R}/255 | Transp.: {finalSettings.Opacity}/255", hslBarX, grayRampBarY, boxWidth - 7, boxHeight) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, grayRampBarY + 2) +
                    grayRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, grayRampBarY + 3) +
                    transparencyRamp.ToString()
                );
            }

            // Buffer the dark/light indicator
            if (ConsoleWrapper.WindowHeight - 3 > grayRampBarY + 2)
            {
                StringBuilder darkLightIndicator = new();
                var indicator = selectedColor.Brightness == ColorBrightness.Light ? ConsoleColors.White : ConsoleColors.Black;
                darkLightIndicator.Append($"{new Color(indicator, finalSettings).VTSequenceBackgroundTrueColor}    {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                selector.Append(
                    BoxFrameColor.RenderBoxFrame(ConsoleWrapper.WindowWidth - 8, grayRampBarY, 4, 2) +
                    CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 7 + 1, grayRampBarY + 2) +
                    darkLightIndicator.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 7 + 1, grayRampBarY + 3) +
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
                    redRamp.Append($"{new Color($"{red};0;0", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                    greenRamp.Append($"{new Color($"0;{green};0", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                    blueRamp.Append($"{new Color($"0;0;{blue}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                selector.Append(
                    BoxFrameColor.RenderBoxFrame($"R: {selectedColor.RGB.R} | G: {selectedColor.RGB.G} | B: {selectedColor.RGB.B}", hslBarX, rgbRampBarY, boxWidth, boxHeight + 1) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, rgbRampBarY + 2) +
                    redRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, rgbRampBarY + 3) +
                    greenRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, rgbRampBarY + 4) +
                    blueRamp.ToString()
                );
            }

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, "[ENTER] Accept - [H] Help - [ESC] Exit"));
            return selector.ToString();
        }

        private static bool HandleKeypress(ref Color selectedColor, ref ColorType type, out bool refresh, ColorSettings finalSettings, Screen screen)
        {
            bool bail = false;
            refresh = false;

            // Wait for input
            SpinWait.SpinUntil(() => PointerListener.InputAvailable);
            if (PointerListener.PointerAvailable)
            {
                // In case user aimed the cursor at the bars
                int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6 + (ConsoleWrapper.WindowWidth % 2 == 0 ? 0 : 1);
                int boxHeight = 2;
                int hueBarX = ConsoleWrapper.WindowWidth / 2 + 2;
                int hueBarY = 1;
                int saturationBarY = hueBarY + boxHeight + 3;
                int lightnessBarY = saturationBarY + boxHeight + 3;
                int grayRampBarY = lightnessBarY + boxHeight + 3;
                int transparencyRampBarY = grayRampBarY + boxHeight + 2;
                int rgbRampBarY = transparencyRampBarY + boxHeight + 2;
                int colorBoxX = 2;
                int colorBoxY = 1;
                int colorBoxWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                int colorBoxHeight = ConsoleWrapper.WindowHeight - 6;

                // Mouse input received.
                var mouse = TermReader.ReadPointer();
                (int x, int y) = mouse.Coordinates;

                // Detect boundaries
                bool withinColorBoxBoundaries = x >= colorBoxX && x <= colorBoxWidth + colorBoxX && y >= colorBoxY && y <= colorBoxHeight + colorBoxY;
                bool withinHueBarBoundaries = x >= hueBarX && x <= colorBoxWidth + hueBarX && y >= hueBarY && y <= boxHeight + hueBarY + 1;
                bool withinSaturationBarBoundaries = x >= hueBarX && x <= colorBoxWidth + hueBarX && y >= saturationBarY && y <= boxHeight + saturationBarY + 1;
                bool withinLightnessBarBoundaries = x >= hueBarX && x <= colorBoxWidth + hueBarX && y >= lightnessBarY && y <= boxHeight + lightnessBarY + 1;
                bool withinTransparencyBarBoundaries = x >= hueBarX && x <= colorBoxWidth + hueBarX && y >= transparencyRampBarY && y <= boxHeight + transparencyRampBarY + 1;
                
                // Do action
                switch (mouse.Button)
                {
                    case PointerButton.WheelUp:
                        if (withinColorBoxBoundaries)
                            DecrementColor(type, new(), mouse.Modifiers);
                        else if (withinHueBarBoundaries)
                            DecrementHue(type);
                        else if (withinSaturationBarBoundaries)
                            DecrementSaturation(type);
                        else if (withinLightnessBarBoundaries)
                            DecrementLightness(type);
                        else if (withinTransparencyBarBoundaries)
                            finalSettings.Opacity--;
                        break;
                    case PointerButton.WheelDown:
                        if (withinColorBoxBoundaries)
                            IncrementColor(type, new(), mouse.Modifiers);
                        else if (withinHueBarBoundaries)
                            IncrementHue(type);
                        else if (withinSaturationBarBoundaries)
                            IncrementSaturation(type);
                        else if (withinLightnessBarBoundaries)
                            IncrementLightness(type);
                        else if (withinTransparencyBarBoundaries)
                            finalSettings.Opacity++;
                        break;
                }
            }
            else if (ConsoleWrapper.KeyAvailable && !PointerListener.PointerActive)
            {
                var keypress = TermReader.ReadKey();
                switch (keypress.Key)
                {
                    // Unified
                    case ConsoleKey.I:
                        ShowColorInfo(selectedColor, screen);
                        refresh = true;
                        break;
                    case ConsoleKey.V:
                        ShowColorInfoVisually(selectedColor, screen);
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
                                type = ColorType.FourBitColor;
                        }
                        else
                        {
                            type++;
                            if (type > ColorType.FourBitColor)
                                type = ColorType.TrueColor;
                        }
                        break;
                    case ConsoleKey.O:
                        finalSettings.Opacity++;
                        break;
                    case ConsoleKey.P:
                        finalSettings.Opacity--;
                        break;

                    // Non-unified
                    case ConsoleKey.LeftArrow:
                        DecrementColor(type, keypress, PointerModifiers.None);
                        break;
                    case ConsoleKey.RightArrow:
                        IncrementColor(type, keypress, PointerModifiers.None);
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
                            case ColorType.EightBitColor:
                            case ColorType.FourBitColor:
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
                        IncrementSaturation(type);
                        break;
                    case ConsoleKey.DownArrow:
                        DecrementSaturation(type);
                        break;
                }
            }
            UpdateColor(ref selectedColor, type, finalSettings);
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
            builder.Append(BoxFrameColor.RenderBoxFrame($"{selectedColor.PlainSequence} [{selectedColor.PlainSequenceTrueColor} | {selectedColor.Name}]", boxX, boxY, boxWidth, boxHeight));

            // then, the box
            builder.Append(
                selectedColor.VTSequenceBackground +
                BoxColor.RenderBox(boxX + 1, boxY, boxWidth, boxHeight) +
                ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
            );
            return builder.ToString();
        }

        private static void UpdateColor(ref Color selectedColor, ColorType newType, ColorSettings finalSettings)
        {
            switch (newType)
            {
                case ColorType.TrueColor:
                    selectedColor = new($"hsl:{trueColorHue};{trueColorSaturation};{trueColorLightness}", finalSettings);
                    break;
                case ColorType.EightBitColor:
                    selectedColor = new(colorValue255, finalSettings);
                    break;
                case ColorType.FourBitColor:
                    selectedColor = new(colorValue16, finalSettings);
                    break;
            }
        }

        private static void ShowColorInfo(Color selectedColor, Screen screen)
        {
            bool bail = false;
            while (!bail)
            {
                var selections = GetColorInfoChoices();
                screen.RequireRefresh();
                int idx = InfoBoxSelectionColor.WriteInfoBoxSelection(selections, "Select color information variant");
                screen.RequireRefresh();
                switch (idx)
                {
                    case 0:
                        ShowColorInfoBox("Color info", selectedColor);
                        break;
                    case 1:
                        ShowColorInfoBox("Color info (Protanomaly)", selectedColor, true);
                        break;
                    case 2:
                        ShowColorInfoBox("Color info (Protanopia)", selectedColor, true, TransformationFormula.Protan, 1.0);
                        break;
                    case 3:
                        ShowColorInfoBox("Color info (Deuteranomaly)", selectedColor, true, TransformationFormula.Deutan);
                        break;
                    case 4:
                        ShowColorInfoBox("Color info (Deuteranopia)", selectedColor, true, TransformationFormula.Deutan, 1.0);
                        break;
                    case 5:
                        ShowColorInfoBox("Color info (Tritanomaly)", selectedColor, true, TransformationFormula.Tritan);
                        break;
                    case 6:
                        ShowColorInfoBox("Color info (Tritanopia)", selectedColor, true, TransformationFormula.Tritan, 1.0);
                        break;
                    case 7:
                        ShowColorInfoBox("Color info (Monochromacy)", selectedColor, true, TransformationFormula.Monochromacy);
                        break;
                    case 8:
                        ShowColorInfoBox("Color info (Inverse)", selectedColor, true, TransformationFormula.Inverse);
                        break;
                    case 9:
                        ShowColorInfoBox("Color info (Blue Monochromacy)", selectedColor, true, TransformationFormula.BlueScale);
                        break;
                    case 10:
                        ShowColorInfoBox("Color info (Green Monochromacy)", selectedColor, true, TransformationFormula.GreenScale);
                        break;
                    case 11:
                        ShowColorInfoBox("Color info (Red Monochromacy)", selectedColor, true, TransformationFormula.RedScale);
                        break;
                    case -1:
                        bail = true;
                        break;
                }
            }
        }

        private static void ShowColorInfoBox(string localizedTextTitle, Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            selectedColor =
                colorBlind ?
                TransformationTools.RenderColorBlindnessAware(selectedColor, formula, severity) :
                selectedColor;
            if (selectedColor.RGB is null)
                throw new TerminauxInternalException("Selected color RGB instance for color infobox is null.");
            var ryb = ConversionTools.ToRyb(selectedColor.RGB);
            var hsl = ConversionTools.ToHsl(selectedColor.RGB);
            var hsv = ConversionTools.ToHsv(selectedColor.RGB);
            var cmyk = ConversionTools.ToCmyk(selectedColor.RGB);
            var cmy = ConversionTools.ToCmy(selectedColor.RGB);
            var yiq = ConversionTools.ToYiq(selectedColor.RGB);
            var yuv = ConversionTools.ToYuv(selectedColor.RGB);
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

        private static void ShowColorInfoVisually(Color selectedColor, Screen screen)
        {
            bool bail = false;
            while (!bail)
            {
                var selections = GetColorInfoChoices();
                screen.RequireRefresh();
                int idx = InfoBoxSelectionColor.WriteInfoBoxSelection(selections, "Select color information variant");
                screen.RequireRefresh();
                switch (idx)
                {
                    case 0:
                        ShowColorUsingBackground("Color info", selectedColor);
                        break;
                    case 1:
                        ShowColorUsingBackground("Color info (Protanomaly)", selectedColor, true);
                        break;
                    case 2:
                        ShowColorUsingBackground("Color info (Protanopia)", selectedColor, true, TransformationFormula.Protan, 1.0);
                        break;
                    case 3:
                        ShowColorUsingBackground("Color info (Deuteranomaly)", selectedColor, true, TransformationFormula.Deutan);
                        break;
                    case 4:
                        ShowColorUsingBackground("Color info (Deuteranopia)", selectedColor, true, TransformationFormula.Deutan, 1.0);
                        break;
                    case 5:
                        ShowColorUsingBackground("Color info (Tritanomaly)", selectedColor, true, TransformationFormula.Tritan);
                        break;
                    case 6:
                        ShowColorUsingBackground("Color info (Tritanopia)", selectedColor, true, TransformationFormula.Tritan, 1.0);
                        break;
                    case 7:
                        ShowColorUsingBackground("Color info (Monochromacy)", selectedColor, true, TransformationFormula.Monochromacy);
                        break;
                    case 8:
                        ShowColorUsingBackground("Color info (Inverse)", selectedColor, true, TransformationFormula.Inverse);
                        break;
                    case 9:
                        ShowColorUsingBackground("Color info (Blue Monochromacy)", selectedColor, true, TransformationFormula.BlueScale);
                        break;
                    case 10:
                        ShowColorUsingBackground("Color info (Green Monochromacy)", selectedColor, true, TransformationFormula.GreenScale);
                        break;
                    case 11:
                        ShowColorUsingBackground("Color info (Red Monochromacy)", selectedColor, true, TransformationFormula.RedScale);
                        break;
                    case -1:
                        bail = true;
                        break;
                }
            }
        }

        private static void ShowColorUsingBackground(string localizedTextTitle, Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            selectedColor =
                colorBlind ?
                TransformationTools.RenderColorBlindnessAware(selectedColor, formula, severity) :
                selectedColor;
            InfoBoxColor.WriteInfoBoxColorBack(localizedTextTitle, ColorTools.GetGray(selectedColor), selectedColor);
        }

        private static void DecrementColor(ColorType type, ConsoleKeyInfo keypress, PointerModifiers mods)
        {
            switch (type)
            {
                case ColorType.TrueColor:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Control) || (mods & PointerModifiers.Ctrl) != 0)
                        DecrementLightness(type);
                    else
                        DecrementHue(type);
                    break;
                case ColorType.EightBitColor:
                    colorValue255--;
                    if (colorValue255 < ConsoleColors.Black)
                        colorValue255 = ConsoleColors.Grey93;
                    break;
                case ColorType.FourBitColor:
                    colorValue16--;
                    if (colorValue16 < ConsoleColor.Black)
                        colorValue16 = ConsoleColor.White;
                    break;
            }
        }

        private static void IncrementColor(ColorType type, ConsoleKeyInfo keypress, PointerModifiers mods)
        {
            switch (type)
            {
                case ColorType.TrueColor:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Control) || (mods & PointerModifiers.Ctrl) != 0)
                        IncrementLightness(type);
                    else
                        IncrementHue(type);
                    break;
                case ColorType.EightBitColor:
                    colorValue255++;
                    if (colorValue255 > ConsoleColors.Grey93)
                        colorValue255 = ConsoleColors.Black;
                    break;
                case ColorType.FourBitColor:
                    colorValue16++;
                    if (colorValue16 > ConsoleColor.White)
                        colorValue16 = ConsoleColor.Black;
                    break;
            }
        }

        private static void DecrementHue(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorHue--;
            if (trueColorHue < 0)
                trueColorHue = 360;
        }

        private static void IncrementHue(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorHue++;
            if (trueColorHue > 360)
                trueColorHue = 0;
        }

        private static void DecrementLightness(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorLightness--;
            if (trueColorLightness < 0)
                trueColorLightness = 100;
        }

        private static void IncrementLightness(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorLightness++;
            if (trueColorLightness > 100)
                trueColorLightness = 0;
        }

        private static void DecrementSaturation(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorSaturation--;
            if (trueColorSaturation < 0)
                trueColorSaturation = 100;
        }

        private static void IncrementSaturation(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorSaturation++;
            if (trueColorSaturation > 100)
                trueColorSaturation = 0;
        }

        private static InputChoiceInfo[] GetColorInfoChoices() =>
            InputChoiceTools.GetInputChoices([
                ("Normal", "Color information (normal)"),
                ("Protanomaly", "Color information (Protanomaly)"),
                ("Protanopia", "Color information (Protanopia)"),
                ("Deuteranomaly", "Color information (Deuteranomaly)"),
                ("Deuteranopia", "Color information (Deuteranopia)"),
                ("Tritanomaly", "Color information (Tritanomaly)"),
                ("Tritanopia", "Color information (Tritanopia)"),
                ("Monochromacy", "Color information (Monochromacy)"),
                ("Inverse", "Color information (Inverse)"),
                ("Blue Monochromacy", "Color information (Blue Monochromacy)"),
                ("Green Monochromacy", "Color information (Green Monochromacy)"),
                ("Red Monochromacy", "Color information (Red Monochromacy)"),
            ]);

        static ColorSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
