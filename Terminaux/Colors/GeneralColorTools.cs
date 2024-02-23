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

namespace Terminaux.Colors
{
    /// <summary>
    /// General color tools class. It doesn't require checking the console.
    /// </summary>
    public static class GeneralColorTools
    {
        /// <summary>
        /// Upon calling functions, Terminaux should check for console.
        /// </summary>
        public static bool CheckConsoleOnCall { get; set; } = true;
    }
}
