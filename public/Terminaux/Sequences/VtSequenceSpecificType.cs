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

namespace Terminaux.Sequences
{
    /// <summary>
    /// Specific VT sequences sorted by type
    /// </summary>
    public enum VtSequenceSpecificType
    {
        /// <summary>
        /// CSI VT sequence (CsiInsertBlankCharacters)
        /// </summary>
        CsiInsertBlankCharacters,
        /// <summary>
        /// CSI VT sequence (CsiShiftLeftColumns)
        /// </summary>
        CsiShiftLeftColumns,
        /// <summary>
        /// CSI VT sequence (CsiMoveCursorUp)
        /// </summary>
        CsiMoveCursorUp,
        /// <summary>
        /// CSI VT sequence (CsiShiftRightColumns)
        /// </summary>
        CsiShiftRightColumns,
        /// <summary>
        /// CSI VT sequence (CsiMoveCursorDown)
        /// </summary>
        CsiMoveCursorDown,
        /// <summary>
        /// CSI VT sequence (CsiMoveCursorRight)
        /// </summary>
        CsiMoveCursorRight,
        /// <summary>
        /// CSI VT sequence (CsiMoveCursorLeft)
        /// </summary>
        CsiMoveCursorLeft,
        /// <summary>
        /// CSI VT sequence (CsiMoveCursorNextLine)
        /// </summary>
        CsiMoveCursorNextLine,
        /// <summary>
        /// CSI VT sequence (CsiMoveCursorPreviousLine)
        /// </summary>
        CsiMoveCursorPreviousLine,
        /// <summary>
        /// CSI VT sequence (CsiCursorCharacterAbsoluteLine)
        /// </summary>
        CsiCursorCharacterAbsoluteLine,
        /// <summary>
        /// CSI VT sequence (CsiCursorPosition)
        /// </summary>
        CsiCursorPosition,
        /// <summary>
        /// CSI VT sequence (CsiCursorForwardTabulation)
        /// </summary>
        CsiCursorForwardTabulation,
        /// <summary>
        /// CSI VT sequence (CsiEraseInDisplay)
        /// </summary>
        CsiEraseInDisplay,
        /// <summary>
        /// CSI VT sequence (CsiEraseInDisplayDecsed)
        /// </summary>
        CsiEraseInDisplayDecsed,
        /// <summary>
        /// CSI VT sequence (CsiEraseInLine)
        /// </summary>
        CsiEraseInLine,
        /// <summary>
        /// CSI VT sequence (CsiEraseInLineDecsel)
        /// </summary>
        CsiEraseInLineDecsel,
        /// <summary>
        /// CSI VT sequence (CsiInsertLine)
        /// </summary>
        CsiInsertLine,
        /// <summary>
        /// CSI VT sequence (CsiDeleteLine)
        /// </summary>
        CsiDeleteLine,
        /// <summary>
        /// CSI VT sequence (CsiDeleteChars)
        /// </summary>
        CsiDeleteChars,
        /// <summary>
        /// CSI VT sequence (CsiPushColorToStack)
        /// </summary>
        CsiPushColorToStack,
        /// <summary>
        /// CSI VT sequence (CsiPushColorToStackWithArgs)
        /// </summary>
        CsiPushColorToStackWithArgs,
        /// <summary>
        /// CSI VT sequence (CsiPopColorFromStack)
        /// </summary>
        CsiPopColorFromStack,
        /// <summary>
        /// CSI VT sequence (CsiPopColorFromStackWithArgs)
        /// </summary>
        CsiPopColorFromStackWithArgs,
        /// <summary>
        /// CSI VT sequence (CsiReportPaletteStack)
        /// </summary>
        CsiReportPaletteStack,
        /// <summary>
        /// CSI VT sequence (CsiScrollUp)
        /// </summary>
        CsiScrollUp,
        /// <summary>
        /// CSI VT sequence (CsiSetGraphicsAttribute)
        /// </summary>
        CsiSetGraphicsAttribute,
        /// <summary>
        /// CSI VT sequence (CsiScrollDown)
        /// </summary>
        CsiScrollDown,
        /// <summary>
        /// CSI VT sequence (CsiInitiateHighlightMouseTracking)
        /// </summary>
        CsiInitiateHighlightMouseTracking,
        /// <summary>
        /// CSI VT sequence (CsiResetTitleModeFeatures)
        /// </summary>
        CsiResetTitleModeFeatures,
        /// <summary>
        /// CSI VT sequence (CsiEraseCharacters)
        /// </summary>
        CsiEraseCharacters,
        /// <summary>
        /// CSI VT sequence (CsiCursorBackwardTabulation)
        /// </summary>
        CsiCursorBackwardTabulation,
        /// <summary>
        /// CSI VT sequence (CsiScrollDownOriginal)
        /// </summary>
        CsiScrollDownOriginal,
        /// <summary>
        /// CSI VT sequence (CsiCursorPositionAbsolute)
        /// </summary>
        CsiCursorPositionAbsolute,
        /// <summary>
        /// CSI VT sequence (CsiCursorPositionRelative)
        /// </summary>
        CsiCursorPositionRelative,
        /// <summary>
        /// CSI VT sequence (CsiRepeatGraphicCharacter)
        /// </summary>
        CsiRepeatGraphicCharacter,
        /// <summary>
        /// CSI VT sequence (CsiSendDeviceAttributesPrimary)
        /// </summary>
        CsiSendDeviceAttributesPrimary,
        /// <summary>
        /// CSI VT sequence (CsiSendDeviceAttributesSecondary)
        /// </summary>
        CsiSendDeviceAttributesSecondary,
        /// <summary>
        /// CSI VT sequence (CsiSendDeviceAttributesTertiary)
        /// </summary>
        CsiSendDeviceAttributesTertiary,
        /// <summary>
        /// CSI VT sequence (CsiLinePositionAbsolute)
        /// </summary>
        CsiLinePositionAbsolute,
        /// <summary>
        /// CSI VT sequence (CsiLinePositionRelative)
        /// </summary>
        CsiLinePositionRelative,
        /// <summary>
        /// CSI VT sequence (CsiLeftTopPosition)
        /// </summary>
        CsiLeftTopPosition,
        /// <summary>
        /// CSI VT sequence (CsiTabClear)
        /// </summary>
        CsiTabClear,
        /// <summary>
        /// CSI VT sequence (CsiSetMode)
        /// </summary>
        CsiSetMode,
        /// <summary>
        /// CSI VT sequence (CsiSetPrivateMode)
        /// </summary>
        CsiSetPrivateMode,
        /// <summary>
        /// CSI VT sequence (CsiMediaCopy)
        /// </summary>
        CsiMediaCopy,
        /// <summary>
        /// CSI VT sequence (CsiMediaCopyPrivate)
        /// </summary>
        CsiMediaCopyPrivate,
        /// <summary>
        /// CSI VT sequence (CsiResetMode)
        /// </summary>
        CsiResetMode,
        /// <summary>
        /// CSI VT sequence (CsiResetPrivateMode)
        /// </summary>
        CsiResetPrivateMode,
        /// <summary>
        /// CSI VT sequence (CsiCharacterAttributes)
        /// </summary>
        CsiCharacterAttributes,
        /// <summary>
        /// CSI VT sequence (CsiSetKeyModifierOptions)
        /// </summary>
        CsiSetKeyModifierOptions,
        /// <summary>
        /// CSI VT sequence (CsiResetKeyModifierOptions)
        /// </summary>
        CsiResetKeyModifierOptions,
        /// <summary>
        /// CSI VT sequence (CsiQueryKeyModifierOptions)
        /// </summary>
        CsiQueryKeyModifierOptions,
        /// <summary>
        /// CSI VT sequence (CsiDeviceStatusReport)
        /// </summary>
        CsiDeviceStatusReport,
        /// <summary>
        /// CSI VT sequence (CsiDisableKeyModifierOptions)
        /// </summary>
        CsiDisableKeyModifierOptions,
        /// <summary>
        /// CSI VT sequence (CsiDeviceStatusReportDec)
        /// </summary>
        CsiDeviceStatusReportDec,
        /// <summary>
        /// CSI VT sequence (CsiSetPointerModeXterm)
        /// </summary>
        CsiSetPointerModeXterm,
        /// <summary>
        /// CSI VT sequence (CsiSoftTerminalReset)
        /// </summary>
        CsiSoftTerminalReset,
        /// <summary>
        /// CSI VT sequence (CsiSetConformanceLevel)
        /// </summary>
        CsiSetConformanceLevel,
        /// <summary>
        /// CSI VT sequence (CsiRequestAnsiMode)
        /// </summary>
        CsiRequestAnsiMode,
        /// <summary>
        /// CSI VT sequence (CsiRequestDecPrivateMode)
        /// </summary>
        CsiRequestDecPrivateMode,
        /// <summary>
        /// CSI VT sequence (CsiPushVideoAttributesToStack)
        /// </summary>
        CsiPushVideoAttributesToStack,
        /// <summary>
        /// CSI VT sequence (CsiPushVideoAttributesToStackWithArgs)
        /// </summary>
        CsiPushVideoAttributesToStackWithArgs,
        /// <summary>
        /// CSI VT sequence (CsiReportXtermVersion)
        /// </summary>
        CsiReportXtermVersion,
        /// <summary>
        /// CSI VT sequence (CsiLoadLeds)
        /// </summary>
        CsiLoadLeds,
        /// <summary>
        /// CSI VT sequence (CsiSetCursorStyle)
        /// </summary>
        CsiSetCursorStyle,
        /// <summary>
        /// CSI VT sequence (CsiSelectCharacterProtectionAttribute)
        /// </summary>
        CsiSelectCharacterProtectionAttribute,
        /// <summary>
        /// CSI VT sequence (CsiPopVideoAttributesFromStack)
        /// </summary>
        CsiPopVideoAttributesFromStack,
        /// <summary>
        /// CSI VT sequence (CsiSetScrollingRegion)
        /// </summary>
        CsiSetScrollingRegion,
        /// <summary>
        /// CSI VT sequence (CsiRestoreDecPrivateModeValues)
        /// </summary>
        CsiRestoreDecPrivateModeValues,
        /// <summary>
        /// CSI VT sequence (CsiChangeAttributesInRectangularArea)
        /// </summary>
        CsiChangeAttributesInRectangularArea,
        /// <summary>
        /// CSI VT sequence (CsiSaveCursor)
        /// </summary>
        CsiSaveCursor,
        /// <summary>
        /// CSI VT sequence (CsiSetLeftRightMargins)
        /// </summary>
        CsiSetLeftRightMargins,
        /// <summary>
        /// CSI VT sequence (CsiSetShiftEscapeOptions)
        /// </summary>
        CsiSetShiftEscapeOptions,
        /// <summary>
        /// CSI VT sequence (CsiDecPrivateModeValues)
        /// </summary>
        CsiDecPrivateModeValues,
        /// <summary>
        /// CSI VT sequence (CsiWindowManipulation)
        /// </summary>
        CsiWindowManipulation,
        /// <summary>
        /// CSI VT sequence (CsiSetTitleModeXterm)
        /// </summary>
        CsiSetTitleModeXterm,
        /// <summary>
        /// CSI VT sequence (CsiSetWarningBellVolume)
        /// </summary>
        CsiSetWarningBellVolume,
        /// <summary>
        /// CSI VT sequence (CsiReverseAttributesInRectangularArea)
        /// </summary>
        CsiReverseAttributesInRectangularArea,
        /// <summary>
        /// CSI VT sequence (CsiRestoreCursor)
        /// </summary>
        CsiRestoreCursor,
        /// <summary>
        /// CSI VT sequence (CsiSetMarginBellVolume)
        /// </summary>
        CsiSetMarginBellVolume,
        /// <summary>
        /// CSI VT sequence (CsiCopyRectangularArea)
        /// </summary>
        CsiCopyRectangularArea,
        /// <summary>
        /// CSI VT sequence (CsiRequestPresentationStateReport)
        /// </summary>
        CsiRequestPresentationStateReport,
        /// <summary>
        /// CSI VT sequence (CsiEnableFilterRectangle)
        /// </summary>
        CsiEnableFilterRectangle,
        /// <summary>
        /// CSI VT sequence (CsiRequestTerminalParameters)
        /// </summary>
        CsiRequestTerminalParameters,
        /// <summary>
        /// CSI VT sequence (CsiSelectAttributeChangeExtent)
        /// </summary>
        CsiSelectAttributeChangeExtent,
        /// <summary>
        /// CSI VT sequence (CsiFillRectangularArea)
        /// </summary>
        CsiFillRectangularArea,
        /// <summary>
        /// CSI VT sequence (CsiSelectChecksumExtension)
        /// </summary>
        CsiSelectChecksumExtension,
        /// <summary>
        /// CSI VT sequence (CsiRectangularAreaChecksum)
        /// </summary>
        CsiRectangularAreaChecksum,
        /// <summary>
        /// CSI VT sequence (CsiLocatorReporting)
        /// </summary>
        CsiLocatorReporting,
        /// <summary>
        /// CSI VT sequence (CsiEraseRectangularArea)
        /// </summary>
        CsiEraseRectangularArea,
        /// <summary>
        /// CSI VT sequence (CsiSelectLocatorEvents)
        /// </summary>
        CsiSelectLocatorEvents,
        /// <summary>
        /// CSI VT sequence (CsiPushVideoAttributesToStackXterm)
        /// </summary>
        CsiPushVideoAttributesToStackXterm,
        /// <summary>
        /// CSI VT sequence (CsiPushVideoAttributesToStackXtermWithArgs)
        /// </summary>
        CsiPushVideoAttributesToStackXtermWithArgs,
        /// <summary>
        /// CSI VT sequence (CsiSelectiveEraseRectangularArea)
        /// </summary>
        CsiSelectiveEraseRectangularArea,
        /// <summary>
        /// CSI VT sequence (CsiReportGraphicsRenditionRectangularArea)
        /// </summary>
        CsiReportGraphicsRenditionRectangularArea,
        /// <summary>
        /// CSI VT sequence (CsiSelectColumnsPerPage)
        /// </summary>
        CsiSelectColumnsPerPage,
        /// <summary>
        /// CSI VT sequence (CsiRequestLocatorPosition)
        /// </summary>
        CsiRequestLocatorPosition,
        /// <summary>
        /// CSI VT sequence (CsiSelectNumberOfLinesPerScreen)
        /// </summary>
        CsiSelectNumberOfLinesPerScreen,
        /// <summary>
        /// CSI VT sequence (CsiPopVideoAttributesFromStackXterm)
        /// </summary>
        CsiPopVideoAttributesFromStackXterm,
        /// <summary>
        /// CSI VT sequence (CsiInsertColumns)
        /// </summary>
        CsiInsertColumns,
        /// <summary>
        /// CSI VT sequence (CsiSelectActiveStatusDisplay)
        /// </summary>
        CsiSelectActiveStatusDisplay,
        /// <summary>
        /// CSI VT sequence (CsiDeleteColumns)
        /// </summary>
        CsiDeleteColumns,
        /// <summary>
        /// CSI VT sequence (CsiSelectStatusLineType)
        /// </summary>
        CsiSelectStatusLineType,
        /// <summary>
        /// OSC VT sequence (OscOperatingSystemCommand)
        /// </summary>
        OscOperatingSystemCommand,
        /// <summary>
        /// OSC VT sequence (OscOperatingSystemCommandAlt)
        /// </summary>
        OscOperatingSystemCommandAlt,
        /// <summary>
        /// ESC VT sequence (Esc7BitControls)
        /// </summary>
        Esc7BitControls,
        /// <summary>
        /// ESC VT sequence (Esc8BitControls)
        /// </summary>
        Esc8BitControls,
        /// <summary>
        /// ESC VT sequence (EscAnsiConformanceLevel1)
        /// </summary>
        EscAnsiConformanceLevel1,
        /// <summary>
        /// ESC VT sequence (EscAnsiConformanceLevel2)
        /// </summary>
        EscAnsiConformanceLevel2,
        /// <summary>
        /// ESC VT sequence (EscAnsiConformanceLevel3)
        /// </summary>
        EscAnsiConformanceLevel3,
        /// <summary>
        /// ESC VT sequence (EscDecDoubleHeightLineTopHalf)
        /// </summary>
        EscDecDoubleHeightLineTopHalf,
        /// <summary>
        /// ESC VT sequence (EscDecDoubleHeightLineBottomHalf)
        /// </summary>
        EscDecDoubleHeightLineBottomHalf,
        /// <summary>
        /// ESC VT sequence (EscDecSingleWidthLine)
        /// </summary>
        EscDecSingleWidthLine,
        /// <summary>
        /// ESC VT sequence (EscDecDoubleWidthLine)
        /// </summary>
        EscDecDoubleWidthLine,
        /// <summary>
        /// ESC VT sequence (EscDecScreenAlignmentTest)
        /// </summary>
        EscDecScreenAlignmentTest,
        /// <summary>
        /// ESC VT sequence (EscSelectDefaultCharacterSet)
        /// </summary>
        EscSelectDefaultCharacterSet,
        /// <summary>
        /// ESC VT sequence (EscSelectUtf8CharacterSet)
        /// </summary>
        EscSelectUtf8CharacterSet,
        /// <summary>
        /// ESC VT sequence (EscDesignateG0CharacterSet)
        /// </summary>
        EscDesignateG0CharacterSet,
        /// <summary>
        /// ESC VT sequence (EscDesignateG1CharacterSet)
        /// </summary>
        EscDesignateG1CharacterSet,
        /// <summary>
        /// ESC VT sequence (EscDesignateG2CharacterSet)
        /// </summary>
        EscDesignateG2CharacterSet,
        /// <summary>
        /// ESC VT sequence (EscDesignateG3CharacterSet)
        /// </summary>
        EscDesignateG3CharacterSet,
        /// <summary>
        /// ESC VT sequence (EscDesignateG1CharacterSetAlt)
        /// </summary>
        EscDesignateG1CharacterSetAlt,
        /// <summary>
        /// ESC VT sequence (EscDesignateG2CharacterSetAlt)
        /// </summary>
        EscDesignateG2CharacterSetAlt,
        /// <summary>
        /// ESC VT sequence (EscDesignateG3CharacterSetAlt)
        /// </summary>
        EscDesignateG3CharacterSetAlt,
        /// <summary>
        /// ESC VT sequence (EscBackIndex)
        /// </summary>
        EscBackIndex,
        /// <summary>
        /// ESC VT sequence (EscSaveCursor)
        /// </summary>
        EscSaveCursor,
        /// <summary>
        /// ESC VT sequence (EscRestoreCursor)
        /// </summary>
        EscRestoreCursor,
        /// <summary>
        /// ESC VT sequence (EscForwardIndex)
        /// </summary>
        EscForwardIndex,
        /// <summary>
        /// ESC VT sequence (EscApplicationKeypad)
        /// </summary>
        EscApplicationKeypad,
        /// <summary>
        /// ESC VT sequence (EscNormalKeypad)
        /// </summary>
        EscNormalKeypad,
        /// <summary>
        /// ESC VT sequence (EscCursorToLowerLeftCorner)
        /// </summary>
        EscCursorToLowerLeftCorner,
        /// <summary>
        /// ESC VT sequence (EscFullReset)
        /// </summary>
        EscFullReset,
        /// <summary>
        /// ESC VT sequence (EscMemoryLock)
        /// </summary>
        EscMemoryLock,
        /// <summary>
        /// ESC VT sequence (EscMemoryUnlock)
        /// </summary>
        EscMemoryUnlock,
        /// <summary>
        /// ESC VT sequence (EscInvokeG2CharacterSetGl)
        /// </summary>
        EscInvokeG2CharacterSetGl,
        /// <summary>
        /// ESC VT sequence (EscInvokeG3CharacterSetGl)
        /// </summary>
        EscInvokeG3CharacterSetGl,
        /// <summary>
        /// ESC VT sequence (EscInvokeG2CharacterSetGr)
        /// </summary>
        EscInvokeG2CharacterSetGr,
        /// <summary>
        /// ESC VT sequence (EscInvokeG3CharacterSetGr)
        /// </summary>
        EscInvokeG3CharacterSetGr,
        /// <summary>
        /// ESC VT sequence (EscInvokeG1CharacterSetGr)
        /// </summary>
        EscInvokeG1CharacterSetGr,
        /// <summary>
        /// APC VT sequence (ApcApplicationProgramCommand)
        /// </summary>
        ApcApplicationProgramCommand,
        /// <summary>
        /// DCS VT sequence (DcsUserDefinedKeys)
        /// </summary>
        DcsUserDefinedKeys,
        /// <summary>
        /// DCS VT sequence (DcsRequestStatusString)
        /// </summary>
        DcsRequestStatusString,
        /// <summary>
        /// DCS VT sequence (DcsRestorePresentationStatus)
        /// </summary>
        DcsRestorePresentationStatus,
        /// <summary>
        /// DCS VT sequence (DcsRequestResourceValues)
        /// </summary>
        DcsRequestResourceValues,
        /// <summary>
        /// DCS VT sequence (DcsSetTermInfoData)
        /// </summary>
        DcsSetTermInfoData,
        /// <summary>
        /// DCS VT sequence (DcsRequestTermInfoData)
        /// </summary>
        DcsRequestTermInfoData,
        /// <summary>
        /// PM VT sequence (PmPrivacyMessage)
        /// </summary>
        PmPrivacyMessage,
        /// <summary>
        /// C1 VT sequence (C1Index)
        /// </summary>
        C1Index,
        /// <summary>
        /// C1 VT sequence (C1NextLine)
        /// </summary>
        C1NextLine,
        /// <summary>
        /// C1 VT sequence (C1TabSet)
        /// </summary>
        C1TabSet,
        /// <summary>
        /// C1 VT sequence (C1ReverseIndex)
        /// </summary>
        C1ReverseIndex,
        /// <summary>
        /// C1 VT sequence (C1SingleShiftSelectG2CharacterSet)
        /// </summary>
        C1SingleShiftSelectG2CharacterSet,
        /// <summary>
        /// C1 VT sequence (C1SingleShiftSelectG3CharacterSet)
        /// </summary>
        C1SingleShiftSelectG3CharacterSet,
        /// <summary>
        /// C1 VT sequence (C1DeviceControlString)
        /// </summary>
        C1DeviceControlString,
        /// <summary>
        /// C1 VT sequence (C1StartOfGuardedArea)
        /// </summary>
        C1StartOfGuardedArea,
        /// <summary>
        /// C1 VT sequence (C1EndOfGuardedArea)
        /// </summary>
        C1EndOfGuardedArea,
        /// <summary>
        /// C1 VT sequence (C1StartOfString)
        /// </summary>
        C1StartOfString,
        /// <summary>
        /// C1 VT sequence (C1ReturnTerminalId)
        /// </summary>
        C1ReturnTerminalId,
        /// <summary>
        /// C1 VT sequence (C1ControlSequenceIndicator)
        /// </summary>
        C1ControlSequenceIndicator,
        /// <summary>
        /// C1 VT sequence (C1StringTerminator)
        /// </summary>
        C1StringTerminator,
        /// <summary>
        /// C1 VT sequence (C1OperatingSystemCommand)
        /// </summary>
        C1OperatingSystemCommand,
        /// <summary>
        /// C1 VT sequence (C1PrivacyMessage)
        /// </summary>
        C1PrivacyMessage,
        /// <summary>
        /// C1 VT sequence (C1ApplicationProgramCommand)
        /// </summary>
        C1ApplicationProgramCommand,
    }
}
