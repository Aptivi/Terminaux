//
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
using System.IO;
using System.Reflection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Extensions;
using SpecProbe.Software.Platform;
using Terminaux.Base.TermInfo;
using System.Collections.Generic;
using Terminaux.Inputs;
using Terminaux.Base.Extensions.Native;
using Terminaux.Shell.Commands.ProcessExecution;

namespace Terminaux.Base.Checks
{
    /// <summary>
    /// Console sanity checking module
    /// </summary>
    public static class ConsoleChecker
    {
        private static bool _dumbSet = false;
        private static bool _dumb = true;

        /// <summary>
        /// Is the console a dumb console?
        /// </summary>
        public static bool IsDumb
        {
            get
            {
                try
                {
                    // Try to cache the value
                    if (!_dumbSet)
                    {
                        _dumbSet = true;
                        int _ = ConsoleWrapper.CursorLeft;
                        _ = ConsoleWrapper.WindowWidth;

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        var filtered = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist);
                        if (!filtered.filtered)
                            _dumb = false;

                        // Additionally, check the isatty output
                        if (PlatformHelper.IsOnUnix())
                            _dumb = NativeMethods.isatty(0) != 1;

                        // Check for output redirection
                        if (Console.IsOutputRedirected || Console.IsErrorRedirected)
                            _dumb = true;
                    }
                }
                catch { }
                ConsoleLogger.Debug("Dumb is {0}", _dumb);
                return _dumb;
            }
        }

