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

namespace Terminaux.Writer.FancyWriters.Tools
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
        }
    }
}
