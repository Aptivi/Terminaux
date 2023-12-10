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
using System.Linq;

namespace Terminaux.Reader.Bindings
{
    internal static class BindingsReader
    {
        internal static void Execute(TermReaderState state)
        {
            // Get the chosen bindings
            var chosenBindings = BindingsList.AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(state.pressedKey));

            // Execute the commands based on them
            foreach (var chosenBinding in chosenBindings)
                chosenBinding.DoAction(state);

            // If there are no bindings, select the "print character" action
            if (!chosenBindings.Any())
                BindingsList.fallbackBinding.DoAction(state);
        }

        internal static bool IsTerminate(ConsoleKeyInfo cki)
        {
            // Get the chosen bindings
            var chosenBindings = BindingsList.AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(cki));

            // Return exit value in one of the bindings
            foreach (var chosenBinding in chosenBindings)
                return chosenBinding.IsExit;

            return false;
        }
    }
}
