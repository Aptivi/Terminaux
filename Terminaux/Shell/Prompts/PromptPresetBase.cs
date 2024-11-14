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

using Nitrocid.Kernel.Debugging;

namespace Terminaux.Shell.Prompts
{
    /// <summary>
    /// Base prompt preset
    /// </summary>
    public class PromptPresetBase : IPromptPreset
    {

        /// <inheritdoc/>
        public virtual string PresetName { get; } = "BasePreset";

        /// <inheritdoc/>
        public virtual string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public virtual string PresetPromptCompletion =>
            PresetPromptCompletionBuilder();

        /// <inheritdoc/>
        public virtual string PresetPromptShowcase =>
            PresetPromptBuilderShowcase();

        /// <inheritdoc/>
        public virtual string PresetPromptCompletionShowcase =>
            PresetPromptCompletionBuilderShowcase();

        /// <inheritdoc/>
        public virtual string PresetShellType { get; } = "Shell";

        internal virtual string PresetPromptBuilder()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Tried to call prompt builder on base.");
            return "> ";
        }

        string IPromptPreset.PresetPromptBuilder() =>
            PresetPromptBuilder();

        internal virtual string PresetPromptCompletionBuilder()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Tried to call prompt builder on base.");
            return "[+] > ";
        }

        string IPromptPreset.PresetPromptCompletionBuilder() =>
            PresetPromptCompletionBuilder();

        internal virtual string PresetPromptBuilderShowcase()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Tried to call prompt builder on base.");
            return "> ";
        }

        string IPromptPreset.PresetPromptBuilderShowcase() =>
            PresetPromptBuilderShowcase();

        internal virtual string PresetPromptCompletionBuilderShowcase()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Tried to call prompt builder on base.");
            return "[+] > ";
        }

        string IPromptPreset.PresetPromptCompletionBuilderShowcase() =>
            PresetPromptCompletionBuilderShowcase();

    }
}
