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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Terminaux.Base;
using Textify.General;

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
        /// Template components
        /// </summary>
        [JsonIgnore]
        public ReadOnlyDictionary<string, Color> Components =>
            new(components);

        /// <summary>
        /// Checks to see if a component exists
        /// </summary>
        /// <param name="componentName">A component name</param>
        /// <returns>True if a component exists. Otherwise, false.</returns>
        public bool ComponentExists(string componentName) =>
            components.ContainsKey(componentName);

        /// <summary>
        /// Checks to see if a component is predefined
        /// </summary>
        /// <param name="componentName">A component name</param>
        /// <returns>True if a component is predefined. Otherwise, false.</returns>
        public bool ComponentPredefined(string componentName)
        {
            var names = Enum.GetNames(typeof(PredefinedComponentType));
            return names.Contains(componentName);
        }

        /// <summary>
        /// Adds a component to the list of components
        /// </summary>
        /// <param name="componentName">Component name to add</param>
        /// <param name="color">Color to associate this component with</param>
        public void AddComponent(string componentName, Color? color)
        {
            if (string.IsNullOrEmpty(componentName))
                throw new TerminauxException("No component name specified.");
            if (color is null)
                throw new TerminauxException("No color specified for component {0}.".FormatString(componentName));
            if (ComponentExists(componentName))
                throw new TerminauxException("Component {0} already exists.".FormatString(componentName));

            // Now, add a component
            components.Add(componentName, color);
        }

        /// <summary>
        /// Removes a component from the list of components
        /// </summary>
        /// <param name="componentName">Component name to remove</param>
        public void RemoveComponent(string componentName)
        {
            if (string.IsNullOrEmpty(componentName))
                throw new TerminauxException("No component name specified.");
            if (!ComponentExists(componentName))
                throw new TerminauxException("Component {0} doesn't exist.".FormatString(componentName));
            if (ComponentPredefined(componentName))
                throw new TerminauxException("Component {0} is predefined and thus cannot be removed.".FormatString(componentName));

            // Now, add a component
            components.Remove(componentName);
        }

        /// <summary>
        /// Edits a component
        /// </summary>
        /// <param name="componentName">Component name to edit</param>
        /// <param name="color">Color to associate this component with</param>
        public void EditComponent(string componentName, Color? color)
        {
            if (string.IsNullOrEmpty(componentName))
                throw new TerminauxException("No component name specified.");
            if (color is null)
                throw new TerminauxException("No color specified for component {0}.".FormatString(componentName));
            if (!ComponentExists(componentName))
                throw new TerminauxException("Component {0} doesn't exist.".FormatString(componentName));

            // Now, add a component
            components[componentName] = color;
        }

        internal bool TemplateComponentValidation()
        {
            var dict = Components.ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
            return TemplateComponentValidation(dict);
        }

        internal bool TemplateComponentValidation(Dictionary<string, Color> components)
        {
            var names = Enum.GetNames(typeof(PredefinedComponentType));
            bool valid = true;
            foreach (string name in names)
            {
                if (!components.ContainsKey(name))
                    valid = false;
            }
            return valid;
        }

        private Dictionary<string, Color> VerifyComponentsCtor(Dictionary<string, Color?> components)
        {
            Dictionary<string, Color> finalComponents = [];
            foreach (var kvp in components)
            {
                string name = kvp.Key;
                Color? color = kvp.Value;
                if (string.IsNullOrEmpty(name))
                    throw new TerminauxException("No component name specified.");
                if (color is null)
                    throw new TerminauxException("No color specified for component {0}.".FormatString(name));
                if (components.Count == 0)
                    throw new TerminauxException("Template has no components.");
                finalComponents.Add(name, color);
            }
            if (!TemplateComponentValidation(finalComponents))
                throw new TerminauxException("Template has no pre-defined components that must be defined ({0}).", string.Join(", ", Enum.GetNames(typeof(PredefinedComponentType))));
            return finalComponents;
        }

        [JsonConstructor]
        private TemplateInfo()
        { }

        /// <summary>
        /// Makes a new instance of the template information class
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="components">List of components in a dictionary</param>
        public TemplateInfo(string name, Dictionary<string, Color?>? components)
        {
            if (string.IsNullOrEmpty(name))
                throw new TerminauxException("Name of the template not provided");
            if (components is null)
                throw new TerminauxException("Components are not provided, but this list can be empty for dynamic addition.");
            var finalComponents = VerifyComponentsCtor(components);
            this.name = name;
            this.components = finalComponents;
        }
    }
}
