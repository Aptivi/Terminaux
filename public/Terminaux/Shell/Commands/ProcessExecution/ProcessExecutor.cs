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
using System.Diagnostics;
using System.Threading;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;

namespace Terminaux.Shell.Commands.ProcessExecution
{
    /// <summary>
    /// Process executor module
    /// </summary>
    public static class ProcessExecutor
    {
        internal static Thread processExecutorThread = new((processParams) => ExecuteProcess((ExecuteProcessThreadParameters?)processParams));

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        internal static void ExecuteProcess(ExecuteProcessThreadParameters? ThreadParams)
        {
            if (ThreadParams is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_EXCEPTION_THREADPARAMSNEEDED_PROCESSEXECUTION"));
            ExecuteProcess(ThreadParams.File, ThreadParams.Args);
        }

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static int ExecuteProcess(string File, string Args) =>
            ExecuteProcess(File, Args, ConsoleFilesystem.CurrentDir);

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="WorkingDirectory">Specifies the working directory</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static int ExecuteProcess(string File, string Args, string WorkingDirectory)
        {
            try
            {
                bool HasProcessExited = false;
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = File,
                    Arguments = Args,
                    WorkingDirectory = WorkingDirectory,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                };
                CommandProcess.StartInfo = CommandProcessStart;
                CommandProcess.EnableRaisingEvents = true;
                CommandProcess.OutputDataReceived += ExecutableOutput;
                CommandProcess.ErrorDataReceived += ExecutableOutput;
                CommandProcess.Exited += (sender, args) => HasProcessExited = true;

                // Start the process
                ConsoleLogger.Info("Starting process {0} with working directory {1} and arguments {2}...", File, WorkingDirectory, Args);
                CommandProcess.Start();
                CommandProcess.BeginOutputReadLine();
                CommandProcess.BeginErrorReadLine();

                // Wait for process exit
                while (!HasProcessExited | !CancellationHandlers.cancelRequested)
                {
                    if (HasProcessExited)
                    {
                        ConsoleLogger.Warning("Process exited! Output may not be complete!");
                        CommandProcess.WaitForExit();
                        ConsoleLogger.Debug("Flushed as much as possible.");
                        break;
                    }
                    else if (CancellationHandlers.cancelRequested)
                    {
                        ConsoleLogger.Warning("Process killed! Output may not be complete!");
                        CommandProcess.Kill();
                        CommandProcess.WaitForExit();
                        ConsoleLogger.Debug("Flushed as much as possible.");
                        break;
                    }
                }
                ConsoleLogger.Debug("Process exited with exit code {0}.", CommandProcess.ExitCode);
                return CommandProcess.ExitCode;
            }
            catch (ThreadInterruptedException)
            {
                CancellationHandlers.cancelRequested = false;
                return default;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_ERROREXECUTE1") + " {2}." + CharManager.NewLine + LanguageTools.GetLocalized("T_COMMON_ERRORDESC"), true, ConsoleColors.Red, ex.GetType().FullName ?? "<null>", ex.Message, File);
            }
            return -1;
        }

        /// <summary>
        /// Executes a file with specified arguments and puts the output to the string
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="exitCode">Application exit code. -1 if internal error occurred</param>
        /// <param name="includeStdErr">Include output printed to StdErr</param>
        /// <returns>Output of a command from stdout</returns>
        public static string ExecuteProcessToString(string File, string Args, ref int exitCode, bool includeStdErr) =>
            ExecuteProcessToString(File, Args, ConsoleFilesystem.CurrentDir, ref exitCode, includeStdErr);

