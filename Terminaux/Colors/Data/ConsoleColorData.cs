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
using System.Diagnostics;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data
    /// </summary>
    [DebuggerDisplay("{Name} [{ColorId}, {HexString}]")]
    public partial class ConsoleColorData : IEquatable<ConsoleColorData>
    {
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

            [JsonConstructor]
            private Rgb()
            { }

            internal Rgb(int r, int g, int b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
            }
        }

        /// <summary>
        /// The hue, saturation, and luminance values
        /// </summary>
        [DebuggerDisplay("HSL = {H}, {S}, {L}")]
        public class Hsl
        {
            [JsonProperty(nameof(h))]
            private readonly double h = 0f;
            [JsonProperty(nameof(s))]
            private readonly int s = 0;
            [JsonProperty(nameof(l))]
            private readonly int l = 0;

            /// <summary>
            /// The hue level
            /// </summary>
            [JsonIgnore]
            public double H =>
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

            [JsonConstructor]
            private Hsl()
            { }

            internal Hsl(double h, int s, int l)
            {
                this.h = h;
                this.s = s;
                this.l = l;
            }
        }

        [JsonConstructor]
        private ConsoleColorData()
        { }

        internal ConsoleColorData(string hexString, int r, int g, int b, double h, int s, int l, string name, int colorId)
        {
            this.hexString = hexString;
            rgb = new(r, g, b);
            hsl = new(h, s, l);
            this.name = name;
            this.colorId = colorId;
        }
    }
}
