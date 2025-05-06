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

using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestBoxFrame : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            TextWriterColor.WriteColor("A simple box frame:", true, new Color(ConsoleColors.Green));
            var frame1 = new BoxFrame()
            {
                Width = 20,
                Height = 7,
                Left = 2,
                Top = 4,
            };
            var frame2 = new BoxFrame()
            {
                Width = 20,
                Height = 7,
                Left = 24,
                Top = 4,
                Rulers =
                [
                    new(3, RulerOrientation.Horizontal),
                ]
            };
            var frame3 = new BoxFrame()
            {
                Width = 20,
                Height = 7,
                Left = 2,
                Top = 13,
                Rulers =
                [
                    new(8, RulerOrientation.Vertical),
                ]
            };
            var frame4 = new BoxFrame()
            {
                Width = 20,
                Height = 7,
                Left = 24,
                Top = 13,
                Rulers =
                [
                    new(3, RulerOrientation.Horizontal),
                    new(8, RulerOrientation.Vertical),

                    // Done on purpose to test distinct array enumeration, so don't remove this regression testing
                    new(3, RulerOrientation.Horizontal),
                    new(8, RulerOrientation.Vertical),
                ]
            };
            TextWriterRaw.WriteRaw(frame1.Render());
            TextWriterRaw.WriteRaw(frame2.Render());
            TextWriterRaw.WriteRaw(frame3.Render());
            TextWriterRaw.WriteRaw(frame4.Render());
        }
    }
}
