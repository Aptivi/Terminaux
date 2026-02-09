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

using Colorimetry.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintPaths : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var path1 = new TextPath()
            {
                PathText = @"C:\WINDOWS\System32\very\long\path\so\that\we\can\read-this.txt",
                Left = 4,
                Top = 2,
                Width = 30,
            };
            var path2 = new TextPath()
            {
                PathText = @"C:\WINDOWS\System32\taskmgr.exe",
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
            var path3 = new TextPath()
            {
                PathText = @"/etc/grub.d/40_custom",
                ForegroundColor = ConsoleColors.Green,
                LastPathColor = ConsoleColors.Blue,
                SeparatorColor = ConsoleColors.Yellow,
                RootDriveColor = ConsoleColors.Red,
                UseColors = true,
                Settings = new() { Alignment = TextAlignment.Middle },
                Left = 4,
                Top = 5,
                Width = 40,
            };
            var path4 = new TextPath()
            {
                PathText = @"Source/Public/Terminaux",
                ForegroundColor = ConsoleColors.Green,
                LastPathColor = ConsoleColors.Blue,
                SeparatorColor = ConsoleColors.Yellow,
                RootDriveColor = ConsoleColors.Red,
                UseColors = true,
                Settings = new() { Alignment = TextAlignment.Right },
                Left = 4,
                Top = 6,
                Width = 40,
            };
            TextWriterRaw.WriteRaw(path1.Render());
            TextWriterRaw.WriteRaw(path2.Render());
            TextWriterRaw.WriteRaw(path3.Render());
            TextWriterRaw.WriteRaw(path4.Render());
        }
    }
}
