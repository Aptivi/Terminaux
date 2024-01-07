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
using Terminaux.Base.Buffered;
using Terminaux.Reader;

namespace Terminaux.Base
{
    /// <summary>
    /// The console resize listener module
    /// </summary>
    public static class ConsoleResizeListener
    {
        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static Thread ResizeListenerThread = new((_) => PollForResize(null)) { Name = "Console Resize Listener Thread", IsBackground = true };
        private static bool ResizeDetected;

        /// <summary>
        /// Whether to run the base console resize handler or not after running a custom action
        /// </summary>
        public static bool RunEssentialHandler { get; set; } = true;

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
        /// Gets the console size from the cached window height and width position
        /// </summary>
        public static (int Width, int Height) GetCurrentConsoleSize()
        {
            if (!ResizeListenerThread.IsAlive)
                return (Console.WindowWidth, Console.WindowHeight);
            return (CurrentWindowWidth, CurrentWindowHeight);
        }

        /// <summary>
        /// Starts the console resize listener
        /// </summary>
        /// <param name="customHandler">Specifies the custom console resize handler that will be called if resize is detected</param>
        public static void StartResizeListener(Action customHandler = null)
        {
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            if (!ResizeListenerThread.IsAlive)
            {
                if (customHandler is not null)
                {
                    ResizeListenerThread = new((l) => PollForResize((Action)l)) { Name = "Console Resize Listener Thread", IsBackground = true };
                    ResizeListenerThread.Start(customHandler);
                }
                else
                    ResizeListenerThread.Start(null);
            }
        }

        private static bool CheckResized()
        {
            if (CurrentWindowHeight != Console.WindowHeight |
                CurrentWindowWidth != Console.WindowWidth)
                return true;
            return false;
        }

        private static void EssentialHandler()
        {
            // Tell the screen-based apps to refresh themselves
            if (ScreenTools.CurrentScreen is not null)
                ScreenTools.Render();

            // Tell the reader to refresh itself
            if (TermReaderTools.Busy)
            {
                var state = TermReader.states[TermReader.states.Count - 1];
                TermReaderTools.RefreshPrompt(ref state);
                TermReader.states[TermReader.states.Count - 1] = state;
            }
        }

        private static void PollForResize(Action customHandler)
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                    bool update = CheckResized();

                    if (update)
                    {
                        ResizeDetected = true;
                        CurrentWindowWidth = Console.WindowWidth;
                        CurrentWindowHeight = Console.WindowHeight;

                        // If the custom handler is set, run it and (optionally) run the custom handler with it. Otherwise, run the
                        // essential handler regardless of the value of RunEssentialHandler to maintain consistency.
                        if (customHandler is not null)
                        {
                            customHandler();
                            if (RunEssentialHandler)
                                EssentialHandler();
                        }
                        else
                            EssentialHandler();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to detect console resize: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
