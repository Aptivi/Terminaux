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
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class KeyInfo : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            TextWriterColor.Write("Enter a key or a combination of keys to display its information.");
            var KeyPress = Input.ReadKey();

            // Pressed key
            TextWriterColor.WriteColor("- Pressed key: ", false, new Color(ConsoleColors.Green));
            TextWriterColor.WriteColor(KeyPress.Key.ToString(), true, new Color(ConsoleColors.DarkGreen));

            // If the pressed key is a control key, don't write the actual key char so as not to corrupt the output
            if (!char.IsControl(KeyPress.KeyChar))
            {
                TextWriterColor.WriteColor("- " + "Pressed key character: ", false, new Color(ConsoleColors.Green));
                TextWriterColor.WriteColor(Convert.ToString(KeyPress.KeyChar), true, new Color(ConsoleColors.DarkGreen));
            }

            // Pressed key character code
            TextWriterColor.WriteColor("- " + "Pressed key character code: ", false, new Color(ConsoleColors.Green));
            TextWriterColor.WriteColor($"0x{Convert.ToInt32(KeyPress.KeyChar):X2} [{Convert.ToInt32(KeyPress.KeyChar)}]", true, new Color(ConsoleColors.DarkGreen));

            // Pressed modifiers
            if (KeyPress.Modifiers != 0)
            {
                TextWriterColor.WriteColor("- " + "Pressed modifiers: ", false, new Color(ConsoleColors.Green));
                TextWriterColor.WriteColor(KeyPress.Modifiers.ToString(), true, new Color(ConsoleColors.DarkGreen));
            }

            // Keyboard shortcut
            TextWriterColor.WriteColor("- " + "Keyboard shortcut: ", false, new Color(ConsoleColors.Green));
            if (KeyPress.Modifiers != 0)
                TextWriterColor.WriteColor($"{string.Join(" + ", KeyPress.Modifiers.ToString().Split(new string[] { ", " }, StringSplitOptions.None))} + {KeyPress.Key}", true, new Color(ConsoleColors.DarkGreen));
            else
                TextWriterColor.WriteColor($"{KeyPress.Key}", true, new Color(ConsoleColors.DarkGreen));
        }
    }
}
