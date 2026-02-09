//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using System.IO;
using Terminaux.Base;
using Terminaux.Base.Extensions.Data;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Inputs;
using Terminaux.Reader.Highlighting;
using Terminaux.Reader.History;
using Terminaux.Themes.Colors;

namespace Terminaux.Reader
{
    /// <summary>
    /// Settings for the reader
    /// </summary>
    public class TermReaderSettings
    {
        internal TermReaderState? state;
        internal Func<string, int, char[], string[]> suggestions = (_, _, _) => [];
        internal char[] suggestionsDelims = [' '];
        private char passwordMaskChar = Input.PasswordMaskChar;
        private bool historyEnabled = true;
        private string historyName = HistoryTools.generalHistory;
        private bool treatCtrlCAsInput;
        private bool keyboardCues;
        private bool playWriteCue = true;
        private bool playRuboutCue = true;
        private bool playEnterCue = true;
        private double cueVolume = 1;
        private bool cueVolumeBoost;
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
        private bool writeDefaultValue;
        private string defaultValueFormat = "[{0}] ";
        private Stream cueEnter = Input.cueEnterFallback;
        private Stream cueRubout = Input.cueRuboutFallback;
        private Stream cueWrite = Input.cueWriteFallback;
        private int initialPosition = -1;
        private ConsoleBell bell = ConsoleBell.Audible;
        private int width;

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
        /// Width of the reader
        /// </summary>
        public int Width
        {
            get => width <= 0 ? ConsoleWrapper.WindowWidth : width;
            set => width = value;
        }

        /// <summary>
        /// Input foreground color
        /// </summary>
        public Color InputForegroundColor
        {
            get => inputForegroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.Input);
            set => inputForegroundColor = value;
        }

        /// <summary>
        /// Input background color
        /// </summary>
        public Color InputBackgroundColor
        {
            get => inputBackgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.Background);
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
            get => inputPromptForegroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
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
        /// Prints the default value. Conflicts with <see cref="WriteDefaultValue"/>
        /// </summary>
        public bool PrintDefaultValue
        {
            get => printDefaultValue;
            set
            {
                printDefaultValue = value;
                writeDefaultValue = false;
            }
        }

        /// <summary>
        /// Writes the default value to the actual input. Conflicts with <see cref="PrintDefaultValue"/>
        /// </summary>
        public bool WriteDefaultValue
        {
            get => writeDefaultValue;
            set
            {
                writeDefaultValue = value;
                printDefaultValue = false;
            }
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
        /// Play keyboard cues for character insertion and other actions
        /// </summary>
        public bool PlayWriteCue
        {
            get => playWriteCue;
            set => playWriteCue = value;
        }

        /// <summary>
        /// Play keyboard cues for pressing Backspace
        /// </summary>
        public bool PlayRuboutCue
        {
            get => playRuboutCue;
            set => playRuboutCue = value;
        }

        /// <summary>
        /// Play keyboard cues for pressing Enter
        /// </summary>
        public bool PlayEnterCue
        {
            get => playEnterCue;
            set => playEnterCue = value;
        }

        /// <summary>
        /// Keyboard cue volume
        /// </summary>
        public double CueVolume
        {
            get => cueVolume;
            set
            {
                cueVolume = value;
                if (cueVolume < 0)
                    cueVolume = 0;
                if (cueVolume > 1)
                {
                    if (CueVolumeBoost && cueVolume > 3)
                        cueVolume = 3;
                    else if (!CueVolumeBoost)
                        cueVolume = 1;
                }
            }
        }

        /// <summary>
        /// Whether to boost cue volume up to 3.0 or not
        /// </summary>
        public bool CueVolumeBoost
        {
            get => cueVolumeBoost;
            set => cueVolumeBoost = value;
        }

        /// <summary>
        /// A stream for the submission key press keyboard cue
        /// </summary>
        public Stream CueEnter
        {
            get
            {
                cueEnter.Seek(0, SeekOrigin.Begin);
                return cueEnter;
            }
            set
            {
                if (!value.CanSeek)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_READER_CUE_EXCEPTION_UNSEEKABLE"));
                cueEnter = value;
            }
        }

        /// <summary>
        /// A stream for the backspace key press keyboard cue
        /// </summary>
        public Stream CueRubout
        {
            get
            {
                cueRubout.Seek(0, SeekOrigin.Begin);
                return cueRubout;
            }
            set
            {
                if (!value.CanSeek)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_READER_CUE_EXCEPTION_UNSEEKABLE"));
                cueRubout = value;
            }
        }

        /// <summary>
        /// A stream for the key press keyboard cue
        /// </summary>
        public Stream CueWrite
        {
            get
            {
                cueWrite.Seek(0, SeekOrigin.Begin);
                return cueWrite;
            }
            set
            {
                if (!value.CanSeek)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_READER_CUE_EXCEPTION_UNSEEKABLE"));
                cueWrite = value;
            }
        }

        /// <summary>
        /// Automatically changes the initial position. This is applied only when the default value is supplied and <see cref="WriteDefaultValue"/> is on.
        /// </summary>
        public bool AutoInitialPosition { get; set; } = true;

        /// <summary>
        /// Initial position of the reader. This is applied only when the default value is supplied, <see cref="AutoInitialPosition"/> is off, and <see cref="WriteDefaultValue"/> is on.
        /// </summary>
        public int InitialPosition
        {
            get => initialPosition < 0 ? state?.CurrentText.Length ?? 0 : initialPosition;
            set => initialPosition = value;
        }

        /// <summary>
        /// Console bell type for invalid reader operation
        /// </summary>
        public ConsoleBell Bell
        {
            get => bell;
            set => bell = value;
        }

        /// <summary>
        /// When input is empty, use default value
        /// </summary>
        public bool UseDefaultValueOnEmpty { get; set; }

        /// <summary>
        /// Initializes an empty reader settings instance
        /// </summary>
        public TermReaderSettings() :
            this(TermReader.globalSettings)
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
            HistoryName = settings.HistoryName;
            LeftMargin = settings.LeftMargin;
            RightMargin = settings.RightMargin;
            Width = settings.Width;
            InputForegroundColor = settings.InputForegroundColor;
            InputBackgroundColor = settings.InputBackgroundColor;
            InputPlaceholderForegroundColor = settings.InputPlaceholderForegroundColor;
            InputPromptForegroundColor = settings.InputPromptForegroundColor;
            Suggestions = settings.suggestions;
            SuggestionsDelimiters = settings.suggestionsDelims;
            TreatCtrlCAsInput = settings.TreatCtrlCAsInput;
            SyntaxHighlighterEnabled = settings.SyntaxHighlighterEnabled;
            SyntaxHighlighter = settings.SyntaxHighlighter;
            PrintDefaultValue = settings.PrintDefaultValue;
            WriteDefaultValue = settings.WriteDefaultValue;
            DefaultValueFormat = settings.DefaultValueFormat;
            PlaceholderText = settings.PlaceholderText;
            KeyboardCues = settings.KeyboardCues;
            PlayWriteCue = settings.PlayWriteCue;
            PlayRuboutCue = settings.PlayRuboutCue;
            PlayEnterCue = settings.PlayEnterCue;
            CueVolume = settings.CueVolume;
            CueVolumeBoost = settings.CueVolumeBoost;
            CueEnter = settings.CueEnter;
            CueRubout = settings.CueRubout;
            CueWrite = settings.CueWrite;
            InitialPosition = settings.InitialPosition;
            Bell = settings.Bell;
            PasswordMaskChar = settings.PasswordMaskChar;
        }
    }
}
