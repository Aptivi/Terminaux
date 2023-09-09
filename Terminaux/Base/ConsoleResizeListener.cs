
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.Threading;

namespace Terminaux.Base
{
    /// <summary>
    /// The console resize listener module
    /// </summary>
    public static class ConsoleResizeListener
    {
        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static readonly Thread ResizeListenerThread = new(PollForResize) { Name = "Console Resize Listener Thread", IsBackground = true };
        private static bool ResizeDetected;

        /// <summary>
        /// This property checks to see if the console has been resized since the last time it has been called or the listener has started.
        /// </summary>
        /// <param name="reset">Reset the resized value once this is called</param>
        public static bool WasResized(bool reset = true)
        {
            if (ResizeDetected)
            {
                // The console has been resized.
                ResizeDetected = !reset;
                return true;
            }
            return false;
        }

        private static void PollForResize()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(100);

                    // We need to call the WindowHeight and WindowWidth properties on the Terminal console driver, because
                    // this polling works for all the terminals. Other drivers that don't use the terminal may not even
                    // implement these two properties.
                    if (CurrentWindowHeight != ConsoleWrappers.ActionWindowHeight() | CurrentWindowWidth != ConsoleWrappers.ActionWindowWidth())
                    {
                        ResizeDetected = true;
                        CurrentWindowWidth = ConsoleWrappers.ActionWindowWidth();
                        CurrentWindowHeight = ConsoleWrappers.ActionWindowHeight();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to detect console resize: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

        internal static void StartResizeListener()
        {
            CurrentWindowWidth = ConsoleWrappers.ActionWindowWidth();
            CurrentWindowHeight = ConsoleWrappers.ActionWindowHeight();
            if (!ResizeListenerThread.IsAlive)
                ResizeListenerThread.Start();
        }
    }
}
