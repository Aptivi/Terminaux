﻿//
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
using Terminaux.Base;
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
            { TransformationFormula.Sepia, new Sepia() },
            { TransformationFormula.Cyanotype, new Cyanotype() },
        };

        internal readonly static Keybinding[] bindings =
        [
            new("Submit", ConsoleKey.Enter),
            new("Cancel", ConsoleKey.Escape),
            new("Help", ConsoleKey.H),
        ];
        internal readonly static Keybinding[] additionalBindingsGeneral =
        [
            new("Color information", ConsoleKey.I),
            new("Next color blindness simulation", ConsoleKey.N),
            new("Previous color blindness simulation", ConsoleKey.M),
            new("Increase transformation frequency", ConsoleKey.N, ConsoleModifiers.Control),
            new("Decrease transformation frequency", ConsoleKey.M, ConsoleModifiers.Control),
        ];
        internal readonly static Keybinding[] additionalBindingsReadWrite =
        [
            .. additionalBindingsGeneral,
            new("Select web color", ConsoleKey.W),
            new("Increase opaqueness", ConsoleKey.O),
            new("Increase transparency", ConsoleKey.P),
            new("Next color mode", ConsoleKey.Tab),
            new("Previous color mode", ConsoleKey.Tab, ConsoleModifiers.Shift),
            new("Increase value", PointerButton.WheelDown, PointerButtonPress.Scrolled),
            new("Decrease value", PointerButton.WheelUp, PointerButtonPress.Scrolled),
        ];
        internal readonly static Keybinding[] additionalBindingsTrueColor =
        [
            .. additionalBindingsReadWrite,
            new("Reduce color hue", ConsoleKey.LeftArrow),
            new("Reduce color lightness", ConsoleKey.LeftArrow, ConsoleModifiers.Control),
            new("Reduce saturation", ConsoleKey.DownArrow),
            new("Increase color hue", ConsoleKey.RightArrow),
            new("Increase color lightness", ConsoleKey.RightArrow, ConsoleModifiers.Control),
            new("Increase saturation", ConsoleKey.UpArrow),
        ];
        internal readonly static Keybinding[] additionalBindingsNormalColor =
        [
            .. additionalBindingsReadWrite,
            new("Previous color", ConsoleKey.LeftArrow),
            new("Next color", ConsoleKey.RightArrow),
            new("Show and hide color list", ConsoleKey.L),
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

        static ColorSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
