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

using System.Diagnostics;
using Terminaux.Sequences.Builder;

namespace Terminaux.Sequences
{
    /// <summary>
    /// VT sequence token info
    /// </summary>
    [DebuggerDisplay("{Type} ({StartType}): {FullSequence} ({Start} -> {End})")]
    public class VtSequenceInfo
    {
        /// <summary>
        /// VT sequence type
        /// </summary>
        public VtSequenceType Type { get; }

        /// <summary>
        /// VT sequence specific type
        /// </summary>
        public VtSequenceSpecificType SpecificType =>
            GetSpecificType();

        /// <summary>
        /// VT sequence start type
        /// </summary>
        public VtSequenceStartType StartType { get; }

        /// <summary>
        /// Prefix of the sequence
        /// </summary>
        public string Prefix { get; } = string.Empty;

        /// <summary>
        /// Parameters of the sequence
        /// </summary>
        public string Parameters { get; } = string.Empty;

        /// <summary>
        /// Intermediates of the sequence
        /// </summary>
        public string Intermediates { get; } = string.Empty;

        /// <summary>
        /// Full sequence
        /// </summary>
        public string FullSequence { get; } = string.Empty;

        /// <summary>
        /// Final char
        /// </summary>
        public char FinalChar { get; }

        /// <summary>
        /// Start index within a sequence of text
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// End index within a sequence of text
        /// </summary>
        public int End =>
            Start + FullSequence.Length - 1;

        private VtSequenceSpecificType GetSpecificType()
        {
            var specificType = (VtSequenceSpecificType)(-1);
            switch (Type)
            {
                case VtSequenceType.Csi:
                    // TODO: Populate this
                    break;
                case VtSequenceType.Osc:
                    if (FinalChar == VtSequenceBasicChars.BellChar)
                        specificType = VtSequenceSpecificType.OscOperatingSystemCommand;
                    if (FinalChar == VtSequenceBasicChars.StChar)
                        specificType = VtSequenceSpecificType.OscOperatingSystemCommandAlt;
                    break;
                case VtSequenceType.Esc:
                    // TODO: Populate this
                    break;
                case VtSequenceType.Apc:
                    if (FinalChar == VtSequenceBasicChars.StChar)
                        specificType = VtSequenceSpecificType.ApcApplicationProgramCommand;
                    break;
                case VtSequenceType.Dcs:
                    // TODO: Populate this
                    break;
                case VtSequenceType.Pm:
                    if (FinalChar == VtSequenceBasicChars.StChar)
                        specificType = VtSequenceSpecificType.PmPrivacyMessage;
                    break;
                case VtSequenceType.C1:
                    // TODO: Populate this
                    break;
            }
            return specificType;
        }

        internal VtSequenceInfo()
        { }

        internal VtSequenceInfo(VtSequenceType type, VtSequenceStartType startType, string prefix, string parameters, string intermediates, string fullSequence, char finalChar, int start)
        {
            Type = type;
            StartType = startType;
            Prefix = prefix;
            Parameters = parameters;
            Intermediates = intermediates;
            FullSequence = fullSequence;
            FinalChar = finalChar;
            Start = start;
        }
    }
}
