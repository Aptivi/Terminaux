﻿//
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
            new GoRight()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
                    new ConsoleKeyInfo('\u0006', ConsoleKey.F, false, false, true)
                ]
            },
            new GoLeft()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
                    new ConsoleKeyInfo('\u0002', ConsoleKey.B, false, false, true)
                ]
            },
            new Home()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false),
                    new ConsoleKeyInfo('\u0001', ConsoleKey.A, false, false, true)
                ]
            },
            new End()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false),
                    new ConsoleKeyInfo('\u0005', ConsoleKey.E, false, false, true)
                ]
            },
            new Rubout()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false),
                    new ConsoleKeyInfo('\u007f', ConsoleKey.Backspace, false, false, false),
                    new ConsoleKeyInfo('\0', ConsoleKey.Backspace, false, false, false),
                    new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, true),
                ]
            },
            new Return()
            {
                BoundKeys =
                [
                    // for Windows
                    new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false),
                    new ConsoleKeyInfo('\n', ConsoleKey.J, false, false, true),

                    // for Linux
                    new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false),
                ]
            },
            new ReturnNothing()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\x03', ConsoleKey.C, false, false, true),
                ]
            },
            new PreviousHistory()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false)
                ]
            },
            new NextHistory()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false)
                ]
            },
            new FirstHistory()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('<', ConsoleKey.OemComma, true, true, false),
                    new ConsoleKeyInfo('<', 0, false, false, false),
                ]
            },
            new LastHistory()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('>', ConsoleKey.OemPeriod, true, true, false),
                    new ConsoleKeyInfo('>', 0, false, false, false),
                ]
            },
            new Delete()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false)
                ]
            },
            new BackwardOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('b', ConsoleKey.B, false, true, false)
                ]
            },
            new ForwardOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('f', ConsoleKey.F, false, true, false)
                ]
            },
            new NextSuggestion()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\t', ConsoleKey.Tab, false, false, false)
                ]
            },
            new PreviousSuggestion()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\t', ConsoleKey.Tab, true, false, false)
                ]
            },
            new CutToStart()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\u0015', ConsoleKey.U, false, false, true)
                ]
            },
            new CutToEnd()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\u000B', ConsoleKey.K, false, false, true)
                ]
            },
            new CutBackwardOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\u0017', ConsoleKey.W, false, false, true)
                ]
            },
            new CutForwardOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('d', ConsoleKey.D, false, true, false),
                    new ConsoleKeyInfo('\xE4', ConsoleKey.D, false, false, false),
                ]
            },
            new Yank()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\u0019', ConsoleKey.Y, false, false, true)
                ]
            },
            new UppercaseOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('u', ConsoleKey.U, false, true, false)
                ]
            },
            new LowercaseOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('l', ConsoleKey.L, false, true, false)
                ]
            },
            new UppercaseAll()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.U, false, true, true)
                ]
            },
            new LowercaseAll()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.L, false, true, true)
                ]
            },
            new UpAndForwardOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('c', ConsoleKey.C, false, true, false)
                ]
            },
            new LowAndForwardOneWord()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('v', ConsoleKey.V, false, true, false)
                ]
            },
            new ShowSuggestions()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('s', ConsoleKey.S, false, true, false),
                ]
            },
            new Refresh()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('r', ConsoleKey.R, false, true, false),
                ]
            },
            new InsertMode()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.Insert, false, false, false)
                ]
            },
            new RefreshClear()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\u000c', ConsoleKey.L, false, false, true),
                ]
            },
            new CutHorizontalLine()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\\', ConsoleKey.Oem5, false, true, false)
                ]
            },
            new SubstituteChars()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\u0014', ConsoleKey.T, false, false, true)
                ]
            },
            new SubstituteWords()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('t', ConsoleKey.T, false, true, false)
                ]
            },
            new Commentize()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('#', ConsoleKey.D3, true, true, false),
                ]
            },
            new Conceal()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('C', ConsoleKey.C, true, true, false),
                ]
            },
            new UndoChange()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo((char)0x1f, ConsoleKey.OemMinus, true, false, true),
                    new ConsoleKeyInfo('\0', ConsoleKey.D7, false, false, true),
                ]
            },
            new UndoChanges()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('r', ConsoleKey.R, false, true, false),
                ]
            },
            new ProcessArgDigit()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('0', ConsoleKey.D0, false, true, false),
                    new ConsoleKeyInfo('1', ConsoleKey.D1, false, true, false),
                    new ConsoleKeyInfo('2', ConsoleKey.D2, false, true, false),
                    new ConsoleKeyInfo('3', ConsoleKey.D3, false, true, false),
                    new ConsoleKeyInfo('4', ConsoleKey.D4, false, true, false),
                    new ConsoleKeyInfo('5', ConsoleKey.D5, false, true, false),
                    new ConsoleKeyInfo('6', ConsoleKey.D6, false, true, false),
                    new ConsoleKeyInfo('7', ConsoleKey.D7, false, true, false),
                    new ConsoleKeyInfo('8', ConsoleKey.D8, false, true, false),
                    new ConsoleKeyInfo('9', ConsoleKey.D9, false, true, false),
                    new ConsoleKeyInfo('-', ConsoleKey.OemMinus, false, true, false),
                    new ConsoleKeyInfo('-', ConsoleKey.Subtract, false, true, false),
                ]
            },

