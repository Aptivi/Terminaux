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
    /// Gets a DPI
    /// </summary>
    class DpiCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool print = parameters.ContainsSwitch("-verbose");

            // Text to process
            string pixelsStr = parameters.ArgumentsList[0];
            string inchesStr = parameters.ArgumentsList[1];
            if (!int.TryParse(pixelsStr, out var pixels) || pixels < 0)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_DPI_PIXELSINVALID"), ThemeColorType.Error);
                return 1;
            }
            if (!double.TryParse(inchesStr, out var inches) || inches <= 0)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_DPI_INCHESINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Calculate the DPI using this formula: pixels / length in inches
            double dpi = pixels / inches;

            // Set the MESH variable to contain the result
            string processed = $"{dpi}";
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
