﻿
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

namespace Terminaux.Reader.Inputs.Interactive
{
    /// <summary>
    /// Interactive TUI binding information class
    /// </summary>
    public class InteractiveTuiBinding
    {
        private readonly string _bindingName;
        private readonly ConsoleKey _bindingKeyName;
        private readonly Action<object, int> _bindingAction;

        /// <summary>
        /// Key binding name
        /// </summary>
        public string BindingName { get => _bindingName; }

        /// <summary>
        /// Which key is bound to the action?
        /// </summary>
        public ConsoleKey BindingKeyName { get => _bindingKeyName; }

        /// <summary>
        /// The action to execute.
        /// The integer argument denotes the currently selected data
        /// </summary>
        public Action<object, int> BindingAction { get => _bindingAction; }

        /// <summary>
        /// Makes a new instance of an interactive TUI key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        public InteractiveTuiBinding(string bindingName, ConsoleKey bindingKeyName, Action<object, int> bindingAction)
        {
            _bindingName = bindingName;
            _bindingKeyName = bindingKeyName;
            _bindingAction = bindingAction;
        }
    }
}
