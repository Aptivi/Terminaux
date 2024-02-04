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

using System.Text.RegularExpressions;
using Textify.Tools;

namespace Terminaux.Sequences.Builder.Types
{
    /// <summary>
    /// List of ESC sequences and their regular expressions
    /// </summary>
    public static class EscSequences
    {
        private static readonly Regex esc7BitControlsSequenceRegex = new(@"\x1b F", RegexOptions.Compiled);
        private static readonly Regex esc8BitControlsSequenceRegex = new(@"\x1b G", RegexOptions.Compiled);
        private static readonly Regex escAnsiConformanceLevel1SequenceRegex = new(@"\x1b L", RegexOptions.Compiled);
        private static readonly Regex escAnsiConformanceLevel2SequenceRegex = new(@"\x1b M", RegexOptions.Compiled);
        private static readonly Regex escAnsiConformanceLevel3SequenceRegex = new(@"\x1b N", RegexOptions.Compiled);
        private static readonly Regex escDecDoubleHeightLineTopHalfSequenceRegex = new(@"\x1b#3", RegexOptions.Compiled);
        private static readonly Regex escDecDoubleHeightLineBottomHalfSequenceRegex = new(@"\x1b#4", RegexOptions.Compiled);
        private static readonly Regex escDecSingleWidthLineSequenceRegex = new(@"\x1b#5", RegexOptions.Compiled);
        private static readonly Regex escDecDoubleWidthLineSequenceRegex = new(@"\x1b#6", RegexOptions.Compiled);
        private static readonly Regex escDecScreenAlignmentTestSequenceRegex = new(@"\x1b#8", RegexOptions.Compiled);
        private static readonly Regex escSelectDefaultCharacterSetSequenceRegex = new(@"\x1b%@", RegexOptions.Compiled);
        private static readonly Regex escSelectUtf8CharacterSetSequenceRegex = new(@"\x1b%G", RegexOptions.Compiled);
        private static readonly Regex escDesignateG0CharacterSetSequenceRegex = new(@"\x1b\(.+", RegexOptions.Compiled);
        private static readonly Regex escDesignateG1CharacterSetSequenceRegex = new(@"\x1b\).+", RegexOptions.Compiled);
        private static readonly Regex escDesignateG2CharacterSetSequenceRegex = new(@"\x1b\*.+", RegexOptions.Compiled);
        private static readonly Regex escDesignateG3CharacterSetSequenceRegex = new(@"\x1b\+.+", RegexOptions.Compiled);
        private static readonly Regex escDesignateG1CharacterSetAltSequenceRegex = new(@"\x1b\-.+", RegexOptions.Compiled);
        private static readonly Regex escDesignateG2CharacterSetAltSequenceRegex = new(@"\x1b,.+", RegexOptions.Compiled);
        private static readonly Regex escDesignateG3CharacterSetAltSequenceRegex = new(@"\x1b/.+", RegexOptions.Compiled);
        private static readonly Regex escBackIndexSequenceRegex = new(@"\x1b6", RegexOptions.Compiled);
        private static readonly Regex escSaveCursorSequenceRegex = new(@"\x1b7", RegexOptions.Compiled);
        private static readonly Regex escRestoreCursorSequenceRegex = new(@"\x1b8", RegexOptions.Compiled);
        private static readonly Regex escForwardIndexSequenceRegex = new(@"\x1b9", RegexOptions.Compiled);
        private static readonly Regex escApplicationKeypadSequenceRegex = new(@"\x1b=", RegexOptions.Compiled);
        private static readonly Regex escNormalKeypadSequenceRegex = new(@"\x1b\>", RegexOptions.Compiled);
        private static readonly Regex escCursorToLowerLeftCornerSequenceRegex = new(@"\x1bF", RegexOptions.Compiled);
        private static readonly Regex escFullResetSequenceRegex = new(@"\x1bc", RegexOptions.Compiled);
        private static readonly Regex escMemoryLockSequenceRegex = new(@"\x1bl", RegexOptions.Compiled);
        private static readonly Regex escMemoryUnlockSequenceRegex = new(@"\x1bm", RegexOptions.Compiled);
        private static readonly Regex escInvokeG2CharacterSetGlSequenceRegex = new(@"\x1bn", RegexOptions.Compiled);
        private static readonly Regex escInvokeG3CharacterSetGlSequenceRegex = new(@"\x1bo", RegexOptions.Compiled);
        private static readonly Regex escInvokeG2CharacterSetGrSequenceRegex = new(@"\x1b\}", RegexOptions.Compiled);
        private static readonly Regex escInvokeG3CharacterSetGrSequenceRegex = new(@"\x1b\|", RegexOptions.Compiled);
        private static readonly Regex escInvokeG1CharacterSetGrSequenceRegex = new(@"\x1b~", RegexOptions.Compiled);

