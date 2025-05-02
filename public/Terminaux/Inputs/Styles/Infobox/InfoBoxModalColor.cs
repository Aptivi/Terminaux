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
using System.Threading;
using Terminaux.Colors;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using System.Diagnostics;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with color support
    /// </summary>
    public static class InfoBoxModalColor
    {
        private static readonly Keybinding[] keybindings =
        [
            new Keybinding("Goes one line up", ConsoleKey.UpArrow),
            new Keybinding("Goes one line down", ConsoleKey.DownArrow),
            new Keybinding("Goes to the first line of text", ConsoleKey.Home),
            new Keybinding("Goes to the last line of text", ConsoleKey.End),
            new Keybinding("Goes to the previous page of text", ConsoleKey.PageUp),
            new Keybinding("Goes to the next page of text", ConsoleKey.PageDown),
            new Keybinding("Goes to the next page of text, or closes the modal informational box", ConsoleKey.Enter),
            new Keybinding("Closes the modal informational box", ConsoleKey.Escape),
        ];

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalPlain(string text, params object[] vars) =>
            WriteInfoBoxModalPlain(text, BorderSettings.GlobalSettings, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalPlain(string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxModalPlain("", text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModal(string text, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxModalColor">InfoBoxModal color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColor(string text, Color InfoBoxModalColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, BorderSettings.GlobalSettings, InfoBoxModalColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxModalColor">InfoBoxModal color</param>
        /// <param name="BackgroundColor">InfoBoxModal background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColorBack(string text, Color InfoBoxModalColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, BorderSettings.GlobalSettings, InfoBoxModalColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModal(string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxModalColor">InfoBoxModal color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColor(string text, BorderSettings settings, Color InfoBoxModalColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, settings, InfoBoxModalColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxModalColor">InfoBoxModal color</param>
        /// <param name="BackgroundColor">InfoBoxModal background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColorBack(string text, BorderSettings settings, Color InfoBoxModalColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalColorBack("", text, settings, InfoBoxModalColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalPlain(string title, string text, params object[] vars) =>
            WriteInfoBoxModalPlain(title, text, BorderSettings.GlobalSettings, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalPlain(string title, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModal(string title, string text, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxModalTitledColor">InfoBoxModalTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColor(string title, string text, Color InfoBoxModalTitledColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, BorderSettings.GlobalSettings, InfoBoxModalTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxModalTitledColor">InfoBoxModalTitled color</param>
        /// <param name="BackgroundColor">InfoBoxModalTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColorBack(string title, string text, Color InfoBoxModalTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, BorderSettings.GlobalSettings, InfoBoxModalTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModal(string title, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxModalTitledColor">InfoBoxModalTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColor(string title, string text, BorderSettings settings, Color InfoBoxModalTitledColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, settings, InfoBoxModalTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxModalTitledColor">InfoBoxModalTitled color</param>
        /// <param name="BackgroundColor">InfoBoxModalTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModalColorBack(string title, string text, BorderSettings settings, Color InfoBoxModalTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, settings, InfoBoxModalTitledColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxModalTitledColor">InfoBoxModalTitled color</param>
        /// <param name="BackgroundColor">InfoBoxModalTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        internal static void WriteInfoBoxModalColorBack(string title, string text, BorderSettings settings, Color InfoBoxModalTitledColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxModalScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxModalColor), infoBoxModalScreenPart);
            try
            {
                // Draw the border and the text
                int currIdx = 0;
                int increment = 0;
                bool bail = false;
                infoBoxModalScreenPart.AddDynamicText(() =>
                {
                    return InfoBoxTools.RenderText(0, title, text, settings, InfoBoxModalTitledColor, BackgroundColor, useColor, ref increment, currIdx, true, true, vars);
                });

                // Main loop
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Wait until the user presses any key to close the box
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines);
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
                                    KeybindingTools.ShowKeybindingInfobox(keybindings);
                                else if (PointerTools.PointerWithinRange(mouse, (buttonLeftCloseMin, buttonsTop), (buttonLeftCloseMax, buttonsTop)))
                                    bail = true;
                            }
                        }

                        // Mouse input received.
                        var mouse = Input.ReadPointer();
                        if (mouse is null)
                            continue;
                        switch (mouse.Button)
                        {
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (DetermineArrowPressed(mouse))
                                    UpdatePositionBasedOnArrowPress(mouse);
                                else if (DetermineButtonsPressed(mouse))
                                    DoActionBasedOnButtonPress(mouse);
                                break;
                            case PointerButton.WheelUp:
                                currIdx -= 3;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case PointerButton.WheelDown:
                                currIdx += 3;
                                if (currIdx > splitFinalLines.Length - maxHeight)
                                    currIdx = splitFinalLines.Length - maxHeight;
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        var key = Input.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.Q:
                                bail = true;
                                break;
                            case ConsoleKey.PageUp:
                                currIdx -= maxHeight;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.PageDown:
                            case ConsoleKey.Enter:
                                bail = key.Key == ConsoleKey.Enter && currIdx == splitFinalLines.Length - maxHeight;
                                currIdx += increment;
                                if (currIdx > splitFinalLines.Length - maxHeight)
                                    currIdx = splitFinalLines.Length - maxHeight;
                                break;
                            case ConsoleKey.UpArrow:
                                currIdx -= 1;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.DownArrow:
                                currIdx += 1;
                                if (currIdx > splitFinalLines.Length - maxHeight)
                                    currIdx = splitFinalLines.Length - maxHeight;
                                break;
                            case ConsoleKey.Home:
                                currIdx = 0;
                                break;
                            case ConsoleKey.End:
                                currIdx = splitFinalLines.Length - maxHeight;
                                if (currIdx < 0)
                                    currIdx = 0;
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
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxModalScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
        }

        static InfoBoxModalColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
