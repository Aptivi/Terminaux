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

using System.Linq;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Textify.Data.NameGen;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Surname generator
    /// </summary>
    /// <remarks>
    /// If you're stuck trying to make out your character names (male or female) in your story, or if you just like to generate names, you can use this command. Please note that it requires Internet access.
    /// </remarks>
    class LastNamesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string term = "";
            string SurnamePrefix = "";
            string SurnameSuffix = "";
            bool nametags = parameters.SwitchesList.Contains("-t");
            string[] NamesList;
            if (parameters.ArgumentsList.Length >= 1)
                term = parameters.ArgumentsList[0];
            if (parameters.ArgumentsList.Length >= 2)
                SurnamePrefix = parameters.ArgumentsList[1];
            if (parameters.ArgumentsList.Length >= 3)
                SurnameSuffix = parameters.ArgumentsList[2];

            // Generate n names
            NameGenerator.PopulateNames();
            NamesList = NameGenerator.FindLastNames(term, SurnamePrefix, SurnameSuffix);

            // Check to see if we need to modify the list to have nametags
            if (nametags)
                for (int i = 0; i < NamesList.Length; i++)
                    NamesList[i] = "@" + NamesList[i].ToLower().Replace(" ", ".");
            foreach (string name in NamesList)
                TextWriterColor.Write(name);
            variableValue = string.Join("\n", NamesList);
            return 0;
        }

    }
}
