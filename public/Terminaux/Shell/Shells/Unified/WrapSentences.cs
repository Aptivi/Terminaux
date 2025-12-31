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
using Textify.General;
using Terminaux.Writer.ConsoleWriters;
using System.Linq;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Themes.Colors;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Wraps the long sentence to a set of sentences
    /// </summary>
    class WrapSentencesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool charWise = parameters.ContainsSwitch("-charwise");
            bool print = parameters.ContainsSwitch("-verbose");

            // Character to process
            string text = parameters.ArgumentsList[0];
            string widthStr = parameters.ArgumentsList[1];
            if (!int.TryParse(widthStr, out var width))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_WRAPSENTENCES_INVALIDWIDTH"), ThemeColorType.Error);
                return 1;
            }

            // Set the MESH variable to contain the result
            string[] sentences = charWise ?
                ConsoleMisc.GetWrappedSentences(text, width) :
                ConsoleMisc.GetWrappedSentencesByWords(text, width);
            variableValue = string.Join("\n", sentences);
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
