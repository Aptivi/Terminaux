﻿//
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

#if !GENERATOR
using Terminaux.Colors.Models;
#endif

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data
    /// </summary>
    [JsonConverter(typeof(ConsoleColorDataSerializer))]
    [DebuggerDisplay("{Name} [{ColorId}, {HexString}, {GetOrderCode()}]")]
    public partial class ConsoleColorData : IEquatable<ConsoleColorData>
    {
        [JsonProperty(nameof(hexString))]
        private readonly string hexString = "";
        [JsonProperty(nameof(rgb))]
        private readonly (int r, int g, int b) rgb = (0, 0, 0);
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
        public
#if GENERATOR
            (int r, int g, int b) RGB =>
                rgb;
#else
            RedGreenBlue RGB =>
                new(rgb.r, rgb.g, rgb.b);
#endif

        /// <summary>
        /// The color name
        /// </summary>
        [JsonIgnore]
        public string Name =>
            name;

        [JsonConstructor]
        private ConsoleColorData()
        { }

        internal ConsoleColorData(string hexString, int r, int g, int b, string name, int colorId)
        {
            this.hexString = hexString;
            rgb = (r, g, b);
            this.name = name;
            this.colorId = colorId;
        }
    }
}
