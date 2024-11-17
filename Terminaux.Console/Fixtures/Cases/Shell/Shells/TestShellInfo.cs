//
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminaux.Console.Fixtures.Cases.Shell.Shells.Commands;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Terminaux.Console.Fixtures.Cases.Shell.Shells
{
    internal class TestShellInfo : BaseShellInfo, IShellInfo
    {
        /// <summary>
        /// List of commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("write", "Writes test text",
                [
                    new CommandArgumentInfo()
                ], new WriteCommand()),

            new CommandInfo("writearg", "Writes test text with argument support",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text")
                    ])
                ], new WriteArgCommand()),
        ];

        public override BaseShell ShellBase => new TestShellInstance();
    }
}
