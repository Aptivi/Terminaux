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

using System.IO;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Terminaux.Shell.Scripting
{
    /// <summary>
    /// MESH operator code functions
    /// </summary>
    public static class MESHOperators
    {
        /// <summary>
        /// Checks to see if the two MESH variables are equal
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the two MESH variables are equal. False if otherwise.</returns>
        public static bool MESHVariableEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1} for equality...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = FirstVarValue == SecondVarValue;
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the two MESH variables are different
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the two MESH variables are different. False if otherwise.</returns>
        public static bool MESHVariableNotEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = FirstVarValue != SecondVarValue;
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two MESH variables is less than the other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two MESH variables is less than the other. False if otherwise.</returns>
        public static bool MESHVariableLessThan(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt < SecondVarInt;
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two MESH variables is greater than the other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two MESH variables is greater than the other. False if otherwise.</returns>
        public static bool MESHVariableGreaterThan(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt > SecondVarInt;
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two MESH variables is less than the other or equal to each other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two MESH variables is less than the other or equal to each other. False if otherwise.</returns>
        public static bool MESHVariableLessThanOrEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt <= SecondVarInt;
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two MESH variables is greater than the other or equal to each other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two MESH variables is greater than the other or equal to each other. False if otherwise.</returns>
        public static bool MESHVariableGreaterThanOrEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt >= SecondVarInt;
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is a file path and exists
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the file exists. False if otherwise.</returns>
        public static bool MESHVariableFileExists(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} for file existence...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            VarValue = ConsoleFilesystem.NeutralizePath(VarValue);
            Satisfied = File.Exists(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is a file path and doesn't exist
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the file doesn't exist. False if otherwise.</returns>
        public static bool MESHVariableFileDoesNotExist(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} for file existence...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            VarValue = ConsoleFilesystem.NeutralizePath(VarValue);
            Satisfied = !File.Exists(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is a directory path and exists
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the directory exists. False if otherwise.</returns>
        public static bool MESHVariableDirectoryExists(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} for directory existence...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            VarValue = ConsoleFilesystem.NeutralizePath(VarValue);
            Satisfied = Directory.Exists(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is a directory path and doesn't exist
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the directory doesn't exist. False if otherwise.</returns>
        public static bool MESHVariableDirectoryDoesNotExist(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} for directory existence...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            VarValue = ConsoleFilesystem.NeutralizePath(VarValue);
            Satisfied = !Directory.Exists(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a MESH variable contains an expression
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool MESHVariableContains(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = FirstVarValue.Contains(SecondVarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a MESH variable doesn't contain an expression
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool MESHVariableNotContains(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = MESHVariables.GetVariable(FirstVariable);
            ConsoleLogger.Debug("Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = MESHVariables.GetVariable(SecondVariable);
            ConsoleLogger.Debug("Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = !FirstVarValue.Contains(SecondVarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is a valid file path
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if valid. False if otherwise.</returns>
        public static bool MESHVariableValidPath(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0}...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = ConsoleFilesystem.TryParsePath(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is not a valid file path
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if invalid. False if otherwise.</returns>
        public static bool MESHVariableInvalidPath(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0}...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = !ConsoleFilesystem.TryParsePath(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is a valid file name
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if valid. False if otherwise.</returns>
        public static bool MESHVariableValidFileName(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0}...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = ConsoleFilesystem.TryParseFileName(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the MESH variable is not a valid file name
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if invalid. False if otherwise.</returns>
        public static bool MESHVariableInvalidFileName(string Variable)
        {
            bool Satisfied;
            ConsoleLogger.Debug("Querying {0}...", Variable);
            string VarValue = MESHVariables.GetVariable(Variable);
            ConsoleLogger.Debug("Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = !ConsoleFilesystem.TryParseFileName(VarValue);
            ConsoleLogger.Debug("Satisfied: {0}", Satisfied);
            return Satisfied;
        }
    }
}
