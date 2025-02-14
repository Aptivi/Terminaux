﻿//
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Reader.History;
using Terminaux.Sequences;

namespace Terminaux.Reader
{
    /// <summary>
    /// State of the reader
    /// </summary>
    [DebuggerDisplay("Pos = {CurrentTextPos}, Start = ({InputPromptLeftBegin}, {InputPromptTopBegin}), Input = ({InputPromptLeft}, {InputPromptTop}), Cursor = ({CurrentCursorPosLeft}, {CurrentCursorPosTop})")]
    public class TermReaderState
    {
        // Instance
        internal int inputPromptLeftBegin;
        internal int inputPromptTopBegin;
        internal int inputPromptLeft;
        internal int inputPromptTop;
        internal int currentCursorPosLeft;
        internal int currentCursorPosTop;
        internal int currentTextPos;
        internal int currentHistoryPos;
        internal int currentSuggestionsPos = -1;
        internal int currentSuggestionsTextPos = -1;
        internal Func<string> inputPromptText = () => "";
        internal StringBuilder currentText = new();
        internal bool passwordMode;
        internal ConsoleKeyInfo pressedKey;
        internal StringBuilder killBuffer = new();
        internal bool oneLineWrap;
        internal bool canInsert = true;
        internal bool writingPrompt;
        internal bool insertIsReplace;
        internal bool commentized;
        internal bool operationWasInvalid;
        internal bool concealing;
        internal bool refreshRequired;
        internal List<char> argNumbers = [];
        internal string oldText = "";
        internal List<string> changes = [];
        internal TermReaderSettings settings = TermReader.GlobalReaderSettings;

        /// <summary>
        /// Left position of the input prompt (where the first letter of the input prompt is located)
        /// </summary>
        public int InputPromptLeftBegin =>
            inputPromptLeftBegin;

        /// <summary>
        /// Top position of the input prompt (where the first letter of the input prompt is located)
        /// </summary>
        public int InputPromptTopBegin =>
            inputPromptTopBegin;

        /// <summary>
        /// Left position of the input prompt (with the left margin)
        /// </summary>
        public int InputPromptLeft =>
            inputPromptLeft;

        /// <summary>
        /// Top position of the input prompt
        /// </summary>
        public int InputPromptTop =>
            inputPromptTop;

        /// <summary>
        /// Left margin
        /// </summary>
        public int LeftMargin =>
            settings.LeftMargin;

        /// <summary>
        /// Right margin
        /// </summary>
        public int RightMargin =>
            settings.RightMargin;

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
        /// Maximum input left position
        /// </summary>
        public int MaximumInputPositionLeft =>
            LongestSentenceLengthFromLeft - 1;

        /// <summary>
        /// Longest sentence length (from the leftmost position, without offset created by the last line in the prompt)
        /// </summary>
        public int LongestSentenceLengthFromLeft
        {
            get
            {
                int width = ConsoleWrapper.WindowWidth;
                return width - settings.RightMargin;
            }
        }

        /// <summary>
        /// Longest sentence length (from the leftmost position, with the length of the last line in the prompt plus the left margin)
        /// </summary>
        public int LongestSentenceLengthFromLeftForFirstLine
        {
            get
            {
                int width = ConsoleWrapper.WindowWidth;
                return width - settings.RightMargin - inputPromptLeft - 1;
            }
        }

        /// <summary>
        /// Longest sentence length (from the leftmost position, with the left margin)
        /// </summary>
        public int LongestSentenceLengthFromLeftForGeneralLine
        {
            get
            {
                int width = ConsoleWrapper.WindowWidth;
                return width - settings.RightMargin - settings.LeftMargin - 1;
            }
        }

        /// <summary>
        /// Current text character number
        /// </summary>
        public int CurrentTextPos =>
            currentTextPos;

        /// <summary>
        /// Input prompt text
        /// </summary>
        public string InputPromptText =>
            inputPromptText.Invoke();

        /// <summary>
        /// Input prompt text
        /// </summary>
        public int InputPromptHeight
        {
            get
            {
                string[] inputPromptLines = ConsoleMisc.GetWrappedSentences(InputPromptText, ConsoleWrapper.WindowWidth);
                return inputPromptLines.Length;
            }
        }

        /// <summary>
        /// Input prompt last line
        /// </summary>
        public string InputPromptLastLine
        {
            get
            {
                string[] inputPromptLines = ConsoleMisc.GetWrappedSentences(InputPromptText, ConsoleWrapper.WindowWidth);
                string inputPromptLastLine = VtSequenceTools.FilterVTSequences(inputPromptLines[inputPromptLines.Length - 1]);
                return inputPromptLastLine;
            }
        }

        /// <summary>
        /// Input prompt last line length
        /// </summary>
        public int InputPromptLastLineLength =>
            InputPromptLastLine.Length;

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
        /// Whether the input is concealed right now
        /// </summary>
        public bool Concealing =>
            concealing;

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

        /// <summary>
        /// Whether the current input character is commentized by the hashtag (<c>#</c>) character or not.
        /// </summary>
        public bool Commentized =>
            commentized;

        /// <summary>
        /// Whether an invalid key was pressed, or an invalid operation was performed, or not.
        /// </summary>
        public bool OperationWasInvalid =>
            operationWasInvalid;

        /// <summary>
        /// History entries
        /// </summary>
        public string[] History =>
            HistoryTools.GetHistoryEntries(Settings.HistoryName);

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
        /// Current suggestions text position
        /// </summary>
        public int CurrentSuggestionsTextPos =>
            currentSuggestionsTextPos;

        /// <summary>
        /// Whether one line wrapping is enabled
        /// </summary>
        public bool OneLineWrap =>
            oneLineWrap;

        /// <summary>
        /// Whether the refresh is required
        /// </summary>
        public bool RefreshRequired
        {
            get => refreshRequired;
            set => refreshRequired = value;
        }

        /// <summary>
        /// Argument number to utilize
        /// </summary>
        public int ArgumentNumber
        {
            get
            {
                if (argNumbers.Count == 0)
                    return 1;
                string argNumberStr = new([.. argNumbers]);
                if (!int.TryParse(argNumberStr, out int argNumber))
                    return 1;
                return argNumber;
            }
        }

        /// <summary>
        /// Gets the current state
        /// </summary>
        public static TermReaderState? CurrentState =>
            TermReader.state;

        internal static void SaveState(TermReaderState state)
        {
            if (!TermReaderTools.Busy)
                return;
            if (TermReader.state is null)
                return;
            TermReader.state = state;
        }

        internal TermReaderState() { }
    }
}
