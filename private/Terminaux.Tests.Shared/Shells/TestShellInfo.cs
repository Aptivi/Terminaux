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

using System.Collections.Generic;
using Terminaux.Tests.Shared.Shells.Commands;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Terminaux.Tests.Shared.Shells.Prompts;

namespace Terminaux.Tests.Shared.Shells
{
    internal class TestShellInfo : BaseShellInfo<TestShellInstance>, IShellInfo
    {
        /// <summary>
        /// List of commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("openempty", "Opens an empty shell", new OpenEmptyCommand()),

            new CommandInfo("write", "Writes test text", new WriteCommand()),

            new CommandInfo("writearg", "Writes test text with argument support",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text")
                    ])
                ], new WriteArgCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "TestPreset", new TestPreset() }
        };
    }
}
