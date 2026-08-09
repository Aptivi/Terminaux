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
using Terminaux.Base;
using Colorimetry.Models.Conversion;
using Terminaux.Shell.Arguments;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Converts the color numbers to a specified color model in KS format.
    /// </summary>
    /// <remarks>
    /// If you want to get the semicolon-delimited sequence of the target model color numbers from the source model color numbers, you can use this command. You can use this to form a valid color sequence to generate new color instances for your mods.
    /// </remarks>
    class ColorToKSCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "colortoks";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_COMMAND_COLORTOKS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "sourceModelName", new CommandArgumentPartOptions()
                    {
                        ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz", "yxy", "cielab", "cielch", "cieluv", "hwb", "hunterlab", "lms", "ycbcrsdtv", "ycbcrhdtv", "ycbcrhivi", "ypbprsdtv", "ypbprhdtv", "ypbprhivi", "ydbdr"],
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_SOURCEMODELNAME_DESC"
                    }),
                    new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                    {
                        ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz", "yxy", "cielab", "cielch", "cieluv", "hwb", "hunterlab", "lms", "ycbcrsdtv", "ycbcrhdtv", "ycbcrhivi", "ypbprsdtv", "ypbprhdtv", "ypbprhivi", "ydbdr"],
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_TARGETMODELNAME_DESC"
                    }),
                    new CommandArgumentPart(true, "number1", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_NUMBER1_DESC"
                    }),
                    new CommandArgumentPart(true, "number2", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_NUMBER2_DESC"
                    }),
                    new CommandArgumentPart(true, "number3", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_NUMBER3_DESC"
                    }),
                    new CommandArgumentPart(false, "number4", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_NUMBER4_DESC"
                    }),
                    new CommandArgumentPart(false, "number5", new CommandArgumentPartOptions()
                    {
                        // TODO: T_SHELL_UNIFIED_COMMAND_ARGUMENT_NUMBER5_DESC -> Fifth number
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_NUMBER5_DESC"
                    }),
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            int fourth = 0, fifth = 0;
            if (!int.TryParse(parameters.ArgumentsList[2], out int first))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_FIRSTLEVELNUMERIC"), true, ThemeColorType.Error);
                return 48;
            }
            if (!int.TryParse(parameters.ArgumentsList[3], out int second))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_SECONDLEVELNUMERIC"), true, ThemeColorType.Error);
                return 48;
            }
            if (!int.TryParse(parameters.ArgumentsList[4], out int third))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_THIRDLEVELNUMERIC"), true, ThemeColorType.Error);
                return 48;
            }
            if (parameters.ArgumentsList.Length > 5 && !int.TryParse(parameters.ArgumentsList[5], out fourth))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_FOURTHLEVELNUMERIC"), true, ThemeColorType.Error);
                return 48;
            }
            if (parameters.ArgumentsList.Length > 6 && !int.TryParse(parameters.ArgumentsList[6], out fifth))
            {
                // TODO: T_SHELL_UNIFIED_COLORCONVERT_FIFTHLEVELNUMERIC -> The fifth key level must be numeric.
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_FIFTHLEVELNUMERIC"), true, ThemeColorType.Error);
                return 48;
            }

            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string target = parameters.ArgumentsList[1];
            var modelConvert = ConversionTools.GetConvertFuncFromModel(source, target);
            if (modelConvert is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_INVALIDMODEL"), true, ThemeColorType.Error);
                return 48;
            }
            var modelConverted = modelConvert.Invoke(first, second, third, fourth, fifth);

            // Do the job
            string finalSequence = modelConverted.ToString();
            TextWriterColor.Write(finalSequence, ThemeColorType.NeutralText);
            variableValue = finalSequence;
            return 0;
        }

    }
}
