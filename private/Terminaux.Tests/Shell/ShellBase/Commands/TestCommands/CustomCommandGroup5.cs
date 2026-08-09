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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Arguments;

namespace Terminaux.Tests.Shell.ShellBase.Commands.TestCommands
{
    internal class CustomCommandGroup5 : BaseCommand, ICommand
    {
        public override string Command =>
            "cmdgroup5";

        public override string HelpDefinition =>
            "My command help definition...";

        public override CommandArgumentInfo[] CommandArgumentInfo => 
            [];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write($"Passed arguments: [{string.Join(", ", parameters.ArgumentsList)}]");
            return 0;
        }

    }
}
