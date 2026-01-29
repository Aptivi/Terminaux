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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using System.Linq;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Structures;
using Textify.General;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with buttons and color support
    /// </summary>
    public static class InfoBoxButtonsColor
    {
        private static Keybinding[] Keybindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_BUTTONS_KEYBINDING_PREVBUTTON"), ConsoleKey.LeftArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_BUTTONS_KEYBINDING_NEXTBUTTON"), ConsoleKey.RightArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), ConsoleKey.Tab),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEUP"), ConsoleKey.W),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEDOWN"), ConsoleKey.S),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVPAGETEXT"), ConsoleKey.E),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTPAGETEXT"), ConsoleKey.D),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_SUBMIT_SINGULAR"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_CANCEL_SINGULAR"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PERFORMORSELECT"), PointerButton.Left),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), PointerButton.Right),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_BUTTONS_KEYBINDING_WHEELUP"), PointerButton.WheelUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_BUTTONS_KEYBINDING_WHEELDOWN"), PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the button info box
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtons(InputChoiceInfo[] buttons, string text, params object[] vars) =>
            WriteInfoBoxButtons(buttons, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the button info box
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtons(InputChoiceInfo[] buttons, string text, InfoBoxSettings settings, params object[] vars)
        {
            // First, check the buttons count
            if (buttons is null || buttons.Length == 0)
                return -1;
            if (buttons.Length > 3)
                return InfoBoxSelectionColor.WriteInfoBoxSelection(buttons, text, settings);

            // Now, the button selection
            int selectedButton = buttons.Any((ici) => ici.ChoiceDefault) ? buttons.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
            
            // Prepare the screen
            bool cancel = false;
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxButtonsColor), infoBoxScreenPart);

            // Make a new infobox instance
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = 3;

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

                    // Get the button width list
                    int maxButtonPanelWidth = maxWidth - 4;
                    var buttonWidths = GetButtonWidths(infoBox, buttons);

                    // Place the buttons from the right for familiarity
                    int buttonPanelPosX = borderX + 2;
                    int buttonPanelPosY = borderY + maxHeight - 2;
                    for (int i = 1; i <= buttons.Length; i++)
                    {
                        // Get the text and the button position
                        int sumWidth = buttonWidths.Take(i).Sum();
                        int finalWidth = buttonWidths[i - 1];
                        int buttonX = buttonPanelPosX + maxButtonPanelWidth - sumWidth - ((i - 1) * 3);
                        string buttonText = buttons[i - 1].ChoiceTitle.Truncate(finalWidth);

                        // Determine whether it's a selected button or not
                        bool selected = i == selectedButton + 1;
                        var buttonForegroundColor = selected ? settings.BackgroundColor : settings.ForegroundColor;
                        var buttonBackgroundColor = selected ? settings.ForegroundColor : settings.BackgroundColor;

                        // Render the button box
                        var border = new Border()
                        {
                            Left = buttonX,
                            Top = buttonPanelPosY,
                            Width = finalWidth,
                            Height = 1,
                            Text = buttonText,
                            UseColors = settings.UseColors,
                            Color = buttonForegroundColor,
                            BackgroundColor = buttonBackgroundColor,
                            TextColor = buttonForegroundColor
                        };
                        infoBox.Elements.AddRenderable($"Button box [{i}]", border);
                    }
                    return infoBox.Render(ref increment, currIdx, true, true);
                });

                // Loop for input
                bool bail = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Wait for keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var (maxWidth, maxHeight, _, borderX, borderY, maxTextHeight, linesLength) = infoBox.Dimensions;

                    // Get positions for arrows
                    int arrowLeft = maxWidth + borderX + 1;
                    int arrowTop = borderY + 1;
                    int arrowBottom = borderY + maxTextHeight;

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
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(Keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var buttonHitboxes = GetButtonHitboxes(infoBox, buttons);

                    // Handle input
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                if (!IsMouseWithinButtons(buttonHitboxes, mouse))
                                    break;
                                selectedButton = GetHighlightIndexBasedOnMouse(buttonHitboxes, mouse);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && linesLength > maxTextHeight)
                                {
                                    arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if (infoboxButtonHelpHitbox.IsPointerWithin(mouse))
                                    infoboxButtonHelpHitbox.ProcessPointer(mouse, out _);
                                else if (infoboxButtonCloseHitbox.IsPointerWithin(mouse))
                                    infoboxButtonCloseHitbox.ProcessPointer(mouse, out _);
                                else if (IsMouseWithinButtons(buttonHitboxes, mouse))
                                {
                                    selectedButton = GetHighlightIndexBasedOnMouse(buttonHitboxes, mouse);
                                    bail = true;
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (!IsMouseWithinButtons(buttonHitboxes, mouse))
                                    break;
                                selectedButton = GetHighlightIndexBasedOnMouse(buttonHitboxes, mouse);
                                var selectedInstance = buttons[selectedButton];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(choiceDesc, new InfoBoxSettings()
                                    {
                                        Title = $"[{choiceName}] {choiceTitle}",
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case PointerButton.WheelUp:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoUp(ref currIdx, 3);
                                else if (IsMouseWithinButtons(buttonHitboxes, mouse))
                                {
                                    selectedButton--;
                                    if (selectedButton < 0)
                                        selectedButton = 0;
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoDown(ref currIdx, infoBox, 3);
                                else if (IsMouseWithinButtons(buttonHitboxes, mouse))
                                {
                                    selectedButton++;
                                    if (selectedButton > buttons.Length - 1)
                                        selectedButton = buttons.Length - 1;
                                }
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                selectedButton++;
                                if (selectedButton > buttons.Length - 1)
                                    selectedButton = buttons.Length - 1;
                                break;
                            case ConsoleKey.RightArrow:
                                selectedButton--;
                                if (selectedButton < 0)
                                    selectedButton = 0;
                                break;
                            case ConsoleKey.Tab:
                                var selectedInstance = buttons[selectedButton];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(choiceDesc, new InfoBoxSettings()
                                    {
                                        Title = $"[{choiceName}] {choiceTitle}",
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.E:
                                InfoBoxTools.GoUp(ref currIdx, maxTextHeight);
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
            }
            catch (Exception ex)
            {
                cancel = true;
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

            // Return the selected button
            if (cancel)
                selectedButton = -1;
            return selectedButton;
        }

        private static PointerHitbox[] GetButtonHitboxes(InfoBox infoBox, InputChoiceInfo[] buttons)
        {
            var (maxWidth, maxHeight, _, borderX, borderY, _, _) = infoBox.Dimensions;
            int buttonPanelPosX = borderX + 2;
            int buttonPanelPosY = borderY + maxHeight - 2;
            int maxButtonPanelWidth = maxWidth - 4;
            int maxButtonWidth = maxButtonPanelWidth / 4;
            List<PointerHitbox> hitboxes = [];
            var buttonWidths = GetButtonWidths(infoBox, buttons);
            for (int i = 1; i <= buttons.Length; i++)
            {
                // Get the button position
                string buttonText = buttons[i - 1].ChoiceTitle;
                int sumWidth = buttonWidths.Take(i).Sum();
                int finalWidth = buttonWidths[i - 1];
                int buttonX = buttonPanelPosX + maxButtonPanelWidth - sumWidth - ((i - 1) * 3);

                // Now, generate a hitbox
                var hitbox = new PointerHitbox(new(buttonX, buttonPanelPosY), new Coordinate(buttonX + maxButtonWidth + 1, buttonPanelPosY + 2), null);
                hitboxes.Add(hitbox);
            }
            return [.. hitboxes];
        }

        private static bool IsMouseWithinButtons(PointerHitbox[] buttonHitboxes, PointerEventContext mouse) =>
            GetHighlightIndexBasedOnMouse(buttonHitboxes, mouse) != -1;

        private static int GetHighlightIndexBasedOnMouse(PointerHitbox[] buttonHitboxes, PointerEventContext mouse)
        {
            for (int i = 0; i < buttonHitboxes.Length; i++)
            {
                PointerHitbox? hitbox = buttonHitboxes[i];
                if (hitbox.IsPointerWithin(mouse))
                    return i;
            }
            return -1;
        }

        private static int[] GetButtonWidths(InfoBox infoBox, InputChoiceInfo[] buttons)
        {
            var (maxWidth, _, _, _, _, _, _) = infoBox.Dimensions;
            int maxButtonPanelWidth = maxWidth - 4;
            int maxButtonWidth = maxButtonPanelWidth / 4;
            List<int> buttonWidths = [];
            for (int i = 1; i <= buttons.Length; i++)
            {
                string buttonText = buttons[i - 1].ChoiceTitle;
                int buttonTextWidth = ConsoleChar.EstimateCellWidth(buttonText);
                int buttonWidth = buttonTextWidth >= maxButtonWidth ? maxButtonWidth : buttonTextWidth;
                buttonWidths.Add(buttonWidth);
            }
            return [.. buttonWidths];
        }
    }
}
