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

using Newtonsoft.Json;

namespace Terminaux.NerdFontGen.Decoy
{
    public class Icon
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("unicode")]
        public string Unicode { get; set; } = "";
    }

    public class NerdFontInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "";

        [JsonProperty("icons")]
        public Icon[] Icons { get; set; } = [];
    }


}
