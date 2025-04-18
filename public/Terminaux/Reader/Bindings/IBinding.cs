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

namespace Terminaux.Reader.Bindings
{
    /// <summary>
    /// Key binding interface
    /// </summary>
    public interface IBinding
    {
        /// <summary>
        /// Keys to bind to
        /// </summary>
        public ConsoleKeyInfo[] BoundKeys { get; }

        /// <summary>
        /// Custom keys to bind this keybinding to
        /// </summary>
        public List<ConsoleKeyInfo>? CustomKeys { get; }

        /// <summary>
        /// Resets the suggestions text position
        /// </summary>
        public bool ResetSuggestionsTextPos { get; }

        /// <summary>
        /// Whether this binding adds manipulated to the changes list or not. If there is no difference after executing
        /// this binding, even if this is set to true, the new input text won't be appended.
        /// </summary>
        public bool AppendsChangesList { get; }

        /// <summary>
        /// Whether this binding uses the argument number for repetition or for other action.
        /// </summary>
        public bool ArgumentNumberIsRepetition { get; }

        /// <summary>
        /// Whether the bind matched
        /// </summary>
        /// <param name="input">Key</param>
        public bool BindMatched(ConsoleKeyInfo input);

        /// <summary>
        /// Do the action
        /// </summary>
        /// <param name="state">State of the reader</param>
        public void DoAction(TermReaderState state);
    }
}
