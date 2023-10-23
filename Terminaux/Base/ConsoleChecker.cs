
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Reflection;
using Terminaux.Reader.Inputs;

namespace Terminaux.Base
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
                        int _ = ConsoleWrappers.ActionCursorLeft();

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
        /// Checks the running console for sanity, like the incompatible consoles, insane console types, etc.
        /// <br></br>
        /// The severity of the checks can be described in two categories:
        /// <br></br>
        /// <br></br>
        /// 1. Error: We'll throw <see cref="Exception"/> when we detect that the console is not conforming to the standards.
        /// <br></br>
        /// 2. Warning: We'll just issue a warning in yellow text using the plain 16-color writer when we detect that the console is not
        ///             supporting optional features, like 256-colors.
        /// </summary>
        public static void CheckConsole()
        {
            if (acknowledged)
                return;
            string TerminalType = ConsolePlatform.GetTerminalType();
            string TerminalEmulator = ConsolePlatform.GetTerminalEmulator();

            // First: Check if the console is running on Apple_Terminal (terminal.app).
            // Severity: Error
            // Explanation below:
            // ---
            // This check is needed because we have the stock Terminal.app (Apple_Terminal according to $TERM_PROGRAM) that has incompatibilities with
            // VT sequences, causing broken display. It claims it supports XTerm, yet it isn't fully XTerm-compliant, so we exit the program early when
            // this stock terminal is spotted.
            // ---
            // More information regarding this check: The blacklisted terminals will not be able to run this application properly, because they have
            // broken support for colors and possibly more features. For example, we have Apple_Terminal that has no support for 255 and true colors;
            // it only supports 16 colors setting by VT sequences and nothing can change that, although it's fully XTerm compliant.
            if (ConsolePlatform.IsOnMacOS())
            {
                if (TerminalEmulator == "Apple_Terminal")
                {
                    throw new Exception("This application makes use of VT escape sequences, but Terminal.app has broken support for 255 and true colors." + ConsolePlatform.NewLine +
                                        "Possible solution: Download iTerm2 here: https://iterm2.com/downloads.html");
                }
            }

            // Check if the terminal type is "dumb".
            // Severity: Error
            // Explanation below:
            // ---
            // The "dumb" terminals usually are not useful for interactive applications, since they only provide the stdout and stderr streams without
            // support for console cursor manipulation, which This application heavily depends on. These terminals are used for streaming output to the
            // appropriate variable, like the frontend applications that rely on console applications and their outputs to do their job (for example,
            // Brasero, a disk burning program, uses wodim, xorriso, and such applications to do its very intent of burning a blank CD-ROM. All these
            // backend applications are console programs).
            if (IsDumb)
            {
                throw new Exception("This application makes use of inputs and cursor manipulation, but the \"dumb\" terminals have no support for such tasks." + ConsolePlatform.NewLine + 
                                    "Possible solution: Use an appropriate terminal emulator or consult your terminal settings to set the terminal type into something other than \"dumb\"." + ConsolePlatform.NewLine +
                                    "                   We recommend using the \"vt100\" terminal emulators to get the most out of This application.");
            }

            // Check if the terminal supports 256 colors
            // Severity: Warning
            // Explanation below
            // ---
            // This application makes use of the 256 colors to print its own text by default. Even if we specify the 16-color compatibility values, we
            // still use the VT sequence to print colored text, but this will be changed later.
            if (!IsConsole256Colors())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (ConsolePlatform.IsOnWindows())
                    Console.WriteLine("Warning: Terminaux makes use of the 256 colors. Don't worry; Nitrocid will automatically set your terminal to handle these sequences, assuming that you're running a supported terminal. Press any key to continue.", true);
                else
                    Console.WriteLine("Warning: This application makes use of the 256 colors. Make sure that your terminal is set to run on 256 color mode. Your terminal is {0}. Press any key to continue.", TerminalType);
                Input.DetectKeypress();
            }

            // Don't check again.
            acknowledged = true;
        }

        /// <summary>
        /// Does the console support 256 colors? Always true on Windows
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
        internal static void CheckConsoleSize()
        {
            // If we're being run on TMUX, the status bar might mess up our interpretation of the window height.
            int MinimumWidth =  80;
            int MinimumHeight = 24;
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
                            MinimumHeight -= numStatusBars;
                    }
                    else
                    {
                        // We couldn't get the status variable from the global TMUX "status" variable because of command error.
                        // Assume that it's 1.
                        MinimumHeight--;
                    }
                }
                catch
                {
                    // We couldn't get the status variable from the global TMUX "status" variable. Assume that it's 1.
                    MinimumHeight--;
                }
            }

            // Check for the minimum console window requirements (80x24)
            while (ConsoleWrappers.ActionWindowWidth() < MinimumWidth | ConsoleWrappers.ActionWindowHeight() < MinimumHeight)
            {
                Console.WriteLine("Your console is too small to run properly: {0}x{1} | buff: {2}x{3}", ConsoleWrappers.ActionWindowWidth(), ConsoleWrappers.ActionWindowHeight(), Console.BufferWidth, ConsoleWrappers.ActionBufferHeight());
                Console.WriteLine("To have a better experience, resize your console window while still being on this screen. Press any key to continue...");
                Input.DetectKeypress();
            }
        }
    }
}
