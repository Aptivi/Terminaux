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

using System;
using Colorimetry.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestException : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            try
            {
                // Let's make a process fail
                throw new Exception("This can't happen.");
            }
            catch (Exception ex)
            {
                var exception = new TextException(ex)
                {
                    ExceptionNameColor = ConsoleColors.Red,
                    ExceptionMessageColor = ConsoleColors.Maroon,
                    MethodMemberColor = ConsoleColors.Lime,
                    MethodColor = ConsoleColors.Yellow,
                    IlOffsetColor = ConsoleColors.Blue,
                    ParameterTypeColor = ConsoleColors.Blue,
                    ParameterColor = ConsoleColors.Aqua,
                    FileNameColor = ConsoleColors.Yellow,
                    FileLineNumberColor = ConsoleColors.Olive,
                    FileColumnNumberColor = ConsoleColors.Olive,
                };
                TextWriterRaw.WriteRaw(exception.Render());
            }
        }
    }
}
