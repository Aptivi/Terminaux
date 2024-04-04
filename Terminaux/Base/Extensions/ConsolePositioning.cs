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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using Textify.General;
using SpecProbe.Platform;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Base.Termios;
using System.Reflection;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console positioning tools
    /// </summary>
    public static class ConsolePositioning
    {
        /// <summary>
        /// Clears the console buffer, but keeps the current cursor position
        /// </summary>
        public static void ClearKeepPosition()
        {
            int Left = ConsoleWrapper.CursorLeft;
            int Top = ConsoleWrapper.CursorTop;
            TextWriterRaw.WriteRaw(
                ConsoleClearing.GetClearWholeScreenSequence() +
                CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)
            );
        }

        /// <summary>
        /// Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="line">Whether to simulate the new line at the end of text or not</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public static (int, int) GetFilteredPositions(string Text, bool line, params object[] Vars)
        {
            int LeftSeekPosition = ConsoleWrapper.CursorLeft;
            int TopSeekPosition = ConsoleWrapper.CursorTop;

            // If the string is null before or after processing the text, don't seek.
            bool noSeek = false;
            if (string.IsNullOrEmpty(Text))
                noSeek = true;

            // Filter all text from the VT escape sequences
            Text = ConsoleMisc.FilterVTSequences(Text);

            // Seek through filtered text (make it seem like it came from Linux by removing CR (\r)), return to the old position, and return the filtered positions
            Text = TextTools.FormatString(Text, Vars);
            Text = Text.Replace(Convert.ToString(Convert.ToChar(13)), "");
            Text = Text.Replace(Convert.ToString(Convert.ToChar(0)), "");
            if (string.IsNullOrEmpty(Text))
                noSeek = true;

            // Really seek if we need to
            if (!noSeek)
            {
                var texts = ConsoleMisc.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth, ConsoleWrapper.CursorLeft);
                for (int i = 0; i < texts.Length; i++)
                {
                    string text = texts[i];
                    for (int j = 1; j <= text.Length; j++)
                    {
                        // If we spotted a new line character, get down by one line.
                        if (text[j - 1] == Convert.ToChar(10))
                        {
                            if (TopSeekPosition < ConsoleWrapper.BufferHeight - 1)
                                TopSeekPosition += 1;
                            LeftSeekPosition = 0;
                        }
                        else
                        {
                            // Simulate seeking through text
                            LeftSeekPosition += 1;
                            if (LeftSeekPosition >= ConsoleWrapper.WindowWidth)
                            {
                                // We've reached end of line
                                LeftSeekPosition = 0;
                            }
                        }
                    }

                    // Get down by one line
                    if (i < texts.Length - 1)
                    {
                        TopSeekPosition += 1;
                        LeftSeekPosition = 0;
                    }
                    if (TopSeekPosition > ConsoleWrapper.BufferHeight - 1)
                    {
                        // We're at the end of buffer! Decrement by one and bail.
                        TopSeekPosition -= 1;
                        LeftSeekPosition = texts[texts.Length - 1].Length;
                        if (LeftSeekPosition >= ConsoleWrapper.WindowWidth)
                            LeftSeekPosition = ConsoleWrapper.WindowWidth - 1;
                        break;
                    }
                }
            }

            // If new line is to be appended at the end of text, just simulate going down.
            if (line)
            {
                // Do the same as if we've inserted a new line in the middle of the text, but make
                // sure that the left seek position is not zero for text that fill the whole line.
                //
                // There are legitimate writers, like SeparatorColor, that attempt to fill the whole
                // line with the separator character. For this very reason, consoles tend to wrap the
                // whole line to the new row with the left position set to zero. For writers that use
                // the Line argument, if the left seek position is above zero after the write, the
                // top will increase by one and the buffer check is done.
                //
                // However, filling the line, as seen by the above logic, requires us to set the left
                // seek position to zero, causing the top seek position to go down one row.
                TopSeekPosition += 1;
                if (TopSeekPosition > ConsoleWrapper.BufferHeight - 1)
                    TopSeekPosition -= 1;
                LeftSeekPosition = 0;
            }

            // Return the filtered positions
            return (LeftSeekPosition, TopSeekPosition);
        }

        /// <summary>
        /// A safe way to get console dimensions
        /// </summary>
        /// <returns>A column and a row in a tuple</returns>
        public static (int column, int row) GetDimensionsSafe()
        {
            // Check to see if we're able to get access to the two properties.
            try
            {
                return (Console.WindowWidth, Console.WindowHeight);
            }
            catch (IOException)
            {
                // Looks like that we failed to get the position.
                if (PlatformHelper.IsOnUnix())
                {
                    // Use Termios, which gives us enough functions to disable input blocking, to get the position.
                    return GetPositionUsingTermios();
                }
                else
                {
                    // We may be running on MinTTY or any other Windows console that doesn't allow calling the two
                    // above properties because of "Unhandled exception. System.IO.IOException: The handle is invalid."
                    // In this case, fall back to the VT sequence method. Interestingly, we're also not allowed to call
                    // the standard Console.CursorLeft and CursorTop due to the same error, so we need to find a way to
                    // somehow get the cursor position.
                    return GetPositionUsingMsysInvocation();
                }
            }
        }

        #region Windows-specific
        private const string winKernel = "kernel32.dll";

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern IntPtr GetStdHandle(int handle);

        /// <summary>
        /// Initializes the VT sequence handling for Windows systems.
        /// </summary>
        /// <returns></returns>
        public static bool InitializeSequences()
        {
            if (!PlatformHelper.IsOnWindows())
                return true;
            IntPtr stdHandle = GetStdHandle(-11);
            int mode = CheckForConHostSequenceSupport();
            if (mode != 7)
                return SetConsoleMode(stdHandle, mode | 4);
            return true;
        }

        internal static int CheckForConHostSequenceSupport()
        {
            IntPtr stdHandle = GetStdHandle(-11);
            GetConsoleMode(stdHandle, out int mode);
            return mode;
        }
        #endregion

        #region Internal extensions
        internal static string NeutralizePath(string Path, string Source, bool Strict = false)
        {
            Path ??= "";
            Source ??= "";

            // Unescape the characters
            Path = Regex.Unescape(Path.Replace(@"\", "/"));
            Source = Regex.Unescape(Source.Replace(@"\", "/"));

            // Append current directory to path
            if (PlatformHelper.IsOnWindows() & !Path.Contains(":/") | PlatformHelper.IsOnUnix() & !Path.StartsWith("/"))
                if (!Source.EndsWith("/"))
                    Path = $"{Source}/{Path}";
                else
                    Path = $"{Source}{Path}";

            // Replace last occurrences of current directory of path with nothing.
            if (!string.IsNullOrEmpty(Source))
                if (Path.Contains(Source) & Path.AllIndexesOf(Source).Count() > 1)
                    Path = Path.ReplaceLastOccurrence(Source, "");

            // Finalize the path in case NeutralizePath didn't normalize it correctly.
            Path = System.IO.Path.GetFullPath(Path).Replace(@"\", "/");

            // If strict, checks for existence of file
            if (Strict)
                if (File.Exists(Path) | Directory.Exists(Path))
                    return Path;
                else
                    throw new TerminauxException($"Neutralized a non-existent path. {Path}");
            else
                return Path;
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
                ResultingPath = NeutralizePath(FilePath, LookupPath);
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
                Debug.WriteLine($"Error trying to execute command {File}. Error {ex.GetType().FullName}: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                exitCode = -1;
            }
            return commandOutputBuilder.ToString();
        }

        internal static string BufferChar(string text, Match[][] sequencesCollections, ref int i, ref int vtSeqIdx, out bool isVtSequence)
        {
            // Before buffering the character, check to see if we're surrounded by the VT sequence. This is to work around
            // the problem in .NET 6.0 Linux that prevents it from actually parsing the VT sequences like it's supposed to
            // do in Windows.
            //
            // Windows 10, Windows 11, and higher contain cmd.exe that checks to see if we passed it the escape character
            // alone, and it tries to parse each sequence passed to it.
            //
            // Linux, on the other hand, the terminal emulator has a completely different behavior, because it just omits
            // the escape character, which results in the entire sequence being printed except the Escape \u001b key, which
            // is not the way that it's supposed to work.
            //
            // To overcome this limitation, we need to print the whole sequence to the console found by the virtual terminal
            // control sequence matcher to match how it works on Windows.
            char ch = text[i];
            string seq = "";
            bool vtSeq = false;
            foreach (var sequences in sequencesCollections)
            {
                if (sequences.Length > 0 && sequences[vtSeqIdx].Index == i)
                {
                    // We're at an index which is the same as the captured VT sequence. Get the sequence
                    seq = sequences[vtSeqIdx].Value;
                    vtSeq = true;

                    // Raise the index in case we have the next sequence, but only if we're sure that we have another
                    if (vtSeqIdx + 1 < sequences.Length)
                        vtSeqIdx++;

                    // Raise the paragraph index by the length of the sequence
                    i += seq.Length - 1;
                }
            }
            isVtSequence = vtSeq;
            return !string.IsNullOrEmpty(seq) ? seq : ch.ToString();
        }

        internal static void SwapIfSourceLarger(this ref int SourceNumber, ref int TargetNumber)
        {
            int Source = SourceNumber;
            int Target = TargetNumber;
            if (SourceNumber > TargetNumber)
            {
                SourceNumber = Target;
                TargetNumber = Source;
            }
        }
        #endregion

        #region Other internals
        internal static (int column, int row) GetPositionUsingTermios()
        {
            static unsafe (int column, int row) ReportCursorStatus()
            {
                // Get two exact copies of Termios settings
                ConsoleLibcTermios termiosOrig, termiosMod;
                ConsoleLibcTermiosTools.tcgetattr(0, &termiosOrig);
                termiosMod = termiosOrig;
                termiosMod.c_lflag &= ~(ConsoleLibcTermiosTools.ICANON | ConsoleLibcTermiosTools.ECHO);
                ConsoleLibcTermiosTools.tcsetattr(0, ConsoleLibcTermiosTools.TCSANOW, &termiosMod);

                string columnStr = "0", rowStr = "0";
                try
                {
                    // Now, write a sequence that gives us the cursor position
                    TextWriterRaw.WriteRaw("\u001b[6n");

                    // Parse the reply
                    bool inEscape = false;
                    bool inRow = true;
                    List<string> columnStrs = [];
                    List<string> rowStrs = [];
                    while (true)
                    {
                        int characterInt = Console.Read();
                        char character = (char)characterInt;
                        if (!inEscape)
                        {
                            if (character == CharManager.GetEsc())
                                inEscape = true;
                        }
                        else if (character != '[')
                        {
                            if (character == ';')
                                inRow = false;
                            if (character == 'R')
                                break;
                            else if (char.IsNumber(character))
                            {
                                string charString = $"{character}";
                                if (inRow)
                                    rowStrs.Add(charString);
                                else
                                    columnStrs.Add(charString);
                            }
                        }
                    }

                    // We're done parsing, so calculate the column and the row
                    columnStr = string.Join("", columnStrs);
                    rowStr = string.Join("", rowStrs);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GetPositionUsingTermios(): {ex}");
                }
                finally
                {
                    ConsoleLibcTermiosTools.tcsetattr(0, ConsoleLibcTermiosTools.TCSANOW, &termiosOrig);
                }

                // Return the result
                return (Convert.ToInt32(columnStr), Convert.ToInt32(rowStr));
            }

            // Do the job!
            var oldStatus = ReportCursorStatus();
            int oldColumn = oldStatus.column;
            int oldRow = oldStatus.row;
            TextWriterRaw.WriteRaw(CsiSequences.GenerateCsiCursorPosition(int.MaxValue, int.MaxValue));
            var newStatus = ReportCursorStatus();
            int width = newStatus.column;
            int height = newStatus.row;
            TextWriterRaw.WriteRaw(CsiSequences.GenerateCsiCursorPosition(oldColumn, oldRow));
            return (width, height);
        }

        internal static (int column, int row) GetPositionUsingMsysInvocation()
        {
            string shellPath = "/usr/bin/bash.exe";
            int exitCode = -1;
            FileExistsInPath("bash.exe", ref shellPath);
            string output = ExecuteProcessToString(shellPath, "-c \"read -sdR -p $'\\E[6n' CURPOS ; echo $\\\"${CURPOS#*[}\\\"\"", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ref exitCode, false);
            string[] split = output.Split(';');
            return (Convert.ToInt32(split[1]), Convert.ToInt32(split[0]));
        }
        #endregion

        static ConsolePositioning()
        {
            if (GeneralColorTools.CheckConsoleOnCall)
                ConsoleChecker.CheckConsole();
        }
    }
}
