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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Tools for the textual UI
    /// </summary>
    public static class TextualUITools
    {
        /// <summary>
        /// Runs the textual user interface
        /// </summary>
        /// <param name="ui">Textual UI to run</param>
        public static void RunTui(TextualUI? ui)
        {
            // Run the validation
            ui = ValidateTui(ui);
            var uiPart = new ScreenPart();
            try
            {
                // Set the screen state
                uiPart.AddDynamicText(() =>
                {
                    var renderBuilder = new StringBuilder();
                    string rendered = ui.Render();
                    renderBuilder.Append(rendered);
                    foreach (var container in ui.Renderables)
                        renderBuilder.Append(ContainerTools.RenderContainer(container));
                    return renderBuilder.ToString();
                });
                ui.uiScreen.AddBufferedPart($"Textual UI - {ui.Name} - {ui.Guid}", uiPart);
                ScreenTools.SetCurrent(ui.uiScreen);

                // Run the UI loop
                while (ui.State != TextualUIState.Bailing)
                {
                    // Render the UI
                    ui.state = TextualUIState.Rendering;
                    ScreenTools.Render();

                    // Now, wait for user input
                    ui.state = TextualUIState.Waiting;
                    Stopwatch sw = new();
                    if (ui.RefreshDelay > 0)
                        sw.Start();
                    bool timedOut = false;
                    SpinWait.SpinUntil(() =>
                    {
                        timedOut = ui.RefreshDelay > 0 && sw.ElapsedMilliseconds >= ui.RefreshDelay;
                        return Input.InputAvailable || timedOut;
                    });
                    if (timedOut)
                        continue;

                    // Process the user input
                    ui.state = TextualUIState.Busy;
                    List<(Keybinding binding, Action<TextualUI, ConsoleKeyInfo, PointerEventContext?> action)> bindings = [];
                    PointerEventContext? mouse = null;
                    ConsoleKeyInfo key = default;
                    if (Input.MouseInputAvailable)
                    {
                        // Mouse input has been received
                        mouse = Input.ReadPointer();
                        if (mouse is null)
                            continue;

                        // Match the mouse binding
                        bindings = MatchBindings(ui, default, mouse);
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        // Keyboard input received
                        key = Input.ReadKey();

                        // Match the key binding
                        bindings = MatchBindings(ui, key, null);
                    }

                    // Execute the action according to the bindings
                    foreach (var binding in bindings)
                        binding.action.Invoke(ui, key, mouse);
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModalColor($"The textual UI has crashed. It's usually a problem in the application end.\n\n{ex}", ConsoleColors.Red);
            }
            finally
            {
                // Bailed.
                ScreenTools.UnsetCurrent(ui.uiScreen);
                ColorTools.LoadBack();
                ui.state = TextualUIState.Ready;
            }
        }

        /// <summary>
        /// Exits the textual UI
        /// </summary>
        /// <param name="ui">Textual UI</param>
        public static void ExitTui(TextualUI ui) =>
            ui.state = TextualUIState.Bailing;

        private static List<(Keybinding binding, Action<TextualUI, ConsoleKeyInfo, PointerEventContext?> action)> MatchBindings(TextualUI ui, ConsoleKeyInfo cki, PointerEventContext? mouseEvent)
        {
            if (mouseEvent is not null)
            {
                var mouseBindings = ui.Keybindings.FindAll((match) =>
                {
                    return
                        match.binding.BindingUsesMouse == true &&
                        match.binding.BindingPointerButton == mouseEvent.Button &&
                        match.binding.BindingPointerButtonPress == mouseEvent.ButtonPress &&
                        match.binding.BindingPointerModifiers == mouseEvent.Modifiers;
                });
                if (mouseBindings.Count == 0 && ui.Fallback is not null)
                    mouseBindings.Add((new("Dynamic", (PointerButton)0), ui.Fallback));
                return mouseBindings;
            }
            else
            {
                var keyboardBindings = ui.Keybindings.FindAll((match) =>
                {
                    return
                        match.binding.BindingUsesMouse == false &&
                        match.binding.BindingKeyName == cki.Key &&
                        match.binding.BindingKeyModifiers == cki.Modifiers;
                });
                if (keyboardBindings.Count == 0 && ui.Fallback is not null)
                    keyboardBindings.Add((new("Dynamic", (PointerButton)0), ui.Fallback));
                return keyboardBindings;
            }
        }

        private static TextualUI ValidateTui(TextualUI? ui)
        {
            // Sanity check
            if (ui is null)
                throw new TerminauxException("Textual UI is not specified");
            if (ui.State != TextualUIState.Ready)
                throw new TerminauxException($"This textual UI [{ui.Name}] is not ready because the state is {ui.State} - {ui.Guid}");

            // Check the keybindings
            foreach ((Keybinding binding, Delegate action) in ui.Keybindings)
            {
                var conflicts = ui.Keybindings.FindAll((match) =>
                {
                    return
                        match.binding.BindingKeyName == binding.BindingKeyName &&
                        match.binding.BindingKeyModifiers == binding.BindingKeyModifiers &&
                        match.binding.BindingUsesMouse == binding.BindingUsesMouse &&
                        match.binding.BindingPointerButton == binding.BindingPointerButton &&
                        match.binding.BindingPointerButtonPress == binding.BindingPointerButtonPress &&
                        match.binding.BindingPointerModifiers == binding.BindingPointerModifiers;
                });
                if (conflicts.Count > 1)
                    throw new TerminauxException($"There are {conflicts.Count - 1} conflicting key binding(s) in the [{ui.Name}] textual UI - {ui.Guid}");
            }

            // Return the UI
            return ui;
        }
    }
}
