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
using Terminaux.Base;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// You can clear your screen from clutter
    /// </summary>
    /// <remarks>
    /// If you are trying to focus on one thing and you can't do it, or if you want the personal info printed by commands hidden, you can clear your screen from clutter to gain focus and reduce eyestrain.
    /// </remarks>
    class ClsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            ConsoleWrapper.Clear();
            return 0;
        }
    }
}
