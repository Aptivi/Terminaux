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

using SpecProbe.Software.Platform;
using System;
using Terminaux.Base.Extensions.Native;
using Terminaux.Inputs;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console mode manipulation
    /// </summary>
    public static class ConsoleMode
    {
        private static bool isRaw = false;

        /// <summary>
        /// Whether the console is in the raw mode or in the cooked mode
        /// </summary>
        public static bool IsRaw =>
            isRaw;

        /// <summary>
        /// Enables raw console (goes to raw mode)
        /// </summary>
        public static void EnableRaw()
        {
            if (PlatformHelper.IsOnWindows())
                return;
            if (Input.EnableMouse)
                return;
            NativeMethods.RawSet(true);
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Input.EnableMouse = false;
            AppDomain.CurrentDomain.UnhandledException += (_, _) => Input.EnableMouse = false;
            isRaw = true;
        }

        /// <summary>
        /// Disables raw console (goes back to cooked mode)
        /// </summary>
        public static void DisableRaw()
        {
            if (PlatformHelper.IsOnWindows())
                return;
            if (Input.EnableMouse)
                return;
            NativeMethods.RawSet(false);
            AppDomain.CurrentDomain.ProcessExit -= (_, _) => Input.EnableMouse = false;
            AppDomain.CurrentDomain.UnhandledException -= (_, _) => Input.EnableMouse = false;
            isRaw = false;
        }
    }
}
