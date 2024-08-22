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
using System.Text;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Reader;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using System.Threading;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using System.Collections.Generic;
using Terminaux.Inputs.Presentation.Inputs;
using System.Linq;
using Textify.General;
using Terminaux.Inputs.Presentation.Elements;
using Terminaux.Writer.MiscWriters.Tools;
using Terminaux.Writer.MiscWriters;

namespace Terminaux.Inputs.Presentation
{
    /// <summary>
    /// Presentation tools
    /// </summary>
    public static class PresentationTools
    {
        private readonly static Keybinding[] bindings =
        [
            new("Advance", ConsoleKey.Enter)
        ];
        private readonly static Keybinding[] nonKioskBindings =
        [
            .. bindings,
            new("Exit", ConsoleKey.Escape)
        ];

        /// <summary>
        /// Present the presentation
        /// </summary>
        /// <param name="presentation">Presentation instance</param>
        public static void Present(Slideshow presentation) =>
            Present(presentation, false, false);

        /// <summary>
        /// Present the presentation
        /// </summary>
        /// <param name="presentation">Presentation instance</param>
        /// <param name="kiosk">Prevent any key other than ENTER from being pressed</param>
        /// <param name="required">Prevents exiting the presentation</param>
        public static void Present(Slideshow presentation, bool kiosk, bool required)
        {
            // Make a screen instance for the presentation
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);

            // Loop for each page
            var pages = presentation.Pages;
            bool presentExit = false;
            for (int i = 0; i < pages.Count; i++)
            {
                // Check to see if we're exiting
                if (presentExit)
                    break;

                // Get the page
                var page = pages[i];

                // Fill the buffer
                var buffer = new ScreenPart();
                screen.AddBufferedPart($"Presentation view for {presentation.Name}", buffer);
                buffer.AddDynamicText(() =>
                {
                    var builder = new StringBuilder();

                    // Populate some variables
                    int presentationUpperBorderLeft = 2;
                    int presentationUpperBorderTop = 1;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;
                    int presentationInformationalTop = ConsoleWrapper.WindowHeight - 2;

                    // Make a border
                    builder.Append(
                        BoxFrameColor.RenderBoxFrame($"{(!kiosk ? $"[{i + 1}/{pages.Count}] - " : "")}{page.Name} - {presentation.Name}", presentationUpperBorderLeft, presentationUpperBorderTop, presentationLowerInnerBorderLeft, presentationLowerInnerBorderTop, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor) +
                        BoxColor.RenderBox(presentationUpperInnerBorderLeft, presentationUpperBorderTop, presentationLowerInnerBorderLeft, presentationLowerInnerBorderTop)
                    );

                    // Write the bindings
                    builder.Append(
                        KeybindingsWriter.RenderKeybindings(!kiosk && !required ? nonKioskBindings : bindings, 0, ConsoleWrapper.WindowHeight - 1)
                    );

                    // Clear the presentation screen
                    for (int y = presentationUpperInnerBorderTop; y <= presentationLowerInnerBorderTop + 1; y++)
                        builder.Append(TextWriterWhereColor.RenderWhere(new string(' ', presentationLowerInnerBorderLeft), presentationUpperInnerBorderLeft, y));

                    // Seek to the first position inside the border
                    builder.Append(CsiSequences.GenerateCsiCursorPosition(presentationUpperInnerBorderLeft + 1, presentationUpperInnerBorderTop + 1));

                    // Generate the final string
                    return builder.ToString();
                });

                // All the elements will be dynamically rendered. Make a presentation grid buffer and the final element rendered string builder.
                var gridBuffer = new ScreenPart();
                var renderedElements = new StringBuilder();

                // Render all elements
                var pageElements = page.Elements;
                for (int elementIdx = 0; elementIdx < pageElements.Length; elementIdx++)
                {
                    IElement? element = pageElements[elementIdx];
                    renderedElements.Append(element.RenderToString());
                    if (elementIdx < pageElements.Length - 1)
                        renderedElements.Append("\n\n");
                }

                // Helper function
                string rendered = renderedElements.ToString();
                string[] GetFinalLines()
                {
                    // Deal with the lines to actually fit text in the infobox
                    int presentationUpperBorderLeft = 2;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    string[] splitLines = rendered.SplitNewLines();
                    List<string> splitFinalLines = [];
                    foreach (var line in splitLines)
                    {
                        var lineSentences = ConsoleMisc.GetWrappedSentencesByWords(line, presentationLowerInnerBorderLeft);
                        foreach (var lineSentence in lineSentences)
                            splitFinalLines.Add(lineSentence);
                    }
                    return [.. splitFinalLines];
                }

                // Then, the text
                int currIdx = 0;
                int increment = 0;
                gridBuffer.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = GetFinalLines();

                    // Populate some variables
                    int presentationUpperBorderLeft = 2;
                    int presentationUpperBorderTop = 1;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;
                    int presentationInformationalTop = ConsoleWrapper.WindowHeight - 2;
                    var boxBuffer = new StringBuilder();
                    int linesMade = 0;
                    for (int i = currIdx; i < splitFinalLines.Length; i++)
                    {
                        var line = splitFinalLines[i];
                        if (linesMade % presentationLowerInnerBorderTop == 0 && linesMade > 0)
                        {
                            // Reached the end of the box. Bail.
                            increment = linesMade;
                            break;
                        }
                        boxBuffer.Append($"{CsiSequences.GenerateCsiCursorPosition(presentationUpperInnerBorderLeft + 1, presentationUpperInnerBorderTop + linesMade % presentationLowerInnerBorderTop + 1)}{line}");
                        linesMade++;
                    }

                    // Render the vertical bar
                    int left = presentationLowerInnerBorderLeft + 3;
                    if (splitFinalLines.Length > presentationLowerInnerBorderTop)
                    {
                        boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↑", left, 2, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor));
                        boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↓", left, presentationLowerInnerBorderTop + 1, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor));
                        boxBuffer.Append(SliderVerticalColor.RenderVerticalSlider((int)((double)currIdx / (splitFinalLines.Length - presentationLowerInnerBorderTop) * splitFinalLines.Length), splitFinalLines.Length, left - 1, 2, presentationLowerInnerBorderTop - 2, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor, false));
                    }
                    return boxBuffer.ToString();
                });
                screen.AddBufferedPart($"Grid view for {presentation.Name}", gridBuffer);

