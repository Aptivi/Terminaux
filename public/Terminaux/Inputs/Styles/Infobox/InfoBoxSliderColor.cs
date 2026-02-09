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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry.Transformation;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with slider and color support
    /// </summary>
    public static class InfoBoxSliderColor
    {
        private static Keybinding[] Keybindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_DECREMENTVALUE"), ConsoleKey.LeftArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_INCREMENTVALUE"), ConsoleKey.RightArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_DECREMENTVALUEMORE"), ConsoleKey.LeftArrow, ConsoleModifiers.Shift),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_INCREMENTVALUEMORE"), ConsoleKey.RightArrow, ConsoleModifiers.Shift),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MANUALVALUE"), ConsoleKey.Spacebar),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MINIMUMVALUE"), ConsoleKey.Home),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MAXIMUMVALUE"), ConsoleKey.End),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEUP"), ConsoleKey.W),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEDOWN"), ConsoleKey.S),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVPAGETEXT"), ConsoleKey.E),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTPAGETEXT"), ConsoleKey.D),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_SUBMIT_SINGULAR"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_CANCEL_SINGULAR"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PERFORMORSELECT"), PointerButton.Left),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELUP"), PointerButton.WheelUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELDOWN"), PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the slider info box
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSlider(int currentPos, int maxPos, string text, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSlider(currentPos, maxPos, text, InfoBoxSettings.GlobalSettings, minPos, vars);

        /// <summary>
        /// Writes the slider info box
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSlider(int currentPos, int maxPos, string text, InfoBoxSettings settings, int minPos = 0, params object[] vars)
        {
            // Prepare the screen
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            int selected = currentPos;
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSliderColor), infoBoxScreenPart);

            // Make a new infobox instance
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = 2;

            // Render it
            try
            {
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Fill the info box with text inside it
                    infoBox.Elements.RemoveRenderables();
                    var (maxWidth, maxHeight, _, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;
                    var boxBuffer = new StringBuilder(
                        infoBox.Render(ref increment, currIdx, true, true)
                    );

                    // Now, write the current position on the border of the slider bar and the arrows
                    int sliderPosX = borderX + 2;
                    int sliderPosY = borderY + maxHeight - 3;
                    int maxSliderWidth = maxWidth - 4;
                    var slider = new Slider(selected, minPos, maxPos)
                    {
                        Width = maxSliderWidth,
                        SliderActiveForegroundColor = settings.ForegroundColor,
                        SliderForegroundColor = TransformationTools.GetDarkBackground(settings.ForegroundColor),
                        SliderBackgroundColor = settings.BackgroundColor,
                        SliderVerticalActiveTrackChar = settings.BorderSettings.BorderRightFrameChar,
                        SliderVerticalInactiveTrackChar = settings.BorderSettings.BorderRightFrameChar,
                    };
                    if (settings.UseColors)
                    {
                        slider.SliderActiveForegroundColor = settings.ForegroundColor;
                        slider.SliderForegroundColor = TransformationTools.GetDarkBackground(settings.ForegroundColor);
                        slider.SliderBackgroundColor = settings.BackgroundColor;
                    }
                    boxBuffer.Append(
                        RendererTools.RenderRenderable(slider, new(sliderPosX + 1, sliderPosY + 2)) +
                        TextWriterWhereColor.RenderWhereColorBack($"{selected}", sliderPosX - 1 + (maxWidth / 2 - ConsoleChar.EstimateCellWidth($"{selected}") / 2), sliderPosY + 3, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", sliderPosX, sliderPosY + 2, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack($"{minPos}", sliderPosX + 1, sliderPosY + 3, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", sliderPosX + maxSliderWidth + 1, sliderPosY + 2, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack($"{maxPos}", sliderPosX + maxSliderWidth + 1 - ConsoleChar.EstimateCellWidth($"{maxPos}"), sliderPosY + 3, settings.ForegroundColor, settings.BackgroundColor)
                    );
                    return boxBuffer.ToString();
                });

                // Wait for input
                bool bail = false;
                bool cancel = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Handle keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var (maxWidth, maxHeight, _, borderX, borderY, maxTextHeight, linesLength) = infoBox.Dimensions;

                    // Get positions for arrows
                    int arrowLeft = maxWidth + borderX + 1;
                    int arrowTop = borderY + 1;
                    int arrowBottom = maxHeight + 1;

                    // Get positions for slider buttons
                    int maxSliderWidth = maxWidth - 4;
                    int sliderArrowTop = borderY + maxHeight - 1;
                    int sliderArrowLeft = borderX + 2;
                    int sliderArrowRight = sliderArrowLeft + maxSliderWidth + 1;

                    // Get positions for infobox buttons
                    string infoboxButtons = InfoBoxTools.GetButtons(settings.BorderSettings);
                    int infoboxButtonsWidth = ConsoleChar.EstimateCellWidth(infoboxButtons);
                    int infoboxButtonLeftHelpMin = maxWidth + borderX - infoboxButtonsWidth;
                    int infoboxButtonLeftHelpMax = infoboxButtonLeftHelpMin + 2;
                    int infoboxButtonLeftCloseMin = infoboxButtonLeftHelpMin + 3;
                    int infoboxButtonLeftCloseMax = infoboxButtonLeftHelpMin + infoboxButtonsWidth;
                    int infoboxButtonsTop = borderY;

                    // Make hitboxes for arrow and button presses
                    var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), new Action<PointerEventContext>((_) => InfoBoxTools.GoUp(ref currIdx))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), new Action<PointerEventContext>((_) => InfoBoxTools.GoDown(ref currIdx, infoBox))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSliderDecreaseHitbox = new PointerHitbox(new(sliderArrowLeft, sliderArrowTop), new Action<PointerEventContext>((_) => ValueGoUp(ref selected, minPos, maxPos))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSliderIncreaseHitbox = new PointerHitbox(new(sliderArrowRight, sliderArrowTop), new Action<PointerEventContext>((_) => ValueGoDown(ref selected, minPos, maxPos))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(Keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                    // Handle input
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoUp(ref currIdx, 3);
                                else if (IsMouseWithinSlider(infoBox, mouse))
                                    ValueGoUp(ref selected, minPos, maxPos);
                                break;
                            case PointerButton.WheelDown:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoDown(ref currIdx, infoBox, 3);
                                else if (IsMouseWithinSlider(infoBox, mouse))
                                    ValueGoDown(ref selected, minPos, maxPos);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && linesLength > maxHeight)
                                {
                                    arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowSliderIncreaseHitbox.IsPointerWithin(mouse) || arrowSliderDecreaseHitbox.IsPointerWithin(mouse)))
                                {
                                    arrowSliderIncreaseHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowSliderDecreaseHitbox.ProcessPointer(mouse, out done);
                                }
                                else if (infoboxButtonHelpHitbox.IsPointerWithin(mouse))
                                    infoboxButtonHelpHitbox.ProcessPointer(mouse, out _);
                                else if (infoboxButtonCloseHitbox.IsPointerWithin(mouse))
                                    infoboxButtonCloseHitbox.ProcessPointer(mouse, out _);
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                {
                                    int factor = 1;
                                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                                        factor = 100;
                                    ValueGoUp(ref selected, minPos, maxPos, factor);
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                {
                                    int factor = 1;
                                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                                        factor = 100;
                                    ValueGoDown(ref selected, minPos, maxPos, factor);
                                }
                                break;
                            case ConsoleKey.Spacebar:
                                string inputString = InfoBoxInputColor.WriteInfoBoxInput($"{selected}", LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ENTERNUMBERVALUE"), vars: [minPos, maxPos]);
                                if (!inputString.IsStringNumeric())
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NUMBERVALUEINVALID"));
                                    break;
                                }
                                SelectionSet(ref selected, minPos, maxPos, int.Parse(inputString));
                                break;
                            case ConsoleKey.Home:
                                SelectionSet(ref selected, minPos, maxPos, minPos);
                                break;
                            case ConsoleKey.End:
                                SelectionSet(ref selected, minPos, maxPos, maxPos);
                                break;
                            case ConsoleKey.E:
                                InfoBoxTools.GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.D:
                                InfoBoxTools.GoDown(ref currIdx, infoBox, increment);
                                break;
                            case ConsoleKey.W:
                                InfoBoxTools.GoUp(ref currIdx);
                                break;
                            case ConsoleKey.S:
                                InfoBoxTools.GoDown(ref currIdx, infoBox);
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                            case ConsoleKey.K:
                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(Keybindings);
                                break;
                        }
                    }
                }
                if (cancel)
                    selected = currentPos;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                if (settings.UseColors)
                {
                    TextWriterRaw.WriteRaw(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
                    );
                }
                TextWriterRaw.WriteRaw(infoBox.Erase());
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
            return selected;
        }

        private static bool IsMouseWithinSlider(InfoBox infoBox, PointerEventContext mouse)
        {
            var (maxWidth, maxHeight, _, borderX, borderY, _, _) = infoBox.Dimensions;

            // Check the dimensions
            int maxSliderWidth = maxWidth - 4;
            int sliderArrowTop = borderY + maxHeight - 1;
            int sliderArrowLeft = borderX + 2;
            int sliderArrowRight = sliderArrowLeft + maxSliderWidth + 1;
            return PointerTools.PointerWithinRange(mouse, (sliderArrowLeft, sliderArrowTop), (sliderArrowRight, sliderArrowTop));
        }

        private static void ValueGoUp(ref int selected, int minPos, int maxPos, int factor = 1)
        {
            if (factor < 1)
                factor = 1;
            selected -= factor;
            if (selected < minPos)
                selected = factor > 1 ? minPos : maxPos;
        }

        private static void ValueGoDown(ref int selected, int minPos, int maxPos, int factor = 1)
        {
            if (factor < 1)
                factor = 1;
            selected += factor;
            if (selected > maxPos)
                selected = factor > 1 ? maxPos : minPos;
        }

        private static void SelectionSet(ref int selected, int minPos, int maxPos, int value)
        {
            selected = value;
            if (selected > maxPos)
                selected = maxPos;
            if (selected < minPos)
                selected = minPos;
        }
    }
}
