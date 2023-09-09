
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using Terminaux.Colors;
using Terminaux.Reader.Inputs;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class KeyInfo : IFixture
    {
        public string FixtureID => "KeyInfo";
        public void RunFixture()
        {
            TextWriterColor.Write("Enter a key or a combination of keys to display its information.");
            var KeyPress = Input.DetectKeypress();

            // Pressed key
            TextWriterColor.Write("- Pressed key: ", false, new Color(ConsoleColors.Green));
            TextWriterColor.Write(KeyPress.Key.ToString(), true, new Color(ConsoleColors.DarkGreen));

            // If the pressed key is a control key, don't write the actual key char so as not to corrupt the output
            if (!char.IsControl(KeyPress.KeyChar))
            {
                TextWriterColor.Write("- " + "Pressed key character: ", false, new Color(ConsoleColors.Green));
                TextWriterColor.Write(Convert.ToString(KeyPress.KeyChar), true, new Color(ConsoleColors.DarkGreen));
            }

            // Pressed key character code
            TextWriterColor.Write("- " + "Pressed key character code: ", false, new Color(ConsoleColors.Green));
            TextWriterColor.Write($"0x{Convert.ToInt32(KeyPress.KeyChar):X2} [{Convert.ToInt32(KeyPress.KeyChar)}]", true, new Color(ConsoleColors.DarkGreen));

            // Pressed modifiers
            TextWriterColor.Write("- " + "Pressed modifiers: ", false, new Color(ConsoleColors.Green));
            TextWriterColor.Write(KeyPress.Modifiers.ToString(), true, new Color(ConsoleColors.DarkGreen));

            // Keyboard shortcut
            TextWriterColor.Write("- " + "Keyboard shortcut: ", false, new Color(ConsoleColors.Green));
            TextWriterColor.Write($"{string.Join(" + ", KeyPress.Modifiers.ToString().Split(new string[] { ", " }, StringSplitOptions.None))} + {KeyPress.Key}", true, new Color(ConsoleColors.DarkGreen));
        }
    }
}
