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

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// State of the textual UI
    /// </summary>
    public enum TextualUIState
    {
        /// <summary>
        /// This textual UI is ready, but hasn't started yet.
        /// </summary>
        Ready,
        /// <summary>
        /// This textual UI is waiting for the render code to complete.
        /// </summary>
        Rendering,
        /// <summary>
        /// This textual UI is waiting for user input.
        /// </summary>
        Waiting,
        /// <summary>
        /// This textual UI is busy because it's processing user input
        /// </summary>
        Busy,
        /// <summary>
        /// This textual UI is about to exit and go back to the <see cref="Ready"/> state.
        /// </summary>
        Bailing,
    }
}
