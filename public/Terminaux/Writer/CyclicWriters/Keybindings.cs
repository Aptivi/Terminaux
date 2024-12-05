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
    /// Keybindings renderable
    /// </summary>
    public class Keybindings : IStaticRenderable
    {
        private Keybinding[] keybindings = [];
        private Keybinding[] builtinKeybindings = [];
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private Color builtinColor = ConsoleColors.Black;
        private Color builtinForegroundColor = ConsoleColors.Lime;
        private Color builtinBackgroundColor = ConsoleColors.Green;
        private Color optionColor = ConsoleColors.Black;
        private Color optionForegroundColor = ConsoleColors.Yellow;
        private Color optionBackgroundColor = ConsoleColors.Olive;
        private bool useColors = true;
        private bool writeLabels = true;

        /// <summary>
        /// Key bindings
        /// </summary>
        public Keybinding[] KeybindingList
        {
            get => keybindings;
            set => keybindings = value;
        }

        /// <summary>
        /// Built-in key bindings
        /// </summary>
        public Keybinding[] BuiltinKeybindings
        {
            get => builtinKeybindings;
            set => builtinKeybindings = value;
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
        /// Built-in option key color
        /// </summary>
        public Color BuiltinColor
        {
            get => builtinColor;
            set => builtinColor = value;
        }

        /// <summary>
        /// Built-in option key name color
        /// </summary>
        public Color BuiltinForegroundColor
        {
            get => builtinForegroundColor;
            set => builtinForegroundColor = value;
        }

        /// <summary>
        /// Built-in option key background color
        /// </summary>
        public Color BuiltinBackgroundColor
        {
            get => builtinBackgroundColor;
            set => builtinBackgroundColor = value;
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
        /// Left position
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Width of the keybinding bar
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Whether to write labels or not
        /// </summary>
        public bool WriteLabels
        {
            get => writeLabels;
            set => writeLabels = value;
        }

        /// <summary>
        /// Help key info
        /// </summary>
        public ConsoleKeyInfo? HelpKeyInfo { get; set; }

        /// <summary>
        /// Renders a Keybindings segment group
        /// </summary>
        /// <returns>Rendered Keybindings text that will be used by the renderer</returns>
        public string Render() =>
            TextWriterWhereColor.RenderWhere(
                RenderKeybindings(KeybindingList, BuiltinKeybindings, BuiltinColor, BuiltinForegroundColor, BuiltinBackgroundColor, OptionColor, OptionForegroundColor, OptionBackgroundColor, BackgroundColor, Width, HelpKeyInfo, UseColors, WriteLabels), Left, Top);

        internal static string RenderKeybindings(Keybinding[] bindings, Keybinding[] builtinKeybindings, Color builtinColor, Color builtinForegroundColor, Color builtinBackgroundColor, Color optionColor, Color optionForegroundColor, Color optionBackgroundColor, Color backgroundColor, int width, ConsoleKeyInfo? helpKeyInfo, bool useColor = true, bool writeLabels = true)
        {
            var bindingsBuilder = new StringBuilder();
            helpKeyInfo ??= new('K', ConsoleKey.K, false, false, false);
            Keybinding[] finalBindings = [.. builtinKeybindings, .. bindings];
            foreach (Keybinding binding in finalBindings)
            {
                // Check the binding mode
                if (binding.BindingUsesMouse)
                    continue;

                // First, check to see if the rendered binding info is going to exceed the console window width
                string renderedBinding = $"{KeybindingTools.GetBindingKeyShortcut(binding, false)} {binding.BindingName}  ";
                string renderedExtraBinding = KeybindingTools.GetBindingKeyShortcut(helpKeyInfo, false);
                int bindingLength = ConsoleChar.EstimateCellWidth(renderedBinding);
                int bindingExtraLength = ConsoleChar.EstimateCellWidth(renderedExtraBinding);
                int actualLength = ConsoleChar.EstimateCellWidth(VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()));
                int maxLength = width - bindingExtraLength;
                bool canDraw = bindingLength + actualLength < maxLength;
                bool isBuiltin = builtinKeybindings.Contains(binding);
                if (canDraw)
                {
                    bindingsBuilder.Append(
                        new KeyShortcut()
                        {
                            Shortcut = binding,
                            BackgroundColor = backgroundColor,
                            OptionColor = isBuiltin ? builtinColor : optionColor,
                            OptionBackgroundColor = isBuiltin ? builtinBackgroundColor : optionBackgroundColor,
                            OptionForegroundColor = isBuiltin ? builtinForegroundColor : optionForegroundColor,
                            UseColors = useColor,
                            Width = maxLength,
                            WriteLabel = writeLabels,
                        }.Render()
                    );
                }
                else
                {
                    // We can't render anymore, so just break and write a binding to show more if it's provided
                    int spaces = width - actualLength - bindingExtraLength;
                    if (spaces <= 0)
                        break;
                    bindingsBuilder.Append(
                        new string(' ', spaces) +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(builtinColor, false, true) : "")}" +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(builtinBackgroundColor, true) : "")}" +
                        renderedExtraBinding
                    );
                    break;
                }
            }
            if (useColor)
            {
                bindingsBuilder.Append(
                    ColorTools.RenderRevertForeground() +
                    ColorTools.RenderRevertBackground()
                );
            }
            return bindingsBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the Keybindings renderer
        /// </summary>
        public Keybindings()
        { }
    }
}
