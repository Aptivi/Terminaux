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
        private readonly Action<PointerEventContext> voidCallback = (_) => { };

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
        /// Gets the button or the scrolling direction
        /// </summary>
        public PointerButton Button { get; set; } = PointerButton.Left;

        /// <summary>
        /// Gets the button press state
        /// </summary>
        public PointerButtonPress ButtonPress { get; set; } = PointerButtonPress.Released;

        /// <summary>
        /// Gets the modifiers pressed at the time of the event
        /// </summary>
        public PointerModifiers Modifiers { get; set; }

        /// <summary>
        /// Whether the pointer is being dragged or not
        /// </summary>
        public bool Dragging { get; set; }

        /// <summary>
        /// Whether to process the click tier or not
        /// </summary>
        public bool ProcessTier { get; set; }

        /// <summary>
        /// Specifies whether this is a single-click (1), double-click (2), or more. Only populated in the <see cref="PointerButtonPress.Released"/> event, and you'll need to populate it when listening to this event via the hitbox instance if <see cref="ProcessTier"/> is on.
        /// </summary>
        public int ClickTier { get; set; }

        /// <summary>
        /// Checks to see whether the pointer is within this hitbox
        /// </summary>
        /// <param name="context">Pointer event context to use in comparison</param>
        /// <returns>True if the pointer is within the hitbox; otherwise, false.</returns>
        public bool IsPointerWithin(PointerEventContext context) =>
            PointerTools.PointerWithinRange(context, (Start.X, Start.Y), (End.X, End.Y));

        /// <summary>
        /// Checks to see whether the pointer matches the modifier conditions specified
        /// </summary>
        /// <param name="context">Pointer event context to use in comparison</param>
        /// <returns>True if the pointer meets all modifier conditions; otherwise, false.</returns>
        public bool IsPointerModifierMatch(PointerEventContext context) =>
            Button.HasFlag(context.Button) &&
            context.ButtonPress == ButtonPress &&
            context.Modifiers == Modifiers &&
            context.Dragging == Dragging &&
            ((ProcessTier && context.ClickTier == ClickTier) || !ProcessTier);

        /// <summary>
        /// Processes the pointer with a callback function
        /// </summary>
        /// <param name="context">Pointer event context to use when processing the pointer</param>
        /// <param name="status">True if the pointer is within the hitbox and meets all modifier conditions; otherwise, false.</param>
        /// <returns>null if <paramref name="status"/> is false or if there is no callback function; otherwise, the callback function return value</returns>
        public object? ProcessPointer(PointerEventContext context, out bool status)
        {
            // Check to see whether the pointer is within this hitbox
            status = IsPointerWithin(context);
            if (!status)
                return null;
            
            // Check to see whether the pointer meets all modifier conditions
            status = IsPointerModifierMatch(context);
            if (!status)
                return null;

            // Now, process the pointer by calling the callback function with this context
            if (voidCallback is not null)
            {
                voidCallback.Invoke(context);
                return null;
            }
            return callback.Invoke(context);
        }

        /// <summary>
        /// Makes a new instance of the pointer hitbox
        /// </summary>
        /// <param name="point">Pointer hitbox to process at this point</param>
        /// <param name="callback">Callback function to run when a mouse event around it is processed</param>
        public PointerHitbox(Coordinate point, Func<PointerEventContext, object?>? callback) :
            this(point, point, callback)
        { }

        /// <summary>
        /// Makes a new instance of the pointer hitbox
        /// </summary>
        /// <param name="start">Starting position of the pointer hitbox</param>
        /// <param name="end">Ending position of the pointer hitbox</param>
        /// <param name="callback">Callback function to run when a mouse event around it is processed</param>
        public PointerHitbox(Coordinate start, Coordinate end, Func<PointerEventContext, object?>? callback) :
            this(start, new Size(end.X - start.X + 1, end.Y - start.Y + 1), callback)
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

        /// <summary>
        /// Makes a new instance of the pointer hitbox
        /// </summary>
        /// <param name="point">Pointer hitbox to process at this point</param>
        /// <param name="callback">Callback function to run when a mouse event around it is processed</param>
        public PointerHitbox(Coordinate point, Action<PointerEventContext>? callback) :
            this(point, point, callback)
        { }

        /// <summary>
        /// Makes a new instance of the pointer hitbox
        /// </summary>
        /// <param name="start">Starting position of the pointer hitbox</param>
        /// <param name="end">Ending position of the pointer hitbox</param>
        /// <param name="callback">Callback function to run when a mouse event around it is processed</param>
        public PointerHitbox(Coordinate start, Coordinate end, Action<PointerEventContext>? callback) :
            this(start, new Size(end.X - start.X + 1, end.Y - start.Y + 1), callback)
        { }

        /// <summary>
        /// Makes a new instance of the pointer hitbox
        /// </summary>
        /// <param name="start">Starting position of the pointer hitbox</param>
        /// <param name="size">Size of the pointer hitbox</param>
        /// <param name="callback">Callback function to run when a mouse event around it is processed</param>
        public PointerHitbox(Coordinate start, Size size, Action<PointerEventContext>? callback)
        {
            this.start = start;
            this.size = size;
            voidCallback = callback ?? new((_) => { });
        }
    }
}
