
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
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Sequences.Tools;

namespace Terminaux.Sequences.Builder
{
    /// <summary>
    /// VT sequence builder tools
    /// </summary>
    public static class VtSequenceBuilderTools
    {
        private static readonly Regex getTypeRegex = new("(?<=[a-z0-9])[A-Z].*", RegexOptions.Compiled);
        private static readonly Dictionary<VtSequenceSpecificTypes, (Delegate generator, Regex matchRegex, int argumentsRequired)> sequenceBuilders = new()
        {
            // APC sequences
            { VtSequenceSpecificTypes.ApcApplicationProgramCommand,
                (new Func<string, string>(ApcSequences.GenerateApcApplicationProgramCommand), ApcSequences.ApcApplicationProgramCommandSequenceRegex, 1) },

            // CSI sequences
            { VtSequenceSpecificTypes.CsiChangeAttributesInRectangularArea,
                (new Func<string, int, int, int, int, string>(CsiSequences.GenerateCsiChangeAttributesInRectangularArea), CsiSequences.CsiChangeAttributesInRectangularAreaSequenceRegex, 5) },
            { VtSequenceSpecificTypes.CsiCharacterAttributes,
                (new Func<string, string>(CsiSequences.GenerateCsiCharacterAttributes), CsiSequences.CsiCharacterAttributesSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiCopyRectangularArea,
                (new Func<int, int, int, int, int, int, int, int, string>(CsiSequences.GenerateCsiCopyRectangularArea), CsiSequences.CsiCopyRectangularAreaSequenceRegex, 8) },
            { VtSequenceSpecificTypes.CsiCursorBackwardTabulation,
                (new Func<int, string>(CsiSequences.GenerateCsiCursorBackwardTabulation), CsiSequences.CsiCursorBackwardTabulationSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiCursorCharacterAbsoluteLine,
                (new Func<int, string>(CsiSequences.GenerateCsiCursorCharacterAbsoluteLine), CsiSequences.CsiCursorCharacterAbsoluteLineSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiCursorForwardTabulation,
                (new Func<int, string>(CsiSequences.GenerateCsiCursorForwardTabulation), CsiSequences.CsiCursorForwardTabulationSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiCursorPosition,
                (new Func<int, int, string>(CsiSequences.GenerateCsiCursorPosition), CsiSequences.CsiCursorPositionSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiCursorPositionAbsolute,
                (new Func<int, string>(CsiSequences.GenerateCsiCursorPositionAbsolute), CsiSequences.CsiCursorPositionAbsoluteSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiCursorPositionRelative,
                (new Func<int, string>(CsiSequences.GenerateCsiCursorPositionRelative), CsiSequences.CsiCursorPositionRelativeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiDecPrivateModeValues,
                (new Func<string, string>(CsiSequences.GenerateCsiDecPrivateModeValues), CsiSequences.CsiDecPrivateModeValuesSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiDeleteChars,
                (new Func<int, string>(CsiSequences.GenerateCsiDeleteChars), CsiSequences.CsiDeleteCharsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiDeleteColumns,
                (new Func<int, string>(CsiSequences.GenerateCsiDeleteColumns), CsiSequences.CsiDeleteColumnsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiDeleteLine,
                (new Func<int, string>(CsiSequences.GenerateCsiDeleteLine), CsiSequences.CsiDeleteLineSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiDeviceStatusReport,
                (new Func<int, string>(CsiSequences.GenerateCsiDeviceStatusReport), CsiSequences.CsiDeviceStatusReportSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiDeviceStatusReportDec,
                (new Func<int, string>(CsiSequences.GenerateCsiDeviceStatusReportDec), CsiSequences.CsiDeviceStatusReportDecSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiDisableKeyModifierOptions,
                (new Func<int, string>(CsiSequences.GenerateCsiDisableKeyModifierOptions), CsiSequences.CsiDisableKeyModifierOptionsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiEnableFilterRectangle,
                (new Func<int, int, int, int, string>(CsiSequences.GenerateCsiEnableFilterRectangle), CsiSequences.CsiEnableFilterRectangleSequenceRegex, 4) },
            { VtSequenceSpecificTypes.CsiEraseCharacters,
                (new Func<int, string>(CsiSequences.GenerateCsiEraseCharacters), CsiSequences.CsiEraseCharactersSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiEraseInDisplay,
                (new Func<int, string>(CsiSequences.GenerateCsiEraseInDisplay), CsiSequences.CsiEraseInDisplaySequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiEraseInDisplayDecsed,
                (new Func<int, string>(CsiSequences.GenerateCsiEraseInDisplayDecsed), CsiSequences.CsiEraseInDisplayDecsedSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiEraseInLine,
                (new Func<int, string>(CsiSequences.GenerateCsiEraseInLine), CsiSequences.CsiEraseInLineSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiEraseInLineDecsel,
                (new Func<int, string>(CsiSequences.GenerateCsiEraseInLineDecsel), CsiSequences.CsiEraseInLineDecselSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiEraseRectangularArea,
                (new Func<int, int, int, int, string>(CsiSequences.GenerateCsiEraseRectangularArea), CsiSequences.CsiEraseRectangularAreaSequenceRegex, 4) },
            { VtSequenceSpecificTypes.CsiFillRectangularArea,
                (new Func<char, int, int, int, int, string>(CsiSequences.GenerateCsiFillRectangularArea), CsiSequences.CsiFillRectangularAreaSequenceRegex, 5) },
            { VtSequenceSpecificTypes.CsiInitiateHighlightMouseTracking,
                (new Func<int, int, int, int, int, string>(CsiSequences.GenerateCsiInitiateHighlightMouseTracking), CsiSequences.CsiInitiateHighlightMouseTrackingSequenceRegex, 5) },
            { VtSequenceSpecificTypes.CsiInsertBlankCharacters,
                (new Func<int, string>(CsiSequences.GenerateCsiInsertBlankCharacters), CsiSequences.CsiInsertBlankCharactersSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiInsertColumns,
                (new Func<int, string>(CsiSequences.GenerateCsiInsertColumns), CsiSequences.CsiInsertColumnsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiInsertLine,
                (new Func<int, string>(CsiSequences.GenerateCsiInsertLine), CsiSequences.CsiInsertLineSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiLeftTopPosition,
                (new Func<int, int, string>(CsiSequences.GenerateCsiLeftTopPosition), CsiSequences.CsiLeftTopPositionSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiLinePositionAbsolute,
                (new Func<int, string>(CsiSequences.GenerateCsiLinePositionAbsolute), CsiSequences.CsiLinePositionAbsoluteSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiLinePositionRelative,
                (new Func<int, string>(CsiSequences.GenerateCsiLinePositionRelative), CsiSequences.CsiLinePositionRelativeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiLoadLeds,
                (new Func<int, string>(CsiSequences.GenerateCsiLoadLeds), CsiSequences.CsiLoadLedsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiLocatorReporting,
                (new Func<int, int, string>(CsiSequences.GenerateCsiLocatorReporting), CsiSequences.CsiLocatorReportingSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiMediaCopy,
                (new Func<int, string>(CsiSequences.GenerateCsiMediaCopy), CsiSequences.CsiMediaCopySequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiMediaCopyPrivate,
                (new Func<int, string>(CsiSequences.GenerateCsiMediaCopyPrivate), CsiSequences.CsiMediaCopyPrivateSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiMoveCursorDown,
                (new Func<int, string>(CsiSequences.GenerateCsiMoveCursorDown), CsiSequences.CsiMoveCursorDownSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiMoveCursorLeft,
                (new Func<int, string>(CsiSequences.GenerateCsiMoveCursorLeft), CsiSequences.CsiMoveCursorLeftSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiMoveCursorNextLine,
                (new Func<int, string>(CsiSequences.GenerateCsiMoveCursorNextLine), CsiSequences.CsiMoveCursorNextLineSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiMoveCursorPreviousLine,
                (new Func<int, string>(CsiSequences.GenerateCsiMoveCursorPreviousLine), CsiSequences.CsiMoveCursorPreviousLineSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiMoveCursorRight,
                (new Func<int, string>(CsiSequences.GenerateCsiMoveCursorRight), CsiSequences.CsiMoveCursorRightSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiMoveCursorUp,
                (new Func<int, string>(CsiSequences.GenerateCsiMoveCursorUp), CsiSequences.CsiMoveCursorUpSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiPopColorFromStack,
                (new Func<string>(CsiSequences.GenerateCsiPopColorFromStack), CsiSequences.CsiPopColorFromStackSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiPopColorFromStackWithArgs,
                (new Func<string, string>(CsiSequences.GenerateCsiPopColorFromStack), CsiSequences.CsiPopColorFromStackWithArgsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiPopVideoAttributesFromStack,
                (new Func<string>(CsiSequences.GenerateCsiPopVideoAttributesFromStack), CsiSequences.CsiPopVideoAttributesFromStackSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiPopVideoAttributesFromStackXterm,
                (new Func<string>(CsiSequences.GenerateCsiPopVideoAttributesFromStackXterm), CsiSequences.CsiPopVideoAttributesFromStackXtermSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiPushColorToStack,
                (new Func<string>(CsiSequences.GenerateCsiPushColorToStack), CsiSequences.CsiPushColorToStackSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiPushColorToStackWithArgs,
                (new Func<string, string>(CsiSequences.GenerateCsiPushColorToStack), CsiSequences.CsiPushColorToStackWithArgsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiPushVideoAttributesToStack,
                (new Func<string>(CsiSequences.GenerateCsiPushVideoAttributesToStack), CsiSequences.CsiPushVideoAttributesToStackSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiPushVideoAttributesToStackWithArgs,
                (new Func<string, string>(CsiSequences.GenerateCsiPushVideoAttributesToStack), CsiSequences.CsiPushVideoAttributesToStackWithArgsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiPushVideoAttributesToStackXterm,
                (new Func<string>(CsiSequences.GenerateCsiPushVideoAttributesToStackXterm), CsiSequences.CsiPushVideoAttributesToStackXtermSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiPushVideoAttributesToStackXtermWithArgs,
                (new Func<string, string>(CsiSequences.GenerateCsiPushVideoAttributesToStackXterm), CsiSequences.CsiPushVideoAttributesToStackXtermWithArgsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiQueryKeyModifierOptions,
                (new Func<int, string>(CsiSequences.GenerateCsiQueryKeyModifierOptions), CsiSequences.CsiQueryKeyModifierOptionsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiRectangularAreaChecksum,
                (new Func<int, int, int, int, int, int, string>(CsiSequences.GenerateCsiRectangularAreaChecksum), CsiSequences.CsiRectangularAreaChecksumSequenceRegex, 6) },
            { VtSequenceSpecificTypes.CsiRepeatGraphicCharacter,
                (new Func<int, string>(CsiSequences.GenerateCsiRepeatGraphicCharacter), CsiSequences.CsiRepeatGraphicCharacterSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiReportGraphicsRenditionRectangularArea,
                (new Func<int, int, int, int, string>(CsiSequences.GenerateCsiReportGraphicsRenditionRectangularArea), CsiSequences.CsiReportGraphicsRenditionRectangularAreaSequenceRegex, 4) },
            { VtSequenceSpecificTypes.CsiReportPaletteStack,
                (new Func<string>(CsiSequences.GenerateCsiReportPaletteStack), CsiSequences.CsiReportPaletteStackSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiReportXtermVersion,
                (new Func<int, string>(CsiSequences.GenerateCsiReportXtermVersion), CsiSequences.CsiReportXtermVersionSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiRequestAnsiMode,
                (new Func<int, string>(CsiSequences.GenerateCsiRequestAnsiMode), CsiSequences.CsiRequestAnsiModeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiRequestDecPrivateMode,
                (new Func<int, string>(CsiSequences.GenerateCsiRequestDecPrivateMode), CsiSequences.CsiRequestDecPrivateModeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiRequestLocatorPosition,
                (new Func<int, string>(CsiSequences.GenerateCsiRequestLocatorPosition), CsiSequences.CsiRequestLocatorPositionSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiRequestPresentationStateReport,
                (new Func<int, string>(CsiSequences.GenerateCsiRequestPresentationStateReport), CsiSequences.CsiRequestPresentationStateReportSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiRequestTerminalParameters,
                (new Func<int, string>(CsiSequences.GenerateCsiRequestTerminalParameters), CsiSequences.CsiRequestTerminalParametersSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiResetKeyModifierOptions,
                (new Func<int, string>(CsiSequences.GenerateCsiResetKeyModifierOptions), CsiSequences.CsiResetKeyModifierOptionsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiResetMode,
                (new Func<string, string>(CsiSequences.GenerateCsiResetMode), CsiSequences.CsiResetModeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiResetPrivateMode,
                (new Func<string, string>(CsiSequences.GenerateCsiResetPrivateMode), CsiSequences.CsiResetPrivateModeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiResetTitleModeFeatures,
                (new Func<int, string>(CsiSequences.GenerateCsiResetTitleModeFeatures), CsiSequences.CsiResetTitleModeFeaturesSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiRestoreCursor,
                (new Func<string>(CsiSequences.GenerateCsiRestoreCursor), CsiSequences.CsiRestoreCursorSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiRestoreDecPrivateModeValues,
                (new Func<string, string>(CsiSequences.GenerateCsiRestoreDecPrivateModeValues), CsiSequences.CsiRestoreDecPrivateModeValuesSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiReverseAttributesInRectangularArea,
                (new Func<string, int, int, int, int, string>(CsiSequences.GenerateCsiReverseAttributesInRectangularArea), CsiSequences.CsiReverseAttributesInRectangularAreaSequenceRegex, 5) },
            { VtSequenceSpecificTypes.CsiSaveCursor,
                (new Func<string>(CsiSequences.GenerateCsiSaveCursor), CsiSequences.CsiSaveCursorSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiScrollDown,
                (new Func<int, string>(CsiSequences.GenerateCsiScrollDown), CsiSequences.CsiScrollDownSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiScrollDownOriginal,
                (new Func<int, string>(CsiSequences.GenerateCsiScrollDownOriginal), CsiSequences.CsiScrollDownOriginalSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiScrollUp,
                (new Func<int, string>(CsiSequences.GenerateCsiScrollUp), CsiSequences.CsiScrollUpSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectActiveStatusDisplay,
                (new Func<int, string>(CsiSequences.GenerateCsiSelectActiveStatusDisplay), CsiSequences.CsiSelectActiveStatusDisplaySequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectAttributeChangeExtent,
                (new Func<int, string>(CsiSequences.GenerateCsiSelectAttributeChangeExtent), CsiSequences.CsiSelectAttributeChangeExtentSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectCharacterProtectionAttribute,
                (new Func<int, string>(CsiSequences.GenerateCsiSelectCharacterProtectionAttribute), CsiSequences.CsiSelectCharacterProtectionAttributeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectChecksumExtension,
                (new Func<int, string>(CsiSequences.GenerateCsiSelectChecksumExtension), CsiSequences.CsiSelectChecksumExtensionSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectColumnsPerPage,
                (new Func<int, string>(CsiSequences.GenerateCsiSelectColumnsPerPage), CsiSequences.CsiSelectColumnsPerPageSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectiveEraseRectangularArea,
                (new Func<int, int, int, int, string>(CsiSequences.GenerateCsiSelectiveEraseRectangularArea), CsiSequences.CsiSelectiveEraseRectangularAreaSequenceRegex, 4) },
            { VtSequenceSpecificTypes.CsiSelectLocatorEvents,
                (new Func<string, string>(CsiSequences.GenerateCsiSelectLocatorEvents), CsiSequences.CsiSelectLocatorEventsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectNumberOfLinesPerScreen,
                (new Func<int, string>(CsiSequences.GenerateCsiSelectNumberOfLinesPerScreen), CsiSequences.CsiSelectNumberOfLinesPerScreenSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSelectStatusLineType,
                (new Func<int, string>(CsiSequences.GenerateCsiSelectStatusLineType), CsiSequences.CsiSelectStatusLineTypeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSendDeviceAttributesPrimary,
                (new Func<int, string>(CsiSequences.GenerateCsiSendDeviceAttributesPrimary), CsiSequences.CsiSendDeviceAttributesPrimarySequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSendDeviceAttributesSecondary,
                (new Func<int, string>(CsiSequences.GenerateCsiSendDeviceAttributesSecondary), CsiSequences.CsiSendDeviceAttributesSecondarySequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSendDeviceAttributesTertiary,
                (new Func<int, string>(CsiSequences.GenerateCsiSendDeviceAttributesTertiary), CsiSequences.CsiSendDeviceAttributesTertiarySequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetConformanceLevel,
                (new Func<int, int, string>(CsiSequences.GenerateCsiSetConformanceLevel), CsiSequences.CsiSetConformanceLevelSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiSetCursorStyle,
                (new Func<int, string>(CsiSequences.GenerateCsiSetCursorStyle), CsiSequences.CsiSetCursorStyleSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetGraphicsAttribute,
                (new Func<int, int, string, string>(CsiSequences.GenerateCsiSetGraphicsAttribute), CsiSequences.CsiSetGraphicsAttributeSequenceRegex, 3) },
            { VtSequenceSpecificTypes.CsiSetKeyModifierOptions,
                (new Func<int, int, string>(CsiSequences.GenerateCsiSetKeyModifierOptions), CsiSequences.CsiSetKeyModifierOptionsSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiSetLeftRightMargins,
                (new Func<int, int, string>(CsiSequences.GenerateCsiSetLeftRightMargins), CsiSequences.CsiSetLeftRightMarginsSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiSetMarginBellVolume,
                (new Func<int, string>(CsiSequences.GenerateCsiSetMarginBellVolume), CsiSequences.CsiSetMarginBellVolumeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetMode,
                (new Func<string, string>(CsiSequences.GenerateCsiSetMode), CsiSequences.CsiSetModeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetPointerModeXterm,
                (new Func<int, string>(CsiSequences.GenerateCsiSetPointerModeXterm), CsiSequences.CsiSetPointerModeXtermSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetPrivateMode,
                (new Func<string, string>(CsiSequences.GenerateCsiSetPrivateMode), CsiSequences.CsiSetPrivateModeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetScrollingRegion,
                (new Func<int, int, string>(CsiSequences.GenerateCsiSetScrollingRegion), CsiSequences.CsiSetScrollingRegionSequenceRegex, 2) },
            { VtSequenceSpecificTypes.CsiSetShiftEscapeOptions,
                (new Func<int, string>(CsiSequences.GenerateCsiSetShiftEscapeOptions), CsiSequences.CsiSetShiftEscapeOptionsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetTitleModeXterm,
                (new Func<string, string>(CsiSequences.GenerateCsiSetTitleModeXterm), CsiSequences.CsiSetTitleModeXtermSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSetWarningBellVolume,
                (new Func<int, string>(CsiSequences.GenerateCsiSetWarningBellVolume), CsiSequences.CsiSetWarningBellVolumeSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiShiftLeftColumns,
                (new Func<int, string>(CsiSequences.GenerateCsiShiftLeftColumns), CsiSequences.CsiShiftLeftColumnsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiShiftRightColumns,
                (new Func<int, string>(CsiSequences.GenerateCsiShiftRightColumns), CsiSequences.CsiShiftRightColumnsSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiSoftTerminalReset,
                (new Func<string>(CsiSequences.GenerateCsiSoftTerminalReset), CsiSequences.CsiSoftTerminalResetSequenceRegex, 0) },
            { VtSequenceSpecificTypes.CsiTabClear,
                (new Func<int, string>(CsiSequences.GenerateCsiTabClear), CsiSequences.CsiTabClearSequenceRegex, 1) },
            { VtSequenceSpecificTypes.CsiWindowManipulation,
                (new Func<int, int, int, string>(CsiSequences.GenerateCsiWindowManipulation), CsiSequences.CsiWindowManipulationSequenceRegex, 3) },

            // DCS sequences
            { VtSequenceSpecificTypes.DcsRequestResourceValues,
                (new Func<string, string>(DcsSequences.GenerateDcsRequestResourceValues), DcsSequences.DcsRequestResourceValuesSequenceRegex, 1) },
            { VtSequenceSpecificTypes.DcsRequestStatusString,
                (new Func<string, string>(DcsSequences.GenerateDcsRequestStatusString), DcsSequences.DcsRequestStatusStringSequenceRegex, 1) },
            { VtSequenceSpecificTypes.DcsRequestTermInfoData,
                (new Func<string, string>(DcsSequences.GenerateDcsRequestTermInfoData), DcsSequences.DcsRequestTermInfoDataSequenceRegex, 1) },
            { VtSequenceSpecificTypes.DcsRestorePresentationStatus,
                (new Func<int, string, string>(DcsSequences.GenerateDcsRestorePresentationStatus), DcsSequences.DcsRestorePresentationStatusSequenceRegex, 2) },
            { VtSequenceSpecificTypes.DcsSetTermInfoData,
                (new Func<string, string>(DcsSequences.GenerateDcsSetTermInfoData), DcsSequences.DcsSetTermInfoDataSequenceRegex, 1) },
            { VtSequenceSpecificTypes.DcsUserDefinedKeys,
                (new Func<int, int, string, string>(DcsSequences.GenerateDcsUserDefinedKeys), DcsSequences.DcsUserDefinedKeysSequenceRegex, 3) },

            // ESC sequences
            { VtSequenceSpecificTypes.Esc7BitControls,
                (new Func<string>(EscSequences.GenerateEsc7BitControls), EscSequences.Esc7BitControlsSequenceRegex, 0) },
            { VtSequenceSpecificTypes.Esc8BitControls,
                (new Func<string>(EscSequences.GenerateEsc8BitControls), EscSequences.Esc8BitControlsSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscAnsiConformanceLevel1,
                (new Func<string>(EscSequences.GenerateEscAnsiConformanceLevel1), EscSequences.EscAnsiConformanceLevel1SequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscAnsiConformanceLevel2,
                (new Func<string>(EscSequences.GenerateEscAnsiConformanceLevel2), EscSequences.EscAnsiConformanceLevel2SequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscAnsiConformanceLevel3,
                (new Func<string>(EscSequences.GenerateEscAnsiConformanceLevel3), EscSequences.EscAnsiConformanceLevel3SequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscApplicationKeypad,
                (new Func<string>(EscSequences.GenerateEscApplicationKeypad), EscSequences.EscApplicationKeypadSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscBackIndex,
                (new Func<string>(EscSequences.GenerateEscBackIndex), EscSequences.EscBackIndexSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscCursorToLowerLeftCorner,
                (new Func<string>(EscSequences.GenerateEscCursorToLowerLeftCorner), EscSequences.EscCursorToLowerLeftCornerSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscDecDoubleHeightLineBottomHalf,
                (new Func<string>(EscSequences.GenerateEscDecDoubleHeightLineBottomHalf), EscSequences.EscDecDoubleHeightLineBottomHalfSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscDecDoubleHeightLineTopHalf,
                (new Func<string>(EscSequences.GenerateEscDecDoubleHeightLineTopHalf), EscSequences.EscDecDoubleHeightLineTopHalfSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscDecDoubleWidthLine,
                (new Func<string>(EscSequences.GenerateEscDecDoubleWidthLine), EscSequences.EscDecDoubleWidthLineSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscDecScreenAlignmentTest,
                (new Func<string>(EscSequences.GenerateEscDecScreenAlignmentTest), EscSequences.EscDecScreenAlignmentTestSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscDecSingleWidthLine,
                (new Func<string>(EscSequences.GenerateEscDecSingleWidthLine), EscSequences.EscDecSingleWidthLineSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscDesignateG0CharacterSet,
                (new Func<string, string>(EscSequences.GenerateEscDesignateG0CharacterSet), EscSequences.EscDesignateG0CharacterSetSequenceRegex, 1) },
            { VtSequenceSpecificTypes.EscDesignateG1CharacterSet,
                (new Func<string, string>(EscSequences.GenerateEscDesignateG1CharacterSet), EscSequences.EscDesignateG1CharacterSetSequenceRegex, 1) },
            { VtSequenceSpecificTypes.EscDesignateG1CharacterSetAlt,
                (new Func<string, string>(EscSequences.GenerateEscDesignateG1CharacterSetAlt), EscSequences.EscDesignateG1CharacterSetAltSequenceRegex, 1) },
            { VtSequenceSpecificTypes.EscDesignateG2CharacterSet,
                (new Func<string, string>(EscSequences.GenerateEscDesignateG2CharacterSet), EscSequences.EscDesignateG2CharacterSetSequenceRegex, 1) },
            { VtSequenceSpecificTypes.EscDesignateG2CharacterSetAlt,
                (new Func<string, string>(EscSequences.GenerateEscDesignateG2CharacterSetAlt), EscSequences.EscDesignateG2CharacterSetAltSequenceRegex, 1) },
            { VtSequenceSpecificTypes.EscDesignateG3CharacterSet,
                (new Func<string, string>(EscSequences.GenerateEscDesignateG3CharacterSet), EscSequences.EscDesignateG3CharacterSetSequenceRegex, 1) },
            { VtSequenceSpecificTypes.EscDesignateG3CharacterSetAlt,
                (new Func<string, string>(EscSequences.GenerateEscDesignateG3CharacterSetAlt), EscSequences.EscDesignateG3CharacterSetAltSequenceRegex, 1) },
            { VtSequenceSpecificTypes.EscForwardIndex,
                (new Func<string>(EscSequences.GenerateEscForwardIndex), EscSequences.EscForwardIndexSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscFullReset,
                (new Func<string>(EscSequences.GenerateEscFullReset), EscSequences.EscFullResetSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscInvokeG1CharacterSetGr,
                (new Func<string>(EscSequences.GenerateEscInvokeG1CharacterSetGr), EscSequences.EscInvokeG1CharacterSetGrSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscInvokeG2CharacterSetGl,
                (new Func<string>(EscSequences.GenerateEscInvokeG2CharacterSetGl), EscSequences.EscInvokeG2CharacterSetGlSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscInvokeG2CharacterSetGr,
                (new Func<string>(EscSequences.GenerateEscInvokeG2CharacterSetGr), EscSequences.EscInvokeG2CharacterSetGrSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscInvokeG3CharacterSetGl,
                (new Func<string>(EscSequences.GenerateEscInvokeG3CharacterSetGl), EscSequences.EscInvokeG3CharacterSetGlSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscInvokeG3CharacterSetGr,
                (new Func<string>(EscSequences.GenerateEscInvokeG3CharacterSetGr), EscSequences.EscInvokeG3CharacterSetGrSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscMemoryLock,
                (new Func<string>(EscSequences.GenerateEscMemoryLock), EscSequences.EscMemoryLockSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscMemoryUnlock,
                (new Func<string>(EscSequences.GenerateEscMemoryUnlock), EscSequences.EscMemoryUnlockSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscNormalKeypad,
                (new Func<string>(EscSequences.GenerateEscNormalKeypad), EscSequences.EscNormalKeypadSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscRestoreCursor,
                (new Func<string>(EscSequences.GenerateEscRestoreCursor), EscSequences.EscRestoreCursorSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscSaveCursor,
                (new Func<string>(EscSequences.GenerateEscSaveCursor), EscSequences.EscSaveCursorSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscSelectDefaultCharacterSet,
                (new Func<string>(EscSequences.GenerateEscSelectDefaultCharacterSet), EscSequences.EscSelectDefaultCharacterSetSequenceRegex, 0) },
            { VtSequenceSpecificTypes.EscSelectUtf8CharacterSet,
                (new Func<string>(EscSequences.GenerateEscSelectUtf8CharacterSet), EscSequences.EscSelectUtf8CharacterSetSequenceRegex, 0) },

            // OSC sequences
            { VtSequenceSpecificTypes.OscOperatingSystemCommand,
                (new Func<string, string>(OscSequences.GenerateOscOperatingSystemCommand), OscSequences.OscOperatingSystemCommandSequenceRegex, 1) },
            { VtSequenceSpecificTypes.OscOperatingSystemCommandAlt,
                (new Func<string, string>(OscSequences.GenerateOscOperatingSystemCommandAlt), OscSequences.OscOperatingSystemCommandAltSequenceRegex, 1) },

            // PM sequences
            { VtSequenceSpecificTypes.PmPrivacyMessage,
                (new Func<string, string>(PmSequences.GeneratePmPrivacyMessage), PmSequences.PmPrivacyMessageSequenceRegex, 1) },

            // C1 sequences
            { VtSequenceSpecificTypes.C1ApplicationProgramCommand,
                (new Func<string>(C1Sequences.GenerateC1ApplicationProgramCommand), C1Sequences.C1ApplicationProgramCommandSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1ControlSequenceIndicator,
                (new Func<string>(C1Sequences.GenerateC1ControlSequenceIndicator), C1Sequences.C1ControlSequenceIndicatorSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1DeviceControlString,
                (new Func<string>(C1Sequences.GenerateC1DeviceControlString), C1Sequences.C1DeviceControlStringSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1EndOfGuardedArea,
                (new Func<string>(C1Sequences.GenerateC1EndOfGuardedArea), C1Sequences.C1EndOfGuardedAreaSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1Index,
                (new Func<string>(C1Sequences.GenerateC1Index), C1Sequences.C1IndexSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1NextLine,
                (new Func<string>(C1Sequences.GenerateC1NextLine), C1Sequences.C1NextLineSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1OperatingSystemCommand,
                (new Func<string>(C1Sequences.GenerateC1OperatingSystemCommand), C1Sequences.C1OperatingSystemCommandSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1PrivacyMessage,
                (new Func<string>(C1Sequences.GenerateC1PrivacyMessage), C1Sequences.C1PrivacyMessageSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1ReturnTerminalId,
                (new Func<string>(C1Sequences.GenerateC1ReturnTerminalId), C1Sequences.C1ReturnTerminalIdSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1ReverseIndex,
                (new Func<string>(C1Sequences.GenerateC1ReverseIndex), C1Sequences.C1ReverseIndexSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1SingleShiftSelectG2CharacterSet,
                (new Func<string>(C1Sequences.GenerateC1SingleShiftSelectG2CharacterSet), C1Sequences.C1SingleShiftSelectG2CharacterSetSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1SingleShiftSelectG3CharacterSet,
                (new Func<string>(C1Sequences.GenerateC1SingleShiftSelectG3CharacterSet), C1Sequences.C1SingleShiftSelectG3CharacterSetSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1StartOfGuardedArea,
                (new Func<string>(C1Sequences.GenerateC1StartOfGuardedArea), C1Sequences.C1StartOfGuardedAreaSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1StartOfString,
                (new Func<string>(C1Sequences.GenerateC1StartOfString), C1Sequences.C1StartOfStringSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1StringTerminator,
                (new Func<string>(C1Sequences.GenerateC1StringTerminator), C1Sequences.C1StringTerminatorSequenceRegex, 0) },
            { VtSequenceSpecificTypes.C1TabSet,
                (new Func<string>(C1Sequences.GenerateC1TabSet), C1Sequences.C1TabSetSequenceRegex, 0) },
        };

        /// <summary>
        /// Builds a VT sequence using specific types
        /// </summary>
        /// <param name="specificType"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string BuildVtSequence(VtSequenceSpecificTypes specificType, params object[] arguments)
        {
            // Check the type
            if (!Enum.IsDefined(typeof(VtSequenceSpecificTypes), specificType))
                throw new TerminauxException($"Cannot build VT sequence for nonexistent type {Convert.ToInt32(specificType)}");

            // Now, check the argument count
            int argCount = sequenceBuilders[specificType].argumentsRequired;
            if (argCount < arguments.Length)
                throw new TerminauxException($"Cannot build VT sequence with missing arguments. Expected {argCount} arguments, got {arguments.Length} arguments. {Convert.ToInt32(specificType)}");

            // Now, get the sequence and statically give arguments for performance to try to escape from DynamicInvoke
            var sequenceRegexGenerator = sequenceBuilders[specificType].generator;
            return DeterministicExecution(sequenceRegexGenerator, arguments);
        }

        /// <summary>
        /// Determines the VT sequence type from the given sequence
        /// </summary>
        /// <param name="sequence">The sequence to query</param>
        /// <returns>A tuple of (<see cref="VtSequenceType"/>, <see cref="VtSequenceSpecificTypes"/>) containing information about a sequence type and a sequence command type</returns>
        /// <exception cref="Exception"></exception>
        public static (VtSequenceType, VtSequenceSpecificTypes) DetermineTypeFromSequence(string sequence)
        {
            // First, get all the VT sequence types
            var seqTypeEnumNames = sequenceBuilders.Keys;

            // Then, iterate through all the sequence types until we find an appropriate one that matches the sequence
            foreach (var seqType in seqTypeEnumNames)
            {
                // Now, get the appropriate regex to check to see if there is a match.
                var regex = sequenceBuilders[seqType].matchRegex;
                if (regex.IsMatch(sequence))
                {
                    // It's a match! Get the type and the specific type of the sequence and return them
                    string enumName = $"{seqType}";
                    string typeName = $"{getTypeRegex.Replace(enumName, "")}";
                    VtSequenceType sequenceType = (VtSequenceType)Enum.Parse(typeof(VtSequenceType), typeName);
                    return (sequenceType, seqType);
                }
            }

            // If still not found, then throw
            throw new TerminauxException("Can't determine type from this sequence. Make sure that you've specified it correctly.");
        }

        private static string DeterministicExecution(Delegate generator, params object[] arguments)
        {
            if (generator is Func<string> generatorNoArgs)
                return generatorNoArgs.Invoke();
            else if (generator is Func<string, string> generatorParameterized1)
                return generatorParameterized1.Invoke(arguments[0].ToString());
            else if (generator is Func<string, int, int, int, int, string> generatorParameterized2)
                return generatorParameterized2.Invoke(arguments[0].ToString(), (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4]);
            else if (generator is Func<int, int, int, int, int, int, int, int, string> generatorParameterized3)
                return generatorParameterized3.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4], (int)arguments[5], (int)arguments[6], (int)arguments[7]);
            else if (generator is Func<int, string> generatorParameterized4)
                return generatorParameterized4.Invoke((int)arguments[0]);
            else if (generator is Func<int, int, string> generatorParameterized5)
                return generatorParameterized5.Invoke((int)arguments[0], (int)arguments[1]);
            else if (generator is Func<int, int, int, int, string> generatorParameterized6)
                return generatorParameterized6.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3]);
            else if (generator is Func<char, int, int, int, int, string> generatorParameterized7)
                return generatorParameterized7.Invoke((char)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4]);
            else if (generator is Func<int, int, int, int, int, string> generatorParameterized8)
                return generatorParameterized8.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4]);
            else if (generator is Func<int, int, int, int, int, int, string> generatorParameterized9)
                return generatorParameterized9.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4], (int)arguments[5]);
            else if (generator is Func<int, int, string, string> generatorParameterized10)
                return generatorParameterized10.Invoke((int)arguments[0], (int)arguments[1], arguments[2].ToString());
            else if (generator is Func<int, string, string> generatorParameterized11)
                return generatorParameterized11.Invoke((int)arguments[0], arguments[1].ToString());
            else if (generator is Func<int, int, int, string> generatorParameterized12)
                return generatorParameterized12.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2]);
            return generator.DynamicInvoke(arguments).ToString();
        }
    }
}
