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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Inputs.Interactive.Selectors
{
    /// <summary>
    /// Cowsay selector
    /// </summary>
    public class CowsaySelectorTui : TextualUI
    {
        private CowName cowName = CowName.Default;
        private int selectedCowName = 0;
        private bool cancel = false;

        /// <inheritdoc/>
        public override string Render()
        {
            // Some initial variables to populate cows
            cowName = CowNameMapping.cowNameStrings.ContainsKey(cowName) ? cowName : CowName.Default;
            string text = "Hello world!";

            // Determine the cowName index
            selectedCowName = (int)cowName;
            var buffer = new StringBuilder();

            // Write the text using the selected cow
            var wrappedSentences = ConsoleMisc.GetWrappedSentences(text, ConsoleWrapper.WindowWidth);
            int cowsayWidth = CowsayTools.GetCowsayWidth(text, cowName);
            int cowsayWidthFallback = CowsayTools.GetCowsayWidth(text, CowName.Default);
            int cowsayWidthSmallText = wrappedSentences.Max(ConsoleChar.EstimateCellWidth);
            int cowsayHeight = CowsayTools.GetCowsayHeight(text, cowName);
            int cowsayHeightFallback = CowsayTools.GetCowsayHeight(text, CowName.Default);
            int cowsayHeightSmallText = wrappedSentences.Length;
            int finalTop = ConsoleWrapper.WindowHeight / 2 - cowsayHeight / 2;
            int finalTopFallback = ConsoleWrapper.WindowHeight / 2 - cowsayHeightFallback / 2;
            int finalTopFallbackSmallText = ConsoleWrapper.WindowHeight / 2 - cowsayHeightSmallText / 2;
            int finalLeft = ConsoleWrapper.WindowWidth / 2 - cowsayWidth / 2;
            int finalLeftFallback = ConsoleWrapper.WindowWidth / 2 - cowsayWidthFallback / 2;
            int finalLeftFallbackSmallText = ConsoleWrapper.WindowWidth / 2 - cowsayWidthSmallText / 2;
            int fallbackLevel =
                (finalTop > 0 && finalLeft > 0) ? 0 :
                (finalTopFallback > 0 && finalLeftFallback > 0) ? 1 :
                2;
            var cowsayDisplay = new AlignedCowsayText(cowName, text)
            {
                Top =
                    fallbackLevel == 0 ? finalTop :
                    fallbackLevel == 1 ? finalTopFallback :
                    finalTopFallbackSmallText,
                Left =
                    fallbackLevel == 0 ? finalLeft :
                    fallbackLevel == 1 ? finalLeftFallback :
                    finalLeftFallbackSmallText,
            };
            buffer.Append(cowsayDisplay.Render());

            // Write the selected cow name and the keybindings
            var cowsayInfo = new AlignedText($"{cowName} - [[{selectedCowName + 1}/{CowNameMapping.cowNameStrings.Count}]]")
            {
                Top = 1,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };
            var cowsayKeybindings = new Keybindings()
            {
                Width = ConsoleWrapper.WindowWidth - 1,
                KeybindingList = CowsaySelector.Bindings,
                WriteHelpKeyInfo = false,
            };
            buffer.Append(cowsayInfo.Render());
            buffer.Append(RendererTools.RenderRenderable(cowsayKeybindings, new(0, ConsoleWrapper.WindowHeight - 1)));
            return buffer.ToString();
        }

        internal CowName GetResultingCowsay() =>
            CowNameMapping.cowNameStrings.ContainsKey(cowName) && !cancel ? cowName : CowName.Default;

        private void Previous(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            selectedCowName--;
            if (selectedCowName < 0)
                selectedCowName = CowNameMapping.cowNameStrings.Count - 1;
            cowName = (CowName)selectedCowName;
            ui.RequireRefresh();
        }

        private void Next(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            selectedCowName++;
            if (selectedCowName > CowNameMapping.cowNameStrings.Count - 1)
                selectedCowName = 0;
            cowName = (CowName)selectedCowName;
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
                string promptedCowName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_IS_COWSAY_COWNAMENAMEPROMPT")).ToLower();
                if (!Enum.TryParse(promptedCowName, out CowName promptedCow))
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_IS_COWSAY_NOCOWNAME"));
                    return;
                }
                else
                    cowName = promptedCow;
            }
            else
            {
                InputChoiceInfo[] cowsaySelections = [.. InputChoiceTools.GetInputChoices(CowNameMapping.cowNameStrings.Values.Select((cowName, num) => ($"{num}", cowName)).ToArray())];
                cowName = (CowName)InfoBoxSelectionColor.WriteInfoBoxSelection(cowsaySelections, LanguageTools.GetLocalized("T_INPUT_IS_COWSAY_COWNAMESELECTPROMPT"), new InfoBoxSettings()
                {
                    Title = LanguageTools.GetLocalized("T_INPUT_IS_COWSAY_COWNAMESELECTPROMPTTITLE")
                });
                if (cowName < 0)
                    cowName = CowName.Default;
            }
            selectedCowName = (int)cowName;
            ui.RequireRefresh();
        }

        internal CowsaySelectorTui(CowName cowName)
        {
            this.cowName = cowName;
            ResetKeybindings();

            // Keyboard bindings
            Keybindings.Add((CowsaySelector.Bindings[0], Previous));
            Keybindings.Add((CowsaySelector.Bindings[1], Next));
            Keybindings.Add((CowsaySelector.Bindings[2], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((CowsaySelector.Bindings[3], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((CowsaySelector.AdditionalBindings[0], (ui, _, _) => Select(ui, false)));
            Keybindings.Add((CowsaySelector.AdditionalBindings[1], (ui, _, _) => Select(ui, true)));

            // Mouse bindings
            Keybindings.Add((CowsaySelector.AdditionalBindings[2], Previous));
            Keybindings.Add((CowsaySelector.AdditionalBindings[3], Next));
        }
    }
}
