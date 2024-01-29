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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terminaux.Base.Resources;
using Terminaux.Colors.Models;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data
    /// </summary>
    [DebuggerDisplay("{Name} [{ColorId}, {HexString}]")]
    public class ConsoleColorData : IEquatable<ConsoleColorData>
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

        /// <summary>
        /// Gets a color data instance that matches 
        /// </summary>
        /// <param name="color">Color to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(Color color) =>
            MatchColorData(color.RGB);

        /// <summary>
        /// Gets a color data instance that matches 
        /// </summary>
        /// <param name="rgb">Color to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(RedGreenBlue rgb) =>
            MatchColorData(rgb.R, rgb.G, rgb.B);

        /// <summary>
        /// Gets a color data instance that matches 
        /// </summary>
        /// <param name="r">Red color level to match</param>
        /// <param name="g">Green color level to match</param>
        /// <param name="b">Blue color level to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(int r, int g, int b)
        {
            var instances = GetColorData();

            // Get an instance that matches the conditions
            var instance = instances.FirstOrDefault((data) =>
                data.RGB.R == r &&
                data.RGB.G == g &&
                data.RGB.B == b
            );
            return instance;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as ConsoleColorData);

        /// <inheritdoc/>
        public bool Equals(ConsoleColorData other) =>
            other is not null &&
            ColorId == other.ColorId;

        /// <inheritdoc/>
        public override int GetHashCode() =>
            -1308032243 + ColorId.GetHashCode();

        /// <inheritdoc/>
        public static bool operator ==(ConsoleColorData left, ConsoleColorData right) =>
            EqualityComparer<ConsoleColorData>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(ConsoleColorData left, ConsoleColorData right) =>
            !(left == right);
    }
}
