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

namespace Terminaux.Writer.CyclicWriters.Builtins
{
    /// <summary>
    /// Built-in spinners
    /// </summary>
    public static class BuiltinSpinners
    {
        // Based on https://rawgit.com/sindresorhus/cli-spinners/master/spinners.json
        /// <summary>
        /// Spinning Braille dots
        /// </summary>
        public static Spinner Dots =>
            new([
                "⠋",
                "⠙",
                "⠹",
                "⠸",
                "⠼",
                "⠴",
                "⠦",
                "⠧",
                "⠇",
                "⠏"
            ]);

        /// <summary>
        /// Falling sand particles
        /// </summary>
        public static Spinner Sand =>
            new([
                "⠁",
                "⠂",
                "⠄",
                "⡀",
                "⡈",
                "⡐",
                "⡠",
                "⣀",
                "⣁",
                "⣂",
                "⣄",
                "⣌",
                "⣔",
                "⣤",
                "⣥",
                "⣦",
                "⣮",
                "⣶",
                "⣷",
                "⣿",
                "⡿",
                "⠿",
                "⢟",
                "⠟",
                "⡛",
                "⠛",
                "⠫",
                "⢋",
                "⠋",
                "⠍",
                "⡉",
                "⠉",
                "⠑",
                "⠡",
                "⢁"
            ]);

        /// <summary>
        /// Rotating line (clockwise)
        /// </summary>
        public static Spinner Line =>
            new([
                "-",
                "/",
                "|",
                "\\",
            ]);

        /// <summary>
        /// Rotating line (counter-clockwise)
        /// </summary>
        public static Spinner LineCcw =>
            new([
                "-",
                "\\",
                "|",
                "/",
            ]);

        /// <summary>
        /// Simple dots
        /// </summary>
        public static Spinner SimpleDots =>
            new([
                ".  ",
                ".. ",
                "...",
                "   "
            ]);

        /// <summary>
        /// Scrolling dots
        /// </summary>
        public static Spinner ScrollingDots =>
            new([
                ".  ",
                ".. ",
                "...",
                " ..",
                "  .",
                "   "
            ]);

        /// <summary>
        /// Spinning arc
        /// </summary>
        public static Spinner Arc =>
            new([
                "◜",
                "◠",
                "◝",
                "◞",
                "◡",
                "◟"
            ]);

        /// <summary>
        /// Bouncing bar
        /// </summary>
        public static Spinner BouncingBar =>
            new([
                "[    ]",
                "[=   ]",
                "[==  ]",
                "[=== ]",
                "[====]",
                "[ ===]",
                "[  ==]",
                "[   =]",
                "[    ]",
                "[   =]",
                "[  ==]",
                "[ ===]",
                "[====]",
                "[=== ]",
                "[==  ]",
                "[=   ]"
            ]);

        /// <summary>
        /// Bouncing ball
        /// </summary>
        public static Spinner BouncingBall =>
            new([
                "( ●    )",
                "(  ●   )",
                "(   ●  )",
                "(    ● )",
                "(     ●)",
                "(    ● )",
                "(   ●  )",
                "(  ●   )",
                "( ●    )",
                "(●     )"
            ]);

        /// <summary>
        /// Pong
        /// </summary>
        public static Spinner Pong =>
            new([
                "▐⠂       ▌",
                "▐⠈       ▌",
                "▐ ⠂      ▌",
                "▐ ⠠      ▌",
                "▐  ⡀     ▌",
                "▐  ⠠     ▌",
                "▐   ⠂    ▌",
                "▐   ⠈    ▌",
                "▐    ⠂   ▌",
                "▐    ⠠   ▌",
                "▐     ⡀  ▌",
                "▐     ⠠  ▌",
                "▐      ⠂ ▌",
                "▐      ⠈ ▌",
                "▐       ⠂▌",
                "▐       ⠠▌",
                "▐       ⡀▌",
                "▐      ⠠ ▌",
                "▐      ⠂ ▌",
                "▐     ⠈  ▌",
                "▐     ⠂  ▌",
                "▐    ⠠   ▌",
                "▐    ⡀   ▌",
                "▐   ⠠    ▌",
                "▐   ⠂    ▌",
                "▐  ⠈     ▌",
                "▐  ⠂     ▌",
                "▐ ⠠      ▌",
                "▐ ⡀      ▌",
                "▐⠠       ▌"
            ]);

        /// <summary>
        /// Aesthetic
        /// </summary>
        public static Spinner Aesthetic =>
            new([
                "▰▱▱▱▱▱▱",
                "▰▰▱▱▱▱▱",
                "▰▰▰▱▱▱▱",
                "▰▰▰▰▱▱▱",
                "▰▰▰▰▰▱▱",
                "▰▰▰▰▰▰▱",
                "▰▰▰▰▰▰▰",
                "▰▱▱▱▱▱▱"
            ]);

