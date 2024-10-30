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
        /// The dots spinner instance.
        /// </summary>
        public static Spinner Dots =>
            new([
                @"⠋",
                @"⠙",
                @"⠹",
                @"⠸",
                @"⠼",
                @"⠴",
                @"⠦",
                @"⠧",
                @"⠇",
                @"⠏",
            ]);

        /// <summary>
        /// The dots2 spinner instance.
        /// </summary>
        public static Spinner Dots2 =>
            new([
                @"⣾",
                @"⣽",
                @"⣻",
                @"⢿",
                @"⡿",
                @"⣟",
                @"⣯",
                @"⣷",
            ]);

        /// <summary>
        /// The dots3 spinner instance.
        /// </summary>
        public static Spinner Dots3 =>
            new([
                @"⠋",
                @"⠙",
                @"⠚",
                @"⠞",
                @"⠖",
                @"⠦",
                @"⠴",
                @"⠲",
                @"⠳",
                @"⠓",
            ]);

        /// <summary>
        /// The dots4 spinner instance.
        /// </summary>
        public static Spinner Dots4 =>
            new([
                @"⠄",
                @"⠆",
                @"⠇",
                @"⠋",
                @"⠙",
                @"⠸",
                @"⠰",
                @"⠠",
                @"⠰",
                @"⠸",
                @"⠙",
                @"⠋",
                @"⠇",
                @"⠆",
            ]);

        /// <summary>
        /// The dots5 spinner instance.
        /// </summary>
        public static Spinner Dots5 =>
            new([
                @"⠋",
                @"⠙",
                @"⠚",
                @"⠒",
                @"⠂",
                @"⠂",
                @"⠒",
                @"⠲",
                @"⠴",
                @"⠦",
                @"⠖",
                @"⠒",
                @"⠐",
                @"⠐",
                @"⠒",
                @"⠓",
                @"⠋",
            ]);

        /// <summary>
        /// The dots6 spinner instance.
        /// </summary>
        public static Spinner Dots6 =>
            new([
                @"⠁",
                @"⠉",
                @"⠙",
                @"⠚",
                @"⠒",
                @"⠂",
                @"⠂",
                @"⠒",
                @"⠲",
                @"⠴",
                @"⠤",
                @"⠄",
                @"⠄",
                @"⠤",
                @"⠴",
                @"⠲",
                @"⠒",
                @"⠂",
                @"⠂",
                @"⠒",
                @"⠚",
                @"⠙",
                @"⠉",
                @"⠁",
            ]);

        /// <summary>
        /// The dots7 spinner instance.
        /// </summary>
        public static Spinner Dots7 =>
            new([
                @"⠈",
                @"⠉",
                @"⠋",
                @"⠓",
                @"⠒",
                @"⠐",
                @"⠐",
                @"⠒",
                @"⠖",
                @"⠦",
                @"⠤",
                @"⠠",
                @"⠠",
                @"⠤",
                @"⠦",
                @"⠖",
                @"⠒",
                @"⠐",
                @"⠐",
                @"⠒",
                @"⠓",
                @"⠋",
                @"⠉",
                @"⠈",
            ]);

        /// <summary>
        /// The dots8 spinner instance.
        /// </summary>
        public static Spinner Dots8 =>
            new([
                @"⠁",
                @"⠁",
                @"⠉",
                @"⠙",
                @"⠚",
                @"⠒",
                @"⠂",
                @"⠂",
                @"⠒",
                @"⠲",
                @"⠴",
                @"⠤",
                @"⠄",
                @"⠄",
                @"⠤",
                @"⠠",
                @"⠠",
                @"⠤",
                @"⠦",
                @"⠖",
                @"⠒",
                @"⠐",
                @"⠐",
                @"⠒",
                @"⠓",
                @"⠋",
                @"⠉",
                @"⠈",
                @"⠈",
            ]);

        /// <summary>
        /// The dots9 spinner instance.
        /// </summary>
        public static Spinner Dots9 =>
            new([
                @"⢹",
                @"⢺",
                @"⢼",
                @"⣸",
                @"⣇",
                @"⡧",
                @"⡗",
                @"⡏",
            ]);

        /// <summary>
        /// The dots10 spinner instance.
        /// </summary>
        public static Spinner Dots10 =>
            new([
                @"⢄",
                @"⢂",
                @"⢁",
                @"⡁",
                @"⡈",
                @"⡐",
                @"⡠",
            ]);

        /// <summary>
        /// The dots11 spinner instance.
        /// </summary>
        public static Spinner Dots11 =>
            new([
                @"⠁",
                @"⠂",
                @"⠄",
                @"⡀",
                @"⢀",
                @"⠠",
                @"⠐",
                @"⠈",
            ]);

        /// <summary>
        /// The dots12 spinner instance.
        /// </summary>
        public static Spinner Dots12 =>
            new([
                @"⢀⠀",
                @"⡀⠀",
                @"⠄⠀",
                @"⢂⠀",
                @"⡂⠀",
                @"⠅⠀",
                @"⢃⠀",
                @"⡃⠀",
                @"⠍⠀",
                @"⢋⠀",
                @"⡋⠀",
                @"⠍⠁",
                @"⢋⠁",
                @"⡋⠁",
                @"⠍⠉",
                @"⠋⠉",
                @"⠋⠉",
                @"⠉⠙",
                @"⠉⠙",
                @"⠉⠩",
                @"⠈⢙",
                @"⠈⡙",
                @"⢈⠩",
                @"⡀⢙",
                @"⠄⡙",
                @"⢂⠩",
                @"⡂⢘",
                @"⠅⡘",
                @"⢃⠨",
                @"⡃⢐",
                @"⠍⡐",
                @"⢋⠠",
                @"⡋⢀",
                @"⠍⡁",
                @"⢋⠁",
                @"⡋⠁",
                @"⠍⠉",
                @"⠋⠉",
                @"⠋⠉",
                @"⠉⠙",
                @"⠉⠙",
                @"⠉⠩",
                @"⠈⢙",
                @"⠈⡙",
                @"⠈⠩",
                @"⠀⢙",
                @"⠀⡙",
                @"⠀⠩",
                @"⠀⢘",
                @"⠀⡘",
                @"⠀⠨",
                @"⠀⢐",
                @"⠀⡐",
                @"⠀⠠",
                @"⠀⢀",
                @"⠀⡀",
            ]);

        /// <summary>
        /// The dots13 spinner instance.
        /// </summary>
        public static Spinner Dots13 =>
            new([
                @"⣼",
                @"⣹",
                @"⢻",
                @"⠿",
                @"⡟",
                @"⣏",
                @"⣧",
                @"⣶",
            ]);

        /// <summary>
        /// The dots14 spinner instance.
        /// </summary>
        public static Spinner Dots14 =>
            new([
                @"⠉⠉",
                @"⠈⠙",
                @"⠀⠹",
                @"⠀⢸",
                @"⠀⣰",
                @"⢀⣠",
                @"⣀⣀",
                @"⣄⡀",
                @"⣆⠀",
                @"⡇⠀",
                @"⠏⠀",
                @"⠋⠁",
            ]);

        /// <summary>
        /// The dots8Bit spinner instance.
        /// </summary>
        public static Spinner Dots8Bit =>
            new([
                @"⠀",
                @"⠁",
                @"⠂",
                @"⠃",
                @"⠄",
                @"⠅",
                @"⠆",
                @"⠇",
                @"⡀",
                @"⡁",
                @"⡂",
                @"⡃",
                @"⡄",
                @"⡅",
                @"⡆",
                @"⡇",
                @"⠈",
                @"⠉",
                @"⠊",
                @"⠋",
                @"⠌",
                @"⠍",
                @"⠎",
                @"⠏",
                @"⡈",
                @"⡉",
                @"⡊",
                @"⡋",
                @"⡌",
                @"⡍",
                @"⡎",
                @"⡏",
                @"⠐",
                @"⠑",
                @"⠒",
                @"⠓",
                @"⠔",
                @"⠕",
                @"⠖",
                @"⠗",
                @"⡐",
                @"⡑",
                @"⡒",
                @"⡓",
                @"⡔",
                @"⡕",
                @"⡖",
                @"⡗",
                @"⠘",
                @"⠙",
                @"⠚",
                @"⠛",
                @"⠜",
                @"⠝",
                @"⠞",
                @"⠟",
                @"⡘",
                @"⡙",
                @"⡚",
                @"⡛",
                @"⡜",
                @"⡝",
                @"⡞",
                @"⡟",
                @"⠠",
                @"⠡",
                @"⠢",
                @"⠣",
                @"⠤",
                @"⠥",
                @"⠦",
                @"⠧",
                @"⡠",
                @"⡡",
                @"⡢",
                @"⡣",
                @"⡤",
                @"⡥",
                @"⡦",
                @"⡧",
                @"⠨",
                @"⠩",
                @"⠪",
                @"⠫",
                @"⠬",
                @"⠭",
                @"⠮",
                @"⠯",
                @"⡨",
                @"⡩",
                @"⡪",
                @"⡫",
                @"⡬",
                @"⡭",
                @"⡮",
                @"⡯",
                @"⠰",
                @"⠱",
                @"⠲",
                @"⠳",
                @"⠴",
                @"⠵",
                @"⠶",
                @"⠷",
                @"⡰",
                @"⡱",
                @"⡲",
                @"⡳",
                @"⡴",
                @"⡵",
                @"⡶",
                @"⡷",
                @"⠸",
                @"⠹",
                @"⠺",
                @"⠻",
                @"⠼",
                @"⠽",
                @"⠾",
                @"⠿",
                @"⡸",
                @"⡹",
                @"⡺",
                @"⡻",
                @"⡼",
                @"⡽",
                @"⡾",
                @"⡿",
                @"⢀",
                @"⢁",
                @"⢂",
                @"⢃",
                @"⢄",
                @"⢅",
                @"⢆",
                @"⢇",
                @"⣀",
                @"⣁",
                @"⣂",
                @"⣃",
                @"⣄",
                @"⣅",
                @"⣆",
                @"⣇",
                @"⢈",
                @"⢉",
                @"⢊",
                @"⢋",
                @"⢌",
                @"⢍",
                @"⢎",
                @"⢏",
                @"⣈",
                @"⣉",
                @"⣊",
                @"⣋",
                @"⣌",
                @"⣍",
                @"⣎",
                @"⣏",
                @"⢐",
                @"⢑",
                @"⢒",
                @"⢓",
                @"⢔",
                @"⢕",
                @"⢖",
                @"⢗",
                @"⣐",
                @"⣑",
                @"⣒",
                @"⣓",
                @"⣔",
                @"⣕",
                @"⣖",
                @"⣗",
                @"⢘",
                @"⢙",
                @"⢚",
                @"⢛",
                @"⢜",
                @"⢝",
                @"⢞",
                @"⢟",
                @"⣘",
                @"⣙",
                @"⣚",
                @"⣛",
                @"⣜",
                @"⣝",
                @"⣞",
                @"⣟",
                @"⢠",
                @"⢡",
                @"⢢",
                @"⢣",
                @"⢤",
                @"⢥",
                @"⢦",
                @"⢧",
                @"⣠",
                @"⣡",
                @"⣢",
                @"⣣",
                @"⣤",
                @"⣥",
                @"⣦",
                @"⣧",
                @"⢨",
                @"⢩",
                @"⢪",
                @"⢫",
                @"⢬",
                @"⢭",
                @"⢮",
                @"⢯",
                @"⣨",
                @"⣩",
                @"⣪",
                @"⣫",
                @"⣬",
                @"⣭",
                @"⣮",
                @"⣯",
                @"⢰",
                @"⢱",
                @"⢲",
                @"⢳",
                @"⢴",
                @"⢵",
                @"⢶",
                @"⢷",
                @"⣰",
                @"⣱",
                @"⣲",
                @"⣳",
                @"⣴",
                @"⣵",
                @"⣶",
                @"⣷",
                @"⢸",
                @"⢹",
                @"⢺",
                @"⢻",
                @"⢼",
                @"⢽",
                @"⢾",
                @"⢿",
                @"⣸",
                @"⣹",
                @"⣺",
                @"⣻",
                @"⣼",
                @"⣽",
                @"⣾",
                @"⣿",
            ]);

        /// <summary>
        /// The dotsCircle spinner instance.
        /// </summary>
        public static Spinner DotsCircle =>
            new([
                @"⢎ ",
                @"⠎⠁",
                @"⠊⠑",
                @"⠈⠱",
                @" ⡱",
                @"⢀⡰",
                @"⢄⡠",
                @"⢆⡀",
            ]);

        /// <summary>
        /// The sand spinner instance.
        /// </summary>
        public static Spinner Sand =>
            new([
                @"⠁",
                @"⠂",
                @"⠄",
                @"⡀",
                @"⡈",
                @"⡐",
                @"⡠",
                @"⣀",
                @"⣁",
                @"⣂",
                @"⣄",
                @"⣌",
                @"⣔",
                @"⣤",
                @"⣥",
                @"⣦",
                @"⣮",
                @"⣶",
                @"⣷",
                @"⣿",
                @"⡿",
                @"⠿",
                @"⢟",
                @"⠟",
                @"⡛",
                @"⠛",
                @"⠫",
                @"⢋",
                @"⠋",
                @"⠍",
                @"⡉",
                @"⠉",
                @"⠑",
                @"⠡",
                @"⢁",
            ]);

        /// <summary>
        /// Rotating line (clockwise)
        /// </summary>
        public static Spinner Line =>
            new([
                @"-",
                @"/",
                @"|",
                @"\",
            ]);

        /// <summary>
        /// Rotating line (counter-clockwise)
        /// </summary>
        public static Spinner LineCcw =>
            new([
                @"-",
                @"\",
                @"|",
                @"/",
            ]);

        /// <summary>
        /// The line2 spinner instance.
        /// </summary>
        public static Spinner Line2 =>
            new([
                @"⠂",
                @"-",
                @"–",
                @"—",
                @"–",
                @"-",
            ]);

        /// <summary>
        /// The pipe spinner instance.
        /// </summary>
        public static Spinner Pipe =>
            new([
                @"┤",
                @"┘",
                @"┴",
                @"└",
                @"├",
                @"┌",
                @"┬",
                @"┐",
            ]);

        /// <summary>
        /// The simpleDots spinner instance.
        /// </summary>
        public static Spinner SimpleDots =>
            new([
                @".  ",
                @".. ",
                @"...",
                @"   ",
            ]);

        /// <summary>
        /// The simpleDotsScrolling spinner instance.
        /// </summary>
        public static Spinner SimpleDotsScrolling =>
            new([
                @".  ",
                @".. ",
                @"...",
                @" ..",
                @"  .",
                @"   ",
            ]);

        /// <summary>
        /// The star spinner instance.
        /// </summary>
        public static Spinner Star =>
            new([
                @"✶",
                @"✸",
                @"✹",
                @"✺",
                @"✹",
                @"✷",
            ]);

        /// <summary>
        /// The star2 spinner instance.
        /// </summary>
        public static Spinner Star2 =>
            new([
                @"+",
                @"x",
                @"*",
            ]);

        /// <summary>
        /// The flip spinner instance.
        /// </summary>
        public static Spinner Flip =>
            new([
                @"_",
                @"_",
                @"_",
                @"-",
                @"`",
                @"`",
                @"'",
                @"´",
                @"-",
                @"_",
                @"_",
                @"_",
            ]);

        /// <summary>
        /// The hamburger spinner instance.
        /// </summary>
        public static Spinner Hamburger =>
            new([
                @"☱",
                @"☲",
                @"☴",
            ]);

        /// <summary>
        /// The growVertical spinner instance.
        /// </summary>
        public static Spinner GrowVertical =>
            new([
                @"▁",
                @"▃",
                @"▄",
                @"▅",
                @"▆",
                @"▇",
                @"▆",
                @"▅",
                @"▄",
                @"▃",
            ]);

        /// <summary>
        /// The growHorizontal spinner instance.
        /// </summary>
        public static Spinner GrowHorizontal =>
            new([
                @"▏",
                @"▎",
                @"▍",
                @"▌",
                @"▋",
                @"▊",
                @"▉",
                @"▊",
                @"▋",
                @"▌",
                @"▍",
                @"▎",
            ]);

        /// <summary>
        /// The balloon spinner instance.
        /// </summary>
        public static Spinner Balloon =>
            new([
                @" ",
                @".",
                @"o",
                @"O",
                @"@",
                @"*",
                @" ",
            ]);

        /// <summary>
        /// The balloon2 spinner instance.
        /// </summary>
        public static Spinner Balloon2 =>
            new([
                @".",
                @"o",
                @"O",
                @"°",
                @"O",
                @"o",
                @".",
            ]);

        /// <summary>
        /// The noise spinner instance.
        /// </summary>
        public static Spinner Noise =>
            new([
                @"▓",
                @"▒",
                @"░",
            ]);

        /// <summary>
        /// The bounce spinner instance.
        /// </summary>
        public static Spinner Bounce =>
            new([
                @"⠁",
                @"⠂",
                @"⠄",
                @"⠂",
            ]);

        /// <summary>
        /// The boxBounce spinner instance.
        /// </summary>
        public static Spinner BoxBounce =>
            new([
                @"▖",
                @"▘",
                @"▝",
                @"▗",
            ]);

        /// <summary>
        /// The boxBounce2 spinner instance.
        /// </summary>
        public static Spinner BoxBounce2 =>
            new([
                @"▌",
                @"▀",
                @"▐",
                @"▄",
            ]);

        /// <summary>
        /// The triangle spinner instance.
        /// </summary>
        public static Spinner Triangle =>
            new([
                @"◢",
                @"◣",
                @"◤",
                @"◥",
            ]);

        /// <summary>
        /// The binary spinner instance.
        /// </summary>
        public static Spinner Binary =>
            new([
                @"010010",
                @"001100",
                @"100101",
                @"111010",
                @"111101",
                @"010111",
                @"101011",
                @"111000",
                @"110011",
                @"110101",
            ]);

        /// <summary>
        /// The arc spinner instance.
        /// </summary>
        public static Spinner Arc =>
            new([
                @"◜",
                @"◠",
                @"◝",
                @"◞",
                @"◡",
                @"◟",
            ]);

        /// <summary>
        /// The circle spinner instance.
        /// </summary>
        public static Spinner Circle =>
            new([
                @"◡",
                @"⊙",
                @"◠",
            ]);

        /// <summary>
        /// The squareCorners spinner instance.
        /// </summary>
        public static Spinner SquareCorners =>
            new([
                @"◰",
                @"◳",
                @"◲",
                @"◱",
            ]);

        /// <summary>
        /// The circleQuarters spinner instance.
        /// </summary>
        public static Spinner CircleQuarters =>
            new([
                @"◴",
                @"◷",
                @"◶",
                @"◵",
            ]);

        /// <summary>
        /// The circleHalves spinner instance.
        /// </summary>
        public static Spinner CircleHalves =>
            new([
                @"◐",
                @"◓",
                @"◑",
                @"◒",
            ]);

        /// <summary>
        /// The squish spinner instance.
        /// </summary>
        public static Spinner Squish =>
            new([
                @"╫",
                @"╪",
            ]);

        /// <summary>
        /// The toggle spinner instance.
        /// </summary>
        public static Spinner Toggle =>
            new([
                @"⊶",
                @"⊷",
            ]);

        /// <summary>
        /// The toggle2 spinner instance.
        /// </summary>
        public static Spinner Toggle2 =>
            new([
                @"▫",
                @"▪",
            ]);

        /// <summary>
        /// The toggle3 spinner instance.
        /// </summary>
        public static Spinner Toggle3 =>
            new([
                @"□",
                @"■",
            ]);

        /// <summary>
        /// The toggle4 spinner instance.
        /// </summary>
        public static Spinner Toggle4 =>
            new([
                @"■",
                @"□",
                @"▪",
                @"▫",
            ]);

        /// <summary>
        /// The toggle5 spinner instance.
        /// </summary>
        public static Spinner Toggle5 =>
            new([
                @"▮",
                @"▯",
            ]);

        /// <summary>
        /// The toggle6 spinner instance.
        /// </summary>
        public static Spinner Toggle6 =>
            new([
                @"ဝ",
                @"၀",
            ]);

        /// <summary>
        /// The toggle7 spinner instance.
        /// </summary>
        public static Spinner Toggle7 =>
            new([
                @"⦾",
                @"⦿",
            ]);

        /// <summary>
        /// The toggle8 spinner instance.
        /// </summary>
        public static Spinner Toggle8 =>
            new([
                @"◍",
                @"◌",
            ]);

        /// <summary>
        /// The toggle9 spinner instance.
        /// </summary>
        public static Spinner Toggle9 =>
            new([
                @"◉",
                @"◎",
            ]);

        /// <summary>
        /// The toggle10 spinner instance.
        /// </summary>
        public static Spinner Toggle10 =>
            new([
                @"㊂",
                @"㊀",
                @"㊁",
            ]);

        /// <summary>
        /// The toggle11 spinner instance.
        /// </summary>
        public static Spinner Toggle11 =>
            new([
                @"⧇",
                @"⧆",
            ]);

        /// <summary>
        /// The toggle12 spinner instance.
        /// </summary>
        public static Spinner Toggle12 =>
            new([
                @"☗",
                @"☖",
            ]);

        /// <summary>
        /// The toggle13 spinner instance.
        /// </summary>
        public static Spinner Toggle13 =>
            new([
                @"=",
                @"*",
                @"-",
            ]);

        /// <summary>
        /// The arrow spinner instance.
        /// </summary>
        public static Spinner Arrow =>
            new([
                @"←",
                @"↖",
                @"↑",
                @"↗",
                @"→",
                @"↘",
                @"↓",
                @"↙",
            ]);

        /// <summary>
        /// The arrow2 spinner instance.
        /// </summary>
        public static Spinner Arrow2 =>
            new([
                @"⬆️ ",
                @"↗️ ",
                @"➡️ ",
                @"↘️ ",
                @"⬇️ ",
                @"↙️ ",
                @"⬅️ ",
                @"↖️ ",
            ]);

        /// <summary>
        /// The arrow3 spinner instance.
        /// </summary>
        public static Spinner Arrow3 =>
            new([
                @"▹▹▹▹▹",
                @"▸▹▹▹▹",
                @"▹▸▹▹▹",
                @"▹▹▸▹▹",
                @"▹▹▹▸▹",
                @"▹▹▹▹▸",
            ]);

        /// <summary>
        /// The bouncingBar spinner instance.
        /// </summary>
        public static Spinner BouncingBar =>
            new([
                @"[    ]",
                @"[=   ]",
                @"[==  ]",
                @"[=== ]",
                @"[====]",
                @"[ ===]",
                @"[  ==]",
                @"[   =]",
                @"[    ]",
                @"[   =]",
                @"[  ==]",
                @"[ ===]",
                @"[====]",
                @"[=== ]",
                @"[==  ]",
                @"[=   ]",
            ]);

        /// <summary>
        /// The bouncingBall spinner instance.
        /// </summary>
        public static Spinner BouncingBall =>
            new([
                @"( ●    )",
                @"(  ●   )",
                @"(   ●  )",
                @"(    ● )",
                @"(     ●)",
                @"(    ● )",
                @"(   ●  )",
                @"(  ●   )",
                @"( ●    )",
                @"(●     )",
            ]);

        /// <summary>
        /// The smiley spinner instance.
        /// </summary>
        public static Spinner Smiley =>
            new([
                @"😄 ",
                @"😝 ",
            ]);

        /// <summary>
        /// The monkey spinner instance.
        /// </summary>
        public static Spinner Monkey =>
            new([
                @"🙈 ",
                @"🙈 ",
                @"🙉 ",
                @"🙊 ",
            ]);

        /// <summary>
        /// The hearts spinner instance.
        /// </summary>
        public static Spinner Hearts =>
            new([
                @"💛",
                @"💙",
                @"💜",
                @"💚",
                @"❤️ ",
            ]);

        /// <summary>
        /// The clock spinner instance.
        /// </summary>
        public static Spinner Clock =>
            new([
                @"🕛 ",
                @"🕐 ",
                @"🕑 ",
                @"🕒 ",
                @"🕓 ",
                @"🕔 ",
                @"🕕 ",
                @"🕖 ",
                @"🕗 ",
                @"🕘 ",
                @"🕙 ",
                @"🕚 ",
            ]);

        /// <summary>
        /// The earth spinner instance.
        /// </summary>
        public static Spinner Earth =>
            new([
                @"🌍 ",
                @"🌎 ",
                @"🌏 ",
            ]);

        /// <summary>
        /// The material spinner instance.
        /// </summary>
        public static Spinner Material =>
            new([
                @"█▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"███▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"████▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"██████▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"██████▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"███████▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"████████▁▁▁▁▁▁▁▁▁▁▁▁",
                @"█████████▁▁▁▁▁▁▁▁▁▁▁",
                @"█████████▁▁▁▁▁▁▁▁▁▁▁",
                @"██████████▁▁▁▁▁▁▁▁▁▁",
                @"███████████▁▁▁▁▁▁▁▁▁",
                @"█████████████▁▁▁▁▁▁▁",
                @"██████████████▁▁▁▁▁▁",
                @"██████████████▁▁▁▁▁▁",
                @"▁██████████████▁▁▁▁▁",
                @"▁██████████████▁▁▁▁▁",
                @"▁██████████████▁▁▁▁▁",
                @"▁▁██████████████▁▁▁▁",
                @"▁▁▁██████████████▁▁▁",
                @"▁▁▁▁█████████████▁▁▁",
                @"▁▁▁▁██████████████▁▁",
                @"▁▁▁▁██████████████▁▁",
                @"▁▁▁▁▁██████████████▁",
                @"▁▁▁▁▁██████████████▁",
                @"▁▁▁▁▁██████████████▁",
                @"▁▁▁▁▁▁██████████████",
                @"▁▁▁▁▁▁██████████████",
                @"▁▁▁▁▁▁▁█████████████",
                @"▁▁▁▁▁▁▁█████████████",
                @"▁▁▁▁▁▁▁▁████████████",
                @"▁▁▁▁▁▁▁▁████████████",
                @"▁▁▁▁▁▁▁▁▁███████████",
                @"▁▁▁▁▁▁▁▁▁███████████",
                @"▁▁▁▁▁▁▁▁▁▁██████████",
                @"▁▁▁▁▁▁▁▁▁▁██████████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁████████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁██████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
                @"█▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                @"██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                @"██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                @"███▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                @"████▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                @"█████▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                @"█████▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                @"██████▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                @"████████▁▁▁▁▁▁▁▁▁▁▁▁",
                @"█████████▁▁▁▁▁▁▁▁▁▁▁",
                @"█████████▁▁▁▁▁▁▁▁▁▁▁",
                @"█████████▁▁▁▁▁▁▁▁▁▁▁",
                @"█████████▁▁▁▁▁▁▁▁▁▁▁",
                @"███████████▁▁▁▁▁▁▁▁▁",
                @"████████████▁▁▁▁▁▁▁▁",
                @"████████████▁▁▁▁▁▁▁▁",
                @"██████████████▁▁▁▁▁▁",
                @"██████████████▁▁▁▁▁▁",
                @"▁██████████████▁▁▁▁▁",
                @"▁██████████████▁▁▁▁▁",
                @"▁▁▁█████████████▁▁▁▁",
                @"▁▁▁▁▁████████████▁▁▁",
                @"▁▁▁▁▁████████████▁▁▁",
                @"▁▁▁▁▁▁███████████▁▁▁",
                @"▁▁▁▁▁▁▁▁█████████▁▁▁",
                @"▁▁▁▁▁▁▁▁█████████▁▁▁",
                @"▁▁▁▁▁▁▁▁▁█████████▁▁",
                @"▁▁▁▁▁▁▁▁▁█████████▁▁",
                @"▁▁▁▁▁▁▁▁▁▁█████████▁",
                @"▁▁▁▁▁▁▁▁▁▁▁████████▁",
                @"▁▁▁▁▁▁▁▁▁▁▁████████▁",
                @"▁▁▁▁▁▁▁▁▁▁▁▁███████▁",
                @"▁▁▁▁▁▁▁▁▁▁▁▁███████▁",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
                @"▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            ]);

        /// <summary>
        /// The moon spinner instance.
        /// </summary>
        public static Spinner Moon =>
            new([
                @"🌑 ",
                @"🌒 ",
                @"🌓 ",
                @"🌔 ",
                @"🌕 ",
                @"🌖 ",
                @"🌗 ",
                @"🌘 ",
            ]);

        /// <summary>
        /// The runner spinner instance.
        /// </summary>
        public static Spinner Runner =>
            new([
                @"🚶 ",
                @"🏃 ",
            ]);

        /// <summary>
        /// The pong spinner instance.
        /// </summary>
        public static Spinner Pong =>
            new([
                @"▐⠂       ▌",
                @"▐⠈       ▌",
                @"▐ ⠂      ▌",
                @"▐ ⠠      ▌",
                @"▐  ⡀     ▌",
                @"▐  ⠠     ▌",
                @"▐   ⠂    ▌",
                @"▐   ⠈    ▌",
                @"▐    ⠂   ▌",
                @"▐    ⠠   ▌",
                @"▐     ⡀  ▌",
                @"▐     ⠠  ▌",
                @"▐      ⠂ ▌",
                @"▐      ⠈ ▌",
                @"▐       ⠂▌",
                @"▐       ⠠▌",
                @"▐       ⡀▌",
                @"▐      ⠠ ▌",
                @"▐      ⠂ ▌",
                @"▐     ⠈  ▌",
                @"▐     ⠂  ▌",
                @"▐    ⠠   ▌",
                @"▐    ⡀   ▌",
                @"▐   ⠠    ▌",
                @"▐   ⠂    ▌",
                @"▐  ⠈     ▌",
                @"▐  ⠂     ▌",
                @"▐ ⠠      ▌",
                @"▐ ⡀      ▌",
                @"▐⠠       ▌",
            ]);

        /// <summary>
        /// The shark spinner instance.
        /// </summary>
        public static Spinner Shark =>
            new([
                @"▐|\____________▌",
                @"▐_|\___________▌",
                @"▐__|\__________▌",
                @"▐___|\_________▌",
                @"▐____|\________▌",
                @"▐_____|\_______▌",
                @"▐______|\______▌",
                @"▐_______|\_____▌",
                @"▐________|\____▌",
                @"▐_________|\___▌",
                @"▐__________|\__▌",
                @"▐___________|\_▌",
                @"▐____________|\▌",
                @"▐____________/|▌",
                @"▐___________/|_▌",
                @"▐__________/|__▌",
                @"▐_________/|___▌",
                @"▐________/|____▌",
                @"▐_______/|_____▌",
                @"▐______/|______▌",
                @"▐_____/|_______▌",
                @"▐____/|________▌",
                @"▐___/|_________▌",
                @"▐__/|__________▌",
                @"▐_/|___________▌",
                @"▐/|____________▌",
            ]);

        /// <summary>
        /// The dqpb spinner instance.
        /// </summary>
        public static Spinner Dqpb =>
            new([
                @"d",
                @"q",
                @"p",
                @"b",
            ]);

        /// <summary>
        /// The weather spinner instance.
        /// </summary>
        public static Spinner Weather =>
            new([
                @"☀️ ",
                @"☀️ ",
                @"☀️ ",
                @"🌤",
                @"⛅️",
                @"🌥",
                @"☁️ ",
                @"🌧",
                @"🌨",
                @"🌧",
                @"🌨",
                @"🌧",
                @"🌨",
                @"⛈ ",
                @"🌨",
                @"🌧",
                @"🌨",
                @"☁️ ",
                @"🌥",
                @"⛅️",
                @"🌤",
                @"☀️ ",
                @"☀️ ",
            ]);

        /// <summary>
        /// The christmas spinner instance.
        /// </summary>
        public static Spinner Christmas =>
            new([
                @"🌲",
                @"🎄",
            ]);

        /// <summary>
        /// The grenade spinner instance.
        /// </summary>
        public static Spinner Grenade =>
            new([
                @"،  ",
                @"′  ",
                @" ´ ",
                @" ‾ ",
                @"  ⸌",
                @"  ⸊",
                @"  |",
                @"  ⁎",
                @"  ⁕",
                @" ෴ ",
                @"  ⁓",
                @"   ",
                @"   ",
                @"   ",
            ]);

        /// <summary>
        /// The point spinner instance.
        /// </summary>
        public static Spinner Point =>
            new([
                @"∙∙∙",
                @"●∙∙",
                @"∙●∙",
                @"∙∙●",
                @"∙∙∙",
            ]);

        /// <summary>
        /// The layer spinner instance.
        /// </summary>
        public static Spinner Layer =>
            new([
                @"-",
                @"=",
                @"≡",
            ]);

        /// <summary>
        /// The betaWave spinner instance.
        /// </summary>
        public static Spinner BetaWave =>
            new([
                @"ρββββββ",
                @"βρβββββ",
                @"ββρββββ",
                @"βββρβββ",
                @"ββββρββ",
                @"βββββρβ",
                @"ββββββρ",
            ]);

        /// <summary>
        /// The fingerDance spinner instance.
        /// </summary>
        public static Spinner FingerDance =>
            new([
                @"🤘 ",
                @"🤟 ",
                @"🖖 ",
                @"✋ ",
                @"🤚 ",
                @"👆 ",
            ]);

        /// <summary>
        /// The fistBump spinner instance.
        /// </summary>
        public static Spinner FistBump =>
            new([
                @"🤜　　　　🤛 ",
                @"🤜　　　　🤛 ",
                @"🤜　　　　🤛 ",
                @"　🤜　　🤛　 ",
                @"　　🤜🤛　　 ",
                @"　🤜✨🤛　　 ",
                @"🤜　✨　🤛　 ",
            ]);

        /// <summary>
        /// The soccerHeader spinner instance.
        /// </summary>
        public static Spinner SoccerHeader =>
            new([
                @" 🧑⚽️       🧑 ",
                @"🧑  ⚽️      🧑 ",
                @"🧑   ⚽️     🧑 ",
                @"🧑    ⚽️    🧑 ",
                @"🧑     ⚽️   🧑 ",
                @"🧑      ⚽️  🧑 ",
                @"🧑       ⚽️🧑  ",
                @"🧑      ⚽️  🧑 ",
                @"🧑     ⚽️   🧑 ",
                @"🧑    ⚽️    🧑 ",
                @"🧑   ⚽️     🧑 ",
                @"🧑  ⚽️      🧑 ",
            ]);

        /// <summary>
        /// The mindblown spinner instance.
        /// </summary>
        public static Spinner Mindblown =>
            new([
                @"😐 ",
                @"😐 ",
                @"😮 ",
                @"😮 ",
                @"😦 ",
                @"😦 ",
                @"😧 ",
                @"😧 ",
                @"🤯 ",
                @"💥 ",
                @"✨ ",
                @"　 ",
                @"　 ",
                @"　 ",
            ]);

        /// <summary>
        /// The speaker spinner instance.
        /// </summary>
        public static Spinner Speaker =>
            new([
                @"🔈 ",
                @"🔉 ",
                @"🔊 ",
                @"🔉 ",
            ]);

        /// <summary>
        /// The orangePulse spinner instance.
        /// </summary>
        public static Spinner OrangePulse =>
            new([
                @"🔸 ",
                @"🔶 ",
                @"🟠 ",
                @"🟠 ",
                @"🔶 ",
            ]);

        /// <summary>
        /// The bluePulse spinner instance.
        /// </summary>
        public static Spinner BluePulse =>
            new([
                @"🔹 ",
                @"🔷 ",
                @"🔵 ",
                @"🔵 ",
                @"🔷 ",
            ]);

        /// <summary>
        /// The orangeBluePulse spinner instance.
        /// </summary>
        public static Spinner OrangeBluePulse =>
            new([
                @"🔸 ",
                @"🔶 ",
                @"🟠 ",
                @"🟠 ",
                @"🔶 ",
                @"🔹 ",
                @"🔷 ",
                @"🔵 ",
                @"🔵 ",
                @"🔷 ",
            ]);

        /// <summary>
        /// The timeTravel spinner instance.
        /// </summary>
        public static Spinner TimeTravel =>
            new([
                @"🕛 ",
                @"🕚 ",
                @"🕙 ",
                @"🕘 ",
                @"🕗 ",
                @"🕖 ",
                @"🕕 ",
                @"🕔 ",
                @"🕓 ",
                @"🕒 ",
                @"🕑 ",
                @"🕐 ",
            ]);

        /// <summary>
        /// The aesthetic spinner instance.
        /// </summary>
        public static Spinner Aesthetic =>
            new([
                @"▰▱▱▱▱▱▱",
                @"▰▰▱▱▱▱▱",
                @"▰▰▰▱▱▱▱",
                @"▰▰▰▰▱▱▱",
                @"▰▰▰▰▰▱▱",
                @"▰▰▰▰▰▰▱",
                @"▰▰▰▰▰▰▰",
                @"▰▱▱▱▱▱▱",
            ]);

        /// <summary>
        /// Aesthetic (looped)
        /// </summary>
        public static Spinner AestheticLooped =>
            new([
                @"▰▱▱▱▱▱▱",
                @"▰▰▱▱▱▱▱",
                @"▰▰▰▱▱▱▱",
                @"▰▰▰▰▱▱▱",
                @"▰▰▰▰▰▱▱",
                @"▰▰▰▰▰▰▱",
                @"▰▰▰▰▰▰▰",
                @"▱▰▰▰▰▰▰",
                @"▱▱▰▰▰▰▰",
                @"▱▱▱▰▰▰▰",
                @"▱▱▱▱▰▰▰",
                @"▱▱▱▱▱▰▰",
                @"▱▱▱▱▱▱▰",
                @"▱▱▱▱▱▱▱"
            ]);

        /// <summary>
        /// The dwarfFortress spinner instance.
        /// </summary>
        public static Spinner DwarfFortress =>
            new([
                @" ██████£££  ",
                @"☺██████£££  ",
                @"☺██████£££  ",
                @"☺▓█████£££  ",
                @"☺▓█████£££  ",
                @"☺▒█████£££  ",
                @"☺▒█████£££  ",
                @"☺░█████£££  ",
                @"☺░█████£££  ",
                @"☺ █████£££  ",
                @" ☺█████£££  ",
                @" ☺█████£££  ",
                @" ☺▓████£££  ",
                @" ☺▓████£££  ",
                @" ☺▒████£££  ",
                @" ☺▒████£££  ",
                @" ☺░████£££  ",
                @" ☺░████£££  ",
                @" ☺ ████£££  ",
                @"  ☺████£££  ",
                @"  ☺████£££  ",
                @"  ☺▓███£££  ",
                @"  ☺▓███£££  ",
                @"  ☺▒███£££  ",
                @"  ☺▒███£££  ",
                @"  ☺░███£££  ",
                @"  ☺░███£££  ",
                @"  ☺ ███£££  ",
                @"   ☺███£££  ",
                @"   ☺███£££  ",
                @"   ☺▓██£££  ",
                @"   ☺▓██£££  ",
                @"   ☺▒██£££  ",
                @"   ☺▒██£££  ",
                @"   ☺░██£££  ",
                @"   ☺░██£££  ",
                @"   ☺ ██£££  ",
                @"    ☺██£££  ",
                @"    ☺██£££  ",
                @"    ☺▓█£££  ",
                @"    ☺▓█£££  ",
                @"    ☺▒█£££  ",
                @"    ☺▒█£££  ",
                @"    ☺░█£££  ",
                @"    ☺░█£££  ",
                @"    ☺ █£££  ",
                @"     ☺█£££  ",
                @"     ☺█£££  ",
                @"     ☺▓£££  ",
                @"     ☺▓£££  ",
                @"     ☺▒£££  ",
                @"     ☺▒£££  ",
                @"     ☺░£££  ",
                @"     ☺░£££  ",
                @"     ☺ £££  ",
                @"      ☺£££  ",
                @"      ☺£££  ",
                @"      ☺▓££  ",
                @"      ☺▓££  ",
                @"      ☺▒££  ",
                @"      ☺▒££  ",
                @"      ☺░££  ",
                @"      ☺░££  ",
                @"      ☺ ££  ",
                @"       ☺££  ",
                @"       ☺££  ",
                @"       ☺▓£  ",
                @"       ☺▓£  ",
                @"       ☺▒£  ",
                @"       ☺▒£  ",
                @"       ☺░£  ",
                @"       ☺░£  ",
                @"       ☺ £  ",
                @"        ☺£  ",
                @"        ☺£  ",
                @"        ☺▓  ",
                @"        ☺▓  ",
                @"        ☺▒  ",
                @"        ☺▒  ",
                @"        ☺░  ",
                @"        ☺░  ",
                @"        ☺   ",
                @"        ☺  &",
                @"        ☺ ☼&",
                @"       ☺ ☼ &",
                @"       ☺☼  &",
                @"      ☺☼  & ",
                @"      ‼   & ",
                @"     ☺   &  ",
                @"    ‼    &  ",
                @"   ☺    &   ",
                @"  ‼     &   ",
                @" ☺     &    ",
                @"‼      &    ",
                @"      &     ",
                @"      &     ",
                @"     &   ░  ",
                @"     &   ▒  ",
                @"    &    ▓  ",
                @"    &    £  ",
                @"   &    ░£  ",
                @"   &    ▒£  ",
                @"  &     ▓£  ",
                @"  &     ££  ",
                @" &     ░££  ",
                @" &     ▒££  ",
                @"&      ▓££  ",
                @"&      £££  ",
                @"      ░£££  ",
                @"      ▒£££  ",
                @"      ▓£££  ",
                @"      █£££  ",
                @"     ░█£££  ",
                @"     ▒█£££  ",
                @"     ▓█£££  ",
                @"     ██£££  ",
                @"    ░██£££  ",
                @"    ▒██£££  ",
                @"    ▓██£££  ",
                @"    ███£££  ",
                @"   ░███£££  ",
                @"   ▒███£££  ",
                @"   ▓███£££  ",
                @"   ████£££  ",
                @"  ░████£££  ",
                @"  ▒████£££  ",
                @"  ▓████£££  ",
                @"  █████£££  ",
                @" ░█████£££  ",
                @" ▒█████£££  ",
                @" ▓█████£££  ",
                @" ██████£££  ",
                @" ██████£££  ",
            ]);
    }
}
