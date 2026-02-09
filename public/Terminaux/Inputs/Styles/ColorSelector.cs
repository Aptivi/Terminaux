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
using System.Collections.Generic;
using Terminaux.Base;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Transformation;
using Colorimetry.Transformation.Formulas;
using Colorimetry.Transformation.Tools;
using Colorimetry.Transformation.Tools.ColorBlind;
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
            { TransformationFormula.Sepia, new Sepia() },
            { TransformationFormula.Cyanotype, new Cyanotype() },
        };

        internal static Keybinding[] Bindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SUBMIT"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_CANCEL"), ConsoleKey.Escape),
        ];
        internal static Keybinding[] AdditionalBindingsGeneral =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_COLORINFO"), ConsoleKey.I),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_NEXTTRANSFORM"), ConsoleKey.N),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_PREVTRANSFORM"), ConsoleKey.M),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASETRANSFORMFREQ"), ConsoleKey.N, ConsoleModifiers.Control),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASETRANSFORMFREQ"), ConsoleKey.M, ConsoleModifiers.Control),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_CHANGETRANSFORM"), PointerButton.Left, PointerButtonPress.Released),
        ];
        internal static Keybinding[] AdditionalBindingsReadWrite =>
        [
            .. AdditionalBindingsGeneral,
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_WEBCOLOR"), ConsoleKey.W),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASEOPAQUENESS"), ConsoleKey.O),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASETANSPARENCY"), ConsoleKey.P),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_NEXTCOLORMODE"), ConsoleKey.Tab),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_PREVCOLORMODE"), ConsoleKey.Tab, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASEVALUE"), PointerButton.WheelDown, PointerButtonPress.Scrolled),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASEVALUE"), PointerButton.WheelUp, PointerButtonPress.Scrolled),
        ];
        internal static Keybinding[] AdditionalBindingsTrueColor =>
        [
            .. AdditionalBindingsReadWrite,
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASEHUE"), ConsoleKey.LeftArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASELIGHTNESS"), ConsoleKey.LeftArrow, ConsoleModifiers.Control),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_DECREASESATURATION"), ConsoleKey.DownArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASEHUE"), ConsoleKey.RightArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASELIGHTNESS"), ConsoleKey.RightArrow, ConsoleModifiers.Control),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_COLORSELECTOR_KEYBINDING_INCREASESATURATION"), ConsoleKey.UpArrow),
        ];
        internal static Keybinding[] AdditionalBindingsNormalColor =>
        [
            .. AdditionalBindingsReadWrite,
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
            ConsoleLogger.Debug("Initial color: {0}", initialColor);
            Color selectedColor = new(initialColor.RGB.R, initialColor.RGB.G, initialColor.RGB.B, finalSettings);
            ConsoleLogger.Debug("Final initial color: {0}", selectedColor);
            var colorSelectorTui = new ColorSelectorTui(selectedColor, finalSettings, readOnly);
            TextualUITools.RunTui(colorSelectorTui);
            var result = colorSelectorTui.GetResultingColor();
            ConsoleLogger.Debug("Result color: {0}", result);
            return result;
        }
    }
}
