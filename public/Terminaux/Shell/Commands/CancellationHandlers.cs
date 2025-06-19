//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Shells;
using Terminaux.Base;
using Terminaux.Shell.Commands.ProcessExecution;
using Terminaux.Base.Wrappers;

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// Cancellation handler tools
    /// </summary>
    public static class CancellationHandlers
    {

        internal static CancellationTokenSource cts = new();
        internal static bool canCancel = false;
        internal static bool installed;
        internal static bool cancelRequested;

        /// <summary>
        /// Whether cancellation is requested
        /// </summary>
        public static bool CancelRequested =>
            cancelRequested;

        /// <summary>
        /// Allows cancelling the current command
        /// </summary>
        public static void AllowCancel() =>
            canCancel = true;

        /// <summary>
        /// Prevents cancelling the current command
        /// </summary>
        public static void InhibitCancel() =>
            canCancel = false;

        internal static void CancelCommand(object? sender, ConsoleCancelEventArgs e)
        {
            // We can't cancel in a situation where there are no shells.
            if (ShellManager.ShellStack.Count <= 0)
            {
                ConsoleLogger.Warning("No shells. Can't cancel.");
                e.Cancel = true;
                return;
            }

            // We can't cancel in situations where cancellation is not possible
            if (!canCancel)
            {
                ConsoleLogger.Warning("Cancellation impossible. Can't cancel.");
                e.Cancel = true;
                return;
            }

            // Now, handle the command cancellation
            try
            {
                var StartCommandThread = ShellManager.ShellStack[ShellManager.ShellStack.Count - 1].ShellCommandThread;
                var syncLock = GetCancelSyncLock(ShellManager.CurrentShellType);
                lock (syncLock)
                {
                    ConsoleLogger.Debug("Locking to cancel...");
                    cancelRequested = true;
                    TextWriterRaw.Write();
                    ConsoleWrapperTools.SetWrapperLocal(nameof(Null));
                    cts.Cancel();
                    ProcessExecutor.processExecutorThread.Interrupt();
                    ProcessExecutor.processExecutorThread.Join();
                    ProcessExecutor.processExecutorThread = new Thread((processParams) => ProcessExecutor.ExecuteProcess((ExecuteProcessThreadParameters?)processParams));
                    ConsoleWrapperTools.UnsetWrapperLocal();
                    ConsoleLogger.Debug("Cancelled command.");
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Cannot cancel. {0}", ex.Message);
            }
            e.Cancel = true;
        }

        internal static void InstallHandler()
        {
            if (!installed)
            {
                Console.CancelKeyPress += CancelCommand;
                installed = true;
            }
        }

        internal static object GetCancelSyncLock(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).ShellLock;

    }
}
