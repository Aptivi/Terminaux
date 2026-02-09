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

using Terminaux.Base.Structures;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Themes.Colors;

namespace Terminaux.Inputs
{
    /// <summary>
    /// Abstract input module class
    /// </summary>
    public abstract class InputModule
    {
        /// <summary>
        /// Gets the input name
        /// </summary>
        public string Name { get; set; } = "";
        
        /// <summary>
        /// Gets the input description
        /// </summary>
        public string Description { get; set; } = "";
        
        /// <summary>
        /// Gets the processed value
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Whether to use the colors or not
        /// </summary>
        public bool UseColor { get; set; } = true;

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color Foreground { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Input);

        /// <summary>
        /// Blank foreground color
        /// </summary>
        public Color BlankForeground { get; set; } = ConsoleColors.Grey;

        /// <summary>
        /// Background color
        /// </summary>
        public Color Background { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// If the input is provided or not
        /// </summary>
        public bool Provided { get; protected set; }

        /// <summary>
        /// Extra popover height to add
        /// </summary>
        public virtual int ExtraPopoverHeight { get; protected set; }

        /// <summary>
        /// Gets a strongly-typed value
        /// </summary>
        /// <typeparam name="T">Target value type</typeparam>
        /// <returns>Strongly-typed value</returns>
        public T? GetValue<T>() =>
            (T?)Value;

        /// <summary>
        /// Renders the input before the control goes to this input module (before <see cref="ProcessInput"/> is called)
        /// </summary>
        /// <param name="width">Input width (excluding the input name width)</param>
        /// <returns>A string that can be rendered to the console</returns>
        public abstract string RenderInput(int width);

        /// <summary>
        /// Processes the input by prompting user
        /// </summary>
        /// <param name="inputPopoverPos">Input popover position. If there is no popover, it can be the same as the beginning of the input</param>
        /// <param name="inputPopoverSize">Input popover size. If there is no popover, it can be the same as the input height and/or width</param>
        public abstract void ProcessInput(Coordinate inputPopoverPos = default, Size inputPopoverSize = default);
    }
}
