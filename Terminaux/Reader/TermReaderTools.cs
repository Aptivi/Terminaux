
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
using System.Collections.Generic;
using System.Threading;
using Terminaux.Base;

namespace Terminaux.Reader
{
    /// <summary>
    /// Terminal reader tools
    /// </summary>
    public static class TermReaderTools
    {
        internal static bool interrupting = false;
        internal static bool isWaitingForInput = false;

        /// <summary>
        /// Interrupts the reading process
        /// </summary>
        public static void Interrupt()
        {
            if (isWaitingForInput)
                interrupting = true;
        }

        /// <summary>
        /// Sets the history
        /// </summary>
        /// <param name="History">List of history entries</param>
        public static void SetHistory(List<string> History)
        {
            TermReaderState.history = History;
            TermReaderState.currentHistoryPos = 0;
        }

        /// <summary>
        /// Clears the history
        /// </summary>
        public static void ClearHistory()
        {
            TermReaderState.history.Clear();
            TermReaderState.currentHistoryPos = 0;
        }

        internal static ConsoleKeyInfo GetInput(bool interruptible)
        {
            if (interruptible)
            {
                SpinWait.SpinUntil(() => ConsoleWrappers.ActionKeyAvailable() || interrupting);
                if (interrupting)
                {
                    interrupting = false;
                    if (ConsoleWrappers.ActionKeyAvailable())
                        ConsoleWrappers.ActionReadKey(true);
                    return new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false);
                }
                else
                    return ConsoleWrappers.ActionReadKey(true);
            }
            else
                return ConsoleWrappers.ActionReadKey(true);
        }
    }
}
