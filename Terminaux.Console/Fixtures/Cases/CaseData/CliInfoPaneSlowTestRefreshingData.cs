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

using System;
using System.Collections.Generic;
using Terminaux.Inputs.Interactive;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class CliInfoPaneSlowTestRefreshingData : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal static List<string> strings = [];
        private static int timesRendered = 0;

        public override List<InteractiveTuiBinding> Bindings { get; set; } =
        [
            new InteractiveTuiBinding("Add",         ConsoleKey.F1, (_, index) => strings.Add($"[{index}] --+-- [{index}]")),
            new InteractiveTuiBinding("Delete",      ConsoleKey.F2, (_, index) => strings.RemoveAt(index)),
            new InteractiveTuiBinding("Delete Last", ConsoleKey.F3, (_, _)     => strings.RemoveAt(strings.Count - 1)),
        ];

        /// <inheritdoc/>
        public override int RefreshInterval =>
            3000;

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            // Some variables
            string selected = item;
            timesRendered++;

            // Check to see if we're given the test info
            if (string.IsNullOrEmpty(selected))
                InteractiveTuiStatus.Status = "No info.";
            else
                InteractiveTuiStatus.Status = $"{selected}";

            // Now, populate the info to the status
            return $"{timesRendered}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item)
        {
            string selected = item;
            return selected;
        }
    }
}
