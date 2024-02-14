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
using System.Linq;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Terminaux.Colors.Templates
{
    /// <summary>
    /// Template tools class to manage templates
    /// </summary>
    public static class TemplateTools
    {
        private static string defaultTemplate = "Default";
        private static readonly TemplateInfo[] baseTemplates =
        [
            new("Default", new Dictionary<string, Color>()
            {
                { $"{PredefinedComponentType.Text}", ConsoleColors.White }
            })
        ];
        private static readonly List<TemplateInfo> customTemplates = [ ];

        /// <summary>
        /// All installed templates
        /// </summary>
        public static TemplateInfo[] Templates =>
            baseTemplates.Union(customTemplates).ToArray();

        /// <summary>
        /// All installed template names
        /// </summary>
        public static string[] TemplateNames =>
            Templates.Select((ti) => ti.Name).ToArray();

        /// <summary>
        /// Checks to see if a template exists or not
        /// </summary>
        /// <param name="template">A specific template to check</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool Exists(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new TerminauxException("No name of the template provided.");
            return TemplateNames.Contains(template);
        }

        /// <summary>
        /// Checks to see if a template is a built-in or not
        /// </summary>
        /// <param name="template">A specific template to check</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool ExistsBuiltin(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new TerminauxException("No name of the template provided.");
            return baseTemplates.Any((templateInfo) => templateInfo.Name == template);
        }

        /// <summary>
        /// Sets the default template
        /// </summary>
        /// <param name="template">Template name to set the default template to</param>
        /// <exception cref="TerminauxException"></exception>
        public static void SetDefaultTemplate(string template)
        {
            // Check to see if we have this template
            if (!Exists(template))
                throw new TerminauxException("Can't find template {0}", template);

            // Now, set the default template
            defaultTemplate = template;
        }

        /// <summary>
        /// Resets the default template
        /// </summary>
        /// <exception cref="TerminauxException"></exception>
        public static void ResetDefaultTemplate() =>
            SetDefaultTemplate("Default");

        /// <summary>
        /// Gets the default template
        /// </summary>
        /// <exception cref="TerminauxException"></exception>
        public static TemplateInfo GetTemplate() =>
            GetTemplate(defaultTemplate);

        /// <summary>
        /// Gets the template
        /// </summary>
        /// <param name="template">Template name</param>
        /// <exception cref="TerminauxException"></exception>
        public static TemplateInfo GetTemplate(string template)
        {
            // Check to see if we have this template
            if (!Exists(template))
                throw new TerminauxException("Can't find template {0}", template);

            // Now, get the template
            int idx = GetTemplateIndexFrom(template);
            return Templates[idx];
        }

        /// <summary>
        /// Gets the default template
        /// </summary>
        /// <param name="componentName">Component name</param>
        /// <exception cref="TerminauxException"></exception>
        public static Color GetColor(PredefinedComponentType componentName) =>
            GetColor(defaultTemplate, componentName);

        /// <summary>
        /// Gets the template
        /// </summary>
        /// <param name="template">Template name</param>
        /// <param name="componentName">Component name</param>
        /// <exception cref="TerminauxException"></exception>
        public static Color GetColor(string template, PredefinedComponentType componentName) =>
            GetColor(template, $"{componentName}");

        /// <summary>
        /// Gets the default template
        /// </summary>
        /// <param name="componentName">Component name</param>
        /// <exception cref="TerminauxException"></exception>
        public static Color GetColor(string componentName) =>
            GetColor(defaultTemplate, componentName);

        /// <summary>
        /// Gets the template
        /// </summary>
        /// <param name="template">Template name</param>
        /// <param name="componentName">Component name</param>
        /// <exception cref="TerminauxException"></exception>
        public static Color GetColor(string template, string componentName)
        {
            // Check to see if we have this template
            if (!Exists(template))
                throw new TerminauxException("Can't find template {0}", template);

            // Now, get the template
            var templateInfo = GetTemplate(template);
            if (!templateInfo.Components.TryGetValue(componentName, out Color componentColor))
                throw new TerminauxException("Can't find component {0} in template {1}", componentName, template);
            return componentColor;
        }

        /// <summary>
        /// Registers a template
        /// </summary>
        /// <param name="template">Template information</param>
        public static void RegisterTemplate(TemplateInfo template)
        {
            // Check the template
            if (template is null)
                throw new TerminauxException("Invalid template.");
            if (string.IsNullOrWhiteSpace(template.Name))
                throw new TerminauxException("Template has no name.");
            if (Exists(template.Name))
                throw new TerminauxException("Template already exists.");
            if (template.Components is null || template.Components.Count == 0)
                throw new TerminauxException("Template has no components.");
            if (!TemplateComponentValidation(template))
                throw new TerminauxException("Template has no pre-defined components that must be defined ({0}).", string.Join(", ", Enum.GetNames(typeof(PredefinedComponentType))));

            // Now, actually register the template
            customTemplates.Add(template);
        }

        /// <summary>
        /// Unregisters a template
        /// </summary>
        /// <param name="template">Template information</param>
        public static void UnregisterTemplate(string template)
        {
            // Check to see if we have this template
            if (!Exists(template))
                throw new TerminauxException("Can't find template {0}", template);
            if (ExistsBuiltin(template))
                throw new TerminauxException("Can't remove built-in template {0}", template);

            // Now, remove the template
            var templateInfo = GetTemplate(template);
            customTemplates.Remove(templateInfo);
        }

        /// <summary>
        /// Gets a JSON representation of the template
        /// </summary>
        /// <returns>A string containing the JSON representation of a template</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string GetTemplateToJson() =>
            GetTemplateToJson(defaultTemplate);

        /// <summary>
        /// Gets a JSON representation of the template
        /// </summary>
        /// <param name="template">Template name to save to JSON</param>
        /// <returns>A string containing the JSON representation of a template</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string GetTemplateToJson(string template)
        {
            // Check to see if we have this template
            if (!Exists(template))
                throw new TerminauxException("Can't find template {0}", template);

            // Now, get the template JSON
            var templateInfo = GetTemplate(template);
            return JsonConvert.SerializeObject(templateInfo, Formatting.Indented);
        }

        /// <summary>
        /// Gets a template from its JSON representation
        /// </summary>
        /// <param name="json">Template JSON contents</param>
        /// <returns>A template info containing JSON representation</returns>
        /// <exception cref="TerminauxException"></exception>
        public static TemplateInfo? GetTemplateFromJson(string json) =>
            JsonConvert.DeserializeObject<TemplateInfo>(json);

        private static int GetTemplateIndexFrom(string template) =>
            TemplateNames.Select((name, idx) => (name, idx)).Where((tuple) => tuple.name == template).First().idx;

        private static bool TemplateComponentValidation(TemplateInfo template)
        {
            var names = Enum.GetNames(typeof(PredefinedComponentType));
            bool valid = true;
            foreach (string name in names)
            {
                if (!template.Components.ContainsKey(name))
                    valid = false;
            }
            return valid;
        }
    }
}
