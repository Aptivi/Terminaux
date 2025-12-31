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

using System.Linq;
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Arguments;

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// Command information class
    /// </summary>
    public class CommandInfo
    {

        /// <summary>
        /// The command
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// The help definition of command
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// Command argument info
        /// </summary>
        public CommandArgumentInfo[] CommandArgumentInfo { get; private set; }
        /// <summary>
        /// Command base for execution
        /// </summary>
        public BaseCommand CommandBase { get; private set; }
        /// <summary>
        /// Aliases for this command
        /// </summary>
        public AliasInfo[] Aliases =>
            AliasManager.aliases
                .Where((ai) => ai.Command == Command)
                .ToArray();

        /// <summary>
        /// Installs a new instance of command info class
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="HelpDefinition">Command help definition</param>
        /// <param name="CommandArgumentInfo">Command argument info</param>
        /// <param name="CommandBase">Command base for execution</param>
        public CommandInfo(string Command, string HelpDefinition, CommandArgumentInfo[]? CommandArgumentInfo, BaseCommand? CommandBase)
        {
            this.Command = Command;
            this.HelpDefinition = HelpDefinition;
            this.CommandArgumentInfo = CommandArgumentInfo ?? [];
            this.CommandBase = CommandBase ?? new UndefinedCommand();
        }

        /// <summary>
        /// Installs a new instance of an empty command info class
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="HelpDefinition">Command help definition</param>
        internal CommandInfo(string Command, string HelpDefinition) :
            this(Command, HelpDefinition, null, null)
        { }
    }
}
