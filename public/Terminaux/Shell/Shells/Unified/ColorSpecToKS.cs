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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Colorimetry.Models;
using Colorimetry.Models.Conversion;
using Terminaux.Base;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Converts the color specifier to the target color model in KS format.
    /// </summary>
    /// <remarks>
    /// If you want to get the target color model representation in KS format from the source color model specifier, you can use this command.
    /// </remarks>
    class ColorSpecToKSCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string specifier = parameters.ArgumentsList[1];
            var modelConvert = ConversionTools.GetConvertFuncFromSingleModel(source);
            if (modelConvert is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_INVALIDMODEL"), true, ThemeColorType.Error);
                return 48;
            }
            var modelConverted = modelConvert.Invoke(specifier);

            // Do the job
            string finalSequence = modelConverted.ToString();
            TextWriterColor.Write(finalSequence, ThemeColorType.NeutralText);
            variableValue = finalSequence;
            return 0;
        }

    }
}
