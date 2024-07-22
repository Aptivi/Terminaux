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
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.MiscWriters.Tools;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Interactive TUI binding information class
    /// </summary>
    public class InteractiveTuiBinding : Keybinding, IEquatable<InteractiveTuiBinding?>
    {
        private readonly bool _bindingCanRunWithoutItems;
        private readonly Action<object?, int>? _bindingAction;

        /// <summary>
        /// Whether the binding can run without items or not
        /// </summary>
        public bool BindingCanRunWithoutItems =>
            _bindingCanRunWithoutItems;

        /// <summary>
        /// The action to execute.
        /// The integer argument denotes the currently selected data
        /// </summary>
        public Action<object?, int>? BindingAction =>
            _bindingAction;

        /// <summary>
        /// Makes a new instance of an interactive TUI key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        /// <param name="canRunWithoutItems">Whether the binding can run without items or not</param>
        public InteractiveTuiBinding(string bindingName, ConsoleKey bindingKeyName, Action<object?, int>? bindingAction, bool canRunWithoutItems = false) :
            this(bindingName, bindingKeyName, default, bindingAction, canRunWithoutItems)
        { }

        /// <summary>
        /// Makes a new instance of an interactive TUI key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="bindingKeyModifiers">Which modifiers of the key is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        /// <param name="canRunWithoutItems">Whether the binding can run without items or not</param>
        public InteractiveTuiBinding(string bindingName, ConsoleKey bindingKeyName, ConsoleModifiers bindingKeyModifiers, Action<object?, int>? bindingAction, bool canRunWithoutItems = false) :
            base(bindingName, bindingKeyName, bindingKeyModifiers)
        {
            _bindingAction = bindingAction;
            _bindingCanRunWithoutItems = canRunWithoutItems;
        }

        /// <summary>
        /// Makes a new instance of an interactive TUI mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which key is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        /// <param name="canRunWithoutItems">Whether the binding can run without items or not</param>
        public InteractiveTuiBinding(string bindingName, PointerButton bindingPointerButton, Action<object?, int>? bindingAction, bool canRunWithoutItems = false) :
            this(bindingName, bindingPointerButton, PointerButtonPress.Moved, PointerModifiers.None, bindingAction, canRunWithoutItems)
        { }

        /// <summary>
        /// Makes a new instance of an interactive TUI mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which button is bound to the action?</param>
        /// <param name="bindingPointerButtonPress">Which press mode of the button is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        /// <param name="canRunWithoutItems">Whether the binding can run without items or not</param>
        public InteractiveTuiBinding(string bindingName, PointerButton bindingPointerButton, PointerButtonPress bindingPointerButtonPress, Action<object?, int>? bindingAction, bool canRunWithoutItems = false) :
            this(bindingName, bindingPointerButton, bindingPointerButtonPress, PointerModifiers.None, bindingAction, canRunWithoutItems)
        { }

        /// <summary>
        /// Makes a new instance of an interactive TUI mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which button is bound to the action?</param>
        /// <param name="bindingPointerButtonPress">Which press mode of the button is bound to the action?</param>
        /// <param name="bindingButtonModifiers">Which modifiers of the button is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        /// <param name="canRunWithoutItems">Whether the binding can run without items or not</param>
        public InteractiveTuiBinding(string bindingName, PointerButton bindingPointerButton, PointerButtonPress bindingPointerButtonPress, PointerModifiers bindingButtonModifiers, Action<object?, int>? bindingAction, bool canRunWithoutItems = false) :
            base(bindingName, bindingPointerButton, bindingPointerButtonPress, bindingButtonModifiers)
        {
            _bindingAction = bindingAction;
            _bindingCanRunWithoutItems = canRunWithoutItems;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            Equals(obj as InteractiveTuiBinding);

        /// <inheritdoc/>
        public bool Equals(InteractiveTuiBinding? other) =>
            other is not null &&
            BindingName == other.BindingName &&
            BindingUsesMouse == other.BindingUsesMouse &&
            BindingKeyName == other.BindingKeyName &&
            BindingKeyModifiers == other.BindingKeyModifiers &&
            BindingPointerButton == other.BindingPointerButton &&
            BindingPointerButtonPress == other.BindingPointerButtonPress &&
            BindingPointerModifiers == other.BindingPointerModifiers &&
            BindingCanRunWithoutItems == other.BindingCanRunWithoutItems &&
            EqualityComparer<Action<object?, int>?>.Default.Equals(BindingAction, other.BindingAction);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1588924110;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BindingName);
            hashCode = hashCode * -1521134295 + BindingUsesMouse.GetHashCode();
            hashCode = hashCode * -1521134295 + BindingCanRunWithoutItems.GetHashCode();
            hashCode = hashCode * -1521134295 + BindingKeyName.GetHashCode();
            hashCode = hashCode * -1521134295 + BindingKeyModifiers.GetHashCode();
            hashCode = hashCode * -1521134295 + BindingPointerButton.GetHashCode();
            hashCode = hashCode * -1521134295 + BindingPointerButtonPress.GetHashCode();
            hashCode = hashCode * -1521134295 + BindingPointerModifiers.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Action<object?, int>?>.Default.GetHashCode(BindingAction);
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(InteractiveTuiBinding? left, InteractiveTuiBinding? right) =>
            left is not null && right is not null &&
            EqualityComparer<InteractiveTuiBinding>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(InteractiveTuiBinding? left, InteractiveTuiBinding? right) =>
            !(left == right);
    }
}
