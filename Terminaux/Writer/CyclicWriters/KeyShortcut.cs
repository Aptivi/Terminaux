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
using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// KeyShortcut renderable
    /// </summary>
    public class KeyShortcut : IStaticRenderable
    {
        private Keybinding? shortcut;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private Color optionColor = ConsoleColors.Black;
        private Color optionForegroundColor = ConsoleColors.Yellow;
        private Color optionBackgroundColor = ConsoleColors.Olive;
        private bool useColors = true;
        private bool writeLabel = true;

        /// <summary>
        /// Key binding
        /// </summary>
        public Keybinding? Shortcut
        {
            get => shortcut;
            set => shortcut = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Keybinding option key color
        /// </summary>
        public Color OptionColor
        {
            get => optionColor;
            set => optionColor = value;
        }

        /// <summary>
        /// Keybinding option key name color
        /// </summary>
        public Color OptionForegroundColor
        {
            get => optionForegroundColor;
            set => optionForegroundColor = value;
        }

        /// <summary>
        /// Keybinding option key background color
        /// </summary>
        public Color OptionBackgroundColor
        {
            get => optionBackgroundColor;
            set => optionBackgroundColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Whether to write label or not
        /// </summary>
        public bool WriteLabel
        {
            get => writeLabel;
            set => writeLabel = value;
        }

        /// <summary>
        /// Width of the key shortcut
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Renders a KeyShortcut segment group
        /// </summary>
        /// <returns>Rendered KeyShortcut text that will be used by the renderer</returns>
        public string Render()
        {
            var bindingBuilder = new StringBuilder();
            if (Shortcut is null || Shortcut.BindingUsesMouse)
                return "";

            // First, check to see if the rendered binding info is going to exceed the console window width
            string renderedBinding = $"{KeybindingTools.GetBindingKeyShortcut(Shortcut, false)} {Shortcut.BindingName}  ";
            int bindingLength = ConsoleChar.EstimateCellWidth(renderedBinding);
            int actualLength = ConsoleChar.EstimateCellWidth(VtSequenceTools.FilterVTSequences(bindingBuilder.ToString()));
            bool canDraw = bindingLength + actualLength < Width;
            if (canDraw)
            {
                bindingBuilder.Append(
                    $"{(UseColors ? ColorTools.RenderSetConsoleColor(optionColor, false, true) : "")}" +
                    $"{(UseColors ? ColorTools.RenderSetConsoleColor(optionBackgroundColor, true) : "")}" +
                    KeybindingTools.GetBindingKeyShortcut(Shortcut, false)
                );
                if (WriteLabel)
                {
                    bindingBuilder.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(optionForegroundColor) : "")}" +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(backgroundColor, true) : "")}" +
                        $" {Shortcut.BindingName}  "
                    );
                }
                else
                {
                    bindingBuilder.Append(
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(optionForegroundColor) : "")}" +
                        $"{(UseColors ? ColorTools.RenderSetConsoleColor(backgroundColor, true) : "")}" +
                        " "
                    );
                }
                if (UseColors)
                {
                    bindingBuilder.Append(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
            }
            return bindingBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the KeyShortcut renderer
        /// </summary>
        public KeyShortcut()
        { }
    }
}
