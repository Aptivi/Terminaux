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

using System;
using System.IO;
using System.Reflection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Extensions;
using SpecProbe.Platform;
using Terminaux.Reader;
using Terminaux.Base.TermInfo;
using Terminaux.Colors.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;

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
                    }
                }
                catch { }
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
            string TerminalType = PlatformHelper.GetTerminalType();
            string TerminalEmulator = PlatformHelper.GetTerminalEmulator();

            // Check if the terminal type is "dumb".
            if (IsDumb)
            {
                throw new TerminauxException(
                    "This application makes use of inputs and cursor manipulation, but the \"dumb\" terminals have no support for such tasks." + Environment.NewLine +
                    "Possible solution: Use an appropriate terminal emulator or consult your terminal settings to set the terminal type into something other than \"dumb\". " +
                    "We recommend using the \"vt100\" terminal emulators to get the most out of this application."
                );
            }

            // Check the blacklist and the graylist for the console type
            var (blacklisted, justification) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist);
            var (graylisted, justification2) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Type, ConsoleFilterSeverity.Graylist);
            if (blacklisted)
            {
                throw new TerminauxException(
                    $"The console type you're currently using, {TerminalType}, is blacklisted: {justification}"
                );
            }
            if (graylisted)
                TextWriterRaw.WritePlain($"The console type you're currently using, {TerminalType}, is graylisted: {justification2}");

            // Check the blacklist and the graylist for the terminal emulator
            var (emuBlacklisted, emuJustification) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Emulator, ConsoleFilterSeverity.Blacklist);
            var (emuGraylisted, emuJustification2) = ConsoleFilter.IsConsoleFiltered(ConsoleFilterType.Emulator, ConsoleFilterSeverity.Graylist);
            if (emuBlacklisted)
            {
                throw new TerminauxException(
                    $"The terminal emulator you're currently using, {TerminalEmulator}, is blacklisted: {emuJustification}"
                );
            }
            if (emuGraylisted)
                TextWriterRaw.WritePlain($"The terminal emulator you're currently using, {TerminalEmulator}, is graylisted: {emuJustification2}");

            // Check for 256 colors
            if (!IsConsole256Colors() && PlatformHelper.IsOnUnix())
                TextWriterRaw.WritePlain($"Terminal type {TerminalType} doesn't support 256 colors according to terminfo");

            // Check to see if we can call cursor info without errors
            try
            {
                int _ = ConsoleWrapper.WindowWidth;
            }
            catch (IOException ex)
            {
                var asm = Assembly.GetEntryAssembly();
                if (asm is not null && !asm.FullName.Contains("testhost") && !asm.FullName.Contains("Terminaux.Tests"))
                {
                    TextWriterColor.WriteColor("You'll need to use winpty to be able to use this program. Can't continue.", ConsoleColors.Red);
                    Environment.FailFast("User tried to run a Terminaux program on Git Bash's MinTTY without winpty.", ex);
                }
            }
            catch
            {
                TextWriterColor.WriteColor("Console positioning is not working properly, so this application might behave erratically.", ConsoleColors.Yellow);
            }

            // Don't check again.
            busy = false;
            acknowledged = true;
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
                return termInfo.MaxColors >= 256;
            }
            else
                return ConsolePositioning.CheckForConHostSequenceSupport() == 7;
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
                    ConsolePositioning.FileExistsInPath("sh", ref shellPath);
                    string output = ConsolePositioning.ExecuteProcessToString(shellPath, "-c \"tmux show-options -v -g status | sed 's/on/1/g' | sed 's/off/0/g'\"", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ref exitCode, false);

                    // If we couldn't get this variable, assume that the status height is 1.
                    if (exitCode == 0)
                    {
                        // We got it! Now, let's parse it.
                        if (int.TryParse(output, out int numStatusBars))
                            minimumHeight -= numStatusBars;
                    }
                    else
                    {
                        // We couldn't get the status variable from the global TMUX "status" variable because of command error.
                        // Assume that it's 1.
                        minimumHeight--;
                    }
                }
                catch
                {
                    // We couldn't get the status variable from the global TMUX "status" variable. Assume that it's 1.
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
                        minimumHeight--;
                        break;
                    }
                }
            }

            // Check for the minimum console window requirements (80x24)
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
                TextWriterRaw.WritePlain("Your console is too small to run properly: {0}x{1} | buff: {2}x{3} | min: {4}x{5}",
                    ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight,
                    Console.BufferWidth, ConsoleWrapper.BufferHeight,
                    minimumWidth, minimumHeight);
                TextWriterRaw.WritePlain("To have a better experience, resize your console window while still being on this screen. Press any key to continue...");
                TermReader.ReadKey();
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
                var consolePtr = GetConsoleWindow();
                var result = SendMessage(consolePtr, WM_GETICON, IntPtr.Zero, IntPtr.Zero);
                conHost = result != IntPtr.Zero;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"IsConHost(): {e}");
            }

            // Return the result
            return conHost;
        }

        #region Windows-specific
        private const string winKernel = "kernel32.dll";
        private const string winUser = "user32.dll";
        private const int WM_GETICON = 0x007F;

        [DllImport(winKernel, SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(winUser, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        #endregion
    }
}
