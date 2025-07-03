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

using Terminaux.Base;

namespace Terminaux.Inputs.Pointer
{
    /// <summary>
    /// Pointer tools
    /// </summary>
    public static class PointerTools
    {
        /// <summary>
        /// Checks to see if the returned pointer event context is in the range of the two dimensions (if the mouse pointer is within the boundaries)
        /// </summary>
        /// <param name="context">Pointer event context (You can easily obtain it using the <see cref="Input.ReadPointer"/> or the <see cref="Input.ReadPointerOrKey"/> functions)</param>
        /// <param name="point">Point position</param>
        /// <returns>True if the pointer is within the specified point position; false otherwise</returns>
        public static bool PointerWithinPoint(PointerEventContext context, (int x, int y) point) =>
            PointerWithinRange(context, point, point);

        /// <summary>
        /// Checks to see if the returned pointer event context is in the range of the two dimensions (if the mouse pointer is within the boundaries)
        /// </summary>
        /// <param name="context">Pointer event context (You can easily obtain it using the <see cref="Input.ReadPointer"/> or the <see cref="Input.ReadPointerOrKey"/> functions)</param>
        /// <param name="start">Starting position representing an upper left corner of the rectangle</param>
        /// <param name="end">Ending position representing a lower right corner of the rectangle</param>
        /// <returns>True if the pointer is within the range; false otherwise</returns>
        public static bool PointerWithinRange(PointerEventContext context, (int x, int y) start, (int x, int y) end)
        {
            if (context is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_POINTER_EXCEPTION_NOEVENTCTX"));
            if (start.x < 0 || start.y < 0)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_POINTER_EXCEPTION_STARTPOSUNDERFLOW"));
            if (end.x < 0 || end.y < 0)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_POINTER_EXCEPTION_ENDPOSUNDERFLOW"));
            if (start.x > end.x)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_POINTER_EXCEPTION_STARTXPOSOVERFLOWSEND"));
            if (start.y > end.y)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_POINTER_EXCEPTION_STARTYPOSOVERFLOWSEND"));
            return
                context.Coordinates.x >= start.x && context.Coordinates.x <= end.x &&
                context.Coordinates.y >= start.y && context.Coordinates.y <= end.y;
        }
    }
}
