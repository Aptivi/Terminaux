//
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
    internal class CliDoublePaneSlowTestData : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal static List<string> strings = [];
        internal static List<string> strings2 = [];

        public override InteractiveTuiBinding[] Bindings { get; } =
        [
            new InteractiveTuiBinding("Add",         ConsoleKey.F1,  (_, index) => Add(index), true),
            new InteractiveTuiBinding("Delete",      ConsoleKey.F2,  (_, index) => Remove(index)),
            new InteractiveTuiBinding("Delete Last", ConsoleKey.F3,  (_, _)     => RemoveLast()),
        ];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override IEnumerable<string> SecondaryDataSource =>
            strings2;

        /// <inheritdoc/>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            string.IsNullOrEmpty(item) ? "No info." : item;

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        private static void Add(int index)
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
                strings2.Add($"[{index}] --2-- [{index}]");
            else
                strings.Add($"[{index}] --1-- [{index}]");
        }

        private static void Remove(int index)
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                if (index < strings2.Count && strings2.Count > 0)
                    strings2.RemoveAt(index == 0 ? index : index - 1);
                if (InteractiveTuiStatus.SecondPaneCurrentSelection > strings2.Count)
                    InteractiveTuiStatus.SecondPaneCurrentSelection = strings2.Count;
            }
            else
            {
                if (index < strings.Count && strings.Count > 0)
                    strings.RemoveAt(index == 0 ? index : index - 1);
                if (InteractiveTuiStatus.FirstPaneCurrentSelection > strings.Count)
                    InteractiveTuiStatus.FirstPaneCurrentSelection = strings.Count;
            }
        }

        private static void RemoveLast()
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                if (strings2.Count > 0)
                    strings2.RemoveAt(strings2.Count - 1);
                if (InteractiveTuiStatus.SecondPaneCurrentSelection > strings2.Count)
                    InteractiveTuiStatus.SecondPaneCurrentSelection = strings2.Count;
            }
            else
            {
                if (strings.Count > 0)
                    strings.RemoveAt(strings.Count - 1);
                if (InteractiveTuiStatus.FirstPaneCurrentSelection > strings.Count)
                    InteractiveTuiStatus.FirstPaneCurrentSelection = strings.Count;
            }
        }
    }
}
