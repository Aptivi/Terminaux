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

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Gets the total number of pixels
    /// </summary>
    class PixelsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool print = parameters.ContainsSwitch("-verbose");

            // Text to process
            string widthStr = parameters.ArgumentsList[0];
            string heightStr = parameters.ArgumentsList[1];
            if (!long.TryParse(widthStr, out var width) || width < 0)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_PIXELS_WIDTHINVALID"), ThemeColorType.Error);
                return 1;
            }
            if (!long.TryParse(heightStr, out var height) || height < 0)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_PIXELS_HEIGHTINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Calculate the total number of pixels
            long pixels = width * height;

            // Set the MESH variable to contain the result
            string processed = $"{pixels}";
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
