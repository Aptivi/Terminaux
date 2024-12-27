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

namespace Terminaux.Reader.Tools
{
    /// <summary>
    /// Conditional tools for reader
    /// </summary>
    public static class ConditionalTools
    {
        /// <summary>
        /// Specifies that this condition should not be met
        /// </summary>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="state">Reader state</param>
        /// <returns>True if the condition is not met; false otherwise and a system beep.</returns>
        public static bool ShouldNot(bool condition, TermReaderState state)
        {
            if (condition)
                state.operationWasInvalid = true;
            return !condition;
        }

        /// <summary>
        /// Specifies that this condition should be met
        /// </summary>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="state">Reader state</param>
        /// <returns>True if the condition is met; false otherwise and a system beep.</returns>
        public static bool Should(bool condition, TermReaderState state)
        {
            if (!condition)
                state.operationWasInvalid = true;
            return condition;
        }
    }
}
