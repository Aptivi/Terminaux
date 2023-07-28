﻿/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Terminaux.Reader
{
    /// <summary>
    /// State of the reader
    /// </summary>
    public class TerminauxerState
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

        internal TerminauxerState() { }
    }
}
