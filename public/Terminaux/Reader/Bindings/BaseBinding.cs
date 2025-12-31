//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Reader.Tools;
using Textify.General;

namespace Terminaux.Reader.Bindings
{
    /// <summary>
    /// Base key binding
    /// </summary>
    public abstract class BaseBinding : IBinding
    {
        private readonly List<ConsoleKeyInfo> customKeys = [];

        /// <summary>
        /// Key to bind this keybinding to
        /// </summary>
        public ConsoleKeyInfo[] BoundKeys { get; internal set; } = [];

        /// <summary>
        /// Custom keys to bind this keybinding to
        /// </summary>
        public List<ConsoleKeyInfo>? CustomKeys =>
            IsBindingOverridable ? customKeys : null;

        /// <summary>
        /// Resets the suggestions text position
        /// </summary>
        public virtual bool ResetSuggestionsTextPos { get; } = true;

        /// <summary>
        /// Whether this binding adds manipulated to the changes list or not. If there is no difference after executing
        /// this binding, even if this is set to true, the new input text won't be appended.
        /// </summary>
        public virtual bool AppendsChangesList { get; } = true;

        /// <summary>
        /// Whether this binding uses the argument number for repetition or for other action.
        /// </summary>
        public virtual bool ArgumentNumberIsRepetition { get; } = true;

        /// <summary>
        /// Does this binding cause the input to exit?
        /// </summary>
        public virtual bool IsExit { get; }

        /// <summary>
        /// Can this binding be overridden?
        /// </summary>
        public virtual bool IsBindingOverridable =>
            false;

        /// <summary>
        /// Whether the binding matched
        /// </summary>
        /// <param name="input">Input key</param>
        public virtual bool BindMatched(ConsoleKeyInfo input) =>
            IsBindingOverridable && CustomKeys?.Count > 0 ? OverridenBindMatched(input) : BaseBindMatched(input);

        /// <summary>
        /// Do the action
        /// </summary>
        /// <param name="state">State of the reader</param>
        public virtual void DoAction(TermReaderState state)
        {
            static string RenderChar(char c) =>
                $"{c}".ReplaceAllRange(
                    // To be replaced
                    [
                        "\t",
                    ],

                    // Replacements
                    [
                        new string(' ', ConsoleMisc.TabWidth),
                    ]
                );

            // Insert the character, but in the condition that it's not a control character
            if (!ConditionalTools.ShouldNot(char.IsControl(state.pressedKey.KeyChar) && state.pressedKey.KeyChar != '\t', state))
            {
                Input.InvalidateInput();
                return;
            }

            // Process the text, replace below characters, and determine if this character is unprintable or not
            var textBuilder = new StringBuilder(RenderChar(state.pressedKey.KeyChar));

            // Capture all the possible input, as long as that text is printable
            while (ConsoleWrapper.KeyAvailable)
            {
                var pressed = Input.ReadKey();
                bool isHighSurrogate = char.IsHighSurrogate(state.pressedKey.KeyChar);
                if (!ConditionalTools.ShouldNot(TextTools.GetCharWidth(state.pressedKey.KeyChar) == 0 && !isHighSurrogate, state))
                {
                    Input.InvalidateInput();
                    return;
                }

                // Check to see if this character is a surrogate (i.e. trying to insert emoji)
                if (isHighSurrogate)
                {
                    // Get all the input, or discard the surrogate because it's a zero width character
                    bool isNextKeySurrogate = char.IsLowSurrogate(pressed.KeyChar);
                    if (!ConditionalTools.ShouldNot(TextTools.GetCharWidth(pressed.KeyChar) == 0 && !isNextKeySurrogate, state))
                    {
                        Input.InvalidateInput();
                        return;
                    }
                }

                // Our next key is a letter.
                textBuilder.Append(RenderChar(pressed.KeyChar));
            }

            // Indicate whether we're replacing or inserting
            string text = textBuilder.ToString();
            if (state.insertIsReplace)
            {
                if (state.CurrentTextPos == state.CurrentText.Length)
                    TermReaderTools.InsertNewText(text);
                else
                {
                    state.CurrentText.Remove(state.CurrentTextPos, 1);
                    state.CurrentText.Insert(state.CurrentTextPos, text);
                    TermReaderTools.RefreshPrompt(ref state, 1);
                }
            }
            else
                TermReaderTools.InsertNewText(text);
        }

        internal bool BaseBindMatched(ConsoleKeyInfo input)
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

        internal bool OverridenBindMatched(ConsoleKeyInfo input)
        {
            bool match = false;
            foreach (var key in CustomKeys ?? [])
            {
                match = input.Key == key.Key &&
                        input.KeyChar == key.KeyChar &&
                        input.Modifiers == key.Modifiers;
                if (match)
                    break;
            }
            return match;
        }
    }
}
