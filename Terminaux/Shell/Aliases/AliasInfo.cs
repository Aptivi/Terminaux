﻿//
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

using Newtonsoft.Json;
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
        [JsonProperty(PropertyName = nameof(Alias))]
        internal string alias = "";
        [JsonProperty(PropertyName = nameof(Command))]
        internal string command = "";
        [JsonProperty(PropertyName = nameof(Type))]
        internal string type = "";

        /// <summary>
        /// Gets the alias that the shell resolves to the actual command
        /// </summary>
        [JsonIgnore]
        public string Alias =>
            alias;
        /// <summary>
        /// The actual command being resolved to
        /// </summary>
        [JsonIgnore]
        public string Command =>
            command;
        /// <summary>
        /// Type of the resolved command
        /// </summary>
        [JsonIgnore]
        public string Type =>
            type;
        /// <summary>
        /// Resolved target command info (for execution)
        /// </summary>
        [JsonIgnore]
        public CommandInfo TargetCommand =>
            CommandManager.GetCommand(Command, Type);

        [JsonConstructor]
        internal AliasInfo()
        { }

        internal AliasInfo(string alias, string command, string type)
        {
            this.alias = alias;
            this.command = command;
            this.type = type;
        }
    }
}
