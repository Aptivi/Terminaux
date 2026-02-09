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
using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Sequences;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Keybindings renderable
    /// </summary>
    public class Keybindings : SimpleCyclicWriter
    {
        private Keybinding[] keybindings = [];
        private Keybinding[] builtinKeybindings = [];
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private Color builtinColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltin);
        private Color builtinForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinForeground);
        private Color builtinBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinBackground);
        private Color optionColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingOption);
        private Color optionForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionForeground);
        private Color optionBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionBackground);
        private bool useColors = true;
        private bool writeLabels = true;
        private bool writeHelpKeyInfo = true;

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
        /// Whether to write help key info at the end of the keybinding bar upon overflow
        /// </summary>
        public bool WriteHelpKeyInfo
        {
            get => writeHelpKeyInfo;
            set => writeHelpKeyInfo = value;
        }

        /// <summary>
        /// Help key info
        /// </summary>
        public ConsoleKeyInfo? HelpKeyInfo { get; set; }

        /// <summary>
        /// Renders the keybindings
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            var bindingsBuilder = new StringBuilder();
            var helpKeyInfo = HelpKeyInfo ?? new('K', ConsoleKey.K, false, false, false);
            Keybinding[] finalBindings = [.. builtinKeybindings, .. keybindings];
            finalBindings = [.. finalBindings.Where((kb) => !kb.BindingHidden)];
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
                int maxLength = Width - bindingExtraLength;
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
                            UseColors = UseColors,
                            Width = maxLength,
                            WriteLabel = writeLabels,
                        }.Render()
                    );
                }
                else if (writeHelpKeyInfo)
                {
                    // We can't render anymore, so just break and write a binding to show more if it's provided
                    int spaces = Width - actualLength - bindingExtraLength + 1;
                    if (spaces <= 0)
                        break;
                    bindingsBuilder.Append(
                        new string(' ', spaces) +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(builtinColor, false, true) : "")}" +
                        $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(builtinBackgroundColor, true) : "")}" +
                        renderedExtraBinding
                    );
                    break;
                }
            }
            if (UseColors)
            {
                bindingsBuilder.Append(
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }
            return bindingsBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the keybindings renderer
        /// </summary>
        public Keybindings()
        { }
    }
}
