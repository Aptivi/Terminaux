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

using Terminaux.Base;
using Terminaux.Reader.History;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Loads shell histories
    /// </summary>
    /// <remarks>
    /// You can use this command to load shell histories.
    /// </remarks>
    class LoadHistoriesUnifiedCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "loadhistories";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_LOADHISTORIES_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            HistoryTools.LoadHistories();
            return 0;
        }

    }
}
