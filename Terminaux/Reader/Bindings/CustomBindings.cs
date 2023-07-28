/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

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
