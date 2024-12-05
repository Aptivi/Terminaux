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

namespace Terminaux.Shell.Arguments.Base
{
    /// <summary>
    /// Base kernel argument executor
    /// </summary>
    public interface IArgument
    {

        /// <summary>
        /// Executes the kernel argument with the given argument
        /// </summary>
        /// <param name="parameters">Argument parameters including passed arguments and switches information</param>
        void Execute(ArgumentParameters parameters);

        /// <summary>
        /// Shows additional information for the argument when "arghelp argument" is invoked in the admin shell
        /// </summary>
        void HelpHelper();

    }
}
