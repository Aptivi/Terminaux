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
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Images.Icons
{
    /// <summary>
    /// Icons selector
    /// </summary>
    public static class IconsSelector
    {
        private readonly static Keybinding[] bindings =
        [
            new("Previous", ConsoleKey.LeftArrow),
            new("Next", ConsoleKey.RightArrow),
            new("Submit", ConsoleKey.Enter),
            new("Cancel", ConsoleKey.Escape),
            new("Help", ConsoleKey.H),
        ];
        private readonly static Keybinding[] additionalBindings =
        [
            new("Select", ConsoleKey.S),
            new("Manual Select", ConsoleKey.S, ConsoleModifiers.Shift),
        ];

        /// <summary>
        /// Prompts the user for an icon
        /// </summary>
        /// <returns>Selected icon</returns>
        public static string PromptForIcons() =>
            PromptForIcons("heart-suit");

        /// <summary>
        /// Prompts the user for an icon
        /// </summary>
        /// <param name="icon">Initial icon to select</param>
        /// <returns>Selected icon</returns>
        public static string PromptForIcons(string icon)
        {
            // Some initial variables to populate icons
            string[] icons = IconsManager.GetIconNames();
            var iconsSelections = InputChoiceTools.GetInputChoices(icons.Select((icon, num) => ($"{num}", icon)).ToArray()).ToArray();
            string iconName = icons.Contains(icon) ? icon : "heart-suit";
            ConsoleLogger.Debug("Got {0} selections with {1} as default", iconsSelections.Length, iconName);

            // Determine the icon index
            int selectedIcon;
            for (selectedIcon = 0; selectedIcon < icons.Length; selectedIcon++)
            {
                string queriedIcon = icons[selectedIcon];
                if (queriedIcon == iconName)
                {
                    ConsoleLogger.Debug("Found {0} at index {1}", queriedIcon, selectedIcon);
                    break;
                }
            }

            // Now, clear the console and let the user select an icon
            bool cancel = false;
            var screen = new Screen();
            try
            {
                bool bail = false;

                // Make a buffer that represents the TUI
                var screenPart = new ScreenPart();
                screenPart.AddDynamicText(() =>
                {
                    var buffer = new StringBuilder();

                    // Write the text using the selected icon
                    int height = ConsoleWrapper.WindowHeight - 8;
                    int width = height * 2;
                    int left = ConsoleWrapper.WindowWidth / 2 - height;
                    int top = ConsoleWrapper.WindowHeight / 2 - height / 2;
                    ConsoleLogger.Debug("Rendering {0} [W: {1}, H: {2}, T: {3}, B: {4}]", iconName, width, height, left, top);
                    buffer.Append(IconsManager.RenderIcon(iconName, width, height, left, top));

                    // Write the selected icon name and the keybindings
                    var iconAlignedText = new AlignedText()
                    {
                        Text = $"{iconName} - [{selectedIcon + 1}/{icons.Length}]",
                        Settings = new() { Alignment = TextAlignment.Middle },
                        Top = 1
                    };
                    var iconKeybindings = new Keybindings()
                    {
                        KeybindingList = bindings,
                        Width = ConsoleWrapper.WindowWidth - 1,
                        WriteHelpKeyInfo = false,
                    };
                    buffer.Append(
                        iconAlignedText.Render() +
                        RendererTools.RenderRenderable(iconKeybindings, new(0, ConsoleWrapper.WindowHeight - 1))
                    );
                    return buffer.ToString();
                });

                // Now, make the interactive TUI resizable.
                screen.AddBufferedPart("Icons selector", screenPart);
                ScreenTools.SetCurrent(screen);
                while (!bail)
                {
                    // Render
                    ScreenTools.Render();

                    // Wait for input
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                selectedIcon--;
                                if (selectedIcon < 0)
                                    selectedIcon = icons.Length - 1;
                                iconName = icons[selectedIcon];
                                screen.RequireRefresh();
                                break;
                            case PointerButton.WheelDown:
                                selectedIcon++;
                                if (selectedIcon > icons.Length - 1)
                                    selectedIcon = 0;
                                iconName = icons[selectedIcon];
                                screen.RequireRefresh();
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                            case ConsoleKey.LeftArrow:
                                selectedIcon--;
                                if (selectedIcon < 0)
                                    selectedIcon = icons.Length - 1;
                                iconName = icons[selectedIcon];
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.RightArrow:
                                selectedIcon++;
                                if (selectedIcon > icons.Length - 1)
                                    selectedIcon = 0;
                                iconName = icons[selectedIcon];
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.S:
                                bool write = cki.Modifiers.HasFlag(ConsoleModifiers.Shift);
                                if (write)
                                {
                                    string promptedIconName = InfoBoxInputColor.WriteInfoBoxInput("Write the icon name. It'll be converted to lowercase.").ToLower();
                                    if (!icons.Contains(promptedIconName))
                                        InfoBoxModalColor.WriteInfoBoxModal("The icon doesn't exist.");
                                    else
                                        iconName = promptedIconName;
                                }
                                else
                                {
                                    selectedIcon = InfoBoxSelectionColor.WriteInfoBoxSelection(iconsSelections, "Select an icon from the list below", new InfoBoxSettings()
                                    {
                                        Title = "Icon selection",
                                    });
                                    iconName = icons[selectedIcon];
                                }
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.H:
                                Keybinding[] allBindings = [.. bindings, .. additionalBindings];
                                KeybindingTools.ShowKeybindingInfobox(allBindings);
                                screen.RequireRefresh();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal("Icons selector failed: " + ex.Message);
            }
            finally
            {
                ScreenTools.UnsetCurrent(screen);
                ColorTools.LoadBack();
            }
            return cancel ? icon : iconName;
        }

        static IconsSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
