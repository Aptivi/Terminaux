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
using Newtonsoft.Json.Linq;
using System;
using Terminaux.Base;
using Textify.General;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color serializer
    /// </summary>
    public class ColorSerializer : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Color);

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var result = JRaw.Create(reader).ToString().ReleaseDoubleQuotes();
            var color = new Color(result);
            return color;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var color = value as Color ??
                throw new TerminauxInternalException("Can't get color.");
            serializer.Serialize(writer, color.ToString());
        }
    }
}
