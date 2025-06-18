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

using Terminaux.Shell.Commands;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Exits the subshell
    /// </summary>
    /// <remarks>
    /// If the MESH shell is a subshell, you can exit it. However, you can't use this command to log out of your account, because it can't exit the mother shell. The only to exit it is to use the logout command.
    /// </remarks>
    class ExitUnifiedCommand : BaseCommand, ICommand
    {
        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            ShellManager.KillShell();
            return 0;
        }
    }
}
