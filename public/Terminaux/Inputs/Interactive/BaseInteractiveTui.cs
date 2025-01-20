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

using System.Collections.Generic;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// A base class for your interactive user interface for terminal apps
    /// </summary>
    public class BaseInteractiveTui<T> : BaseInteractiveTui<T, T>, IInteractiveTui<T, T>
    { }

    /// <summary>
    /// A base class for your interactive user interface for terminal apps
    /// </summary>
    public class BaseInteractiveTui<TPrimary, TSecondary> : IInteractiveTui<TPrimary, TSecondary>
    {
        /// <summary>
        /// Current selection for the first pane
        /// </summary>
        public int FirstPaneCurrentSelection { get; internal set; } = 1;
        /// <summary>
        /// Current selection for the second pane
        /// </summary>
        public int SecondPaneCurrentSelection { get; internal set; } = 1;
        /// <summary>
        /// Current status
        /// </summary>
        public string Status { get; internal set; } = "";
        /// <summary>
        /// Current pane
        /// </summary>
        public int CurrentPane { get; internal set; } = 1;
        /// <summary>
        /// Current info line index
        /// </summary>
        public int CurrentInfoLine { get; internal set; } = 0;
        /// <summary>
        /// Interactive TUI settings
        /// </summary>
        public InteractiveTuiSettings Settings { get; set; } = InteractiveTuiSettings.GlobalSettings;
        /// <summary>
        /// All key bindings for your interactive user interface
        /// </summary>
        public List<InteractiveTuiBinding<TPrimary, TSecondary>> Bindings { get; internal set; } = [];

        /// <inheritdoc/>
        public virtual bool SecondPaneInteractable => false;
        /// <inheritdoc/>
        public virtual int RefreshInterval => 0;
        /// <inheritdoc/>
        public virtual bool AcceptsEmptyData => false;

        /// <inheritdoc/>
        public virtual IEnumerable<TPrimary> PrimaryDataSource => [];
        /// <inheritdoc/>
        public virtual IEnumerable<TSecondary> SecondaryDataSource => [];

        /// <inheritdoc/>
        public virtual string GetEntryFromItem(TPrimary item) =>
            item is not null ? item.ToString() : "";

        /// <inheritdoc/>
        public virtual string GetInfoFromItem(TPrimary item) =>
            item is not null ? "No info." : "";

        /// <inheritdoc/>
        public virtual string GetStatusFromItem(TPrimary item) =>
            !string.IsNullOrEmpty(GetEntryFromItem(item)) ? GetEntryFromItem(item) : "No status";

        /// <inheritdoc/>
        public virtual string GetEntryFromItemSecondary(TSecondary item) =>
            item is not null ? item.ToString() : "";

        /// <inheritdoc/>
        public virtual string GetInfoFromItemSecondary(TSecondary item) =>
            item is not null ? "No info." : "";

        /// <inheritdoc/>
        public virtual string GetStatusFromItemSecondary(TSecondary item) =>
            !string.IsNullOrEmpty(GetEntryFromItemSecondary(item)) ? GetEntryFromItemSecondary(item) : "No status";

        /// <inheritdoc/>
        public virtual void HandleExit() { }
    }
}
