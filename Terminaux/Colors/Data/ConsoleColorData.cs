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
using System.Diagnostics;
using Terminaux.Base.Resources;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data
    /// </summary>
    [DebuggerDisplay("{Name} [{ColorId}, {HexString}]")]
    public class ConsoleColorData
    {
        [JsonIgnore]
        private static ConsoleColorData[] instances = null;
        [JsonProperty(nameof(hexString))]
        private readonly string hexString = "";
        [JsonProperty(nameof(rgb))]
        private readonly Rgb rgb = default;
        [JsonProperty(nameof(hsl))]
        private readonly Hsl hsl = default;
        [JsonProperty(nameof(name))]
        private readonly string name = "";
        [JsonProperty(nameof(colorId))]
        private readonly int colorId = 0;

        /// <summary>
        /// The color ID
        /// </summary>
        [JsonIgnore]
        public int ColorId =>
            colorId;

        /// <summary>
        /// Hexadecimal representation of the color
        /// </summary>
        [JsonIgnore]
        public string HexString =>
            hexString;

        /// <summary>
        /// The RGB values
        /// </summary>
        [JsonIgnore]
        public Rgb RGB =>
            rgb;

        /// <summary>
        /// The HSL values
        /// </summary>
        [JsonIgnore]
        public Hsl HSL =>
            hsl;

        /// <summary>
        /// The color name
        /// </summary>
        [JsonIgnore]
        public string Name =>
            name;

        /// <summary>
        /// Decoy class for RGB
        /// </summary>
        [DebuggerDisplay("RGB = {R}, {G}, {B}")]
        public class Rgb
        {
            [JsonProperty(nameof(r))]
            private readonly int r = 0;
            [JsonProperty(nameof(g))]
            private readonly int g = 0;
            [JsonProperty(nameof(b))]
            private readonly int b = 0;

            /// <summary>
            /// Red color level
            /// </summary>
            [JsonIgnore]
            public int R =>
                r;

            /// <summary>
            /// Green color level
            /// </summary>
            [JsonIgnore]
            public int G =>
                g;

            /// <summary>
            /// Blue color level
            /// </summary>
            [JsonIgnore]
            public int B =>
                b;
        }

        /// <summary>
        /// The hue, saturation, and luminance values
        /// </summary>
        [DebuggerDisplay("HSL = {H}, {S}, {L}")]
        public class Hsl
        {
            [JsonProperty(nameof(h))]
            private readonly float h = 0f;
            [JsonProperty(nameof(s))]
            private readonly int s = 0;
            [JsonProperty(nameof(l))]
            private readonly int l = 0;

            /// <summary>
            /// The hue level
            /// </summary>
            [JsonIgnore]
            public float H =>
                h;

            /// <summary>
            /// The saturation level
            /// </summary>
            [JsonIgnore]
            public int S =>
                s;

            /// <summary>
            /// The lightness level
            /// </summary>
            [JsonIgnore]
            public int L =>
                l;
        }

        /// <summary>
        /// Gets the console colors data
        /// </summary>
        /// <returns></returns>
        public static ConsoleColorData[] GetColorData()
        {
            instances ??= JsonConvert.DeserializeObject<ConsoleColorData[]>(ConsoleResources.ConsoleColorsData);
            return instances;
        }
    }
}
