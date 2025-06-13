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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data serializer
    /// </summary>
    public class ConsoleColorDataSerializer : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(ConsoleColorData);

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            // Load the token
            var token = JToken.Load(reader);
            if (token is null)
                return null;

            // Now, get the three essential properties
            int colorId = (int?)token["colorId"] ?? 0;
            string hexString = (string?)token["hexString"] ?? "#000000";
            string name = (string?)token["name"] ?? "Black";

            // Now, get the RGB values
            var rgbToken = token["rgb"];
            if (rgbToken == null)
                return null;
            int r = (int?)rgbToken["r"] ?? 0;
            int g = (int?)rgbToken["g"] ?? 0;
            int b = (int?)rgbToken["b"] ?? 0;

            // Finally, install the data instance
            ConsoleColorData color = new(hexString, r, g, b, name, colorId);
            return color;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var color = value as ConsoleColorData ??
                throw new Exception("Can't get color data.");
            serializer.Serialize(writer, $"{color.RGB.R};{color.RGB.G};{color.RGB.B}");
        }
    }
}
