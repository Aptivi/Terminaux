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
using Terminaux.Reader;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using System.Threading;

namespace Terminaux.Base
{
    /// <summary>
    /// The console resize listener module
    /// </summary>
    public static class ConsoleResizeHandler
    {
        internal static bool usesSigWinch = false;
        internal static int CurrentWindowWidth;
        internal static int CurrentWindowHeight;
        private static bool ResizeDetected;
        private static Thread ResizeListenerThread = new((_) => PollForResize(null)) { Name = "Console Resize Listener Thread", IsBackground = true };

        /// <summary>
        /// Whether to run the base console resize handler or not after running a custom action
        /// </summary>
        public static bool RunEssentialHandler { get; set; } = true;

        /// <summary>
        /// Whether the console resize handler is listening to resize events or not
        /// </summary>
        public static bool IsListening =>
            ResizeListenerThread.IsAlive;

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

        /// <summary>
        /// Starts the console resize listener
        /// </summary>
        /// <param name="customHandler">Specifies the custom console resize handler that will be called if resize is detected</param>
        public static void StartResizeListener(Action<int, int, int, int>? customHandler = null)
        {
            if (IsListening || ConsoleChecker.IsDumb)
                return;

            SetCachedWindowDimensions(Console.WindowWidth, Console.WindowHeight);
            if (customHandler is not null)
            {
                ResizeListenerThread = new((l) => PollForResize((Action<int, int, int, int>?)l)) { Name = "Console Resize Listener Thread", IsBackground = true };
                ResizeListenerThread.Start(customHandler);
            }
            else
            {
                ResizeListenerThread = new((_) => PollForResize(null)) { Name = "Console Resize Listener Thread", IsBackground = true };
                ResizeListenerThread.Start(null);
            }
        }

        /// <summary>
        /// Stops the console resize listener
        /// </summary>
        public static void StopResizeListener()
        {
            if (!IsListening)
                return;
            ResizeListenerThread.Interrupt();
        }

        /// <summary>
        /// Gets the console size from the cached window height and width position
        /// </summary>
        public static (int Width, int Height) GetCurrentConsoleSize()
        {
            if (!IsListening && !usesSigWinch)
                return (Console.WindowWidth, Console.WindowHeight);
            return (CurrentWindowWidth, CurrentWindowHeight);
        }

        internal static bool CheckResized()
        {
            if (CurrentWindowHeight != Console.WindowHeight |
                CurrentWindowWidth != Console.WindowWidth)
                return true;
            return false;
        }

        internal static void HandleResize(Action<int, int, int, int>? customHandler)
        {
            int oldX = CurrentWindowWidth;
            int oldY = CurrentWindowHeight;
            ResizeDetected = true;
            SetCachedWindowDimensions(Console.WindowWidth, Console.WindowHeight);

            // If the custom handler is set, run it and (optionally) run the custom handler with it. Otherwise, run the
            // essential handler regardless of the value of RunEssentialHandler to maintain consistency.
            if (customHandler is not null)
            {
                customHandler(oldX, oldY, CurrentWindowWidth, CurrentWindowHeight);
                if (RunEssentialHandler)
                    EssentialHandler();
            }
            else
                EssentialHandler();
        }

        internal static void SetCachedWindowDimensions(int width, int height)
        {
            CurrentWindowWidth = width;
            CurrentWindowHeight = height;
        }

        private static void EssentialHandler()
        {
            // Tell the screen-based apps to refresh themselves
            if (ScreenTools.CurrentScreen is not null)
                ScreenTools.Render();

            // Tell the reader to refresh itself
            if (TermReaderTools.Busy)
                TermReaderTools.Refresh();
        }

        private static void PollForResize(Action<int, int, int, int>? customHandler)
        {
            try
            {
                while (true)
                {
                    SpinWait.SpinUntil(CheckResized);
                    HandleResize(customHandler);
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"Failed to detect console resize: {ex.Message}");
            }
        }
    }
}
