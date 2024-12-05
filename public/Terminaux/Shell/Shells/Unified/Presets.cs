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

using Terminaux.Shell.Commands;
using Terminaux.Shell.Prompts;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Changes the shell preset
    /// </summary>
    /// <remarks>
    /// This command allows you to change your shell presets to either one of the pre-defined presets or your custom preset installed by a mod.
    /// </remarks>
    class PresetsUnifiedCommand : BaseCommand, ICommand
    {
        public override void Execute(CommandParameters parameters) =>
            PromptPresetManager.PromptForPresets();
    }
}
