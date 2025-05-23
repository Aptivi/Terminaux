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

using System;
using System.Collections.Generic;
using System.Threading;
using Terminaux.Base.Checks;
using Terminaux.Writer.ConsoleWriters;
using ThreadState = System.Threading.ThreadState;

namespace Terminaux.Base.Buffered
{
    /// <summary>
    /// Buffered screen tools
    /// </summary>
    public static class ScreenTools
    {
        private static Screen? cyclicScreen = null;
        private static Thread? cyclicScreenThread = null;
        private static readonly List<Screen> screens = [];

        /// <summary>
        /// Gets the currently displaying screen
        /// </summary>
        public static Screen? CurrentScreen =>
            screens.Count > 0 ? screens[screens.Count - 1] : null;

        /// <summary>
        /// Renders the current screen one time
        /// </summary>
        public static void Render() =>
            Render(CurrentScreen);

        /// <summary>
        /// Renders the screen one time
        /// </summary>
        /// <param name="screen">The screen to be rendered</param>
        public static void Render(Screen? screen)
        {
            // Check the screen instance
            if (screen is null)
                throw new TerminauxException("Screen is not specified.");

            // Now, render the screen
            string buffer = screen.GetBuffer();
            if (string.IsNullOrEmpty(buffer))
                return;
            ConsoleWrapper.CursorVisible = false;
            ConsoleLogger.Debug("Writing {0} bytes from screen buffer...", buffer.Length);
            TextWriterRaw.WriteRaw(buffer);
        }

        /// <summary>
        /// Sets the current screen instance
        /// </summary>
        /// <param name="screen">The screen to add to the list</param>
        /// <exception cref="TerminauxException"></exception>
        public static void SetCurrent(Screen screen)
        {
            // Check the screen instance
            if (screen is null)
                throw new TerminauxException("Screen is not specified.");

            // Add the screen to the list
            screens.Add(screen);
        }

        /// <summary>
        /// Unsets the current screen instance
        /// </summary>
        /// <param name="screen">The screen to remove from the list</param>
        /// <exception cref="TerminauxException"></exception>
        public static void UnsetCurrent(Screen screen)
        {
            // Check the screen instance
            if (screen is null)
                throw new TerminauxException("Screen is not specified.");

            // Remove the screen from the list
            screens.Remove(screen);
        }

        /// <summary>
        /// Sets the current cyclic screen instance
        /// </summary>
        /// <param name="screen">The screen to add to the list</param>
        /// <exception cref="TerminauxException"></exception>
        public static void SetCurrentCyclic(Screen screen)
        {
            // Check the screen instance
            if (screen is null)
                throw new TerminauxException("Screen is not specified.");
            if (screen.CycleFrequency <= 0)
                throw new TerminauxException("Screen is not cyclic.");

            // Add the screen to the cyclic screen variable
            ConsoleLogger.Debug("Adding cyclic screen with frequency {0} ms...", screen.CycleFrequency);
            cyclicScreen = screen;
        }

        /// <summary>
        /// Unsets the current cyclic screen instance
        /// </summary>
        /// <exception cref="TerminauxException"></exception>
        public static void UnsetCurrentCyclic() =>
            cyclicScreen = null;

        /// <summary>
        /// Starts the current cyclic screen (you can set the current cyclic screen with <see cref="SetCurrentCyclic(Screen)"/>)
        /// </summary>
        public static void StartCyclicScreen() =>
            StartCyclicScreen(cyclicScreen);

        /// <summary>
        /// Starts the current cyclic screen (you can set the current cyclic screen with <see cref="SetCurrentCyclic(Screen)"/>)
        /// </summary>
        /// <param name="screen">Screen to cycle</param>
        public static void StartCyclicScreen(Screen? screen)
        {
            // Check the screen instance
            if (screen is null)
                throw new TerminauxException("Screen is not specified.");

            // Now, start a thread with cyclic screen info
            StopCyclicScreen();
            cyclicScreenThread = new(() => CycleScreen(screen));
            cyclicScreenThread.Start();
            ConsoleLogger.Debug("Cyclic screen with frequency {0} ms has started...", screen.CycleFrequency);
        }

        /// <summary>
        /// Checks to see whether the cyclic screen thread is running
        /// </summary>
        /// <returns></returns>
        public static bool CyclicScreenRunning() =>
            cyclicScreenThread?.ThreadState == ThreadState.Running;

        /// <summary>
        /// Stops the current cyclic screen
        /// </summary>
        public static void StopCyclicScreen()
        {
            cyclicScreenThread?.Interrupt();
            cyclicScreenThread?.Join(10000);
        }

        private static void CycleScreen(Screen screen)
        {
            try
            {
                while (CyclicScreenRunning())
                {
                    Render(screen);
                    Thread.Sleep(screen.CycleFrequency);
                }
            }
            catch (ThreadInterruptedException)
            {
                ConsoleLogger.Warning("Thread is interrupted");
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"Failed to render cyclic screen: {ex.Message}");
            }
        }

        static ScreenTools()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
