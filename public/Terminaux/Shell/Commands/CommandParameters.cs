﻿//
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

using System.Diagnostics;

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// Command parameters that holds information about arguments and switches
    /// </summary>
    [DebuggerDisplay("Cmd = {CommandText}, Args = {ArgumentsText}")]
    public class CommandParameters
    {
        private readonly string stringCommand;
        private readonly string stringArgs;
        private readonly string[] listArgsOnly;
        private readonly string stringArgsOrig;
        private readonly string[] listArgsOnlyOrig;
        private readonly string[] listSwitchesOnly;

        /// <summary>
        /// Name of command
        /// </summary>
        public string CommandText =>
            stringCommand;
        /// <summary>
        /// Text of arguments (filtered)
        /// </summary>
        public string ArgumentsText =>
            stringArgs;
        /// <summary>
        /// List of passed arguments (filtered)
        /// </summary>
        public string[] ArgumentsList =>
            listArgsOnly;
        /// <summary>
        /// Text of arguments (original)
        /// </summary>
        public string ArgumentsOriginalText =>
            stringArgsOrig;
        /// <summary>
        /// List of passed arguments (original)
        /// </summary>
        public string[] ArgumentsOriginalList =>
            listArgsOnlyOrig;
        /// <summary>
        /// List of passed switches
        /// </summary>
        public string[] SwitchesList =>
            listSwitchesOnly;
        /// <summary>
        /// Whether the <c>-set=var</c> switch has been passed to the command or not
        /// </summary>
        public bool SwitchSetPassed { get; internal set; }

        internal CommandParameters(string stringArgs, string[] listArgsOnly, string stringArgsOrig, string[] listArgsOnlyOrig, string[] listSwitchesOnly, string commandName)
        {
            this.stringArgs = stringArgs;
            this.listArgsOnly = listArgsOnly;
            this.stringArgsOrig = stringArgsOrig;
            this.listArgsOnlyOrig = listArgsOnlyOrig;
            this.listSwitchesOnly = listSwitchesOnly;
            stringCommand = commandName;
        }
    }
}
