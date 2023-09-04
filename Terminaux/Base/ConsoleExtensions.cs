
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using Terminaux.Sequences.Builder;
using Terminaux.Sequences.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base
{
    /// <summary>
    /// Additional routines for the console
    /// </summary>
    public static class ConsoleExtensions
    {

        /// <summary>
        /// Clears the console buffer, but keeps the current cursor position
        /// </summary>
        public static void ClearKeepPosition()
        {
            int Left = Console.CursorLeft;
            int Top = Console.CursorTop;
            Console.Clear();
            Console.SetCursorPosition(Left, Top);
        }

        /// <summary>
        /// Clears the line to the right
        /// </summary>
        public static void ClearLineToRight() => Console.Write(Convert.ToString(VtSequenceBasicChars.EscapeChar) + "[0K");

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset) => (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * ((Console.WindowWidth - WidthOffset) * 0.01d));

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="TargetWidth">The target width</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeatTargeted(int CurrentNumber, int MaximumNumber, int TargetWidth) => (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * (TargetWidth * 0.01d));

        /// <summary>
        /// Filters the VT sequences that matches the regex
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text)
        {
            // Filter all sequences
            Text = VtSequenceTools.FilterVTSequences(Text);
            return Text;
        }

        /// <summary>
        /// Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public static (int, int) GetFilteredPositions(string Text, params object[] Vars)
        {
            // Filter all text from the VT escape sequences
            Text = FilterVTSequences(Text);

            // Seek through filtered text (make it seem like it came from Linux by removing CR (\r)), return to the old position, and return the filtered positions
            Text = FormatString(Text, Vars);
            Text = Text.Replace(Convert.ToString(Convert.ToChar(13)), "");
            Text = Text.Replace(Convert.ToString(Convert.ToChar(0)), "");
            int LeftSeekPosition = Console.CursorLeft;
            int TopSeekPosition = Console.CursorTop;
            for (int i = 1; i <= Text.Length; i++)
            {
                // If we spotted a new line character, get down by one line.
                if (Text[i - 1] == Convert.ToChar(10))
                {
                    if (TopSeekPosition < Console.BufferHeight - 1)
                        TopSeekPosition += 1;
                    LeftSeekPosition = 0;
                }
                else
                {
                    // Simulate seeking through text
                    LeftSeekPosition += 1;
                    if (LeftSeekPosition >= Console.WindowWidth)
                    {
                        // We've reached end of line
                        LeftSeekPosition = 0;

                        // Get down by one line
                        TopSeekPosition += 1;
                        if (TopSeekPosition > Console.BufferHeight - 1)
                        {
                            // We're at the end of buffer! Decrement by one.
                            TopSeekPosition -= 1;
                        }
                    }
                }
            }

            // Return the filtered positions
            return (LeftSeekPosition, TopSeekPosition);
        }

        /// <summary>
        /// Sets the console title
        /// </summary>
        /// <param name="Text">The text to be set</param>
        public static void SetTitle(string Text)
        {
            char BellChar = Convert.ToChar(7);
            char EscapeChar = Convert.ToChar(27);
            string Sequence = $"{EscapeChar}]0;{Text}{BellChar}";
            TextWriterColor.WritePlain(Sequence, false);
        }

        /// <summary>
        /// Formats the string
        /// </summary>
        /// <param name="Format">The string to format</param>
        /// <param name="Vars">The variables used</param>
        /// <returns>A formatted string if successful, or the unformatted one if failed.</returns>
        public static string FormatString(string Format, params object[] Vars)
        {
            string FormattedString = Format;
            try
            {
                if (Vars.Length > 0)
                    FormattedString = string.Format(Format, Vars);
            }
            catch
            {
                return Format;
            }
            return FormattedString;
        }

        #region Windows-specific
        private const string winKernel = "kernel32.dll";

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern IntPtr GetStdHandle(int handle);

        internal static bool InitializeSequences()
        {
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
        internal static IEnumerable<int> AllIndexesOf(this string target, string value)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Empty string specified", nameof(value));
            int index = 0;
            while (true)
            {
                index = target.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
                index += value.Length;
            }
        }

        internal static string ReplaceLastOccurrence(this string source, string searchText, string replace)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (searchText is null)
                throw new ArgumentNullException(nameof(searchText));
            int position = source.LastIndexOf(searchText);
            if (position == -1)
                return source;
            string result = source.Remove(position, searchText.Length).Insert(position, replace);
            return result;
        }

        internal static string NeutralizePath(string Path, string Source, bool Strict = false)
        {
            Path ??= "";
            Source ??= "";

            // Unescape the characters
            Path = Regex.Unescape(Path.Replace(@"\", "/"));
            Source = Regex.Unescape(Source.Replace(@"\", "/"));

            // Append current directory to path
            if (ConsolePlatform.IsOnWindows() & !Path.Contains(":/") | ConsolePlatform.IsOnUnix() & !Path.StartsWith("/"))
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
                    throw new Exception($"Neutralized a non-existent path. {Path}");
            else
                return Path;
        }

        internal static string PathLookupDelimiter =>
            Convert.ToString(Path.PathSeparator);

        internal static string PathsToLookup =>
            Environment.GetEnvironmentVariable("PATH");

        internal static List<string> GetPathList() =>
            PathsToLookup.Split(Convert.ToChar(PathLookupDelimiter)).ToList();

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

        internal static string Truncate(this string target, int threshold)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));

            // Try to truncate string. If the string length is bigger than the threshold, it'll be truncated to the length of
            // the threshold, putting three dots next to it. We don't use ellipsis marks here because we're dealing with the
            // terminal, and some terminals and some monospace fonts may not support that character, so we mimick it by putting
            // the three dots.
            if (target.Length > threshold)
                return target.Substring(0, threshold - 1) + "...";
            else
                return target;
        }

        internal static string BufferChar(string text, MatchCollection[] sequencesCollections, ref int i, ref int vtSeqIdx)
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
            foreach (var sequences in sequencesCollections)
            {
                if (sequences.Count > 0 && sequences[vtSeqIdx].Index == i)
                {
                    // We're at an index which is the same as the captured VT sequence. Get the sequence
                    seq = sequences[vtSeqIdx].Value;

                    // Raise the index in case we have the next sequence, but only if we're sure that we have another
                    if (vtSeqIdx + 1 < sequences.Count)
                        vtSeqIdx++;

                    // Raise the paragraph index by the length of the sequence
                    i += seq.Length - 1;
                }
            }
            return !string.IsNullOrEmpty(seq) ? seq : ch.ToString();
        }

        internal static string[] SplitNewLines(this string target) =>
            target.Replace(Convert.ToChar(13).ToString(), "")
               .Split(Convert.ToChar(10));

        internal static string[] GetWrappedSentences(string text, int maximumLength) =>
            GetWrappedSentences(text, maximumLength, 0);

        internal static string[] GetWrappedSentences(string text, int maximumLength, int indentLength)
        {
            if (string.IsNullOrEmpty(text))
                return new string[] { "" };

            // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
            // sizes.
            var IncompleteSentences = new List<string>();
            var IncompleteSentenceBuilder = new StringBuilder();

            // Make the text look like it came from Linux
            text = text.Replace(Convert.ToString(Convert.ToChar(13)), "");

            // This indent length count tells us how many spaces are used for indenting the paragraph. This is only set for
            // the first time and will be reverted back to zero after the incomplete sentence is formed.
            var sequencesCollections = VtSequenceTools.MatchVTSequences(text);
            foreach (var sequences in sequencesCollections)
            {
                int vtSeqIdx = 0;
                int vtSeqCompensate = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    // Check the character to see if we're at the VT sequence
                    bool explicitNewLine = text[text.Length - 1] == '\n';
                    char ParagraphChar = text[i];
                    bool isNewLine = text[i] == '\n';
                    string seq = "";
                    if (sequences.Count > 0 && sequences[vtSeqIdx].Index == i)
                    {
                        // We're at an index which is the same as the captured VT sequence. Get the sequence
                        seq = sequences[vtSeqIdx].Value;

                        // Raise the index in case we have the next sequence, but only if we're sure that we have another
                        if (vtSeqIdx + 1 < sequences.Count)
                            vtSeqIdx++;

                        // Raise the paragraph index by the length of the sequence
                        i += seq.Length - 1;
                        vtSeqCompensate += seq.Length;
                    }

                    // Append the character into the incomplete sentence builder.
                    if (!isNewLine)
                        IncompleteSentenceBuilder.Append(!string.IsNullOrEmpty(seq) ? seq : ParagraphChar.ToString());

                    // Also, compensate the \0 characters
                    if (text[i] == '\0')
                        vtSeqCompensate++;

                    // Check to see if we're at the maximum character number or at the new line
                    if (IncompleteSentenceBuilder.Length == maximumLength - indentLength + vtSeqCompensate |
                        i == text.Length - 1 |
                        isNewLine)
                    {
                        // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                        IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());
                        if (explicitNewLine)
                            IncompleteSentences.Add("");

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        indentLength = 0;
                        vtSeqCompensate = 0;
                    }
                }
            }

            return IncompleteSentences.ToArray();
        }
        #endregion

    }
}
