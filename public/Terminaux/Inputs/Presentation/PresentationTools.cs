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
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using System.Collections.Generic;
using System.Linq;
using Textify.General;
using Terminaux.Inputs.Presentation.Elements;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Base.Structures;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Inputs.Presentation
{
    /// <summary>
    /// Presentation tools
    /// </summary>
    public static class PresentationTools
    {
        private static Keybinding[] Bindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_PRESENTATION_KEYBINDING_ADVANCE"), ConsoleKey.Enter)
        ];

        private static Keybinding[] NonKioskBindings =>
        [
            .. Bindings,
            new(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_EXIT"), ConsoleKey.Escape)
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
            ConsoleColoring.LoadBackDry(presentation.BackgroundColor);
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
                    int presentationUpperBorderLeft = 0;
                    int presentationUpperBorderTop = 0;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;

                    // Make a border
                    var frame = new BoxFrame()
                    {
                        Text = $"{(!kiosk ? $"[{i + 1}/{pages.Count}] - " : "")}{page.Name} - {presentation.Name}",
                        Left = presentationUpperBorderLeft,
                        Top = presentationUpperBorderTop,
                        Width = presentationLowerInnerBorderLeft,
                        Height = presentationLowerInnerBorderTop,
                        Settings = presentation.BorderSettings,
                        FrameColor = presentation.FrameColor,
                        BackgroundColor = presentation.BackgroundColor
                    };
                    var box = new Box()
                    {
                        Left = presentationUpperInnerBorderLeft,
                        Top = presentationUpperInnerBorderTop,
                        Width = presentationLowerInnerBorderLeft,
                        Height = presentationLowerInnerBorderTop,
                        Color = presentation.BackgroundColor
                    };
                    builder.Append(
                        frame.Render() +
                        box.Render()
                    );

                    // Write the bindings
                    var keybindingsRenderable = new Keybindings()
                    {
                        KeybindingList = !kiosk && !required ? NonKioskBindings : Bindings,
                        BackgroundColor = presentation.BackgroundColor,
                        Width = ConsoleWrapper.WindowWidth - 1,
                    };
                    builder.Append(RendererTools.RenderRenderable(keybindingsRenderable, new(0, ConsoleWrapper.WindowHeight - 1)));

                    // Clear the presentation screen
                    for (int y = presentationUpperInnerBorderTop; y <= presentationLowerInnerBorderTop; y++)
                        builder.Append(TextWriterWhereColor.RenderWhereColorBack(new string(' ', presentationLowerInnerBorderLeft), presentationUpperInnerBorderLeft, y, presentation.FrameColor, presentation.BackgroundColor));

                    // Generate the final string
                    return builder.ToString();
                });

                // All the elements will be dynamically rendered. Make a presentation grid buffer and the final element rendered string builder.
                var gridBuffer = new ScreenPart();

                // Then, the text
                string rendered = "";
                int currIdx = 0;
                int increment = 0;
                gridBuffer.AddDynamicText(() =>
                {
                    // Render all elements
                    var renderedElements = new StringBuilder();
                    var pageElements = page.Elements;
                    for (int elementIdx = 0; elementIdx < pageElements.Length; elementIdx++)
                    {
                        IElement? element = pageElements[elementIdx];
                        renderedElements.Append(element.RenderToString());
                        if (elementIdx < pageElements.Length - 1)
                            renderedElements.Append("\n\n");
                    }
                    string rendered = renderedElements.ToString();

                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = GetFinalLines(rendered);

                    // Populate some variables
                    int presentationUpperBorderLeft = 0;
                    int presentationUpperBorderTop = 0;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;
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
                    int left = presentationLowerInnerBorderLeft + 1;
                    if (splitFinalLines.Length > presentationLowerInnerBorderTop)
                    {
                        var dataSlider = new Slider((int)((double)currIdx / (splitFinalLines.Length - presentationLowerInnerBorderTop) * splitFinalLines.Length), 0, splitFinalLines.Length)
                        {
                            Vertical = true,
                            Height = presentationLowerInnerBorderTop - 2,
                            SliderActiveForegroundColor = presentation.FrameColor,
                            SliderForegroundColor = TransformationTools.GetDarkBackground(presentation.FrameColor),
                            SliderBackgroundColor = presentation.BackgroundColor,
                            SliderVerticalActiveTrackChar = presentation.BorderSettings.BorderRightFrameChar,
                            SliderVerticalInactiveTrackChar = presentation.BorderSettings.BorderRightFrameChar,
                        };
                        boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▲", left, 1, presentation.FrameColor, presentation.BackgroundColor));
                        boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▼", left, presentationLowerInnerBorderTop, presentation.FrameColor, presentation.BackgroundColor));
                        boxBuffer.Append(RendererTools.RenderRenderable(dataSlider, new(left, 2)));
                    }
                    return boxBuffer.ToString();
                });
                screen.AddBufferedPart($"Grid view for {presentation.Name}", gridBuffer);

                // Wait for the ENTER key to be pressed if in kiosk mode. If not in kiosk mode, handle any key
                bool pageExit = false;
                while (!pageExit)
                {
                    // Get the keypress or mouse press
                    screen.CycleFrequency = page.CycleFrequency;
                    ScreenTools.StopCyclicScreen();
                    if (screen.CycleFrequency > 0)
                        ScreenTools.StartCyclicScreen(screen);
                    ScreenTools.Render();
                    InputEventInfo data = Input.ReadPointerOrKey();

                    // Get the lines and the positions
                    string[] splitFinalLines = GetFinalLines(rendered);
                    int presentationUpperBorderLeft = 0;
                    int presentationUpperBorderTop = 0;
                    int presentationUpperInnerBorderLeft = presentationUpperBorderLeft + 1;
                    int presentationUpperInnerBorderTop = presentationUpperBorderTop + 1;
                    int presentationLowerInnerBorderLeft = ConsoleWrapper.WindowWidth - presentationUpperInnerBorderLeft * 2;
                    int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;
                    int arrowLeft = presentationLowerInnerBorderLeft + 1;
                    int arrowTop = 1;
                    int arrowBottom = presentationLowerInnerBorderTop;

                    // Then, determine if the pointer or the keypress is available
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Some hitboxes
                        var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), (_) => GoUp(ref currIdx)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), (_) => GoDown(rendered, ref currIdx)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var scrollUpHitbox = new PointerHitbox(new(presentationUpperInnerBorderLeft, presentationUpperInnerBorderTop), new Coordinate(presentationLowerInnerBorderLeft, presentationLowerInnerBorderTop), (_) => GoUp(ref currIdx, 3)) { Button = PointerButton.WheelUp, ButtonPress = PointerButtonPress.Scrolled };
                        var scrollDownHitbox = new PointerHitbox(new(presentationUpperInnerBorderLeft, presentationUpperInnerBorderTop), new Coordinate(presentationLowerInnerBorderLeft, presentationLowerInnerBorderTop), (_) => GoDown(rendered, ref currIdx, 3)) { Button = PointerButton.WheelDown, ButtonPress = PointerButtonPress.Scrolled };
                        var inputHitbox = new PointerHitbox(new(presentationUpperInnerBorderLeft, presentationUpperInnerBorderTop), new Coordinate(presentationLowerInnerBorderLeft, presentationLowerInnerBorderTop), (_) => ProcessInput(page, screen)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                if (!done)
                                    arrowDownHitbox.ProcessPointer(mouse, out done);
                                if (!done)
                                    inputHitbox.ProcessPointer(mouse, out _);
                                screen.RequireRefresh();
                                break;
                            case PointerButton.WheelUp:
                                scrollUpHitbox.ProcessPointer(mouse, out done);
                                break;
                            case PointerButton.WheelDown:
                                scrollDownHitbox.ProcessPointer(mouse, out done);
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        // Get the key
                        switch (cki.Key)
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
                                GoUp(ref currIdx, presentationLowerInnerBorderTop * 2 - 1);
                                break;
                            case ConsoleKey.PageDown:
                                GoDown(rendered, ref currIdx, increment);
                                break;
                            case ConsoleKey.UpArrow:
                                GoUp(ref currIdx);
                                break;
                            case ConsoleKey.DownArrow:
                                GoDown(rendered, ref currIdx);
                                break;
                            case ConsoleKey.Home:
                                GoFirst(ref currIdx);
                                break;
                            case ConsoleKey.End:
                                GoLast(rendered, ref currIdx);
                                break;
                        }
                    }
                }
                ScreenTools.StopCyclicScreen();
                screen.RemoveBufferedPart(gridBuffer.Id);
                screen.RemoveBufferedPart(buffer.Id);
            }

            // Clean up after ourselves
            ScreenTools.UnsetCurrent(screen);
            ConsoleColoring.LoadBack();
            ConsoleWrapper.CursorVisible = true;
        }

        private static string[] GetFinalLines(string rendered)
        {
            // Deal with the lines to actually fit text in the infobox
            int presentationUpperBorderLeft = 0;
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

        private static void GoUp(ref int currIdx, int level = 1)
        {
            currIdx -= level;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void GoDown(string rendered, ref int currIdx, int level = 1)
        {
            int presentationUpperBorderTop = 0;
            int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;
            string[] splitFinalLines = GetFinalLines(rendered);
            currIdx += level;
            if (currIdx > splitFinalLines.Length - presentationLowerInnerBorderTop)
                currIdx = splitFinalLines.Length - presentationLowerInnerBorderTop;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void GoFirst(ref int currIdx) =>
            currIdx = 0;

        private static void GoLast(string rendered, ref int currIdx)
        {
            int presentationUpperBorderTop = 0;
            int presentationLowerInnerBorderTop = ConsoleWrapper.WindowHeight - presentationUpperBorderTop * 2 - 3;
            string[] splitFinalLines = GetFinalLines(rendered);
            currIdx = splitFinalLines.Length - presentationLowerInnerBorderTop;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static bool ProcessInput(PresentationPage page, Screen screen)
        {
            if (page.Inputs.Length > 0)
            {
                // Make a selection infobox that lets the user select the inputs
                bool inputBail = false;
                while (!inputBail)
                {
                    // Populate the choices
                    var modules = page.Inputs.Select((pii) => pii.InputMethod).ToArray();

                    // Let the user select an option, then process the input
                    inputBail = InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, LanguageTools.GetLocalized("T_INPUT_PRESENTATION_NEEDSINPUTPROMPT"), new InfoBoxSettings()
                    {
                        Title = LanguageTools.GetLocalized("T_INPUT_PRESENTATION_NEEDSINPUTPROMPTTITLE"),
                    });
                    if (inputBail)
                    {
                        // Check the required inputs if they have been filled
                        var requiredInputs = page.Inputs.Where((ii) => ii.InputRequired).ToArray();
                        var filledRequiredInputs = requiredInputs.Where((ii) => ii.InputMethod.Provided).ToArray();
                        inputBail = filledRequiredInputs.Length == requiredInputs.Length;
                        if (inputBail)
                        {
                            // All required inputs have been provided. Submit the inputs to the presentation page after processing them.
                            var processedRequiredInputs = filledRequiredInputs.Where((ii) => ii.ProcessFunction(ii.InputMethod.Value)).ToArray();
                            inputBail = processedRequiredInputs.Length == filledRequiredInputs.Length;
                            if (!inputBail)
                                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_PRESENTATION_INPUTFILLEDINCORRECTLY") + $"\n\n  - {string.Join("\n  - ", filledRequiredInputs.Except(processedRequiredInputs).Select((ii) => ii.InputName).ToArray())}", new InfoBoxSettings()
                                {
                                    Title = LanguageTools.GetLocalized("T_INPUT_PRESENTATION_INPUTFILLEDINCORRECTLYTITLE"),
                                });
                        }
                        else
                        {
                            InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_PRESENTATION_INPUTNOTPROVIDED") + $"\n\n  - {string.Join("\n  - ", requiredInputs.Except(filledRequiredInputs).Select((ii) => ii.InputName).ToArray())}", new InfoBoxSettings()
                            {
                                Title = LanguageTools.GetLocalized("T_INPUT_PRESENTATION_INPUTNOTPROVIDEDTITLE"),
                            });
                        }
                    }
                }
                return inputBail;
            }
            else
                return true;
        }
    }
}
