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

using System;
using System.Linq;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestInputInfoBoxMultiInputLong : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            var modules = new InputModule[]
            {
                new TextBoxModule()
                {
                    Name = "Name",
                    Description = "Write the name of this person",
                },
                new TextBoxModule()
                {
                    Name = "E-mail address",
                    Description = "Write the e-mail address of this person",
                },
                new TextBoxModule()
                {
                    Name = "E-mail business address",
                    Description = "Write the e-mail address of this person's business",
                },
                new TextBoxModule()
                {
                    Name = "Gender",
                    Description = "Is this person a male, a female, or some other gender?",
                },
                new DateBoxModule()
                {
                    Name = "Birthdate",
                    Description = "When is this person's birthday?",
                },
                new TextBoxModule()
                {
                    Name = "Phone number",
                    Description = "What phone number do you use to contact them?",
                },
                new TextBoxModule()
                {
                    Name = "Location",
                    Description = "In what location are they currently found in?",
                },
                new TextBoxModule()
                {
                    Name = "Business",
                    Description = "Write the name of the business that they're currently working on",
                },
                new TextBoxModule()
                {
                    Name = "Website",
                    Description = "Write the URL of the website that points to the personal blog or website",
                },
                new TextBoxModule()
                {
                    Name = "Interests",
                    Description = "Write this person's interests",
                },
                new TextBoxModule()
                {
                    Name = "Hobbies",
                    Description = "Write this person's hobbies",
                },
                new TextBoxModule()
                {
                    Name = "Experiences",
                    Description = "Write this person's experiences",
                },
                new DateBoxModule()
                {
                    Name = "Registration Date",
                    Description = "Write this person's registration date",
                    Value = DateTimeOffset.Now,
                },
                new TimeBoxModule()
                {
                    Name = "Registration Time",
                    Description = "Write this person's registration time",
                    Value = DateTimeOffset.Now,
                },
            };
            InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, "Write details of this person. Please provide the correct information so that we can process this person and add them to the list of approved people. Any incorrect information may cause your request to be rejected.", new InfoBoxSettings()
            {
                Title = nameof(TestInputInfoBoxMultiInputLong)
            });
            string[] rendered = [.. modules.Select((im) => im.Value?.ToString() ?? "")];
            TextWriterWhereColor.WriteWhere("  - " + string.Join("\n  - ", rendered), 0, 0);
            Input.EnableMouse = false;
        }
    }
}
