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
using Textify.Tools.Placeholder;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Echoes the text
    /// </summary>
    /// <remarks>
    /// This command will repeat back the string that you have entered. It is used in scripting to print text. It supports $variable parsing.
    /// </remarks>
    class EchoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool parsePlaces = !parameters.ContainsSwitch("-noparse");
            if (parameters.SwitchesList.Length == 0)
                parsePlaces = true;
            string result = parsePlaces ? PlaceParse.ProbePlaces(parameters.ArgumentsText) : parameters.ArgumentsText;
            TextWriterColor.Write(result);
            variableValue = result;
            return 0;
        }
    }
}
