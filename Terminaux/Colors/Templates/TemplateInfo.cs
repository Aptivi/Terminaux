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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Terminaux.Colors.Templates
{
    /// <summary>
    /// Template information containing colors set to each component of the template
    /// </summary>
    [DebuggerDisplay("{Name}: {Components.Count} components")]
    public class TemplateInfo
    {
        [JsonProperty(nameof(Name), Required = Required.Always)]
        private readonly string name = "";
        [JsonProperty(nameof(Components), Required = Required.Always)]
        private readonly Dictionary<string, Color> components = [];

        /// <summary>
        /// Template name
        /// </summary>
        [JsonIgnore]
        public string Name =>
            name;
        /// <summary>
        /// Template name
        /// </summary>
        [JsonIgnore]
        public ReadOnlyDictionary<string, Color> Components =>
            new(components);

        [JsonConstructor]
        private TemplateInfo()
        { }

        internal TemplateInfo(string name, Dictionary<string, Color> components)
        {
            this.name = name;
            this.components = components;
        }
    }
}
