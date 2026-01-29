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
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Base.Structures;
using Textify.General;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with color support
    /// </summary>
    public static class InfoBoxModalColor
    {
        private static Keybinding[] Keybindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEUP"), ConsoleKey.UpArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEDOWN"), ConsoleKey.DownArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MODAL_KEYBINDING_FIRSTLINE"), ConsoleKey.Home),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MODAL_KEYBINDING_LASTLINE"), ConsoleKey.End),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVPAGETEXT"), ConsoleKey.PageUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTPAGETEXT"), ConsoleKey.PageDown),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MODAL_KEYBINDING_DOWNORCLOSE"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MODAL_KEYBINDING_CLOSE"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MODAL_KEYBINDING_PERFORMACTION"), PointerButton.Left),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MODAL_KEYBINDING_WHEELUP"), PointerButton.WheelUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_MODAL_KEYBINDING_WHEELDOWN"), PointerButton.WheelDown),
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
        public static void WriteInfoBoxModal(string text, InfoBoxSettings settings, params object[] vars)
        {
            // Prepare the screen
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxModalColor), infoBoxScreenPart);

            // Make a new infobox instance
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = 0;

            // Render it
            try
            {
                // Draw the border and the text
                int currIdx = 0;
                int increment = 0;
                bool bail = false;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    return infoBox.Render(ref increment, currIdx, true, true);
                });

                // Main loop
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Wait until the user presses any key to close the box
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var (maxWidth, maxHeight, _, borderX, borderY, maxTextHeight, linesLength) = infoBox.Dimensions;

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
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(Keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
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
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && linesLength > maxHeight)
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
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoUp(ref currIdx, 3);
                                break;
                            case PointerButton.WheelDown:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoDown(ref currIdx, infoBox, 3);
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
                                InfoBoxTools.GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.PageDown:
                            case ConsoleKey.Enter:
                                bail = cki.Key == ConsoleKey.Enter && currIdx >= linesLength - maxHeight;
                                InfoBoxTools.GoDown(ref currIdx, infoBox, increment);
                                break;
                            case ConsoleKey.UpArrow:
                                InfoBoxTools.GoUp(ref currIdx);
                                break;
                            case ConsoleKey.DownArrow:
                                InfoBoxTools.GoDown(ref currIdx, infoBox);
                                break;
                            case ConsoleKey.Home:
                                currIdx = 0;
                                break;
                            case ConsoleKey.End:
                                currIdx = linesLength - maxHeight;
                                if (currIdx < 0)
                                    currIdx = 0;
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
        }
    }
}
