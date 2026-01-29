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
using System.Threading;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Tests.Shared.Shells
{
    internal class TestShellEmptyInstance : BaseShell, IShell
    {
        /// <inheritdoc/>
        public override string ShellType => "TestShellEmpty";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            while (!Bail)
            {
                try
                {
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterRaw.WritePlain("There was an error in the shell." + CharManager.NewLine + "Error {0}: {1}", ex.GetType().FullName ?? "<null>", ex.Message);
                    continue;
                }
            }
        }
    }
}
