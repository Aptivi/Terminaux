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
using Textify.General;
using Terminaux.Themes.Colors;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Encodes the text to its BASE64 representation
    /// </summary>
    /// <remarks>
    /// This command will encode a text to its BASE64 representation.
    /// </remarks>
    class EncodeBase64Command : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string orig = parameters.ArgumentsList[0];
            string encoded = orig.GetBase64Encoded();
            TextWriterColor.Write(encoded, true, ThemeColorType.Success);
            return 0;
        }
    }
}
