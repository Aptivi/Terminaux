/*
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

namespace Terminaux.Reader
{
    /// <summary>
    /// Settings for the reader
    /// </summary>
    public static class TermReaderSettings
    {
        private static char passwordMaskChar = '*';
        private static bool historyEnabled = true;
        private static bool treatCtrlCAsInput;
        private static int leftMargin = 0;
        private static int rightMargin = 0;
        internal static Func<string, int, char[], string[]> suggestions = (_, _, _) => Array.Empty<string>();
        internal static char[] suggestionsDelims = new char[] { ' ' };

        /// <summary>
        /// Password mask character
        /// </summary>
        public static char PasswordMaskChar 
        { 
            get => passwordMaskChar;
            set => passwordMaskChar = value; 
        }

        /// <summary>
        /// Input history enabled
        /// </summary>
        public static bool HistoryEnabled
        {
            get => historyEnabled;
            set => historyEnabled = value;
        }

        /// <summary>
        /// Left margin
        /// </summary>
        public static int LeftMargin
        {
            get => leftMargin;
            set => leftMargin = value;
        }

        /// <summary>
        /// Right margin
        /// </summary>
        public static int RightMargin
        {
            get => rightMargin;
            set => rightMargin = value;
        }

        /// <summary>
        /// Suggestion entries
        /// </summary>
        public static Func<string, int, char[], string[]> Suggestions
        {
            set => suggestions = value ?? ((_, _, _) => Array.Empty<string>());
        }

        /// <summary>
        /// Suggestion delimiters
        /// </summary>
        public static char[] SuggestionsDelimiters
        {
            set => suggestionsDelims = value ?? new char[] { ' ' };
        }

        /// <summary>
        /// Treat Ctrl + C as input
        /// </summary>
        public static bool TreatCtrlCAsInput
        {
            get => treatCtrlCAsInput;
            set => treatCtrlCAsInput = value;
        }
    }
}
