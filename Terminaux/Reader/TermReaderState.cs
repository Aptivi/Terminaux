
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Terminaux.Reader
{
    /// <summary>
    /// State of the reader
    /// </summary>
    [DebuggerDisplay("Pos = {CurrentTextPos}, Input = ({InputPromptLeft}, {InputPromptTop}), Cursor = ({CurrentCursorPosLeft}, {CurrentCursorPosTop})")]
    public class TermReaderState
    {
        // Instance
        internal int inputPromptLeft;
        internal int inputPromptTop;
        internal int currentCursorPosLeft;
        internal int currentCursorPosTop;
        internal int currentTextPos;
        internal string inputPromptText;
        internal StringBuilder currentText = new();
        internal bool passwordMode;
        internal ConsoleKeyInfo pressedKey;
        internal StringBuilder killBuffer = new();
        internal bool oneLineWrap;
        internal bool canInsert = true;
        internal TermReaderSettings settings;

        // Shared
        internal static List<string> history = [];
        internal static int currentHistoryPos;
        internal static int currentSuggestionsPos = -1;

        // To instance variables
        /// <summary>
        /// Left position of the input prompt
        /// </summary>
        public int InputPromptLeft =>
            inputPromptLeft;
        /// <summary>
        /// Top position of the input prompt
        /// </summary>
        public int InputPromptTop =>
            inputPromptTop;
        /// <summary>
        /// Current cursor left position
        /// </summary>
        public int CurrentCursorPosLeft =>
            currentCursorPosLeft;
        /// <summary>
        /// Current cursor top position
        /// </summary>
        public int CurrentCursorPosTop =>
            currentCursorPosTop;
        /// <summary>
        /// Current text character number
        /// </summary>
        public int CurrentTextPos =>
            currentTextPos;
        /// <summary>
        /// Input prompt text
        /// </summary>
        public string InputPromptText =>
            inputPromptText;
        /// <summary>
        /// Current text
        /// </summary>
        public StringBuilder CurrentText =>
            currentText;
        /// <summary>
        /// Password Mode
        /// </summary>
        public bool PasswordMode =>
            passwordMode;
        /// <summary>
        /// Currently pressed key
        /// </summary>
        public ConsoleKeyInfo PressedKey =>
            pressedKey;
        /// <summary>
        /// Text to be pasted
        /// </summary>
        public StringBuilder KillBuffer =>
            killBuffer;
        /// <summary>
        /// Reader settings (general or overridden)
        /// </summary>
        public TermReaderSettings Settings =>
            settings;
        /// <summary>
        /// Can insert a new character?
        /// </summary>
        public bool CanInsert =>
            canInsert;

        // To static variables
        /// <summary>
        /// History entries
        /// </summary>
        public List<string> History =>
            history;
        /// <summary>
        /// Current history number
        /// </summary>
        public int CurrentHistoryPos =>
            currentHistoryPos;
        /// <summary>
        /// Current suggestion number
        /// </summary>
        public int CurrentSuggestionsPos =>
            currentSuggestionsPos;
        /// <summary>
        /// Whether one line wrapping is enabled
        /// </summary>
        public bool OneLineWrap =>
            oneLineWrap;

        internal TermReaderState() { }
    }
}
