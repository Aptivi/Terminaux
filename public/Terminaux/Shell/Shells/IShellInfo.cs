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

using System.Collections.Generic;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Prompts;

namespace Terminaux.Shell.Shells
{
    /// <summary>
    /// Shell information interface
    /// </summary>
    public interface IShellInfo
    {
        /// <summary>
        /// Shell sync lock object
        /// </summary>
        object ShellLock { get; }
        /// <summary>
        /// Built-in shell commands
        /// </summary>
        List<CommandInfo> Commands { get; }
        /// <summary>
        /// Built-in shell presets
        /// </summary>
        Dictionary<string, PromptPresetBase> ShellPresets { get; }
        /// <summary>
        /// Mod shell presets
        /// </summary>
        Dictionary<string, PromptPresetBase> CustomShellPresets { get; }
        /// <summary>
        /// Gets the shell base
        /// </summary>
        BaseShell? ShellBase { get; }
        /// <summary>
        /// Gets the current preset
        /// </summary>
        PromptPresetBase CurrentPreset { get; }
        /// <summary>
        /// Whether the shell uses one line for input
        /// </summary>
        bool OneLineWrap { get; }
        /// <summary>
        /// Whether the shell uses a slash before the command executes
        /// </summary>
        bool SlashCommand { get; }
        /// <summary>
        /// For shells that have <see cref="SlashCommand"/> enabled, this is the command that gets executed every time we encounter text that doesn't start with the slash.
        /// </summary>
        CommandInfo NonSlashCommandInfo { get; }
    }
}