        /// <summary>
        /// [ESC SP F] Regular expression for 7-bit controls
        /// </summary>
        public static Regex Esc7BitControlsSequenceRegex =>
            esc7BitControlsSequenceRegex;

        /// <summary>
        /// [ESC SP G] Regular expression for 8-bit controls
        /// </summary>
        public static Regex Esc8BitControlsSequenceRegex =>
            esc8BitControlsSequenceRegex;

        /// <summary>
        /// [ESC SP L] Regular expression for setting ANSI conformance level 1
        /// </summary>
        public static Regex EscAnsiConformanceLevel1SequenceRegex =>
            escAnsiConformanceLevel1SequenceRegex;

        /// <summary>
        /// [ESC SP M] Regular expression for setting ANSI conformance level 2
        /// </summary>
        public static Regex EscAnsiConformanceLevel2SequenceRegex =>
            escAnsiConformanceLevel2SequenceRegex;

        /// <summary>
        /// [ESC SP N] Regular expression for setting ANSI conformance level 3
        /// </summary>
        public static Regex EscAnsiConformanceLevel3SequenceRegex =>
            escAnsiConformanceLevel3SequenceRegex;

        /// <summary>
        /// [ESC # 3] Regular expression for DEC double-height line top half
        /// </summary>
        public static Regex EscDecDoubleHeightLineTopHalfSequenceRegex =>
            escDecDoubleHeightLineTopHalfSequenceRegex;

        /// <summary>
        /// [ESC # 4] Regular expression for DEC double-height line bottom half
        /// </summary>
        public static Regex EscDecDoubleHeightLineBottomHalfSequenceRegex =>
            escDecDoubleHeightLineBottomHalfSequenceRegex;

        /// <summary>
        /// [ESC # 5] Regular expression for DEC single-width line
        /// </summary>
        public static Regex EscDecSingleWidthLineSequenceRegex =>
            escDecSingleWidthLineSequenceRegex;

        /// <summary>
        /// [ESC # 6] Regular expression for DEC double-width line
        /// </summary>
        public static Regex EscDecDoubleWidthLineSequenceRegex =>
            escDecDoubleWidthLineSequenceRegex;

        /// <summary>
        /// [ESC # 8] Regular expression for DEC screen alignment test
        /// </summary>
        public static Regex EscDecScreenAlignmentTestSequenceRegex =>
            escDecScreenAlignmentTestSequenceRegex;

        /// <summary>
        /// [ESC % @] Regular expression for selecting default character set
        /// </summary>
        public static Regex EscSelectDefaultCharacterSetSequenceRegex =>
            escSelectDefaultCharacterSetSequenceRegex;

        /// <summary>
        /// [ESC % G] Regular expression for selecting UTF-8 character set
        /// </summary>
        public static Regex EscSelectUtf8CharacterSetSequenceRegex =>
            escSelectUtf8CharacterSetSequenceRegex;

        /// <summary>
        /// [ESC ( Pc] Regular expression for designating the G0 character set
        /// </summary>
        public static Regex EscDesignateG0CharacterSetSequenceRegex =>
            escDesignateG0CharacterSetSequenceRegex;

        /// <summary>
        /// [ESC ) Pc] Regular expression for designating the G1 character set
        /// </summary>
        public static Regex EscDesignateG1CharacterSetSequenceRegex =>
            escDesignateG1CharacterSetSequenceRegex;

        /// <summary>
        /// [ESC * Pc] Regular expression for designating the G2 character set
        /// </summary>
        public static Regex EscDesignateG2CharacterSetSequenceRegex =>
            escDesignateG2CharacterSetSequenceRegex;

