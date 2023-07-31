
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

namespace Terminaux.Reader
{
    /// <summary>
    /// Settings for the reader
    /// </summary>
    public class TermReaderSettings
    {
        private char passwordMaskChar = '*';
        private bool historyEnabled = true;
        private bool treatCtrlCAsInput;
        private int leftMargin = 0;
        private int rightMargin = 0;
        internal Func<string, int, char[], string[]> suggestions = (_, _, _) => Array.Empty<string>();
        internal char[] suggestionsDelims = new char[] { ' ' };

        /// <summary>
        /// Password mask character
        /// </summary>
        public char PasswordMaskChar 
        { 
            get => passwordMaskChar;
            set => passwordMaskChar = value; 
        }

        /// <summary>
        /// Input history enabled
        /// </summary>
        public bool HistoryEnabled
        {
            get => historyEnabled;
            set => historyEnabled = value;
        }

        /// <summary>
        /// Left margin
        /// </summary>
        public int LeftMargin
        {
            get => leftMargin;
            set => leftMargin = value;
        }

        /// <summary>
        /// Right margin
        /// </summary>
        public int RightMargin
        {
            get => rightMargin;
            set => rightMargin = value;
        }

        /// <summary>
        /// Suggestion entries
        /// </summary>
        public Func<string, int, char[], string[]> Suggestions
        {
            set => suggestions = value ?? ((_, _, _) => Array.Empty<string>());
        }

        /// <summary>
        /// Suggestion delimiters
        /// </summary>
        public char[] SuggestionsDelimiters
        {
            set => suggestionsDelims = value ?? new char[] { ' ' };
        }

        /// <summary>
        /// Treat Ctrl + C as input
        /// </summary>
        public bool TreatCtrlCAsInput
        {
            get => treatCtrlCAsInput;
            set => treatCtrlCAsInput = value;
        }

        /// <summary>
        /// Initializes empty settings instance
        /// </summary>
        public TermReaderSettings() { }
    }
}
