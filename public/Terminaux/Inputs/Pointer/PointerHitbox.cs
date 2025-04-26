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
using Terminaux.Base.Structures;

namespace Terminaux.Inputs.Pointer
{
    /// <summary>
    /// Pointer hitbox class
    /// </summary>
    public class PointerHitbox
    {
        private readonly Coordinate start;
        private readonly Size size;
        private readonly Func<PointerEventContext, object?> callback = (_) => null;

        /// <summary>
        /// Gets the position of where the mouse hitbox starts (usually upper left corner of a rectangle)
        /// </summary>
        public Coordinate Start =>
            start;

        /// <summary>
        /// Gets the position of where the mouse hitbox ends (usually lower right corner of a rectangle)
        /// </summary>
        public Coordinate End =>
            new(start.X + (size.Width > 0 ? size.Width - 1 : 0), start.Y + (size.Height > 0 ? size.Height - 1 : 0));

        /// <summary>
        /// Gets the size of the hitbox
        /// </summary>
        public Size Size =>
            size;

        /// <summary>
        /// Checks to see whether the pointer is within this hitbox
        /// </summary>
        /// <param name="context">Pointer event context to use in comparison</param>
        /// <returns>True if the pointer is within the hitbox; otherwise, false.</returns>
        public bool IsPointerWithin(PointerEventContext context) =>
            PointerTools.PointerWithinRange(context, (Start.X, Start.Y), (End.X, End.Y));

        /// <summary>
        /// Processes the pointer with a callback function
        /// </summary>
        /// <param name="context">Pointer event context to use when processing the pointer</param>
        /// <param name="inRange">True if the pointer is within the hitbox; otherwise, false.</param>
        /// <returns>null if <paramref name="inRange"/> is false or if there is no callback function; otherwise, the callback function return value</returns>
        public object? ProcessPointer(PointerEventContext context, out bool inRange)
        {
            // Check to see whether the pointer is within this hitbox
            inRange = IsPointerWithin(context);
            if (!inRange)
                return null;

            // Now, process the pointer by calling the callback function with this context
            return callback.Invoke(context);
        }

        /// <summary>
        /// Makes a new instance of the pointer hitbox
        /// </summary>
        /// <param name="start">Starting position of the pointer hitbox</param>
        /// <param name="end">Ending position of the pointer hitbox</param>
        /// <param name="callback">Callback function to run when a mouse event around it is processed</param>
        public PointerHitbox(Coordinate start, Coordinate end, Func<PointerEventContext, object?>? callback) :
            this(start, new Size(end.X, end.Y), callback)
        { }

        /// <summary>
        /// Makes a new instance of the pointer hitbox
        /// </summary>
        /// <param name="start">Starting position of the pointer hitbox</param>
        /// <param name="size">Size of the pointer hitbox</param>
        /// <param name="callback">Callback function to run when a mouse event around it is processed</param>
        public PointerHitbox(Coordinate start, Size size, Func<PointerEventContext, object?>? callback)
        {
            this.start = start;
            this.size = size;
            this.callback = callback ?? new((_) => null);
        }
    }
}
