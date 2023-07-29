
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;

namespace Terminaux.Reader.Bindings
{
    /// <summary>
    /// Custom binding handler
    /// </summary>
    public static class CustomBindings
    {
        /// <summary>
        /// Binds the key to the custom bindings list to be executed on press
        /// </summary>
        /// <param name="binding">Base containing information about key binding</param>
        public static void Bind(BaseBinding binding)
        {
            // If we have the key in the bound keys list (either built-in or custom), don't add the key to the list.
            bool found = false;
            foreach (var baseBinding in BindingsList.AllBindings)
                foreach (var baseKey in baseBinding.BoundKeys)
                    if (binding.BoundKeys.Contains(baseKey))
                        found = true;

            // Else, just add it.
            if (!found)
                BindingsList.customBindings.Add(binding);
        }

        /// <summary>
        /// Unbinds the key from the custom bindings list
        /// </summary>
        /// <param name="cki">Key information to remove the binding</param>
        public static void Unbind(ConsoleKeyInfo cki)
        {
            // Get the chosen bindings
            var chosenBindings = BindingsList.AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(cki));

            // Execute the commands based on them
            foreach (var chosenBinding in chosenBindings)
                BindingsList.customBindings.Remove(chosenBinding);
        }
    }
}
