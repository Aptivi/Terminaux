//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using System.Collections.Generic;
using System.Threading;

namespace Terminaux.Shell.Shells
{
    /// <summary>
    /// Shell execution information
    /// </summary>
    public class ShellExecuteInfo
    {
        internal int lastErrorCode = 0;
        internal readonly List<Thread> AltCommandThreads = [];
        internal bool interrupting = false;
        internal Thread shellCommandThread;
        private readonly string shellType;
        private readonly BaseShell? shellBase;

        /// <summary>
        /// Shell type
        /// </summary>
        public string ShellType =>
            shellType;

        /// <summary>
        /// Shell base class
        /// </summary>
        public BaseShell? ShellBase =>
            shellBase;

        /// <summary>
        /// Shell command thread
        /// </summary>
        public Thread ShellCommandThread =>
            shellCommandThread;

        /// <summary>
        /// Last error code for this shell
        /// </summary>
        public int LastErrorCode =>
            lastErrorCode;

        /// <summary>
        /// Installs the values to a new instance of ShellInfo
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellBase">Shell base class</param>
        /// <param name="ShellCommandThread">Shell command thread</param>
        public ShellExecuteInfo(string ShellType, BaseShell? ShellBase, Thread ShellCommandThread)
        {
            shellType = ShellType;
            shellBase = ShellBase;
            shellCommandThread = ShellCommandThread;
        }

    }
}
