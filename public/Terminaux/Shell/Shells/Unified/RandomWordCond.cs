//
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
using Textify.General;
using Terminaux.Writer.ConsoleWriters;
using System.Linq;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Themes.Colors;
using System.Text.RegularExpressions;
using Textify.Data.Words;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Generates a random word conditionally
    /// </summary>
    class RandomWordCondCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool print = !parameters.ContainsSwitch("-quiet");

            // Conditions to process
            string maxLengthStr = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList[0] : "0";
            string startsWith = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "";
            string endsWith = parameters.ArgumentsList.Length > 2 ? parameters.ArgumentsList[2] : "";
            string exactLengthStr = parameters.ArgumentsList.Length > 3 ? parameters.ArgumentsList[3] : "0";
            if (!int.TryParse(maxLengthStr, out var maxLength))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_RANDOMWORDCOND_MAXLENGTHINVALID"), ThemeColorType.Error);
                return 1;
            }
            if (!int.TryParse(exactLengthStr, out var exactLength))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_RANDOMWORDCOND_EXACTLENGTHINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Set the MESH variable to contain the result
            string processed = WordManager.GetRandomWordConditional(maxLength, startsWith, endsWith, exactLength);
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
