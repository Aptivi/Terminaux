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
using Colorimetry.Gradients;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Colors
{
    internal class ColorLevelTest : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Color;

        public void RunFixture()
        {
            var stage1NoSmooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.1);
            var stage2NoSmooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.3);
            var stage3NoSmooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.6);
            var stage4NoSmooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.9);
            var stage1Smooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.1, smooth: true);
            var stage2Smooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.3, smooth: true);
            var stage3Smooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.6, smooth: true);
            var stage4Smooth = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, 0.9, smooth: true);
            TextWriterColor.WriteColorBack("Stage 1 (not smooth)", ConsoleColors.White, stage1NoSmooth);
            TextWriterColor.WriteColorBack("Stage 1 (smooth)", ConsoleColors.White, stage1Smooth);
            TextWriterColor.WriteColorBack("Stage 2 (not smooth)", ConsoleColors.White, stage2NoSmooth);
            TextWriterColor.WriteColorBack("Stage 2 (smooth)", ConsoleColors.White, stage2Smooth);
            TextWriterColor.WriteColorBack("Stage 3 (not smooth)", ConsoleColors.White, stage3NoSmooth);
            TextWriterColor.WriteColorBack("Stage 3 (smooth)", ConsoleColors.White, stage3Smooth);
            TextWriterColor.WriteColorBack("Stage 4 (not smooth)", ConsoleColors.White, stage4NoSmooth);
            TextWriterColor.WriteColorBack("Stage 4 (smooth)", ConsoleColors.White, stage4Smooth);
        }
    }
}
