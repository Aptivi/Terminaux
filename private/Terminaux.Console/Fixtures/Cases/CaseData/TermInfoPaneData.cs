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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Terminaux.Base.TermInfo;
using Terminaux.Inputs;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class TermInfoPaneData : BaseInteractiveTui<TermInfoDesc>, IInteractiveTui<TermInfoDesc>
    {
        private static bool loaded = false;
        private static readonly List<TermInfoDesc> descs = [];

        public override InteractiveTuiBinding[] Bindings =>
        [
            new InteractiveTuiBinding("Custom...", ConsoleKey.C, (_, _) => ShowCustomInfo())
        ];

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
            return ShowDesc(selected);
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
                if (stream is null)
                    continue;
                var desc = TermInfoDesc.Load(stream);
                if (desc is not null)
                    descs.Add(desc);
            }
            loaded = true;
            return [.. descs];
        }

        private void ShowCustomInfo()
        {
            string[] names = TermInfoDesc.GetBuiltins();
            int idx = InfoBoxSelectionColor.WriteInfoBoxSelection("TermInfo", names.Select((name, idx) => new InputChoiceInfo($"{idx + 1}", name)).ToArray(), "Write the terminal info name");
            if (idx < 0)
                return;
            string name = names[idx];
            if (!TermInfoDesc.TryLoad(name, out TermInfoDesc? desc))
                return;
            string descString = ShowDesc(desc);
            InfoBoxColor.WriteInfoBox(name, descString);
        }

        private string ShowDesc(TermInfoDesc? desc)
        {
            // Populate the info
            if (desc is null)
                return "";
            var builder = new StringBuilder();
            builder.AppendLine(
                 $$"""
                 Name: {{desc.Names[0]}}
                 Description: {{desc.Names[1]}}

                 Maximum colors: {{desc.MaxColors}}
                 Extended capabilities: {{desc.Extended.Count}}
                 """
            );

            // Detect the extended attributes
            if (desc.Extended.Count > 0)
            {
                var namesBool = desc.Extended.GetNames(TermInfoCapsKind.Boolean);
                var namesNum = desc.Extended.GetNames(TermInfoCapsKind.Num);
                var namesString = desc.Extended.GetNames(TermInfoCapsKind.String);
                builder.AppendLine();
                builder.AppendLine($"Boolean extended capabilities: {string.Join(", ", namesBool)}");
                builder.AppendLine($"Numeric extended capabilities: {string.Join(", ", namesNum)}");
                builder.AppendLine($"String extended capabilities: {string.Join(", ", namesString)}");
            }
            return $"{builder}";
        }
    }
}
