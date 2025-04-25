//
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
using System.Diagnostics;
using Terminaux.Inputs.Pointer;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Keybinding class
    /// </summary>
    [DebuggerDisplay("{BindingName} (M: {BindingUsesMouse} | H: {BindingHidden}) [K: {BindingKeyModifiers} + {BindingKeyName}] [M: {BindingPointerModifiers} + {BindingPointerButton} {BindingPointerButtonPress}]")]
    public class Keybinding
    {
        private readonly string _bindingName;
        private readonly bool _bindingUsesMouse;
        private readonly bool _bindingHidden;
        private readonly ConsoleKey _bindingKeyName = (ConsoleKey)(-1);
        private readonly ConsoleModifiers _bindingKeyModifiers = (ConsoleModifiers)(-1);
        private readonly PointerButton _bindingPointerButton = (PointerButton)(-1);
        private readonly PointerButtonPress _bindingPointerButtonPress = (PointerButtonPress)(-1);
        private readonly PointerModifiers _bindingPointerModifiers = (PointerModifiers)(-1);

        /// <summary>
        /// Key binding name
        /// </summary>
        public string BindingName =>
            _bindingName;

        /// <summary>
        /// Whether the binding uses the mouse or the keyboard
        /// </summary>
        public bool BindingUsesMouse =>
            _bindingUsesMouse;

        /// <summary>
        /// Whether the key binding is hidden or not
        /// </summary>
        public bool BindingHidden =>
            _bindingHidden;

        /// <summary>
        /// Which key is bound to the action?
        /// </summary>
        public ConsoleKey BindingKeyName =>
            _bindingKeyName;

        /// <summary>
        /// Which key modifiers are bound to the action?
        /// </summary>
        public ConsoleModifiers BindingKeyModifiers =>
            _bindingKeyModifiers;

        /// <summary>
        /// Which pointer button is bound to the action?
        /// </summary>
        public PointerButton BindingPointerButton =>
            _bindingPointerButton;

        /// <summary>
        /// Which pointer button press mode is bound to the action?
        /// </summary>
        public PointerButtonPress BindingPointerButtonPress =>
            _bindingPointerButtonPress;

        /// <summary>
        /// Which pointer modifier is bound to the action?
        /// </summary>
        public PointerModifiers BindingPointerModifiers =>
            _bindingPointerModifiers;

        /// <summary>
        /// Makes a new instance of a key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="hidden">Whether the key binding is hidden or not</param>
        public Keybinding(string bindingName, ConsoleKey bindingKeyName, bool hidden = false) :
            this(bindingName, bindingKeyName, default, hidden)
        { }

        /// <summary>
        /// Makes a new instance of a key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="bindingKeyModifiers">Which modifiers of the key are bound to the action?</param>
        /// <param name="hidden">Whether the key binding is hidden or not</param>
        public Keybinding(string bindingName, ConsoleKey bindingKeyName, ConsoleModifiers bindingKeyModifiers, bool hidden = false)
        {
            _bindingName = bindingName;
            _bindingHidden = hidden;
            _bindingKeyName = bindingKeyName;
            _bindingKeyModifiers = bindingKeyModifiers;
        }

        /// <summary>
        /// Makes a new instance of a mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which key is bound to the action?</param>
        /// <param name="hidden">Whether the key binding is hidden or not</param>
        public Keybinding(string bindingName, PointerButton bindingPointerButton, bool hidden = false) :
            this(bindingName, bindingPointerButton, PointerButtonPress.Moved, PointerModifiers.None, hidden)
        { }

        /// <summary>
        /// Makes a new instance of a mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which button is bound to the action?</param>
        /// <param name="bindingPointerButtonPress">Which press mode of the button is bound to the action?</param>
        /// <param name="hidden">Whether the key binding is hidden or not</param>
        public Keybinding(string bindingName, PointerButton bindingPointerButton, PointerButtonPress bindingPointerButtonPress, bool hidden = false) :
            this(bindingName, bindingPointerButton, bindingPointerButtonPress, PointerModifiers.None, hidden)
        { }

        /// <summary>
        /// Makes a new instance of a mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which button is bound to the action?</param>
        /// <param name="bindingPointerButtonPress">Which press mode of the button is bound to the action?</param>
        /// <param name="bindingButtonModifiers">Which modifiers of the button are bound to the action?</param>
        /// <param name="hidden">Whether the key binding is hidden or not</param>
        public Keybinding(string bindingName, PointerButton bindingPointerButton, PointerButtonPress bindingPointerButtonPress, PointerModifiers bindingButtonModifiers, bool hidden = false)
        {
            _bindingName = bindingName;
            _bindingUsesMouse = true;
            _bindingHidden = hidden;
            _bindingPointerButton = bindingPointerButton;
            _bindingPointerButtonPress = bindingPointerButtonPress;
            _bindingPointerModifiers = bindingButtonModifiers;
        }
    }
}
