﻿
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

using System.Collections.Generic;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Buffered
{
    /// <summary>
    /// Buffered screen tools
    /// </summary>
    public static class ScreenTools
    {
        private static readonly List<Screen> screens = [];

        /// <summary>
        /// Gets the currently displaying screen
        /// </summary>
        public static Screen CurrentScreen =>
            screens.Count > 0 ? screens[screens.Count - 1] : null;

        /// <summary>
        /// Renders the current screen one time
        /// </summary>
        /// <param name="clearScreen">Whether to clear the screen before writing the buffer down to the console</param>
        public static void Render(bool clearScreen = false) =>
            Render(CurrentScreen, clearScreen);

        /// <summary>
        /// Renders the screen one time
        /// </summary>
        /// <param name="screen">The screen to be rendered</param>
        /// <param name="clearScreen">Whether to clear the screen before writing the buffer down to the console</param>
        public static void Render(Screen screen, bool clearScreen = false)
        {
            // Check the screen instance
            if (screen is null)
                throw new TerminauxException("Screen is not specified.");

            // Clear if needed
            if (clearScreen)
                ColorTools.LoadBack();

            // Now, render the screen
            string buffer = screen.GetBuffer();
            if (string.IsNullOrEmpty(buffer))
                return;
            ConsoleWrapper.CursorVisible = false;
            TextWriterColor.WritePlain(buffer, false);
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
    }
}