        /// <summary>
        /// [ESC + Pc] Regular expression for designating the G3 character set
        /// </summary>
        public static Regex EscDesignateG3CharacterSetSequenceRegex =>
            escDesignateG3CharacterSetSequenceRegex;

        /// <summary>
        /// [ESC - Pc] Regular expression for designating the G1 character set
        /// </summary>
        public static Regex EscDesignateG1CharacterSetAltSequenceRegex =>
            escDesignateG1CharacterSetAltSequenceRegex;

        /// <summary>
        /// [ESC , Pc] Regular expression for designating the G2 character set
        /// </summary>
        public static Regex EscDesignateG2CharacterSetAltSequenceRegex =>
            escDesignateG2CharacterSetAltSequenceRegex;

        /// <summary>
        /// [ESC / Pc] Regular expression for designating the G3 character set
        /// </summary>
        public static Regex EscDesignateG3CharacterSetAltSequenceRegex =>
            escDesignateG3CharacterSetAltSequenceRegex;

        /// <summary>
        /// [ESC 6] Regular expression for back index
        /// </summary>
        public static Regex EscBackIndexSequenceRegex =>
            escBackIndexSequenceRegex;

        /// <summary>
        /// [ESC 7] Regular expression for saving cursor
        /// </summary>
        public static Regex EscSaveCursorSequenceRegex =>
            escSaveCursorSequenceRegex;

        /// <summary>
        /// [ESC 8] Regular expression for restoring cursor
        /// </summary>
        public static Regex EscRestoreCursorSequenceRegex =>
            escRestoreCursorSequenceRegex;

        /// <summary>
        /// [ESC 9] Regular expression for forward index
        /// </summary>
        public static Regex EscForwardIndexSequenceRegex =>
            escForwardIndexSequenceRegex;

        /// <summary>
        /// [ESC =] Regular expression for application keypad
        /// </summary>
        public static Regex EscApplicationKeypadSequenceRegex =>
            escApplicationKeypadSequenceRegex;

        /// <summary>
        /// [ESC >] Regular expression for normal keypad
        /// </summary>
        public static Regex EscNormalKeypadSequenceRegex =>
            escNormalKeypadSequenceRegex;

        /// <summary>
        /// [ESC F] Regular expression for cursor to lower left corner
        /// </summary>
        public static Regex EscCursorToLowerLeftCornerSequenceRegex =>
            escCursorToLowerLeftCornerSequenceRegex;

        /// <summary>
        /// [ESC c] Regular expression for full reset
        /// </summary>
        public static Regex EscFullResetSequenceRegex =>
            escFullResetSequenceRegex;

        /// <summary>
        /// [ESC l] Regular expression for memory lock
        /// </summary>
        public static Regex EscMemoryLockSequenceRegex =>
            escMemoryLockSequenceRegex;

        /// <summary>
        /// [ESC m] Regular expression for memory unlock
        /// </summary>
        public static Regex EscMemoryUnlockSequenceRegex =>
            escMemoryUnlockSequenceRegex;

        /// <summary>
        /// [ESC n] Regular expression for invoking the G2 character set as GL
        /// </summary>
        public static Regex EscInvokeG2CharacterSetGlSequenceRegex =>
            escInvokeG2CharacterSetGlSequenceRegex;

        /// <summary>
        /// [ESC o] Regular expression for invoking the G3 character set as GL
        /// </summary>
        public static Regex EscInvokeG3CharacterSetGlSequenceRegex =>
            escInvokeG3CharacterSetGlSequenceRegex;

        /// <summary>
        /// [ESC }] Regular expression for invoking the G2 character set as GR
        /// </summary>
        public static Regex EscInvokeG2CharacterSetGrSequenceRegex =>
            escInvokeG2CharacterSetGrSequenceRegex;

        /// <summary>
        /// [ESC |] Regular expression for invoking the G3 character set as GR
        /// </summary>
        public static Regex EscInvokeG3CharacterSetGrSequenceRegex =>
            escInvokeG3CharacterSetGrSequenceRegex;

