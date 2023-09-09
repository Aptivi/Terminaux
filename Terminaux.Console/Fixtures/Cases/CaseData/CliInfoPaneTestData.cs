
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
    internal class CliInfoPaneTestData : BaseInteractiveTui, IInteractiveTui
    {
        internal static List<string> strings = new();

        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            new InteractiveTuiBinding(/* Localizable */ "Add",         ConsoleKey.F1, (_, index) => Add(index)),
            new InteractiveTuiBinding(/* Localizable */ "Delete",      ConsoleKey.F2, (_, index) => Remove(index)),
            new InteractiveTuiBinding(/* Localizable */ "Delete Last", ConsoleKey.F3, (_, _)     => RemoveLast()),
            new InteractiveTuiBinding(/* Localizable */ "Redraw",      ConsoleKey.F4, (_, _)     => InteractiveTuiStatus.RedrawRequired = true),
        };

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

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

        private static void Add(int index)
        {
            strings.Add($"[{index}] --+-- [{index}]");
            InteractiveTuiTools.ForceRefreshSelection();
        }

        private static void Remove(int index)
        {
            strings.RemoveAt(index);
            InteractiveTuiTools.ForceRefreshSelection();
        }

        private static void RemoveLast()
        {
            strings.RemoveAt(strings.Count - 1);
            InteractiveTuiTools.ForceRefreshSelection();
        }
    }
}
