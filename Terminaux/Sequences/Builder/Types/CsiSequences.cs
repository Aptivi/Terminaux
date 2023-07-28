/*
 * MIT License
 * 
 * Copyright (c) 2022 Aptivi
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
 */

using System;
using System.Text.RegularExpressions;

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of CSI sequences and their regular expressions
    /// </summary>
    public static class CsiSequences
    {
        /// <summary>
        /// [CSI Ps @] Regular expression for inserting the blank characters Ps times
        /// </summary>
        public static string CsiInsertBlankCharactersSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[@]"; }

        /// <summary>
        /// [CSI Ps SP @] Regular expression for shifting left Ps columns
        /// </summary>
        public static string CsiShiftLeftColumnsSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[ ][@]"; }

        /// <summary>
        /// [CSI Ps A] Regular expression for moving the cursor up Ps times
        /// </summary>
        public static string CsiMoveCursorUpSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[A]"; }

        /// <summary>
        /// [CSI Ps SP A] Regular expression for shifting right Ps columns
        /// </summary>
        public static string CsiShiftRightColumnsSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[ ][A]"; }

        /// <summary>
        /// [CSI Ps B] Regular expression for moving the cursor down Ps times
        /// </summary>
        public static string CsiMoveCursorDownSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[B]"; }

        /// <summary>
        /// [CSI Ps C] Regular expression for moving the cursor to the right Ps times
        /// </summary>
        public static string CsiMoveCursorRightSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[C]"; }

        /// <summary>
        /// [CSI Ps D] Regular expression for moving the cursor to the left Ps times
        /// </summary>
        public static string CsiMoveCursorLeftSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[D]"; }

        /// <summary>
        /// [CSI Ps E] Regular expression for moving the cursor to the next line Ps times
        /// </summary>
        public static string CsiMoveCursorNextLineSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[E]"; }

        /// <summary>
        /// [CSI Ps F] Regular expression for moving the cursor to the previous line Ps times
        /// </summary>
        public static string CsiMoveCursorPreviousLineSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[F]"; }

        /// <summary>
        /// [CSI Ps G] Regular expression for cursor character absolute
        /// </summary>
        public static string CsiCursorCharacterAbsoluteLineSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[G]"; }

        /// <summary>
        /// [CSI Ps ; Ps H] Regular expression for cursor position (Ps column ; Ps row)
        /// </summary>
        public static string CsiCursorPositionSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[;][0-9]*[H]"; }

        /// <summary>
        /// [CSI Ps I] Regular expression for cursor forward tabulation Ps tab stops
        /// </summary>
        public static string CsiCursorForwardTabulationSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[I]"; }

        /// <summary>
        /// [CSI Ps J] Regular expression for erasing in display (ED)
        /// </summary>
        public static string CsiEraseInDisplaySequenceRegex { get => @"(\x9B|\x1B\[)[0-3]*[J]"; }

        /// <summary>
        /// [CSI ? Ps J] Regular expression for erasing in display (DECSED)
        /// </summary>
        public static string CsiEraseInDisplayDecsedSequenceRegex { get => @"(\x9B|\x1B\[)\?[0-3]*[J]"; }

        /// <summary>
        /// [CSI Ps K] Regular expression for erasing in line (EL)
        /// </summary>
        public static string CsiEraseInLineSequenceRegex { get => @"(\x9B|\x1B\[)[0-3]*[K]"; }

        /// <summary>
        /// [CSI ? Ps K] Regular expression for erasing in line (DECSEL)
        /// </summary>
        public static string CsiEraseInLineDecselSequenceRegex { get => @"(\x9B|\x1B\[)\?[0-3]*[K]"; }

        /// <summary>
        /// [CSI Ps L] Regular expression for inserting Ps lines
        /// </summary>
        public static string CsiInsertLineSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[L]"; }

        /// <summary>
        /// [CSI Ps M] Regular expression for deleting Ps lines
        /// </summary>
        public static string CsiDeleteLineSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[M]"; }

        /// <summary>
        /// [CSI Ps P] Regular expression for deleting Ps characters
        /// </summary>
        public static string CsiDeleteCharsSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[P]"; }

        /// <summary>
        /// [CSI # P] Regular expression for pushing color into the stack
        /// </summary>
        public static string CsiPushColorToStackSequenceRegex { get => @"(\x9B|\x1B\[)#[P]"; }

        /// <summary>
        /// [CSI Pm # P] Regular expression for pushing color into the stack
        /// </summary>
        public static string CsiPushColorToStackWithArgsSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)#[P]"; }

        /// <summary>
        /// [CSI # Q] Regular expression for popping color from the stack
        /// </summary>
        public static string CsiPopColorFromStackSequenceRegex { get => @"(\x9B|\x1B\[)#[Q]"; }

        /// <summary>
        /// [CSI Pm # Q] Regular expression for popping color from the stack
        /// </summary>
        public static string CsiPopColorFromStackWithArgsSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)#[Q]"; }

        /// <summary>
        /// [CSI # R] Regular expression for reporting the palette stack
        /// </summary>
        public static string CsiReportPaletteStackSequenceRegex { get => @"(\x9B|\x1B\[)#[R]"; }

        /// <summary>
        /// [CSI Ps S] Regular expression for scrolling up Ps lines
        /// </summary>
        public static string CsiScrollUpSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[S]"; }

        /// <summary>
        /// [CSI ? Pi ; Pa ; Pv S] Regular expression for setting graphics attribute
        /// </summary>
        public static string CsiSetGraphicsAttributeSequenceRegex { get => @"(\x9B|\x1B\[)\?[0-9]*\;[0-9]*\;(.+?)*[S]"; }

        /// <summary>
        /// [CSI Ps T] Regular expression for scrolling down Ps lines
        /// </summary>
        public static string CsiScrollDownSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[T]"; }

        /// <summary>
        /// [CSI Ps ; Ps ; Ps ; Ps ; Ps T] Regular expression for initiating highlight mouse tracking
        /// </summary>
        public static string CsiInitiateHighlightMouseTrackingSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[T]"; }

        /// <summary>
        /// [CSI > Pm T] Regular expression for resetting title mode features
        /// </summary>
        public static string CsiResetTitleModeFeaturesSequenceRegex { get => @"(\x9B|\x1B\[)\>(.+?)[T]"; }

        /// <summary>
        /// [CSI Ps X] Regular expression for erasing Ps characters
        /// </summary>
        public static string CsiEraseCharactersSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[X]"; }

        /// <summary>
        /// [CSI Ps Z] Regular expression for cursor backward tabulation Ps tab stops
        /// </summary>
        public static string CsiCursorBackwardTabulationSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[Z]"; }

        /// <summary>
        /// [CSI Ps ^] Regular expression for scrolling down Ps lines
        /// </summary>
        public static string CsiScrollDownOriginalSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[\^]"; }

        /// <summary>
        /// [CSI Ps `] Regular expression for cursor position (absolute)
        /// </summary>
        public static string CsiCursorPositionAbsoluteSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[`]"; }

        /// <summary>
        /// [CSI Ps a] Regular expression for cursor position (relative)
        /// </summary>
        public static string CsiCursorPositionRelativeSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[a]"; }

        /// <summary>
        /// [CSI Ps b] Regular expression for repeating a graphic character
        /// </summary>
        public static string CsiRepeatGraphicCharacterSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[b]"; }

        /// <summary>
        /// [CSI Ps c] Regular expression for sending device attributes (Primary DA)
        /// </summary>
        public static string CsiSendDeviceAttributesPrimarySequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[c]"; }

        /// <summary>
        /// [CSI = Ps c] Regular expression for sending device attributes (Secondary DA)
        /// </summary>
        public static string CsiSendDeviceAttributesSecondarySequenceRegex { get => @"(\x9B|\x1B\[)=[0-9]*[c]"; }

        /// <summary>
        /// [CSI > Ps c] Regular expression for sending device attributes (Tertiary DA)
        /// </summary>
        public static string CsiSendDeviceAttributesTertiarySequenceRegex { get => @"(\x9B|\x1B\[)\>[0-9]*[c]"; }

        /// <summary>
        /// [CSI Ps d] Regular expression for line position (absolute)
        /// </summary>
        public static string CsiLinePositionAbsoluteSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[d]"; }

        /// <summary>
        /// [CSI Ps e] Regular expression for line position (relative)
        /// </summary>
        public static string CsiLinePositionRelativeSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[e]"; }

        /// <summary>
        /// [CSI Ps ; Ps f] Regular expression for horizontal and vertical position
        /// </summary>
        public static string CsiLeftTopPositionSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*[f]"; }

        /// <summary>
        /// [CSI Ps g] Regular expression for tab clear
        /// </summary>
        public static string CsiTabClearSequenceRegex { get => @"(\x9B|\x1B\[)[03][g]"; }

        /// <summary>
        /// [CSI Pm h] Regular expression for setting mode
        /// </summary>
        public static string CsiSetModeSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)[h]"; }

        /// <summary>
        /// [CSI ? Pm h] Regular expression for setting mode (Private mode set)
        /// </summary>
        public static string CsiSetPrivateModeSequenceRegex { get => @"(\x9B|\x1B\[)\?(.+?)[h]"; }

        /// <summary>
        /// [CSI Ps i] Regular expression for media copy
        /// </summary>
        public static string CsiMediaCopySequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[i]"; }

        /// <summary>
        /// [CSI ? Ps i] Regular expression for media copy (Private)
        /// </summary>
        public static string CsiMediaCopyPrivateSequenceRegex { get => @"(\x9B|\x1B\[)\?[0-9]*[i]"; }

        /// <summary>
        /// [CSI Pm l] Regular expression for reset mode
        /// </summary>
        public static string CsiResetModeSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)[l]"; }

        /// <summary>
        /// [CSI ? Pm l] Regular expression for reset mode (Private mode reset)
        /// </summary>
        public static string CsiResetPrivateModeSequenceRegex { get => @"(\x9B|\x1B\[)\?(.+?)[l]"; }

        /// <summary>
        /// [CSI Pm m] Regular expression for character attributes
        /// </summary>
        public static string CsiCharacterAttributesSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)[m]"; }

        /// <summary>
        /// [CSI > Pp ; Pv m] Regular expression for setting key modifier options
        /// </summary>
        public static string CsiSetKeyModifierOptionsSequenceRegex { get => @"(\x9B|\x1B\[)\>[0-9]*\;[0-9]*[m]"; }

        /// <summary>
        /// [CSI > Pp m] Regular expression for resetting key modifier options
        /// </summary>
        public static string CsiResetKeyModifierOptionsSequenceRegex { get => @"(\x9B|\x1B\[)\>[0-9]*[m]"; }

        /// <summary>
        /// [CSI > Pp m] Regular expression for querying key modifier options
        /// </summary>
        public static string CsiQueryKeyModifierOptionsSequenceRegex { get => @"(\x9B|\x1B\[)\?[0-9]*[m]"; }

        /// <summary>
        /// [CSI Ps n] Regular expression for device status report
        /// </summary>
        public static string CsiDeviceStatusReportSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[n]"; }

        /// <summary>
        /// [CSI > Ps n] Regular expression for disabling key modifier options
        /// </summary>
        public static string CsiDisableKeyModifierOptionsSequenceRegex { get => @"(\x9B|\x1B\[)\>[0-9]*[n]"; }

        /// <summary>
        /// [CSI ? Ps n] Regular expression for device status report (DEC-specific)
        /// </summary>
        public static string CsiDeviceStatusReportDecSequenceRegex { get => @"(\x9B|\x1B\[)\?[0-9]*[n]"; }

        /// <summary>
        /// [CSI > Ps p] Regular expression for setting pointerMode for xterm
        /// </summary>
        public static string CsiSetPointerModeXtermSequenceRegex { get => @"(\x9B|\x1B\[)\>[0-9]*[p]"; }

        /// <summary>
        /// [CSI ! p] Regular expression for soft terminal reset
        /// </summary>
        public static string CsiSoftTerminalResetSequenceRegex { get => @"(\x9B|\x1B\[)![p]"; }

        /// <summary>
        /// [CSI Pl ; Pc " p] Regular expression for setting conformance level
        /// </summary>
        public static string CsiSetConformanceLevelSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*[""]p"; }

        /// <summary>
        /// [CSI Ps $ p] Regular expression for requesting ANSI mode
        /// </summary>
        public static string CsiRequestAnsiModeSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[$]p"; }

        /// <summary>
        /// [CSI ? Ps $ p] Regular expression for requesting DEC private mode
        /// </summary>
        public static string CsiRequestDecPrivateModeSequenceRegex { get => @"(\x9B|\x1B\[)\?[0-9]*[$]p"; }

        /// <summary>
        /// [CSI # p] Regular expression for pushing video attributes into the stack
        /// </summary>
        public static string CsiPushVideoAttributesToStackSequenceRegex { get => @"(\x9B|\x1B\[)#[p]"; }

        /// <summary>
        /// [CSI Pm # p] Regular expression for pushing video attributes into the stack
        /// </summary>
        public static string CsiPushVideoAttributesToStackWithArgsSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)#[p]"; }

        /// <summary>
        /// [CSI > Ps q] Regular expression for reporting the xterm version
        /// </summary>
        public static string CsiReportXtermVersionSequenceRegex { get => @"(\x9B|\x1B\[)\>[0-9]*[q]"; }

        /// <summary>
        /// [CSI Ps q] Regular expression for loading LEDs
        /// </summary>
        public static string CsiLoadLedsSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[q]"; }

        /// <summary>
        /// [CSI Ps SP q] Regular expression for setting cursor style
        /// </summary>
        public static string CsiSetCursorStyleSequenceRegex { get => @"(\x9B|\x1B\[)[0-6]* [q]"; }

        /// <summary>
        /// [CSI Ps " q] Regular expression for selecting character protection attribute
        /// </summary>
        public static string CsiSelectCharacterProtectionAttributeSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[""]q"; }

        /// <summary>
        /// [CSI # q] Regular expression for popping video attributes from the stack
        /// </summary>
        public static string CsiPopVideoAttributesFromStackSequenceRegex { get => @"(\x9B|\x1B\[)#[q]"; }

        /// <summary>
        /// [CSI Ps ; Ps r] Regular expression for setting scroll region
        /// </summary>
        public static string CsiSetScrollingRegionSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*[r]"; }

        /// <summary>
        /// [CSI ? Pm r] Regular expression for restoring DEC private mode values
        /// </summary>
        public static string CsiRestoreDecPrivateModeValuesSequenceRegex { get => @"(\x9B|\x1B\[)\?(.+?)[r]"; }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ r] Regular expression for changing attributes in rectangular area
        /// </summary>
        public static string CsiChangeAttributesInRectangularAreaSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;(.+?)[$]r"; }

        /// <summary>
        /// [CSI s] Regular expression for saving the cursor
        /// </summary>
        public static string CsiSaveCursorSequenceRegex { get => @"(\x9B|\x1B\[)s"; }

        /// <summary>
        /// [CSI Pl ; Pr s] Regular expression for setting left and right margins
        /// </summary>
        public static string CsiSetLeftRightMarginsSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*[s]"; }

        /// <summary>
        /// [CSI > Ps s] Regular expression for setting shift-escape options
        /// </summary>
        public static string CsiSetShiftEscapeOptionsSequenceRegex { get => @"(\x9B|\x1B\[)\>[0-9]*[s]"; }

        /// <summary>
        /// [CSI ? Pm s] Regular expression for saving DEC private mode values
        /// </summary>
        public static string CsiDecPrivateModeValuesSequenceRegex { get => @"(\x9B|\x1B\[)\?(.+?)[s]"; }

        /// <summary>
        /// [CSI Ps ; Ps ; Ps t] Regular expression for window manipulation
        /// </summary>
        public static string CsiWindowManipulationSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*[t]"; }

        /// <summary>
        /// [CSI > Pm t] Regular expression for setting title mode for xterm
        /// </summary>
        public static string CsiSetTitleModeXtermSequenceRegex { get => @"(\x9B|\x1B\[)\>(.+?)[t]"; }

        /// <summary>
        /// [CSI Ps SP t] Regular expression for setting warning bell volume
        /// </summary>
        public static string CsiSetWarningBellVolumeSequenceRegex { get => @"(\x9B|\x1B\[)[0-8]* [t]"; }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ t] Regular expression for reversing attributes in rectangular area
        /// </summary>
        public static string CsiReverseAttributesInRectangularAreaSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;(.+?)[$]t"; }

        /// <summary>
        /// [CSI u] Regular expression for restoring the cursor
        /// </summary>
        public static string CsiRestoreCursorSequenceRegex { get => @"(\x9B|\x1B\[)u"; }

        /// <summary>
        /// [CSI Ps SP u] Regular expression for setting margin bell volume
        /// </summary>
        public static string CsiSetMarginBellVolumeSequenceRegex { get => @"(\x9B|\x1B\[)[0-8] [u]"; }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pp ; Pt ; Pl ; Pp $ v] Regular expression for copying rectangular area
        /// </summary>
        public static string CsiCopyRectangularAreaSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]v"; }

        /// <summary>
        /// [CSI Ps $ w] Regular expression for requesting presentation state report
        /// </summary>
        public static string CsiRequestPresentationStateReportSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[$]w"; }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ' w] Regular expression for enabling filter rectangle
        /// </summary>
        public static string CsiEnableFilterRectangleSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[']w"; }

        /// <summary>
        /// [CSI Ps x] Regular expression for requesting terminal parameters
        /// </summary>
        public static string CsiRequestTerminalParametersSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[x]"; }

        /// <summary>
        /// [CSI Ps * x] Regular expression for selecting attribute change extent
        /// </summary>
        public static string CsiSelectAttributeChangeExtentSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[\*]x"; }

        /// <summary>
        /// [CSI Pc ; Pt ; Pl ; Pb ; Pr $ x] Regular expression for filling rectangular area
        /// </summary>
        public static string CsiFillRectangularAreaSequenceRegex { get => @"(\x9B|\x1B\[)\D+\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]x"; }

        /// <summary>
        /// [CSI Ps # y] Regular expression for selecting checksum extension
        /// </summary>
        public static string CsiSelectChecksumExtensionSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[#]y"; }

        /// <summary>
        /// [CSI Pi ; Pg ; Pt ; Pl ; Pb ; Pr * y] Regular expression for reporting a checksum of a rectangular area
        /// </summary>
        public static string CsiRectangularAreaChecksumSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[\*]y"; }

        /// <summary>
        /// [CSI Ps ; Pu ' z] Regular expression for enabling the locator reporting feature
        /// </summary>
        public static string CsiLocatorReportingSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*[']z"; }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ z] Regular expression for erasing rectangular area
        /// </summary>
        public static string CsiEraseRectangularAreaSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]z"; }

        /// <summary>
        /// [CSI Pm ' {] Regular expression for selecting locator events
        /// </summary>
        public static string CsiSelectLocatorEventsSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)[']\{"; }

        /// <summary>
        /// [CSI # {] Regular expression for pushing video attributes into the stack for xterm
        /// </summary>
        public static string CsiPushVideoAttributesToStackXtermSequenceRegex { get => @"(\x9B|\x1B\[)#[\{]"; }

        /// <summary>
        /// [CSI Pm # {] Regular expression for pushing video attributes into the stack for xterm
        /// </summary>
        public static string CsiPushVideoAttributesToStackXtermWithArgsSequenceRegex { get => @"(\x9B|\x1B\[)(.+?)#[\{]"; }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ {] Regular expression for selectively erasing rectangular area
        /// </summary>
        public static string CsiSelectiveEraseRectangularAreaSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[$]\{"; }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr # |] Regular expression for reporting graphics rendition for a rectangular area
        /// </summary>
        public static string CsiReportGraphicsRenditionRectangularAreaSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*\;[0-9]*\;[0-9]*\;[0-9]*[#]\|"; }

        /// <summary>
        /// [CSI Ps $ |] Regular expression for selecting columns per page
        /// </summary>
        public static string CsiSelectColumnsPerPageSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[$]\|"; }

        /// <summary>
        /// [CSI Ps ' |] Regular expression for requesting locator position
        /// </summary>
        public static string CsiRequestLocatorPositionSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[']\|"; }

        /// <summary>
        /// [CSI Ps * |] Regular expression for selecting number of lines per screen
        /// </summary>
        public static string CsiSelectNumberOfLinesPerScreenSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[\*]\|"; }

        /// <summary>
        /// [CSI # }] Regular expression for popping video attributes from the stack for xterm
        /// </summary>
        public static string CsiPopVideoAttributesFromStackXtermSequenceRegex { get => @"(\x9B|\x1B\[)[#]\}"; }

        /// <summary>
        /// [CSI Ps ' }] Regular expression for inserting Ps columns
        /// </summary>
        public static string CsiInsertColumnsSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[']\}"; }

        /// <summary>
        /// [CSI Ps $ }] Regular expression for selecting active status display
        /// </summary>
        public static string CsiSelectActiveStatusDisplaySequenceRegex { get => @"(\x9B|\x1B\[)[01]*[$]\}"; }

        /// <summary>
        /// [CSI Ps ' ~] Regular expression for deleting Ps columns
        /// </summary>
        public static string CsiDeleteColumnsSequenceRegex { get => @"(\x9B|\x1B\[)[0-9]*[']~"; }

        /// <summary>
        /// [CSI Ps $ ~] Regular expression for selecting status line type
        /// </summary>
        public static string CsiSelectStatusLineTypeSequenceRegex { get => @"(\x9B|\x1B\[)[0-2]*[$]~"; }
	
	    /// <summary>
        /// [CSI Ps @] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateCsiInsertBlankCharacters(int blanks)
	    {
		    string result = $"{VtSequenceBasicChars.EscapeChar}[{blanks}@";
	        var regexParser = new Regex(CsiInsertBlankCharactersSequenceRegex);
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
	        var regexParser = new Regex(CsiShiftLeftColumnsSequenceRegex);
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
	        var regexParser = new Regex(CsiMoveCursorUpSequenceRegex);
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
	        var regexParser = new Regex(CsiShiftRightColumnsSequenceRegex);
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
	        var regexParser = new Regex(CsiMoveCursorDownSequenceRegex);
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
	        var regexParser = new Regex(CsiMoveCursorRightSequenceRegex);
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
	        var regexParser = new Regex(CsiMoveCursorLeftSequenceRegex);
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
	        var regexParser = new Regex(CsiMoveCursorNextLineSequenceRegex);
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
	        var regexParser = new Regex(CsiMoveCursorPreviousLineSequenceRegex);
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
	        var regexParser = new Regex(CsiCursorCharacterAbsoluteLineSequenceRegex);
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
	        var regexParser = new Regex(CsiCursorPositionSequenceRegex);
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
	        var regexParser = new Regex(CsiCursorForwardTabulationSequenceRegex);
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
	        var regexParser = new Regex(CsiEraseInDisplaySequenceRegex);
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
	        var regexParser = new Regex(CsiEraseInDisplayDecsedSequenceRegex);
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
	        var regexParser = new Regex(CsiEraseInLineSequenceRegex);
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
	        var regexParser = new Regex(CsiEraseInLineDecselSequenceRegex);
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
	        var regexParser = new Regex(CsiInsertLineSequenceRegex);
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
	        var regexParser = new Regex(CsiDeleteLineSequenceRegex);
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
	        var regexParser = new Regex(CsiDeleteCharsSequenceRegex);
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
	        var regexParser = new Regex(CsiPushColorToStackSequenceRegex);
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
	        var regexParser = new Regex(CsiPushColorToStackWithArgsSequenceRegex);
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
	        var regexParser = new Regex(CsiPopColorFromStackSequenceRegex);
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
	        var regexParser = new Regex(CsiPopColorFromStackWithArgsSequenceRegex);
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
	        var regexParser = new Regex(CsiReportPaletteStackSequenceRegex);
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
	        var regexParser = new Regex(CsiScrollUpSequenceRegex);
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
	        var regexParser = new Regex(CsiSetGraphicsAttributeSequenceRegex);
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
	        var regexParser = new Regex(CsiScrollDownSequenceRegex);
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
	        var regexParser = new Regex(CsiInitiateHighlightMouseTrackingSequenceRegex);
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
	        var regexParser = new Regex(CsiResetTitleModeFeaturesSequenceRegex);
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
	        var regexParser = new Regex(CsiEraseCharactersSequenceRegex);
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
	        var regexParser = new Regex(CsiCursorBackwardTabulationSequenceRegex);
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
	        var regexParser = new Regex(CsiScrollDownOriginalSequenceRegex);
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
	        var regexParser = new Regex(CsiCursorPositionAbsoluteSequenceRegex);
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
	        var regexParser = new Regex(CsiCursorPositionRelativeSequenceRegex);
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
	        var regexParser = new Regex(CsiRepeatGraphicCharacterSequenceRegex);
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
	        var regexParser = new Regex(CsiSendDeviceAttributesPrimarySequenceRegex);
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
	        var regexParser = new Regex(CsiSendDeviceAttributesSecondarySequenceRegex);
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
	        var regexParser = new Regex(CsiSendDeviceAttributesTertiarySequenceRegex);
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
	        var regexParser = new Regex(CsiLinePositionAbsoluteSequenceRegex);
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
	        var regexParser = new Regex(CsiLinePositionRelativeSequenceRegex);
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
	        var regexParser = new Regex(CsiLeftTopPositionSequenceRegex);
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
	        var regexParser = new Regex(CsiTabClearSequenceRegex);
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
	        var regexParser = new Regex(CsiSetModeSequenceRegex);
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
	        var regexParser = new Regex(CsiSetPrivateModeSequenceRegex);
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
	        var regexParser = new Regex(CsiMediaCopySequenceRegex);
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
	        var regexParser = new Regex(CsiMediaCopyPrivateSequenceRegex);
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
	        var regexParser = new Regex(CsiResetModeSequenceRegex);
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
	        var regexParser = new Regex(CsiResetPrivateModeSequenceRegex);
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
	        var regexParser = new Regex(CsiCharacterAttributesSequenceRegex);
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
	        var regexParser = new Regex(CsiSetKeyModifierOptionsSequenceRegex);
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
	        var regexParser = new Regex(CsiResetKeyModifierOptionsSequenceRegex);
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
	        var regexParser = new Regex(CsiQueryKeyModifierOptionsSequenceRegex);
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
	        var regexParser = new Regex(CsiDeviceStatusReportSequenceRegex);
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
	        var regexParser = new Regex(CsiDisableKeyModifierOptionsSequenceRegex);
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
	        var regexParser = new Regex(CsiDeviceStatusReportDecSequenceRegex);
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
	        var regexParser = new Regex(CsiSetPointerModeXtermSequenceRegex);
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
	        var regexParser = new Regex(CsiSoftTerminalResetSequenceRegex);
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
	        var regexParser = new Regex(CsiSetConformanceLevelSequenceRegex);
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
	        var regexParser = new Regex(CsiRequestAnsiModeSequenceRegex);
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
	        var regexParser = new Regex(CsiRequestDecPrivateModeSequenceRegex);
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
	        var regexParser = new Regex(CsiPushVideoAttributesToStackSequenceRegex);
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
	        var regexParser = new Regex(CsiPushVideoAttributesToStackWithArgsSequenceRegex);
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
	        var regexParser = new Regex(CsiReportXtermVersionSequenceRegex);
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
	        var regexParser = new Regex(CsiLoadLedsSequenceRegex);
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
	        var regexParser = new Regex(CsiSetCursorStyleSequenceRegex);
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
	        var regexParser = new Regex(CsiSelectCharacterProtectionAttributeSequenceRegex);
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
            var regexParser = new Regex(CsiPopVideoAttributesFromStackSequenceRegex);
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
            var regexParser = new Regex(CsiSetScrollingRegionSequenceRegex);
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
            var regexParser = new Regex(CsiRestoreDecPrivateModeValuesSequenceRegex);
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
            var regexParser = new Regex(CsiChangeAttributesInRectangularAreaSequenceRegex);
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
            var regexParser = new Regex(CsiSaveCursorSequenceRegex);
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
            var regexParser = new Regex(CsiSetLeftRightMarginsSequenceRegex);
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
            var regexParser = new Regex(CsiSetShiftEscapeOptionsSequenceRegex);
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
            var regexParser = new Regex(CsiDecPrivateModeValuesSequenceRegex);
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
            var regexParser = new Regex(CsiWindowManipulationSequenceRegex);
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
            var regexParser = new Regex(CsiSetTitleModeXtermSequenceRegex);
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
            var regexParser = new Regex(CsiSetWarningBellVolumeSequenceRegex);
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
            var regexParser = new Regex(CsiReverseAttributesInRectangularAreaSequenceRegex);
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
            var regexParser = new Regex(CsiRestoreCursorSequenceRegex);
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
            var regexParser = new Regex(CsiSetMarginBellVolumeSequenceRegex);
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
            var regexParser = new Regex(CsiCopyRectangularAreaSequenceRegex);
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
            var regexParser = new Regex(CsiRequestPresentationStateReportSequenceRegex);
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
            var regexParser = new Regex(CsiEnableFilterRectangleSequenceRegex);
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
            var regexParser = new Regex(CsiRequestTerminalParametersSequenceRegex);
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
            var regexParser = new Regex(CsiSelectAttributeChangeExtentSequenceRegex);
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
            var regexParser = new Regex(CsiFillRectangularAreaSequenceRegex);
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
            var regexParser = new Regex(CsiSelectChecksumExtensionSequenceRegex);
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
            var regexParser = new Regex(CsiRectangularAreaChecksumSequenceRegex);
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
            var regexParser = new Regex(CsiLocatorReportingSequenceRegex);
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
            var regexParser = new Regex(CsiEraseRectangularAreaSequenceRegex);
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
            var regexParser = new Regex(CsiSelectLocatorEventsSequenceRegex);
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
            var regexParser = new Regex(CsiPushVideoAttributesToStackXtermSequenceRegex);
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
            var regexParser = new Regex(CsiPushVideoAttributesToStackXtermWithArgsSequenceRegex);
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
            var regexParser = new Regex(CsiSelectiveEraseRectangularAreaSequenceRegex);
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
            var regexParser = new Regex(CsiReportGraphicsRenditionRectangularAreaSequenceRegex);
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
            var regexParser = new Regex(CsiSelectColumnsPerPageSequenceRegex);
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
            var regexParser = new Regex(CsiRequestLocatorPositionSequenceRegex);
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
            var regexParser = new Regex(CsiSelectNumberOfLinesPerScreenSequenceRegex);
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
            var regexParser = new Regex(CsiPopVideoAttributesFromStackXtermSequenceRegex);
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
            var regexParser = new Regex(CsiInsertColumnsSequenceRegex);
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
            var regexParser = new Regex(CsiSelectActiveStatusDisplaySequenceRegex);
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
            var regexParser = new Regex(CsiDeleteColumnsSequenceRegex);
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
            var regexParser = new Regex(CsiSelectStatusLineTypeSequenceRegex);
            if (!regexParser.IsMatch(result))
                throw new Exception("Terminaux failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }
    }
}
