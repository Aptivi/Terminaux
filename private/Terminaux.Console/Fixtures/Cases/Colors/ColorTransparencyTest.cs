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
using Colorimetry.Transformation;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Colors
{
    internal class ColorTransparencyTest : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Color;

        public void RunFixture()
        {
            var opaque =
                new Color(ConsoleColors.Green);
            var greenQuarterTransparent =
                new Color(ConsoleColors.Green, new ColorSettings() { Opacity = 192, OpacityColor = ConsoleColors.Red });
            var greenHalfTransparent =
                new Color(ConsoleColors.Green, new ColorSettings() { Opacity = 128, OpacityColor = ConsoleColors.Red });
            var greenThirdQuarterTransparent =
                new Color(ConsoleColors.Green, new ColorSettings() { Opacity = 64, OpacityColor = ConsoleColors.Red });
            var greenTransparent =
                new Color(ConsoleColors.Green, new ColorSettings() { Opacity = 0, OpacityColor = ConsoleColors.Red });

            // Disable cursor
            ConsoleWrapper.CursorVisible = false;

            // 100% opaque
            ConsoleColoring.LoadBackDry(opaque, true);
            var infoBorder100 = new Border()
            {
                Text = "100% green, 0% red",
                Left = 2,
                Top = 1,
                Width = "100% green, 0% red".Length,
                Height = 1,
                Color = opaque,
                BackgroundColor = TransformationTools.GetDarkBackground(opaque),
            };
            TextWriterRaw.WriteRaw(infoBorder100.Render());
            Input.ReadKey();

            // 75% opaque
            ConsoleColoring.LoadBackDry(greenQuarterTransparent, true);
            var infoBorder75 = new Border()
            {
                Text = "75% green, 25% red",
                Left = 2,
                Top = 1,
                Width = "75% green, 25% red".Length,
                Height = 1,
                Color = greenQuarterTransparent,
                BackgroundColor = TransformationTools.GetDarkBackground(greenQuarterTransparent),
            };
            TextWriterRaw.WriteRaw(infoBorder75.Render());
            Input.ReadKey();

            // 50% opaque
            ConsoleColoring.LoadBackDry(greenHalfTransparent, true);
            var infoBorder50 = new Border()
            {
                Text = "50% green, 50% red",
                Left = 2,
                Top = 1,
                Width = "50% green, 50% red".Length,
                Height = 1,
                Color = greenHalfTransparent,
                BackgroundColor = TransformationTools.GetDarkBackground(greenHalfTransparent),
            };
            TextWriterRaw.WriteRaw(infoBorder50.Render());
            Input.ReadKey();

            // 25% opaque
            ConsoleColoring.LoadBackDry(greenThirdQuarterTransparent, true);
            var infoBorder25 = new Border()
            {
                Text = "25% green, 75% red",
                Left = 2,
                Top = 1,
                Width = "25% green, 75% red".Length,
                Height = 1,
                Color = greenThirdQuarterTransparent,
                BackgroundColor = TransformationTools.GetDarkBackground(greenThirdQuarterTransparent),
            };
            TextWriterRaw.WriteRaw(infoBorder25.Render());
            Input.ReadKey();

            // 0% opaque
            ConsoleColoring.LoadBackDry(greenTransparent, true);
            var infoBorder0 = new Border()
            {
                Text = "0% green, 100% red",
                Left = 2,
                Top = 1,
                Width = "0% green, 100% red".Length,
                Height = 1,
                Color = greenTransparent,
                BackgroundColor = TransformationTools.GetDarkBackground(greenTransparent),
            };
            TextWriterRaw.WriteRaw(infoBorder0.Render());
            Input.ReadKey();

            // Reset
            ConsoleColoring.LoadBack();
        }
    }
}
