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

using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Writer.FancyWriters;

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
            ColorTools.LoadBackDry(opaque);
            BorderTextColor.WriteBorder("100% green, 0% red", 2, 1, "100% green, 0% red".Length, 1, opaque);
            Input.ReadKey();

            // 75% opaque
            ColorTools.LoadBackDry(greenQuarterTransparent);
            BorderTextColor.WriteBorder("75% green, 25% red", 2, 1, "75% green, 25% red".Length, 1, greenQuarterTransparent);
            Input.ReadKey();

            // 50% opaque
            ColorTools.LoadBackDry(greenHalfTransparent);
            BorderTextColor.WriteBorder("50% green, 50% red", 2, 1, "50% green, 50% red".Length, 1, greenHalfTransparent);
            Input.ReadKey();

            // 25% opaque
            ColorTools.LoadBackDry(greenThirdQuarterTransparent);
            BorderTextColor.WriteBorder("25% green, 75% red", 2, 1, "25% green, 75% red".Length, 1, greenThirdQuarterTransparent);
            Input.ReadKey();

            // 0% opaque
            ColorTools.LoadBackDry(greenTransparent);
            BorderTextColor.WriteBorder("0% green, 100% red", 2, 1, "0% green, 100% red".Length, 1, greenTransparent);
            Input.ReadKey();

            // Reset
            ColorTools.LoadBack();
        }
    }
}
