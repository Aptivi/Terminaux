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
using System.Threading;
using Terminaux.Base;
using Terminaux.Reader;

namespace Terminaux.Inputs
{
    /// <summary>
    /// Console input module
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static (ConsoleKeyInfo result, bool provided) ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            TermReaderTools.isWaitingForInput = true;
            bool result = SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, Timeout);
            TermReaderTools.isWaitingForInput = false;
            return (!result ? default : ConsoleWrapper.ReadKey(Intercept), result);
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypress()
        {
            TermReaderTools.isWaitingForInput = true;
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);
            TermReaderTools.isWaitingForInput = false;
            return ConsoleWrapper.ReadKey(true);
        }
    }
}
