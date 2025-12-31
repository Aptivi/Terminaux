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

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// All border tools here
    /// </summary>
    public class BorderSettings
    {
        private static readonly BorderSettings globalSettings = new();
        internal char _borderUpperLeftCornerChar = '╭';
        internal char _borderUpperRightCornerChar = '╮';
        internal char _borderLowerLeftCornerChar = '╰';
        internal char _borderLowerRightCornerChar = '╯';
        internal char _borderUpperFrameChar = '─';
        internal char _borderLowerFrameChar = '─';
        internal char _borderLeftFrameChar = '│';
        internal char _borderRightFrameChar = '│';
        internal char _borderLeftHorizontalIntersectionChar = '├';
        internal char _borderRightHorizontalIntersectionChar = '┤';
        internal char _borderHorizontalIntersectionChar = '─';
        internal char _borderTopVerticalIntersectionChar = '┬';
        internal char _borderBottomVerticalIntersectionChar = '┴';
        internal char _borderVerticalIntersectionChar = '│';
        internal char _borderWholeIntersectionChar = '┼';
        internal bool _borderUpperLeftCornerEnabled = true;
        internal bool _borderUpperRightCornerEnabled = true;
        internal bool _borderLowerLeftCornerEnabled = true;
        internal bool _borderLowerRightCornerEnabled = true;
        internal bool _borderUpperFrameEnabled = true;
        internal bool _borderLowerFrameEnabled = true;
        internal bool _borderLeftFrameEnabled = true;
        internal bool _borderRightFrameEnabled = true;
        internal bool _borderLeftHorizontalIntersectionEnabled = true;
        internal bool _borderRightHorizontalIntersectionEnabled = true;
        internal bool _borderHorizontalIntersectionEnabled = true;
        internal bool _borderTopVerticalIntersectionEnabled = true;
        internal bool _borderBottomVerticalIntersectionEnabled = true;
        internal bool _borderVerticalIntersectionEnabled = true;
        internal bool _borderWholeIntersectionEnabled = true;

        /// <summary>
        /// Global border settings
        /// </summary>
        public static BorderSettings GlobalSettings =>
            globalSettings;

        /// <summary>
        /// Upper left corner character 
        /// </summary>
        public char BorderUpperLeftCornerChar
        {
            get => _borderUpperLeftCornerChar;
            set => _borderUpperLeftCornerChar = value;
        }
        /// <summary>
        /// Upper right corner character 
        /// </summary>
        public char BorderUpperRightCornerChar
        {
            get => _borderUpperRightCornerChar;
            set => _borderUpperRightCornerChar = value;
        }
        /// <summary>
        /// Lower left corner character 
        /// </summary>
        public char BorderLowerLeftCornerChar
        {
            get => _borderLowerLeftCornerChar;
            set => _borderLowerLeftCornerChar = value;
        }
        /// <summary>
        /// Lower right corner character 
        /// </summary>
        public char BorderLowerRightCornerChar
        {
            get => _borderLowerRightCornerChar;
            set => _borderLowerRightCornerChar = value;
        }
        /// <summary>
        /// Upper frame character 
        /// </summary>
        public char BorderUpperFrameChar
        {
            get => _borderUpperFrameChar;
            set => _borderUpperFrameChar = value;
        }
        /// <summary>
        /// Lower frame character 
        /// </summary>
        public char BorderLowerFrameChar
        {
            get => _borderLowerFrameChar;
            set => _borderLowerFrameChar = value;
        }
        /// <summary>
        /// Left frame character 
        /// </summary>
        public char BorderLeftFrameChar
        {
            get => _borderLeftFrameChar;
            set => _borderLeftFrameChar = value;
        }
        /// <summary>
        /// Right frame character 
        /// </summary>
        public char BorderRightFrameChar
        {
            get => _borderRightFrameChar;
            set => _borderRightFrameChar = value;
        }
        /// <summary>
        /// Left horizontal intersection character 
        /// </summary>
        public char BorderLeftHorizontalIntersectionChar
        {
            get => _borderLeftHorizontalIntersectionChar;
            set => _borderLeftHorizontalIntersectionChar = value;
        }
        /// <summary>
        /// Right horizontal intersection character 
        /// </summary>
        public char BorderRightHorizontalIntersectionChar
        {
            get => _borderRightHorizontalIntersectionChar;
            set => _borderRightHorizontalIntersectionChar = value;
        }
        /// <summary>
        /// Horizontal intersection character 
        /// </summary>
        public char BorderHorizontalIntersectionChar
        {
            get => _borderHorizontalIntersectionChar;
            set => _borderHorizontalIntersectionChar = value;
        }
        /// <summary>
        /// Top vertical intersection character 
        /// </summary>
        public char BorderTopVerticalIntersectionChar
        {
            get => _borderTopVerticalIntersectionChar;
            set => _borderTopVerticalIntersectionChar = value;
        }
        /// <summary>
        /// Bottom vertical intersection character 
        /// </summary>
        public char BorderBottomVerticalIntersectionChar
        {
            get => _borderBottomVerticalIntersectionChar;
            set => _borderBottomVerticalIntersectionChar = value;
        }
        /// <summary>
        /// Vertical intersection character 
        /// </summary>
        public char BorderVerticalIntersectionChar
        {
            get => _borderVerticalIntersectionChar;
            set => _borderVerticalIntersectionChar = value;
        }
        /// <summary>
        /// Whole intersection character 
        /// </summary>
        public char BorderWholeIntersectionChar
        {
            get => _borderWholeIntersectionChar;
            set => _borderWholeIntersectionChar = value;
        }

        /// <summary>
        /// Upper left corner enabled 
        /// </summary>
        public bool BorderUpperLeftCornerEnabled
        {
            get => _borderUpperLeftCornerEnabled;
            set => _borderUpperLeftCornerEnabled = value;
        }
        /// <summary>
        /// Upper right corner enabled 
        /// </summary>
        public bool BorderUpperRightCornerEnabled
        {
            get => _borderUpperRightCornerEnabled;
            set => _borderUpperRightCornerEnabled = value;
        }
        /// <summary>
        /// Lower left corner enabled 
        /// </summary>
        public bool BorderLowerLeftCornerEnabled
        {
            get => _borderLowerLeftCornerEnabled;
            set => _borderLowerLeftCornerEnabled = value;
        }
        /// <summary>
        /// Lower right corner enabled 
        /// </summary>
        public bool BorderLowerRightCornerEnabled
        {
            get => _borderLowerRightCornerEnabled;
            set => _borderLowerRightCornerEnabled = value;
        }
        /// <summary>
        /// Upper frame enabled 
        /// </summary>
        public bool BorderUpperFrameEnabled
        {
            get => _borderUpperFrameEnabled;
            set => _borderUpperFrameEnabled = value;
        }
        /// <summary>
        /// Lower frame enabled 
        /// </summary>
        public bool BorderLowerFrameEnabled
        {
            get => _borderLowerFrameEnabled;
            set => _borderLowerFrameEnabled = value;
        }
        /// <summary>
        /// Left frame enabled 
        /// </summary>
        public bool BorderLeftFrameEnabled
        {
            get => _borderLeftFrameEnabled;
            set => _borderLeftFrameEnabled = value;
        }
        /// <summary>
        /// Right frame enabled 
        /// </summary>
        public bool BorderRightFrameEnabled
        {
            get => _borderRightFrameEnabled;
            set => _borderRightFrameEnabled = value;
        }
        /// <summary>
        /// Left horizontal intersection enabled 
        /// </summary>
        public bool BorderLeftHorizontalIntersectionEnabled
        {
            get => _borderLeftHorizontalIntersectionEnabled;
            set => _borderLeftHorizontalIntersectionEnabled = value;
        }
        /// <summary>
        /// Right horizontal intersection enabled 
        /// </summary>
        public bool BorderRightHorizontalIntersectionEnabled
        {
            get => _borderRightHorizontalIntersectionEnabled;
            set => _borderRightHorizontalIntersectionEnabled = value;
        }
        /// <summary>
        /// Horizontal intersection enabled 
        /// </summary>
        public bool BorderHorizontalIntersectionEnabled
        {
            get => _borderHorizontalIntersectionEnabled;
            set => _borderHorizontalIntersectionEnabled = value;
        }
        /// <summary>
        /// Top vertical intersection enabled 
        /// </summary>
        public bool BorderTopVerticalIntersectionEnabled
        {
            get => _borderTopVerticalIntersectionEnabled;
            set => _borderTopVerticalIntersectionEnabled = value;
        }
        /// <summary>
        /// Bottom vertical intersection enabled 
        /// </summary>
        public bool BorderBottomVerticalIntersectionEnabled
        {
            get => _borderBottomVerticalIntersectionEnabled;
            set => _borderBottomVerticalIntersectionEnabled = value;
        }
        /// <summary>
        /// Vertical intersection enabled 
        /// </summary>
        public bool BorderVerticalIntersectionEnabled
        {
            get => _borderVerticalIntersectionEnabled;
            set => _borderVerticalIntersectionEnabled = value;
        }
        /// <summary>
        /// Whole intersection enabled 
        /// </summary>
        public bool BorderWholeIntersectionEnabled
        {
            get => _borderWholeIntersectionEnabled;
            set => _borderWholeIntersectionEnabled = value;
        }

        /// <summary>
        /// Makes a new instance of the border settings
        /// </summary>
        public BorderSettings()
        { }

        /// <summary>
        /// Makes a new instance of the border settings with the copied settings
        /// </summary>
        /// <param name="settings">Settings instance to copy settings from</param>
        public BorderSettings(BorderSettings settings)
        {
            BorderUpperLeftCornerChar = settings.BorderUpperLeftCornerChar;
            BorderUpperRightCornerChar = settings.BorderUpperRightCornerChar;
            BorderLowerLeftCornerChar = settings.BorderLowerLeftCornerChar;
            BorderLowerRightCornerChar = settings.BorderLowerRightCornerChar;
            BorderUpperFrameChar = settings.BorderUpperFrameChar;
            BorderLowerFrameChar = settings.BorderLowerFrameChar;
            BorderLeftFrameChar = settings.BorderLeftFrameChar;
            BorderRightFrameChar = settings.BorderRightFrameChar;
            BorderLeftHorizontalIntersectionChar = settings.BorderLeftHorizontalIntersectionChar;
            BorderRightHorizontalIntersectionChar = settings.BorderRightHorizontalIntersectionChar;
            BorderHorizontalIntersectionChar = settings.BorderHorizontalIntersectionChar;
            BorderTopVerticalIntersectionChar = settings.BorderTopVerticalIntersectionChar;
            BorderBottomVerticalIntersectionChar = settings.BorderBottomVerticalIntersectionChar;
            BorderVerticalIntersectionChar = settings.BorderVerticalIntersectionChar;
            BorderWholeIntersectionChar = settings.BorderWholeIntersectionChar;
            BorderUpperLeftCornerEnabled = settings.BorderUpperLeftCornerEnabled;
            BorderUpperRightCornerEnabled = settings.BorderUpperRightCornerEnabled;
            BorderLowerLeftCornerEnabled = settings.BorderLowerLeftCornerEnabled;
            BorderLowerRightCornerEnabled = settings.BorderLowerRightCornerEnabled;
            BorderUpperFrameEnabled = settings.BorderUpperFrameEnabled;
            BorderLowerFrameEnabled = settings.BorderLowerFrameEnabled;
            BorderLeftFrameEnabled = settings.BorderLeftFrameEnabled;
            BorderRightFrameEnabled = settings.BorderRightFrameEnabled;
            BorderLeftHorizontalIntersectionEnabled = settings.BorderLeftHorizontalIntersectionEnabled;
            BorderRightHorizontalIntersectionEnabled = settings.BorderRightHorizontalIntersectionEnabled;
            BorderHorizontalIntersectionEnabled = settings.BorderHorizontalIntersectionEnabled;
            BorderTopVerticalIntersectionEnabled = settings.BorderTopVerticalIntersectionEnabled;
            BorderBottomVerticalIntersectionEnabled = settings.BorderBottomVerticalIntersectionEnabled;
            BorderVerticalIntersectionEnabled = settings.BorderVerticalIntersectionEnabled;
            BorderWholeIntersectionEnabled = settings.BorderWholeIntersectionEnabled;
        }
    }
}
