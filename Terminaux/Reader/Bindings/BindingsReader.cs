﻿/*
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