#if DEBUG
            new DebugPos()
            {
                BoundKeys =
                [
                    new ConsoleKeyInfo('\0', ConsoleKey.D, true, true, true)
                ]
            }
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
            {
                foreach (var baseKey in baseBinding.BoundKeys)
                    if (binding.BoundKeys.Contains(baseKey))
                        found = true;
                foreach (var baseKey in baseBinding.CustomKeys ?? [])
                    if (binding.CustomKeys?.Contains(baseKey) ?? false)
                        found = true;
            }

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

            // Unbind a custom binding
            foreach (var chosenBinding in chosenBindings)
                customBindings.Remove(chosenBinding);
        }

        /// <summary>
        /// Overrides the binding using a console key info instance
        /// </summary>
        /// <param name="sourceCki">Source console key info to search for</param>
        /// <param name="targetCki">Target console key info to override the base bindings defined by <see cref="BaseBinding.CustomKeys"/></param>
        public static void Override(ConsoleKeyInfo sourceCki, ConsoleKeyInfo targetCki)
        {
            // Check to see if the target key is already used
            var chosenBindings = AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(targetCki));
            if (chosenBindings.Any())
                return;

            // Override the binding
            foreach (var binding in AllBindings)
            {
                // Use the base bind matching to detect a binding
                if (!binding.BaseBindMatched(sourceCki))
                    continue;

                // Add this key to the override list
                if (binding.CustomKeys is not null && binding.IsBindingOverridable)
                    binding.CustomKeys.Add(targetCki);
            }
        }

        /// <summary>
        /// Removes the binding override using a console key info instance
        /// </summary>
        /// <param name="sourceCki">Source console key info to search for</param>
        /// <param name="targetCki">Target console key info to remove an override as defined in <see cref="BaseBinding.CustomKeys"/></param>
        public static void RemoveOverride(ConsoleKeyInfo sourceCki, ConsoleKeyInfo targetCki)
        {
            // Check to see if the target key is already used
            var chosenBindings = AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(targetCki));
            if (!chosenBindings.Any())
                return;

            // Override the binding
            foreach (var binding in AllBindings)
            {
                // Use the base bind matching to detect a binding
                if (!binding.BaseBindMatched(sourceCki))
                    continue;

                // Add this key to the override list
                if (binding.CustomKeys is not null && binding.IsBindingOverridable)
                    binding.CustomKeys.Remove(targetCki);
            }
        }

        internal static void Execute(TermReaderState state)
        {
            // Some variables
            string oldInput = state.CurrentText.ToString();
            bool addToUndo = false;

            // Get the chosen bindings
            var chosenBindings = AllBindings.Where((bindingInfo) => bindingInfo.BindMatched(state.pressedKey));
            bool clearArgumentNumber = false;

            // Execute the commands based on them
            foreach (var chosenBinding in chosenBindings)
            {
                addToUndo = chosenBinding.AppendsChangesList;
                clearArgumentNumber = chosenBinding is not ProcessArgDigit;
                int repetitions = chosenBinding.ArgumentNumberIsRepetition ? state.ArgumentNumber : 1;
                for (int i = 0; i < repetitions; i++)
                    chosenBinding.DoAction(state);
                if (chosenBinding.ResetSuggestionsTextPos)
                    state.currentSuggestionsTextPos = -1;
                ChangeChangesList(state, oldInput, state.CurrentText.ToString(), addToUndo);
            }

            // If there are no bindings, select the "print character" action
            if (!chosenBindings.Any())
            {
                addToUndo = true;
                clearArgumentNumber = true;
                for (int i = 0; i < state.ArgumentNumber; i++)
                    fallbackBinding.DoAction(state);
                state.currentSuggestionsTextPos = -1;
                ChangeChangesList(state, oldInput, state.CurrentText.ToString(), addToUndo);
            }

            // Clear the argument number if needed
            if (clearArgumentNumber)
                state.argNumbers.Clear();
        }

        private static void ChangeChangesList(TermReaderState state, string oldInput, string newInput, bool add)
        {
            // Determine whether this execution results in a change of input and add this change to the undo list,
            // if necessary.
            if (newInput != oldInput && add)
                state.changes.Add(newInput);
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