        /// <summary>
        /// Platform-dependent home path
        /// </summary>
        public static string HomePath
        {
            get
            {
                if (PlatformHelper.IsOnUnix())
                    return Environment.GetEnvironmentVariable("HOME");
                else
                    return Environment.GetEnvironmentVariable("USERPROFILE").Replace(@"\", "/");
            }
        }

        /// <summary>
        /// Does the console support 256 colors?
        /// </summary>
        public static bool IsConsole256Colors()
        {
            if (PlatformHelper.IsOnUnix())
            {
                string TerminalType = PlatformHelper.GetTerminalType();
                var termInfo = TermInfoDesc.Load(TerminalType);
                if (termInfo == null)
                    return true;
                if (termInfo.MaxColors is null)
                    return false;
                ConsoleLogger.Debug("Max colors is {0}", termInfo.MaxColors);
                ConsoleLogger.Info("Max color requirement met: {0}", termInfo.MaxColors.Value >= 256);
                return termInfo.MaxColors.Value >= 256;
            }
            else
            {
                IntPtr stdHandle = NativeMethods.GetStdHandle(-11);
                uint mode = ConsoleMisc.GetMode(stdHandle);
                ConsoleLogger.Debug("Mode is {0}", mode);
                ConsoleLogger.Info("Max color requirement met: {0}", (mode & 4) == 0);
                return (mode & 4) == 0;
            }
        }

        /// <summary>
        /// Checks the console size with edge cases
        /// </summary>
        /// <param name="minimumWidth">Minimum console window width to check</param>
        /// <param name="minimumHeight">Minimum console window height to check</param>
        public static bool CheckConsoleSize(int minimumWidth = 80, int minimumHeight = 24)
        {
            // If we're being run on TMUX, the status bar might mess up our interpretation of the window height.
            if (PlatformHelper.IsRunningFromTmux())
            {
                ConsoleLogger.Info("Running from TMUX. Total size may be affected.");
                try
                {
                    // Try to get the status variable from the global TMUX "status" variable. Additionally, we need to replace
                    // the value "on" with 1 and "off" with 0 to best reflect the state. It can take a value larger than 1, like
                    // a TMUX configuration that contains two status bars, then that variable would be 2.
                    //
                    // So, we invoke the `tmux show-options -v -g status` command to get the value from the running TMUX session
                    // via the global configuration options, filtering the result as necessary.
                    string shellPath = "/bin/sh";
                    int exitCode = -1;
                    FileExistsInPath("sh", ref shellPath);
                    ProcessExecutor.ExecuteProcessInternal(shellPath, "-c \"tmux show-options -v -g status | sed 's/on/1/g' | sed 's/off/0/g'\"", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ref exitCode, false, false, out string output, out _);
                    ConsoleLogger.Debug("tmux show-options... exited with status {0} and with output {1}", exitCode, output);

                    // If we couldn't get this variable, assume that the status height is 1.
                    if (exitCode == 0)
                    {
                        // We got it! Now, let's parse it.
                        if (int.TryParse(output, out int numStatusBars))
                        {
                            minimumHeight -= numStatusBars;
                            ConsoleLogger.Debug("Status bar height is {0}", numStatusBars);
                        }
                    }
                    else
                    {
                        // We couldn't get the status variable from the global TMUX "status" variable because of command error.
                        // Assume that it's 1.
                        minimumHeight--;
                    }
                }
                catch (Exception ex)
                {
                    // We couldn't get the status variable from the global TMUX "status" variable. Assume that it's 1.
                    ConsoleLogger.Error(ex, "Failed to try to get \"status\" variable");
                    minimumHeight--;
                }
            }
            else if (PlatformHelper.IsRunningFromScreen())
            {
                // The status bar for GNU screen is always one row long.
                string confPath = HomePath + "/.screenrc";
                string statusKey = "hardstatus";
                string[] screenRcLines = File.ReadAllLines(confPath);
                foreach (string line in screenRcLines)
                {
                    if (line.StartsWith(statusKey) && line != "hardstatus off")
                    {
                        ConsoleLogger.Debug("Found hardstatus being on [{0}]", line);
                        minimumHeight--;
                        break;
                    }
                }
            }

            // Check for the minimum console window requirements (80x24)
            ConsoleLogger.Info("Minimum width is {0}, and height is {1}", minimumWidth, minimumHeight);
            return
                !(ConsoleWrapper.WindowWidth < minimumWidth |
                  ConsoleWrapper.WindowHeight < minimumHeight);
        }

        /// <summary>
        /// Checks the console size with edge cases, prompting the user to resize the screen if the minimum console size requirements is not satisfied.
        /// </summary>
        /// <param name="minimumWidth">Minimum console window width to check</param>
        /// <param name="minimumHeight">Minimum console window height to check</param>
        public static void CheckConsoleSizePrompt(int minimumWidth = 80, int minimumHeight = 24)
        {
            // Check for the minimum console window requirements (80x24)
            while (!CheckConsoleSize(minimumWidth, minimumHeight))
            {
                TextWriterRaw.WritePlain(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_COORDS_TOO_SMALL_1"),
                    ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight,
                    ConsoleWrapper.BufferWidth, ConsoleWrapper.BufferHeight,
                    minimumWidth, minimumHeight);
                TextWriterRaw.WritePlain(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_COORDS_TOO_SMALL_2"));
                Input.ReadKey();
            }
        }

        /// <summary>
        /// Checks whether this current application is running off ConHost or not
        /// </summary>
        /// <returns>True if running off ConHost; otherwise, false. Always false on Linux systems.</returns>
        public static bool IsConHost()
        {
            // ConHost is not supported on non-Windows systems.
            if (!PlatformHelper.IsOnWindows())
                return false;

            // Now, try to perform ConHost-specific operations on the current terminal.
            bool conHost = false;
            try
            {
                var consolePtr = NativeMethods.GetConsoleWindow();
                var result = NativeMethods.SendMessage(consolePtr, NativeMethods.WM_GETICON, IntPtr.Zero, IntPtr.Zero);
                conHost = result != IntPtr.Zero;
                ConsoleLogger.Debug("Conhost detection returned {0}", conHost);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"Error while detecting conhost: {ex.Message}");
            }

            // Return the result
            return conHost;
        }

        internal static string PathLookupDelimiter =>
            Convert.ToString(Path.PathSeparator);

        internal static string PathsToLookup =>
            Environment.GetEnvironmentVariable("PATH");

        internal static List<string> GetPathList() =>
            [.. PathsToLookup.Split(Convert.ToChar(PathLookupDelimiter))];

        internal static bool FileExistsInPath(string FilePath, ref string Result)
        {
            var LookupPaths = GetPathList();
            string ResultingPath;
            foreach (string LookupPath in LookupPaths)
            {
                ResultingPath = Path.Combine(FilePath, LookupPath);
                if (File.Exists(ResultingPath))
                {
                    Result = ResultingPath;
                    return true;
                }
            }
            return false;
        }
    }
}
