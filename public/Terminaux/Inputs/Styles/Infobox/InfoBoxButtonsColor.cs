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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors;
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using System.Diagnostics;
using Terminaux.Colors.Data;
using System.Linq;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using System.Threading;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters;
using System.Collections.Generic;

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
        ];

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsPlain(InputChoiceInfo[] buttons, string text, params object[] vars) =>
            WriteInfoBoxButtonsPlain("", buttons, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsPlain(InputChoiceInfo[] buttons, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxButtonsPlain("", buttons, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtons(InputChoiceInfo[] buttons, string text, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(buttons, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="InfoBoxButtonsColor">InfoBoxButtons color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColor(InputChoiceInfo[] buttons, string text, Color InfoBoxButtonsColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(buttons, text, BorderSettings.GlobalSettings, InfoBoxButtonsColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="InfoBoxButtonsColor">InfoBoxButtons color</param>
        /// <param name="BackgroundColor">InfoBoxButtons background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColorBack(InputChoiceInfo[] buttons, string text, Color InfoBoxButtonsColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(buttons, text, BorderSettings.GlobalSettings, InfoBoxButtonsColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtons(InputChoiceInfo[] buttons, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(buttons, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxButtonsColor">InfoBoxButtons color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColor(InputChoiceInfo[] buttons, string text, BorderSettings settings, Color InfoBoxButtonsColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(buttons, text, settings, InfoBoxButtonsColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxButtonsColor">InfoBoxButtons color</param>
        /// <param name="BackgroundColor">InfoBoxButtons background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColorBack(InputChoiceInfo[] buttons, string text, BorderSettings settings, Color InfoBoxButtonsColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack("", buttons, text, settings, InfoBoxButtonsColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsPlain(string title, InputChoiceInfo[] buttons, string text, params object[] vars) =>
            WriteInfoBoxButtonsPlain(title, buttons, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsPlain(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(title, buttons, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtons(string title, InputChoiceInfo[] buttons, string text, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(title, buttons, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="InfoBoxTitledButtonsColor">InfoBoxTitledButtons color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColor(string title, InputChoiceInfo[] buttons, string text, Color InfoBoxTitledButtonsColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(title, buttons, text, BorderSettings.GlobalSettings, InfoBoxTitledButtonsColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="InfoBoxTitledButtonsColor">InfoBoxTitledButtons color</param>
        /// <param name="BackgroundColor">InfoBoxTitledButtons background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColorBack(string title, InputChoiceInfo[] buttons, string text, Color InfoBoxTitledButtonsColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(title, buttons, text, BorderSettings.GlobalSettings, InfoBoxTitledButtonsColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtons(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(title, buttons, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledButtonsColor">InfoBoxTitledButtons color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColor(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, Color InfoBoxTitledButtonsColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(title, buttons, text, settings, InfoBoxTitledButtonsColor,
                ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledButtonsColor">InfoBoxTitledButtons color</param>
        /// <param name="BackgroundColor">InfoBoxTitledButtons background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxButtonsColorBack(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, Color InfoBoxTitledButtonsColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxButtonsColorBack(title, buttons, text, settings, InfoBoxTitledButtonsColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledButtonsColor">InfoBoxTitledButtons color</param>
        /// <param name="BackgroundColor">InfoBoxTitledButtons background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        internal static int WriteInfoBoxButtonsColorBack(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, Color InfoBoxTitledButtonsColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            // First, check the buttons count
            if (buttons is null || buttons.Length == 0)
                return -1;
            if (buttons.Length > 3)
            {
                // Looks like that we have more than three buttons. Use the selection choice instead.
                return InfoBoxSelectionColor.WriteInfoBoxSelectionColorBack(title, buttons, text, InfoBoxTitledButtonsColor, BackgroundColor);
            }

            // Now, the button selection
            int selectedButton = buttons.Any((ici) => ici.ChoiceDefault) ? buttons.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
            bool cancel = false;
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxButtonsColor), infoBoxScreenPart);
            try
            {
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 5);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(5, title, text, settings, InfoBoxTitledButtonsColor, BackgroundColor, useColor, ref increment, currIdx, true, true, vars)
                    );

                    // Get the button width list
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

                    // Place the buttons from the right for familiarity
                    int buttonPanelPosX = borderX + 2;
                    int buttonPanelPosY = borderY + maxHeight - 3;
                    for (int i = 1; i <= buttons.Length; i++)
                    {
                        // Get the text and the button position
                        string buttonText = buttons[i - 1].ChoiceTitle;
                        int sumWidth = buttonWidths.Take(i).Sum();
                        int finalWidth = buttonWidths[i - 1];
                        int buttonX = buttonPanelPosX + maxButtonPanelWidth - sumWidth - ((i - 1) * 3);

                        // Determine whether it's a selected button or not
                        bool selected = i == selectedButton + 1;
                        var buttonForegroundColor = selected ? BackgroundColor : InfoBoxTitledButtonsColor;
                        var buttonBackgroundColor = selected ? InfoBoxTitledButtonsColor : BackgroundColor;

                        // Render the button box
                        var border = new Border()
                        {
                            Left = buttonX,
                            Top = buttonPanelPosY,
                            InteriorWidth = finalWidth,
                            InteriorHeight = 1,
                            Text = buttonText,
                        };
                        if (useColor)
                        {
                            border.Color = buttonForegroundColor;
                            border.BackgroundColor = buttonBackgroundColor;
                            border.TextColor = buttonForegroundColor;
                        }
                        boxBuffer.Append(border.Render());
                    }

                    // Reset colors
                    if (useColor)
                    {
                        boxBuffer.Append(
                            ColorTools.RenderRevertForeground() +
                            ColorTools.RenderRevertBackground()
                        );
                    }
                    return boxBuffer.ToString();
                });

                // Loop for input
                bool bail = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Wait for keypress
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 5);
                    maxHeight -= 5;
                    if (Input.MouseInputAvailable)
                    {
                        bool DetermineArrowPressed(PointerEventContext mouse)
                        {
                            if (splitFinalLines.Length <= maxHeight)
                                return false;
                            int arrowLeft = maxWidth + borderX + 1;
                            int arrowTop = 2;
                            int arrowBottom = maxHeight + 1;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (arrowLeft, arrowTop),
                                    (arrowLeft, arrowBottom));
                        }

                        void UpdatePositionBasedOnArrowPress(PointerEventContext mouse)
                        {
                            if (splitFinalLines.Length <= maxHeight)
                                return;
                            int arrowLeft = maxWidth + borderX + 1;
                            int arrowTop = 2;
                            int arrowBottom = maxHeight + 1;
                            if (mouse.Coordinates.x == arrowLeft)
                            {
                                if (mouse.Coordinates.y == arrowTop)
                                {
                                    currIdx -= 1;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                }
                                else if (mouse.Coordinates.y == arrowBottom)
                                {
                                    currIdx += 1;
                                    if (currIdx > splitFinalLines.Length - maxHeight)
                                        currIdx = splitFinalLines.Length - maxHeight;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                }
                            }
                        }

                        void UpdateHighlightBasedOnMouse(PointerEventContext mouse)
                        {
                            int buttonPanelPosX = borderX + 2;
                            int buttonPanelPosY = borderY + maxHeight + 5 - 3;
                            if (mouse.Coordinates.y < buttonPanelPosY || mouse.Coordinates.y > buttonPanelPosY + 2)
                                return;
                            int maxButtonPanelWidth = maxWidth - 4;
                            int maxButtonWidth = maxButtonPanelWidth / 4;
                            for (int i = 1; i <= buttons.Length; i++)
                            {
                                // Get the button position
                                int buttonX = buttonPanelPosX + maxButtonPanelWidth - i * maxButtonWidth;
                                if (mouse.Coordinates.x < buttonX || mouse.Coordinates.x >= buttonX + maxButtonWidth - 1)
                                    continue;

                                // Now, change the highlight
                                selectedButton = i - 1;
                            }
                        }

                        bool DetermineButtonsPressed(PointerEventContext mouse)
                        {
                            string buttons = InfoBoxTools.GetButtons(settings);
                            int buttonsWidth = ConsoleChar.EstimateCellWidth(buttons);
                            int buttonsLeftMin = maxWidth + borderX - buttonsWidth;
                            int buttonsLeftMax = buttonsLeftMin + buttonsWidth;
                            int buttonsTop = borderY;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (buttonsLeftMin, buttonsTop),
                                    (buttonsLeftMax, buttonsTop));
                        }

                        void DoActionBasedOnButtonPress(PointerEventContext mouse)
                        {
                            string buttons = InfoBoxTools.GetButtons(settings);
                            int buttonsWidth = ConsoleChar.EstimateCellWidth(buttons);
                            int buttonLeftHelpMin = maxWidth + borderX - buttonsWidth;
                            int buttonLeftHelpMax = buttonLeftHelpMin + 2;
                            int buttonLeftCloseMin = buttonLeftHelpMin + 3;
                            int buttonLeftCloseMax = buttonLeftHelpMin + buttonsWidth;
                            int buttonsTop = borderY;
                            if (mouse.Coordinates.y == buttonsTop)
                            {
                                if (PointerTools.PointerWithinRange(mouse, (buttonLeftHelpMin, buttonsTop), (buttonLeftHelpMax, buttonsTop)))
                                    KeybindingTools.ShowKeybindingInfobox(Keybindings);
                                else if (PointerTools.PointerWithinRange(mouse, (buttonLeftCloseMin, buttonsTop), (buttonLeftCloseMax, buttonsTop)))
                                {
                                    bail = true;
                                    cancel = true;
                                }
                            }
                        }

                        // Mouse input received.
                        var mouse = Input.ReadPointer();
                        if (mouse is null)
                            continue;
                        switch (mouse.Button)
                        {
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                UpdateHighlightBasedOnMouse(mouse);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (DetermineArrowPressed(mouse))
                                    UpdatePositionBasedOnArrowPress(mouse);
                                else if (DetermineButtonsPressed(mouse))
                                    DoActionBasedOnButtonPress(mouse);
                                else
                                {
                                    UpdateHighlightBasedOnMouse(mouse);
                                    bail = true;
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                var selectedInstance = buttons[selectedButton];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case PointerButton.WheelUp:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                {
                                    currIdx -= 3;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                }
                                else
                                {
                                    selectedButton--;
                                    if (selectedButton < 0)
                                        selectedButton = 0;
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                {
                                    currIdx += 3;
                                    if (currIdx > splitFinalLines.Length - maxHeight)
                                        currIdx = splitFinalLines.Length - maxHeight;
                                }
                                else
                                {
                                    selectedButton++;
                                    if (selectedButton > buttons.Length - 1)
                                        selectedButton = buttons.Length - 1;
                                }
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        var key = Input.ReadKey();
                        switch (key.Key)
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
                                    InfoBoxModalColor.WriteInfoBoxModal($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.E:
                                currIdx -= maxHeight;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.D:
                                currIdx += increment;
                                if (currIdx > splitFinalLines.Length - maxHeight)
                                    currIdx = splitFinalLines.Length - maxHeight;
                                break;
                            case ConsoleKey.W:
                                currIdx -= 1;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.S:
                                currIdx += 1;
                                if (currIdx > splitFinalLines.Length - maxHeight)
                                    currIdx = splitFinalLines.Length - maxHeight;
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

            // Return the selected button
            if (cancel)
                selectedButton = -1;
            return selectedButton;
        }

        static InfoBoxButtonsColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
