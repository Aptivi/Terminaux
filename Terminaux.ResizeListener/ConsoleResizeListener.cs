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
using Terminaux.Reader;
using Terminaux.Base.Buffered;

#if NET8_0_OR_GREATER
using SpecProbe.Platform;
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
        internal static bool usesSigWinch = false;
        private static bool ResizeDetected;
        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static Thread ResizeListenerThread = new((_) => PollForResize(null)) { Name = "Console Resize Listener Thread", IsBackground = true };

#if NET8_0_OR_GREATER
        internal static List<PosixSignalRegistration> signalHandlers = [];
#endif

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
            if (!ResizeListenerThread.IsAlive && !usesSigWinch)
                return (Console.WindowWidth, Console.WindowHeight);
            return (CurrentWindowWidth, CurrentWindowHeight);
        }

        /// <summary>
        /// Starts the console resize listener
        /// </summary>
        /// <param name="customHandler">Specifies the custom console resize handler that will be called if resize is detected</param>
        public static void StartResizeListener(Action<int, int, int, int> customHandler = null)
        {
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;

            // PosixSignalRegistration is not available for .NET Framework, so we need to 
#if NET8_0_OR_GREATER
            usesSigWinch = PlatformHelper.IsOnUnix();
            if (usesSigWinch)
            {
                // This is to get around the platform compatibility since we've been already guarded by ConsolePlatform.IsOnUnix().
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
                    ResizeListenerThread = new((l) => PollForResize((Action<int, int, int, int>)l)) { Name = "Console Resize Listener Thread", IsBackground = true };
                    ResizeListenerThread.Start(customHandler);
                }
                else
                    ResizeListenerThread.Start(null);
            }
#if NET8_0_OR_GREATER
            }
#endif
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

        private static void HandleResize(Action<int, int, int, int> customHandler)
        {
            int oldX = CurrentWindowWidth;
            int oldY = CurrentWindowHeight;
            ResizeDetected = true;
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;

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

        private static void PollForResize(Action<int, int, int, int> customHandler)
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
                Debug.WriteLine($"Failed to detect console resize: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

#if NET8_0_OR_GREATER
        private static void SigWindowChange(PosixSignalContext psc, Action<int, int, int, int> customHandler)
        {
            HandleResize(customHandler);
            psc.Cancel = true;
        }
#endif
    }
}
