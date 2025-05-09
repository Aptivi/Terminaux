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
using Terminaux.Colors;
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using System.Diagnostics;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using System.Threading;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Base.Structures;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with slider and color support
    /// </summary>
    public static class InfoBoxSliderColor
    {
        private static readonly Keybinding[] keybindings =
        [
            new Keybinding("Decrements the current value", ConsoleKey.UpArrow),
            new Keybinding("Increments the current value", ConsoleKey.DownArrow),
            new Keybinding("Sets the value to the minimum value", ConsoleKey.Home),
            new Keybinding("Sets the value to the maximum value", ConsoleKey.End),
            new Keybinding("Goes one line up", ConsoleKey.W),
            new Keybinding("Goes one line down", ConsoleKey.S),
            new Keybinding("Goes to the previous page of text", ConsoleKey.E),
            new Keybinding("Goes to the next page of text", ConsoleKey.D),
            new Keybinding("Submits the value", ConsoleKey.Enter),
            new Keybinding("Closes without submitting the value", ConsoleKey.Escape),
        ];

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderPlain(int currentPos, int maxPos, string text, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderPlain(currentPos, maxPos, text, BorderSettings.GlobalSettings, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderPlain(int currentPos, int maxPos, string text, BorderSettings settings, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderPlain("", currentPos, maxPos, text, settings, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSlider(int currentPos, int maxPos, string text, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(currentPos, maxPos, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="InfoBoxSliderColor">InfoBoxSlider color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColor(int currentPos, int maxPos, string text, Color InfoBoxSliderColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(currentPos, maxPos, text, BorderSettings.GlobalSettings, InfoBoxSliderColor, ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="InfoBoxSliderColor">InfoBoxSlider color</param>
        /// <param name="BackgroundColor">InfoBoxSlider background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColorBack(int currentPos, int maxPos, string text, Color InfoBoxSliderColor, Color BackgroundColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(currentPos, maxPos, text, BorderSettings.GlobalSettings, InfoBoxSliderColor, BackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSlider(int currentPos, int maxPos, string text, BorderSettings settings, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(currentPos, maxPos, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSliderColor">InfoBoxSlider color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColor(int currentPos, int maxPos, string text, BorderSettings settings, Color InfoBoxSliderColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(currentPos, maxPos, text, settings, InfoBoxSliderColor, ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSliderColor">InfoBoxSlider color</param>
        /// <param name="BackgroundColor">InfoBoxSlider background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColorBack(int currentPos, int maxPos, string text, BorderSettings settings, Color InfoBoxSliderColor, Color BackgroundColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack("", currentPos, maxPos, text, settings, InfoBoxSliderColor, BackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderPlain(string title, int currentPos, int maxPos, string text, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderPlain(title, currentPos, maxPos, text, BorderSettings.GlobalSettings, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderPlain(string title, int currentPos, int maxPos, string text, BorderSettings settings, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(title, currentPos, maxPos, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, false, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSlider(string title, int currentPos, int maxPos, string text, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(title, currentPos, maxPos, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="InfoBoxTitledSliderColor">InfoBoxTitledSlider color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColor(string title, int currentPos, int maxPos, string text, Color InfoBoxTitledSliderColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(title, currentPos, maxPos, text, BorderSettings.GlobalSettings, InfoBoxTitledSliderColor, ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="InfoBoxTitledSliderColor">InfoBoxTitledSlider color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSlider background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColorBack(string title, int currentPos, int maxPos, string text, Color InfoBoxTitledSliderColor, Color BackgroundColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(title, currentPos, maxPos, text, BorderSettings.GlobalSettings, InfoBoxTitledSliderColor, BackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSlider(string title, int currentPos, int maxPos, string text, BorderSettings settings, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(title, currentPos, maxPos, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSliderColor">InfoBoxTitledSlider color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColor(string title, int currentPos, int maxPos, string text, BorderSettings settings, Color InfoBoxTitledSliderColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(title, currentPos, maxPos, text, settings, InfoBoxTitledSliderColor, ColorTools.currentBackgroundColor, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSliderColor">InfoBoxTitledSlider color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSlider background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static int WriteInfoBoxSliderColorBack(string title, int currentPos, int maxPos, string text, BorderSettings settings, Color InfoBoxTitledSliderColor, Color BackgroundColor, int minPos = 0, params object[] vars) =>
            WriteInfoBoxSliderColorBack(title, currentPos, maxPos, text, settings, InfoBoxTitledSliderColor, BackgroundColor, true, minPos, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="currentPos">Current position out of maximum position</param>
        /// <param name="minPos">Maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSliderColor">InfoBoxTitledSlider color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSlider background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        internal static int WriteInfoBoxSliderColorBack(string title, int currentPos, int maxPos, string text, BorderSettings settings, Color InfoBoxTitledSliderColor, Color BackgroundColor, bool useColor, int minPos = 0, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            int selected = currentPos;
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSliderColor), infoBoxScreenPart);
            try
            {
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 1);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(1, title, text, settings, InfoBoxTitledSliderColor, BackgroundColor, useColor, ref increment, currIdx, false, true, vars)
                    );

                    // Now, write the current position on the border of the slider bar and the arrows
                    int sliderPosX = borderX + 2;
                    int sliderPosY = borderY + maxHeight - 3;
                    int maxSliderWidth = maxWidth - 4;
                    var slider = new Slider(selected, minPos, maxPos)
                    {
                        Width = maxSliderWidth,
                        SliderActiveForegroundColor = InfoBoxTitledSliderColor,
                        SliderForegroundColor = TransformationTools.GetDarkBackground(InfoBoxTitledSliderColor),
                        SliderBackgroundColor = BackgroundColor,
                        SliderVerticalActiveTrackChar = settings.BorderRightFrameChar,
                        SliderVerticalInactiveTrackChar = settings.BorderRightFrameChar,
                    };
                    boxBuffer.Append(
                        RendererTools.RenderRenderable(slider, new(sliderPosX + 1, sliderPosY + 3)) +
                        TextWriterWhereColor.RenderWhereColorBack($"{settings.BorderRightHorizontalIntersectionChar} {selected} / {maxPos} {settings.BorderLeftHorizontalIntersectionChar}", sliderPosX - 1, sliderPosY + 4, InfoBoxTitledSliderColor, BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", sliderPosX, sliderPosY + 3, InfoBoxTitledSliderColor, BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", sliderPosX + maxSliderWidth + 1, sliderPosY + 3, InfoBoxTitledSliderColor, BackgroundColor)
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
                    var (mouse, key) = Input.ReadPointerOrKey();
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 1);

                    // Get positions for arrows
                    int arrowLeft = maxWidth + borderX + 1;
                    int arrowTop = 2;
                    int arrowBottom = maxHeight + 1;

                    // Get positions for slider buttons
                    int maxSliderWidth = maxWidth - 4;
                    int sliderArrowTop = borderY + maxHeight;
                    int sliderArrowLeft = borderX + 2;
                    int sliderArrowRight = sliderArrowLeft + maxSliderWidth + 1;

                    // Get positions for infobox buttons
                    string infoboxButtons = InfoBoxTools.GetButtons(settings);
                    int infoboxButtonsWidth = ConsoleChar.EstimateCellWidth(infoboxButtons);
                    int infoboxButtonLeftHelpMin = maxWidth + borderX - infoboxButtonsWidth;
                    int infoboxButtonLeftHelpMax = infoboxButtonLeftHelpMin + 2;
                    int infoboxButtonLeftCloseMin = infoboxButtonLeftHelpMin + 3;
                    int infoboxButtonLeftCloseMax = infoboxButtonLeftHelpMin + infoboxButtonsWidth;
                    int infoboxButtonsTop = borderY;

                    // Make hitboxes for arrow and button presses
                    var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), new Action<PointerEventContext>((_) => GoUp(ref currIdx))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), new Action<PointerEventContext>((_) => GoDown(ref currIdx, text, vars))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSliderDecreaseHitbox = new PointerHitbox(new(sliderArrowLeft, sliderArrowTop), new Action<PointerEventContext>((_) => ValueGoUp(ref selected, minPos, maxPos))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSliderIncreaseHitbox = new PointerHitbox(new(sliderArrowRight, sliderArrowTop), new Action<PointerEventContext>((_) => ValueGoDown(ref selected, minPos, maxPos))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                    // Handle input
                    if (mouse is not null)
                    {
                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                    GoUp(ref currIdx, 3);
                                else
                                    ValueGoUp(ref selected, minPos, maxPos);
                                break;
                            case PointerButton.WheelDown:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                    GoDown(ref currIdx, text, vars, 3);
                                else
                                    ValueGoDown(ref selected, minPos, maxPos);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && splitFinalLines.Length > maxHeight)
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
                    else if (key is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                ValueGoUp(ref selected, minPos, maxPos);
                                break;
                            case ConsoleKey.RightArrow:
                                ValueGoDown(ref selected, minPos, maxPos);
                                break;
                            case ConsoleKey.Home:
                                SelectionSet(ref selected, minPos);
                                break;
                            case ConsoleKey.End:
                                SelectionSet(ref selected, maxPos);
                                break;
                            case ConsoleKey.E:
                                GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.D:
                                GoDown(ref currIdx, text, vars, increment);
                                break;
                            case ConsoleKey.W:
                                GoUp(ref currIdx);
                                break;
                            case ConsoleKey.S:
                                GoDown(ref currIdx, text, vars);
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
                                KeybindingTools.ShowKeybindingInfobox(keybindings);
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
                if (useColor)
                {
                    TextWriterRaw.WriteRaw(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
            return selected;
        }

        private static void GoUp(ref int currIdx, int level = 1)
        {
            currIdx -= level;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void GoDown(ref int currIdx, string text, object[] vars, int level = 1)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (_, maxHeight, _, _, _) = InfoBoxTools.GetDimensions(splitFinalLines, 5);
            currIdx += level;
            if (currIdx > splitFinalLines.Length - maxHeight)
                currIdx = splitFinalLines.Length - maxHeight;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void ValueGoUp(ref int selected, int minPos, int maxPos)
        {
            selected--;
            if (selected < minPos)
                selected = maxPos;
        }

        private static void ValueGoDown(ref int selected, int minPos, int maxPos)
        {
            selected++;
            if (selected > maxPos)
                selected = minPos;
        }

        private static void SelectionSet(ref int selected, int value) =>
            selected = value;

        static InfoBoxSliderColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
