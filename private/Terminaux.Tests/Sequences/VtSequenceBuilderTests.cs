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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder;
using Terminaux.Sequences.Builder.Types;

namespace Terminaux.Tests.Sequences
{
    [TestClass]
    public class VtSequenceBuilderTests
    {
        /// <summary>
        /// [APC Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Kermit")]
        public void TestGenerateApcApplicationProgramCommand(string proprietaryCommand)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}_{proprietaryCommand}\x9c";
            string actual = "";
            Should.NotThrow(() => actual = ApcSequences.GenerateApcApplicationProgramCommand(proprietaryCommand));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Apc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.ApcApplicationProgramCommand);
        }

        /// <summary>
        /// [ESC D] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1Index()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}D";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1Index());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1Index);
        }

        /// <summary>
        /// [ESC E] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1NextLine()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}E";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1NextLine());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1NextLine);
        }

        /// <summary>
        /// [ESC H] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1TabSet()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}H";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1TabSet());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1TabSet);
        }

        /// <summary>
        /// [ESC M] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1ReverseIndex()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}M";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1ReverseIndex());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1ReverseIndex);
        }

        /// <summary>
        /// [ESC N] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1SingleShiftSelectG2CharacterSet()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}N";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1SingleShiftSelectG2CharacterSet());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1SingleShiftSelectG2CharacterSet);
        }

        /// <summary>
        /// [ESC O] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1SingleShiftSelectG3CharacterSet()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}O";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1SingleShiftSelectG3CharacterSet());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1SingleShiftSelectG3CharacterSet);
        }

        /// <summary>
        /// [ESC P] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1DeviceControlString()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}P";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1DeviceControlString());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1DeviceControlString);
        }

        /// <summary>
        /// [ESC V] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1StartOfGuardedArea()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}V";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1StartOfGuardedArea());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1StartOfGuardedArea);
        }

        /// <summary>
        /// [ESC W] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1EndOfGuardedArea()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}W";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1EndOfGuardedArea());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1EndOfGuardedArea);
        }

        /// <summary>
        /// [ESC X] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1StartOfString()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}X";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1StartOfString());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1StartOfString);
        }

        /// <summary>
        /// [ESC Z] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1ReturnTerminalId()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}Z";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1ReturnTerminalId());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1ReturnTerminalId);
        }

        /// <summary>
        /// [ESC [] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1ControlSequenceIndicator()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1ControlSequenceIndicator());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1ControlSequenceIndicator);
        }

        /// <summary>
        /// [ESC \] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1StringTerminator()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}\\";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1StringTerminator());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1StringTerminator);
        }

        /// <summary>
        /// [ESC ]] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1OperatingSystemCommand()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}]";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1OperatingSystemCommand());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1OperatingSystemCommand);
        }

        /// <summary>
        /// [ESC ^] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1PrivacyMessage()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}^";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1PrivacyMessage());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1PrivacyMessage);
        }

        /// <summary>
        /// [ESC _] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateC1ApplicationProgramCommand()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}_";
            string actual = "";
            Should.NotThrow(() => actual = C1Sequences.GenerateC1ApplicationProgramCommand());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.C1);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.C1ApplicationProgramCommand);
        }

        /// <summary>
        /// [CSI Ps @] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiInsertBlankCharacters(int blanks)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{blanks}@";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiInsertBlankCharacters(blanks));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiInsertBlankCharacters);
        }

        /// <summary>
        /// [CSI Ps SP @] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiShiftLeftColumns(int columns)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columns} @";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiShiftLeftColumns(columns));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiShiftLeftColumns);
        }

        /// <summary>
        /// [CSI Ps A] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiMoveCursorUp(int times)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{times}A";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMoveCursorUp(times));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMoveCursorUp);
        }

        /// <summary>
        /// [CSI Ps SP A] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiShiftRightColumns(int columns)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columns} A";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiShiftRightColumns(columns));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiShiftRightColumns);
        }

        /// <summary>
        /// [CSI Ps B] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiMoveCursorDown(int times)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{times}B";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMoveCursorDown(times));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMoveCursorDown);
        }

        /// <summary>
        /// [CSI Ps C] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiMoveCursorRight(int times)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{times}C";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMoveCursorRight(times));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMoveCursorRight);
        }

        /// <summary>
        /// [CSI Ps D] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiMoveCursorLeft(int times)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{times}D";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMoveCursorLeft(times));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMoveCursorLeft);
        }

        /// <summary>
        /// [CSI Ps E] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiMoveCursorNextLine(int times)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{times}E";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMoveCursorNextLine(times));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMoveCursorNextLine);
        }

        /// <summary>
        /// [CSI Ps F] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiMoveCursorPreviousLine(int times)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{times}F";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMoveCursorPreviousLine(times));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMoveCursorPreviousLine);
        }

        /// <summary>
        /// [CSI Ps G] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiCursorCharacterAbsoluteLine(int column)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{column}G";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCursorCharacterAbsoluteLine(column));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCursorCharacterAbsoluteLine);
        }

        /// <summary>
        /// [CSI Ps ; Ps H] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2, 2)]
        public void TestGenerateCsiCursorPosition(int column, int row)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{row};{column}H";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCursorPosition(column, row));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCursorPosition);
        }

        /// <summary>
        /// [CSI Ps I] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiCursorForwardTabulation(int stops)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{stops}I";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCursorForwardTabulation(stops));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCursorForwardTabulation);
        }

        /// <summary>
        /// [CSI Ps J] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiEraseInDisplay(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}J";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiEraseInDisplay(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiEraseInDisplay);
        }

        /// <summary>
        /// [CSI ? Ps J] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiEraseInDisplayDecsed(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}J";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiEraseInDisplayDecsed(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiEraseInDisplayDecsed);
        }

        /// <summary>
        /// [CSI Ps K] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiEraseInLine(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}K";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiEraseInLine(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiEraseInLine);
        }

        /// <summary>
        /// [CSI ? Ps K] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiEraseInLineDecsel(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}K";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiEraseInLineDecsel(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiEraseInLineDecsel);
        }

        /// <summary>
        /// [CSI Ps L] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiInsertLine(int lines)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}L";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiInsertLine(lines));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiInsertLine);
        }

        /// <summary>
        /// [CSI Ps M] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiDeleteLine(int lines)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}M";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiDeleteLine(lines));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiDeleteLine);
        }

        /// <summary>
        /// [CSI Ps P] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiDeleteChars(int chars)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{chars}P";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiDeleteChars(chars));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiDeleteChars);
        }

        /// <summary>
        /// [CSI # P] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiPushColorToStack()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#P";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPushColorToStack());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPushColorToStack);
        }

        /// <summary>
        /// [CSI Pm # P] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("2;5")]
        public void TestGenerateCsiPushColorToStackWithArgs(string parameters)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}#P";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPushColorToStackWithArgs(parameters));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPushColorToStackWithArgs);
        }

        /// <summary>
        /// [CSI # Q] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiPopColorFromStack()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#Q";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPopColorFromStack());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPopColorFromStack);
        }

        /// <summary>
        /// [CSI Pm # Q] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("2;5")]
        public void TestGenerateCsiPopColorFromStackWithArgs(string parameters)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}#Q";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPopColorFromStackWithArgs(parameters));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPopColorFromStackWithArgs);
        }

        /// <summary>
        /// [CSI # R] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiReportPaletteStack()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#R";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiReportPaletteStack());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiReportPaletteStack);
        }

        /// <summary>
        /// [CSI Ps S] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiScrollUp(int lines)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}S";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiScrollUp(lines));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiScrollUp);
        }

        /// <summary>
        /// [CSI ? Pi ; Pa ; Pv S] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 2, "4;5")]
        public void TestGenerateCsiSetGraphicsAttribute(int itemType, int attributeManager, string geometry)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{itemType};{attributeManager};{geometry}S";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetGraphicsAttribute(itemType, attributeManager, geometry));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetGraphicsAttribute);
        }

        /// <summary>
        /// [CSI Ps T] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiScrollDown(int lines)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}T";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiScrollDown(lines));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiScrollDown);
        }

        /// <summary>
        /// [CSI Ps ; Ps ; Ps ; Ps ; Ps T] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 2, 3, 4, 5)]
        public void TestGenerateCsiInitiateHighlightMouseTracking(int func, int startX, int startY, int firstRow, int lastRow)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{func};{startX};{startY};{firstRow};{lastRow}T";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiInitiateHighlightMouseTracking(func, startX, startY, firstRow, lastRow));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiInitiateHighlightMouseTracking);
        }

        /// <summary>
        /// [CSI > Pm T] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiResetTitleModeFeatures(int resetOptions)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{resetOptions}T";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiResetTitleModeFeatures(resetOptions));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiResetTitleModeFeatures);
        }

        /// <summary>
        /// [CSI Ps X] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiEraseCharacters(int chars)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{chars}X";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiEraseCharacters(chars));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiEraseCharacters);
        }

        /// <summary>
        /// [CSI Ps Z] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiCursorBackwardTabulation(int stops)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{stops}Z";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCursorBackwardTabulation(stops));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCursorBackwardTabulation);
        }

        /// <summary>
        /// [CSI Ps ^] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiScrollDownOriginal(int lines)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}^";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiScrollDownOriginal(lines));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiScrollDownOriginal);
        }

        /// <summary>
        /// [CSI Ps `] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiCursorPositionAbsolute(int column)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{column}`";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCursorPositionAbsolute(column));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCursorPositionAbsolute);
        }

        /// <summary>
        /// [CSI Ps a] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiCursorPositionRelative(int column)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{column}a";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCursorPositionRelative(column));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCursorPositionRelative);
        }

        /// <summary>
        /// [CSI Ps b] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiRepeatGraphicCharacter(int times)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{times}b";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRepeatGraphicCharacter(times));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRepeatGraphicCharacter);
        }

        /// <summary>
        /// [CSI Ps c] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiSendDeviceAttributesPrimary(int attributes)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{attributes}c";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSendDeviceAttributesPrimary(attributes));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSendDeviceAttributesPrimary);
        }

        /// <summary>
        /// [CSI = Ps c] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiSendDeviceAttributesSecondary(int attributes)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[={attributes}c";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSendDeviceAttributesSecondary(attributes));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSendDeviceAttributesSecondary);
        }

        /// <summary>
        /// [CSI > Ps c] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiSendDeviceAttributesTertiary(int attributes)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{attributes}c";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSendDeviceAttributesTertiary(attributes));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSendDeviceAttributesTertiary);
        }

        /// <summary>
        /// [CSI Ps d] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiLinePositionAbsolute(int row)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{row}d";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiLinePositionAbsolute(row));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiLinePositionAbsolute);
        }

        /// <summary>
        /// [CSI Ps e] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiLinePositionRelative(int row)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{row}e";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiLinePositionRelative(row));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiLinePositionRelative);
        }

        /// <summary>
        /// [CSI Ps ; Ps f] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2, 2)]
        public void TestGenerateCsiLeftTopPosition(int column, int row)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{row};{column}f";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiLeftTopPosition(column, row));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiLeftTopPosition);
        }

        /// <summary>
        /// [CSI Ps g] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(3)]
        public void TestGenerateCsiTabClear(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}g";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiTabClear(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiTabClear);
        }

        /// <summary>
        /// [CSI Pm h] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("20")]
        public void TestGenerateCsiSetMode(string mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}h";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetMode(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetMode);
        }

        /// <summary>
        /// [CSI ? Pm h] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("30")]
        public void TestGenerateCsiSetPrivateMode(string mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}h";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetPrivateMode(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetPrivateMode);
        }

        /// <summary>
        /// [CSI Ps i] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(4)]
        public void TestGenerateCsiMediaCopy(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}i";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMediaCopy(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMediaCopy);
        }

        /// <summary>
        /// [CSI ? Ps i] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(4)]
        public void TestGenerateCsiMediaCopyPrivate(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}i";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiMediaCopyPrivate(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiMediaCopyPrivate);
        }

        /// <summary>
        /// [CSI Pm l] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("4")]
        public void TestGenerateCsiResetMode(string mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}l";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiResetMode(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiResetMode);
        }

        /// <summary>
        /// [CSI ? Pm l] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("2")]
        public void TestGenerateCsiResetPrivateMode(string mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}l";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiResetPrivateMode(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiResetPrivateMode);
        }

        /// <summary>
        /// [CSI Pm m] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("38;2;3;2;1")]
        public void TestGenerateCsiCharacterAttributes(string mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}m";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCharacterAttributes(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCharacterAttributes);
        }

        /// <summary>
        /// [CSI > Pp ; Pv m] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(4, 1)]
        public void TestGenerateCsiSetKeyModifierOptions(int resource, int modify)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{resource};{modify}m";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetKeyModifierOptions(resource, modify));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetKeyModifierOptions);
        }

        /// <summary>
        /// [CSI > Pp m] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(4)]
        public void TestGenerateCsiResetKeyModifierOptions(int resource)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{resource}m";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiResetKeyModifierOptions(resource));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiResetKeyModifierOptions);
        }

        /// <summary>
        /// [CSI ? Pp m] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(4)]
        public void TestGenerateCsiQueryKeyModifierOptions(int resource)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{resource}m";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiQueryKeyModifierOptions(resource));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiQueryKeyModifierOptions);
        }

        /// <summary>
        /// [CSI Ps n] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(6)]
        public void TestGenerateCsiDeviceStatusReport(int report)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{report}n";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiDeviceStatusReport(report));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiDeviceStatusReport);
        }

        /// <summary>
        /// [CSI > Ps n] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(4)]
        public void TestGenerateCsiDisableKeyModifierOptions(int report)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{report}n";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiDisableKeyModifierOptions(report));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiDisableKeyModifierOptions);
        }

        /// <summary>
        /// [CSI ? Ps n] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(6)]
        public void TestGenerateCsiDeviceStatusReportDec(int report)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{report}n";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiDeviceStatusReportDec(report));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiDeviceStatusReportDec);
        }

        /// <summary>
        /// [CSI > Ps p] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1)]
        public void TestGenerateCsiSetPointerModeXterm(int hideMode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{hideMode}p";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetPointerModeXterm(hideMode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetPointerModeXterm);
        }

        /// <summary>
        /// [CSI ! p] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiSoftTerminalReset()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[!p";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSoftTerminalReset());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSoftTerminalReset);
        }

        /// <summary>
        /// [CSI Pl ; Pc " p] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(61, 0)]
        public void TestGenerateCsiSetConformanceLevel(int level, int conformance)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{level};{conformance}\"p";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetConformanceLevel(level, conformance));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetConformanceLevel);
        }

        /// <summary>
        /// [CSI Ps $ p] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiRequestAnsiMode(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}$p";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRequestAnsiMode(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRequestAnsiMode);
        }

        /// <summary>
        /// [CSI ? Ps $ p] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiRequestDecPrivateMode(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{mode}$p";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRequestDecPrivateMode(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRequestDecPrivateMode);
        }

        /// <summary>
        /// [CSI # p] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiPushVideoAttributesToStack()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#p";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPushVideoAttributesToStack());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPushVideoAttributesToStack);
        }

        /// <summary>
        /// [CSI Pm # p] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("2;5")]
        public void TestGenerateCsiPushVideoAttributesToStackWithArgs(string args)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{args}#p";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPushVideoAttributesToStackWithArgs(args));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPushVideoAttributesToStackWithArgs);
        }

        /// <summary>
        /// [CSI > Ps q] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiReportXtermVersion(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{mode}q";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiReportXtermVersion(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiReportXtermVersion);
        }

        /// <summary>
        /// [CSI Ps q] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiLoadLeds(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}q";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiLoadLeds(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiLoadLeds);
        }

        /// <summary>
        /// [CSI Ps SP q] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiSetCursorStyle(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode} q";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetCursorStyle(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetCursorStyle);
        }

        /// <summary>
        /// [CSI Ps " q] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiSelectCharacterProtectionAttribute(int mode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{mode}\"q";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectCharacterProtectionAttribute(mode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectCharacterProtectionAttribute);
        }

        /// <summary>
        /// [CSI # q] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiPopVideoAttributesFromStack()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#q";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPopVideoAttributesFromStack());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPopVideoAttributesFromStack);
        }

        /// <summary>
        /// [CSI Ps ; Ps r] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0, 22)]
        public void TestGenerateCsiSetScrollingRegion(int top, int bottom)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{top};{bottom}r";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetScrollingRegion(top, bottom));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetScrollingRegion);
        }

        /// <summary>
        /// [CSI ? Pm r] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("1;2")]
        public void TestGenerateCsiRestoreDecPrivateModeValues(string values)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{values}r";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRestoreDecPrivateModeValues(values));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRestoreDecPrivateModeValues);
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ r] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 3, 3, 7, "38;2;56;21;255")]
        public void TestGenerateCsiChangeAttributesInRectangularArea(int pt, int pl, int pb, int pr, string attributes)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr};{attributes}$r";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiChangeAttributesInRectangularArea(pt, pl, pb, pr, attributes));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiChangeAttributesInRectangularArea);
        }

        /// <summary>
        /// [CSI s] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiSaveCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[s";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSaveCursor());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSaveCursor);
        }

        /// <summary>
        /// [CSI Pl ; Pr s] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0, 22)]
        public void TestGenerateCsiSetLeftRightMargins(int left, int right)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{left};{right}s";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetLeftRightMargins(left, right));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetLeftRightMargins);
        }

        /// <summary>
        /// [CSI > Ps s] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiSetShiftEscapeOptions(int option)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{option}s";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetShiftEscapeOptions(option));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetShiftEscapeOptions);
        }

        /// <summary>
        /// [CSI ? Pm s] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("1;2")]
        public void TestGenerateCsiDecPrivateModeValues(string values)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[?{values}s";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiDecPrivateModeValues(values));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiDecPrivateModeValues);
        }

        /// <summary>
        /// [CSI Ps ; Ps ; Ps ; t] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 6, 7)]
        public void TestGenerateCsiWindowManipulation(int windowAction, int windowAction2, int windowAction3)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{windowAction};{windowAction2};{windowAction3}t";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiWindowManipulation(windowAction, windowAction2, windowAction3));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiWindowManipulation);
        }

        /// <summary>
        /// [CSI > Pm t] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("1;2")]
        public void TestGenerateCsiSetTitleModeXterm(string modes)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[>{modes}t";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetTitleModeXterm(modes));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetTitleModeXterm);
        }

        /// <summary>
        /// [CSI Ps SP t] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(8)]
        public void TestGenerateCsiSetWarningBellVolume(int level)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{level} t";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetWarningBellVolume(level));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetWarningBellVolume);
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pm $ t] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 3, 3, 7, "38;2;56;21;255")]
        public void TestGenerateCsiReverseAttributesInRectangularArea(int pt, int pl, int pb, int pr, string attributes)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr};{attributes}$t";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiReverseAttributesInRectangularArea(pt, pl, pb, pr, attributes));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiReverseAttributesInRectangularArea);
        }

        /// <summary>
        /// [CSI u] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiRestoreCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[u";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRestoreCursor());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRestoreCursor);
        }

        /// <summary>
        /// [CSI Ps SP u] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(8)]
        public void TestGenerateCsiSetMarginBellVolume(int level)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{level} u";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSetMarginBellVolume(level));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSetMarginBellVolume);
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ; Pp ; Pt ; Pl ; Pp $ v] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 3, 3, 7, 1, 6, 6, 2)]
        public void TestGenerateCsiCopyRectangularArea(int ptSrc, int plSrc, int pbSrc, int prSrc, int sourcePage, int ptTarget, int plTarget, int targetPage)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{ptSrc};{plSrc};{pbSrc};{prSrc};{sourcePage};{ptTarget};{plTarget};{targetPage}$v";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiCopyRectangularArea(ptSrc, plSrc, pbSrc, prSrc, sourcePage, ptTarget, plTarget, targetPage));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiCopyRectangularArea);
        }

        /// <summary>
        /// [CSI Ps $ w] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiRequestPresentationStateReport(int state)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{state}$w";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRequestPresentationStateReport(state));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRequestPresentationStateReport);
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr ' w] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 3, 3, 7)]
        public void TestGenerateCsiEnableFilterRectangle(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}'w";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiEnableFilterRectangle(pt, pl, pb, pr));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiEnableFilterRectangle);
        }

        /// <summary>
        /// [CSI Ps x] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1)]
        public void TestGenerateCsiRequestTerminalParameters(int parameter)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameter}x";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRequestTerminalParameters(parameter));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRequestTerminalParameters);
        }

        /// <summary>
        /// [CSI Ps * x] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1)]
        public void TestGenerateCsiSelectAttributeChangeExtent(int extent)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{extent}*x";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectAttributeChangeExtent(extent));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectAttributeChangeExtent);
        }

        /// <summary>
        /// [CSI Pc ; Pt ; Pl ; Pb ; Pr $ x] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(' ', 1, 3, 3, 7)]
        public void TestGenerateCsiFillRectangularArea(char character, int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{character};{pt};{pl};{pb};{pr}$x";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiFillRectangularArea(character, pt, pl, pb, pr));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiFillRectangularArea);
        }

        /// <summary>
        /// [CSI Ps # y] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1)]
        public void TestGenerateCsiSelectChecksumExtension(int modifier)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{modifier}#y";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectChecksumExtension(modifier));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectChecksumExtension);
        }

        /// <summary>
        /// [CSI Pi ; Pg ; Pt ; Pl ; Pb ; Pr * y] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(434, 0, 1, 3, 3, 7)]
        public void TestGenerateCsiRectangularAreaChecksum(int requestId, int pageNumber, int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{requestId};{pageNumber};{pt};{pl};{pb};{pr}*y";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRectangularAreaChecksum(requestId, pageNumber, pt, pl, pb, pr));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRectangularAreaChecksum);
        }

        /// <summary>
        /// [CSI Ps ; Pu ' z] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0, 0)]
        public void TestGenerateCsiLocatorReporting(int locatorMode, int measurement)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{locatorMode};{measurement}'z";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiLocatorReporting(locatorMode, measurement));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiLocatorReporting);
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ z] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 3, 3, 7)]
        public void TestGenerateCsiEraseRectangularArea(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}$z";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiEraseRectangularArea(pt, pl, pb, pr));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiEraseRectangularArea);
        }

        /// <summary>
        /// [CSI Pm ' {] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("1;3")]
        public void TestGenerateCsiSelectLocatorEvents(string parameters)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}'{{";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectLocatorEvents(parameters));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectLocatorEvents);
        }

        /// <summary>
        /// [CSI # {] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiPushVideoAttributesToStackXterm()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#{{";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPushVideoAttributesToStackXterm());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPushVideoAttributesToStackXterm);
        }

        /// <summary>
        /// [CSI Pm # {] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("1;2")]
        public void TestGenerateCsiPushVideoAttributesToStackXtermWithArgs(string parameters)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{parameters}#{{";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPushVideoAttributesToStackXtermWithArgs(parameters));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPushVideoAttributesToStackXtermWithArgs);
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr $ {] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 3, 3, 7)]
        public void TestGenerateCsiSelectiveEraseRectangularArea(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}${{";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectiveEraseRectangularArea(pt, pl, pb, pr));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectiveEraseRectangularArea);
        }

        /// <summary>
        /// [CSI Pt ; Pl ; Pb ; Pr # |] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(1, 3, 3, 7)]
        public void TestGenerateCsiReportGraphicsRenditionRectangularArea(int pt, int pl, int pb, int pr)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{pt};{pl};{pb};{pr}#|";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiReportGraphicsRenditionRectangularArea(pt, pl, pb, pr));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiReportGraphicsRenditionRectangularArea);
        }

        /// <summary>
        /// [CSI Ps $ |] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(80)]
        public void TestGenerateCsiSelectColumnsPerPage(int columnMode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columnMode}$|";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectColumnsPerPage(columnMode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectColumnsPerPage);
        }

        /// <summary>
        /// [CSI Ps ' |] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiRequestLocatorPosition(int transmit)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{transmit}'|";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiRequestLocatorPosition(transmit));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiRequestLocatorPosition);
        }

        /// <summary>
        /// [CSI Ps * |] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiSelectNumberOfLinesPerScreen(int lines)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{lines}*|";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectNumberOfLinesPerScreen(lines));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectNumberOfLinesPerScreen);
        }

        /// <summary>
        /// [CSI # }] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateCsiPopVideoAttributesFromStackXterm()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[#}}";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiPopVideoAttributesFromStackXterm());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiPopVideoAttributesFromStackXterm);
        }

        /// <summary>
        /// [CSI Ps ' }] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(5)]
        public void TestGenerateCsiInsertColumns(int columns)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columns}'}}";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiInsertColumns(columns));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiInsertColumns);
        }

        /// <summary>
        /// [CSI Ps $ }] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        public void TestGenerateCsiSelectActiveStatusDisplay(int displayMode)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{displayMode}$}}";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectActiveStatusDisplay(displayMode));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectActiveStatusDisplay);
        }

        /// <summary>
        /// [CSI Ps ' ~] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(5)]
        public void TestGenerateCsiDeleteColumns(int columns)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{columns}'~";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiDeleteColumns(columns));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiDeleteColumns);
        }

        /// <summary>
        /// [CSI Ps $ ~] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(2)]
        public void TestGenerateCsiSelectStatusLineType(int type)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}[{type}$~";
            string actual = "";
            Should.NotThrow(() => actual = CsiSequences.GenerateCsiSelectStatusLineType(type));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Csi);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.CsiSelectStatusLineType);
        }

        /// <summary>
        /// [DCS Ps ; Ps | Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0, 0, "17/18;19/20")]
        public void TestGenerateDcsUserDefinedKeys(int clearUdkDefinitions, int dontLockKeys, string keybindings)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}P{clearUdkDefinitions};{dontLockKeys}|{keybindings}\x9c";
            string actual = "";
            Should.NotThrow(() => actual = DcsSequences.GenerateDcsUserDefinedKeys(clearUdkDefinitions, dontLockKeys, keybindings));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Dcs);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.DcsUserDefinedKeys);
        }

        /// <summary>
        /// [DCS $ q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("$|")]
        public void TestGenerateDcsRequestStatusString(string status)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}P$q{status}\x9c";
            string actual = "";
            Should.NotThrow(() => actual = DcsSequences.GenerateDcsRequestStatusString(status));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Dcs);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.DcsRequestStatusString);
        }

        /// <summary>
        /// [DCS Ps $ t Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow(0, "1")]
        public void TestGenerateDcsRestorePresentationStatus(int controlConvert, string status)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}P{controlConvert}$t{status}\x9c";
            string actual = "";
            Should.NotThrow(() => actual = DcsSequences.GenerateDcsRestorePresentationStatus(controlConvert, status));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Dcs);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.DcsRestorePresentationStatus);
        }

        /// <summary>
        /// [DCS + Q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("776964654368617273")]
        public void TestGenerateDcsRequestResourceValues(string xtermResourceNames)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}P+Q{xtermResourceNames}\x9c";
            string actual = "";
            Should.NotThrow(() => actual = DcsSequences.GenerateDcsRequestResourceValues(xtermResourceNames));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Dcs);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.DcsRequestResourceValues);
        }

        /// <summary>
        /// [DCS + p Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("787465726D")]
        public void TestGenerateDcsSetTermInfoData(string termName)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}P+p{termName}\x9c";
            string actual = "";
            Should.NotThrow(() => actual = DcsSequences.GenerateDcsSetTermInfoData(termName));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Dcs);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.DcsSetTermInfoData);
        }

        /// <summary>
        /// [DCS + q Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("787465726D")]
        public void TestGenerateDcsRequestTermInfoData(string termName)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}P+q{termName}\x9c";
            string actual = "";
            Should.NotThrow(() => actual = DcsSequences.GenerateDcsRequestTermInfoData(termName));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Dcs);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.DcsRequestTermInfoData);
        }

        /// <summary>
        /// [ESC SP F] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEsc7BitControls()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} F";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEsc7BitControls());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.Esc7BitControls);
        }

        /// <summary>
        /// [ESC SP G] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEsc8BitControls()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} G";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEsc8BitControls());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.Esc8BitControls);
        }

        /// <summary>
        /// [ESC SP L] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscAnsiConformanceLevel1()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} L";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscAnsiConformanceLevel1());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscAnsiConformanceLevel1);
        }

        /// <summary>
        /// [ESC SP M] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscAnsiConformanceLevel2()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} M";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscAnsiConformanceLevel2());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscAnsiConformanceLevel2);
        }

        /// <summary>
        /// [ESC SP N] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscAnsiConformanceLevel3()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar} N";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscAnsiConformanceLevel3());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscAnsiConformanceLevel3);
        }

        /// <summary>
        /// [ESC # 3] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscDecDoubleHeightLineTopHalf()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#3";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDecDoubleHeightLineTopHalf());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDecDoubleHeightLineTopHalf);
        }

        /// <summary>
        /// [ESC # 4] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscDecDoubleHeightLineBottomHalf()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#4";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDecDoubleHeightLineBottomHalf());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDecDoubleHeightLineBottomHalf);
        }

        /// <summary>
        /// [ESC # 5] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscDecSingleWidthLine()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#5";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDecSingleWidthLine());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDecSingleWidthLine);
        }

        /// <summary>
        /// [ESC # 6] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscDecDoubleWidthLine()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#6";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDecDoubleWidthLine());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDecDoubleWidthLine);
        }

        /// <summary>
        /// [ESC # 8] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscDecScreenAlignmentTest()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}#8";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDecScreenAlignmentTest());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDecScreenAlignmentTest);
        }

        /// <summary>
        /// [ESC % @] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscSelectDefaultCharacterSet()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}%@";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscSelectDefaultCharacterSet());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscSelectDefaultCharacterSet);
        }

        /// <summary>
        /// [ESC % G] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscSelectUtf8CharacterSet()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}%G";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscSelectUtf8CharacterSet());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscSelectUtf8CharacterSet);
        }

        /// <summary>
        /// [ESC ( Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Z")]
        public void TestGenerateEscDesignateG0CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}({charSet}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDesignateG0CharacterSet(charSet));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDesignateG0CharacterSet);
        }

        /// <summary>
        /// [ESC ) Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Z")]
        public void TestGenerateEscDesignateG1CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}){charSet}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDesignateG1CharacterSet(charSet));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDesignateG1CharacterSet);
        }

        /// <summary>
        /// [ESC * Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Z")]
        public void TestGenerateEscDesignateG2CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}*{charSet}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDesignateG2CharacterSet(charSet));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDesignateG2CharacterSet);
        }

        /// <summary>
        /// [ESC + Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Z")]
        public void TestGenerateEscDesignateG3CharacterSet(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}+{charSet}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDesignateG3CharacterSet(charSet));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDesignateG3CharacterSet);
        }

        /// <summary>
        /// [ESC - Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Z")]
        public void TestGenerateEscDesignateG1CharacterSetAlt(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}-{charSet}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDesignateG1CharacterSetAlt(charSet));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDesignateG1CharacterSetAlt);
        }

        /// <summary>
        /// [ESC , Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Z")]
        public void TestGenerateEscDesignateG2CharacterSetAlt(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar},{charSet}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDesignateG2CharacterSetAlt(charSet));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDesignateG2CharacterSetAlt);
        }

        /// <summary>
        /// [ESC / Pc] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Z")]
        public void TestGenerateEscDesignateG3CharacterSetAlt(string charSet)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}/{charSet}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscDesignateG3CharacterSetAlt(charSet));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscDesignateG3CharacterSetAlt);
        }

        /// <summary>
        /// [ESC 6] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscBackIndex()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}6";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscBackIndex());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscBackIndex);
        }

        /// <summary>
        /// [ESC 7] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscSaveCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}7";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscSaveCursor());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscSaveCursor);
        }

        /// <summary>
        /// [ESC 8] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscRestoreCursor()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}8";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscRestoreCursor());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscRestoreCursor);
        }

        /// <summary>
        /// [ESC 9] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscForwardIndex()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}9";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscForwardIndex());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscForwardIndex);
        }

        /// <summary>
        /// [ESC =] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscApplicationKeypad()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}=";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscApplicationKeypad());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscApplicationKeypad);
        }

        /// <summary>
        /// [ESC >] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscNormalKeypad()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}>";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscNormalKeypad());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscNormalKeypad);
        }

        /// <summary>
        /// [ESC F] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscCursorToLowerLeftCorner()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}F";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscCursorToLowerLeftCorner());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscCursorToLowerLeftCorner);
        }

        /// <summary>
        /// [ESC c] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscFullReset()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}c";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscFullReset());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscFullReset);
        }

        /// <summary>
        /// [ESC l] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscMemoryLock()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}l";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscMemoryLock());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscMemoryLock);
        }

        /// <summary>
        /// [ESC m] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscMemoryUnlock()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}m";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscMemoryUnlock());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscMemoryUnlock);
        }

        /// <summary>
        /// [ESC n] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscInvokeG2CharacterSetGl()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}n";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscInvokeG2CharacterSetGl());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscInvokeG2CharacterSetGl);
        }

        /// <summary>
        /// [ESC o] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscInvokeG3CharacterSetGl()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}o";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscInvokeG3CharacterSetGl());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscInvokeG3CharacterSetGl);
        }

        /// <summary>
        /// [ESC |] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscInvokeG3CharacterSetGr()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}|";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscInvokeG3CharacterSetGr());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscInvokeG3CharacterSetGr);
        }

        /// <summary>
        /// [ESC }] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscInvokeG2CharacterSetGr()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}}}";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscInvokeG2CharacterSetGr());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscInvokeG2CharacterSetGr);
        }

        /// <summary>
        /// [ESC ~] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        public void TestGenerateEscInvokeG1CharacterSetGr()
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}~";
            string actual = "";
            Should.NotThrow(() => actual = EscSequences.GenerateEscInvokeG1CharacterSetGr());
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Esc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.EscInvokeG1CharacterSetGr);
        }

        /// <summary>
        /// [OSC Ps ; Pt BEL] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow($"0;Hello")]
        public void TestGenerateOscOperatingSystemCommand(string proprietaryCommands)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}]{proprietaryCommands}{VtSequenceBasicChars.BellChar}";
            string actual = "";
            Should.NotThrow(() => actual = OscSequences.GenerateOscOperatingSystemCommand(proprietaryCommands));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Osc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.OscOperatingSystemCommand);
        }

        /// <summary>
        /// [OSC Ps ; Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("0;Hello")]
        public void TestGenerateOscOperatingSystemCommandAlt(string proprietaryCommands)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}]{proprietaryCommands}{VtSequenceBasicChars.StChar}";
            string actual = "";
            Should.NotThrow(() => actual = OscSequences.GenerateOscOperatingSystemCommandAlt(proprietaryCommands));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Osc);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.OscOperatingSystemCommandAlt);
        }

        /// <summary>
        /// [PM Pt ST] Generates an escape sequence that can be used for the console
        /// </summary>
        [TestMethod]
        [DataRow("Kermit")]
        public void TestGeneratePmPrivacyMessage(string proprietaryCommands)
        {
            string result = $"{VtSequenceBasicChars.EscapeChar}^{proprietaryCommands}{VtSequenceBasicChars.StChar}";
            string actual = "";
            Should.NotThrow(() => actual = PmSequences.GeneratePmPrivacyMessage(proprietaryCommands));
            actual.ShouldBe(result);
            (VtSequenceType type, VtSequenceSpecificTypes specificType) tuple = default;
            Should.NotThrow(() => tuple = VtSequenceBuilderTools.DetermineTypesFromSequence(actual).Single());
            tuple.type.ShouldBe(VtSequenceType.Pm);
            tuple.specificType.ShouldBe(VtSequenceSpecificTypes.PmPrivacyMessage);
        }

        /// <summary>
        /// Builds VT sequence with arguments
        /// </summary>
        [TestMethod]
        [DataRow("\x1B[1;3;3;7$z", VtSequenceSpecificTypes.CsiEraseRectangularArea, 1, 3, 3, 7)]
        [DataRow("\x1B^Kermit\x9C", VtSequenceSpecificTypes.PmPrivacyMessage, "Kermit")]
        [DataRow("\x1B]0;Hello\x07", VtSequenceSpecificTypes.OscOperatingSystemCommand, "0;Hello")]
        [DataRow("\x1B]0;Hello\x9C", VtSequenceSpecificTypes.OscOperatingSystemCommandAlt, "0;Hello")]
        [DataRow("\x1B,Z", VtSequenceSpecificTypes.EscDesignateG2CharacterSetAlt, "Z")]
        [DataRow("\x1BP+Q776964654368617273\x9C", VtSequenceSpecificTypes.DcsRequestResourceValues, "776964654368617273")]
        [DataRow("\x1B_Kermit\x9C", VtSequenceSpecificTypes.ApcApplicationProgramCommand, "Kermit")]
        public void TestBuildVtSequence(string expected, VtSequenceSpecificTypes specificType, params object[] arguments)
        {
            string actual = "";
            Should.NotThrow(() => actual = VtSequenceBuilderTools.BuildVtSequence(specificType, arguments));
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Builds VT sequence without arguments
        /// </summary>
        [TestMethod]
        [DataRow("\x1B~", VtSequenceSpecificTypes.EscInvokeG1CharacterSetGr)]
        [DataRow("\x1BZ", VtSequenceSpecificTypes.C1ReturnTerminalId)]
        public void TestBuildVtSequenceNoArgs(string expected, VtSequenceSpecificTypes specificType)
        {
            string actual = "";
            Should.NotThrow(() => actual = VtSequenceBuilderTools.BuildVtSequence(specificType));
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Determines the type from the sequence
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromSequenceEsc()
        {
            (VtSequenceType, VtSequenceSpecificTypes) expected = (VtSequenceType.Esc, VtSequenceSpecificTypes.EscInvokeG1CharacterSetGr);
            (VtSequenceType, VtSequenceSpecificTypes) actual = default;
            Should.NotThrow(() => actual = VtSequenceBuilderTools.DetermineTypesFromSequence($"{VtSequenceBasicChars.EscapeChar}~").Single());
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Determines the type from the sequence
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromSequenceC1()
        {
            (VtSequenceType, VtSequenceSpecificTypes) expected = (VtSequenceType.C1, VtSequenceSpecificTypes.C1ReturnTerminalId);
            (VtSequenceType, VtSequenceSpecificTypes) actual = default;
            Should.NotThrow(() => actual = VtSequenceBuilderTools.DetermineTypesFromSequence($"{VtSequenceBasicChars.EscapeChar}Z").Single());
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Determines the type from the sequence
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromSequenceCsi()
        {
            (VtSequenceType, VtSequenceSpecificTypes) expected = (VtSequenceType.Csi, VtSequenceSpecificTypes.CsiEraseRectangularArea);
            (VtSequenceType, VtSequenceSpecificTypes) actual = default;
            Should.NotThrow(() => actual = VtSequenceBuilderTools.DetermineTypesFromSequence($"{VtSequenceBasicChars.EscapeChar}[1;3;3;7$z").Single());
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Determines the type from the sequence
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromSequencePm()
        {
            (VtSequenceType, VtSequenceSpecificTypes) expected = (VtSequenceType.Pm, VtSequenceSpecificTypes.PmPrivacyMessage);
            (VtSequenceType, VtSequenceSpecificTypes) actual = default;
            Should.NotThrow(() => actual = VtSequenceBuilderTools.DetermineTypesFromSequence($"{VtSequenceBasicChars.EscapeChar}^Kermit\x9c").Single());
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Determines the type from the sequence
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromSequenceOsc()
        {
            (VtSequenceType, VtSequenceSpecificTypes) expected = (VtSequenceType.Osc, VtSequenceSpecificTypes.OscOperatingSystemCommand);
            (VtSequenceType, VtSequenceSpecificTypes) actual = default;
            Should.NotThrow(() => actual = VtSequenceBuilderTools.DetermineTypesFromSequence($"{VtSequenceBasicChars.EscapeChar}]0;Hello\x07").Single());
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Determines the type from the sequence
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromSequenceDcs()
        {
            (VtSequenceType, VtSequenceSpecificTypes) expected = (VtSequenceType.Dcs, VtSequenceSpecificTypes.DcsRequestResourceValues);
            (VtSequenceType, VtSequenceSpecificTypes) actual = default;
            Should.NotThrow(() => actual = VtSequenceBuilderTools.DetermineTypesFromSequence($"{VtSequenceBasicChars.EscapeChar}P+Q776964654368617273\x9c").Single());
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Determines the type from the sequence
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromSequenceApc()
        {
            (VtSequenceType, VtSequenceSpecificTypes) expected = (VtSequenceType.Apc, VtSequenceSpecificTypes.ApcApplicationProgramCommand);
            (VtSequenceType, VtSequenceSpecificTypes) actual = default;
            Should.NotThrow(() => actual = VtSequenceBuilderTools.DetermineTypesFromSequence($"{VtSequenceBasicChars.EscapeChar}_Kermit\x9c").Single());
            actual.ShouldBe(expected);
        }
    }
}
