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
using System.Collections.Generic;
using System.Linq;
using Terminaux.Reader.Bindings.BaseBindings;

namespace Terminaux.Reader.Bindings
{
    /// <summary>
    /// Terminal reader binding tools
    /// </summary>
    public static class BindingsTools
    {
        internal static List<BaseBinding> baseBindings =
        [
            new GoRight(),
            new GoLeft(),
            new Home(),
            new End(),
            new Rubout(),
            new Return(),
            new ReturnNothing(),
            new PreviousHistory(),
            new NextHistory(),
            new Delete(),
            new BackwardOneWord(),
            new ForwardOneWord(),
            new NextSuggestion(),
            new PreviousSuggestion(),
            new CutToStart(),
            new CutToEnd(),
            new CutBackwardOneWord(),
            new CutForwardOneWord(),
            new Yank(),
            new UppercaseOneWord(),
            new LowercaseOneWord(),
            new UpAndForwardOneWord(),
            new LowAndForwardOneWord(),
            new ShowSuggestions(),
            new Refresh(),
            new InsertMode(),
            new RefreshClear(),
            new CutHorizontalLine(),
            new SubstituteChars(),
            new SubstituteWords(),
            new Commentize(),
            new Conceal(),

#if DEBUG
            new DebugPos()
#endif
        ];

        internal static List<BaseBinding> customBindings = [];

        internal static List<BaseBinding> AllBindings =>
            [.. baseBindings, .. customBindings];

        internal static BaseBinding fallbackBinding = new InsertSelf();

        /// <summary>
        /// Binds the key to the custom bindings list to be executed on press
        /// </summary>
        /// <param name="binding">Base containing information about key binding</param>
        public static void Bind(BaseBinding binding)
        {
            // If we have the key in the bound keys list (either built-in or custom), don't add the key to the list.
            bool found = false;
            foreach (var baseBinding in AllBindings)
                foreach (var baseKey in baseBinding.BoundKeys)
                    if (binding.BoundKeys.Contains(baseKey))
                        found = true;

            // Else, just add it.
            if (!found)
                customBindings.Add(binding);
        }

        /// <summary>
        /// Unbinds the key from the custom bindings list
        /// </summary>
        /// <param name="cki">Key information to remove the binding</param>
        public static void Unbind(ConsoleKeyInfo cki)
        {
            // Get the chosen bindings
            var chosenBindings = AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(cki));

            // Execute the commands based on them
            foreach (var chosenBinding in chosenBindings)
                customBindings.Remove(chosenBinding);
        }

        internal static void Execute(TermReaderState state)
        {
            // Get the chosen bindings
            var chosenBindings = AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(state.pressedKey));

            // Execute the commands based on them
            foreach (var chosenBinding in chosenBindings)
            {
                chosenBinding.DoAction(state);
                if (chosenBinding.ResetSuggestionsTextPos)
                    state.currentSuggestionsTextPos = -1;
            }

            // If there are no bindings, select the "print character" action
            if (!chosenBindings.Any())
            {
                fallbackBinding.DoAction(state);
                state.currentSuggestionsTextPos = -1;
            }
        }

        internal static bool IsTerminate(ConsoleKeyInfo cki)
        {
            // Get the chosen bindings
            var chosenBindings = AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(cki));

            // Return exit value in one of the bindings
            foreach (var chosenBinding in chosenBindings)
                return chosenBinding.IsExit;

            return false;
        }
    }
}
