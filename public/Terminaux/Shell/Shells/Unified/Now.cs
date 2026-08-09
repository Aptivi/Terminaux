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

using System;
using System.Globalization;
using System.Text;
using Terminaux.Base;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Shows the current time and date
    /// </summary>
    /// <remarks>
    /// If you want to know what time is it without repeatedly going into the clock, you can use this command to show you the current time and date.
    /// </remarks>
    class NowCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "now";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_SHOWTD_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo([
                    new SwitchInfo("date", /* Localizable */ "T_SHELL_UNIFIED_DATE_SWITCH_DATE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["time", "full"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("time", /* Localizable */ "T_SHELL_UNIFIED_DATE_SWITCH_TIME_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["date", "full"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("full", /* Localizable */ "T_SHELL_UNIFIED_SHOWTD_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["date", "time"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("utc", /* Localizable */ "T_SHELL_UNIFIED_DATE_SWITCH_UTC_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ], true)
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Determine how to show date and time
            bool showDate = true;
            bool showTime = true;
            bool useUtc = false;
            if (parameters.SwitchesList.Length > 0)
            {
                showDate = parameters.ContainsSwitch("-date") || parameters.ContainsSwitch("-full");
                showTime = parameters.ContainsSwitch("-time") || parameters.ContainsSwitch("-full");
                useUtc = parameters.ContainsSwitch("-utc");
                if (!showDate && !showTime)
                    showDate = showTime = true;
            }

            // Render the date/time string
            StringBuilder builder = new();
            var dateTime = useUtc ? DateTime.UtcNow : DateTime.Now;
            if (showDate)
            {
                string rendered = dateTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
                builder.Append(rendered);
                if (showTime)
                    builder.Append(" ");
            }
            if (showTime)
            {
                string rendered = dateTime.ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, CultureInfo.InvariantCulture);
                builder.Append(rendered);
            }

            // Now, show the date and the time
            variableValue = builder.ToString();
            TextWriterColor.Write(variableValue);
            return 0;
        }
    }
}
