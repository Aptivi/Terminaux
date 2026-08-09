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
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Decodes the text from its BASE64 representation
    /// </summary>
    /// <remarks>
    /// This command will decode a text from its BASE64 representation.
    /// </remarks>
    class DecodeBase64Command : BaseCommand, ICommand
    {
        public override string Command =>
            "decodebase64";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_DECODEBASE64_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "encoded", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_DECODEBASE64_ARGUMENT_ENCODED_DESC"
                    })
                ])
            ];

        public override CommandFlags Flags => 
            CommandFlags.Hidden;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string orig = parameters.ArgumentsList[0];
            string decoded = orig.GetBase64Decoded();
            TextWriterColor.Write(decoded, true, ThemeColorType.Success);
            return 0;
        }
    }
}
