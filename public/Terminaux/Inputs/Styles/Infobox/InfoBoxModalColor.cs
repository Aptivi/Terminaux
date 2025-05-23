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
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Base.Structures;

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
            new Keybinding("Performs an action", PointerButton.Left),
            new Keybinding("Previous three lines of text", PointerButton.WheelUp),
            new Keybinding("Next three lines of text", PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the info box that appears and waits for user input
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModal(string text, params object[] vars) =>
            WriteInfoBoxModal(text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box that appears and waits for user input
        /// </summary>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxModal(string text, InfoBoxSettings settings, params object[] vars) =>
            WriteInfoBoxModalInternal(settings.Title, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, true, vars);

        #region To be removed
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalPlain(string text, params object[] vars) =>
            WriteInfoBoxModalPlain(text, BorderSettings.GlobalSettings, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalPlain(string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxModalPlain("", text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxModalColor">InfoBoxModal color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalColor(string text, Color InfoBoxModalColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, BorderSettings.GlobalSettings, InfoBoxModalColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxModalColor">InfoBoxModal color</param>
        /// <param name="BackgroundColor">InfoBoxModal background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalColorBack(string text, Color InfoBoxModalColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, BorderSettings.GlobalSettings, InfoBoxModalColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModal(string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxModalColorBack(text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxModalColor">InfoBoxModal color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalColorBack(string text, BorderSettings settings, Color InfoBoxModalColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalInternal("", text, settings, InfoBoxModalColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalPlain(string title, string text, params object[] vars) =>
            WriteInfoBoxModalPlain(title, text, BorderSettings.GlobalSettings, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalPlain(string title, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxModalInternal(title, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModal(string title, string text, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxModalTitledColor">InfoBoxModalTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalColorBack(string title, string text, Color InfoBoxModalTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalColorBack(title, text, BorderSettings.GlobalSettings, InfoBoxModalTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
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
        [Obsolete("This legacy function is to be removed from the final release of Terminaux 7.0. While you can use this in Beta 3, please move all settings to InfoBoxSettings. This is done to clean up the legacy codebase.")]
        public static void WriteInfoBoxModalColorBack(string title, string text, BorderSettings settings, Color InfoBoxModalTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxModalInternal(title, text, settings, InfoBoxModalTitledColor, BackgroundColor, true, vars);
        #endregion

        internal static void WriteInfoBoxModalInternal(string title, string text, BorderSettings settings, Color InfoBoxModalTitledColor, Color BackgroundColor, bool useColor, params object[] vars)
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
                    InputEventInfo data = Input.ReadPointerOrKey();
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines);

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
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                    // Handle input
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Mouse input received.
                        switch (mouse.Button)
                        {
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
                                break;
                            case PointerButton.WheelUp:
                                if (IsMouseWithinText(text, vars, mouse))
                                    GoUp(ref currIdx, 3);
                                break;
                            case PointerButton.WheelDown:
                                if (IsMouseWithinText(text, vars, mouse))
                                    GoDown(ref currIdx, text, vars, 3);
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Q:
                                bail = true;
                                break;
                            case ConsoleKey.PageUp:
                                GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.PageDown:
                            case ConsoleKey.Enter:
                                bail = cki.Key == ConsoleKey.Enter && currIdx == splitFinalLines.Length - maxHeight;
                                GoDown(ref currIdx, text, vars, increment);
                                break;
                            case ConsoleKey.UpArrow:
                                GoUp(ref currIdx);
                                break;
                            case ConsoleKey.DownArrow:
                                GoDown(ref currIdx, text, vars);
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

        private static bool IsMouseWithinText(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines);

            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (borderX + 1, borderY + 1), (borderX + maxWidth, borderY + maxHeight));
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
            var (_, maxHeight, _, _, _) = InfoBoxTools.GetDimensions(splitFinalLines);
            currIdx += level;
            if (currIdx > splitFinalLines.Length - maxHeight)
                currIdx = splitFinalLines.Length - maxHeight;
            if (currIdx < 0)
                currIdx = 0;
        }

        static InfoBoxModalColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
