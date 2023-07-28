/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Sequences.Tools;
using Terminaux.Tools;

namespace Terminaux.Reader.Bindings
{
    /// <summary>
    /// Base key binding
    /// </summary>
    public abstract class BaseBinding : IBinding
    {
        /// <summary>
        /// Key to bind to
        /// </summary>
        public virtual ConsoleKeyInfo[] BoundKeys { get; }

        /// <summary>
        /// Does this binding cause the input to exit?
        /// </summary>
        public virtual bool IsExit { get; }

        /// <summary>
        /// Whether the binding matched
        /// </summary>
        /// <param name="input">Input key</param>
        public virtual bool BindMatched(ConsoleKeyInfo input)
        {
            bool match = false;
            foreach (var key in BoundKeys)
            {
                match = input.Key == key.Key &&
                        input.KeyChar == key.KeyChar &&
                        input.Modifiers == key.Modifiers;
                if (match)
                    break;
            }
            return match;
        }

        /// <summary>
        /// Do the action
        /// </summary>
        /// <param name="state">State of the reader</param>
        public virtual void DoAction(TermReaderState state)
        {
            // Insert the character, but in the condition that it's not a control character
            if (char.IsControl(state.pressedKey.KeyChar))
                return;
            state.CurrentText.Insert(state.CurrentTextPos, state.pressedKey.KeyChar);

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? new string(TermReaderSettings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

            // In the case of one line wrap, get the list of sentences
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleWrapperTools.ActionWindowWidth() - TermReaderSettings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrapperTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length));
                PositioningTools.GoForwardOneLineWrapAware(1, ref state, incompleteSentences);
            }
            else
            {
                ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrapperTools.ActionWriteString(renderedText);
                PositioningTools.GoForward(1, true, ref state);
            }
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }

        internal static string GetOneLineWrappedSentenceToRender(string[] incompleteSentences, TermReaderState state) =>
            GetOneLineWrappedSentenceToRender(incompleteSentences, state.CurrentTextPos);

        internal static string GetOneLineWrappedSentenceToRender(string[] incompleteSentences, int targetIndex)
        {
            string finalRenderedString = "";

            // Deal with trying to count the characters incrementally for each incomplete sentence until we find an index
            // that we want, then give the rendered string back.
            int currentIndex = 0;
            foreach (string sentence in incompleteSentences)
            {
                finalRenderedString = sentence;
                for (int i = 0; i < sentence.Length && currentIndex != targetIndex; i++)
                    currentIndex++;
                if (currentIndex == targetIndex)
                    break;
            }

            // Return it!
            return finalRenderedString;
        }

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        /// <param name="indentLength">Indentation length</param>
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

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        indentLength = 0;
                        vtSeqCompensate = 0;
                    }
                }
            }

            return IncompleteSentences.ToArray();
        }
    }
}
