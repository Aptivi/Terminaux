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
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Checks
{
    /// <summary>
    /// Console sanity checking module
    /// </summary>
    public static class ConsoleChecker
    {

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
                    // Get terminal type
                    string TerminalType = ConsolePlatform.GetTerminalType();

                    // Try to cache the value
                    if (!_dumbSet)
                    {
                        _dumbSet = true;
                        int _ = ConsoleWrapper.CursorLeft;

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        if (TerminalType != "dumb" && TerminalType != "unknown")
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
                if (ConsolePlatform.IsOnUnix())
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
            string TerminalType = ConsolePlatform.GetTerminalType();
            string TerminalEmulator = ConsolePlatform.GetTerminalEmulator();

            // Check if the terminal type is "dumb".
            if (IsDumb)
            {
                throw new TerminauxException(
                    "This application makes use of inputs and cursor manipulation, but the \"dumb\" terminals have no support for such tasks." + Environment.NewLine +
                    "Possible solution: Use an appropriate terminal emulator or consult your terminal settings to set the terminal type into something other than \"dumb\"." + Environment.NewLine +
                    "                   We recommend using the \"vt100\" terminal emulators to get the most out of this application."
                );
            }

            // Check the blacklist and the greylist for the console type
            var (blacklisted, justification) = TerminalTypeBlacklist.IsTypeBlacklisted();
            var (greylisted, justification2) = TerminalTypeGreylist.IsTypeGreylisted();
            if (blacklisted)
            {
                throw new TerminauxException(
                    $"The console type you're currently using, {TerminalType}, is blacklisted: {justification}"
                );
            }
            if (greylisted)
                TextWriterColor.WritePlain($"The console type you're currently using, {TerminalType}, is greylisted: {justification2}");

            // Check the blacklist and the greylist for the terminal emulator
            var (emuBlacklisted, emuJustification) = TerminalEmulatorBlacklist.IsEmulatorBlacklisted();
            var (emuGreylisted, emuJustification2) = TerminalEmulatorGreylist.IsEmulatorGreylisted();
            if (emuBlacklisted)
            {
                throw new TerminauxException(
                    $"The terminal emulator you're currently using, {TerminalEmulator}, is blacklisted: {emuJustification}"
                );
            }
            if (emuGreylisted)
                TextWriterColor.WritePlain($"The terminal emulator you're currently using, {TerminalEmulator}, is greylisted: {emuJustification2}");

            // Don't check again.
            acknowledged = true;
        }

        /// <summary>
        /// Does the console support 256 colors?
        /// </summary>
        public static bool IsConsole256Colors()
        {
            if (ConsolePlatform.IsOnUnix())
            {
                string TerminalType = ConsolePlatform.GetTerminalType();
                return TerminalType.Contains("-256col");
            }
            else
                return ConsoleExtensions.CheckForConHostSequenceSupport() == 7;
        }

        /// <summary>
        /// Checks the console size with edge cases
        /// </summary>
        /// <param name="minimumWidth">Minimum console window width to check</param>
        /// <param name="minimumHeight">Minimum console window height to check</param>
        public static bool CheckConsoleSize(int minimumWidth = 80, int minimumHeight = 24)
        {
            // If we're being run on TMUX, the status bar might mess up our interpretation of the window height.
            if (ConsolePlatform.IsRunningFromTmux())
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
                    ConsoleExtensions.FileExistsInPath("sh", ref shellPath);
                    string output = ConsoleExtensions.ExecuteProcessToString(shellPath, "-c \"tmux show-options -v -g status | sed 's/on/1/g' | sed 's/off/0/g'\"", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ref exitCode, false);

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
            else if (ConsolePlatform.IsRunningFromScreen())
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
                TextWriterColor.WritePlain("Your console is too small to run properly: {0}x{1} | buff: {2}x{3} | min: {4}x{5}",
                    ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight,
                    Console.BufferWidth, ConsoleWrapper.BufferHeight,
                    minimumWidth, minimumHeight);
                TextWriterColor.WritePlain("To have a better experience, resize your console window while still being on this screen. Press any key to continue...");
                Input.DetectKeypress();
            }
        }
    }
}
