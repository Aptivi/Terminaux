//
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

namespace Terminaux.Sequences.Builder
{
    /// <summary>
    /// Specific VT sequences sorted by type
    /// </summary>
    public enum VtSequenceSpecificTypes
    {
        /// <summary>
        /// Application program command (Kermit)
        /// </summary>
        ApcApplicationProgramCommand,
        /// <summary>
        /// Tab set
        /// </summary>
        C1TabSet,
        /// <summary>
        /// String terminator
        /// </summary>
        C1StringTerminator,
        /// <summary>
        /// Start of string
        /// </summary>
        C1StartOfString,
        /// <summary>
        /// Start of guarded area
        /// </summary>
        C1StartOfGuardedArea,
        /// <summary>
        /// Single shift G3 character set
        /// </summary>
        C1SingleShiftSelectG3CharacterSet,
        /// <summary>
        /// Single shift G2 character set
        /// </summary>
        C1SingleShiftSelectG2CharacterSet,
        /// <summary>
        /// Reverse index
        /// </summary>
        C1ReverseIndex,
        /// <summary>
        /// Return terminal ID
        /// </summary>
        C1ReturnTerminalId,
        /// <summary>
        /// Privacy Message
        /// </summary>
        C1PrivacyMessage,
        /// <summary>
        /// Operating system command
        /// </summary>
        C1OperatingSystemCommand,
        /// <summary>
        /// Next line
        /// </summary>
        C1NextLine,
        /// <summary>
        /// Index
        /// </summary>
        C1Index,
        /// <summary>
        /// End of guarded area
        /// </summary>
        C1EndOfGuardedArea,
        /// <summary>
        /// Device control string
        /// </summary>
        C1DeviceControlString,
        /// <summary>
        /// Control sequence indicator
        /// </summary>
        C1ControlSequenceIndicator,
        /// <summary>
        /// Application program command
        /// </summary>
        C1ApplicationProgramCommand,
        /// <summary>
        /// Insert blank characters
        /// </summary>
        CsiInsertBlankCharacters,
        /// <summary>
        /// Shift left columns
        /// </summary>
        CsiShiftLeftColumns,
        /// <summary>
        /// Move cursor up
        /// </summary>
        CsiMoveCursorUp,
        /// <summary>
        /// Shift right columns
        /// </summary>
        CsiShiftRightColumns,
        /// <summary>
        /// Move cursor down
        /// </summary>
        CsiMoveCursorDown,
        /// <summary>
        /// Move cursor right
        /// </summary>
        CsiMoveCursorRight,
        /// <summary>
        /// Move cursor left
        /// </summary>
        CsiMoveCursorLeft,
        /// <summary>
        /// Move cursor to next line
        /// </summary>
        CsiMoveCursorNextLine,
        /// <summary>
        /// Move cursor to previous line
        /// </summary>
        CsiMoveCursorPreviousLine,
        /// <summary>
        /// Cursor character absolute (line)
        /// </summary>
        CsiCursorCharacterAbsoluteLine,
        /// <summary>
        /// Cursor position
        /// </summary>
        CsiCursorPosition,
        /// <summary>
        /// Cursor forward tabulation
        /// </summary>
        CsiCursorForwardTabulation,
        /// <summary>
        /// Erase in display
        /// </summary>
        CsiEraseInDisplay,
        /// <summary>
        /// Erase in display (DECSED)
        /// </summary>
        CsiEraseInDisplayDecsed,
        /// <summary>
        /// Erase in line
        /// </summary>
        CsiEraseInLine,
        /// <summary>
        /// Erase in line (DECSEL)
        /// </summary>
        CsiEraseInLineDecsel,
        /// <summary>
        /// Insert line
        /// </summary>
        CsiInsertLine,
        /// <summary>
        /// Delete line
        /// </summary>
        CsiDeleteLine,
        /// <summary>
        /// Delete characters
        /// </summary>
        CsiDeleteChars,
        /// <summary>
        /// Push color to stack
        /// </summary>
        CsiPushColorToStack,
        /// <summary>
        /// Push color to stack with arguments
        /// </summary>
        CsiPushColorToStackWithArgs,
        /// <summary>
        /// Pop color from stack
        /// </summary>
        CsiPopColorFromStack,
        /// <summary>
        /// Pop color from stack with arguments
        /// </summary>
        CsiPopColorFromStackWithArgs,
        /// <summary>
        /// Report palette stack
        /// </summary>
        CsiReportPaletteStack,
        /// <summary>
        /// Scroll up
        /// </summary>
        CsiScrollUp,
        /// <summary>
        /// Set graphics attribute
        /// </summary>
        CsiSetGraphicsAttribute,
        /// <summary>
        /// Scroll down
        /// </summary>
        CsiScrollDown,
        /// <summary>
        /// Initiate highlihgt mouse tracking
        /// </summary>
        CsiInitiateHighlightMouseTracking,
        /// <summary>
        /// Reset title mode features
        /// </summary>
        CsiResetTitleModeFeatures,
        /// <summary>
        /// Erase characters
        /// </summary>
        CsiEraseCharacters,
        /// <summary>
        /// Cursor backward tabulation
        /// </summary>
        CsiCursorBackwardTabulation,
        /// <summary>
        /// Scroll down
        /// </summary>
        CsiScrollDownOriginal,
        /// <summary>
        /// Cursor position (absolute)
        /// </summary>
        CsiCursorPositionAbsolute,
        /// <summary>
        /// Cursor position (relative)
        /// </summary>
        CsiCursorPositionRelative,
        /// <summary>
        /// Repeat graphic character
        /// </summary>
        CsiRepeatGraphicCharacter,
        /// <summary>
        /// Send device attributes (primary)
        /// </summary>
        CsiSendDeviceAttributesPrimary,
        /// <summary>
        /// Send device attributes (secondary)
        /// </summary>
        CsiSendDeviceAttributesSecondary,
        /// <summary>
        /// Send device attributes (tertiary)
        /// </summary>
        CsiSendDeviceAttributesTertiary,
        /// <summary>
        /// Line position (absolute)
        /// </summary>
        CsiLinePositionAbsolute,
        /// <summary>
        /// Line position (relative)
        /// </summary>
        CsiLinePositionRelative,
        /// <summary>
        /// Left top position
        /// </summary>
        CsiLeftTopPosition,
        /// <summary>
        /// Tab clear
        /// </summary>
        CsiTabClear,
        /// <summary>
        /// Set mode
        /// </summary>
        CsiSetMode,
        /// <summary>
        /// Set private mode
        /// </summary>
        CsiSetPrivateMode,
        /// <summary>
        /// Media copy
        /// </summary>
        CsiMediaCopy,
        /// <summary>
        /// Media copy (private)
        /// </summary>
        CsiMediaCopyPrivate,
        /// <summary>
        /// Reset mode
        /// </summary>
        CsiResetMode,
        /// <summary>
        /// Reset private mode
        /// </summary>
        CsiResetPrivateMode,
        /// <summary>
        /// Character attributes (color, bold, formatting, ...)
        /// </summary>
        CsiCharacterAttributes,
        /// <summary>
        /// Set key modifier options
        /// </summary>
        CsiSetKeyModifierOptions,
        /// <summary>
        /// Reset key modifier options
        /// </summary>
        CsiResetKeyModifierOptions,
        /// <summary>
        /// Query key modifier options
        /// </summary>
        CsiQueryKeyModifierOptions,
        /// <summary>
        /// Device status report
        /// </summary>
        CsiDeviceStatusReport,
        /// <summary>
        /// Disable key modifier options
        /// </summary>
        CsiDisableKeyModifierOptions,
        /// <summary>
        /// Device status report (DEC)
        /// </summary>
        CsiDeviceStatusReportDec,
        /// <summary>
        /// Set pointer mode for xterm
        /// </summary>
        CsiSetPointerModeXterm,
        /// <summary>
        /// Soft terminal reset
        /// </summary>
        CsiSoftTerminalReset,
        /// <summary>
        /// Set confirmance level
        /// </summary>
        CsiSetConformanceLevel,
        /// <summary>
        /// Request ANSI mode
        /// </summary>
        CsiRequestAnsiMode,
        /// <summary>
        /// Request DEC private mode
        /// </summary>
        CsiRequestDecPrivateMode,
        /// <summary>
        /// Push video attributes to stack
        /// </summary>
        CsiPushVideoAttributesToStack,
        /// <summary>
        /// Push video attributes to stack with arguments
        /// </summary>
        CsiPushVideoAttributesToStackWithArgs,
        /// <summary>
        /// Report xterm version
        /// </summary>
        CsiReportXtermVersion,
        /// <summary>
        /// Load LEDs
        /// </summary>
        CsiLoadLeds,
        /// <summary>
        /// Set cursor style
        /// </summary>
        CsiSetCursorStyle,
        /// <summary>
        /// Select character protection attribute
        /// </summary>
        CsiSelectCharacterProtectionAttribute,
        /// <summary>
        /// Pop video attributes from stack
        /// </summary>
        CsiPopVideoAttributesFromStack,
        /// <summary>
        /// Set scrolling region
        /// </summary>
        CsiSetScrollingRegion,
        /// <summary>
        /// Restore DEC private mode values
        /// </summary>
        CsiRestoreDecPrivateModeValues,
        /// <summary>
        /// Change attributes in a rectangular area
        /// </summary>
        CsiChangeAttributesInRectangularArea,
        /// <summary>
        /// Save cursor
        /// </summary>
        CsiSaveCursor,
        /// <summary>
        /// Set left right margins
        /// </summary>
        CsiSetLeftRightMargins,
        /// <summary>
        /// Set shift-escape options
        /// </summary>
        CsiSetShiftEscapeOptions,
        /// <summary>
        /// DEC private mode values
        /// </summary>
        CsiDecPrivateModeValues,
        /// <summary>
        /// Window manipulation
        /// </summary>
        CsiWindowManipulation,
        /// <summary>
        /// Set title mode for xterm
        /// </summary>
        CsiSetTitleModeXterm,
        /// <summary>
        /// Set warning bell volume
        /// </summary>
        CsiSetWarningBellVolume,
        /// <summary>
        /// Reverse attributes in rectangular area
        /// </summary>
        CsiReverseAttributesInRectangularArea,
        /// <summary>
        /// Restore cursor
        /// </summary>
        CsiRestoreCursor,
        /// <summary>
        /// Set margin bell volume
        /// </summary>
        CsiSetMarginBellVolume,
        /// <summary>
        /// Copy rectangular area
        /// </summary>
        CsiCopyRectangularArea,
        /// <summary>
        /// Request presentation state report
        /// </summary>
        CsiRequestPresentationStateReport,
        /// <summary>
        /// Enable filter rectangle
        /// </summary>
        CsiEnableFilterRectangle,
        /// <summary>
        /// Request terminal parameters
        /// </summary>
        CsiRequestTerminalParameters,
        /// <summary>
        /// Select attribute change extent
        /// </summary>
        CsiSelectAttributeChangeExtent,
        /// <summary>
        /// Fill a rectuangular area
        /// </summary>
        CsiFillRectangularArea,
        /// <summary>
        /// Select checksum extension
        /// </summary>
        CsiSelectChecksumExtension,
        /// <summary>
        /// Checksum of a rectangular area
        /// </summary>
        CsiRectangularAreaChecksum,
        /// <summary>
        /// Locator reporting
        /// </summary>
        CsiLocatorReporting,
        /// <summary>
        /// Erase a rectangular area
        /// </summary>
        CsiEraseRectangularArea,
        /// <summary>
        /// Select locator events
        /// </summary>
        CsiSelectLocatorEvents,
        /// <summary>
        /// Push video attributes to stack for xterm
        /// </summary>
        CsiPushVideoAttributesToStackXterm,
        /// <summary>
        /// Push video attributes to stack for xterm with arguments
        /// </summary>
        CsiPushVideoAttributesToStackXtermWithArgs,
        /// <summary>
        /// Selectively erase a rectangular area
        /// </summary>
        CsiSelectiveEraseRectangularArea,
        /// <summary>
        /// Report graphics rendition for a rectangular area
        /// </summary>
        CsiReportGraphicsRenditionRectangularArea,
        /// <summary>
        /// Select columns per page
        /// </summary>
        CsiSelectColumnsPerPage,
        /// <summary>
        /// Request locator position
        /// </summary>
        CsiRequestLocatorPosition,
        /// <summary>
        /// Select number of lines per screen
        /// </summary>
        CsiSelectNumberOfLinesPerScreen,
        /// <summary>
        /// Pop video attributes from stack for xterm
        /// </summary>
        CsiPopVideoAttributesFromStackXterm,
        /// <summary>
        /// Insert columns
        /// </summary>
        CsiInsertColumns,
        /// <summary>
        /// Select active status display
        /// </summary>
        CsiSelectActiveStatusDisplay,
        /// <summary>
        /// Delete columns
        /// </summary>
        CsiDeleteColumns,
        /// <summary>
        /// Select status line type
        /// </summary>
        CsiSelectStatusLineType,
        /// <summary>
        /// User defined keys
        /// </summary>
        DcsUserDefinedKeys,
        /// <summary>
        /// Request status string
        /// </summary>
        DcsRequestStatusString,
        /// <summary>
        /// Restore presentation status
        /// </summary>
        DcsRestorePresentationStatus,
        /// <summary>
        /// Request resource values
        /// </summary>
        DcsRequestResourceValues,
        /// <summary>
        /// Set terminfo data
        /// </summary>
        DcsSetTermInfoData,
        /// <summary>
        /// Request termino data
        /// </summary>
        DcsRequestTermInfoData,
        /// <summary>
        /// 7-bit controls
        /// </summary>
        Esc7BitControls,
        /// <summary>
        /// 8-bit controls
        /// </summary>
        Esc8BitControls,
        /// <summary>
        /// ANSI conformance level 1
        /// </summary>
        EscAnsiConformanceLevel1,
        /// <summary>
        /// ANSI conformance level 2
        /// </summary>
        EscAnsiConformanceLevel2,
        /// <summary>
        /// ANSI conformance level 3
        /// </summary>
        EscAnsiConformanceLevel3,
        /// <summary>
        /// Double height line top half (DEC)
        /// </summary>
        EscDecDoubleHeightLineTopHalf,
        /// <summary>
        /// Double height line bottom half (DEC)
        /// </summary>
        EscDecDoubleHeightLineBottomHalf,
        /// <summary>
        /// Single width line (DEC)
        /// </summary>
        EscDecSingleWidthLine,
        /// <summary>
        /// Double width line (DEC)
        /// </summary>
        EscDecDoubleWidthLine,
        /// <summary>
        /// Screen alignment test (DEC)
        /// </summary>
        EscDecScreenAlignmentTest,
        /// <summary>
        /// Select default character set
        /// </summary>
        EscSelectDefaultCharacterSet,
        /// <summary>
        /// Select UTF8 character set
        /// </summary>
        EscSelectUtf8CharacterSet,
        /// <summary>
        /// Designate the G0 character set
        /// </summary>
        EscDesignateG0CharacterSet,
        /// <summary>
        /// Designate the G1 character set
        /// </summary>
        EscDesignateG1CharacterSet,
        /// <summary>
        /// Designate the G2 character set
        /// </summary>
        EscDesignateG2CharacterSet,
        /// <summary>
        /// Designate the G3 character set
        /// </summary>
        EscDesignateG3CharacterSet,
        /// <summary>
        /// Designate the G1 character set (alt)
        /// </summary>
        EscDesignateG1CharacterSetAlt,
        /// <summary>
        /// Designate the G2 character set (alt)
        /// </summary>
        EscDesignateG2CharacterSetAlt,
        /// <summary>
        /// Designate the G3 character set (alt)
        /// </summary>
        EscDesignateG3CharacterSetAlt,
        /// <summary>
        /// back index
        /// </summary>
        EscBackIndex,
        /// <summary>
        /// Save cursor
        /// </summary>
        EscSaveCursor,
        /// <summary>
        /// Restore cursor
        /// </summary>
        EscRestoreCursor,
        /// <summary>
        /// Forward index
        /// </summary>
        EscForwardIndex,
        /// <summary>
        /// Application keypad
        /// </summary>
        EscApplicationKeypad,
        /// <summary>
        /// Normal keypad
        /// </summary>
        EscNormalKeypad,
        /// <summary>
        /// Cursor to lower left corner
        /// </summary>
        EscCursorToLowerLeftCorner,
        /// <summary>
        /// Full reset
        /// </summary>
        EscFullReset,
        /// <summary>
        /// Memory lock
        /// </summary>
        EscMemoryLock,
        /// <summary>
        /// Memory unlock
        /// </summary>
        EscMemoryUnlock,
        /// <summary>
        /// Invoke G2 character set in GL mode
        /// </summary>
        EscInvokeG2CharacterSetGl,
        /// <summary>
        /// Invoke G3 character set in GL mode
        /// </summary>
        EscInvokeG3CharacterSetGl,
        /// <summary>
        /// Invoke G3 character set in GR mode
        /// </summary>
        EscInvokeG3CharacterSetGr,
        /// <summary>
        /// Invoke G2 character set in GR mode
        /// </summary>
        EscInvokeG2CharacterSetGr,
        /// <summary>
        /// Invoke G1 character set in GR mode
        /// </summary>
        EscInvokeG1CharacterSetGr,
        /// <summary>
        /// Operating system command sequence
        /// </summary>
        OscOperatingSystemCommand,
        /// <summary>
        /// Operating system command sequence (alternative)
        /// </summary>
        OscOperatingSystemCommandAlt,
        /// <summary>
        /// Privacy message command sequence (Kermit)
        /// </summary>
        PmPrivacyMessage,
    }
}
