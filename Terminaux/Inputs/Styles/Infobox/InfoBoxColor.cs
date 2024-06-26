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
using Terminaux.Reader;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with color support
    /// </summary>
    public static class InfoBoxColor
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string text, params object[] vars) =>
            WriteInfoBoxPlain(text, BorderSettings.GlobalSettings, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string text, bool waitForInput, params object[] vars) =>
            WriteInfoBoxPlain("", text, BorderSettings.GlobalSettings, waitForInput, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string text, BorderSettings settings, bool waitForInput, params object[] vars) =>
            WriteInfoBoxPlain("", text, settings, waitForInput, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, params object[] vars) =>
            WriteInfoBoxColorBack(text, true, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput, params object[] vars) =>
            WriteInfoBoxColorBack(text, waitForInput, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string text, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxColorBack(text, true, BorderSettings.GlobalSettings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string text, bool waitForInput, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxColorBack(text, waitForInput, BorderSettings.GlobalSettings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string text, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack(text, true, BorderSettings.GlobalSettings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string text, bool waitForInput, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack(text, waitForInput, BorderSettings.GlobalSettings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxColorBack(text, true, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxColorBack(text, waitForInput, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string text, BorderSettings settings, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxColorBack(text, true, settings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack(text, true, settings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string text, bool waitForInput, BorderSettings settings, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxColorBack(text, waitForInput, settings, InfoBoxColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string text, bool waitForInput, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack("", text, waitForInput, settings, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string title, string text, params object[] vars) =>
            WriteInfoBoxPlain(title, text, BorderSettings.GlobalSettings, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string title, string text, bool waitForInput, params object[] vars) =>
            WriteInfoBoxPlain(title, text, BorderSettings.GlobalSettings, waitForInput, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string title, string text, BorderSettings settings, bool waitForInput, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, waitForInput, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string title, string text, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, true, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string title, string text, bool waitForInput, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, waitForInput, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string title, string text, Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, true, BorderSettings.GlobalSettings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string title, string text, bool waitForInput, Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, waitForInput, BorderSettings.GlobalSettings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string title, string text, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, true, BorderSettings.GlobalSettings, InfoBoxTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string title, string text, bool waitForInput, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, waitForInput, BorderSettings.GlobalSettings, InfoBoxTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string title, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, true, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string title, string text, bool waitForInput, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, waitForInput, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string title, string text, BorderSettings settings, Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, true, settings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string title, string text, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, true, settings, InfoBoxTitledColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColor(string title, string text, bool waitForInput, BorderSettings settings, Color InfoBoxTitledColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, waitForInput, settings, InfoBoxTitledColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxColorBack(string title, string text, bool waitForInput, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxColorBack(title, text, waitForInput, settings, InfoBoxTitledColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledColor">InfoBoxTitled color</param>
        /// <param name="BackgroundColor">InfoBoxTitled background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        internal static void WriteInfoBoxColorBack(string title, string text, bool waitForInput, BorderSettings settings, Color InfoBoxTitledColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            bool initialCursorVisible = waitForInput && ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var infoBoxPageScreenPart = new ScreenPart() { Order = 1 };
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxColor), infoBoxScreenPart);
            ScreenTools.CurrentScreen?.AddBufferedPart("Informational box - Page", infoBoxPageScreenPart);
            try
            {
                // Draw the border and the text
                int currIdx = 0;
                int increment = 0;
                bool exiting = false;
                bool delay = false;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    return RenderText(0, title, text, settings, InfoBoxTitledColor, BackgroundColor, useColor, ref increment, ref delay, ref exiting, currIdx, true, vars);
                });

                // Main loop
                while (!exiting)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Wait until the user presses any key to close the box
                    string[] splitFinalLines;
                    if (waitForInput)
                    {
                        SpinWait.SpinUntil(() => PointerListener.InputAvailable);
                        splitFinalLines = GetFinalLines(text, vars);
                        var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensions(splitFinalLines);
                        if (PointerListener.PointerAvailable)
                        {
                            bool DetermineArrowPressed(PointerEventContext mouse)
                            {
                                if (splitFinalLines.Length <= maxHeight)
                                    return false;
                                int arrowLeft = maxWidth + borderX + 1;
                                int arrowTop = 2;
                                int arrowBottom = maxHeight + 1;
                                return
                                    mouse.Coordinates.x == arrowLeft &&
                                    (mouse.Coordinates.y == arrowTop || mouse.Coordinates.y == arrowBottom);
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

                            // Mouse input received.
                            var mouse = TermReader.ReadPointer();
                            switch (mouse.Button)
                            {
                                case PointerButton.Left:
                                    if (mouse.ButtonPress != PointerButtonPress.Released)
                                        break;
                                    if (DetermineArrowPressed(mouse))
                                        UpdatePositionBasedOnArrowPress(mouse);
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
                            delay = false;
                            exiting = false;
                        }
                        else if (ConsoleWrapper.KeyAvailable && !PointerListener.PointerActive)
                        {
                            var key = TermReader.ReadKey();
                            switch (key.Key)
                            {
                                case ConsoleKey.Q:
                                    exiting = true;
                                    break;
                                case ConsoleKey.PageUp:
                                    currIdx -= maxHeight * 2 - 1;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                    delay = false;
                                    exiting = false;
                                    break;
                                case ConsoleKey.PageDown:
                                    currIdx += increment;
                                    if (currIdx > splitFinalLines.Length - maxHeight)
                                        currIdx = splitFinalLines.Length - maxHeight;
                                    delay = false;
                                    exiting = false;
                                    break;
                                case ConsoleKey.UpArrow:
                                    currIdx -= 1;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                    delay = false;
                                    exiting = false;
                                    break;
                                case ConsoleKey.DownArrow:
                                    currIdx += 1;
                                    if (currIdx > splitFinalLines.Length - maxHeight)
                                        currIdx = splitFinalLines.Length - maxHeight;
                                    delay = false;
                                    exiting = false;
                                    break;
                                case ConsoleKey.Home:
                                    currIdx = 0;
                                    delay = false;
                                    exiting = false;
                                    break;
                                case ConsoleKey.End:
                                    currIdx = splitFinalLines.Length - maxHeight;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                    delay = false;
                                    exiting = false;
                                    break;
                                case ConsoleKey.K:
                                    // Keys function
                                    WriteInfoBox("Available keybindings",
                                        """
                                        [UP]        | Goes one line up
                                        [DOWN]      | Goes one line down
                                        [HOME]      | Goes to the first line of text
                                        [END]       | Goes to the last line of text
                                        [PAGE UP]   | Goes to the previous page of text
                                        [PAGE DOWN] | Goes to the next page of text
                                        [ENTER]     | Goes to the next page of text, or closes the modal informational box
                                        [Q]         | Closes the modal informational box
                                        """
                                    );
                                    delay = false;
                                    exiting = false;
                                    break;
                            }
                        }

                        if (delay && !exiting)
                        {
                            currIdx += increment;
                            if (currIdx > splitFinalLines.Length - maxHeight)
                                currIdx = splitFinalLines.Length - maxHeight;
                        }
                    }
                    else if (delay)
                    {
                        Thread.Sleep(5000);
                        splitFinalLines = GetFinalLines(text, vars);
                        var (_, maxHeight, _, _, _) = GetDimensions(splitFinalLines);
                        currIdx += increment;
                        if (currIdx > splitFinalLines.Length - maxHeight)
                            currIdx = splitFinalLines.Length - maxHeight;
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
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxPageScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
        }

        internal static string[] GetFinalLines(string text, params object[] vars)
        {
            // Deal with the lines to actually fit text in the infobox
            string finalInfoRendered = TextTools.FormatString(text, vars);
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
            int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, ref bool delay, ref bool exiting, int currIdx, bool drawBar, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensions(splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, maxHeightOffset, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, ref delay, ref exiting, currIdx, drawBar, true, vars);
        }

        internal static string RenderTextInput(
            int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, ref bool delay, ref bool exiting, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensionsInput(splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, maxHeightOffset, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, ref delay, ref exiting, currIdx, drawBar, writeBinding, vars);
        }

        internal static string RenderTextSelection(
            InputChoiceInfo[] choices, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, ref bool delay, ref bool exiting, int currIdx, bool drawBar, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY, _, _, _, _, _, selectionReservedHeight) = GetDimensionsSelection(choices, splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, selectionReservedHeight, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, ref delay, ref exiting, currIdx, drawBar, true, vars);
        }

        internal static string RenderText(
            int maxWidth, int maxHeight, int borderX, int borderY, int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, ref bool delay, ref bool exiting, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = GetFinalLines(text, vars);

            // Fill the info box with text inside it
            var boxBuffer = new StringBuilder();
            string border =
                !string.IsNullOrEmpty(title) ?
                BorderColor.RenderBorderPlain(title, borderX, borderY, maxWidth, maxHeight, settings) :
                BorderColor.RenderBorderPlain(borderX, borderY, maxWidth, maxHeight, settings);
            boxBuffer.Append(
                $"{(useColor ? InfoBoxColor.VTSequenceForeground : "")}" +
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
                    delay = true;
                    break;
                }
                if (i == splitFinalLines.Length - 1)
                    exiting = true;
                else
                    // In case resize caused us to have an extra page
                    exiting = false;
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
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↑", left, 2, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↓", left, maxHeight - maxHeightOffset + 1, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(SliderVerticalColor.RenderVerticalSlider((int)((double)currIdx / (splitFinalLines.Length - (maxHeight - maxHeightOffset)) * splitFinalLines.Length), splitFinalLines.Length, left - 1, 2, maxHeight - maxHeightOffset - 2, InfoBoxColor, BackgroundColor, BackgroundColor, false));
            }

            // Render a keybinding that points to the help page
            if (writeBinding && maxWidth >= 5)
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("[K]", left - 4, borderY + maxHeight + 1, InfoBoxColor, BackgroundColor));
            return boxBuffer.ToString();
        }

        static InfoBoxColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
