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

using System.Collections.Generic;
using Terminaux.Base.Buffered;
using Terminaux.Extensions.Enumerations;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// A base class for your interactive user interface for terminal apps
    /// </summary>
    public class BaseInteractiveTui<T> : IInteractiveTui<T>
    {
        internal static List<BaseInteractiveTui<T>> instances = [];
        internal Dictionary<string, ScreenPart> trackedParts = [];
        internal Screen? screen;
        internal bool isExiting = false;

        /// <inheritdoc/>
        public virtual InteractiveTuiBinding[] Bindings { get; } = [];
        /// <inheritdoc/>
        public virtual bool SecondPaneInteractable => false;
        /// <inheritdoc/>
        public virtual int RefreshInterval => 0;
        /// <inheritdoc/>
        public virtual bool AcceptsEmptyData => false;

        /// <inheritdoc/>
        public virtual IEnumerable<T> PrimaryDataSource => [];
        /// <inheritdoc/>
        public virtual IEnumerable<T> SecondaryDataSource => [];

        /// <summary>
        /// Data source for the current pane
        /// </summary>
        public IEnumerable<T> DataSource =>
            InteractiveTuiStatus.CurrentPane == 2 ? SecondaryDataSource : PrimaryDataSource;

        /// <summary>
        /// The interactive TUI instance
        /// </summary>
        public static BaseInteractiveTui<T>? Instance =>
            instances.Count > 0 ?
            instances[instances.Count - 1] :
            null;

        /// <summary>
        /// The screen instance for this interactive TUI
        /// </summary>
        public Screen? Screen =>
            screen;

        /// <inheritdoc/>
        public virtual string GetEntryFromItem(T item) =>
            item is not null ? item.ToString() : "";

        /// <inheritdoc/>
        public virtual string GetInfoFromItem(T item) =>
            item is not null ? "No info." : "";

        /// <inheritdoc/>
        public virtual string GetStatusFromItem(T item) =>
            !string.IsNullOrEmpty(GetEntryFromItem(item)) ? GetEntryFromItem(item) : "No status";

        /// <inheritdoc/>
        public virtual void HandleExit() { }

        /// <summary>
        /// Goes down to the last element upon overflow (caused by remove operation, ...). This applies to the first and the second pane.
        /// </summary>
        public void LastOnOverflow()
        {
            int primaryCount = PrimaryDataSource.Length();
            int secondaryCount = SecondaryDataSource.Length();
            if (InteractiveTuiStatus.FirstPaneCurrentSelection > primaryCount)
                InteractiveTuiStatus.FirstPaneCurrentSelection = primaryCount;
            if (InteractiveTuiStatus.SecondPaneCurrentSelection > secondaryCount)
                InteractiveTuiStatus.SecondPaneCurrentSelection = secondaryCount;
        }

        /// <summary>
        /// Goes up to the first element upon underflow (caused by remove operation, ...). This applies to the first and the second pane.
        /// </summary>
        public void FirstOnUnderflow()
        {
            int primaryCount = PrimaryDataSource.Length();
            int secondaryCount = SecondaryDataSource.Length();
            if (InteractiveTuiStatus.FirstPaneCurrentSelection <= 0 && primaryCount > 0)
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
            if (InteractiveTuiStatus.SecondPaneCurrentSelection <= 0 && secondaryCount > 0)
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
        }
    }
}
