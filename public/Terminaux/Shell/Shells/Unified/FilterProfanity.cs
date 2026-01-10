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

using Terminaux.Shell.Commands;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Textify.Data.Words.Profanity;
using System;
using Terminaux.Themes.Colors;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Filters profanity in a string
    /// </summary>
    class FilterProfanityCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool print = !parameters.ContainsSwitch("-quiet");

            // Text to process
            string text = parameters.ArgumentsList[0];
            string profanityType = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "Shallow";
            if (!Enum.TryParse(profanityType, out ProfanitySearchType profanitySearchType))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_FINDPROFANITY_PROFANITYTYPEINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Set the MESH variable to contain the result
            string result = ProfanityManager.FilterProfanities(text, profanitySearchType);
            variableValue = result;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