        /// <summary>
        /// [ESC ~] Regular expression for invoking the G1 character set as GR
        /// </summary>
        public static Regex EscInvokeG1CharacterSetGrSequenceRegex =>
            escInvokeG1CharacterSetGrSequenceRegex;

        /// <summary>
        /// [ESC SP F] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEsc7BitControls()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} F";
            var regexParser = Esc7BitControlsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC SP G] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEsc8BitControls()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} G";
            var regexParser = Esc8BitControlsSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC SP L] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscAnsiConformanceLevel1()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} L";
            var regexParser = EscAnsiConformanceLevel1SequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC SP M] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscAnsiConformanceLevel2()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} M";
            var regexParser = EscAnsiConformanceLevel2SequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC SP N] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscAnsiConformanceLevel3()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} N";
            var regexParser = EscAnsiConformanceLevel3SequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC # 3] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecDoubleHeightLineTopHalf()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#3";
            var regexParser = EscDecDoubleHeightLineTopHalfSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC # 4] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecDoubleHeightLineBottomHalf()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#4";
            var regexParser = EscDecDoubleHeightLineBottomHalfSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC # 5] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecSingleWidthLine()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#5";
            var regexParser = EscDecSingleWidthLineSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC # 6] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecDoubleWidthLine()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#6";
            var regexParser = EscDecDoubleWidthLineSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC # 8] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDecScreenAlignmentTest()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#8";
            var regexParser = EscDecScreenAlignmentTestSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC % @] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscSelectDefaultCharacterSet()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}%@";
            var regexParser = EscSelectDefaultCharacterSetSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC % G] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscSelectUtf8CharacterSet()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}%G";
            var regexParser = EscSelectUtf8CharacterSetSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC ( Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG0CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}({charSet}";
            var regexParser = EscDesignateG0CharacterSetSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC ) Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG1CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}){charSet}";
            var regexParser = EscDesignateG1CharacterSetSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC * Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG2CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}*{charSet}";
            var regexParser = EscDesignateG2CharacterSetSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC + Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG3CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}+{charSet}";
            var regexParser = EscDesignateG3CharacterSetSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC - Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG1CharacterSetAlt(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}-{charSet}";
            var regexParser = EscDesignateG1CharacterSetAltSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC , Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG2CharacterSetAlt(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar},{charSet}";
            var regexParser = EscDesignateG2CharacterSetAltSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC / Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscDesignateG3CharacterSetAlt(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}/{charSet}";
            var regexParser = EscDesignateG3CharacterSetAltSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC 6] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscBackIndex()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}6";
            var regexParser = EscBackIndexSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC 7] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscSaveCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}7";
            var regexParser = EscSaveCursorSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC 8] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscRestoreCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}8";
            var regexParser = EscRestoreCursorSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC 9] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscForwardIndex()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}9";
            var regexParser = EscForwardIndexSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC =] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscApplicationKeypad()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}=";
            var regexParser = EscApplicationKeypadSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC >] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscNormalKeypad()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}>";
            var regexParser = EscNormalKeypadSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC F] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscCursorToLowerLeftCorner()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}F";
            var regexParser = EscCursorToLowerLeftCornerSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC c] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscFullReset()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}c";
            var regexParser = EscFullResetSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC l] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscMemoryLock()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}l";
            var regexParser = EscMemoryLockSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC m] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscMemoryUnlock()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}m";
            var regexParser = EscMemoryUnlockSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC n] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG2CharacterSetGl()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}n";
            var regexParser = EscInvokeG2CharacterSetGlSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC o] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG3CharacterSetGl()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}o";
            var regexParser = EscInvokeG3CharacterSetGlSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC |] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG3CharacterSetGr()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}|";
            var regexParser = EscInvokeG3CharacterSetGrSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC }] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG2CharacterSetGr()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}}}";
            var regexParser = EscInvokeG2CharacterSetGrSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }

        /// <summary>
        /// [ESC ~] Generates an escape sequence that can be used for the console
        /// </summary>
        public static string GenerateEscInvokeG1CharacterSetGr()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}~";
            var regexParser = EscInvokeG1CharacterSetGrSequenceRegex;
            if (!regexParser.IsMatch(result))
                throw new TextifyException("We have failed to generate a working VT sequence. Make sure that you've specified values correctly.");
            return result;
        }
    }
}
