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
using Terminaux.Sequences.Builder;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console cursor tools
    /// </summary>
    public static class ConsoleCursor
    {
        private static bool cursorVisible = true;
        private static ConsoleCursorType cursorType = ConsoleCursorType.User;

        /// <summary>
        /// Determines whether the cursor is visible or not
        /// </summary>
        public static bool CursorVisible
        {
            get => cursorVisible;
            set
            {
                if (!ConsoleWrapper.IsDumb)
                {
                    // We can't call Get accessor of the primary CursorVisible since this is Windows only, so we have this decoy variable
                    // to make it work on Linux, Android, and macOS.
                    cursorVisible = value;
                    Console.CursorVisible = value;
                }
            }
        }

        /// <summary>
        /// Specifies the cursor type
        /// </summary>
        public static ConsoleCursorType CursorType
        {
            get => cursorType;
            set
            {
                if (!ConsoleWrapper.IsDumb)
                {
                    if (value < ConsoleCursorType.User || value > ConsoleCursorType.BarSteady)
                        value = ConsoleCursorType.User;
                    int typeInt = (int)value;
                    TextWriterRaw.WritePlain(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiSetCursorStyle, typeInt));
                    cursorType = value;
                }
            }
        }
    }
}
