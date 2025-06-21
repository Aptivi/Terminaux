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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System.Linq;
using System;
using System.Globalization;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Determine how to show date and time
            bool showDate = true;
            bool showTime = true;
            bool useUtc = false;
            if (parameters.SwitchesList.Length > 0)
            {
                showDate = parameters.SwitchesList.Contains("-date") || parameters.SwitchesList.Contains("-full");
                showTime = parameters.SwitchesList.Contains("-time") || parameters.SwitchesList.Contains("-full");
                useUtc = parameters.SwitchesList.Contains("-utc");
                if (!showDate && !showTime)
                    showDate = showTime = true;
            }

            // Now, show the date and the time
            if (showDate)
            {
                if (useUtc)
                {
                    string rendered = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
                else
                {
                    string rendered = DateTime.Now.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
            }
            if (showTime)
            {
                if (useUtc)
                {
                    string rendered = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, CultureInfo.InvariantCulture);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
                else
                {
                    string rendered = DateTime.Now.ToString(CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, CultureInfo.InvariantCulture);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
            }
            return 0;
        }
    }
}
