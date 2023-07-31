
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
using System.Text;

namespace Terminaux.Reader
{
    /// <summary>
    /// State of the reader
    /// </summary>
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
        internal TermReaderSettings settings;

        // Shared
        internal static List<string> history = new();
        internal static int currentHistoryPos;
        internal static int currentSuggestionsPos = -1;

        // To instance variables
        /// <summary>
        /// Left position of the input prompt
        /// </summary>
        public int InputPromptLeft { get => inputPromptLeft; }
        /// <summary>
        /// Top position of the input prompt
        /// </summary>
        public int InputPromptTop { get => inputPromptTop; }
        /// <summary>
        /// Current cursor left position
        /// </summary>
        public int CurrentCursorPosLeft { get => currentCursorPosLeft; }
        /// <summary>
        /// Current cursor top position
        /// </summary>
        public int CurrentCursorPosTop { get => currentCursorPosTop; }
        /// <summary>
        /// Current text character number
        /// </summary>
        public int CurrentTextPos { get => currentTextPos; }
        /// <summary>
        /// Input prompt text
        /// </summary>
        public string InputPromptText { get => inputPromptText; }
        /// <summary>
        /// Current text
        /// </summary>
        public StringBuilder CurrentText { get => currentText; }
        /// <summary>
        /// Password Mode
        /// </summary>
        public bool PasswordMode { get => passwordMode; }
        /// <summary>
        /// Currently pressed key
        /// </summary>
        public ConsoleKeyInfo PressedKey { get => pressedKey; }
        /// <summary>
        /// Text to be pasted
        /// </summary>
        public StringBuilder KillBuffer { get => killBuffer; }
        /// <summary>
        /// Reader settings (general or overridden)
        /// </summary>
        public TermReaderSettings Settings { get => settings; }

        // To static variables
        /// <summary>
        /// History entries
        /// </summary>
        public List<string> History { get => history; }
        /// <summary>
        /// Current history number
        /// </summary>
        public int CurrentHistoryPos { get => currentHistoryPos; }
        /// <summary>
        /// Current suggestion number
        /// </summary>
        public int CurrentSuggestionsPos { get => currentSuggestionsPos; }
        /// <summary>
        /// Whether one line wrapping is enabled
        /// </summary>
        public bool OneLineWrap { get => oneLineWrap; }

        internal TermReaderState() { }
    }
}
