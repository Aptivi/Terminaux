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

using System;
using System.Diagnostics;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Checks;

#if NET8_0_OR_GREATER
using SpecProbe.Software.Platform;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endif

namespace Terminaux.ResizeListener
{
    /// <summary>
    /// The console resize listener module
    /// </summary>
    public static class ConsoleResizeListener
    {
        private static Thread ResizeListenerThread = new((_) => PollForResize(null)) { Name = "Console Resize Listener Thread", IsBackground = true };

#if NET8_0_OR_GREATER
        internal static List<PosixSignalRegistration> signalHandlers = [];
#endif

        /// <summary>
        /// Starts the console resize listener
        /// </summary>
        /// <param name="customHandler">Specifies the custom console resize handler that will be called if resize is detected</param>
        public static void StartResizeListener(Action<int, int, int, int>? customHandler = null)
        {
            ConsoleResizeHandler.SetCachedWindowDimensions(Console.WindowWidth, Console.WindowHeight);

            // PosixSignalRegistration is not available for .NET Framework, so we need to guard accordingly
#if NET8_0_OR_GREATER
            ConsoleResizeHandler.usesSigWinch = PlatformHelper.IsOnUnix();
            if (ConsoleResizeHandler.usesSigWinch)
            {
                // This is to get around the platform compatibility since we've been already guarded by PlatformHelper.IsOnUnix().
                const PosixSignal winch = (PosixSignal)(-7);
                signalHandlers.Add(PosixSignalRegistration.Create(winch, (psc) => SigWindowChange(psc, (oldX, oldY, newX, newY) => PollForResize(customHandler))));
            }
            else
            {
#endif
                if (!ResizeListenerThread.IsAlive)
                {
                    if (customHandler is not null)
                    {
                        ResizeListenerThread = new((l) => PollForResize((Action<int, int, int, int>?)l)) { Name = "Console Resize Listener Thread", IsBackground = true };
                        ResizeListenerThread.Start(customHandler);
                    }
                    else
                        ResizeListenerThread.Start(null);
                }
#if NET8_0_OR_GREATER
            }
#endif
            ConsoleResizeHandler.listening = true;
        }

        private static void PollForResize(Action<int, int, int, int>? customHandler)
        {
            try
            {
                while (true)
                {
                    SpinWait.SpinUntil(ConsoleResizeHandler.CheckResized);
                    ConsoleResizeHandler.HandleResize(customHandler);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to detect console resize: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

#if NET8_0_OR_GREATER
        private static void SigWindowChange(PosixSignalContext psc, Action<int, int, int, int> customHandler)
        {
            ConsoleResizeHandler.HandleResize(customHandler);
            psc.Cancel = true;
        }
#endif

        static ConsoleResizeListener()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
