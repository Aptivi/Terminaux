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

using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Sequences.Builder;
using Terminaux.Sequences.Builder.Types;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console clearing tools
    /// </summary>
    public static class ConsoleClearing
    {
        /// <summary>
        /// Gets a sequence that clears the whole screen
        /// </summary>
        public static string GetClearWholeScreenSequence() =>
            $"{CsiSequences.GenerateCsiCursorPosition(1, 1)}" +
            $"{CsiSequences.GenerateCsiEraseInDisplay(0)}";

        /// <summary>
        /// Gets a sequence that clears the line to the right
        /// </summary>
        public static string GetClearLineToRightSequence() =>
            VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiEraseInLine, 0);

        /// <summary>
        /// Clears the line to the right
        /// </summary>
        public static void ClearLineToRight() =>
            ConsoleWrapper.Write(GetClearLineToRightSequence());

        /// <summary>
        /// Resets the entire console
        /// </summary>
        public static void ResetAll()
        {
            ConsoleWrapper.Write(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.EscFullReset));
            ConsoleWrapper.Write(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiSoftTerminalReset));
        }

        static ConsoleClearing()
        {
            if (GeneralColorTools.CheckConsoleOnCall)
                ConsoleChecker.CheckConsole();
        }
    }
}
