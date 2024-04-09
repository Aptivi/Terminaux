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

namespace Terminaux.Inputs.Pointer
{
    /// <summary>
    /// Pointer event context
    /// </summary>
    public class PointerEventContext
    {
        private readonly PointerButton mouseButton;
        private readonly PointerButtonPress mouseButtonPress;
        private readonly PointerModifiers mouseMods;
        private readonly bool dragging;
        private readonly int x, y;

        /// <summary>
        /// Gets the button or the scrolling direction
        /// </summary>
        public PointerButton Button =>
            mouseButton;

        /// <summary>
        /// Gets the button press state
        /// </summary>
        public PointerButtonPress ButtonPress =>
            mouseButtonPress;

        /// <summary>
        /// Gets the modifiers pressed at the time of the event
        /// </summary>
        public PointerModifiers Modifiers =>
            mouseMods;

        /// <summary>
        /// Whether the pointer is being dragged or not
        /// </summary>
        public bool Dragging =>
            dragging;

        /// <summary>
        /// Gets the coordinates in character cells where the mouse cursor was placed <b>starting from zero</b>.
        /// </summary>
        public (int x, int y) Coordinates =>
            (x, y);

        internal PointerEventContext(PointerButton mouseButton, PointerButtonPress mouseButtonPress, PointerModifiers mouseMods, bool dragging, int x, int y)
        {
            this.mouseButton = mouseButton;
            this.mouseButtonPress = mouseButtonPress;
            this.mouseMods = mouseMods;
            this.dragging = dragging;
            this.x = x;
            this.y = y;
        }
    }
}
