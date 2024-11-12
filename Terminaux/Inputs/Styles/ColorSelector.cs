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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Colors.Transformation.Formulas;
using Terminaux.Colors.Transformation.Tools;
using Terminaux.Colors.Transformation.Tools.ColorBlind;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

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
        private static int colorBlindnessSimulationIdx = 0;
        private static double colorBlindnessSeverity = 0.6;
        private static ConsoleColors colorValue255 = ConsoleColors.Fuchsia;
        private static ConsoleColor colorValue16 = ConsoleColor.Magenta;
        private static bool save = true;
        private static readonly Dictionary<TransformationFormula, BaseTransformationFormula> transformationFormulas = new()
        {
            { TransformationFormula.Monochromacy, new Monochromacy() },
            { TransformationFormula.Inverse, new Inverse() },
            { TransformationFormula.Protan, new ColorBlind() { Deficiency = ColorBlindDeficiency.Protan } },
            { TransformationFormula.Deutan, new ColorBlind() { Deficiency = ColorBlindDeficiency.Deutan } },
            { TransformationFormula.Tritan, new ColorBlind() { Deficiency = ColorBlindDeficiency.Tritan } },
            { TransformationFormula.ProtanVienot, new ColorBlind() { Deficiency = ColorBlindDeficiency.Protan, Simple = true } },
            { TransformationFormula.DeutanVienot, new ColorBlind() { Deficiency = ColorBlindDeficiency.Deutan, Simple = true } },
            { TransformationFormula.TritanVienot, new ColorBlind() { Deficiency = ColorBlindDeficiency.Tritan, Simple = true } },
            { TransformationFormula.BlueScale, new Monochromacy() { Type = MonochromacyType.Blue } },
            { TransformationFormula.GreenScale, new Monochromacy() { Type = MonochromacyType.Green } },
            { TransformationFormula.RedScale, new Monochromacy() { Type = MonochromacyType.Red } },
            { TransformationFormula.YellowScale, new Monochromacy() { Type = MonochromacyType.Yellow } },
            { TransformationFormula.AquaScale, new Monochromacy() { Type = MonochromacyType.Cyan } },
            { TransformationFormula.PinkScale, new Monochromacy() { Type = MonochromacyType.Magenta } },
        };

        private readonly static Keybinding[] bindings =
        [
            new("Submit", ConsoleKey.Enter),
            new("Cancel", ConsoleKey.Escape),
            new("Help", ConsoleKey.H),
        ];
        private readonly static Keybinding[] additionalBindingsGeneral =
        [
            new("Color information", ConsoleKey.I),
            new("Select web color", ConsoleKey.W),
            new("Increase opaqueness", ConsoleKey.O),
            new("Increase transparency", ConsoleKey.P),
            new("Change color modes", ConsoleKey.Tab),
            new("Next color blindness simulation", ConsoleKey.N),
            new("Previous color blindness simulation", ConsoleKey.M),
            new("Increase transformation frequency", ConsoleKey.N, ConsoleModifiers.Control),
            new("Decrease transformation frequency", ConsoleKey.M, ConsoleModifiers.Control),
            new("Increase value", PointerButton.WheelDown),
            new("Decrease value", PointerButton.WheelUp),
        ];
        private readonly static Keybinding[] additionalBindingsTrueColor =
        [
            .. additionalBindingsGeneral,
            new("Reduce color hue", ConsoleKey.LeftArrow),
            new("Reduce color lightness", ConsoleKey.LeftArrow, ConsoleModifiers.Control),
            new("Reduce saturation", ConsoleKey.DownArrow),
            new("Increase color hue", ConsoleKey.RightArrow),
            new("Increase color lightness", ConsoleKey.RightArrow, ConsoleModifiers.Control),
            new("Increase saturation", ConsoleKey.UpArrow),
        ];
        private readonly static Keybinding[] additionalBindingsNormalColor =
        [
            .. additionalBindingsGeneral,
            new("Previous color", ConsoleKey.LeftArrow),
            new("Next color", ConsoleKey.RightArrow),
        ];

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

            // Reset some variables
            colorBlindnessSimulationIdx = 0;
            colorBlindnessSeverity = 0.6;

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
                InfoBoxModalColor.WriteInfoBoxModal(
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
            int infoRampBarY = rgbRampBarY + boxHeight + 4;
            var initialBackground = ColorTools.CurrentBackgroundColor;

            // Buffer the hue ramp
            if (ConsoleWrapper.WindowHeight - 2 > hslBarY + 2)
            {
                int finalHue = type == ColorType.TrueColor ? trueColorHue : ConversionTools.ToHsl(selectedColor.RGB).HueWhole;
                int finalSaturation = type == ColorType.TrueColor ? trueColorSaturation : ConversionTools.ToHsl(selectedColor.RGB).SaturationWhole;
                int finalLightness = type == ColorType.TrueColor ? trueColorLightness : ConversionTools.ToHsl(selectedColor.RGB).LightnessWhole;

                // Make a box frame for the HSL indicator
                var hslIndicator = new BoxFrame($"H: {finalHue}/360 | S: {finalSaturation}/100 | L: {finalLightness}/100")
                {
                    Left = hslBarX,
                    Top = hslBarY,
                    InteriorWidth = boxWidth,
                    InteriorHeight = boxHeight * 3,
                };

                // Deal with the hue
                StringBuilder hueRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int hue = (int)(360 * width);
                    hueRamp.Append($"{new Color($"hsl:{hue};100;50", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                var finalHueColor = new Color($"hsl:{finalHue};100;50", finalSettings);
                var hueSlider = new Slider(finalHue, 0, 360)
                {
                    Width = boxWidth,
                    SliderActiveForegroundColor = finalHueColor,
                    SliderForegroundColor = TransformationTools.GetDarkBackground(finalHueColor),
                    SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                    SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                };

                // Deal with the saturation
                StringBuilder satRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int sat = (int)(100 * width);
                    satRamp.Append($"{new Color($"hsl:{finalHue};{sat};50", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                var finalSatColor = new Color($"hsl:{finalHue};{finalSaturation};50", finalSettings);
                var satSlider = new Slider(finalSaturation, 0, 100)
                {
                    Width = boxWidth,
                    SliderActiveForegroundColor = finalSatColor,
                    SliderForegroundColor = TransformationTools.GetDarkBackground(finalSatColor),
                    SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                    SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                };

                // Deal with the lightness
                StringBuilder ligRamp = new();
                for (int i = 0; i < boxWidth; i++)
                {
                    double width = (double)i / boxWidth;
                    int lig = (int)(100 * width);
                    ligRamp.Append($"{new Color($"hsl:{finalHue};100;{lig}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                }
                var finalLigColor = new Color($"hsl:{finalHue};100;{finalLightness}", finalSettings);
                var ligSlider = new Slider(finalLightness, 0, 100)
                {
                    Width = boxWidth,
                    SliderActiveForegroundColor = finalLigColor,
                    SliderForegroundColor = TransformationTools.GetDarkBackground(finalLigColor),
                    SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                    SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                };
                selector.Append(
                    hslIndicator.Render() +
                    ContainerTools.RenderRenderable(hueSlider, new(hslBarX + 1, hslBarY + 2)) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 2) +
                    hueRamp.ToString() +
                    ContainerTools.RenderRenderable(satSlider, new(hslBarX + 1, hslBarY + 4)) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 4) +
                    satRamp.ToString() +
                    ContainerTools.RenderRenderable(ligSlider, new(hslBarX + 1, hslBarY + 6)) +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 6) +
                    ligRamp.ToString()
                );
            }

            // Buffer the gray ramp
            if (ConsoleWrapper.WindowHeight - 2 > grayRampBarY + 2)
            {
                StringBuilder grayRamp = new();
                StringBuilder transparencyRamp = new();
                var mono = new Color(selectedColor.RGB.R, selectedColor.RGB.G, selectedColor.RGB.B, new(finalSettings)
                {
                    Transformations = [new Monochromacy()
                    {
                        Frequency = colorBlindnessSeverity
                    }]
                });
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
                var rampFrame = new BoxFrame($"Gray: {mono.RGB.R}/255 | Transp.: {finalSettings.Opacity}/255")
                {
                    Left = hslBarX,
                    Top = grayRampBarY,
                    InteriorWidth = boxWidth - 7,
                    InteriorHeight = boxHeight,
                };
                selector.Append(
                    rampFrame.Render() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, grayRampBarY + 2) +
                    grayRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, grayRampBarY + 3) +
                    transparencyRamp.ToString()
                );
            }

            // Buffer the dark/light indicator
            if (ConsoleWrapper.WindowHeight - 2 > grayRampBarY + 2)
            {
                StringBuilder darkLightIndicator = new();
                var indicator = selectedColor.Brightness == ColorBrightness.Light ? ConsoleColors.White : ConsoleColors.Black;
                darkLightIndicator.Append($"{new Color(indicator, finalSettings).VTSequenceBackgroundTrueColor}    {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                var darkLightFrame = new BoxFrame("")
                {
                    Left = ConsoleWrapper.WindowWidth - 8,
                    Top = grayRampBarY,
                    InteriorWidth = 4,
                    InteriorHeight = 2,
                };
                selector.Append(
                    darkLightFrame.Render() +
                    CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 7 + 1, grayRampBarY + 2) +
                    darkLightIndicator.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 7 + 1, grayRampBarY + 3) +
                    darkLightIndicator.ToString()
                );
            }

            // Buffer the RGB ramp
            if (ConsoleWrapper.WindowHeight - 2 > rgbRampBarY + 4)
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
                var rgbFrame = new BoxFrame($"R: {selectedColor.RGB.R} | G: {selectedColor.RGB.G} | B: {selectedColor.RGB.B}")
                {
                    Left = hslBarX,
                    Top = rgbRampBarY,
                    InteriorWidth = boxWidth,
                    InteriorHeight = boxHeight + 1,
                };
                selector.Append(
                    rgbFrame.Render() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, rgbRampBarY + 2) +
                    redRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, rgbRampBarY + 3) +
                    greenRamp.ToString() +
                    CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, rgbRampBarY + 4) +
                    blueRamp.ToString()
                );
            }

            // Buffer the color info and the color blindness boxes
            if (ConsoleWrapper.WindowHeight - 2 > infoRampBarY + 5)
            {
                // Render the two boxes
                int halfBoxWidth = (boxWidth / 2) - 2;
                int otherHalfLeft = hslBarX + (boxWidth / 2) + 2;
                var colorInfoFrame = new BoxFrame("Color info")
                {
                    Left = hslBarX,
                    Top = infoRampBarY,
                    InteriorWidth = halfBoxWidth,
                    InteriorHeight = boxHeight + 2,
                };
                var colorBlindnessFrame = new BoxFrame($"Transform [{colorBlindnessSeverity:0.00}]")
                {
                    Left = otherHalfLeft,
                    Top = infoRampBarY,
                    InteriorWidth = (halfBoxWidth * 2) + 5 == boxWidth ? halfBoxWidth + 1 : halfBoxWidth,
                    InteriorHeight = boxHeight + 2,
                };
                selector.Append(
                    colorInfoFrame.Render() +
                    colorBlindnessFrame.Render()
                );

                // Render the color info
                string colorInfoText = ShowColorInfo(selectedColor);
                string[] wrapped = colorInfoText.GetWrappedSentencesByWords(halfBoxWidth);
                for (int i = 0; i < wrapped.Length; i++)
                {
                    int x = hslBarX + 1;
                    int y = infoRampBarY + 1 + i;
                    if (y > infoRampBarY + boxHeight + 2)
                        break;
                    string wrappedMessage = wrapped[i];
                    string spaces = new(' ', halfBoxWidth - wrappedMessage.Length);
                    selector.Append(
                        TextWriterWhereColor.RenderWhere(wrappedMessage + spaces, x, y)
                    );
                }

                // Render the color blindness selections
                var selectionNames = Enum.GetNames(typeof(TransformationFormula)).ToList();
                selectionNames.Insert(0, "None");
                var finalSelections = selectionNames.Select((type, idx) => new InputChoiceInfo($"{idx + 1}", type)).ToArray();
                selector.Append(
                    SelectionInputTools.RenderSelections(finalSelections, otherHalfLeft + 1, infoRampBarY + 1, colorBlindnessSimulationIdx, boxHeight + 2, halfBoxWidth)
                );
            }

            // Finally, the keybindings
            var figletKeybindings = new Keybindings()
            {
                Width = ConsoleWrapper.WindowWidth - 1,
                Top = ConsoleWrapper.WindowHeight - 1,
                KeybindingList = bindings,
            };
            selector.Append(figletKeybindings.Render());
            return selector.ToString();
        }

        private static bool HandleKeypress(ref Color selectedColor, ref ColorType type, out bool refresh, ColorSettings finalSettings, Screen screen)
        {
            bool bail = false;
            refresh = false;

            // Wait for input
            SpinWait.SpinUntil(() => Input.InputAvailable);
            if (Input.MouseInputAvailable)
            {
                // In case user aimed the cursor at the bars
                int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6 + (ConsoleWrapper.WindowWidth % 2 == 0 ? 0 : 1);
                int boxHeight = 2;
                int hslBarX = ConsoleWrapper.WindowWidth / 2 + 2;
                int hslBarY = 1;
                int grayRampBarY = hslBarY + (boxHeight * 3) + 3;
                int rgbRampBarY = grayRampBarY + boxHeight + 3;
                int colorBoxX = 2;
                int colorBoxY = 1;
                int colorBoxWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                int colorBoxHeight = ConsoleWrapper.WindowHeight - 5;

                // Mouse input received.
                var mouse = Input.ReadPointer();
                if (mouse is null)
                    return false;

                // Detect boundaries
                bool withinColorBoxBoundaries = PointerTools.PointerWithinRange(mouse, (colorBoxX + 1, colorBoxY + 1), (colorBoxWidth + colorBoxX, colorBoxHeight + colorBoxY));
                bool withinHueBarBoundaries = PointerTools.PointerWithinRange(mouse, (hslBarX + 1, hslBarY + 1), (hslBarX + boxWidth, hslBarY + 2));
                bool withinSaturationBarBoundaries = PointerTools.PointerWithinRange(mouse, (hslBarX + 1, hslBarY + 3), (hslBarX + boxWidth, hslBarY + 4));
                bool withinLightnessBarBoundaries = PointerTools.PointerWithinRange(mouse, (hslBarX + 1, hslBarY + 5), (hslBarX + boxWidth, hslBarY + 6));
                bool withinTransparencyBarBoundaries = PointerTools.PointerWithinRange(mouse, (hslBarX + 1, grayRampBarY + 1), (hslBarX + boxWidth - 7, grayRampBarY + 2));

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
            else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
            {
                var keypress = Input.ReadKey();
                switch (keypress.Key)
                {
                    // Unified
                    case ConsoleKey.I:
                        ShowColorInfo(selectedColor, screen);
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
                    case ConsoleKey.N:
                        if (keypress.Modifiers == ConsoleModifiers.Control)
                        {
                            colorBlindnessSeverity += 0.01;
                            if (colorBlindnessSeverity > 1.0)
                                colorBlindnessSeverity = 1.0;
                        }
                        else
                        {
                            colorBlindnessSimulationIdx++;
                            if (colorBlindnessSimulationIdx >= Enum.GetNames(typeof(TransformationFormula)).Length + 1)
                                colorBlindnessSimulationIdx--;
                        }
                        break;
                    case ConsoleKey.M:
                        if (keypress.Modifiers == ConsoleModifiers.Control)
                        {
                            colorBlindnessSeverity -= 0.01;
                            if (colorBlindnessSeverity < 0)
                                colorBlindnessSeverity = 0;
                        }
                        else
                        {
                            colorBlindnessSimulationIdx--;
                            if (colorBlindnessSimulationIdx < 0)
                                colorBlindnessSimulationIdx++;
                        }
                        break;
                    case ConsoleKey.W:
                        var colors = WebSafeColors.GetColorList();
                        string[] names = WebSafeColors.GetColorNames();
                        int idx = InfoBoxSelectionColor.WriteInfoBoxSelection(names.Select((n, idx) => new InputChoiceInfo($"{idx + 1}", n)).ToArray(), "Select a web-safe color from the list below.");
                        if (idx < 0)
                            break;
                        type = ColorType.TrueColor;
                        var webSafeColor = colors[idx];
                        var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(webSafeColor.RGB);
                        trueColorHue = hsl.HueWhole;
                        trueColorSaturation = hsl.SaturationWhole;
                        trueColorLightness = hsl.LightnessWhole;
                        refresh = true;
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
                                {
                                    Keybinding[] allBindings = [.. bindings, .. additionalBindingsTrueColor];
                                    KeybindingTools.ShowKeybindingInfobox(allBindings);
                                }
                                refresh = true;
                                break;
                            case ColorType.EightBitColor:
                            case ColorType.FourBitColor:
                                {
                                    Keybinding[] allBindings = [.. bindings, .. additionalBindingsNormalColor];
                                    KeybindingTools.ShowKeybindingInfobox(allBindings);
                                }
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
            int boxHeight = ConsoleWrapper.WindowHeight - 5;
            Color finalColor = selectedColor;
            if (colorBlindnessSimulationIdx > 0)
            {
                var formula = (TransformationFormula)(colorBlindnessSimulationIdx - 1);
                var formulas = new BaseTransformationFormula[]{ transformationFormulas[formula] };
                formulas[0].Frequency = colorBlindnessSeverity;
                var transformed = new Color(selectedColor.RGB.R, selectedColor.RGB.G, selectedColor.RGB.B, new(){ Transformations = formulas });
                finalColor = transformed;
            }

            // First, draw the border
            var previewBorder = new BoxFrame(selectedColor.Name)
            {
                Left = boxX,
                Top = boxY,
                InteriorWidth = boxWidth,
                InteriorHeight = boxHeight,
            };
            builder.Append(previewBorder.Render());

            // then, the box in two halves (normal and transformed)
            var normalBox = new Box()
            {
                Left = boxX + 1,
                Top = boxY,
                InteriorWidth = boxWidth,
                InteriorHeight = boxHeight / 2,
                Color = selectedColor,
            };
            var transformedBox = new Box()
            {
                Left = boxX + 1,
                Top = boxY + (boxHeight / 2),
                InteriorWidth = boxWidth,
                InteriorHeight = (boxHeight / 2) + (ConsoleWrapper.WindowHeight % 2 == 0 ? 1 : 0),
                Color = finalColor,
            };
            builder.Append(
                normalBox.Render() +
                transformedBox.Render()
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
            screen.RequireRefresh();
            switch (colorBlindnessSimulationIdx)
            {
                case 0:
                    ShowColorInfoBox("Color info", selectedColor);
                    break;
                default:
                    var formula = (TransformationFormula)(colorBlindnessSimulationIdx - 1);
                    ShowColorInfoBox($"Color info ({formula})", selectedColor, true, (TransformationFormula)(colorBlindnessSimulationIdx - 1), colorBlindnessSeverity);
                    break;
            }
        }

        private static string ShowColorInfo(Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            if (colorBlind)
            {
                var formulas = new BaseTransformationFormula[] { transformationFormulas[formula] };
                formulas[0].Frequency = severity;
                var transformed = new Color(selectedColor.RGB.R, selectedColor.RGB.G, selectedColor.RGB.B, new() { Transformations = formulas });
                selectedColor = transformed;
            }
            if (selectedColor.RGB is null)
                throw new TerminauxInternalException("Selected color RGB instance for color info is null.");

            // Get all the types except RGB and show their values individually
            var baseType = typeof(BaseColorModel);
            var derivedTypes = baseType.Assembly.GetTypes().Where((type) => type.IsSubclassOf(baseType) && type != typeof(RedGreenBlue)).ToArray();
            StringBuilder builder = new();
            foreach (var colorType in derivedTypes)
            {
                // Get the value
                var converted = ConversionTools.ConvertFromRgb(selectedColor.RGB, colorType);
                builder.AppendLine(converted.ToString());
            }
            return 
                $$"""
                RGB:      {{selectedColor.PlainSequence}}
                True:     {{selectedColor.PlainSequenceTrueColor}}
                Hex:      {{selectedColor.Hex}}
                Type:     {{selectedColor.Type}}
                    
                {{builder}}
                """;
        }

        private static void ShowColorInfoBox(string title, Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            string rendered = ShowColorInfo(selectedColor, colorBlind, formula, severity);
            InfoBoxModalColor.WriteInfoBoxModal(title, rendered);
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

        static ColorSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