        /// <summary>
        /// Executes a file with specified arguments and puts the output to the string
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="WorkingDirectory">Specifies the working directory</param>
        /// <param name="exitCode">Application exit code. -1 if internal error occurred</param>
        /// <param name="includeStdErr">Include output printed to StdErr</param>
        /// <returns>Output of a command from stdout</returns>
        public static string ExecuteProcessToString(string File, string Args, string WorkingDirectory, ref int exitCode, bool includeStdErr)
        {
            var commandOutputBuilder = new StringBuilder();
            try
            {
                bool HasProcessExited = false;
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = includeStdErr,
                    FileName = File,
                    Arguments = Args,
                    WorkingDirectory = WorkingDirectory,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                };
                CommandProcess.StartInfo = CommandProcessStart;

                // Set events up
                void DataReceivedHandler(object _, DataReceivedEventArgs data)
                {
                    if (data.Data is not null)
                        commandOutputBuilder.Append(data.Data);
                }
                CommandProcess.EnableRaisingEvents = true;
                CommandProcess.OutputDataReceived += DataReceivedHandler;
                if (includeStdErr)
                    CommandProcess.ErrorDataReceived += DataReceivedHandler;
                CommandProcess.Exited += (sender, args) => HasProcessExited = true;

                // Start the process
                ConsoleLogger.Debug("Starting process {0} with working directory {1} and arguments {2}...", File, WorkingDirectory, Args);
                CommandProcess.Start();
                CommandProcess.BeginOutputReadLine();
                if (includeStdErr)
                    CommandProcess.BeginErrorReadLine();

                // Wait for process exit
                while (!HasProcessExited | !CancellationHandlers.cancelRequested)
                {
                    if (HasProcessExited)
                    {
                        ConsoleLogger.Warning("Process exited! Output may not be complete!");
                        CommandProcess.WaitForExit();
                        ConsoleLogger.Debug("Flushed as much as possible.");
                        break;
                    }
                    else if (CancellationHandlers.cancelRequested)
                    {
                        ConsoleLogger.Warning("Process killed! Output may not be complete!");
                        CommandProcess.Kill();
                        CommandProcess.WaitForExit();
                        ConsoleLogger.Debug("Flushed as much as possible.");
                        break;
                    }
                }
                ConsoleLogger.Debug("Process exited with exit code {0}.", CommandProcess.ExitCode);
                exitCode = CommandProcess.ExitCode;
            }
            catch (ThreadInterruptedException)
            {
                CancellationHandlers.cancelRequested = false;
                exitCode = -1;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_ERROREXECUTE1") + " {2}." + CharManager.NewLine + LanguageTools.GetLocalized("T_COMMON_ERRORDESC"), true, ConsoleColors.Red, ex.GetType().FullName ?? "<null>", ex.Message, File);
                exitCode = -1;
            }
            return commandOutputBuilder.ToString();
        }

        /// <summary>
        /// Executes a file with specified arguments to a separate window. Doesn't block.
        /// </summary>
        internal static void ExecuteProcessForked(ExecuteProcessThreadParameters ThreadParams) =>
            ExecuteProcessForked(ThreadParams.File, ThreadParams.Args);

        /// <summary>
        /// Executes a file with specified arguments to a separate window. Doesn't block.
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static void ExecuteProcessForked(string File, string Args) =>
            ExecuteProcessForked(File, Args, ConsoleFilesystem.CurrentDir);

        /// <summary>
        /// Executes a file with specified arguments to a separate window. Doesn't block.
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="WorkingDirectory">Specifies the working directory</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static void ExecuteProcessForked(string File, string Args, string WorkingDirectory)
        {
            try
            {
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    FileName = File,
                    Arguments = Args,
                    WorkingDirectory = WorkingDirectory,
                    UseShellExecute = true,
                };
                CommandProcess.StartInfo = CommandProcessStart;

                // Start the process
                ConsoleLogger.Debug("Starting process {0} with working directory {1} and arguments {2}...", File, WorkingDirectory, Args);
                CommandProcess.Start();
            }
            catch (ThreadInterruptedException)
            {
                CancellationHandlers.cancelRequested = false;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_ERROREXECUTE1") + " {2}." + CharManager.NewLine + LanguageTools.GetLocalized("T_COMMON_ERRORDESC"), true, ConsoleColors.Red, ex.GetType().FullName ?? "<null>", ex.Message, File);
            }
        }

        /// <summary>
        /// Handles executable output
        /// </summary>
        /// <param name="sendingProcess">Sender</param>
        /// <param name="outLine">Output</param>
        private static void ExecutableOutput(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data is null)
                return;
            ConsoleLogger.Debug(outLine.Data);
            TextWriterColor.Write(outLine.Data);
        }

    }
}
