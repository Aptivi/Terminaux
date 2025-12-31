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
using Textify.Data.NameGen;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Last name generator
    /// </summary>
    /// <remarks>
    /// If you're stuck trying to make out your character names (male or female) in your story, or if you just like to generate names, you can use this command. Please note that it requires Internet access.
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-male</term>
    /// <description>Generate names using the male names list</description>
    /// </item>
    /// <item>
    /// <term>-female</term>
    /// <description>Generate names using the female names list</description>
    /// </item>
    /// <item>
    /// <term>-both</term>
    /// <description>Generate names using both male and female names list</description>
    /// </item>
    /// </list>
    /// </remarks>
    class GenLastNamesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int SurnamesCount = 10;
            string SurnamePrefix = "";
            string SurnameSuffix = "";
            string[] SurnamesList;
            if (parameters.ArgumentsList.Length >= 1)
                SurnamesCount = int.Parse(parameters.ArgumentsList[0]);
            if (parameters.ArgumentsList.Length >= 2)
                SurnamePrefix = parameters.ArgumentsList[1];
            if (parameters.ArgumentsList.Length >= 3)
                SurnameSuffix = parameters.ArgumentsList[2];

            // Generate n last names
            NameGenerator.PopulateNames();
            SurnamesList = NameGenerator.GenerateLastNames(SurnamesCount, SurnamePrefix, SurnameSuffix);
            foreach (string surname in SurnamesList)
                TextWriterColor.Write(surname);
            variableValue = string.Join("\n", SurnamesList);
            return 0;
        }

    }
}
