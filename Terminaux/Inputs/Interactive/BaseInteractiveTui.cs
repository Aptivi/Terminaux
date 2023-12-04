
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

namespace Terminaux.Reader.Inputs.Interactive
{
    /// <summary>
    /// A base class for your interactive user interface for terminal apps
    /// </summary>
    public class BaseInteractiveTui : IInteractiveTui
    {
        internal bool isExiting = false;

        /// <inheritdoc/>
        public virtual List<InteractiveTuiBinding> Bindings { get; set; }
        /// <inheritdoc/>
        public virtual bool SecondPaneInteractable => false;
        /// <inheritdoc/>
        public virtual int RefreshInterval => 0;
        /// <inheritdoc/>
        public virtual bool AcceptsEmptyData => false;
        /// <inheritdoc/>
        public virtual bool FastRefresh => true;

        /// <inheritdoc/>
        public virtual IEnumerable PrimaryDataSource => Array.Empty<string>();
        /// <inheritdoc/>
        public virtual IEnumerable SecondaryDataSource => Array.Empty<string>();

        /// <inheritdoc/>
        public virtual string GetEntryFromItem(object item) =>
            item is not null ? item.ToString() : "???";

        /// <inheritdoc/>
        public virtual string GetInfoFromItem(object item) =>
            item is not null ? "No info." : "???";

        /// <inheritdoc/>
        public virtual void HandleExit() { }

        /// <inheritdoc/>
        public virtual void RenderStatus(object item) { }

        /// <inheritdoc/>
        public virtual void LastOnOverflow()
        {
            int primaryCount = InteractiveTuiTools.CountElements(PrimaryDataSource);
            int secondaryCount = InteractiveTuiTools.CountElements(SecondaryDataSource);
            if (InteractiveTuiStatus.FirstPaneCurrentSelection > primaryCount)
                InteractiveTuiStatus.FirstPaneCurrentSelection = primaryCount;
            if (InteractiveTuiStatus.SecondPaneCurrentSelection > secondaryCount)
                InteractiveTuiStatus.SecondPaneCurrentSelection = secondaryCount;
        }
    }
}
