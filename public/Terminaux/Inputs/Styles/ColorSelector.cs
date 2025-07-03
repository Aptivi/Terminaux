//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using System.Collections.Generic;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation;
using Terminaux.Colors.Transformation.Formulas;
using Terminaux.Colors.Transformation.Tools;
using Terminaux.Colors.Transformation.Tools.ColorBlind;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Color selection application
    /// </summary>
    public static class ColorSelector
    {
        internal static readonly Dictionary<TransformationFormula, BaseTransformationFormula> transformationFormulas = new()
        {
            { TransformationFormula.Monochromacy, new Monochromacy() },
            { TransformationFormula.Inverse, new Inverse() },
            { TransformationFormula.Protan, new ColorBlind() { Deficiency = ColorBlindDeficiency.Protan } },
            { TransformationFormula.Deutan, new ColorBlind() { Deficiency = ColorBlindDeficiency.Deutan } },
            { TransformationFormula.Tritan, new ColorBlind() { Deficiency = ColorBlindDeficiency.Tritan } },
            { TransformationFormula.ProtanVienot, new ColorBlind() { Deficiency = ColorBlindDeficiency.Protan, Simple = true } },
            { TransformationFormula.DeutanVienot, new ColorBlind() { Deficiency = ColorBlindDeficiency.Deutan, Simple = true } },
            { TransformationFormula.TritanVienot, new ColorBlind() { Deficiency = ColorBlindDeficiency.Tritan, Simple = true } },
            { TransformationFormula.BlueScale, new Monochromacy() { Type = MonochromacyType.Blue } },
            { TransformationFormula.GreenScale, new Monochromacy() { Type = MonochromacyType.Green } },
            { TransformationFormula.RedScale, new Monochromacy() { Type = MonochromacyType.Red } },
            { TransformationFormula.YellowScale, new Monochromacy() { Type = MonochromacyType.Yellow } },
            { TransformationFormula.AquaScale, new Monochromacy() { Type = MonochromacyType.Cyan } },
            { TransformationFormula.PinkScale, new Monochromacy() { Type = MonochromacyType.Magenta } },
        };

        internal readonly static Keybinding[] bindings =
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SUBMIT"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_CANCEL"), ConsoleKey.Escape),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_HELP"), ConsoleKey.H),
        ];
        internal readonly static Keybinding[] additionalBindingsGeneral =
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_COLORINFO"), ConsoleKey.I),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_NEXTTRANSFORM"), ConsoleKey.N),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_PREVTRANSFORM"), ConsoleKey.M),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASETRANSFORMFREQ"), ConsoleKey.N, ConsoleModifiers.Control),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASETRANSFORMFREQ"), ConsoleKey.M, ConsoleModifiers.Control),
        ];
        internal readonly static Keybinding[] additionalBindingsReadWrite =
        [
            .. additionalBindingsGeneral,
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_WEBCOLOR"), ConsoleKey.W),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASEOPAQUENESS"), ConsoleKey.O),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASETANSPARENCY"), ConsoleKey.P),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_NEXTCOLORMODE"), ConsoleKey.Tab),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_PREVCOLORMODE"), ConsoleKey.Tab, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASEVALUE"), PointerButton.WheelDown, PointerButtonPress.Scrolled),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASEVALUE"), PointerButton.WheelUp, PointerButtonPress.Scrolled),
        ];
        internal readonly static Keybinding[] additionalBindingsTrueColor =
        [
            .. additionalBindingsReadWrite,
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASEHUE"), ConsoleKey.LeftArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASELIGHTNESS"), ConsoleKey.LeftArrow, ConsoleModifiers.Control),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASESATURATION"), ConsoleKey.DownArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASEHUE"), ConsoleKey.RightArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASELIGHTNESS"), ConsoleKey.RightArrow, ConsoleModifiers.Control),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASESATURATION"), ConsoleKey.UpArrow),
        ];
        internal readonly static Keybinding[] additionalBindingsNormalColor =
        [
            .. additionalBindingsReadWrite,
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_PREVCOLOR"), ConsoleKey.LeftArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_NEXTCOLOR"), ConsoleKey.RightArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_SHOWHIDECOLORLIST"), ConsoleKey.L),
        ];

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="settings">Settings to use</param>
        /// <param name="readOnly">Whether you need to make the color selector read-only or read-write.</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(ColorSettings? settings = null, bool readOnly = false) =>
            OpenColorSelector(ConsoleColors.White, settings, readOnly);

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <param name="settings">Settings to use</param>
        /// <param name="readOnly">Whether you need to make the color selector read-only or read-write.</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(Color initialColor, ColorSettings? settings = null, bool readOnly = false)
        {
            // Select appropriate settings
            var finalSettings = settings ?? new(ColorTools.GlobalSettings);

            // Initial color is selected
            Color selectedColor = new(initialColor.RGB.R, initialColor.RGB.G, initialColor.RGB.B, finalSettings);
            var colorSelectorTui = new ColorSelectorTui(selectedColor, finalSettings, readOnly);
            TextualUITools.RunTui(colorSelectorTui);
            return colorSelectorTui.GetResultingColor();
        }

        static ColorSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
