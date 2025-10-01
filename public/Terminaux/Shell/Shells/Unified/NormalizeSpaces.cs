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
using Textify.Data.Words.Profanity;
using System;
using Textify.SpaceManager.Conversion;
using Textify.SpaceManager.Analysis;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Normalizes spaces in a string
    /// </summary>
    class NormalizeSpacesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool simple = parameters.ContainsSwitch("-simple");
            bool analytical = parameters.ContainsSwitch("-analytical");
            bool print = parameters.ContainsSwitch("-verbose");

            // Text to process
            string text = parameters.ArgumentsList[0];

            // Set the MESH variable to contain the result
            string result = "";
            if (simple)
                result = SpaceConversionTools.ConvertSpacesSimple(text);
            else
            {
                var analysis = SpaceAnalysisTools.AnalyzeSpaces(text);
                result = SpaceConversionTools.ConvertSpacesToString(analysis);
            }
            variableValue = result;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
