﻿
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using Terminaux.Reader.Inputs.Interactive;

namespace Terminaux.ConsoleDemo.Fixtures.Cases.CaseData
{
    internal class CliInfoPaneSlowTestData : BaseInteractiveTui, IInteractiveTui
    {
        internal static List<string> strings = [];

        public override List<InteractiveTuiBinding> Bindings { get; set; } =
        [
            new InteractiveTuiBinding(/* Localizable */ "Add",         ConsoleKey.F1, (_, index) => strings.Add($"[{index}] --+-- [{index}]")),
            new InteractiveTuiBinding(/* Localizable */ "Delete",      ConsoleKey.F2, (_, index) => strings.RemoveAt(index)),
            new InteractiveTuiBinding(/* Localizable */ "Delete Last", ConsoleKey.F3, (_, _)     => strings.RemoveAt(strings.Count - 1)),
            new InteractiveTuiBinding(/* Localizable */ "Redraw",      ConsoleKey.F4, (_, _)     => InteractiveTuiStatus.RedrawRequired = true),
        ];

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override bool FastRefresh =>
            false;

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            string selected = (string)item;

            // Check to see if we're given the test info
            if (string.IsNullOrEmpty(selected))
                InteractiveTuiStatus.Status = "No info.";
            else
                InteractiveTuiStatus.Status = $"{selected}";

            // Now, populate the info to the status
            return $" {InteractiveTuiStatus.Status}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            string selected = (string)item;
            return selected;
        }
    }
}