                // Wait for the ENTER key to be pressed if in kiosk mode. If not in kiosk mode, handle any key
                bool pageExit = false;
                while (!pageExit)
                {
                    // Get the keypress or mouse press
                    ScreenTools.Render();
                    SpinWait.SpinUntil(() => Input.InputAvailable);

                    // Get the lines and the positions
                    string[] splitFinalLines = GetFinalLines();
                    int presentationUpperBorderLeft = 2;
                    int presentationUpperBorderTop = 1;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;

                    // Then, determine if the pointer or the keypress is available
                    if (Input.MouseInputAvailable)
                    {
                        bool DetermineArrowPressed(PointerEventContext mouse)
                        {
                            if (splitFinalLines.Length <= presentationLowerInnerBorderTop)
                                return false;
                            int arrowLeft = presentationLowerInnerBorderLeft + 3;
                            int arrowTop = 2;
                            int arrowBottom = presentationLowerInnerBorderTop + 1;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (arrowLeft, arrowTop),
                                    (arrowLeft, arrowBottom));
                        }

                        void UpdatePositionBasedOnArrowPress(PointerEventContext mouse)
                        {
                            if (splitFinalLines.Length <= presentationLowerInnerBorderTop)
                                return;
                            int arrowLeft = presentationLowerInnerBorderLeft + 3;
                            int arrowTop = 2;
                            int arrowBottom = presentationLowerInnerBorderTop + 1;
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
                                    if (currIdx > splitFinalLines.Length - presentationLowerInnerBorderTop)
                                        currIdx = splitFinalLines.Length - presentationLowerInnerBorderTop;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                }
                            }
                        }

                        // Mouse input received.
                        var mouse = Input.ReadPointer();
                        switch (mouse.Button)
                        {
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (DetermineArrowPressed(mouse))
                                    UpdatePositionBasedOnArrowPress(mouse);
                                else
                                    pageExit = ProcessInput(page, screen);
                                screen.RequireRefresh();
                                break;
                            case PointerButton.WheelUp:
                                currIdx -= 3;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case PointerButton.WheelDown:
                                currIdx += 3;
                                if (currIdx > splitFinalLines.Length - presentationLowerInnerBorderTop)
                                    currIdx = splitFinalLines.Length - presentationLowerInnerBorderTop;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        // Get the key
                        var key = Input.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.Escape:
                                if (required)
                                    break;
                                if (kiosk)
                                    break;
                                presentExit = true;
                                pageExit = true;
                                break;
                            case ConsoleKey.Enter:
                                pageExit = ProcessInput(page, screen);
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.PageUp:
                                currIdx -= presentationLowerInnerBorderTop * 2 - 1;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.PageDown:
                                currIdx += increment;
                                if (currIdx > splitFinalLines.Length - presentationLowerInnerBorderTop)
                                    currIdx = splitFinalLines.Length - presentationLowerInnerBorderTop;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.UpArrow:
                                currIdx -= 1;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.DownArrow:
                                currIdx += 1;
                                if (currIdx > splitFinalLines.Length - presentationLowerInnerBorderTop)
                                    currIdx = splitFinalLines.Length - presentationLowerInnerBorderTop;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.Home:
                                currIdx = 0;
                                break;
                            case ConsoleKey.End:
                                currIdx = splitFinalLines.Length - presentationLowerInnerBorderTop;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                        }
                    }
                }
                screen.RemoveBufferedPart(gridBuffer.Id);
                screen.RemoveBufferedPart(buffer.Id);
            }

            // Clean up after ourselves
            ScreenTools.UnsetCurrent(screen);
            ColorTools.LoadBack();
            ConsoleWrapper.CursorVisible = true;
        }

        private static bool ProcessInput(PresentationPage page, Screen screen)
        {
            if (page.Inputs.Length > 0)
            {
                // Make a selection infobox that lets the user select the inputs
                bool inputBail = false;
                bool processed = false;
                while (!inputBail)
                {
                    // Populate the choices
                    List<InputChoiceInfo> choices = [];
                    for (int inputIdx = 0; inputIdx < page.Inputs.Length; inputIdx++)
                    {
                        InputInfo? input = page.Inputs[inputIdx];
                        choices.Add(new InputChoiceInfo($"{inputIdx + 1}{(input.InputRequired ? "*" : " ")}", $"{input.InputName} [{input.InputMethod.DisplayInput}]", input.InputDescription));
                    }
                    choices.Add(new InputChoiceInfo($"{page.Inputs.Length + 1}", "Submit", "Submits the required fields to the presentation"));
                    choices.Add(new InputChoiceInfo($"{page.Inputs.Length + 2}", "Exit", "Goes back to this presentation"));

                    // Let the user select an option, then process the input
                    screen.RequireRefresh();
                    int selected = InfoBoxSelectionColor.WriteInfoBoxSelection("Input required", [.. choices], "This presentation page requires the following inputs to be fulfilled before being able to advance to the next page. The asterisk next to each step indicates a required input that should be filled before being able to proceed.");
                    if (selected >= page.Inputs.Length)
                    {
                        // Either submit or exit has been selected.
                        if (selected == page.Inputs.Length)
                        {
                            // Check the required inputs if they have been filled
                            var requiredInputs = page.Inputs.Where((ii) => ii.InputRequired).ToArray();
                            var filledRequiredInputs = requiredInputs.Where((ii) => ii.InputMethod.Provided).ToArray();
                            if (filledRequiredInputs.Length == requiredInputs.Length)
                            {
                                // All required inputs have been provided. Submit the inputs to the presentation page after processing them.
                                var processedRequiredInputs = filledRequiredInputs.Where((ii) => ii.InputMethod.Process()).ToArray();
                                inputBail = processedRequiredInputs.Length == filledRequiredInputs.Length;
                                if (!inputBail)
                                {
                                    InfoBoxColor.WriteInfoBox("Incorrect Input", $"One or more of the following inputs have not been filled correctly:\n\n  - {string.Join("\n  - ", filledRequiredInputs.Except(processedRequiredInputs).Select((ii) => ii.InputName).ToArray())}");
                                }
                            }
                            else
                            {
                                screen.RequireRefresh();
                                InfoBoxColor.WriteInfoBox("Input not provided", $"Required inputs have not been provided. You'll need to fill in the values of the following inputs:\n\n  - {string.Join("\n  - ", requiredInputs.Except(filledRequiredInputs).Select((ii) => ii.InputName).ToArray())}");
                            }
                        }
                        else
                            inputBail = true;
                    }
                    else if (selected >= 0 && selected < page.Inputs.Length)
                    {
                        // User has selected one of the inputs. In this case, fetch the input instance and display it.
                        var input = page.Inputs[selected];
                        screen.RequireRefresh();
                        input.InputMethod.PromptInput();
                        processed = input.InputMethod.Process();
                    }
                    else
                        // User has exited the infobox
                        inputBail = true;
                }
                return processed;
            }
            else
                return true;
        }

        static PresentationTools()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
