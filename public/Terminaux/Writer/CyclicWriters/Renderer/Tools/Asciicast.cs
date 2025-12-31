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

using Newtonsoft.Json;
using System.Collections.Generic;

#pragma warning disable CS0649

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Asciicast v1 class containing info for recorded terminal screencast
    /// </summary>
    public class Asciicast
    {
        [JsonProperty("version")]
        private readonly int version;
        [JsonProperty("width")]
        private readonly int width;
        [JsonProperty("height")]
        private readonly int height;

        /// <summary>
        /// Asciicast version (usually 1)
        /// </summary>
        [JsonIgnore]
        public int Version =>
            version;

        /// <summary>
        /// Initial console width
        /// </summary>
        [JsonIgnore]
        public int Width =>
            width;

        /// <summary>
        /// Initial console height
        /// </summary>
        [JsonIgnore]
        public int Height =>
            height;

        /// <summary>
        /// Delay and Data
        /// </summary>
        [JsonIgnore]
        public virtual List<(double, string, string)> StdOutData =>
            [];
    }
}
