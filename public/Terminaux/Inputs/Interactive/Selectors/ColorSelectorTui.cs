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
using Terminaux.Base.Structures;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Gradients;
using Colorimetry.Models;
using Colorimetry.Models.Conversion;
using Colorimetry.Transformation;
using Colorimetry.Transformation.Contrast;
using Colorimetry.Transformation.Formulas;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;

namespace Terminaux.Inputs.Interactive.Selectors
{
    /// <summary>
    /// Color selection application
    /// </summary>
    public class ColorSelectorTui : TextualUI
    {
        private int trueColorHue = 0;
        private int trueColorSaturation = 100;
        private int trueColorLightness = 50;
        private int colorBlindnessSimulationIdx = 0;
        private double colorBlindnessSeverity = 0.6;
        private ConsoleColors colorValue255 = ConsoleColors.Fuchsia;
        private ConsoleColor colorValue16 = ConsoleColor.Magenta;
        private bool cancel = false;
        private bool showColorList = false;
        private bool readOnly = false;
        private Color selectedColor = ConsoleColors.White;
        private ColorType type = ColorType.FourBitColor;
        private readonly ColorSettings finalSettings = new(ColorTools.GlobalSettings);
        private readonly Color initialColor = ConsoleColors.White;

        /// <inheritdoc/>
        public override string Render()
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, decide whether to render the bars or to render the color list
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6 + (ConsoleWrapper.WindowWidth % 2 == 0 ? 0 : 1);
            int boxHeight = ConsoleWrapper.WindowHeight - 5;
            int generalX = ConsoleWrapper.WindowWidth / 2 + 2;
            if (type != ColorType.TrueColor && showColorList)
            {
                var selectionNames = Enum.GetNames(type == ColorType.FourBitColor ? typeof(ConsoleColor) : typeof(ConsoleColors)).ToList();
                var finalSelections = selectionNames.Select((type, idx) => new InputChoiceInfo($"{idx + 1}", type)).ToArray();
                var selectionBorder = new BoxFrame(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_LISTOFPREDEFINEDCOLORS").FormatString(finalSelections.Length))
                {
                    Left = generalX,
                    Top = 1,
                    Width = boxWidth,
                    Height = boxHeight,
                };
                var selections = new Selection(finalSelections)
                {
                    Left = generalX + 1,
                    Top = 2,
                    CurrentSelection = type == ColorType.FourBitColor ? (int)colorValue16 : (int)colorValue255,
                    Height = boxHeight,
                    Width = boxWidth,
                };
                selector.Append(
                    selectionBorder.Render() +
                    selections.Render()
                );
            }
            else
            {
                // Render the hue, saturation, and lightness bars
                boxHeight = 2;
                int hslBarX = generalX;
                int hslBarY = 1;
                int grayRampBarY = hslBarY + boxHeight * 3 + 3;
                int rgbRampBarY = grayRampBarY + boxHeight + 3;
                int shadeTintRampBarY = rgbRampBarY + boxHeight + 4;
                int infoRampBarY = shadeTintRampBarY + boxHeight + 3;
                var initialBackground = ThemeColorsTools.GetColor(ThemeColorType.Background);

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
                        Width = boxWidth,
                        Height = boxHeight * 3,
                    };

                    // Deal with the hue
                    StringBuilder hueRamp = new();
                    for (int i = 0; i < boxWidth; i++)
                    {
                        double width = (double)i / boxWidth;
                        int hue = (int)(360 * width);
                        hueRamp.Append($"{new Color($"hsl:{hue};100;50", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
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
                        satRamp.Append($"{new Color($"hsl:{finalHue};{sat};50", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
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
                        ligRamp.Append($"{new Color($"hsl:{finalHue};100;{lig}", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
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
                        RendererTools.RenderRenderable(hueSlider, new(hslBarX + 1, hslBarY + 2)) +
                        CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 2) +
                        hueRamp.ToString() +
                        RendererTools.RenderRenderable(satSlider, new(hslBarX + 1, hslBarY + 4)) +
                        CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, hslBarY + 4) +
                        satRamp.ToString() +
                        RendererTools.RenderRenderable(ligSlider, new(hslBarX + 1, hslBarY + 6)) +
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
                    for (int i = 0; i < boxWidth - 7; i++)
                    {
                        double width = (double)i / boxWidth;
                        int gray = (int)(mono.RGB.R * width);
                        grayRamp.Append($"{new Color($"{gray};{gray};{gray}", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                        int transparency = (int)(mono.RGB.originalAlpha * width);
                        transparencyRamp.Append($"{new Color($"{transparency};{transparency};{transparency}", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                    }
                    var rampFrame = new BoxFrame(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_RAMPFRAME_GRAY") + $": {mono.RGB.R}/255 | " + LanguageTools.GetLocalized("T_INPUT_IS_COLOR_RAMPFRAME_TRANSPARENCY") + $": {finalSettings.Opacity}/255")
                    {
                        Left = hslBarX,
                        Top = grayRampBarY,
                        Width = boxWidth - 7,
                        Height = boxHeight,
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
                    darkLightIndicator.Append($"{new Color(indicator, finalSettings).VTSequenceBackgroundTrueColor()}    {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                    var darkLightFrame = new BoxFrame("")
                    {
                        Left = ConsoleWrapper.WindowWidth - 8,
                        Top = grayRampBarY,
                        Width = 4,
                        Height = 2,
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
                        redRamp.Append($"{new Color($"{red};0;0", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                        greenRamp.Append($"{new Color($"0;{green};0", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                        blueRamp.Append($"{new Color($"0;0;{blue}", finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                    }
                    var rgbFrame = new BoxFrame($"R: {selectedColor.RGB.R} | G: {selectedColor.RGB.G} | B: {selectedColor.RGB.B}")
                    {
                        Left = hslBarX,
                        Top = rgbRampBarY,
                        Width = boxWidth,
                        Height = boxHeight + 1,
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

                // Buffer the shade and tint ramp
                if (ConsoleWrapper.WindowHeight - 2 > shadeTintRampBarY + 3)
                {
                    StringBuilder shadeRamp = new();
                    StringBuilder tintRamp = new();
                    var shades = ColorGradients.GetShades(selectedColor, boxWidth);
                    var tints = ColorGradients.GetTints(selectedColor, boxWidth);
                    for (int i = 0; i < boxWidth; i++)
                    {
                        shadeRamp.Append($"{new Color(shades[i].IntermediateColor.ToString(), finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                        tintRamp.Append($"{new Color(tints[i].IntermediateColor.ToString(), finalSettings).VTSequenceBackgroundTrueColor()} {ConsoleColoring.RenderSetConsoleColor(initialBackground, true)}");
                    }
                    var shadeTintFrame = new BoxFrame(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_SHADETINTFRAME"))
                    {
                        Left = hslBarX,
                        Top = shadeTintRampBarY,
                        Width = boxWidth,
                        Height = boxHeight,
                    };
                    selector.Append(
                        shadeTintFrame.Render() +
                        CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, shadeTintRampBarY + 2) +
                        shadeRamp.ToString() +
                        CsiSequences.GenerateCsiCursorPosition(hslBarX + 2, shadeTintRampBarY + 3) +
                        tintRamp.ToString()
                    );
                }

                // Buffer the color info and the color blindness boxes
                if (ConsoleWrapper.WindowHeight - 2 > infoRampBarY + 5)
                {
                    // Render the two boxes
                    int halfBoxWidth = boxWidth / 2 - 2;
                    int otherHalfLeft = hslBarX + boxWidth / 2 + 2;
                    var colorInfoFrame = new BoxFrame(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_COLORINFOFRAME"))
                    {
                        Left = hslBarX,
                        Top = infoRampBarY,
                        Width = halfBoxWidth,
                        Height = boxHeight + 2,
                    };
                    var colorBlindnessFrame = new BoxFrame(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_TRANSFORMFRAME") + $" [[{colorBlindnessSeverity:0.00}]]")
                    {
                        Left = otherHalfLeft,
                        Top = infoRampBarY,
                        Width = halfBoxWidth * 2 + 5 == boxWidth ? halfBoxWidth + 1 : halfBoxWidth,
                        Height = boxHeight + 2,
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
                            TextWriterWhereColor.RenderWherePlain(wrappedMessage + spaces, x, y)
                        );
                    }

                    // Render the color blindness selections
                    var selectionNames = Enum.GetNames(typeof(TransformationFormula)).ToList();
                    selectionNames.Insert(0, "None");
                    var finalSelections = selectionNames.Select((type, idx) => new InputChoiceInfo($"{idx + 1}", type)).ToArray();
                    var selections = new Selection(finalSelections)
                    {
                        Left = otherHalfLeft + 1,
                        Top = infoRampBarY + 1,
                        CurrentSelection = colorBlindnessSimulationIdx,
                        Height = boxHeight + 2,
                        Width = halfBoxWidth,
                    };
                    selector.Append(
                        selections.Render()
                    );
                }
            }

            // Finally, the keybindings
            var figletKeybindings = new Keybindings()
            {
                Width = ConsoleWrapper.WindowWidth - 1,
                KeybindingList = ColorSelector.Bindings,
                WriteHelpKeyInfo = false,
            };
            selector.Append(RendererTools.RenderRenderable(figletKeybindings, new(0, ConsoleWrapper.WindowHeight - 1)));
            return selector.ToString();
        }

        private string RenderPreviewBox(Color selectedColor)
        {
            var builder = new StringBuilder();

            // Draw the box that represents the currently selected color
            int boxX = 2;
            int boxY = 1;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 4;
            int boxHeight = ConsoleWrapper.WindowHeight - 5;
            var settings = new ColorSettings()
            {
                UseTerminalPalette = finalSettings.UseTerminalPalette && type != ColorType.TrueColor
            };
            Color originalColor = new(selectedColor.RGB.R, selectedColor.RGB.G, selectedColor.RGB.B, settings);
            Color finalColor = originalColor;
            if (colorBlindnessSimulationIdx > 0)
            {
                var formula = (TransformationFormula)(colorBlindnessSimulationIdx - 1);
                var formulas = new BaseTransformationFormula[] { ColorSelector.transformationFormulas[formula] };
                formulas[0].Frequency = colorBlindnessSeverity;
                var transformedSettings = new ColorSettings()
                {
                    Transformations = formulas,
                    UseTerminalPalette = finalSettings.UseTerminalPalette && type != ColorType.TrueColor
                };
                var transformed = new Color(selectedColor.RGB.R, selectedColor.RGB.G, selectedColor.RGB.B, transformedSettings);
                finalColor = transformed;
            }

            // First, draw the border
            var previewBorder = new BoxFrame(selectedColor.Name)
            {
                Left = boxX,
                Top = boxY,
                Width = boxWidth,
                Height = boxHeight,
            };
            builder.Append(previewBorder.Render());

            // then, the box in two halves (normal and transformed)
            var normalBox = new Box()
            {
                Left = boxX + 1,
                Top = boxY + 1,
                Width = boxWidth,
                Height = boxHeight / 2,
                Color = originalColor,
            };
            var transformedBox = new Box()
            {
                Left = boxX + 1,
                Top = boxY + 1 + boxHeight / 2,
                Width = boxWidth,
                Height = boxHeight / 2 + (ConsoleWrapper.WindowHeight % 2 == 0 ? 1 : 0),
                Color = finalColor,
            };
            builder.Append(
                normalBox.Render() +
                transformedBox.Render()
            );
            return builder.ToString();
        }

        private void UpdateColor(ref Color selectedColor, ColorType newType, ColorSettings finalSettings)
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

        private void UpdateKeybindings()
        {
            ResetKeybindings();

            // General keybindings
            Keybindings.Add((ColorSelector.Bindings[0], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((ColorSelector.Bindings[1], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((ColorSelector.AdditionalBindingsGeneral[0], ShowColorInfo));

            // Simulation of color-blindness and transformations
            Keybindings.Add((ColorSelector.AdditionalBindingsGeneral[1], (_, _, _) => ChangeSimulation(false)));
            Keybindings.Add((ColorSelector.AdditionalBindingsGeneral[2], (_, _, _) => ChangeSimulation(true)));
            Keybindings.Add((ColorSelector.AdditionalBindingsGeneral[3], (_, _, _) => ChangeSimulationSeverity(false)));
            Keybindings.Add((ColorSelector.AdditionalBindingsGeneral[4], (_, _, _) => ChangeSimulationSeverity(true)));
            Keybindings.Add((ColorSelector.AdditionalBindingsGeneral[5], (_, _, mouse) => ChangeValue(mouse, false)));

            // These require write access
            if (!readOnly)
            {
                Keybindings.Add((ColorSelector.AdditionalBindingsReadWrite[6], SelectWebColor));
                Keybindings.Add((ColorSelector.AdditionalBindingsReadWrite[7], (_, _, _) =>
                {
                    finalSettings.Opacity++;
                    UpdateColor(ref selectedColor, type, finalSettings);
                }));
                Keybindings.Add((ColorSelector.AdditionalBindingsReadWrite[8], (_, _, _) =>
                {
                    finalSettings.Opacity--;
                    UpdateColor(ref selectedColor, type, finalSettings);
                }));
                Keybindings.Add((ColorSelector.AdditionalBindingsReadWrite[9], (_, _, _) => ChangeMode(false)));
                Keybindings.Add((ColorSelector.AdditionalBindingsReadWrite[10], (_, _, _) => ChangeMode(true)));

                // Mouse bindings
                Keybindings.Add((ColorSelector.AdditionalBindingsReadWrite[11], (_, _, mouse) => ChangeValue(mouse, false)));
                Keybindings.Add((ColorSelector.AdditionalBindingsReadWrite[12], (_, _, mouse) => ChangeValue(mouse, true)));

                // Type-specific bindings
                switch (type)
                {
                    case ColorType.TrueColor:
                        Keybindings.Add((ColorSelector.AdditionalBindingsTrueColor[13], (_, _, _) => ChangeHue(true)));
                        Keybindings.Add((ColorSelector.AdditionalBindingsTrueColor[14], (_, _, _) => ChangeLightness(true)));
                        Keybindings.Add((ColorSelector.AdditionalBindingsTrueColor[15], (_, _, _) => ChangeSaturation(true)));
                        Keybindings.Add((ColorSelector.AdditionalBindingsTrueColor[16], (_, _, _) => ChangeHue(false)));
                        Keybindings.Add((ColorSelector.AdditionalBindingsTrueColor[17], (_, _, _) => ChangeLightness(false)));
                        Keybindings.Add((ColorSelector.AdditionalBindingsTrueColor[18], (_, _, _) => ChangeSaturation(false)));
                        break;
                    case ColorType.EightBitColor:
                    case ColorType.FourBitColor:
                        Keybindings.Add((ColorSelector.AdditionalBindingsNormalColor[13], (_, _, _) => ChangeColor(true)));
                        Keybindings.Add((ColorSelector.AdditionalBindingsNormalColor[14], (_, _, _) => ChangeColor(false)));
                        Keybindings.Add((ColorSelector.AdditionalBindingsNormalColor[15], (_, _, _) => ShowColorList()));
                        break;
                }
            }
        }

        internal Color GetResultingColor() =>
            !cancel ? selectedColor : initialColor;

        private void Exit(TextualUI ui, bool cancel)
        {
            this.cancel = cancel;
            TextualUITools.ExitTui(ui);
        }

        private void ShowColorInfo(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            switch (colorBlindnessSimulationIdx)
            {
                case 0:
                    ShowColorInfoBox(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_COLORINFOFRAME"), selectedColor);
                    break;
                default:
                    var formula = (TransformationFormula)(colorBlindnessSimulationIdx - 1);
                    ShowColorInfoBox(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_COLORINFOFRAME") + $" ({formula})", selectedColor, true, (TransformationFormula)(colorBlindnessSimulationIdx - 1), colorBlindnessSeverity);
                    break;
            }
            ui.RequireRefresh();
        }

        private string ShowColorInfo(Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            if (colorBlind)
            {
                var formulas = new BaseTransformationFormula[] { ColorSelector.transformationFormulas[formula] };
                formulas[0].Frequency = severity;
                var transformed = new Color(selectedColor.RGB.R, selectedColor.RGB.G, selectedColor.RGB.B, new() { Transformations = formulas });
                selectedColor = transformed;
            }

            // Get all the types except RGB and show their values individually
            var baseType = typeof(BaseColorModel);
            var derivedTypes = baseType.Assembly.GetTypes().Where((type) => type.IsSubclassOf(baseType) && type != typeof(RedGreenBlue)).ToArray();
            int maxBindingLength = derivedTypes.Max((colorType) => ConsoleChar.EstimateCellWidth(colorType.Name));
            StringBuilder builder = new();
            foreach (var colorType in derivedTypes)
            {
                // Get the value
                var converted = ConversionTools.ConvertFromRgb(selectedColor.RGB, colorType);
                builder.AppendLine(colorType.Name + new string(' ', maxBindingLength - ConsoleChar.EstimateCellWidth(colorType.Name)) + $" | {converted}");
            }
            return
                $$"""
                RGB:      {{selectedColor.PlainSequence}}
                True:     {{selectedColor.PlainSequenceTrueColor}}
                Hex:      {{selectedColor.Hex}}
                Type:     {{selectedColor.Type}}
                
                ---

                {{builder}}
                """;
        }

        private void ShowColorInfoBox(string title, Color selectedColor, bool colorBlind = false, TransformationFormula formula = TransformationFormula.Protan, double severity = 0.6)
        {
            string rendered = ShowColorInfo(selectedColor, colorBlind, formula, severity);
            InfoBoxModalColor.WriteInfoBoxModal(rendered, new InfoBoxSettings()
            {
                Title = title,
            });
        }

        private void SelectWebColor(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            var colors = WebSafeColors.GetColorList();
            string[] names = WebSafeColors.GetColorNames();
            int idx = InfoBoxSelectionColor.WriteInfoBoxSelection(names.Select((n, idx) => new InputChoiceInfo($"{idx + 1}", n)).ToArray(), "Select a web-safe color from the list below.");
            if (idx < 0)
                return;
            type = ColorType.TrueColor;
            var webSafeColor = colors[idx];
            var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(webSafeColor.RGB);
            trueColorHue = hsl.HueWhole;
            trueColorSaturation = hsl.SaturationWhole;
            trueColorLightness = hsl.LightnessWhole;
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void ChangeMode(bool goBack)
        {
            if (goBack)
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
            UpdateKeybindings();
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void ChangeSimulation(bool goBack)
        {
            if (goBack)
            {
                colorBlindnessSimulationIdx--;
                if (colorBlindnessSimulationIdx < 0)
                    colorBlindnessSimulationIdx++;
            }
            else
            {
                colorBlindnessSimulationIdx++;
                if (colorBlindnessSimulationIdx >= Enum.GetNames(typeof(TransformationFormula)).Length + 1)
                    colorBlindnessSimulationIdx--;
            }
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void ChangeSimulationSeverity(bool goBack)
        {
            if (goBack)
            {
                colorBlindnessSeverity -= 0.01;
                if (colorBlindnessSeverity < 0)
                    colorBlindnessSeverity = 0;
            }
            else
            {
                colorBlindnessSeverity += 0.01;
                if (colorBlindnessSeverity > 1.0)
                    colorBlindnessSeverity = 1.0;
            }
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void ChangeValue(PointerEventContext? mouse, bool goBack)
        {
            if (mouse is null)
                return;

            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6 + (ConsoleWrapper.WindowWidth % 2 == 0 ? 0 : 1);
            int boxHeight = ConsoleWrapper.WindowHeight - 5;
            int generalX = ConsoleWrapper.WindowWidth / 2 + 2;
            int colorBoxX = 2, colorBoxY = 1;
            int colorBoxWidth = ConsoleWrapper.WindowWidth / 2 - 4;
            int grayRampBarY = colorBoxY + 9;
            int halfBoxWidth = boxWidth / 2 - 2;
            int otherHalfLeft = generalX + 1 + boxWidth / 2 + 2;
            int infoRampBarY = colorBoxY + 25;

            // Make pointer hitboxes to detect boundaries
            var colorBoxHitbox = new PointerHitbox(new(colorBoxX + 1, colorBoxY + 1), new Coordinate(colorBoxWidth + colorBoxX, boxHeight + colorBoxY), (pec) => ChangeColor(goBack)) { Button = PointerButton.WheelUp | PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
            var colorListHitbox = new PointerHitbox(new(generalX + 1, colorBoxY + 1), new Coordinate(colorBoxWidth + generalX, boxHeight + colorBoxY), (pec) => ChangeColor(goBack)) { Button = PointerButton.WheelUp | PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
            var colorHueBarHitbox = new PointerHitbox(new(generalX + 1, colorBoxY + 1), new Coordinate(generalX + boxWidth, colorBoxY + 2), (pec) => ChangeHue(goBack)) { Button = PointerButton.WheelUp | PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
            var colorSaturationBarHitbox = new PointerHitbox(new(generalX + 1, colorBoxY + 3), new Coordinate(generalX + boxWidth, colorBoxY + 4), (pec) => ChangeSaturation(goBack)) { Button = PointerButton.WheelUp | PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
            var colorLightnessBarHitbox = new PointerHitbox(new(generalX + 1, colorBoxY + 5), new Coordinate(generalX + boxWidth, colorBoxY + 6), (pec) => ChangeLightness(goBack)) { Button = PointerButton.WheelUp | PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
            var colorTransparencyBarHitbox = new PointerHitbox(new(generalX + 1, grayRampBarY + 1), new Coordinate(generalX + boxWidth - 6, grayRampBarY + 2), (pec) => ChangeTransparency(goBack)) { Button = PointerButton.WheelUp | PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
            var colorBlindnessSelectionHitbox = new PointerHitbox(new(otherHalfLeft, infoRampBarY + 1), new Coordinate(otherHalfLeft + halfBoxWidth - 1, infoRampBarY + 4), (pec) => ChangeSimulation(goBack)) { Button = PointerButton.WheelUp | PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
            var colorBlindnessSelectionArrowUpHitbox = new PointerHitbox(new(otherHalfLeft + halfBoxWidth, infoRampBarY + 1), new Coordinate(otherHalfLeft + halfBoxWidth, infoRampBarY + 1), (pec) => ChangeSimulation(true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var colorBlindnessSelectionArrowDownHitbox = new PointerHitbox(new(otherHalfLeft + halfBoxWidth, infoRampBarY + 4), new Coordinate(otherHalfLeft + halfBoxWidth, infoRampBarY + 4), (pec) => ChangeSimulation(false)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

            // Detect the boundaries and do the action!
            if (colorBoxHitbox.IsPointerWithin(mouse))
                colorBoxHitbox.ProcessPointer(mouse, out _);
            else
            {
                if (type != ColorType.TrueColor && showColorList)
                    colorListHitbox.ProcessPointer(mouse, out _);
                else if (colorHueBarHitbox.IsPointerWithin(mouse))
                    colorHueBarHitbox.ProcessPointer(mouse, out _);
                else if (colorSaturationBarHitbox.IsPointerWithin(mouse))
                    colorSaturationBarHitbox.ProcessPointer(mouse, out _);
                else if (colorLightnessBarHitbox.IsPointerWithin(mouse))
                    colorLightnessBarHitbox.ProcessPointer(mouse, out _);
                else if (colorTransparencyBarHitbox.IsPointerWithin(mouse))
                    colorTransparencyBarHitbox.ProcessPointer(mouse, out _);
                else if (colorBlindnessSelectionHitbox.IsPointerWithin(mouse))
                    colorBlindnessSelectionHitbox.ProcessPointer(mouse, out _);
                else if (colorBlindnessSelectionArrowUpHitbox.IsPointerWithin(mouse))
                    colorBlindnessSelectionArrowUpHitbox.ProcessPointer(mouse, out _);
                else if (colorBlindnessSelectionArrowDownHitbox.IsPointerWithin(mouse))
                    colorBlindnessSelectionArrowDownHitbox.ProcessPointer(mouse, out _);
            }
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void ChangeHue(bool goBack)
        {
            if (goBack)
                DecrementHue(type);
            else
                IncrementHue(type);
        }

        private void ChangeLightness(bool goBack)
        {
            if (goBack)
                DecrementLightness(type);
            else
                IncrementLightness(type);
        }

        private void ChangeSaturation(bool goBack)
        {
            if (goBack)
                DecrementSaturation(type);
            else
                IncrementSaturation(type);
        }

        private void ChangeTransparency(bool goBack)
        {
            if (goBack)
                finalSettings.Opacity--;
            else
                finalSettings.Opacity++;
        }

        private void ChangeColor(bool goBack)
        {
            if (goBack)
                DecrementColor(type);
            else
                IncrementColor(type);
        }

        private void ShowColorList() =>
            showColorList = !showColorList;

        private void DecrementColor(ColorType type)
        {
            switch (type)
            {
                case ColorType.TrueColor:
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
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void IncrementColor(ColorType type)
        {
            switch (type)
            {
                case ColorType.TrueColor:
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
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void DecrementHue(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorHue--;
            if (trueColorHue < 0)
                trueColorHue = 360;
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void IncrementHue(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorHue++;
            if (trueColorHue > 360)
                trueColorHue = 0;
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void DecrementLightness(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorLightness--;
            if (trueColorLightness < 0)
                trueColorLightness = 100;
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void IncrementLightness(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorLightness++;
            if (trueColorLightness > 100)
                trueColorLightness = 0;
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void DecrementSaturation(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorSaturation--;
            if (trueColorSaturation < 0)
                trueColorSaturation = 100;
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void IncrementSaturation(ColorType type)
        {
            if (type != ColorType.TrueColor)
                return;
            trueColorSaturation++;
            if (trueColorSaturation > 100)
                trueColorSaturation = 0;
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        internal ColorSelectorTui(Color color, ColorSettings settings, bool readOnly)
        {
            initialColor = color;
            selectedColor = color;
            finalSettings = settings;
            this.readOnly = readOnly;

            // Reset some variables
            colorBlindnessSimulationIdx = 0;
            colorBlindnessSeverity = 0.6;

            // Set initial colors
            var hsl = ConversionTools.ToHsl(selectedColor.RGB);
            type = selectedColor.Type;
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
                    throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_IS_COLOR_EXCEPTION_INVALIDCOLORTYPE"));
            }
            UpdateColor(ref selectedColor, type, finalSettings);
            UpdateKeybindings();
        }
    }
}
