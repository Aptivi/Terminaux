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

using Textify.Data.Figlet;
using System;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Inputs.Interactive.Selectors
{
    /// <summary>
    /// Figlet selector
    /// </summary>
    public class FigletSelectorTui : TextualUI
    {
        private string font = "small";
        private int screenNum = 0;
        private int[] chars = Enumerable.Range(65, 90 - 65 + 1)
            .Union(Enumerable.Range(97, 122 - 97 + 1))
            .Union(Enumerable.Range(48, 57 - 48 + 1))
            .ToArray();
        private int selectedFont = 0;
        private int showcaseIndex = 0;
        private bool cancel = false;

        /// <inheritdoc/>
        public override string Render()
        {
            // Some initial variables to populate figlet fonts
            font = FigletSelector.fonts.Contains(font) ? font : "small";
            string text = "Test";

            // Determine the font index
            selectedFont = FigletSelector.DetermineFigletIndex(font);

            if (screenNum == 0)
            {
                var buffer = new StringBuilder();

                // Write the text using the selected figlet font
                var figletFont = FigletTools.GetFigletFont(font);
                int figletHeight = FigletTools.GetFigletHeight(text, figletFont);
                var figletDisplay = new AlignedFigletText(figletFont, text)
                {
                    Top = ConsoleWrapper.WindowHeight / 2 - figletHeight / 2,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                };
                buffer.Append(figletDisplay.Render());

                // Write the selected font name and the keybindings
                var figletInfo = new AlignedText($"{font} - [[{selectedFont + 1}/{FigletSelector.fonts.Length}]]")
                {
                    Top = 1,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    }
                };
                var figletKeybindings = new Keybindings()
                {
                    Width = ConsoleWrapper.WindowWidth - 1,
                    KeybindingList = FigletSelector.Bindings,
                    WriteHelpKeyInfo = false,
                };
                buffer.Append(figletInfo.Render());
                buffer.Append(RendererTools.RenderRenderable(figletKeybindings, new(0, ConsoleWrapper.WindowHeight - 1)));
                return buffer.ToString();
            }
            else if (screenNum == 1)
            {
                var buffer = new StringBuilder();

                // Write the text using the selected figlet font
                string character = ((char)chars[showcaseIndex]).ToString();
                var figletFont = FigletTools.GetFigletFont(font);
                var figletDisplay = new AlignedFigletText(figletFont, character)
                {
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    }
                };
                buffer.Append(figletDisplay.Render());

                // Write the selected character name and the keybindings
                var figletInfo = new AlignedText($"{character} - [[{showcaseIndex + 1}/{chars.Length}]]")
                {
                    Top = 1,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    }
                };
                var figletKeybindings = new Keybindings()
                {
                    Width = ConsoleWrapper.WindowWidth - 1,
                    KeybindingList = FigletSelector.CharSelectBindings,
                };
                buffer.Append(figletInfo.Render());
                buffer.Append(RendererTools.RenderRenderable(figletKeybindings, new(0, ConsoleWrapper.WindowHeight - 1)));
                return buffer.ToString();
            }
            return "";
        }

        internal string GetResultingFigletName()
        {
            string[] fonts = [.. FigletTools.GetFigletFonts().Keys];
            return fonts.Contains(font) && !cancel ? font : "small";
        }

        private void UpdateKeybindings(int screenNum)
        {
            ResetKeybindings();
            if (screenNum == 0)
            {
                // Keyboard bindings
                Keybindings.Add((FigletSelector.Bindings[0], Previous));
                Keybindings.Add((FigletSelector.Bindings[1], Next));
                Keybindings.Add((FigletSelector.Bindings[2], (ui, _, _) => Exit(ui, false)));
                Keybindings.Add((FigletSelector.Bindings[3], (ui, _, _) => Exit(ui, true)));
                Keybindings.Add((FigletSelector.AdditionalBindings[0], (ui, _, _) => Select(ui, false)));
                Keybindings.Add((FigletSelector.AdditionalBindings[1], (ui, _, _) => Select(ui, true)));
                Keybindings.Add((FigletSelector.AdditionalBindings[2], Showcase));

                // Mouse bindings
                Keybindings.Add((FigletSelector.AdditionalBindings[3], Previous));
                Keybindings.Add((FigletSelector.AdditionalBindings[4], Next));
            }
            else
            {
                Keybindings.Add((FigletSelector.CharSelectBindings[0], Previous));
                Keybindings.Add((FigletSelector.CharSelectBindings[1], Next));
                Keybindings.Add((FigletSelector.CharSelectBindings[2], GoBack));
            }
        }

        private void Previous(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (screenNum == 0)
            {
                selectedFont--;
                if (selectedFont < 0)
                    selectedFont = FigletSelector.fonts.Length - 1;
                font = FigletSelector.fonts[selectedFont];
            }
            else
            {
                showcaseIndex--;
                if (showcaseIndex < 0)
                    showcaseIndex = chars.Length - 1;
            }
            ui.RequireRefresh();
        }

        private void Next(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (screenNum == 0)
            {
                selectedFont++;
                if (selectedFont > FigletSelector.fonts.Length - 1)
                    selectedFont = 0;
                font = FigletSelector.fonts[selectedFont];
            }
            else
            {
                showcaseIndex++;
                if (showcaseIndex > chars.Length - 1)
                    showcaseIndex = 0;
            }
            ui.RequireRefresh();
        }

        private void Exit(TextualUI ui, bool cancel)
        {
            this.cancel = cancel;
            TextualUITools.ExitTui(ui);
        }

        private void Select(TextualUI ui, bool write)
        {
            if (write)
            {
                string promptedfont = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_IS_FIGLET_FONTNAMEPROMPT")).ToLower();
                if (!FigletSelector.fonts.Contains(promptedfont))
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_IS_FIGLET_NOFONT"));
                    return;
                }
                else
                    font = promptedfont;
            }
            else
            {
                InputChoiceInfo[] figletSelections = [.. InputChoiceTools.GetInputChoices(FigletSelector.fonts.Select((font, num) => ($"{num}", font)).ToArray())];
                selectedFont = InfoBoxSelectionColor.WriteInfoBoxSelection(figletSelections, LanguageTools.GetLocalized("T_INPUT_IS_FIGLET_FONTSELECTPROMPT"), new InfoBoxSettings()
                {
                    Title = LanguageTools.GetLocalized("T_INPUT_IS_FIGLET_FONTSELECTPROMPTTITLE")
                });
                font = FigletSelector.fonts[selectedFont];
            }
            selectedFont = FigletSelector.DetermineFigletIndex(font);
            ui.RequireRefresh();
        }

        private void Showcase(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            UpdateKeybindings(++screenNum);
            ui.RequireRefresh();
        }

        private void GoBack(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            UpdateKeybindings(--screenNum);
            ui.RequireRefresh();
        }

        internal FigletSelectorTui(string font)
        {
            this.font = font;
            UpdateKeybindings(0);
        }
    }
}
