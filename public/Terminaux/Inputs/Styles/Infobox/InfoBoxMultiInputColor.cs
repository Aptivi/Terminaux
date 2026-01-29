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
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Inputs.Pointer;
using Terminaux.Base.Extensions;
using System.Linq;
using Textify.Tools;
using System.Text.RegularExpressions;
using Selections = Terminaux.Writer.CyclicWriters.Graphical.Selection;
using System.Collections.Generic;
using Terminaux.Base.Structures;
using Textify.General;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with input and color support
    /// </summary>
    public static class InfoBoxMultiInputColor
    {
        private static Keybinding[] Keybindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_GOUP"), ConsoleKey.UpArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_GODOWN"), ConsoleKey.DownArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_GOFIRST"), ConsoleKey.Home),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_GOLAST"), ConsoleKey.End),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_PREVPAGEINPUTS"), ConsoleKey.PageUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_NEXTPAGEINPUTS"), ConsoleKey.PageDown),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), ConsoleKey.Tab),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_SEARCHINPUTS"), ConsoleKey.F),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEUP"), ConsoleKey.W),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEDOWN"), ConsoleKey.S),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVPAGETEXT"), ConsoleKey.E),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTPAGETEXT"), ConsoleKey.D),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_SUBMIT_PLURAL"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MULTIINPUT_KEYBINDING_CHANGEINPUTVALUE"), ConsoleKey.Spacebar),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_CANCEL_PLURAL"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PERFORMORSELECT"), PointerButton.Left),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), PointerButton.Right),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELUPCHOICE"), PointerButton.WheelUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELDOWNCHOICE"), PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the multi-input method info box
        /// </summary>
        /// <param name="modules">Input modules to represent their values.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static bool WriteInfoBoxMultiInput(InputModule[] modules, string text, params object[] vars) =>
            WriteInfoBoxMultiInput(modules, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the multi-input method info box
        /// </summary>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="modules">Input modules to represent their values.</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static bool WriteInfoBoxMultiInput(InputModule[] modules, string text, InfoBoxSettings settings, params object[] vars)
        {
            // Prepare the screen
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxMultiInputColor), infoBoxScreenPart);

            // Make a new infobox instance
            int selectionChoices = modules.Length > 10 ? 10 : modules.Length;
            int selectionReservedHeight = 2 + selectionChoices;
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = selectionReservedHeight;

            // Render it
            bool cancel = false;
            try
            {
                int maxModuleSelectWidth = modules.Max((im) => ConsoleChar.EstimateCellWidth($"  {im.Name}) "));
                int currentSelection = 0;
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Fill the info box with text inside it
                    infoBox.Elements.RemoveRenderables();
                    var (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;
                    int selectionBoxPosX = borderX + 2;
                    int selectionBoxPosY = borderY + maxTextHeight + 1;
                    int maxSelectionWidth = maxWidth - 4;

                    // Buffer the selection box
                    var border = new Border()
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY,
                        Width = maxSelectionWidth,
                        Height = selectionChoices,
                        Settings = settings.BorderSettings,
                        UseColors = settings.UseColors,
                        Color = settings.ForegroundColor,
                        BackgroundColor = settings.BackgroundColor,
                    };
                    infoBox.Elements.AddRenderable("Multi-input selection box", border);

                    // Prepare the selections
                    int maxModuleWidth = maxSelectionWidth - maxModuleSelectWidth;
                    List<InputChoiceInfo> choices = [];
                    foreach (var module in modules)
                    {
                        var moduleChoice = new InputChoiceInfo(module.Name, module.RenderInput(maxModuleWidth));
                        choices.Add(moduleChoice);
                    }

                    // Now, render the selections
                    InputChoiceInfo[] choicesArray = [.. choices];
                    var selectionsRendered = new Selections(choicesArray)
                    {
                        Left = selectionBoxPosX + 1,
                        Top = selectionBoxPosY + 1,
                        CurrentSelection = currentSelection,
                        Height = selectionChoices,
                        Width = maxSelectionWidth,
                        SwapSelectedColors = true,
                        Ellipsis = false,
                        UseColors = settings.UseColors,
                        Settings = new()
                        {
                            OptionColor = settings.ForegroundColor,
                            SelectedOptionColor = settings.ForegroundColor,
                            BackgroundColor = settings.BackgroundColor,
                        }
                    };
                    infoBox.Elements.AddRenderable("Rendered selections", selectionsRendered);
                    return infoBox.Render(ref increment, currIdx, true, true);
                });

                // Render the screen
                ScreenTools.Render();

                // Wait until the user presses any key to close the box
                bool bail = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Handle keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, linesLength) = infoBox.Dimensions;
                    int selectionBoxPosX = borderX + 2;
                    int selectionBoxPosY = borderY + maxTextHeight + 2;
                    int maxSelectionWidth = maxWidth - 4;
                    int arrowSelectLeft = selectionBoxPosX + maxWidth - 3;

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
                    var arrowSelectUpHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY), new Action<PointerEventContext>((_) => SelectionGoUp(ref currentSelection))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSelectDownHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY + selectionChoices - 1), new Action<PointerEventContext>((_) => SelectionGoDown(ref currentSelection, modules))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(Keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                    // Determine whether we need to use the popover
                    Coordinate popoverPos = default;
                    Size popoverSize = default;
                    void UpdatePopoverParameters()
                    {
                        if (settings.UsePopover)
                        {
                            // Prepare the selections
                            int maxModuleWidth = maxSelectionWidth - maxModuleSelectWidth;
                            List<InputChoiceInfo> choices = [];
                            InputModule chosenModule = modules[currentSelection];
                            foreach (var module in modules)
                            {
                                var moduleChoice = new InputChoiceInfo(module.Name, module.RenderInput(maxModuleWidth));
                                choices.Add(moduleChoice);
                            }

                            // Make a selection renderable to get the hitbox index and to get the popover parameters
                            InputChoiceInfo[] choicesArray = [.. choices];
                            var selectionsRendered = new Selections(choicesArray)
                            {
                                Left = selectionBoxPosX + 1,
                                Top = selectionBoxPosY + 1,
                                CurrentSelection = currentSelection,
                                Height = selectionChoices,
                                Width = maxSelectionWidth,
                                Ellipsis = false,
                            };
                            int hitboxIdx = selectionsRendered.GetHitboxIndex();
                            var hitbox = selectionsRendered.GenerateSelectionHitbox(hitboxIdx);
                            popoverPos = new(hitbox.hitbox.Start.X + 1 + maxModuleSelectWidth, hitbox.hitbox.Start.Y);
                            popoverSize = new(hitbox.hitbox.Size.Width - maxModuleSelectWidth - 2, chosenModule.ExtraPopoverHeight > 0 ? chosenModule.ExtraPopoverHeight : hitbox.hitbox.Size.Height);
                        }
                    }

                    // Handle input
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        bool UpdatePositionBasedOnMouse(PointerEventContext mouse)
                        {
                            // Make pages based on console window height
                            int currentPage = currentSelection / selectionChoices;
                            int startIndex = selectionChoices * currentPage;

                            // Now, translate coordinates to the selected index
                            if (!PointerTools.PointerWithinRange(mouse,
                                    (selectionBoxPosX + 1, selectionBoxPosY),
                                    (selectionBoxPosX + maxSelectionWidth, selectionBoxPosY + selectionChoices - 1)))
                                return false;
                            int listIndex = mouse.Coordinates.y - selectionBoxPosY;
                            listIndex = startIndex + listIndex;
                            if (listIndex >= modules.Length)
                                return false;
                            currentSelection = listIndex;
                            return true;
                        }

                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoUp(ref currIdx, 3);
                                else if (IsMouseWithinInputBox(infoBox, mouse))
                                    SelectionGoUp(ref currentSelection);
                                break;
                            case PointerButton.WheelDown:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoDown(ref currIdx, infoBox, 3);
                                else if (IsMouseWithinInputBox(infoBox, mouse))
                                    SelectionGoDown(ref currentSelection, modules);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && linesLength > maxHeight)
                                {
                                    arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowSelectUpHitbox.IsPointerWithin(mouse) || arrowSelectDownHitbox.IsPointerWithin(mouse)) && modules.Length > selectionChoices)
                                {
                                    arrowSelectUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowSelectDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if (infoboxButtonHelpHitbox.IsPointerWithin(mouse))
                                    infoboxButtonHelpHitbox.ProcessPointer(mouse, out _);
                                else if (infoboxButtonCloseHitbox.IsPointerWithin(mouse))
                                    infoboxButtonCloseHitbox.ProcessPointer(mouse, out _);
                                else if (UpdatePositionBasedOnMouse(mouse))
                                {
                                    if (!Input.EnableMovementEvents)
                                        ScreenTools.Render();
                                    UpdatePopoverParameters();
                                    modules[currentSelection].ProcessInput(popoverPos, popoverSize);
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (!UpdatePositionBasedOnMouse(mouse))
                                    break;
                                var selectedInstance = modules[currentSelection];
                                string name = selectedInstance.Name;
                                string desc = selectedInstance.Description;
                                if (!string.IsNullOrWhiteSpace(desc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(desc, new InfoBoxSettings()
                                    {
                                        Title = name,
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                UpdatePositionBasedOnMouse(mouse);
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        var selectedInstance = modules[currentSelection];
                        switch (cki.Key)
                        {
                            case ConsoleKey.UpArrow:
                                SelectionGoUp(ref currentSelection);
                                break;
                            case ConsoleKey.DownArrow:
                                SelectionGoDown(ref currentSelection, modules);
                                break;
                            case ConsoleKey.Home:
                                SelectionSet(ref currentSelection, modules, 0);
                                break;
                            case ConsoleKey.End:
                                SelectionSet(ref currentSelection, modules, modules.Length - 1);
                                break;
                            case ConsoleKey.PageUp:
                                {
                                    int currentPageMove = (currentSelection - 1) / selectionChoices;
                                    int startIndexMove = selectionChoices * currentPageMove;
                                    SelectionSet(ref currentSelection, modules, startIndexMove);
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    int currentPageMove = currentSelection / selectionChoices;
                                    int startIndexMove = selectionChoices * (currentPageMove + 1);
                                    SelectionSet(ref currentSelection, modules, startIndexMove);
                                }
                                break;
                            case ConsoleKey.Tab:
                                string name = selectedInstance.Name;
                                string desc = selectedInstance.Description;
                                if (!string.IsNullOrWhiteSpace(desc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(desc, new InfoBoxSettings()
                                    {
                                        Title = name,
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (selectionChoices <= 0)
                                    break;
                                var entriesString = modules.Select((entry) => (entry.Name, entry.Description)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_COMMON_SEARCHPROMPT"));
                                if (!RegexTools.IsValidRegex(keyword))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_INVALIDQUERY"));
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                    break;
                                }
                                var regex = new Regex(keyword);
                                var resultEntries = entriesString
                                    .Select((entry, idx) => (entry.Name, entry.Description, idx))
                                    .Where((entry) => regex.IsMatch(entry.Name) || regex.IsMatch(entry.Description)).ToArray();
                                if (resultEntries.Length > 1)
                                {
                                    var foundChoices = resultEntries.Select((tuple) => new InputChoiceInfo(tuple.Name, tuple.Description)).ToArray();
                                    int answer = InfoBoxSelectionColor.WriteInfoBoxSelection(foundChoices, LanguageTools.GetLocalized("T_INPUT_COMMON_ENTRYPROMPT"));
                                    if (answer < 0)
                                        break;
                                    currentSelection = resultEntries[answer].idx;
                                }
                                else if (resultEntries.Length == 1)
                                    currentSelection = resultEntries[0].idx;
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_NOITEMS"));
                                ScreenTools.CurrentScreen?.RequireRefresh();
                                break;
                            case ConsoleKey.E:
                                InfoBoxTools.GoUp(ref currIdx, maxHeight);
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
                            case ConsoleKey.Spacebar:
                                UpdatePopoverParameters();
                                selectedInstance.ProcessInput(popoverPos, popoverSize);
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
            return !cancel;
        }

        private static bool IsMouseWithinInputBox(InfoBox infoBox, PointerEventContext mouse)
        {
            var (maxWidth, _, _, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;
            int selectionBoxPosX = borderX + 2;
            int selectionBoxPosY = borderY + maxTextHeight + 2;
            int maxSelectionWidth = maxWidth - 4;

            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (selectionBoxPosX + 1, selectionBoxPosY), (selectionBoxPosX + maxSelectionWidth, selectionBoxPosY + infoBox.Positioning.ExtraHeight - 3));
        }

        private static void SelectionGoUp(ref int currentSelection)
        {
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = 0;
        }

        private static void SelectionGoDown(ref int currentSelection, InputModule[] modules)
        {
            currentSelection++;
            if (currentSelection > modules.Length - 1)
                currentSelection = modules.Length - 1;
        }

        private static void SelectionSet(ref int currentSelection, InputModule[] modules, int value)
        {
            currentSelection = value;
            if (currentSelection > modules.Length - 1)
                currentSelection = modules.Length - 1;
            if (currentSelection < 0)
                currentSelection = 0;
        }
    }
}
