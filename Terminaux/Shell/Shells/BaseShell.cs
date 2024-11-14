﻿//
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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;

namespace Terminaux.Shell.Shells
{
    /// <summary>
    /// Base abstract shell class
    /// </summary>
    public abstract class BaseShell : IShell
    {

        /// <inheritdoc/>
        public virtual string ShellType { get; private set; } = "";

        /// <inheritdoc/>
        public virtual bool Bail { get; set; }

        /// <inheritdoc/>
        public virtual void InitializeShell(params object[] ShellArgs)
        {
            DebugWriter.WriteDebug(DebugLevel.F, "We shouldn't be here!!!");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

    }
}
