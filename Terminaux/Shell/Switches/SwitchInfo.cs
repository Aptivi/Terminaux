﻿//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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

namespace Terminaux.Shell.Switches
{
    /// <summary>
    /// Information for command switches
    /// </summary>
    public class SwitchInfo
    {

        /// <summary>
        /// The switch name (without the dash)
        /// </summary>
        public string SwitchName { get; private set; }
        /// <summary>
        /// The help definition of this switch
        /// </summary>
        public string HelpDefinition { get; private set; }
        /// <summary>
        /// Switch options
        /// </summary>
        public SwitchOptions Options { get; private set; }
        /// <summary>
        /// Is the switch required?
        /// </summary>
        public bool IsRequired =>
            Options.IsRequired;
        /// <summary>
        /// Does the switch require arguments?
        /// </summary>
        public bool ArgumentsRequired =>
            Options.ArgumentsRequired;
        /// <summary>
        /// Does the switch accept values?
        /// </summary>
        public bool AcceptsValues =>
            Options.AcceptsValues;
        /// <summary>
        /// Does the switch conflict with the provided switches?
        /// </summary>
        public string[]? ConflictsWith =>
            Options.ConflictsWith;
        /// <summary>
        /// Whether to make the last N required arguments optional. This is useful for some switches, like -list.
        /// </summary>
        public int OptionalizeLastRequiredArguments =>
            Options.OptionalizeLastRequiredArguments;
        /// <summary>
        /// Whether to make this switch only accept numbers
        /// </summary>
        public bool IsNumeric =>
            Options.IsNumeric;

        /// <summary>
        /// Installs a new instance of switch info class
        /// </summary>
        /// <param name="Switch">Switch (without the dash)</param>
        /// <param name="HelpDefinition">Switch help definition</param>
        public SwitchInfo(string Switch, string HelpDefinition)
        {
            SwitchName = Switch;
            this.HelpDefinition = HelpDefinition;
            Options = new SwitchOptions();
        }

        /// <summary>
        /// Installs a new instance of switch info class
        /// </summary>
        /// <param name="Switch">Switch (without the dash)</param>
        /// <param name="HelpDefinition">Switch help definition</param>
        /// <param name="options">Switch options</param>
        public SwitchInfo(string Switch, string HelpDefinition, SwitchOptions options)
        {
            SwitchName = Switch;
            this.HelpDefinition = HelpDefinition;
            Options = options ?? new SwitchOptions();
        }

        /// <summary>
        /// Installs a new instance of switch info class
        /// </summary>
        /// <param name="Switch">Switch (without the dash)</param>
        /// <param name="HelpDefinition">Switch help definition</param>
        /// <param name="IsRequired">Is the switch required?</param>
        /// <param name="ArgumentsRequired">Whether the switch requires a value to be set</param>
        /// <param name="conflictsWith">Does the switch conflict with the provided switches?</param>
        /// <param name="optionalizeLastRequiredArguments">Whether to make the last N required arguments optional. This is useful for some switches, like -list.</param>
        /// <param name="IsNumeric">Whether to make this switch only accept numbers</param>
        /// <param name="AcceptsValues">Does the switch accept values?</param>
        public SwitchInfo(string Switch, string HelpDefinition, bool IsRequired = false, bool ArgumentsRequired = false, string[]? conflictsWith = null, int optionalizeLastRequiredArguments = 0, bool AcceptsValues = true, bool IsNumeric = false)
        {
            SwitchName = Switch;
            this.HelpDefinition = HelpDefinition;
            Options = new SwitchOptions()
            {
                AcceptsValues = ArgumentsRequired || AcceptsValues,
                ArgumentsRequired = ArgumentsRequired,
                ConflictsWith = conflictsWith,
                IsRequired = IsRequired,
                OptionalizeLastRequiredArguments = optionalizeLastRequiredArguments,
                IsNumeric = IsNumeric,
            };
        }
    }
}
