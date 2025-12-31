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
using System.Diagnostics;
using Terminaux.Base.Structures;
using Terminaux.Inputs.Pointer;

namespace Terminaux.Inputs
{
    /// <summary>
    /// Input event info
    /// </summary>
    [DebuggerDisplay("[{EventType}] {PointerEventContext} | {ConsoleKeyInfo?.Key} | {ReportedPos}")]
    public class InputEventInfo
    {
        private readonly PointerEventContext? ctx = null;
        private readonly ConsoleKeyInfo? cki = null;
        private readonly Coordinate? reportedPos = null;

        /// <summary>
        /// Event type to report
        /// </summary>
        public InputEventType EventType =>
            PointerEventContext is not null ? InputEventType.Mouse :
            ConsoleKeyInfo is not null ? InputEventType.Keyboard :
            ReportedPos is not null ? InputEventType.Position :
            InputEventType.None;

        /// <summary>
        /// Pointer event context
        /// </summary>
        public PointerEventContext? PointerEventContext =>
            ctx;

        /// <summary>
        /// Console key info
        /// </summary>
        public ConsoleKeyInfo? ConsoleKeyInfo =>
            cki;

        /// <summary>
        /// Reported position
        /// </summary>
        public Coordinate? ReportedPos =>
            reportedPos;

        internal InputEventInfo()
        { }

        internal InputEventInfo(PointerEventContext? ctx, ConsoleKeyInfo? cki, Coordinate? reportedPos)
        {
            this.ctx = ctx;
            this.cki = cki;
            this.reportedPos = reportedPos;
        }
    }
}
