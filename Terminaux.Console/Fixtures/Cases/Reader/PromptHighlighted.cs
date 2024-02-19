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

using Terminaux.Reader;
using Terminaux.Reader.Highlighting;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Reader
{
    internal class PromptHighlighted : IFixture
    {
        public string FixtureID => "PromptHighlighted";

        public void RunFixture()
        {
            // No highlight
            TextWriterColor.Write("Using no highlight.");
            TermReader.Read("  >> ");

            // Built-in highlight
            TextWriterColor.Write("Using command highlight.");
            var settings = new TermReaderSettings()
            {
                SyntaxHighlighterEnabled = true,
                SyntaxHighlighter = SyntaxHighlightingTools.GetHighlighter("Command"),
            };
            TermReader.Read("D >> ", settings);

            // Custom highlight
            string highlighterJson =
                $$"""
                {
                    "Name": "custom",
                    "Components": {
                        "FirstWord": {
                            "ComponentMatch": "/(?:^|(?:[.!?]\\s))(\\w+)/",
                            "ComponentForegroundColor": "#00FF00",
                            "ComponentBackgroundColor": "#000000",
                        }
                    }
                }
                """;
            var template = SyntaxHighlightingTools.GetHighlighterFromJson(highlighterJson);
            SyntaxHighlightingTools.RegisterHighlighter(template);
            var settingsCustom = new TermReaderSettings()
            {
                SyntaxHighlighterEnabled = true,
                SyntaxHighlighter = template,
            };
            TermReader.Read("E >> ", settingsCustom);
            SyntaxHighlightingTools.UnregisterHighlighter("custom");

            // Custom highlight (more than one component)
            string highlighterDoubleJson =
                $$"""
                {
                    "Name": "custom",
                    "Components": {
                        "FirstWord": {
                            "ComponentMatch": "/(?:^|(?:[.!?]\\s))(\\w+)/",
                            "ComponentForegroundColor": "#00FF00",
                            "ComponentBackgroundColor": "#000000",
                            "UseBackgroundColor": false,
                            "UseForegroundColor": true,
                        },
                        "Spaces": {
                            "ComponentMatch": "/[ ]/",
                            "ComponentForegroundColor": "#000000",
                            "ComponentBackgroundColor": "#00FF00",
                            "UseBackgroundColor": true,
                            "UseForegroundColor": false,
                        },
                    }
                }
                """;
            var templateDouble = SyntaxHighlightingTools.GetHighlighterFromJson(highlighterDoubleJson);
            SyntaxHighlightingTools.RegisterHighlighter(templateDouble);
            var settingsDoubleCustom = new TermReaderSettings()
            {
                SyntaxHighlighterEnabled = true,
                SyntaxHighlighter = templateDouble,
            };
            TermReader.Read("O >> ", settingsDoubleCustom);
            SyntaxHighlightingTools.UnregisterHighlighter("custom");
        }
    }
}
