﻿//
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
using Terminaux.Colors.Data;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.MiscWriters.Tools;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintAlignMiddleOffCjk : IFixture
    {
        public void RunFixture()
        {
            AlignedTextColor.WriteAlignedColor($"你好，世界！它位于{ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Blue))}控制台的中心{ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Green))}，并且故意设计得很长，以演示右边距偏移和左边距偏移的偏心效果。", new Color(ConsoleColors.Green), TextAlignment.Middle, 15, 30);
        }
    }
}
