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
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Structures;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with buttons and color support
    /// </summary>
    public static class InfoBoxButtonsColor
    {
        private static readonly Keybinding[] keybindings =
        [
            new Keybinding("Goes to the previous button", ConsoleKey.LeftArrow),
            new Keybinding("Goes to the next button", ConsoleKey.RightArrow),
            new Keybinding("Shows more info in an infobox", ConsoleKey.Tab),
            new Keybinding("Goes one line up", ConsoleKey.W),
            new Keybinding("Goes one line down", ConsoleKey.S),
            new Keybinding("Goes to the previous page of text", ConsoleKey.E),
            new Keybinding("Goes to the next page of text", ConsoleKey.D),
            new Keybinding("Submits the value", ConsoleKey.Enter),
            new Keybinding("Closes without submitting the value", ConsoleKey.Escape),
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
        public static int WriteInfoBoxButtons(InputChoiceInfo[] buttons, string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxButtonsInternal(settings.Title, buttons, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, vars);

        #region To be removed
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxButtonsPlain(InputChoiceInfo[] buttons, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxButtonsPlain("", buttons, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="InfoBoxButtonsColor">InfoBoxButtons color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxButtonsPlain(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxButtonsInternal(title, buttons, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="buttons">Button names to define. This must be from 1 to 3 buttons. Any more of them and you'll have to use the <see cref="InfoBoxSelectionColor"/> to get an option to use more buttons as choice selections.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static int WriteInfoBoxButtonsColorBack(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, Color InfoBoxTitledButtonsColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxButtonsInternal(title, buttons, text, settings, InfoBoxTitledButtonsColor, BackgroundColor, true, vars);
        #endregion

        internal static int WriteInfoBoxButtonsInternal(string title, InputChoiceInfo[] buttons, string text, BorderSettings settings, Color InfoBoxTitledButtonsColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            // First, check the buttons count
            if (buttons is null || buttons.Length == 0)
                return -1;
            if (buttons.Length > 3)
            {
                // Looks like that we have more than three buttons. Use the selection choice instead.
                // TODO: Use InfoBoxSettings variable instead of reconstructing once we remove deprecated functions.
                var finalSettings = new InfoBoxSettings()
                {
                    Title = title,
                    BorderSettings = settings,
                    ForegroundColor = InfoBoxTitledButtonsColor,
                    BackgroundColor = BackgroundColor,
                    UseColors = useColor,
                };
                return InfoBoxSelectionColor.WriteInfoBoxSelection(buttons, text, finalSettings);
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
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 3);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(3, title, text, settings, InfoBoxTitledButtonsColor, BackgroundColor, useColor, ref increment, currIdx, true, true, vars)
                    );

                    // Get the button width list
                    int maxButtonPanelWidth = maxWidth - 4;
                    int maxButtonWidth = maxButtonPanelWidth / 4 - 4;
                    var buttonWidths = GetButtonWidths(text, vars, buttons);

                    // Place the buttons from the right for familiarity
                    int buttonPanelPosX = borderX + 2;
                    int buttonPanelPosY = borderY + maxHeight - 2;
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
                            Width = finalWidth,
                            Height = 1,
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
                    InputEventInfo data = Input.ReadPointerOrKey();
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 3);
                    maxHeight -= 3;

                    // Get positions for arrows
                    int arrowLeft = maxWidth + borderX + 1;
                    int arrowTop = 2;
                    int arrowBottom = maxHeight + 1;

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
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var buttonHitboxes = GetButtonHitboxes(text, vars, buttons);

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
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && splitFinalLines.Length > maxHeight)
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
                                if (IsMouseWithinText(text, vars, mouse))
                                    GoUp(ref currIdx, 3);
                                else if (IsMouseWithinButtons(buttonHitboxes, mouse))
                                {
                                    selectedButton--;
                                    if (selectedButton < 0)
                                        selectedButton = 0;
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (IsMouseWithinText(text, vars, mouse))
                                    GoDown(ref currIdx, text, vars, 3);
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

        private static void GoUp(ref int currIdx, int level = 1)
        {
            currIdx -= level;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void GoDown(ref int currIdx, string text, object[] vars, int level = 1)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (_, maxHeight, _, _, _) = InfoBoxTools.GetDimensions(splitFinalLines, 3);
            currIdx += level;
            if (currIdx > splitFinalLines.Length - maxHeight)
                currIdx = splitFinalLines.Length - maxHeight;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static PointerHitbox[] GetButtonHitboxes(string text, object[] vars, InputChoiceInfo[] buttons)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 3);
            int buttonPanelPosX = borderX + 2;
            int buttonPanelPosY = borderY + maxHeight - 2;
            int maxButtonPanelWidth = maxWidth - 4;
            int maxButtonWidth = maxButtonPanelWidth / 4 - 4;
            List<PointerHitbox> hitboxes = [];
            var buttonWidths = GetButtonWidths(text, vars, buttons);
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

        private static bool IsMouseWithinText(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 3);
            maxHeight -= 3;

            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (borderX + 1, borderY + 1), (borderX + maxWidth, borderY + maxHeight));
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

        private static int[] GetButtonWidths(string text, object[] vars, InputChoiceInfo[] buttons)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, _, _, _, _) = InfoBoxTools.GetDimensions(splitFinalLines, 3);
            int maxButtonPanelWidth = maxWidth - 4;
            int maxButtonWidth = maxButtonPanelWidth / 4 - 4;
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

        static InfoBoxButtonsColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
