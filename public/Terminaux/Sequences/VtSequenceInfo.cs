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
    [DebuggerDisplay("{Type} ({SpecificType}, {StartType}): {FullSequence} ({Start} -> {End})")]
    public class VtSequenceInfo
    {
        /// <summary>
        /// VT sequence type
        /// </summary>
        public VtSequenceType Type { get; }

        /// <summary>
        /// VT sequence specific type
        /// </summary>
        public VtSequenceSpecificTypes SpecificType =>
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

        private VtSequenceSpecificTypes GetSpecificType()
        {
            var specificType = (VtSequenceSpecificTypes)(-1);
            char lastChar = FullSequence[End];
            switch (Type)
            {
                case VtSequenceType.Csi:
                    if (FinalChar == '@')
                    {
                        specificType = Intermediates == " " ?
                            VtSequenceSpecificTypes.CsiShiftLeftColumns :
                            VtSequenceSpecificTypes.CsiInsertBlankCharacters;
                    }
                    else if (FinalChar == 'A')
                    {
                        specificType = Intermediates == " " ?
                            VtSequenceSpecificTypes.CsiShiftRightColumns :
                            VtSequenceSpecificTypes.CsiMoveCursorUp;
                    }
                    else if (FinalChar == 'B')
                        specificType = VtSequenceSpecificTypes.CsiMoveCursorDown;
                    else if (FinalChar == 'C')
                        specificType = VtSequenceSpecificTypes.CsiMoveCursorRight;
                    else if (FinalChar == 'D')
                        specificType = VtSequenceSpecificTypes.CsiMoveCursorLeft;
                    else if (FinalChar == 'E')
                        specificType = VtSequenceSpecificTypes.CsiMoveCursorNextLine;
                    else if (FinalChar == 'F')
                        specificType = VtSequenceSpecificTypes.CsiMoveCursorPreviousLine;
                    else if (FinalChar == 'G')
                        specificType = VtSequenceSpecificTypes.CsiCursorCharacterAbsoluteLine;
                    else if (FinalChar == 'H')
                        specificType = VtSequenceSpecificTypes.CsiCursorPosition;
                    else if (FinalChar == 'I')
                        specificType = VtSequenceSpecificTypes.CsiCursorForwardTabulation;
                    else if (FinalChar == 'J')
                    {
                        specificType = Parameters[0] == '?' ?
                            VtSequenceSpecificTypes.CsiEraseInDisplayDecsed :
                            VtSequenceSpecificTypes.CsiEraseInDisplay;
                    }
                    else if (FinalChar == 'K')
                    {
                        specificType = Parameters[0] == '?' ?
                            VtSequenceSpecificTypes.CsiEraseInLineDecsel :
                            VtSequenceSpecificTypes.CsiEraseInLine;
                    }
                    else if (FinalChar == 'L')
                        specificType = VtSequenceSpecificTypes.CsiInsertLine;
                    else if (FinalChar == 'M')
                        specificType = VtSequenceSpecificTypes.CsiDeleteLine;
                    else if (FinalChar == 'P')
                    {
                        if (Intermediates == "#")
                            specificType = Parameters.Length > 0 ?
                                VtSequenceSpecificTypes.CsiPushColorToStackWithArgs :
                                VtSequenceSpecificTypes.CsiPushColorToStack;
                        else
                            specificType = VtSequenceSpecificTypes.CsiDeleteChars;
                    }
                    else if (FinalChar == 'Q')
                    {
                        if (Intermediates == "#")
                            specificType = Parameters.Length > 0 ?
                                VtSequenceSpecificTypes.CsiPopColorFromStackWithArgs :
                                VtSequenceSpecificTypes.CsiPopColorFromStack;
                    }
                    else if (FinalChar == 'R')
                    {
                        if (Intermediates == "#")
                            specificType = VtSequenceSpecificTypes.CsiReportPaletteStack;
                    }
                    else if (FinalChar == 'S')
                    {
                        specificType = Parameters[0] == '?' ?
                            VtSequenceSpecificTypes.CsiSetGraphicsAttribute :
                            VtSequenceSpecificTypes.CsiScrollUp;
                    }
                    else if (FinalChar == 'T')
                    {
                        if (Parameters[0] == '>')
                            specificType = VtSequenceSpecificTypes.CsiResetTitleModeFeatures;
                        else
                            specificType = Parameters.Split(';').Length == 5 ?
                                VtSequenceSpecificTypes.CsiInitiateHighlightMouseTracking :
                                VtSequenceSpecificTypes.CsiScrollDown;
                    }
                    else if (FinalChar == 'X')
                        specificType = VtSequenceSpecificTypes.CsiEraseCharacters;
                    else if (FinalChar == 'Z')
                        specificType = VtSequenceSpecificTypes.CsiCursorBackwardTabulation;
                    else if (FinalChar == '^')
                        specificType = VtSequenceSpecificTypes.CsiScrollDownOriginal;
                    else if (FinalChar == '`')
                        specificType = VtSequenceSpecificTypes.CsiCursorPositionAbsolute;
                    else if (FinalChar == 'a')
                        specificType = VtSequenceSpecificTypes.CsiCursorPositionRelative;
                    else if (FinalChar == 'b')
                        specificType = VtSequenceSpecificTypes.CsiRepeatGraphicCharacter;
                    else if (FinalChar == 'c')
                    {
                        specificType = Parameters[0] == '>' ?
                            VtSequenceSpecificTypes.CsiSendDeviceAttributesTertiary :
                            Parameters[0] == '=' ?
                            VtSequenceSpecificTypes.CsiSendDeviceAttributesSecondary :
                            VtSequenceSpecificTypes.CsiSendDeviceAttributesPrimary;
                    }
                    else if (FinalChar == 'd')
                        specificType = VtSequenceSpecificTypes.CsiLinePositionAbsolute;
                    else if (FinalChar == 'e')
                        specificType = VtSequenceSpecificTypes.CsiLinePositionRelative;
                    else if (FinalChar == 'f')
                        specificType = VtSequenceSpecificTypes.CsiLeftTopPosition;
                    else if (FinalChar == 'g')
                        specificType = VtSequenceSpecificTypes.CsiTabClear;
                    else if (FinalChar == 'h')
                    {
                        specificType = Parameters[0] == '?' ?
                            VtSequenceSpecificTypes.CsiSetPrivateMode :
                            VtSequenceSpecificTypes.CsiSetMode;
                    }
                    else if (FinalChar == 'i')
                    {
                        specificType = Parameters[0] == '?' ?
                            VtSequenceSpecificTypes.CsiMediaCopyPrivate :
                            VtSequenceSpecificTypes.CsiMediaCopy;
                    }
                    else if (FinalChar == 'l')
                    {
                        specificType = Parameters[0] == '?' ?
                            VtSequenceSpecificTypes.CsiResetPrivateMode :
                            VtSequenceSpecificTypes.CsiResetMode;
                    }
                    else if (FinalChar == 'm')
                    {
                        if (Parameters.Split(';').Length == 2 && Parameters[0] == '>')
                            specificType = VtSequenceSpecificTypes.CsiSetKeyModifierOptions;
                        else
                            specificType = Parameters[0] == '>' ?
                                VtSequenceSpecificTypes.CsiResetKeyModifierOptions :
                                Parameters[0] == '?' ?
                                VtSequenceSpecificTypes.CsiQueryKeyModifierOptions :
                                VtSequenceSpecificTypes.CsiCharacterAttributes;
                    }
                    else if (FinalChar == 'n')
                    {
                        specificType = Parameters[0] == '>' ?
                            VtSequenceSpecificTypes.CsiDisableKeyModifierOptions :
                            Parameters[0] == '?' ?
                            VtSequenceSpecificTypes.CsiDeviceStatusReportDec :
                            VtSequenceSpecificTypes.CsiDeviceStatusReport;
                    }
                    else if (FinalChar == 'p')
                    {
                        if (Parameters.Length > 0 && Parameters[0] == '>')
                            specificType = VtSequenceSpecificTypes.CsiSetPointerModeXterm;
                        else if (Intermediates == "!")
                            specificType = VtSequenceSpecificTypes.CsiSoftTerminalReset;
                        else if (Intermediates == "\"" && Parameters.Split(';').Length == 2)
                            specificType = VtSequenceSpecificTypes.CsiSetConformanceLevel;
                        else if (Intermediates == "$")
                        {
                            specificType = Parameters[0] == '?' ?
                                VtSequenceSpecificTypes.CsiRequestDecPrivateMode :
                                VtSequenceSpecificTypes.CsiRequestAnsiMode;
                        }
                        else if (Intermediates == "#")
                        {
                            specificType = Parameters.Length > 0 ?
                                VtSequenceSpecificTypes.CsiPushVideoAttributesToStackWithArgs :
                                VtSequenceSpecificTypes.CsiPushVideoAttributesToStack;
                        }
                    }
                    else if (FinalChar == 'q')
                    {
                        if (Parameters.Length > 0)
                        {
                            if (Parameters[0] == '>')
                                specificType = VtSequenceSpecificTypes.CsiReportXtermVersion;
                            else if (Intermediates == "\"")
                                specificType = VtSequenceSpecificTypes.CsiSelectCharacterProtectionAttribute;
                            else if (Intermediates == " ")
                                specificType = VtSequenceSpecificTypes.CsiSetCursorStyle;
                            else
                                specificType = VtSequenceSpecificTypes.CsiLoadLeds;
                        }
                        else if (Intermediates == "#")
                            specificType = VtSequenceSpecificTypes.CsiPopVideoAttributesFromStack;
                    }
                    else if (FinalChar == 'r')
                    {
                        if (Parameters[0] == '?')
                            specificType = VtSequenceSpecificTypes.CsiRestoreDecPrivateModeValues;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiChangeAttributesInRectangularArea;
                        else
                            specificType = VtSequenceSpecificTypes.CsiSetScrollingRegion;
                    }
                    else if (FinalChar == 's')
                    {
                        if (Parameters.Length == 0)
                            specificType = VtSequenceSpecificTypes.CsiSaveCursor;
                        else
                            specificType = Parameters[0] == '?' ?
                                VtSequenceSpecificTypes.CsiDecPrivateModeValues :
                                Parameters[0] == '>' ?
                                VtSequenceSpecificTypes.CsiSetShiftEscapeOptions :
                                VtSequenceSpecificTypes.CsiSetLeftRightMargins;
                    }
                    else if (FinalChar == 't')
                    {
                        if (Parameters[0] == '>')
                            specificType = VtSequenceSpecificTypes.CsiSetTitleModeXterm;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiReverseAttributesInRectangularArea;
                        else if (Intermediates == " ")
                            specificType = VtSequenceSpecificTypes.CsiSetWarningBellVolume;
                        else
                            specificType = VtSequenceSpecificTypes.CsiWindowManipulation;
                    }
                    else if (FinalChar == 'u')
                    {
                        specificType = Intermediates == " " ?
                            VtSequenceSpecificTypes.CsiSetMarginBellVolume :
                            VtSequenceSpecificTypes.CsiRestoreCursor;
                    }
                    else if (FinalChar == 'v' && Intermediates == "$")
                        specificType = VtSequenceSpecificTypes.CsiCopyRectangularArea;
                    else if (FinalChar == 'w')
                    {
                        if (Intermediates == "'")
                            specificType = VtSequenceSpecificTypes.CsiEnableFilterRectangle;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiRequestPresentationStateReport;
                    }
                    else if (FinalChar == 'x')
                    {
                        specificType = FullSequence[End - 1] == '*' ?
                            VtSequenceSpecificTypes.CsiSelectAttributeChangeExtent :
                            FullSequence[End - 1] == '$' ?
                            VtSequenceSpecificTypes.CsiFillRectangularArea :
                            VtSequenceSpecificTypes.CsiRequestTerminalParameters;
                    }
                    else if (FinalChar == 'y')
                    {
                        if (Intermediates == "#")
                            specificType = VtSequenceSpecificTypes.CsiSelectChecksumExtension;
                        else if (Intermediates == "*")
                            specificType = VtSequenceSpecificTypes.CsiRectangularAreaChecksum;
                    }
                    else if (FinalChar == 'z')
                    {
                        if (Intermediates == "'")
                            specificType = VtSequenceSpecificTypes.CsiLocatorReporting;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiEraseRectangularArea;
                    }
                    else if (FinalChar == '{')
                    {
                        if (Intermediates == "'")
                            specificType = VtSequenceSpecificTypes.CsiSelectLocatorEvents;
                        else if (Intermediates == "#")
                            specificType = Parameters.Length > 0 ?
                                VtSequenceSpecificTypes.CsiPushVideoAttributesToStackXtermWithArgs :
                                VtSequenceSpecificTypes.CsiPushVideoAttributesToStackXterm;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiSelectiveEraseRectangularArea;
                    }
                    else if (FinalChar == '|')
                    {
                        if (Intermediates == "'")
                            specificType = VtSequenceSpecificTypes.CsiRequestLocatorPosition;
                        else if (Intermediates == "#")
                            specificType = VtSequenceSpecificTypes.CsiReportGraphicsRenditionRectangularArea;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiSelectColumnsPerPage;
                        else if (Intermediates == "*")
                            specificType = VtSequenceSpecificTypes.CsiSelectNumberOfLinesPerScreen;
                    }
                    else if (FinalChar == '}')
                    {
                        if (Intermediates == "'")
                            specificType = VtSequenceSpecificTypes.CsiInsertColumns;
                        else if (Intermediates == "#")
                            specificType = VtSequenceSpecificTypes.CsiPopVideoAttributesFromStackXterm;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiSelectActiveStatusDisplay;
                    }
                    else if (FinalChar == '~')
                    {
                        if (Intermediates == "'")
                            specificType = VtSequenceSpecificTypes.CsiDeleteColumns;
                        else if (Intermediates == "$")
                            specificType = VtSequenceSpecificTypes.CsiSelectStatusLineType;
                    }
                    break;
                case VtSequenceType.Osc:
                    if (FinalChar == VtSequenceBasicChars.BellChar)
                        specificType = VtSequenceSpecificTypes.OscOperatingSystemCommand;
                    if (FinalChar == VtSequenceBasicChars.StChar)
                        specificType = VtSequenceSpecificTypes.OscOperatingSystemCommandAlt;
                    break;
                case VtSequenceType.Esc:
                    if (FullSequence[1] == ' ')
                    {
                        // First group
                        if (lastChar == 'F')
                            specificType = VtSequenceSpecificTypes.Esc7BitControls;
                        else if (lastChar == 'G')
                            specificType = VtSequenceSpecificTypes.Esc8BitControls;
                        else if (lastChar == 'L')
                            specificType = VtSequenceSpecificTypes.EscAnsiConformanceLevel1;
                        else if (lastChar == 'M')
                            specificType = VtSequenceSpecificTypes.EscAnsiConformanceLevel2;
                        else if (lastChar == 'N')
                            specificType = VtSequenceSpecificTypes.EscAnsiConformanceLevel3;
                    }
                    else if (FullSequence[1] == '#')
                    {
                        // Second group
                        if (lastChar == '3')
                            specificType = VtSequenceSpecificTypes.EscDecDoubleHeightLineTopHalf;
                        else if (lastChar == '4')
                            specificType = VtSequenceSpecificTypes.EscDecDoubleHeightLineBottomHalf;
                        else if (lastChar == '5')
                            specificType = VtSequenceSpecificTypes.EscDecSingleWidthLine;
                        else if (lastChar == '6')
                            specificType = VtSequenceSpecificTypes.EscDecDoubleWidthLine;
                        else if (lastChar == '8')
                            specificType = VtSequenceSpecificTypes.EscDecScreenAlignmentTest;
                    }
                    else if (FullSequence[1] == '%')
                    {
                        // Second group
                        if (lastChar == '@')
                            specificType = VtSequenceSpecificTypes.EscSelectDefaultCharacterSet;
                        else if (lastChar == 'G')
                            specificType = VtSequenceSpecificTypes.EscSelectUtf8CharacterSet;
                    }
                    else if (FullSequence[1] == '(')
                        specificType = VtSequenceSpecificTypes.EscDesignateG0CharacterSet;
                    else if (FullSequence[1] == ')')
                        specificType = VtSequenceSpecificTypes.EscDesignateG1CharacterSet;
                    else if (FullSequence[1] == '*')
                        specificType = VtSequenceSpecificTypes.EscDesignateG2CharacterSet;
                    else if (FullSequence[1] == '+')
                        specificType = VtSequenceSpecificTypes.EscDesignateG3CharacterSet;
                    else if (FullSequence[1] == '-')
                        specificType = VtSequenceSpecificTypes.EscDesignateG1CharacterSetAlt;
                    else if (FullSequence[1] == ',')
                        specificType = VtSequenceSpecificTypes.EscDesignateG2CharacterSetAlt;
                    else if (FullSequence[1] == '/')
                        specificType = VtSequenceSpecificTypes.EscDesignateG3CharacterSetAlt;
                    else if (FullSequence[1] == '6')
                        specificType = VtSequenceSpecificTypes.EscBackIndex;
                    else if (FullSequence[1] == '7')
                        specificType = VtSequenceSpecificTypes.EscSaveCursor;
                    else if (FullSequence[1] == '8')
                        specificType = VtSequenceSpecificTypes.EscRestoreCursor;
                    else if (FullSequence[1] == '9')
                        specificType = VtSequenceSpecificTypes.EscForwardIndex;
                    else if (FullSequence[1] == '=')
                        specificType = VtSequenceSpecificTypes.EscApplicationKeypad;
                    else if (FullSequence[1] == '>')
                        specificType = VtSequenceSpecificTypes.EscNormalKeypad;
                    else if (FullSequence[1] == 'F')
                        specificType = VtSequenceSpecificTypes.EscCursorToLowerLeftCorner;
                    else if (FullSequence[1] == 'c')
                        specificType = VtSequenceSpecificTypes.EscFullReset;
                    else if (FullSequence[1] == 'l')
                        specificType = VtSequenceSpecificTypes.EscMemoryLock;
                    else if (FullSequence[1] == 'm')
                        specificType = VtSequenceSpecificTypes.EscMemoryUnlock;
                    else if (FullSequence[1] == 'n')
                        specificType = VtSequenceSpecificTypes.EscInvokeG2CharacterSetGl;
                    else if (FullSequence[1] == 'o')
                        specificType = VtSequenceSpecificTypes.EscInvokeG3CharacterSetGl;
                    else if (FullSequence[1] == '|')
                        specificType = VtSequenceSpecificTypes.EscInvokeG3CharacterSetGr;
                    else if (FullSequence[1] == '}')
                        specificType = VtSequenceSpecificTypes.EscInvokeG2CharacterSetGr;
                    else if (FullSequence[1] == '~')
                        specificType = VtSequenceSpecificTypes.EscInvokeG1CharacterSetGr;
                    break;
                case VtSequenceType.Apc:
                    if (FinalChar == VtSequenceBasicChars.StChar)
                        specificType = VtSequenceSpecificTypes.ApcApplicationProgramCommand;
                    break;
                case VtSequenceType.Dcs:
                    if (FinalChar == VtSequenceBasicChars.StChar)
                    {
                        if (Parameters.StartsWith("$q"))
                            specificType = VtSequenceSpecificTypes.DcsRequestStatusString;
                        else if (Parameters.StartsWith("+Q"))
                            specificType = VtSequenceSpecificTypes.DcsRequestResourceValues;
                        else if (Parameters.StartsWith("+p"))
                            specificType = VtSequenceSpecificTypes.DcsSetTermInfoData;
                        else if (Parameters.StartsWith("+q"))
                            specificType = VtSequenceSpecificTypes.DcsRequestTermInfoData;
                        else
                        {
                            // Check to see if we have the restore presentation status indicator after the number
                            for (int i = 0; i <= Parameters.Length; i++)
                            {
                                char character = Parameters[i];
                                if (char.IsNumber(character))
                                    continue;
                                if (character == '$' && Parameters[i + 1] == 't')
                                {
                                    specificType = VtSequenceSpecificTypes.DcsRestorePresentationStatus;
                                    break;
                                }
                                else if (character == ';')
                                {
                                    specificType = VtSequenceSpecificTypes.DcsUserDefinedKeys;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case VtSequenceType.Pm:
                    if (FinalChar == VtSequenceBasicChars.StChar)
                        specificType = VtSequenceSpecificTypes.PmPrivacyMessage;
                    break;
                case VtSequenceType.C1:
                    if (lastChar == 'D')
                        specificType = VtSequenceSpecificTypes.C1Index;
                    else if (lastChar == 'E')
                        specificType = VtSequenceSpecificTypes.C1NextLine;
                    else if (lastChar == 'H')
                        specificType = VtSequenceSpecificTypes.C1TabSet;
                    else if (lastChar == 'M')
                        specificType = VtSequenceSpecificTypes.C1ReverseIndex;
                    else if (lastChar == 'N')
                        specificType = VtSequenceSpecificTypes.C1SingleShiftSelectG2CharacterSet;
                    else if (lastChar == 'O')
                        specificType = VtSequenceSpecificTypes.C1SingleShiftSelectG3CharacterSet;
                    else if (lastChar == 'P')
                        specificType = VtSequenceSpecificTypes.C1DeviceControlString;
                    else if (lastChar == 'V')
                        specificType = VtSequenceSpecificTypes.C1StartOfGuardedArea;
                    else if (lastChar == 'W')
                        specificType = VtSequenceSpecificTypes.C1EndOfGuardedArea;
                    else if (lastChar == 'X')
                        specificType = VtSequenceSpecificTypes.C1StartOfString;
                    else if (lastChar == 'Z')
                        specificType = VtSequenceSpecificTypes.C1ReturnTerminalId;
                    else if (lastChar == VtSequenceBasicChars.CsiPrefixChar)
                        specificType = VtSequenceSpecificTypes.C1ControlSequenceIndicator;
                    else if (lastChar == '\\')
                        specificType = VtSequenceSpecificTypes.C1StringTerminator;
                    else if (lastChar == VtSequenceBasicChars.OSCPrefixChar)
                        specificType = VtSequenceSpecificTypes.C1OperatingSystemCommand;
                    else if (lastChar == VtSequenceBasicChars.PMPrefixChar)
                        specificType = VtSequenceSpecificTypes.C1PrivacyMessage;
                    else if (lastChar == VtSequenceBasicChars.APCPrefixChar)
                        specificType = VtSequenceSpecificTypes.C1ApplicationProgramCommand;
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
