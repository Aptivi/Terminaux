//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;
using Terminaux.Colors.Models;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Colors.Transformation.Formulas;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Sequences.Builder.Types;
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
                var selectionBorder = new BoxFrame($"List of {finalSelections.Length} predefined colors")
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
                        Width = boxWidth,
                        Height = boxHeight * 3,
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
                        grayRamp.Append($"{new Color($"{gray};{gray};{gray}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                        int transparency = (int)(mono.RGB.originalAlpha * width);
                        transparencyRamp.Append($"{new Color($"{transparency};{transparency};{transparency}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                    }
                    var rampFrame = new BoxFrame($"Gray: {mono.RGB.R}/255 | Transp.: {finalSettings.Opacity}/255")
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
                    darkLightIndicator.Append($"{new Color(indicator, finalSettings).VTSequenceBackgroundTrueColor}    {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
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
                        redRamp.Append($"{new Color($"{red};0;0", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                        greenRamp.Append($"{new Color($"0;{green};0", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                        blueRamp.Append($"{new Color($"0;0;{blue}", finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
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
                        shadeRamp.Append($"{new Color(shades[i].IntermediateColor.ToString(), finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                        tintRamp.Append($"{new Color(tints[i].IntermediateColor.ToString(), finalSettings).VTSequenceBackgroundTrueColor} {ColorTools.RenderSetConsoleColor(initialBackground, true)}");
                    }
                    var shadeTintFrame = new BoxFrame("Shade and Tint")
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
                    var colorInfoFrame = new BoxFrame("Color info")
                    {
                        Left = hslBarX,
                        Top = infoRampBarY,
                        Width = halfBoxWidth,
                        Height = boxHeight + 2,
                    };
                    var colorBlindnessFrame = new BoxFrame($"Transform [[{colorBlindnessSeverity:0.00}]]")
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
                            TextWriterWhereColor.RenderWhere(wrappedMessage + spaces, x, y)
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
                KeybindingList = ColorSelector.bindings,
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
            Color finalColor = selectedColor;
            if (colorBlindnessSimulationIdx > 0)
            {
                var formula = (TransformationFormula)(colorBlindnessSimulationIdx - 1);
                var formulas = new BaseTransformationFormula[] { ColorSelector.transformationFormulas[formula] };
                formulas[0].Frequency = colorBlindnessSeverity;
                var transformed = new Color(selectedColor.RGB.R, selectedColor.RGB.G, selectedColor.RGB.B, new() { Transformations = formulas });
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
                Top = boxY,
                Width = boxWidth,
                Height = boxHeight / 2,
                Color = selectedColor,
            };
            var transformedBox = new Box()
            {
                Left = boxX + 1,
                Top = boxY + boxHeight / 2,
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
            Keybindings.Clear();

            // General keybindings
            Keybindings.Add((ColorSelector.bindings[0], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((ColorSelector.bindings[1], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((ColorSelector.bindings[2], Help));
            Keybindings.Add((ColorSelector.additionalBindingsGeneral[0], ShowColorInfo));

            // Simulation of color-blindness and transformations
            Keybindings.Add((ColorSelector.additionalBindingsGeneral[1], (ui, _, _) => ChangeSimulation(ui, false)));
            Keybindings.Add((ColorSelector.additionalBindingsGeneral[2], (ui, _, _) => ChangeSimulation(ui, true)));
            Keybindings.Add((ColorSelector.additionalBindingsGeneral[3], (ui, _, _) => ChangeSimulationSeverity(ui, false)));
            Keybindings.Add((ColorSelector.additionalBindingsGeneral[4], (ui, _, _) => ChangeSimulationSeverity(ui, true)));

            // These require write access
            if (!readOnly)
            {
                Keybindings.Add((ColorSelector.additionalBindingsReadWrite[5], SelectWebColor));
                Keybindings.Add((ColorSelector.additionalBindingsReadWrite[6], (_, _, _) =>
                {
                    finalSettings.Opacity++;
                    UpdateColor(ref selectedColor, type, finalSettings);
                }));
                Keybindings.Add((ColorSelector.additionalBindingsReadWrite[7], (_, _, _) =>
                {
                    finalSettings.Opacity--;
                    UpdateColor(ref selectedColor, type, finalSettings);
                }));
                Keybindings.Add((ColorSelector.additionalBindingsReadWrite[8], (ui, _, _) => ChangeMode(ui, false)));
                Keybindings.Add((ColorSelector.additionalBindingsReadWrite[9], (ui, _, _) => ChangeMode(ui, true)));

                // Mouse bindings
                Keybindings.Add((ColorSelector.additionalBindingsReadWrite[10], (_, _, mouse) => ChangeValue(mouse, false)));
                Keybindings.Add((ColorSelector.additionalBindingsReadWrite[11], (_, _, mouse) => ChangeValue(mouse, true)));

                // Type-specific bindings
                switch (type)
                {
                    case ColorType.TrueColor:
                        Keybindings.Add((ColorSelector.additionalBindingsTrueColor[12], (ui, _, _) => ChangeHue(ui, true)));
                        Keybindings.Add((ColorSelector.additionalBindingsTrueColor[13], (ui, _, _) => ChangeLightness(ui, true)));
                        Keybindings.Add((ColorSelector.additionalBindingsTrueColor[14], (ui, _, _) => ChangeSaturation(ui, true)));
                        Keybindings.Add((ColorSelector.additionalBindingsTrueColor[15], (ui, _, _) => ChangeHue(ui, false)));
                        Keybindings.Add((ColorSelector.additionalBindingsTrueColor[16], (ui, _, _) => ChangeLightness(ui, false)));
                        Keybindings.Add((ColorSelector.additionalBindingsTrueColor[17], (ui, _, _) => ChangeSaturation(ui, false)));
                        break;
                    case ColorType.EightBitColor:
                    case ColorType.FourBitColor:
                        Keybindings.Add((ColorSelector.additionalBindingsNormalColor[12], (ui, _, _) => ChangeColor(ui, true)));
                        Keybindings.Add((ColorSelector.additionalBindingsNormalColor[13], (ui, _, _) => ChangeColor(ui, false)));
                        Keybindings.Add((ColorSelector.additionalBindingsNormalColor[14], (ui, _, _) => ShowColorList(ui)));
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

        private void Help(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            Keybinding[] allBindings =
                readOnly ?
                [.. ColorSelector.bindings, ..ColorSelector.additionalBindingsGeneral] :
                type == ColorType.TrueColor ?
                [.. ColorSelector.bindings, ..ColorSelector.additionalBindingsTrueColor] :
                [.. ColorSelector.bindings, ..ColorSelector.additionalBindingsNormalColor] ;
            KeybindingTools.ShowKeybindingInfobox(allBindings);
            ui.RequireRefresh();
        }

        private void ShowColorInfo(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
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
            InfoBoxModalColor.WriteInfoBoxModal(title, rendered);
        }

        private void SelectWebColor(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            ui.RequireRefresh();
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

        private void ChangeMode(TextualUI ui, bool goBack)
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
            ui.RequireRefresh();
        }

        private void ChangeSimulation(TextualUI ui, bool goBack)
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
            ui.RequireRefresh();
        }

        private void ChangeSimulationSeverity(TextualUI ui, bool goBack)
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
            ui.RequireRefresh();
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

            // Detect boundaries
            bool withinColorBoxBoundaries = PointerTools.PointerWithinRange(mouse, (colorBoxX + 1, colorBoxY + 1), (colorBoxWidth + colorBoxX, boxHeight + colorBoxY));

            // Do the action!
            if (withinColorBoxBoundaries)
            {
                if (goBack)
                    DecrementColor(type);
                else
                    IncrementColor(type);
            }
            else
            {
                if (type != ColorType.TrueColor && showColorList)
                {
                    bool withinColorListBoundaries = PointerTools.PointerWithinRange(mouse, (generalX + 1, colorBoxY + 1), (colorBoxWidth + generalX, boxHeight + colorBoxY));
                    if (withinColorListBoundaries)
                    {
                        if (goBack)
                            DecrementColor(type);
                        else
                            IncrementColor(type);
                    }
                }
                else
                {
                    boxHeight = 2;
                    int hslBarY = 1;
                    int grayRampBarY = hslBarY + (boxHeight * 3) + 3;

                    // Detect boundaries
                    bool withinHueBarBoundaries = PointerTools.PointerWithinRange(mouse, (generalX + 1, hslBarY + 1), (generalX + boxWidth, hslBarY + 2));
                    bool withinSaturationBarBoundaries = PointerTools.PointerWithinRange(mouse, (generalX + 1, hslBarY + 3), (generalX + boxWidth, hslBarY + 4));
                    bool withinLightnessBarBoundaries = PointerTools.PointerWithinRange(mouse, (generalX + 1, hslBarY + 5), (generalX + boxWidth, hslBarY + 6));
                    bool withinTransparencyBarBoundaries = PointerTools.PointerWithinRange(mouse, (generalX + 1, grayRampBarY + 1), (generalX + boxWidth - 7, grayRampBarY + 2));

                    // Do the action!
                    if (goBack)
                    {
                        if (withinHueBarBoundaries)
                            DecrementHue(type);
                        else if (withinSaturationBarBoundaries)
                            DecrementSaturation(type);
                        else if (withinLightnessBarBoundaries)
                            DecrementLightness(type);
                        else if (withinTransparencyBarBoundaries)
                            finalSettings.Opacity--;
                    }
                    else
                    {
                        if (withinHueBarBoundaries)
                            IncrementHue(type);
                        else if (withinSaturationBarBoundaries)
                            IncrementSaturation(type);
                        else if (withinLightnessBarBoundaries)
                            IncrementLightness(type);
                        else if (withinTransparencyBarBoundaries)
                            finalSettings.Opacity++;
                    }
                }
            }
            UpdateColor(ref selectedColor, type, finalSettings);
        }

        private void ChangeHue(TextualUI ui, bool goBack)
        {
            if (goBack)
                DecrementHue(type);
            else
                IncrementHue(type);
            ui.RequireRefresh();
        }

        private void ChangeLightness(TextualUI ui, bool goBack)
        {
            if (goBack)
                DecrementLightness(type);
            else
                IncrementLightness(type);
            ui.RequireRefresh();
        }

        private void ChangeSaturation(TextualUI ui, bool goBack)
        {
            if (goBack)
                DecrementSaturation(type);
            else
                IncrementSaturation(type);
            ui.RequireRefresh();
        }

        private void ChangeColor(TextualUI ui, bool goBack)
        {
            if (goBack)
                DecrementColor(type);
            else
                IncrementColor(type);
            ui.RequireRefresh();
        }

        private void ShowColorList(TextualUI ui)
        {
            showColorList = !showColorList;
            ui.RequireRefresh();
        }

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
                    throw new TerminauxException("Invalid color type in the color selector");
            }
            UpdateColor(ref selectedColor, type, finalSettings);
            UpdateKeybindings();
        }
    }
}
