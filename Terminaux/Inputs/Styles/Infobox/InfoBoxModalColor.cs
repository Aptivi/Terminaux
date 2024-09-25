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
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.FancyWriters;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Textify.General;
using System.Diagnostics;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.MiscWriters.Tools;
using Terminaux.Writer.MiscWriters;

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
                    return RenderText(0, title, text, settings, InfoBoxModalTitledColor, BackgroundColor, useColor, ref increment, currIdx, true, true, vars);
                });

                // Main loop
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Wait until the user presses any key to close the box
                    string[] splitFinalLines = GetFinalLines(text, vars);
                    var (_, maxHeightOut, _, _, _) = GetDimensions(splitFinalLines);
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    splitFinalLines = GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensions(splitFinalLines);
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
                            string buttons = "[K][X]";
                            int buttonsLeftMin = maxWidth + borderX - buttons.Length;
                            int buttonsLeftMax = buttonsLeftMin + buttons.Length;
                            int buttonsTop = borderY;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (buttonsLeftMin, buttonsTop),
                                    (buttonsLeftMax, buttonsTop));
                        }

                        void DoActionBasedOnButtonPress(PointerEventContext mouse)
                        {
                            string buttons = "[K][X]";
                            int buttonLeftHelpMin = maxWidth + borderX - buttons.Length;
                            int buttonLeftHelpMax = buttonLeftHelpMin + 2;
                            int buttonLeftCloseMin = buttonLeftHelpMin + 3;
                            int buttonLeftCloseMax = buttonLeftHelpMin + buttons.Length;
                            int buttonsTop = borderY;
                            if (mouse.Coordinates.y == buttonsTop)
                            {
                                if (PointerTools.PointerWithinRange(mouse, (buttonLeftHelpMin, buttonsTop), (buttonLeftHelpMax, buttonsTop)))
                                    KeybindingsWriter.ShowKeybindingInfobox(keybindings);
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
                                KeybindingsWriter.ShowKeybindingInfobox(keybindings);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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

        internal static string[] GetFinalLines(string text, params object[] vars)
        {
            // Deal with the lines to actually fit text in the infoboxmodal
            string finalInfoRendered = text.FormatString(vars);
            string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
            List<string> splitFinalLines = [];
            foreach (var line in splitLines)
            {
                var lineSentences = ConsoleMisc.GetWrappedSentencesByWords(line, ConsoleWrapper.WindowWidth - 4);
                foreach (var lineSentence in lineSentences)
                    splitFinalLines.Add(lineSentence);
            }

            // Trim the new lines until we reach a full line
            for (int i = splitFinalLines.Count - 1; i >= 0; i--)
            {
                string line = splitFinalLines[i];
                if (!string.IsNullOrWhiteSpace(line))
                    break;
                splitFinalLines.RemoveAt(i);
            }
            return [.. splitFinalLines];
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) GetDimensions(string[] splitFinalLines)
        {
            int maxWidth = splitFinalLines.Max(ConsoleChar.EstimateCellWidth);
            if (maxWidth >= ConsoleWrapper.WindowWidth)
                maxWidth = ConsoleWrapper.WindowWidth - 4;
            int maxHeight = splitFinalLines.Length;
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) GetDimensionsInput(string[] splitFinalLines)
        {
            int maxWidth = ConsoleWrapper.WindowWidth - 4;
            int maxHeight = splitFinalLines.Length + 5;
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int selectionBoxPosX, int selectionBoxPosY, int leftPos, int maxSelectionWidth, int left, int selectionReservedHeight) GetDimensionsSelection(InputChoiceInfo[] selections, string[] splitFinalLines)
        {
            int selectionChoices = selections.Length > 10 ? 10 : selections.Length;
            int selectionReservedHeight = 4 + selectionChoices;
            int maxWidth = ConsoleWrapper.WindowWidth - 4;
            int maxHeight = splitFinalLines.Length + selectionReservedHeight;
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;

            // Fill in some selection properties
            int selectionBoxPosX = borderX + 4;
            int selectionBoxPosY = borderY + maxHeight - selectionReservedHeight + 3;
            int leftPos = selectionBoxPosX + 1;
            int maxSelectionWidth = maxWidth - selectionBoxPosX * 2 + 2;
            int left = maxWidth - 2;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, selectionBoxPosX, selectionBoxPosY, leftPos, maxSelectionWidth, left, selectionReservedHeight);
        }

        internal static string RenderText(
            int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxModalColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infoboxmodal
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensions(splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, maxHeightOffset, title, text, settings, InfoBoxModalColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, writeBinding, vars);
        }

        internal static string RenderTextInput(
            int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxModalColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infoboxmodal
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensionsInput(splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, maxHeightOffset, title, text, settings, InfoBoxModalColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, writeBinding, vars);
        }

        internal static string RenderTextSelection(
            InputChoiceInfo[] choices, string title, string text, BorderSettings settings, Color InfoBoxModalColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infoboxmodal
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY, _, _, _, _, _, selectionReservedHeight) = GetDimensionsSelection(choices, splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, selectionReservedHeight, title, text, settings, InfoBoxModalColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, true, vars);
        }

        internal static string RenderText(
            int maxWidth, int maxHeight, int borderX, int borderY, int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxModalColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infoboxmodal
            string buttons = "[K][X]";
            string[] splitFinalLines = GetFinalLines(text, vars);

            // Fill the info box with text inside it
            var boxBuffer = new StringBuilder();
            string border =
                !string.IsNullOrEmpty(title) ?
                BorderColor.RenderBorderPlain(writeBinding && maxWidth >= buttons.Length + 2 ? title.Truncate(maxWidth - buttons.Length - 7) : title, borderX, borderY, maxWidth, maxHeight, settings) :
                BorderColor.RenderBorderPlain(borderX, borderY, maxWidth, maxHeight, settings);
            boxBuffer.Append(
                $"{(useColor ? InfoBoxModalColor.VTSequenceForeground : "")}" +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                $"{border}"
            );

            // Render text inside it
            ConsoleWrapper.CursorVisible = false;
            int linesMade = 0;
            for (int i = currIdx; i < splitFinalLines.Length; i++)
            {
                var line = splitFinalLines[i];
                if (linesMade % (maxHeight - maxHeightOffset) == 0 && linesMade > 0)
                {
                    // Reached the end of the box. Bail.
                    increment = linesMade;
                    break;
                }
                boxBuffer.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + linesMade % maxHeight + 1)}" +
                    $"{line}"
                );
                linesMade++;
            }

            // Render the vertical bar
            int left = maxWidth + borderX + 1;
            if (splitFinalLines.Length > maxHeight - maxHeightOffset && drawBar)
            {
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↑", left, 2, InfoBoxModalColor, BackgroundColor));
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↓", left, maxHeight - maxHeightOffset + 1, InfoBoxModalColor, BackgroundColor));
                boxBuffer.Append(SliderVerticalColor.RenderVerticalSlider((int)((double)currIdx / (splitFinalLines.Length - (maxHeight - maxHeightOffset)) * splitFinalLines.Length), splitFinalLines.Length, left - 1, 2, maxHeight - maxHeightOffset - 2, InfoBoxModalColor, BackgroundColor, BackgroundColor, false));
            }

            // Render a keybinding that points to the help page
            if (writeBinding && maxWidth >= buttons.Length + 2)
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(buttons, left - buttons.Length - 1, borderY, InfoBoxModalColor, BackgroundColor));
            return boxBuffer.ToString();
        }

        static InfoBoxModalColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