        /// <summary>
        /// Aesthetic (looped)
        /// </summary>
        public static Spinner AestheticLooped =>
            new([
                "▰▱▱▱▱▱▱",
                "▰▰▱▱▱▱▱",
                "▰▰▰▱▱▱▱",
                "▰▰▰▰▱▱▱",
                "▰▰▰▰▰▱▱",
                "▰▰▰▰▰▰▱",
                "▰▰▰▰▰▰▰",
                "▱▰▰▰▰▰▰",
                "▱▱▰▰▰▰▰",
                "▱▱▱▰▰▰▰",
                "▱▱▱▱▰▰▰",
                "▱▱▱▱▱▰▰",
                "▱▱▱▱▱▱▰",
                "▱▱▱▱▱▱▱"
            ]);

        /// <summary>
        /// Dwarf Fortress
        /// </summary>
        public static Spinner DwarfFortress =>
            new([
                " ██████£££  ",
                "☺██████£££  ",
                "☺██████£££  ",
                "☺▓█████£££  ",
                "☺▓█████£££  ",
                "☺▒█████£££  ",
                "☺▒█████£££  ",
                "☺░█████£££  ",
                "☺░█████£££  ",
                "☺ █████£££  ",
                " ☺█████£££  ",
                " ☺█████£££  ",
                " ☺▓████£££  ",
                " ☺▓████£££  ",
                " ☺▒████£££  ",
                " ☺▒████£££  ",
                " ☺░████£££  ",
                " ☺░████£££  ",
                " ☺ ████£££  ",
                "  ☺████£££  ",
                "  ☺████£££  ",
                "  ☺▓███£££  ",
                "  ☺▓███£££  ",
                "  ☺▒███£££  ",
                "  ☺▒███£££  ",
                "  ☺░███£££  ",
                "  ☺░███£££  ",
                "  ☺ ███£££  ",
                "   ☺███£££  ",
                "   ☺███£££  ",
                "   ☺▓██£££  ",
                "   ☺▓██£££  ",
                "   ☺▒██£££  ",
                "   ☺▒██£££  ",
                "   ☺░██£££  ",
                "   ☺░██£££  ",
                "   ☺ ██£££  ",
                "    ☺██£££  ",
                "    ☺██£££  ",
                "    ☺▓█£££  ",
                "    ☺▓█£££  ",
                "    ☺▒█£££  ",
                "    ☺▒█£££  ",
                "    ☺░█£££  ",
                "    ☺░█£££  ",
                "    ☺ █£££  ",
                "     ☺█£££  ",
                "     ☺█£££  ",
                "     ☺▓£££  ",
                "     ☺▓£££  ",
                "     ☺▒£££  ",
                "     ☺▒£££  ",
                "     ☺░£££  ",
                "     ☺░£££  ",
                "     ☺ £££  ",
                "      ☺£££  ",
                "      ☺£££  ",
                "      ☺▓££  ",
                "      ☺▓££  ",
                "      ☺▒££  ",
                "      ☺▒££  ",
                "      ☺░££  ",
                "      ☺░££  ",
                "      ☺ ££  ",
                "       ☺££  ",
                "       ☺££  ",
                "       ☺▓£  ",
                "       ☺▓£  ",
                "       ☺▒£  ",
                "       ☺▒£  ",
                "       ☺░£  ",
                "       ☺░£  ",
                "       ☺ £  ",
                "        ☺£  ",
                "        ☺£  ",
                "        ☺▓  ",
                "        ☺▓  ",
                "        ☺▒  ",
                "        ☺▒  ",
                "        ☺░  ",
                "        ☺░  ",
                "        ☺   ",
                "        ☺  &",
                "        ☺ ☼&",
                "       ☺ ☼ &",
                "       ☺☼  &",
                "      ☺☼  & ",
                "      ‼   & ",
                "     ☺   &  ",
                "    ‼    &  ",
                "   ☺    &   ",
                "  ‼     &   ",
                " ☺     &    ",
                "‼      &    ",
                "      &     ",
                "      &     ",
                "     &   ░  ",
                "     &   ▒  ",
                "    &    ▓  ",
                "    &    £  ",
                "   &    ░£  ",
                "   &    ▒£  ",
                "  &     ▓£  ",
                "  &     ££  ",
                " &     ░££  ",
                " &     ▒££  ",
                "&      ▓££  ",
                "&      £££  ",
                "      ░£££  ",
                "      ▒£££  ",
                "      ▓£££  ",
                "      █£££  ",
                "     ░█£££  ",
                "     ▒█£££  ",
                "     ▓█£££  ",
                "     ██£££  ",
                "    ░██£££  ",
                "    ▒██£££  ",
                "    ▓██£££  ",
                "    ███£££  ",
                "   ░███£££  ",
                "   ▒███£££  ",
                "   ▓███£££  ",
                "   ████£££  ",
                "  ░████£££  ",
                "  ▒████£££  ",
                "  ▓████£££  ",
                "  █████£££  ",
                " ░█████£££  ",
                " ▒█████£££  ",
                " ▓█████£££  ",
                " ██████£££  ",
                " ██████£££  "
            ]);
    }
}
