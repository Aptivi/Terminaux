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
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Inputs.Modules
{
    internal class TestSliderBoxModule : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            // Make a box frame and a combo box module instance
            var boxFrame = new BoxFrame()
            {
                Left = 4,
                Top = 2,
                Width = ConsoleWrapper.WindowWidth - 14,
                Height = 1,
            };
            var boxModule = new SliderBoxModule()
            {
                MinPos = 0,
                MaxPos = 100,
                Value = 50,
            };

            // Render the input placeholder
            TextWriterRaw.WriteRaw(
                boxFrame.Render() +
                ConsolePositioning.RenderChangePosition(5, 3) +
                boxModule.RenderInput(ConsoleWrapper.WindowWidth - 14)
            );

            // Let the user press any key
            Input.ReadKey();

            // Process the input using the standard method
            boxModule.ProcessInput();

            // Render the input placeholder again
            TextWriterRaw.WriteRaw(
                boxFrame.Render() +
                ConsolePositioning.RenderChangePosition(5, 3) +
                boxModule.RenderInput(ConsoleWrapper.WindowWidth - 14)
            );
        }
    }
}
