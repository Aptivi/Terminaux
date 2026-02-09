//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using Spectre.Console;
using Colorimetry.Data;
using Terminaux.Spectre;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using TTextPath = Terminaux.Writer.CyclicWriters.Graphical.TextPath;

namespace Terminaux.Console.Fixtures.Cases.Spectre
{
    internal class TextPathTranslation : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Spectre;

        public void RunFixture()
        {
            var path = new TTextPath()
            {
                PathText = @"C:\very\long\path\to\somewhere\where\you\can\find\our\work\here.txt",
                ForegroundColor = ConsoleColors.Green,
                LastPathColor = ConsoleColors.Blue,
                SeparatorColor = ConsoleColors.Yellow,
                RootDriveColor = ConsoleColors.Red,
                UseColors = true,
                Settings = new() { Alignment = TextAlignment.Left },
                Left = 4,
                Top = 4,
                Width = 40,
            };

            // Write our text path
            TextWriterColor.Write("Terminaux's TextPath:");
            TextWriterColor.Write(path.Render());

            // Now, convert them to Spectre's text path
            var spectreTextPath = TranslationTools.GetTextPath(path);

            // Write Spectre's text path
            TextWriterRaw.Write();
            TextWriterColor.Write("Spectre.Console's TextPath:");
            AnsiConsole.Write(spectreTextPath);
            AnsiConsole.WriteLine();
        }
    }
}
