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
using System.Linq;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Base;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Lets the user make a selection
    /// </summary>
    /// <remarks>
    /// This command can be used in a scripting file that ends in .MESH file extension. It lets the user select the correct answers when answering this question and passes the chosen answer to the specified variable.
    /// </remarks>
    class SelectCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var Titles = new List<(string, string)>();

            // Add the provided working titles
            var titles = parameters.ArgumentsList.Skip(2).ToArray();
            var split = parameters.ArgumentsList[0].Split('/');
            for (int i = 0; i < split.Length; i++)
            {
                string answer = split[i];
                string title = i >= titles.Length ? $"[{i + 1}]" : titles[i];
                Titles.Add((answer, title));
            }

            // Prompt for selection
            int SelectedAnswer = SelectionStyle.PromptSelection(parameters.ArgumentsList[1], [.. Titles]);
            variableValue = $"{(SelectedAnswer == -1 ? -1 : SelectedAnswer + 1)}";
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLS_MESH_CHOICE_HELPER"));

    }
}
