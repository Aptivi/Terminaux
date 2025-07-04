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
using Terminaux.Colors.Data;
using System.Diagnostics;
using System.Collections.Generic;
using Textify.General;
using System.Text;
using System.Threading;
using Terminaux.Inputs;
using Terminaux.Base.Extensions.Native;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Colors.Themes.Colors;

namespace Terminaux.Base.Checks
{
    /// <summary>
    /// Console sanity checking module
    /// </summary>
    public static class ConsoleChecker
    {

        internal static bool busy = false;
        private static bool acknowledged = false;
        private static bool _dumbSet = false;
        private static bool _dumb = true;
        private static readonly string[] whitelist = ["testhost", "Terminaux.Tests"];
        private static readonly List<string> customWhitelist = [];

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

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        var filtered = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist);
                        if (!filtered.filtered)
                            _dumb = false;

                        // Additionally, check the isatty output
                        if (PlatformHelper.IsOnUnix())
                            _dumb = NativeMethods.isatty(0) != 1;
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
        /// Checks the running console for sanity, like the incompatible consoles, insane console types, etc.
        /// </summary>
        public static void CheckConsole()
        {
            if (acknowledged)
                return;
            busy = true;
            ConsoleLogger.Info("Checking for console");

            // First, get the assembly for whitelist
            var asm = Assembly.GetEntryAssembly();
            if (asm is null || asm.FullName.ContainsAnyOf(whitelist) || asm.FullName.ContainsAnyOf([.. customWhitelist]))
            {
                ConsoleLogger.Debug("Not going to check console due to assembly being whitelisted (B: {0}, C: {1}) or null", whitelist.Length, customWhitelist.Count);
                busy = false;
                acknowledged = true;
                return;
            }

            // Now, check the type
            string TerminalType = PlatformHelper.GetTerminalType();
            string TerminalEmulator = PlatformHelper.GetTerminalEmulator();
            ConsoleLogger.Debug("Checking against terminal {0} on {1}", TerminalType, TerminalEmulator);

            // Check if the terminal type is "dumb".
            if (IsDumb)
            {
                ConsoleLogger.Fatal("Terminal {0} on {1} is dumb!", TerminalType, TerminalEmulator);
                FastFail(
                    LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_ISDUMB_EVT_MESSAGE"),
                    LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_ISDUMB_MESSAGE_1") + Environment.NewLine +
                    LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_ISDUMB_MESSAGE_2")
                );
            }

            // Check the blacklist and the graylist for the console type
            var (blacklisted, justification) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist);
            var (graylisted, justification2) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Type, ConsoleFilterSeverity.Graylist);
            ConsoleLogger.Debug("Terminal {0} on {1} blacklisted: {2}: {3}", TerminalType, TerminalEmulator, blacklisted, justification);
            ConsoleLogger.Debug("Terminal {0} on {1} graylisted: {2}: {3}", TerminalType, TerminalEmulator, graylisted, justification2);
            if (blacklisted)
            {
                ConsoleLogger.Fatal("Terminal {0} on {1} is blacklisted!", TerminalType, TerminalEmulator);
                FastFail(
                    LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_TT_BLACKLISTED_EVT_MESSAGE"),
                    LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_TT_BLACKLISTED_MESSAGE_1").FormatString(TerminalType, justification)
                );
            }
            if (graylisted)
            {
                ConsoleLogger.Warning("Terminal {0} on {1} is graylisted!", TerminalType, TerminalEmulator);
                TextWriterRaw.WritePlain(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_TT_GRAYLISTED_MESSAGE").FormatString(TerminalType, justification2));
            }

            // Check the blacklist and the graylist for the terminal emulator
            var (emuBlacklisted, emuJustification) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Emulator, ConsoleFilterSeverity.Blacklist);
            var (emuGraylisted, emuJustification2) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Emulator, ConsoleFilterSeverity.Graylist);
            ConsoleLogger.Debug("Emulator {0} blacklisted: {1}: {2}", TerminalEmulator, emuBlacklisted, emuJustification);
            ConsoleLogger.Debug("Emulator {0} graylisted: {1}: {2}", TerminalEmulator, emuGraylisted, emuJustification2);
            if (emuBlacklisted)
            {
                ConsoleLogger.Fatal("Emulator {0} is blacklisted!", TerminalEmulator);
                FastFail(
                    LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_TE_BLACKLISTED_EVT_MESSAGE"),
                    LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_TE_BLACKLISTED_MESSAGE_1").FormatString(TerminalEmulator, emuJustification)
                );
            }
            if (emuGraylisted)
            {
                ConsoleLogger.Warning("Emulator {0} is graylisted!", TerminalEmulator);
                TextWriterRaw.WritePlain(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_TE_GRAYLISTED_MESSAGE").FormatString(TerminalEmulator, emuJustification2));
            }

            // Check for 256 colors
            if (!IsConsole256Colors() && PlatformHelper.IsOnUnix())
                TextWriterRaw.WritePlain(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_TT_NO256COLORS_MESSAGE").FormatString(TerminalType));

            // Check to see if we can call cursor info without errors
            try
            {
                int _ = ConsoleWrapper.WindowWidth;
            }
            catch (IOException ex)
            {
                ConsoleLogger.Error(ex, "Checking against terminal {0} on {1} for positioning failed with an I/O error", TerminalType, TerminalEmulator);
                if (PlatformHelper.IsOnWindows())
                {
                    ConsoleLogger.Fatal("It may be running in Git Bash's MinTTY!");
                    FastFail(
                        LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_MINTTY_EVT_MESSAGE"),
                        LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_MINTTY_MESSAGE_1"),
                        ex
                    );
                }
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_POS_IOERROR_MESSAGE"), ThemeColorType.Error);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Checking against terminal {0} on {1} for positioning failed", TerminalType, TerminalEmulator);
                TextWriterColor.Write(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_POS_ERROR_MESSAGE"), ThemeColorType.Error);
            }

            // Set the encoding
            if (PlatformHelper.IsOnWindows())
            {
                ConsoleLogger.Debug("Setting codepage...");
                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;
            }

            // Don't check again.
            busy = false;
            acknowledged = true;
            ConsoleLogger.Info("This Terminaux application can now run!");

            var version = typeof(ConsoleChecker).Assembly.GetName().Version;
            InfoBoxModalColor.WriteInfoBoxModal(
                "Welcome to the Terminaux Beta Program!\n\n" +
                $"Terminaux {version.Major}.{version.Minor} (Beta 4 - July 31st, 2025)\n\n" +
                "We are introducing you to this beta program to get an early taste of the upcoming " +
                "version of Terminaux with its new features and improved existing features. You can " +
                "report bugs or feature suggestions to the Terminaux project via GitHub. However, we " +
                "can't guarantee that everything works flawlessly, and there may be some features that " +
                "might not make it to the final version.\n\n" +
                "To get started, please press any key.",
                new InfoBoxSettings()
                {
                    Title = $"Terminaux {version.Major}.{version.Minor} Beta Program",
                    ForegroundColor = ConsoleColors.Yellow,
                }
            );
            ConsoleWrapper.Clear();
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
                    string output = ExecuteProcessToString(shellPath, "-c \"tmux show-options -v -g status | sed 's/on/1/g' | sed 's/off/0/g'\"", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ref exitCode, false);
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

        /// <summary>
        /// Adds the assembly to the check whitelist
        /// </summary>
        /// <param name="asm">Assembly to add</param>
        public static void AddToCheckWhitelist(Assembly asm)
        {
            if (asm is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_EXCEPTION_NOASSEMBLY"));

            // Add the partial name for the assembly to the whitelist
            if (!customWhitelist.Contains(asm.FullName))
            {
                customWhitelist.Add(asm.FullName);
                ConsoleLogger.Debug("Added assembly {0} to check whitelist", asm.FullName);
            }
        }

        /// <summary>
        /// Removes the assembly from the check whitelist
        /// </summary>
        /// <param name="asm">Assembly to remove</param>
        public static void RemoveFromCheckWhitelist(Assembly asm)
        {
            if (asm is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_EXCEPTION_NOASSEMBLY"));

            // Add the partial name for the assembly to the whitelist
            if (customWhitelist.Contains(asm.FullName))
            {
                customWhitelist.Remove(asm.FullName);
                ConsoleLogger.Debug("Removed assembly {0} from check whitelist", asm.FullName);
            }
        }

        internal static void FastFail(string eventMessage, string description, Exception? ex = null)
        {
            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_BC_CONSOLECHECKER_FASTFAIL_RUDEEXIT").FormatString(description), ConsoleColors.Red);
            ConsoleLogger.Fatal("Event: {0}, desc: {1}", eventMessage, description);
            ConsoleLogger.Fatal("Rude exit");
            if (ex is null)
                Environment.FailFast(eventMessage);
            else
                Environment.FailFast(eventMessage, ex);
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

        internal static string ExecuteProcessToString(string File, string Args, string WorkingDirectory, ref int exitCode, bool includeStdErr)
        {
            var commandOutputBuilder = new StringBuilder();
            try
            {
                bool HasProcessExited = false;
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    RedirectStandardInput = false,
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
                CommandProcess.Start();
                CommandProcess.BeginOutputReadLine();
                if (includeStdErr)
                    CommandProcess.BeginErrorReadLine();

                // Wait for process exit
                while (!HasProcessExited)
                {
                    if (HasProcessExited)
                    {
                        CommandProcess.WaitForExit();
                        break;
                    }
                }
                exitCode = CommandProcess.ExitCode;
            }
            catch (ThreadInterruptedException)
            {
                exitCode = -1;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"Error trying to execute command {File}. Error {ex.GetType().FullName}: {ex.Message}");
                exitCode = -1;
            }
            return commandOutputBuilder.ToString();
        }
    }
}
