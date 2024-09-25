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
using System.Linq;
using Terminaux.Base;
using Terminaux.Graphics.NerdFonts;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class CliInfoPaneNfTestData : BaseInteractiveTui<string, string>, IInteractiveTui<string, string>
    {
        internal static string[] types = Enum.GetNames(typeof(NerdFontsTypes));

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            types;

        /// <inheritdoc/>
        public override IEnumerable<string> SecondaryDataSource
        {
            get
            {
                string typeName = types[FirstPaneCurrentSelection - 1];
                if (!Enum.TryParse<NerdFontsTypes>(typeName, out var type))
                    throw new TerminauxException($"Type name {typeName} is invalid.");
                var nerdFontNames = NerdFontsTools.GetNerdFontCharNames(type);
                return nerdFontNames.Select((nf) => $"{NerdFontsTools.GetNerdFontChar(type, nf)} {nf}");
            }
        }

        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            string.IsNullOrEmpty(item) ? "No info." : item;

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary(string item) =>
            string.IsNullOrEmpty(item) ? "No info." : item;

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary(string item) =>
            item;

        internal void Show()
        {
            string typeName = types[FirstPaneCurrentSelection - 1];
            if (!Enum.TryParse<NerdFontsTypes>(typeName, out var type))
                throw new TerminauxException($"Type name {typeName} is invalid.");
            string charName = SecondaryDataSource.ElementAt(SecondPaneCurrentSelection - 1).Substring(2).Trim();
            string nerdFontChar = NerdFontsTools.GetNerdFontChar(type, charName);
            InfoBoxModalColor.WriteInfoBoxModal("NF Character Info",
                $"WARNING: You must use a font that supports NF glyphs. Otherwise, you'll see the \"missing character\" symbol.\n" +
                $"\n" +
                $"NF character name: {charName} ({typeName})\n" +
                $"Surrogate pair: {(nerdFontChar.Length == 2 ? "Yes" : "No")}\n" +
                $"NF character: {nerdFontChar}");
        }
    }
}
