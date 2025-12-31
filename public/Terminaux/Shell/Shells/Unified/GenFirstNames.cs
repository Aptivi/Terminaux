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
    /// First name generator
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
    class GenFirstNamesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int NamesCount = 10;
            string NamePrefix = "";
            string NameSuffix = "";
            NameGenderType genderType = NameGenderType.Unified;
            if (parameters.ContainsSwitch("-male"))
                genderType = NameGenderType.Male;
            else if (parameters.ContainsSwitch("-female"))
                genderType = NameGenderType.Female;
            string[] NamesList;
            if (parameters.ArgumentsList.Length >= 1)
                NamesCount = int.Parse(parameters.ArgumentsList[0]);
            if (parameters.ArgumentsList.Length >= 2)
                NamePrefix = parameters.ArgumentsList[1];
            if (parameters.ArgumentsList.Length >= 3)
                NameSuffix = parameters.ArgumentsList[2];

            // Generate n first names
            NameGenerator.PopulateNames();
            NamesList = NameGenerator.GenerateFirstNames(NamesCount, NamePrefix, NameSuffix, genderType);
            foreach (string name in NamesList)
                TextWriterColor.Write(name);
            variableValue = string.Join("\n", NamesList);
            return 0;
        }

    }
}
