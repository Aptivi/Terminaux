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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using System.Collections.Generic;
using System.Reflection;

namespace Terminaux.Inputs.Interactive.Selectors
{
    /// <summary>
    /// Spinner selector
    /// </summary>
    public class SpinnerSelectorTui : BaseInteractiveTui<(string, Spinner)>, IInteractiveTui<(string, Spinner)>
    {
        internal readonly List<(string, Spinner)> firstPaneListing = [];
        internal string spinnerToFind = nameof(BuiltinSpinners.SpinMore);

        /// <inheritdoc/>
        public override IEnumerable<(string, Spinner)> PrimaryDataSource
        {
            get
            {
                if (firstPaneListing.Count > 0)
                    return firstPaneListing;
                var builtinSpinners = typeof(BuiltinSpinners).GetProperties();
                bool foundSpinner = false;
                int spinnerIndex = 0;
                for (int i = 0; i < builtinSpinners.Length; i++)
                {
                    PropertyInfo spinnerProp = builtinSpinners[i];
                    var spinner = spinnerProp.GetGetMethod()?.Invoke(null, null);
                    if (spinner is Spinner finalSpinner)
                    {
                        finalSpinner.UseColors = false;
                        firstPaneListing.Add((spinnerProp.Name, finalSpinner));
                        if (spinnerProp.Name == spinnerToFind)
                        {
                            foundSpinner = true;
                            spinnerIndex = i;
                        }
                    }
                }
                if (foundSpinner)
                    InteractiveTuiTools.SelectionMovement(this, spinnerIndex + 1);
                return firstPaneListing;
            }
        }

        /// <inheritdoc/>
        public override int RefreshInterval =>
            100;

        /// <inheritdoc/>
        public override string GetStatusFromItem((string, Spinner) item) =>
            item.Item1;

        /// <inheritdoc/>
        public override string GetEntryFromItem((string, Spinner) item) =>
            item.Item2.Render();

        /// <inheritdoc/>
        public override string GetInfoFromItem((string, Spinner) item) =>
            item.Item1 + ": " + item.Item2.Peek();
    }
}
