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
using Terminaux.Reader;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// This command will ask the user a question, and the user has to write the password.
    /// </summary>
    /// <remarks>
    /// This command can be used in shell scripts to ask the user a specified question, which has to be answered using text. It will then pass the password in plain text to the specified $variable.
    /// </remarks>
    class InputPassCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Answer = TermReader.Read(parameters.ArgumentsList[0], true);
            variableValue = Answer;
            return 0;
        }
    }
}
