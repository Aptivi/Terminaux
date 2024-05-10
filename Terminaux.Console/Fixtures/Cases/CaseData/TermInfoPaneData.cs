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
using System.Reflection;
using System.Text;
using Terminaux.Base.TermInfo;
using Terminaux.Inputs.Interactive;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class TermInfoPaneData : BaseInteractiveTui<TermInfoDesc>, IInteractiveTui<TermInfoDesc>
    {
        private static bool loaded = false;
        private static readonly List<TermInfoDesc> descs = [];

        /// <inheritdoc/>
        public override IEnumerable<TermInfoDesc> PrimaryDataSource =>
            GetDescs();

        /// <inheritdoc/>
        public override string GetInfoFromItem(TermInfoDesc item)
        {
            var selected = item;

            // Check to see if we're given the test info
            if (selected is null)
                InteractiveTuiStatus.Status = "No info.";
            else
                InteractiveTuiStatus.Status = $"{selected.Names[0]} - {selected.Names[1]}";

            // Now, populate the info
            var builder = new StringBuilder();
            builder.AppendLine(
                 $$"""
                 Name: {{selected.Names[0]}}
                 Description: {{selected.Names[1]}}

                 Maximum colors: {{selected.MaxColors}}
                 Extended capabilities: {{selected.Extended.Count}}
                 """   
            );
            if (selected.Extended.Count > 0)
            {
                var namesBool = selected.Extended.GetNames(TermInfoCapsKind.Boolean);
                var namesNum = selected.Extended.GetNames(TermInfoCapsKind.Num);
                var namesString = selected.Extended.GetNames(TermInfoCapsKind.String);
                builder.AppendLine();
                builder.AppendLine($"Boolean extended capabilities: {string.Join(", ", namesBool)}");
                builder.AppendLine($"Numeric extended capabilities: {string.Join(", ", namesNum)}");
                builder.AppendLine($"String extended capabilities: {string.Join(", ", namesString)}");
            }
            return $"{builder}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(TermInfoDesc item)
        {
            var selected = item;
            return selected.Names[0];
        }

        private TermInfoDesc[] GetDescs()
        {
            if (loaded)
                return [.. descs];
            var asm = Assembly.GetExecutingAssembly();
            var termInfoNames = asm.GetManifestResourceNames();
            foreach (string termInfoName in termInfoNames)
            {
                if (!termInfoName.StartsWith("Terminaux.Console.Assets.TermInfoData."))
                    continue;
                var stream = asm.GetManifestResourceStream(termInfoName);
                var desc = TermInfoDesc.Load(stream);
                if (desc is not null)
                    descs.Add(desc);
            }
            loaded = true;
            return [.. descs];
        }
    }
}
