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

namespace Terminaux.Base.Extensions.Data
{
    /// <summary>
    /// Console synth info
    /// </summary>
    public class SynthInfo
    {
        /// <summary>
        /// Name of the console synth (usually a music name)
        /// </summary>
        [JsonProperty("name")]
        [JsonRequired]
        public string Name { get; set; } = "";

        /// <summary>
        /// Beep synth chapters
        /// </summary>
        [JsonProperty("chapters")]
        [JsonRequired]
        public Chapter[] Chapters { get; set; } = [];

        /// <summary>
        /// Chapter synthesis class
        /// </summary>
        public class Chapter
        {
            /// <summary>
            /// Name of the chapter
            /// </summary>
            [JsonProperty("name")]
            [JsonRequired]
            public string Name { get; set; } = "";

            /// <summary>
            /// List of synths in this format: <c>&lt;freq&gt; &lt;ms&gt;</c>
            /// </summary>
            [JsonProperty("synths")]
            [JsonRequired]
            public string[] Synths { get; set; } = [];

            internal Chapter()
            { }
        }

        internal SynthInfo()
        { }
    }
}
