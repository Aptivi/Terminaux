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
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Reader.Highlighting;
using Terminaux.Reader.History;

namespace Terminaux.Reader
{
    /// <summary>
    /// Settings for the reader
    /// </summary>
    public class TermReaderSettings
    {
        private char passwordMaskChar = '*';
        private bool historyEnabled = true;
        private string historyName = HistoryTools.generalHistory;
        private bool treatCtrlCAsInput;
        private bool keyboardCues;
        private int leftMargin = 0;
        private int rightMargin = 0;
        private Color? inputForegroundColor;
        private Color? inputBackgroundColor;
        private Color? inputPlaceholderForegroundColor;
        private Color? inputPromptForegroundColor;
        private SyntaxHighlighting? syntaxHighlighter;
        private bool syntaxHighlighterEnabled;
        private string placeholderText = "";
        private bool printDefaultValue;
        private string defaultValueFormat = "[{0}] ";
        private string bassBoomLibraryRoot = "";
        internal TermReaderState? state;
        internal Func<string, int, char[], string[]> suggestions = (_, _, _) => [];
        internal char[] suggestionsDelims = [' '];

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
        /// Input history name
        /// </summary>
        public string HistoryName
        {
            get => historyName;
            set
            {
                if (HistoryTools.IsHistoryRegistered(value))
                    historyName = value;
                else
                    historyName = HistoryTools.generalHistory;
            }
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
        /// Input foreground color
        /// </summary>
        public Color InputForegroundColor
        {
            get => inputForegroundColor ?? ColorTools.currentForegroundColor;
            set => inputForegroundColor = value;
        }

        /// <summary>
        /// Input background color
        /// </summary>
        public Color InputBackgroundColor
        {
            get => inputBackgroundColor ?? ColorTools.currentBackgroundColor;
            set => inputBackgroundColor = value;
        }

        /// <summary>
        /// Input placeholder foreground color
        /// </summary>
        public Color InputPlaceholderForegroundColor
        {
            get => inputPlaceholderForegroundColor ?? new Color(ConsoleColors.Grey);
            set => inputPlaceholderForegroundColor = value;
        }

        /// <summary>
        /// Input prompt foreground color
        /// </summary>
        public Color InputPromptForegroundColor
        {
            get => inputPromptForegroundColor ?? ColorTools.currentForegroundColor;
            set => inputPromptForegroundColor = value;
        }

        /// <summary>
        /// Suggestion entries
        /// </summary>
        public Func<string, int, char[], string[]> Suggestions
        {
            set => suggestions = value ?? ((_, _, _) => []);
        }

        /// <summary>
        /// Suggestion delimiters
        /// </summary>
        public char[] SuggestionsDelimiters
        {
            set => suggestionsDelims = value ?? [' '];
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
        /// Syntax highlighter is enabled
        /// </summary>
        public bool SyntaxHighlighterEnabled
        {
            get => syntaxHighlighterEnabled;
            set => syntaxHighlighterEnabled = value;
        }

        /// <summary>
        /// Syntax highlighter to use. It must be registered.
        /// </summary>
        public SyntaxHighlighting? SyntaxHighlighter
        {
            get => syntaxHighlighter;
            set => syntaxHighlighter = value ?? SyntaxHighlightingTools.GetHighlighter("Command");
        }

        /// <summary>
        /// Prints the default value
        /// </summary>
        public bool PrintDefaultValue
        {
            get => printDefaultValue;
            set => printDefaultValue = value;
        }

        /// <summary>
        /// Default value format when showing it is enabled
        /// </summary>
        public string DefaultValueFormat
        {
            get => defaultValueFormat ?? "[{0}] ";
            set => defaultValueFormat = value;
        }

        /// <summary>
        /// Show placeholder hint text
        /// </summary>
        public string PlaceholderText
        {
            get => placeholderText ?? "";
            set => placeholderText = value;
        }

        /// <summary>
        /// Play keyboard cues for each keypress
        /// </summary>
        public bool KeyboardCues
        {
            get => keyboardCues;
            set => keyboardCues = value;
        }

        /// <summary>
        /// Root path to BassBoom's library path
        /// </summary>
        public string BassBoomLibraryPath
        {
            get => bassBoomLibraryRoot ?? "";
            set => bassBoomLibraryRoot = value;
        }

        /// <summary>
        /// Initializes an empty reader settings instance
        /// </summary>
        public TermReaderSettings()
        { }

        /// <summary>
        /// Initializes a reader settings instance
        /// </summary>
        /// <param name="settings">Settings to copy from</param>
        public TermReaderSettings(TermReaderSettings settings)
        {
            if (settings == null)
                return;

            PasswordMaskChar = settings.PasswordMaskChar;
            HistoryEnabled = settings.HistoryEnabled;
            LeftMargin = settings.LeftMargin;
            RightMargin = settings.RightMargin;
            InputForegroundColor = settings.InputForegroundColor;
            InputBackgroundColor = settings.InputBackgroundColor;
            Suggestions = settings.suggestions;
            SuggestionsDelimiters = settings.suggestionsDelims;
            TreatCtrlCAsInput = settings.TreatCtrlCAsInput;
            SyntaxHighlighterEnabled = settings.SyntaxHighlighterEnabled;
            SyntaxHighlighter = settings.SyntaxHighlighter;
            PlaceholderText = settings.PlaceholderText;
            KeyboardCues = settings.KeyboardCues;
        }
    }
}
