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

using System.Diagnostics;
using Terminaux.Shell.Commands;

namespace Terminaux.Shell.Aliases
{
    /// <summary>
    /// Command alias information
    /// </summary>
    [DebuggerDisplay("[{Type}] {Alias} -> {Command}")]
    public class AliasInfo
    {
        internal string alias = "";
        internal string command = "";
        internal string type = "";

        /// <summary>
        /// Gets the alias that the shell resolves to the actual command
        /// </summary>
        public string Alias =>
            alias;
        /// <summary>
        /// The actual command being resolved to
        /// </summary>
        public string Command =>
            command;
        /// <summary>
        /// Type of the resolved command
        /// </summary>
        public string Type =>
            type;
        /// <summary>
        /// Resolved target command info (for execution)
        /// </summary>
        public CommandInfo TargetCommand =>
            CommandManager.GetCommand(Command, Type);

        internal AliasInfo(string alias, string command, string type)
        {
            this.alias = alias;
            this.command = command;
            this.type = type;
        }
    }
}
