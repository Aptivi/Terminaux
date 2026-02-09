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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class BorderText : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            ConsoleColoring.LoadBack();
            TextWriterRaw.WritePlain("{0}{1}{2}{3}",
                new Border()
                {
                    Text = "Hello world!",
                    Left = 2,
                    Top = 1,
                    Width = 6,
                    Height = 2,
                    Color = new Color(ConsoleColors.Green),
                }.Render(),
                new Border()
                {
                    Text = "Hello world!",
                    Left = ConsoleWrapper.WindowWidth - 10,
                    Top = 1,
                    Width = 6,
                    Height = 2,
                    Color = new Color(ConsoleColors.Black),
                    BackgroundColor = new Color(ConsoleColors.Yellow),
                    TextColor = new Color(ConsoleColors.Black),
                }.Render(),
                new Border()
                {
                    Title = "Middle",
                    Text = "Hello world!",
                    Left = ConsoleWrapper.WindowWidth / 2 - 6,
                    Top = 1,
                    Width = 12,
                    Height = 1,
                    Color = new Color(ConsoleColors.Red),
                }.Render(),
                new Border()
                {
                    Title = "Center",
                    Text = "Hello world!",
                    Left = ConsoleWrapper.WindowWidth / 2 - 6,
                    Top = ConsoleWrapper.WindowHeight / 2 - 1,
                    Width = 12,
                    Height = 1,
                }.Render());
            TextWriterWhereColor.WriteWhere("If you can see these, it's a success!", 0, ConsoleWrapper.WindowHeight - 1);
        }
    }
}
