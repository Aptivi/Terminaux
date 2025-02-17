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

namespace Terminaux.Shell.Arguments.Base
{
    /// <summary>
    /// Argument information class
    /// </summary>
    public class ArgumentInfo
    {

        /// <summary>
        /// The argument
        /// </summary>
        public string Argument { get; private set; }
        /// <summary>
        /// The help definition of argument
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// Argument info
        /// </summary>
        public CommandArgumentInfo[] ArgArgumentInfo { get; private set; }
        /// <summary>
        /// Argument base for execution
        /// </summary>
        public ArgumentExecutor ArgumentBase { get; private set; }
        /// <summary>
        /// Is the argument obsolete?
        /// </summary>
        public bool Obsolete { get; private set; }

        /// <summary>
        /// Installs a new instance of argument info class
        /// </summary>
        /// <param name="Argument">Argument</param>
        /// <param name="HelpDefinition">Argument help definition</param>
        /// <param name="ArgumentBase">Kernel argument base for execution</param>
        /// <param name="Obsolete">Is the command obsolete?</param>
        public ArgumentInfo(string Argument, string HelpDefinition, ArgumentExecutor? ArgumentBase, bool Obsolete = false) :
            this(Argument, HelpDefinition, null, ArgumentBase, Obsolete)
        { }

        /// <summary>
        /// Installs a new instance of argument info class
        /// </summary>
        /// <param name="Argument">Argument</param>
        /// <param name="HelpDefinition">Argument help definition</param>
        /// <param name="ArgArgumentInfo">Argument info</param>
        /// <param name="ArgumentBase">Argument base for execution</param>
        /// <param name="Obsolete">Is the command obsolete?</param>
        public ArgumentInfo(string Argument, string HelpDefinition, CommandArgumentInfo[]? ArgArgumentInfo, ArgumentExecutor? ArgumentBase, bool Obsolete = false)
        {
            this.Argument = Argument;
            this.HelpDefinition = HelpDefinition;
            this.ArgArgumentInfo = ArgArgumentInfo ?? [];
            this.ArgumentBase = ArgumentBase ?? new UndefinedArgument();
            this.Obsolete = Obsolete;
        }

    }
}
