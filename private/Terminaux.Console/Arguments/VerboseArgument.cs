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

using Aptivestigate.Logging;
using Aptivestigate.Serilog;
using Serilog;
using Terminaux.Base;
using Terminaux.Shell.Arguments.Base;

namespace Terminaux.Console.Arguments
{
    internal class VerboseArgument : ArgumentExecutor, IArgument
    {
        public override void Execute(ArgumentParameters parameters)
        {
            ConsoleLogger.AbstractLogger = new SerilogLogger(new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(LogTools.GenerateLogFilePath(out _)));
            ConsoleLogger.EnableLogging = true;
        }
    }
}
