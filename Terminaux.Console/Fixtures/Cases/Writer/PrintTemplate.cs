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

using Terminaux.Colors;
using Terminaux.Colors.Templates;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintTemplate : IFixture
    {
        public string FixtureID => "PrintTemplate";
        public void RunFixture()
        {
            string name = "template";

            // Default template
            TextWriterColor.WriteColor("[Default - {0}] Hello world!", true, TemplateTools.GetColor(PredefinedComponentType.Text), TemplateTools.Exists(name));

            // Custom template
            string templateJson =
                $$"""
                {
                    "Name": "{{name}}",
                    "Components": {
                        "Text": "#876543",
                        "Component": "#345678"
                    }
                }
                """;
            var template = TemplateTools.GetTemplateFromJson(templateJson);
            TemplateTools.RegisterTemplate(template);
            TextWriterColor.WriteColor("[Before - {0}] Hello world!", true, TemplateTools.GetColor(PredefinedComponentType.Text), TemplateTools.Exists(name));
            TemplateTools.SetDefaultTemplate(name);
            TextWriterColor.WriteColor("[After - Text] Hello world!", true, TemplateTools.GetColor(PredefinedComponentType.Text));
            TextWriterColor.WriteColor("[After - Component] Hello world!\n", true, TemplateTools.GetColor("Component"));
            TemplateTools.SetColor("Component", new Color(86));
            TextWriterColor.WriteColor("[After - Component] Hello world!\n", true, TemplateTools.GetColor("Component"));
            TextWriterColor.Write(TemplateTools.GetTemplateToJson());
            TemplateTools.ResetDefaultTemplate();
            TextWriterColor.WriteColor("\n[Reset - {0}] Hello world!", true, TemplateTools.GetColor(PredefinedComponentType.Text), TemplateTools.Exists(name));
            TemplateTools.UnregisterTemplate(name);
            TextWriterColor.WriteColor("[Reset - {0}] Hello world!\n", true, TemplateTools.GetColor(PredefinedComponentType.Text), TemplateTools.Exists(name));
            TextWriterColor.Write(TemplateTools.GetTemplateToJson());
        }
    }
}
