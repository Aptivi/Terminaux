﻿//
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

using System.Collections.Generic;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// An interface for your interactive user interface for terminal apps
    /// </summary>
    public interface IInteractiveTui<T> : IInteractiveTui<T, T>
    { }

    /// <summary>
    /// An interface for your interactive user interface for terminal apps
    /// </summary>
    public interface IInteractiveTui<TPrimary, TSecondary>
    {
        /// <summary>
        /// Whether the user can switch to the second path
        /// </summary>
        public bool SecondPaneInteractable { get; }
        /// <summary>
        /// How many milliseconds to wait before refreshing? Only applies to single-pane interactive TUI instances. 0 to disable.
        /// </summary>
        public int RefreshInterval { get; }
        /// <summary>
        /// Whether empty data is accepted
        /// </summary>
        public bool AcceptsEmptyData { get; }

        /// <summary>
        /// An array, a dictionary, a list, or an enumerable that holds data (pane one)
        /// </summary>
        public IEnumerable<TPrimary> PrimaryDataSource { get; }
        /// <summary>
        /// An array, a dictionary, a list, or an enumerable that holds data (pane two)
        /// </summary>
        public IEnumerable<TSecondary> SecondaryDataSource { get; }

        /// <summary>
        /// Gets an entry string from a specified item for listing
        /// </summary>
        /// <param name="item">Target item</param>
        public string GetEntryFromItem(TPrimary item);
        /// <summary>
        /// Gets the info from the item
        /// </summary>
        /// <param name="item">Target item</param>
        /// <returns>The rendered info so that <see cref="InteractiveTuiTools"/> can handle its rendering</returns>
        public string GetInfoFromItem(TPrimary item);
        /// <summary>
        /// Gets the status from the item
        /// </summary>
        /// <param name="item">Target item</param>
        public string GetStatusFromItem(TPrimary item);
        /// <summary>
        /// Gets an entry string from a specified item for listing
        /// </summary>
        /// <param name="item">Target item</param>
        public string GetEntryFromItemSecondary(TSecondary item);
        /// <summary>
        /// Gets the info from the item
        /// </summary>
        /// <param name="item">Target item</param>
        /// <returns>The rendered info so that <see cref="InteractiveTuiTools"/> can handle its rendering</returns>
        public string GetInfoFromItemSecondary(TSecondary item);
        /// <summary>
        /// Gets the status from the item
        /// </summary>
        /// <param name="item">Target item</param>
        public string GetStatusFromItemSecondary(TSecondary item);
        /// <summary>
        /// Handles exiting the interactive TUI
        /// </summary>
        public void HandleExit();
    }
}
