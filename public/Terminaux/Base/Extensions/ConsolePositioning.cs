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
using System.Text.RegularExpressions;
using Textify.General;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Sequences;

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
                            int cells = ConsoleChar.EstimateCellWidth(text, j);
                            if (j + 1 < text.Length && char.IsSurrogatePair(text[j], text[j + 1]))
                                i++;
                            LeftSeekPosition += cells;
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
                        LeftSeekPosition = ConsoleChar.EstimateCellWidth(texts[texts.Length - 1]);
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

        internal static string BufferChar(string text, (VtSequenceType type, Match[] sequences)[] sequencesCollections, ref int i, ref int vtSeqIdx, out bool isVtSequence)
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
            foreach ((var _, var sequences) in sequencesCollections)
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
            return
                !string.IsNullOrEmpty(seq) ? seq :
                ch == '\t' ? new string(' ', ConsoleMisc.TabWidth) :
                ch.ToString();
        }

        static ConsolePositioning()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
