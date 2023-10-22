
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
using System.Text.RegularExpressions;

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of CSI sequences and their regular expressions
    /// </summary>
    public static class CsiSequences
    {
        private static readonly Regex csiInsertBlankCharactersSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[@]", RegexOptions.Compiled);
        private static readonly Regex csiShiftLeftColumnsSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[ ][@]", RegexOptions.Compiled);
        private static readonly Regex csiMoveCursorUpSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[A]", RegexOptions.Compiled);
        private static readonly Regex csiShiftRightColumnsSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[ ][A]", RegexOptions.Compiled);
        private static readonly Regex csiMoveCursorDownSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[B]", RegexOptions.Compiled);
        private static readonly Regex csiMoveCursorRightSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[C]", RegexOptions.Compiled);
        private static readonly Regex csiMoveCursorLeftSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[D]", RegexOptions.Compiled);
        private static readonly Regex csiMoveCursorNextLineSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[E]", RegexOptions.Compiled);
        private static readonly Regex csiMoveCursorPreviousLineSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[F]", RegexOptions.Compiled);
        private static readonly Regex csiCursorCharacterAbsoluteLineSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[G]", RegexOptions.Compiled);
        private static readonly Regex csiCursorPositionSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[;][0-9]*[H]", RegexOptions.Compiled);
        private static readonly Regex csiCursorForwardTabulationSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[I]", RegexOptions.Compiled);
        private static readonly Regex csiEraseInDisplaySequenceRegex = new(@"(\x9B|\x1B\[)[0-3]*[J]", RegexOptions.Compiled);
        private static readonly Regex csiEraseInDisplayDecsedSequenceRegex = new(@"(\x9B|\x1B\[)\?[0-3]*[J]", RegexOptions.Compiled);
        private static readonly Regex csiEraseInLineSequenceRegex = new(@"(\x9B|\x1B\[)[0-3]*[K]", RegexOptions.Compiled);
        private static readonly Regex csiEraseInLineDecselSequenceRegex = new(@"(\x9B|\x1B\[)\?[0-3]*[K]", RegexOptions.Compiled);
        private static readonly Regex csiInsertLineSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[L]", RegexOptions.Compiled);
        private static readonly Regex csiDeleteLineSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[M]", RegexOptions.Compiled);
        private static readonly Regex csiDeleteCharsSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[P]", RegexOptions.Compiled);
        private static readonly Regex csiPushColorToStackSequenceRegex = new(@"(\x9B|\x1B\[)#[P]", RegexOptions.Compiled);
        private static readonly Regex csiPushColorToStackWithArgsSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)#[P]", RegexOptions.Compiled);
        private static readonly Regex csiPopColorFromStackSequenceRegex = new(@"(\x9B|\x1B\[)#[Q]", RegexOptions.Compiled);
        private static readonly Regex csiPopColorFromStackWithArgsSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)#[Q]", RegexOptions.Compiled);
        private static readonly Regex csiReportPaletteStackSequenceRegex = new(@"(\x9B|\x1B\[)#[R]", RegexOptions.Compiled);
        private static readonly Regex csiScrollUpSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[S]", RegexOptions.Compiled);
        private static readonly Regex csiSetGraphicsAttributeSequenceRegex = new(@"(\x9B|\x1B\[)\?[0-9]*\;[0-9]*\;(.+?)*[S]", RegexOptions.Compiled);
        private static readonly Regex csiScrollDownSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[T]", RegexOptions.Compiled);
        private static readonly Regex csiInitiateHighlightMouseTrackingSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[T]", RegexOptions.Compiled);
        private static readonly Regex csiResetTitleModeFeaturesSequenceRegex = new(@"(\x9B|\x1B\[)\>(.+?)[T]", RegexOptions.Compiled);
        private static readonly Regex csiEraseCharactersSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[X]", RegexOptions.Compiled);
        private static readonly Regex csiCursorBackwardTabulationSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[Z]", RegexOptions.Compiled);
        private static readonly Regex csiScrollDownOriginalSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[\^]", RegexOptions.Compiled);
        private static readonly Regex csiCursorPositionAbsoluteSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[`]", RegexOptions.Compiled);
        private static readonly Regex csiCursorPositionRelativeSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[a]", RegexOptions.Compiled);
        private static readonly Regex csiRepeatGraphicCharacterSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[b]", RegexOptions.Compiled);
        private static readonly Regex csiSendDeviceAttributesPrimarySequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[c]", RegexOptions.Compiled);
        private static readonly Regex csiSendDeviceAttributesSecondarySequenceRegex = new(@"(\x9B|\x1B\[)=[0-9]*[c]", RegexOptions.Compiled);
        private static readonly Regex csiSendDeviceAttributesTertiarySequenceRegex = new(@"(\x9B|\x1B\[)\>[0-9]*[c]", RegexOptions.Compiled);
        private static readonly Regex csiLinePositionAbsoluteSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[d]", RegexOptions.Compiled);
        private static readonly Regex csiLinePositionRelativeSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[e]", RegexOptions.Compiled);
        private static readonly Regex csiLeftTopPositionSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*[f]", RegexOptions.Compiled);
        private static readonly Regex csiTabClearSequenceRegex = new(@"(\x9B|\x1B\[)[03][g]", RegexOptions.Compiled);
        private static readonly Regex csiSetModeSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)[h]", RegexOptions.Compiled);
        private static readonly Regex csiSetPrivateModeSequenceRegex = new(@"(\x9B|\x1B\[)\?(.+?)[h]", RegexOptions.Compiled);
        private static readonly Regex csiMediaCopySequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[i]", RegexOptions.Compiled);
        private static readonly Regex csiMediaCopyPrivateSequenceRegex = new(@"(\x9B|\x1B\[)\?[0-9]*[i]", RegexOptions.Compiled);
        private static readonly Regex csiResetModeSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)[l]", RegexOptions.Compiled);
        private static readonly Regex csiResetPrivateModeSequenceRegex = new(@"(\x9B|\x1B\[)\?(.+?)[l]", RegexOptions.Compiled);
        private static readonly Regex csiCharacterAttributesSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)[m]", RegexOptions.Compiled);
        private static readonly Regex csiSetKeyModifierOptionsSequenceRegex = new(@"(\x9B|\x1B\[)\>[0-9]*\;[0-9]*[m]", RegexOptions.Compiled);
        private static readonly Regex csiResetKeyModifierOptionsSequenceRegex = new(@"(\x9B|\x1B\[)\>[0-9]*[m]", RegexOptions.Compiled);
        private static readonly Regex csiQueryKeyModifierOptionsSequenceRegex = new(@"(\x9B|\x1B\[)\?[0-9]*[m]", RegexOptions.Compiled);
        private static readonly Regex csiDeviceStatusReportSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[n]", RegexOptions.Compiled);
        private static readonly Regex csiDisableKeyModifierOptionsSequenceRegex = new(@"(\x9B|\x1B\[)\>[0-9]*[n]", RegexOptions.Compiled);
        private static readonly Regex csiDeviceStatusReportDecSequenceRegex = new(@"(\x9B|\x1B\[)\?[0-9]*[n]", RegexOptions.Compiled);
        private static readonly Regex csiSetPointerModeXtermSequenceRegex = new(@"(\x9B|\x1B\[)\>[0-9]*[p]", RegexOptions.Compiled);
        private static readonly Regex csiSoftTerminalResetSequenceRegex = new(@"(\x9B|\x1B\[)![p]", RegexOptions.Compiled);
        private static readonly Regex csiSetConformanceLevelSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*[""]p", RegexOptions.Compiled);
        private static readonly Regex csiRequestAnsiModeSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[$]p", RegexOptions.Compiled);
        private static readonly Regex csiRequestDecPrivateModeSequenceRegex = new(@"(\x9B|\x1B\[)\?[0-9]*[$]p", RegexOptions.Compiled);
        private static readonly Regex csiPushVideoAttributesToStackSequenceRegex = new(@"(\x9B|\x1B\[)#[p]", RegexOptions.Compiled);
        private static readonly Regex csiPushVideoAttributesToStackWithArgsSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)#[p]", RegexOptions.Compiled);
        private static readonly Regex csiReportXtermVersionSequenceRegex = new(@"(\x9B|\x1B\[)\>[0-9]*[q]", RegexOptions.Compiled);
        private static readonly Regex csiLoadLedsSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[q]", RegexOptions.Compiled);
        private static readonly Regex csiSetCursorStyleSequenceRegex = new(@"(\x9B|\x1B\[)[0-6]* [q]", RegexOptions.Compiled);
        private static readonly Regex csiSelectCharacterProtectionAttributeSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[""]q", RegexOptions.Compiled);
        private static readonly Regex csiPopVideoAttributesFromStackSequenceRegex = new(@"(\x9B|\x1B\[)#[q]", RegexOptions.Compiled);
        private static readonly Regex csiSetScrollingRegionSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*[r]", RegexOptions.Compiled);
        private static readonly Regex csiRestoreDecPrivateModeValuesSequenceRegex = new(@"(\x9B|\x1B\[)\?(.+?)[r]", RegexOptions.Compiled);
        private static readonly Regex csiChangeAttributesInRectangularAreaSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;(.+?)[$]r", RegexOptions.Compiled);
        private static readonly Regex csiSaveCursorSequenceRegex = new(@"(\x9B|\x1B\[)s", RegexOptions.Compiled);
        private static readonly Regex csiSetLeftRightMarginsSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*[s]", RegexOptions.Compiled);
        private static readonly Regex csiSetShiftEscapeOptionsSequenceRegex = new(@"(\x9B|\x1B\[)\>[0-9]*[s]", RegexOptions.Compiled);
        private static readonly Regex csiDecPrivateModeValuesSequenceRegex = new(@"(\x9B|\x1B\[)\?(.+?)[s]", RegexOptions.Compiled);
        private static readonly Regex csiWindowManipulationSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*[t]", RegexOptions.Compiled);
        private static readonly Regex csiSetTitleModeXtermSequenceRegex = new(@"(\x9B|\x1B\[)\>(.+?)[t]", RegexOptions.Compiled);
        private static readonly Regex csiSetWarningBellVolumeSequenceRegex = new(@"(\x9B|\x1B\[)[0-8]* [t]", RegexOptions.Compiled);
        private static readonly Regex csiReverseAttributesInRectangularAreaSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;(.+?)[$]t", RegexOptions.Compiled);
        private static readonly Regex csiRestoreCursorSequenceRegex = new(@"(\x9B|\x1B\[)u", RegexOptions.Compiled);
        private static readonly Regex csiSetMarginBellVolumeSequenceRegex = new(@"(\x9B|\x1B\[)[0-8] [u]", RegexOptions.Compiled);
        private static readonly Regex csiCopyRectangularAreaSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]v", RegexOptions.Compiled);
        private static readonly Regex csiRequestPresentationStateReportSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[$]w", RegexOptions.Compiled);
        private static readonly Regex csiEnableFilterRectangleSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[']w", RegexOptions.Compiled);
        private static readonly Regex csiRequestTerminalParametersSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[x]", RegexOptions.Compiled);
        private static readonly Regex csiSelectAttributeChangeExtentSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[\*]x", RegexOptions.Compiled);
        private static readonly Regex csiFillRectangularAreaSequenceRegex = new(@"(\x9B|\x1B\[)\D+\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]x", RegexOptions.Compiled);
        private static readonly Regex csiSelectChecksumExtensionSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[#]y", RegexOptions.Compiled);
        private static readonly Regex csiRectangularAreaChecksumSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[\*]y", RegexOptions.Compiled);
        private static readonly Regex csiLocatorReportingSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*[']z", RegexOptions.Compiled);
        private static readonly Regex csiEraseRectangularAreaSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]z", RegexOptions.Compiled);
        private static readonly Regex csiSelectLocatorEventsSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)[']\{", RegexOptions.Compiled);
        private static readonly Regex csiPushVideoAttributesToStackXtermSequenceRegex = new(@"(\x9B|\x1B\[)#[\{]", RegexOptions.Compiled);
        private static readonly Regex csiPushVideoAttributesToStackXtermWithArgsSequenceRegex = new(@"(\x9B|\x1B\[)(.+?)#[\{]", RegexOptions.Compiled);
        private static readonly Regex csiSelectiveEraseRectangularAreaSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]\{", RegexOptions.Compiled);
        private static readonly Regex csiReportGraphicsRenditionRectangularAreaSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[#]\|", RegexOptions.Compiled);
        private static readonly Regex csiSelectColumnsPerPageSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[$]\|", RegexOptions.Compiled);
        private static readonly Regex csiRequestLocatorPositionSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[']\|", RegexOptions.Compiled);
        private static readonly Regex csiSelectNumberOfLinesPerScreenSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[\*]\|", RegexOptions.Compiled);
        private static readonly Regex csiPopVideoAttributesFromStackXtermSequenceRegex = new(@"(\x9B|\x1B\[)[#]\}", RegexOptions.Compiled);
        private static readonly Regex csiInsertColumnsSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[']\}", RegexOptions.Compiled);
        private static readonly Regex csiSelectActiveStatusDisplaySequenceRegex = new(@"(\x9B|\x1B\[)[01]*[$]\}", RegexOptions.Compiled);
        private static readonly Regex csiDeleteColumnsSequenceRegex = new(@"(\x9B|\x1B\[)[0-9]*[']~", RegexOptions.Compiled);
        private static readonly Regex csiSelectStatusLineTypeSequenceRegex = new(@"(\x9B|\x1B\[)[0-2]*[$]~", RegexOptions.Compiled);

        /// <summary>
        /// [CSI Ps @] Regular expression for inserting the blank characters Ps times
        /// </summary>
        public static Regex CsiInsertBlankCharactersSequenceRegex =>
            csiInsertBlankCharactersSequenceRegex;

        /// <summary>
        /// [CSI Ps SP @] Regular expression for shifting left Ps columns
        /// </summary>
        public static Regex CsiShiftLeftColumnsSequenceRegex =>
            csiShiftLeftColumnsSequenceRegex;

        /// <summary>
        /// [CSI Ps A] Regular expression for moving the cursor up Ps times
        /// </summary>
        public static Regex CsiMoveCursorUpSequenceRegex =>
            csiMoveCursorUpSequenceRegex;

        /// <summary>
        /// [CSI Ps SP A] Regular expression for shifting right Ps columns
        /// </summary>
        public static Regex CsiShiftRightColumnsSequenceRegex =>
            csiShiftRightColumnsSequenceRegex;

        /// <summary>
        /// [CSI Ps B] Regular expression for moving the cursor down Ps times
        /// </summary>
        public static Regex CsiMoveCursorDownSequenceRegex =>
            csiMoveCursorDownSequenceRegex;

        /// <summary>
        /// [CSI Ps C] Regular expression for moving the cursor to the right Ps times
        /// </summary>
        public static Regex CsiMoveCursorRightSequenceRegex =>
            csiMoveCursorRightSequenceRegex;

        /// <summary>
        /// [CSI Ps D] Regular expression for moving the cursor to the left Ps times
        /// </summary>
        public static Regex CsiMoveCursorLeftSequenceRegex =>
            csiMoveCursorLeftSequenceRegex;

        /// <summary>
        /// [CSI Ps E] Regular expression for moving the cursor to the next line Ps times
        /// </summary>
        public static Regex CsiMoveCursorNextLineSequenceRegex =>
            csiMoveCursorNextLineSequenceRegex;

        /// <summary>
        /// [CSI Ps F] Regular expression for moving the cursor to the previous line Ps times
        /// </summary>
        public static Regex CsiMoveCursorPreviousLineSequenceRegex =>
            csiMoveCursorPreviousLineSequenceRegex;

        /// <summary>
        /// [CSI Ps G] Regular expression for cursor character absolute
        /// </summary>
        public static Regex CsiCursorCharacterAbsoluteLineSequenceRegex =>
            csiCursorCharacterAbsoluteLineSequenceRegex;

        /// <summary>
        /// [CSI Ps ; Ps H] Regular expression for cursor position (Ps column ; Ps row)
        /// </summary>
        public static Regex CsiCursorPositionSequenceRegex =>
            csiCursorPositionSequenceRegex;

        /// <summary>
        /// [CSI Ps I] Regular expression for cursor forward tabulation Ps tab stops
        /// </summary>
        public static Regex CsiCursorForwardTabulationSequenceRegex =>
            csiCursorForwardTabulationSequenceRegex;

        /// <summary>
        /// [CSI Ps J] Regular expression for erasing in display (ED)
        /// </summary>
        public static Regex CsiEraseInDisplaySequenceRegex =>
            csiEraseInDisplaySequenceRegex;

        /// <summary>
        /// [CSI ? Ps J] Regular expression for erasing in display (DECSED)
        /// </summary>
        public static Regex CsiEraseInDisplayDecsedSequenceRegex =>
            csiEraseInDisplayDecsedSequenceRegex;

        /// <summary>
        /// [CSI Ps K] Regular expression for erasing in line (EL)
        /// </summary>
        public static Regex CsiEraseInLineSequenceRegex =>
            csiEraseInLineSequenceRegex;

        /// <summary>
        /// [CSI ? Ps K] Regular expression for erasing in line (DECSEL)
        /// </summary>
        public static Regex CsiEraseInLineDecselSequenceRegex =>
            csiEraseInLineDecselSequenceRegex;

        /// <summary>
        /// [CSI Ps L] Regular expression for inserting Ps lines
        /// </summary>
        public static Regex CsiInsertLineSequenceRegex =>
            csiInsertLineSequenceRegex;

        /// <summary>
        /// [CSI Ps M] Regular expression for deleting Ps lines
        /// </summary>
        public static Regex CsiDeleteLineSequenceRegex =>
            csiDeleteLineSequenceRegex;

        /// <summary>
        /// [CSI Ps P] Regular expression for deleting Ps characters
        /// </summary>
        public static Regex CsiDeleteCharsSequenceRegex =>
            csiDeleteCharsSequenceRegex;

        /// <summary>
        /// [CSI # P] Regular expression for pushing color into the stack
        /// </summary>
        public static Regex CsiPushColorToStackSequenceRegex =>
            csiPushColorToStackSequenceRegex;

        /// <summary>
        /// [CSI Pm # P] Regular expression for pushing color into the stack
        /// </summary>
        public static Regex CsiPushColorToStackWithArgsSequenceRegex =>
            csiPushColorToStackWithArgsSequenceRegex;

        /// <summary>
        /// [CSI # Q] Regular expression for popping color from the stack
        /// </summary>
        public static Regex CsiPopColorFromStackSequenceRegex =>
            csiPopColorFromStackSequenceRegex;

        /// <summary>
        /// [CSI Pm # Q] Regular expression for popping color from the stack
        /// </summary>
        public static Regex CsiPopColorFromStackWithArgsSequenceRegex =>
            csiPopColorFromStackWithArgsSequenceRegex;

        /// <summary>
        /// [CSI # R] Regular expression for reporting the palette stack
        /// </summary>
        public static Regex CsiReportPaletteStackSequenceRegex =>
            csiReportPaletteStackSequenceRegex;

        /// <summary>
        /// [CSI Ps S] Regular expression for scrolling up Ps lines
        /// </summary>
        public static Regex CsiScrollUpSequenceRegex =>
            csiScrollUpSequenceRegex;

        /// <summary>
        /// [CSI ? Pi ; Pa ; Pv S] Regular expression for setting graphics attribute
        /// </summary>
        public static Regex CsiSetGraphicsAttributeSequenceRegex =>
            csiSetGraphicsAttributeSequenceRegex;

        /// <summary>
        /// [CSI Ps T] Regular expression for scrolling down Ps lines
        /// </summary>
        public static Regex CsiScrollDownSequenceRegex =>
            csiScrollDownSequenceRegex;

        /// <summary>
        /// [CSI Ps ; Ps ; Ps ; Ps ; Ps T] Regular expression for initiating highlight mouse tracking
        /// </summary>
        public static Regex CsiInitiateHighlightMouseTrackingSequenceRegex =>
            csiInitiateHighlightMouseTrackingSequenceRegex;

        /// <summary>
        /// [CSI > Pm T] Regular expression for resetting title mode features
        /// </summary>
        public static Regex CsiResetTitleModeFeaturesSequenceRegex =>
            csiResetTitleModeFeaturesSequenceRegex;

        /// <summary>
        /// [CSI Ps X] Regular expression for erasing Ps characters
        /// </summary>
        public static Regex CsiEraseCharactersSequenceRegex =>
            csiEraseCharactersSequenceRegex;

        /// <summary>
        /// [CSI Ps Z] Regular expression for cursor backward tabulation Ps tab stops
        /// </summary>
        public static Regex CsiCursorBackwardTabulationSequenceRegex =>
            csiCursorBackwardTabulationSequenceRegex;

        /// <summary>
        /// [CSI Ps ^] Regular expression for scrolling down Ps lines
        /// </summary>
        public static Regex CsiScrollDownOriginalSequenceRegex =>
            csiScrollDownOriginalSequenceRegex;

        /// <summary>
        /// [CSI Ps `] Regular expression for cursor position (absolute)
        /// </summary>
        public static Regex CsiCursorPositionAbsoluteSequenceRegex =>
            csiCursorPositionAbsoluteSequenceRegex;

        /// <summary>
        /// [CSI Ps a] Regular expression for cursor position (relative)
        /// </summary>
        public static Regex CsiCursorPositionRelativeSequenceRegex =>
            csiCursorPositionRelativeSequenceRegex;

        /// <summary>
        /// [CSI Ps b] Regular expression for repeating a graphic character
        /// </summary>
        public static Regex CsiRepeatGraphicCharacterSequenceRegex =>
            csiRepeatGraphicCharacterSequenceRegex;

        /// <summary>
        /// [CSI Ps c] Regular expression for sending device attributes (Primary DA)
        /// </summary>
        public static Regex CsiSendDeviceAttributesPrimarySequenceRegex =>
            csiSendDeviceAttributesPrimarySequenceRegex;

        /// <summary>
        /// [CSI = Ps c] Regular expression for sending device attributes (Secondary DA)
        /// </summary>
        public static Regex CsiSendDeviceAttributesSecondarySequenceRegex =>
            csiSendDeviceAttributesSecondarySequenceRegex;

        /// <summary>
        /// [CSI > Ps c] Regular expression for sending device attributes (Tertiary DA)
        /// </summary>
        public static Regex CsiSendDeviceAttributesTertiarySequenceRegex =>
            csiSendDeviceAttributesTertiarySequenceRegex;

        /// <summary>
        /// [CSI Ps d] Regular expression for line position (absolute)
        /// </summary>
        public static Regex CsiLinePositionAbsoluteSequenceRegex =>
            csiLinePositionAbsoluteSequenceRegex;

        /// <summary>
        /// [CSI Ps e] Regular expression for line position (relative)
        /// </summary>
        public static Regex CsiLinePositionRelativeSequenceRegex =>
            csiLinePositionRelativeSequenceRegex;

        /// <summary>
        /// [CSI Ps ; Ps f] Regular expression for horizontal and vertical position
        /// </summary>
        public static Regex CsiLeftTopPositionSequenceRegex =>
            csiLeftTopPositionSequenceRegex;

        /// <summary>
        /// [CSI Ps g] Regular expression for tab clear
        /// </summary>
        public static Regex CsiTabClearSequenceRegex =>
            csiTabClearSequenceRegex;

        /// <summary>
        /// [CSI Pm h] Regular expression for setting mode
        /// </summary>
        public static Regex CsiSetModeSequenceRegex =>
            csiSetModeSequenceRegex;

        /// <summary>
        /// [CSI ? Pm h] Regular expression for setting mode (Private mode set)
        /// </summary>
        public static Regex CsiSetPrivateModeSequenceRegex =>
            csiSetPrivateModeSequenceRegex;

        /// <summary>
        /// [CSI Ps i] Regular expression for media copy
        /// </summary>
        public static Regex CsiMediaCopySequenceRegex =>
            csiMediaCopySequenceRegex;

        /// <summary>
        /// [CSI ? Ps i] Regular expression for media copy (Private)
        /// </summary>
        public static Regex CsiMediaCopyPrivateSequenceRegex =>
            csiMediaCopyPrivateSequenceRegex;

        /// <summary>
        /// [CSI Pm l] Regular expression for reset mode
        /// </summary>
        public static Regex CsiResetModeSequenceRegex =>
            csiResetModeSequenceRegex;

        /// <summary>
        /// [CSI ? Pm l] Regular expression for reset mode (Private mode reset)
        /// </summary>
        public static Regex CsiResetPrivateModeSequenceRegex =>
            csiResetPrivateModeSequenceRegex;

        /// <summary>
        /// [CSI Pm m] Regular expression for character attributes
        /// </summary>
        public static Regex CsiCharacterAttributesSequenceRegex =>
            csiCharacterAttributesSequenceRegex;

        /// <summary>
        /// [CSI > Pp ; Pv m] Regular expression for setting key modifier options
        /// </summary>
        public static Regex CsiSetKeyModifierOptionsSequenceRegex =>
            csiSetKeyModifierOptionsSequenceRegex;

        /// <summary>
        /// [CSI > Pp m] Regular expression for resetting key modifier options
        /// </summary>
        public static Regex CsiResetKeyModifierOptionsSequenceRegex =>
            csiResetKeyModifierOptionsSequenceRegex;

        /// <summary>
        /// [CSI > Pp m] Regular expression for querying key modifier options
        /// </summary>
        public static Regex CsiQueryKeyModifierOptionsSequenceRegex =>
            csiQueryKeyModifierOptionsSequenceRegex;

        /// <summary>
        /// [CSI Ps n] Regular expression for device status report
        /// </summary>
        public static Regex CsiDeviceStatusReportSequenceRegex =>
            csiDeviceStatusReportSequenceRegex;

        /// <summary>
        /// [CSI > Ps n] Regular expression for disabling key modifier options
        /// </summary>
        public static Regex CsiDisableKeyModifierOptionsSequenceRegex =>
            csiDisableKeyModifierOptionsSequenceRegex;

        /// <summary>
        /// [CSI ? Ps n] Regular expression for device status report (DEC-specific)
        /// </summary>
        public static Regex CsiDeviceStatusReportDecSequenceRegex =>
            csiDeviceStatusReportDecSequenceRegex;

        /// <summary>
        /// [CSI > Ps p] Regular expression for setting pointerMode for xterm
        /// </summary>
        public static Regex CsiSetPointerModeXtermSequenceRegex =>
            csiSetPointerModeXtermSequenceRegex;

        /// <summary>
        /// [CSI ! p] Regular expression for soft terminal reset
        /// </summary>
        public static Regex CsiSoftTerminalResetSequenceRegex =>
            csiSoftTerminalResetSequenceRegex;

        /// <summary>
        /// [CSI Pl ; Pc " p] Regular expression for setting conformance level
        /// </summary>
        public static Regex CsiSetConformanceLevelSequenceRegex =>
            csiSetConformanceLevelSequenceRegex;

        /// <summary>
        /// [CSI Ps $ p] Regular expression for requesting ANSI mode
        /// </summary>
        public static Regex CsiRequestAnsiModeSequenceRegex =>
            csiRequestAnsiModeSequenceRegex;

        /// <summary>
        /// [CSI ? Ps $ p] Regular expression for requesting DEC private mode
        /// </summary>
        public static Regex CsiRequestDecPrivateModeSequenceRegex =>
            csiRequestDecPrivateModeSequenceRegex;

        /// <summary>
        /// [CSI # p] Regular expression for pushing video attributes into the stack
        /// </summary>
        public static Regex CsiPushVideoAttributesToStackSequenceRegex =>
            csiPushVideoAttributesToStackSequenceRegex;

        /// <summary>
        /// [CSI Pm # p] Regular expression for pushing video attributes into the stack
        /// </summary>
        public static Regex CsiPushVideoAttributesToStackWithArgsSequenceRegex =>
            csiPushVideoAttributesToStackWithArgsSequenceRegex;

        /// <summary>
        /// [CSI > Ps q] Regular expression for reporting the xterm version
        /// </summary>
        public static Regex CsiReportXtermVersionSequenceRegex =>
            csiReportXtermVersionSequenceRegex;

        /// <summary>
        /// [CSI Ps q] Regular expression for loading LEDs
        /// </summary>
        public static Regex CsiLoadLedsSequenceRegex =>
            csiLoadLedsSequenceRegex;

        /// <summary>
        /// [CSI Ps SP q] Regular expression for setting cursor style
        /// </summary>
        public static Regex CsiSetCursorStyleSequenceRegex =>
            csiSetCursorStyleSequenceRegex;

        /// <summary>
        /// [CSI Ps " q] Regular expression for selecting character protection attribute
        /// </summary>
        public static Regex CsiSelectCharacterProtectionAttributeSequenceRegex =>
            csiSelectCharacterProtectionAttributeSequenceRegex;

        /// <summary>
        /// [CSI # q] Regular expression for popping video attributes from the stack
        /// </summary>
        public static Regex CsiPopVideoAttributesFromStackSequenceRegex =>
            csiPopVideoAttributesFromStackSequenceRegex;

        /// <summary>
        /// [CSI Ps ; Ps r] Regular expression for setting scroll region
        /// </summary>
        public static Regex CsiSetScrollingRegionSequenceRegex =>
            csiSetScrollingRegionSequenceRegex;

        /// <summary>
        /// [CSI ? Pm r] Regular expression for restoring DEC private mode values
        /// </summary>
        public static Regex CsiRestoreDecPrivateModeValuesSequenceRegex =>
            csiRestoreDecPrivateModeValuesSequenceRegex;

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ r] Regular expression for changing attributes in rectangular area
        /// </summary>
        public static Regex CsiChangeAttributesInRectangularAreaSequenceRegex =>
            csiChangeAttributesInRectangularAreaSequenceRegex;

        /// <summary>
        /// [CSI s] Regular expression for saving the cursor
        /// </summary>
        public static Regex CsiSaveCursorSequenceRegex =>
            csiSaveCursorSequenceRegex;

        /// <summary>
        /// [CSI Pl ; Pr s] Regular expression for setting left and right margins
        /// </summary>
        public static Regex CsiSetLeftRightMarginsSequenceRegex =>
            csiSetLeftRightMarginsSequenceRegex;

        /// <summary>
        /// [CSI > Ps s] Regular expression for setting shift-escape options
        /// </summary>
        public static Regex CsiSetShiftEscapeOptionsSequenceRegex =>
            csiSetShiftEscapeOptionsSequenceRegex;

        /// <summary>
        /// [CSI ? Pm s] Regular expression for saving DEC private mode values
        /// </summary>
        public static Regex CsiDecPrivateModeValuesSequenceRegex =>
            csiDecPrivateModeValuesSequenceRegex;

        /// <summary>
        /// [CSI Ps ; Ps ; Ps t] Regular expression for window manipulation
        /// </summary>
        public static Regex CsiWindowManipulationSequenceRegex =>
            csiWindowManipulationSequenceRegex;

        /// <summary>
        /// [CSI > Pm t] Regular expression for setting title mode for xterm
        /// </summary>
        public static Regex CsiSetTitleModeXtermSequenceRegex =>
            csiSetTitleModeXtermSequenceRegex;

        /// <summary>
        /// [CSI Ps SP t] Regular expression for setting warning bell volume
        /// </summary>
        public static Regex CsiSetWarningBellVolumeSequenceRegex =>
            csiSetWarningBellVolumeSequenceRegex;

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ t] Regular expression for reversing attributes in rectangular area
        /// </summary>
        public static Regex CsiReverseAttributesInRectangularAreaSequenceRegex =>
            csiReverseAttributesInRectangularAreaSequenceRegex;

        /// <summary>
        /// [CSI u] Regular expression for restoring the cursor
        /// </summary>
        public static Regex CsiRestoreCursorSequenceRegex =>
            csiRestoreCursorSequenceRegex;

        /// <summary>
        /// [CSI Ps SP u] Regular expression for setting margin bell volume
        /// </summary>
        public static Regex CsiSetMarginBellVolumeSequenceRegex =>
            csiSetMarginBellVolumeSequenceRegex;

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pp ; Pt ; Pl ; Pp $ v] Regular expression for copying rectangular area
        /// </summary>
        public static Regex CsiCopyRectangularAreaSequenceRegex =>
            csiCopyRectangularAreaSequenceRegex;

        /// <summary>
        /// [CSI Ps $ w] Regular expression for requesting presentation state report
        /// </summary>
        public static Regex CsiRequestPresentationStateReportSequenceRegex =>
            csiRequestPresentationStateReportSequenceRegex;

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ' w] Regular expression for enabling filter rectangle
        /// </summary>
        public static Regex CsiEnableFilterRectangleSequenceRegex =>
            csiEnableFilterRectangleSequenceRegex;

        /// <summary>
        /// [CSI Ps x] Regular expression for requesting terminal parameters
        /// </summary>
        public static Regex CsiRequestTerminalParametersSequenceRegex =>
            csiRequestTerminalParametersSequenceRegex;

        /// <summary>
        /// [CSI Ps * x] Regular expression for selecting attribute change extent
        /// </summary>
        public static Regex CsiSelectAttributeChangeExtentSequenceRegex =>
            csiSelectAttributeChangeExtentSequenceRegex;

        /// <summary>
        /// [CSI Pc ; Pt ; Pl ; Pb ; Pr $ x] Regular expression for filling rectangular area
        /// </summary>
        public static Regex CsiFillRectangularAreaSequenceRegex =>
            csiFillRectangularAreaSequenceRegex;

        /// <summary>
        /// [CSI Ps # y] Regular expression for selecting checksum extension
        /// </summary>
        public static Regex CsiSelectChecksumExtensionSequenceRegex =>
            csiSelectChecksumExtensionSequenceRegex;

        /// <summary>
        /// [CSI Pi ; Pg ; Pt ; Pl ; Pb ; Pr * y] Regular expression for reporting a checksum of a rectangular area
        /// </summary>
        public static Regex CsiRectangularAreaChecksumSequenceRegex =>
            csiRectangularAreaChecksumSequenceRegex;

        /// <summary>
        /// [CSI Ps ; Pu ' z] Regular expression for enabling the locator reporting feature
        /// </summary>
        public static Regex CsiLocatorReportingSequenceRegex =>
            csiLocatorReportingSequenceRegex;

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ z] Regular expression for erasing rectangular area
        /// </summary>
        public static Regex CsiEraseRectangularAreaSequenceRegex =>
            csiEraseRectangularAreaSequenceRegex;

        /// <summary>
        /// [CSI Pm ' {] Regular expression for selecting locator events
        /// </summary>
        public static Regex CsiSelectLocatorEventsSequenceRegex =>
            csiSelectLocatorEventsSequenceRegex;

        /// <summary>
        /// [CSI # {] Regular expression for pushing video attributes into the stack for xterm
        /// </summary>
        public static Regex CsiPushVideoAttributesToStackXtermSequenceRegex =>
            csiPushVideoAttributesToStackXtermSequenceRegex;

        /// <summary>
        /// [CSI Pm # {] Regular expression for pushing video attributes into the stack for xterm
        /// </summary>
        public static Regex CsiPushVideoAttributesToStackXtermWithArgsSequenceRegex =>
            csiPushVideoAttributesToStackXtermWithArgsSequenceRegex;

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ {] Regular expression for selectively erasing rectangular area
        /// </summary>
        public static Regex CsiSelectiveEraseRectangularAreaSequenceRegex =>
            csiSelectiveEraseRectangularAreaSequenceRegex;

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr # |] Regular expression for reporting graphics rendition for a rectangular area
        /// </summary>
        public static Regex CsiReportGraphicsRenditionRectangularAreaSequenceRegex =>
            csiReportGraphicsRenditionRectangularAreaSequenceRegex;

        /// <summary>
        /// [CSI Ps $ |] Regular expression for selecting columns per page
        /// </summary>
        public static Regex CsiSelectColumnsPerPageSequenceRegex =>
            csiSelectColumnsPerPageSequenceRegex;

        /// <summary>
        /// [CSI Ps ' |] Regular expression for requesting locator position
        /// </summary>
        public static Regex CsiRequestLocatorPositionSequenceRegex =>
            csiRequestLocatorPositionSequenceRegex;

        /// <summary>
        /// [CSI Ps * |] Regular expression for selecting number of lines per screen
        /// </summary>
        public static Regex CsiSelectNumberOfLinesPerScreenSequenceRegex =>
            csiSelectNumberOfLinesPerScreenSequenceRegex;

        /// <summary>
        /// [CSI # }] Regular expression for popping video attributes from the stack for xterm
        /// </summary>
        public static Regex CsiPopVideoAttributesFromStackXtermSequenceRegex =>
            csiPopVideoAttributesFromStackXtermSequenceRegex;

        /// <summary>
        /// [CSI Ps ' }] Regular expression for inserting Ps columns
        /// </summary>
        public static Regex CsiInsertColumnsSequenceRegex =>
            csiInsertColumnsSequenceRegex;

        /// <summary>
        /// [CSI Ps $ }] Regular expression for selecting active status display
        /// </summary>
        public static Regex CsiSelectActiveStatusDisplaySequenceRegex =>
            csiSelectActiveStatusDisplaySequenceRegex;

        /// <summary>
        /// [CSI Ps ' ~] Regular expression for deleting Ps columns
        /// </summary>
        public static Regex CsiDeleteColumnsSequenceRegex =>
            csiDeleteColumnsSequenceRegex;

        /// <summary>
        /// [CSI Ps $ ~] Regular expression for selecting status line type
        /// </summary>
        public static Regex CsiSelectStatusLineTypeSequenceRegex =>
            csiSelectStatusLineTypeSequenceRegex;
	
	    /// <summary>
        /// [CSI Ps @] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiInsertBlankCharacters(int blanks)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{blanks}@";
	        var regexParser = CsiInsertBlankCharactersSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps SP @] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiShiftLeftColumns(int columns)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{columns} @";
	        var regexParser = CsiShiftLeftColumnsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps A] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMoveCursorUp(int times)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{times}A";
	        var regexParser = CsiMoveCursorUpSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps SP A] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiShiftRightColumns(int columns)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{columns} A";
	        var regexParser = CsiShiftRightColumnsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps B] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMoveCursorDown(int times)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{times}B";
	        var regexParser = CsiMoveCursorDownSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps C] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMoveCursorRight(int times)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{times}C";
	        var regexParser = CsiMoveCursorRightSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps D] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMoveCursorLeft(int times)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{times}D";
	        var regexParser = CsiMoveCursorLeftSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps E] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMoveCursorNextLine(int times)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{times}E";
	        var regexParser = CsiMoveCursorNextLineSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps F] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMoveCursorPreviousLine(int times)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{times}F";
	        var regexParser = CsiMoveCursorPreviousLineSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps G] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCursorCharacterAbsoluteLine(int column)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{column}G";
	        var regexParser = CsiCursorCharacterAbsoluteLineSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps ; Ps H] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCursorPosition(int column, int row)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{row};{column}H";
	        var regexParser = CsiCursorPositionSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps I] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCursorForwardTabulation(int stops)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{stops}I";
	        var regexParser = CsiCursorForwardTabulationSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps J] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiEraseInDisplay(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}J";
	        var regexParser = CsiEraseInDisplaySequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Ps J] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiEraseInDisplayDecsed(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}J";
	        var regexParser = CsiEraseInDisplayDecsedSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps K] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiEraseInLine(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}K";
	        var regexParser = CsiEraseInLineSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Ps K] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiEraseInLineDecsel(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}K";
	        var regexParser = CsiEraseInLineDecselSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps L] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiInsertLine(int lines)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}L";
	        var regexParser = CsiInsertLineSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps M] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiDeleteLine(int lines)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}M";
	        var regexParser = CsiDeleteLineSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps P] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiDeleteChars(int chars)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{chars}P";
	        var regexParser = CsiDeleteCharsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI # P] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPushColorToStack()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[#P";
	        var regexParser = CsiPushColorToStackSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Pm # P] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPushColorToStack(string parameters)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}#P";
	        var regexParser = CsiPushColorToStackWithArgsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI # Q] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPopColorFromStack()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[#Q";
	        var regexParser = CsiPopColorFromStackSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Pm # Q] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPopColorFromStack(string parameters)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}#Q";
	        var regexParser = CsiPopColorFromStackWithArgsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI # R] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiReportPaletteStack()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[#R";
	        var regexParser = CsiReportPaletteStackSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps S] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiScrollUp(int lines)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}S";
	        var regexParser = CsiScrollUpSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Pi ; Pa ; Pv S] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetGraphicsAttribute(int itemType, int attributeManager, string geometry)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{itemType};{attributeManager};{geometry}S";
	        var regexParser = CsiSetGraphicsAttributeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps T] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiScrollDown(int lines)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}T";
	        var regexParser = CsiScrollDownSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps ; Ps ; Ps ; Ps ; Ps T] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiInitiateHighlightMouseTracking(int func, int startX, int startY, int firstRow, int lastRow)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{func};{startX};{startY};{firstRow};{lastRow}T";
	        var regexParser = CsiInitiateHighlightMouseTrackingSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI > Pm T] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiResetTitleModeFeatures(int resetOptions)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[>{resetOptions}T";
	        var regexParser = CsiResetTitleModeFeaturesSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps X] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiEraseCharacters(int chars)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{chars}X";
	        var regexParser = CsiEraseCharactersSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps Z] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCursorBackwardTabulation(int stops)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{stops}Z";
	        var regexParser = CsiCursorBackwardTabulationSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps ^] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiScrollDownOriginal(int lines)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}^";
	        var regexParser = CsiScrollDownOriginalSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps `] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCursorPositionAbsolute(int column)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{column}`";
	        var regexParser = CsiCursorPositionAbsoluteSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps a] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCursorPositionRelative(int column)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{column}a";
	        var regexParser = CsiCursorPositionRelativeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps b] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRepeatGraphicCharacter(int times)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{times}b";
	        var regexParser = CsiRepeatGraphicCharacterSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps c] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSendDeviceAttributesPrimary(int attributes)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{attributes}c";
	        var regexParser = CsiSendDeviceAttributesPrimarySequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI = Ps c] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSendDeviceAttributesSecondary(int attributes)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[={attributes}c";
	        var regexParser = CsiSendDeviceAttributesSecondarySequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI > Ps c] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSendDeviceAttributesTertiary(int attributes)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[>{attributes}c";
	        var regexParser = CsiSendDeviceAttributesTertiarySequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps d] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiLinePositionAbsolute(int row)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{row}d";
	        var regexParser = CsiLinePositionAbsoluteSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps e] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiLinePositionRelative(int row)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{row}e";
	        var regexParser = CsiLinePositionRelativeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps ; Ps f] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiLeftTopPosition(int column, int row)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{row};{column}f";
	        var regexParser = CsiLeftTopPositionSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps g] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiTabClear(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}g";
	        var regexParser = CsiTabClearSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Pm h] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetMode(string mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}h";
	        var regexParser = CsiSetModeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Pm h] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetPrivateMode(string mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}h";
	        var regexParser = CsiSetPrivateModeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps i] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMediaCopy(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}i";
	        var regexParser = CsiMediaCopySequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Ps i] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiMediaCopyPrivate(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}i";
	        var regexParser = CsiMediaCopyPrivateSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Pm l] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiResetMode(string mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}l";
	        var regexParser = CsiResetModeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Pm l] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiResetPrivateMode(string mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}l";
	        var regexParser = CsiResetPrivateModeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Pm m] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCharacterAttributes(string mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}m";
	        var regexParser = CsiCharacterAttributesSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI > Pp ; Pv m] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetKeyModifierOptions(int resource, int modify)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[>{resource};{modify}m";
	        var regexParser = CsiSetKeyModifierOptionsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI > Pp m] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiResetKeyModifierOptions(int resource)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[>{resource}m";
	        var regexParser = CsiResetKeyModifierOptionsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Pp m] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiQueryKeyModifierOptions(int resource)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{resource}m";
	        var regexParser = CsiQueryKeyModifierOptionsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps n] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiDeviceStatusReport(int report)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{report}n";
	        var regexParser = CsiDeviceStatusReportSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI > Ps n] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiDisableKeyModifierOptions(int report)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[>{report}n";
	        var regexParser = CsiDisableKeyModifierOptionsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Ps n] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiDeviceStatusReportDec(int report)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{report}n";
	        var regexParser = CsiDeviceStatusReportDecSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI > Ps p] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetPointerModeXterm(int hideMode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[>{hideMode}p";
	        var regexParser = CsiSetPointerModeXtermSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ! p] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSoftTerminalReset()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[!p";
	        var regexParser = CsiSoftTerminalResetSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Pl ; Pc " p] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetConformanceLevel(int level, int conformance)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{level};{conformance}\"p";
	        var regexParser = CsiSetConformanceLevelSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps $ p] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRequestAnsiMode(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}$p";
	        var regexParser = CsiRequestAnsiModeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI ? Ps $ p] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRequestDecPrivateMode(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}$p";
	        var regexParser = CsiRequestDecPrivateModeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI # p] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPushVideoAttributesToStack()
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[#p";
	        var regexParser = CsiPushVideoAttributesToStackSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Pm # p] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPushVideoAttributesToStack(string args)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{args}#p";
	        var regexParser = CsiPushVideoAttributesToStackWithArgsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI > Ps q] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiReportXtermVersion(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[>{mode}q";
	        var regexParser = CsiReportXtermVersionSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps q] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiLoadLeds(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}q";
	        var regexParser = CsiLoadLedsSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps SP q] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetCursorStyle(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode} q";
	        var regexParser = CsiSetCursorStyleSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
	    }
	
	    /// <summary>
        /// [CSI Ps " q] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectCharacterProtectionAttribute(int mode)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}\"q";
	        var regexParser = CsiSelectCharacterProtectionAttributeSequenceRegex;
		    if (!regexParser.IsMatch(result))
		        throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
		    return result;
        }

        /// <summary>
        /// [CSI # q] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPopVideoAttributesFromStack()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#q";
            var regexParser = CsiPopVideoAttributesFromStackSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps ; Ps r] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetScrollingRegion(int top, int bottom)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{top};{bottom}r";
            var regexParser = CsiSetScrollingRegionSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI ? Pm r] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRestoreDecPrivateModeValues(string values)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{values}r";
            var regexParser = CsiRestoreDecPrivateModeValuesSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ r] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiChangeAttributesInRectangularArea(string attributes, int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr};{attributes}$r";
            var regexParser = CsiChangeAttributesInRectangularAreaSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI s] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSaveCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[s";
            var regexParser = CsiSaveCursorSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pl ; Pr s] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetLeftRightMargins(int left, int right)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{left};{right}s";
            var regexParser = CsiSetLeftRightMarginsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI > Ps s] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetShiftEscapeOptions(int option)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{option}s";
            var regexParser = CsiSetShiftEscapeOptionsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI ? Pm s] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiDecPrivateModeValues(string values)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{values}s";
            var regexParser = CsiDecPrivateModeValuesSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps ; Ps ; Ps ; t] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiWindowManipulation(int windowAction, int windowAction2, int windowAction3)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{windowAction};{windowAction2};{windowAction3}t";
            var regexParser = CsiWindowManipulationSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI > Pm t] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetTitleModeXterm(string modes)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{modes}t";
            var regexParser = CsiSetTitleModeXtermSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps SP t] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetWarningBellVolume(int level)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{level} t";
            var regexParser = CsiSetWarningBellVolumeSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ t] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiReverseAttributesInRectangularArea(string attributes, int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr};{attributes}$t";
            var regexParser = CsiReverseAttributesInRectangularAreaSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI u] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRestoreCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[u";
            var regexParser = CsiRestoreCursorSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps SP u] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSetMarginBellVolume(int level)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{level} u";
            var regexParser = CsiSetMarginBellVolumeSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pp ; Pt ; Pl ; Pp $ v] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiCopyRectangularArea(int ptSrc, int plSrc, int pbSrc, int prSrc, int sourcePage, int ptTarget, int plTarget, int targetPage)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{ptSrc};{plSrc};{pbSrc};{prSrc};{sourcePage};{ptTarget};{plTarget};{targetPage}$v";
            var regexParser = CsiCopyRectangularAreaSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps $ w] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRequestPresentationStateReport(int state)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{state}$w";
            var regexParser = CsiRequestPresentationStateReportSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ' w] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiEnableFilterRectangle(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}'w";
            var regexParser = CsiEnableFilterRectangleSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps x] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRequestTerminalParameters(int parameter)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameter}x";
            var regexParser = CsiRequestTerminalParametersSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps * x] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectAttributeChangeExtent(int extent)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{extent}*x";
            var regexParser = CsiSelectAttributeChangeExtentSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pc ; Pt ; Pl ; Pb ; Pr $ x] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiFillRectangularArea(char character, int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{character};{pt};{pl};{pb};{pr}$x";
            var regexParser = CsiFillRectangularAreaSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps # y] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectChecksumExtension(int modifier)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{modifier}#y";
            var regexParser = CsiSelectChecksumExtensionSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pi ; Pg ; Pt ; Pl ; Pb ; Pr * y] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRectangularAreaChecksum(int requestId, int pageNumber, int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{requestId};{pageNumber};{pt};{pl};{pb};{pr}*y";
            var regexParser = CsiRectangularAreaChecksumSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps ; Pu ' z] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiLocatorReporting(int locatorMode, int measurement)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{locatorMode};{measurement}'z";
            var regexParser = CsiLocatorReportingSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ z] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiEraseRectangularArea(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}$z";
            var regexParser = CsiEraseRectangularAreaSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pm ' {] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectLocatorEvents(string parameters)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}'{{";
            var regexParser = CsiSelectLocatorEventsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI # {] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPushVideoAttributesToStackXterm()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#{{";
            var regexParser = CsiPushVideoAttributesToStackXtermSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pm # {] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPushVideoAttributesToStackXterm(string parameters)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}#{{";
            var regexParser = CsiPushVideoAttributesToStackXtermWithArgsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ {] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectiveEraseRectangularArea(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}${{";
            var regexParser = CsiSelectiveEraseRectangularAreaSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr # |] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiReportGraphicsRenditionRectangularArea(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}#|";
            var regexParser = CsiReportGraphicsRenditionRectangularAreaSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps $ |] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectColumnsPerPage(int columnMode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columnMode}$|";
            var regexParser = CsiSelectColumnsPerPageSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps ' |] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiRequestLocatorPosition(int transmit)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{transmit}'|";
            var regexParser = CsiRequestLocatorPositionSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps * |] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectNumberOfLinesPerScreen(int lines)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}*|";
            var regexParser = CsiSelectNumberOfLinesPerScreenSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI # }] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiPopVideoAttributesFromStackXterm()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#}}";
            var regexParser = CsiPopVideoAttributesFromStackXtermSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps ' }] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiInsertColumns(int columns)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columns}'}}";
            var regexParser = CsiInsertColumnsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps $ }] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectActiveStatusDisplay(int displayMode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{displayMode}$}}";
            var regexParser = CsiSelectActiveStatusDisplaySequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps ' ~] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiDeleteColumns(int columns)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columns}'~";
            var regexParser = CsiDeleteColumnsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [CSI Ps $ ~] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiSelectStatusLineType(int type)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{type}$~";
            var regexParser = CsiSelectStatusLineTypeSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }
    }
}
