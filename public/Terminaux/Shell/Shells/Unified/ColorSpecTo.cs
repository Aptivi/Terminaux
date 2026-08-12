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
using Terminaux.Shell.Arguments;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Converts the color specifier to the target color model.
    /// </summary>
    /// <remarks>
    /// If you want to get the target color model representation from the source color model specifier, you can use this command.
    /// </remarks>
    class ColorSpecToCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "colorspecto";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_COMMAND_COLORSPECTO_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                    {
                        ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz", "yxy", "cielab", "cielch", "cieluv", "hwb", "hunterlab", "lms", "ycbcrsdtv", "ycbcrhdtv", "ycbcrhivi", "ypbprsdtv", "ypbprhdtv", "ypbprhivi", "ydbdr"],
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_TARGETMODELNAME_DESC"
                    }),
                    new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_SPECIFIER_DESC"
                    }),
                ], true)
            ];

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
            switch (source)
            {
                case "rgb":
                    var rgb = (RedGreenBlue)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_REDCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{rgb.R}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_GREENCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{rgb.G}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_BLUECOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{rgb.B}", true, ThemeColorType.ListValue);
                    break;
                case "ryb":
                    var ryb = (RedYellowBlue)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_REDCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ryb.R}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ryb.Y}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_BLUECOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ryb.B}", true, ThemeColorType.ListValue);
                    break;
                case "cmy":
                    var cmy = (CyanMagentaYellow)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_CYANCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmy.CWhole} [{cmy.C:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_MAGENTACOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmy.MWhole} [{cmy.M:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmy.YWhole} [{cmy.Y:0.00}]", true, ThemeColorType.ListValue);
                    break;
                case "cmyk":
                    var cmyk = (CyanMagentaYellowKey)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_CYANCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.CMY.CWhole} [{cmyk.CMY.C:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_MAGENTACOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.CMY.MWhole} [{cmyk.CMY.M:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.CMY.YWhole} [{cmyk.CMY.Y:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_BLACKKEY") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.KWhole} [{cmyk.K:0.00}]", true, ThemeColorType.ListValue);
                    break;
                case "hsv":
                    var hsv = (HueSaturationValue)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_HUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsv.HueWhole} [{hsv.Hue:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_SATURATION") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsv.SaturationWhole} [{hsv.Saturation:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_VALUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsv.ValueWhole} [{hsv.Value:0.00}]", true, ThemeColorType.ListValue);
                    break;
                case "hsl":
                    var hsl = (HueSaturationLightness)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_HUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsl.HueWhole} [{hsl.Hue:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_SATURATION") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsl.SaturationWhole} [{hsl.Saturation:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_LUMINANCE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsl.LightnessWhole} [{hsl.Lightness:0.00}]", true, ThemeColorType.ListValue);
                    break;
                case "yiq":
                    var yiq = (LumaInPhaseQuadrature)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_LUMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yiq.Luma}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_INPHASE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yiq.InPhase}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_QUADRATURE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yiq.Quadrature}", true, ThemeColorType.ListValue);
                    break;
                case "yuv":
                    var yuv = (LumaChromaUv)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_LUMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yuv.Luma}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_UCHROMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yuv.ChromaU}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_VCHROMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yuv.ChromaV}", true, ThemeColorType.ListValue);
                    break;
                case "xyz":
                    var xyz = (Xyz)modelConverted;
                    TextWriterColor.Write("- X: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{xyz.X:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{xyz.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Z: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{xyz.Z:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "yxy":
                    var yxy = (Yxy)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yxy.Y1:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- X: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yxy.X:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yxy.Y2:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "cielab":
                    var cielab = (CieLab)modelConverted;
                    TextWriterColor.Write("- L: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielab.L:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- A: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielab.A:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- B: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielab.B:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_ILLUMINANT") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielab.Illuminant}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_OBSERVER") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielab.Observer}", true, ThemeColorType.ListValue);
                    break;
                case "cielch":
                    var cielch = (CieLch)modelConverted;
                    TextWriterColor.Write("- L: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielch.L:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- C: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielch.C:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- H: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielch.H:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_ILLUMINANT") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielch.Illuminant}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_OBSERVER") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cielch.Observer}", true, ThemeColorType.ListValue);
                    break;
                case "cieluv":
                    var cieluv = (CieLuv)modelConverted;
                    TextWriterColor.Write("- L: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cieluv.L:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- U: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cieluv.U:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- V: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cieluv.V:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_ILLUMINANT") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cieluv.Illuminant}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_OBSERVER") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cieluv.Observer}", true, ThemeColorType.ListValue);
                    break;
                case "hwb":
                    var hwb = (HueWhiteBlack)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_HUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hwb.HueWhole}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_WHITE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hwb.WhitenessWhole}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("T_SHELL_UNIFIED_COLORCONVERT_BLACK") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hwb.BlacknessWhole}", true, ThemeColorType.ListValue);
                    break;
                case "hunterlab":
                    var hunterlab = (HunterLab)modelConverted;
                    TextWriterColor.Write("- L: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hunterlab.L:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- A: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hunterlab.A:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- B: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hunterlab.B:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "lms":
                    var lms = (Lms)modelConverted;
                    TextWriterColor.Write("- L: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{lms.L:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- M: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{lms.M:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- S: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{lms.S:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "ycbcrsdtv":
                    var ycbcrsdtv = (YCbCrSDTV)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrsdtv.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Cb: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrsdtv.Cb:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Cr: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrsdtv.Cr:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "ycbcrhdtv":
                    var ycbcrhdtv = (YCbCrHDTV)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrhdtv.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Cb: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrhdtv.Cb:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Cr: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrhdtv.Cr:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "ycbcrhivi":
                    var ycbcrhivi = (YCbCrHiVi)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrhivi.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Cb: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrhivi.Cb:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Cr: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ycbcrhivi.Cr:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "ypbprsdtv":
                    var ypbprsdtv = (YPbPrSDTV)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprsdtv.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Pb: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprsdtv.Pb:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Pr: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprsdtv.Pr:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "ypbprhdtv":
                    var ypbprhdtv = (YPbPrHDTV)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprhdtv.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Pb: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprhdtv.Pb:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Pr: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprhdtv.Pr:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "ypbprhivi":
                    var ypbprhivi = (YPbPrHiVi)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprhivi.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Pb: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprhivi.Pb:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Pr: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ypbprhivi.Pr:0.##}", true, ThemeColorType.ListValue);
                    break;
                case "ydbdr":
                    var ydbdr = (YDbDr)modelConverted;
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ydbdr.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Db: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ydbdr.Db:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Dr: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ydbdr.Dr:0.##}", true, ThemeColorType.ListValue);
                    break;
            }
            variableValue = modelConverted.ToString();
            return 0;
        }

    }
}
