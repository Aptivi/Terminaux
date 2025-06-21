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

namespace Terminaux.Writer.CyclicWriters.Graphical.Calendaring
{
    /// <summary>
    /// Calendar types
    /// </summary>
    public enum CalendarTypes
    {
        /// <summary>
        /// The Gregorian calendar (en-US culture)
        /// </summary>
        Gregorian,
        /// <summary>
        /// The Hijri calendar (ar culture)
        /// </summary>
        Hijri,
        /// <summary>
        /// The Persian calendar (fa culture)
        /// </summary>
        Persian,
        /// <summary>
        /// The Saudi-Hijri calendar (ar-SA culture)
        /// </summary>
        SaudiHijri,
        /// <summary>
        /// The Thai-Buddhist calendar (th-TH culture)
        /// </summary>
        ThaiBuddhist,
        /// <summary>
        /// The Chinese calendar (zh-CN culture)
        /// </summary>
        Chinese,
        /// <summary>
        /// The Japanese calendar (ja-JP culture)
        /// </summary>
        Japanese,
        /// <summary>
        /// The Taiwanese calendar (zh-TW culture)
        /// </summary>
        Taiwanese,
        /// <summary>
        /// Variant calendar that adapts to the current kernel culture
        /// </summary>
        Variant,
    }
}
