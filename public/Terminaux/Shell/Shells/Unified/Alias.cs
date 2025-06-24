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

using System;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// You can set an alternative shortcut to the command if you want to use shorter words for long commands.
    /// </summary>
    /// <remarks>
    /// Some commands in this kernel are long, and some people doesn't write fast on computers. The alias command fixes this problem by providing the shorter terms for long commands.
    /// <br></br>
    /// You can also use this command if you plan to make scripts if the real file system will be added in the future, or if you are rushing for something and you don't have time to execute the long command.
    /// <br></br>
    /// You can add or remove the alias to the long command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class AliasCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string mode = parameters.ArgumentsList[0];
            string type = parameters.ArgumentsList[1];
            string aliasCmd = parameters.ArgumentsList[2];
            bool shouldSave = false;
            if (parameters.ArgumentsList.Length > 3)
            {
                string destCmd = parameters.ArgumentsList[3];
                if (mode == "add" & ShellManager.AvailableShells.ContainsKey(type))
                {
                    // User tries to add an alias.
                    try
                    {
                        AliasManager.AddAlias(destCmd, aliasCmd, type);
                        shouldSave = true;
                        TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_ALIAS_SUCCESS"), ThemeColorType.Success, aliasCmd, destCmd);
                    }
                    catch (Exception ex)
                    {
                        ConsoleLogger.Error(ex, "Failed to add alias. {0}", ex.Message);
                        TextWriterColor.Write(ex.Message, ThemeColorType.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_ALIAS_EXCEPTION_INVALIDTYPE"), ThemeColorType.Error, type);
                    return 1;
                }
            }
            else if (parameters.ArgumentsList.Length == 3)
            {
                if (parameters.ArgumentsList[0] == "rem" & ShellManager.AvailableShells.ContainsKey(type))
                {
                    // User tries to remove an alias
                    try
                    {
                        AliasManager.RemoveAlias(aliasCmd, type);
                        shouldSave = true;
                        TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_ALIAS_REMOVALSUCCESS"), ThemeColorType.Success, aliasCmd);
                    }
                    catch (Exception ex)
                    {
                        ConsoleLogger.Error(ex, "Failed to remove alias. Stack trace written using WStkTrc(). {0}", ex.Message);
                        TextWriterColor.Write(ex.Message, ThemeColorType.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_ALIAS_EXCEPTION_INVALIDTYPE"), ThemeColorType.Error, type);
                    return 1;
                }
            }

            // Save all aliases if the addition or the removal is successful
            if (shouldSave)
                AliasManager.SaveAliases();
            return 0;
        }

    }
}
