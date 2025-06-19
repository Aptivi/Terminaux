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

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Terminaux.Shell.Aliases
{
    /// <summary>
    /// Alias management module
    /// </summary>
    public static class AliasManager
    {
        internal static List<AliasInfo> aliases = [];

        /// <summary>
        /// Initializes aliases
        /// </summary>
        public static void InitAliases() =>
            InitAliasesPath(ConsoleFilesystem.GetSubPath("Aliases") + $"/{ShellManager.CurrentShellType}.json");

        /// <summary>
        /// Initializes aliases
        /// </summary>
        /// <param name="shellType">The shell type</param>
        public static void InitAliases(string shellType) =>
            InitAliasesPath(ConsoleFilesystem.GetSubPath("Aliases") + $"/{shellType}.json");

        /// <summary>
        /// Initializes aliases
        /// </summary>
        /// <param name="aliasPath">Path to alias JSON file</param>
        public static void InitAliasesPath(string aliasPath)
        {
            // Get all aliases from file
            if (!File.Exists(aliasPath))
                File.WriteAllText(aliasPath, "[]");
            string AliasJsonContent = File.ReadAllText(aliasPath);
            var aliasesArray = JsonConvert.DeserializeObject<AliasInfo[]>(AliasJsonContent) ?? [];
            foreach (var alias in aliasesArray)
            {
                if (!DoesAliasExist(alias.Alias, alias.Type))
                    aliases.Add(alias);
            }
        }

        /// <summary>
        /// Saves aliases
        /// </summary>
        public static void SaveAliases() =>
            SaveAliasesPath(ConsoleFilesystem.GetSubPath("Aliases") + $"/{ShellManager.CurrentShellType}.json");

        /// <summary>
        /// Saves aliases
        /// </summary>
        /// <param name="shellType">The shell type</param>
        public static void SaveAliases(string shellType) =>
            SaveAliasesPath(ConsoleFilesystem.GetSubPath("Aliases") + $"/{shellType}.json");

        /// <summary>
        /// Saves aliases
        /// </summary>
        /// <param name="aliasPath">Path to alias JSON file</param>
        public static void SaveAliasesPath(string aliasPath)
        {
            // Save all aliases
            ConsoleLogger.Debug("Saving aliases...");
            string serialized = JsonConvert.SerializeObject(aliases.ToArray(), Formatting.Indented);
            File.WriteAllText(aliasPath, serialized);
        }

        /// <summary>
        /// Adds alias
        /// </summary>
        /// <param name="SourceAlias">A command to be aliased. It should exist in the shell.</param>
        /// <param name="Destination">A one-word command to alias to.</param>
        /// <param name="Type">Shell type.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool AddAlias(string SourceAlias, string Destination, string Type)
        {
            if (ShellManager.ShellTypeExists(Type))
            {
                ConsoleLogger.Debug("Checking alias {0} pointing to {1} under type {2}...", SourceAlias, Destination, Type);
                if (SourceAlias == Destination)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_ALIAS_EXCEPTION_ALIASCMDSAMENAME"));
                else if (!CommandManager.IsCommandFound(SourceAlias, Type))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_ALIAS_EXCEPTION_CMDNOTFOUND"), Destination);
                else if (DoesAliasExist(Destination, Type))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_ALIAS_EXCEPTION_ALIASFOUND") + ": {0}", SourceAlias);
                ConsoleLogger.Info("Adding alias {0} pointing to {1} under type {2}...", SourceAlias, Destination, Type);
                var aliasInstance = new AliasInfo(Destination, SourceAlias, Type);
                aliases.Add(aliasInstance);
                return true;
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_COMMON_EXCEPTION_INVALIDTYPE") + " {0}.", Type);
        }

        /// <summary>
        /// Removes alias
        /// </summary>
        /// <param name="TargetAlias">An alias that needs to be removed.</param>
        /// <param name="Type">Shell type.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool RemoveAlias(string TargetAlias, string Type)
        {
            // Do the action!
            if (DoesAliasExist(TargetAlias, Type))
            {
                var AliasInfo = GetAlias(TargetAlias, Type);
                ConsoleLogger.Info("Removing alias {0} pointing to {1} under type {2}...", TargetAlias, AliasInfo.Command, Type);
                return aliases.Remove(AliasInfo);
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_ALIAS_EXCEPTION_ALIASNOTFOUND_REMOVE"), TargetAlias);
        }

        /// <summary>
        /// Checks to see if the specified alias exists.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">Shell type.</param>
        /// <returns>True if it exists; false if it doesn't exist</returns>
        public static bool DoesAliasExist(string TargetAlias, string Type) =>
            GetAliasListFromType(Type).Any((info) => info.Alias == TargetAlias && info.Type == Type);

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static List<AliasInfo> GetAliasListFromType(string ShellType) =>
            aliases.Where((info) => info.Type == ShellType).ToList();

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">Shell type.</param>
        /// <returns>Alias info if it exists. Throws if it doesn't exist.</returns>
        public static AliasInfo GetAlias(string TargetAlias, string Type)
        {
            if (!DoesAliasExist(TargetAlias, Type))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_ALIAS_EXCEPTION_ALIASNOTFOUND_QUERY"), TargetAlias);

            // Get the list of available aliases and get an alias matching the target alias
            var aliases = GetAliasListFromType(Type);
            ConsoleLogger.Info("Finding alias {0} under type {1} in {2} aliases...", TargetAlias, Type, aliases.Count);
            return aliases.Single((info) => info.Alias == TargetAlias);
        }
    }
}
