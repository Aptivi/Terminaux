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
using Terminaux.Shell.Scripting;
using System;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Themes.Colors;

namespace Terminaux.Shell.Shells.Unified
{
    class LintScriptCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                string pathToScript = ConsoleFilesystem.NeutralizePath(parameters.ArgumentsList[0]);
                MESHParse.Execute(pathToScript, "", true);
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_LINTSCRIPT_SUCCESS"), true, ThemeColorType.Success);
                variableValue = "1";
                return 0;
            }
            catch (TerminauxException tex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_LINTSCRIPT_FAILED"), true, ThemeColorType.Error);
                TextWriterColor.Write(tex.Message, true, ThemeColorType.Error);
                variableValue = "0";
                return tex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_LINTSCRIPT_UNEXPECTEDERROR") + $" {ex.Message}", true, ThemeColorType.Error);
                variableValue = "0";
                return ex.GetHashCode();
            }
        }

    }
}
