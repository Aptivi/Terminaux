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

namespace Terminaux.Inputs.Presentation
{
    /// <summary>
    /// Presentation tools
    /// </summary>
    public static class PresentationTools
    {
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
            var buffer = new ScreenPart();
            ScreenTools.SetCurrent(screen);
            screen.AddBufferedPart("Presentation view", buffer);

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
                buffer.AddDynamicText(() =>
                {
                    var builder = new StringBuilder();

                    // Populate some variables
                    int presentationUpperBorderLeft = 2;
                    int presentationUpperBorderTop = 1;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 4;
                    int presentationInformationalTop = ConsoleWrapper.WindowHeight - 2;

                    // Make a border
                    builder.Append(
                        BoxFrameColor.RenderBoxFrame($"{(!kiosk ? $"[{i + 1}/{pages.Count}] - " : "")}{page.Name} - {presentation.Name}", presentationUpperBorderLeft, presentationUpperBorderTop, presentationLowerInnerBorderLeft, presentationLowerInnerBorderTop, new Color(ConsoleColors.Silver), ColorTools.CurrentBackgroundColor) +
                        BoxColor.RenderBox(presentationUpperInnerBorderLeft, presentationUpperBorderTop, presentationLowerInnerBorderLeft, presentationLowerInnerBorderTop)
                    );

                    // Write the bindings
                    builder.Append(
                        CenteredTextColor.RenderCentered(presentationInformationalTop, $"[ENTER] Advance{(!kiosk && !required ? $" - [ESC] Exit" : "")}".Truncate(presentationLowerInnerBorderLeft + 1), new Color(ConsoleColors.White), ColorTools.CurrentBackgroundColor)
                    );

                    // Clear the presentation screen
                    builder.Append(
                        ClearPresentation()
                    );

                    // Generate the final string
                    return builder.ToString();
                });

                // We need to dynamically render all the elements, so screen ends here.
                ScreenTools.Render();

                // Render all elements
                var pageElements = page.Elements;
                bool checkOutOfBounds = false;
                foreach (var element in pageElements)
                {
                    // Check for possible out-of-bounds
                    if (element.IsPossibleOutOfBounds() && checkOutOfBounds)
                    {
                        TermReader.ReadPointerOrKey();
                        TextWriterRaw.WriteRaw(ClearPresentation());
                    }
                    checkOutOfBounds = true;

                    // Render it to the view
                    element.Render();
                }

                // Wait for the ENTER key to be pressed if in kiosk mode. If not in kiosk mode, handle any key
                bool pageExit = false;
                while (!pageExit)
                {
                    // Get the keypress or mouse press
                    SpinWait.SpinUntil(() => PointerListener.InputAvailable);
                    if (PointerListener.PointerAvailable)
                    {
                        // Mouse input received.
                        var mouse = TermReader.ReadPointer();
                        switch (mouse.Button)
                        {
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                pageExit = ProcessInput(page);
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !PointerListener.PointerActive)
                    {
                        var key = TermReader.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.Escape:
                                if (required)
                                    break;
                                if (kiosk)
                                    break;
                                presentExit = true;
                                pageExit = ProcessInput(page);
                                break;
                            case ConsoleKey.Enter:
                                pageExit = ProcessInput(page);
                                break;
                        }
                    }
                }
            }

            // Clean up after ourselves
            ScreenTools.UnsetCurrent(screen);
            ColorTools.LoadBack();
            ConsoleWrapper.CursorVisible = true;
        }

        /// <summary>
        /// Clears the presentation
        /// </summary>
        public static string ClearPresentation()
        {
            var builder = new StringBuilder();

            // Populate some variables
            int presentationUpperBorderLeft = 2;
            int presentationUpperBorderTop = 1;
            int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
            int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
            int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
            int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 4;

            // Clear the presentation screen
            for (int y = presentationUpperInnerBorderTop; y <= presentationLowerInnerBorderTop + 1; y++)
                builder.Append(TextWriterWhereColor.RenderWhere(new string(' ', presentationLowerInnerBorderLeft), presentationUpperInnerBorderLeft, y));

            // Seek to the first position inside the border
            builder.Append(CsiSequences.GenerateCsiCursorPosition(presentationUpperInnerBorderLeft + 1, presentationUpperInnerBorderTop + 1));
            return builder.ToString();
        }

        private static bool ProcessInput(PresentationPage page)
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
                                    InfoBoxColor.WriteInfoBox("Incorrect Input", $"One or more of the following inputs have not been filled correctly:\n\n  - {string.Join("\n  - ", filledRequiredInputs.Except(processedRequiredInputs).Select((ii) => ii.InputName).ToArray())}");
                            }
                            else
                                InfoBoxColor.WriteInfoBox("Input not provided", $"Required inputs have not been provided. You'll need to fill in the values of the following inputs:\n\n  - {string.Join("\n  - ", requiredInputs.Except(filledRequiredInputs).Select((ii) => ii.InputName).ToArray())}");
                        }
                        else
                            inputBail = true;
                    }
                    else if (selected >= 0 && selected < page.Inputs.Length)
                    {
                        // User has selected one of the inputs. In this case, fetch the input instance and display it.
                        var input = page.Inputs[selected];
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
