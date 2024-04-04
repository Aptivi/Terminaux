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

using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Inputs.Pointer
{
    /// <summary>
    /// Pointer (mouse) event listener
    /// </summary>
    public static class PointerListener
    {
        private static bool listening;

        /// <summary>
        /// Starts the pointer listener
        /// </summary>
        public static void StartListener()
        {
            if (listening)
                return;
            listening = true;

            // Start the listener
            TextWriterRaw.WriteRaw(CsiSequences.GenerateCsiLocatorReporting(1, 0));
            TextWriterRaw.WriteRaw(CsiSequences.GenerateCsiSelectLocatorEvents("1"));
        }

        /// <summary>
        /// Stops the pointer listener
        /// </summary>
        public static void StopListener()
        {
            if (!listening)
                return;
            listening = false;

            // Stop the listener
            TextWriterRaw.WriteRaw(CsiSequences.GenerateCsiLocatorReporting(0, 0));
        }
    }
}
