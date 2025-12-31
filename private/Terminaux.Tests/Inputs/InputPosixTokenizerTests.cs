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
using System;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;

namespace Terminaux.Tests.Inputs
{
    [TestClass]
    public class InputPosixTokenizerTests
    {
        [TestMethod]
        [DataRow("\x1b[M &*", InputEventType.Mouse)]
        [DataRow("\x1b[M#&*", InputEventType.Mouse)]
        [DataRow("\x1b[M`()", InputEventType.Mouse)]
        [DataRow("\x1b[Ma()", InputEventType.Mouse)]
        [DataRow("\x1b[<0;10;11M", InputEventType.Mouse)]
        [DataRow("\x1b[<0;10;11m", InputEventType.Mouse)]
        [DataRow("\x1b[<64;26;9M", InputEventType.Mouse)]
        [DataRow("\x1b[<65;26;9M", InputEventType.Mouse)]
        [DataRow("\x1b[A", InputEventType.Keyboard)]
        [DataRow("\x1b[a", InputEventType.Keyboard)]
        [DataRow("\x1b[s", InputEventType.Keyboard)]
        [DataRow("\x1b[y", InputEventType.Keyboard)]
        [DataRow("\x1b[1;5A", InputEventType.Keyboard)]
        [DataRow("\x1b[17@", InputEventType.Keyboard)]
        [DataRow("\x1b[1@", InputEventType.Keyboard)]
        [DataRow("\x1b[2^", InputEventType.Keyboard)]
        [DataRow("\x1b[3^", InputEventType.Keyboard)]
        [DataRow("\x1b[28~", InputEventType.Keyboard)]
        [DataRow("\x1b[31$", InputEventType.Keyboard)]
        [DataRow("\x1b[8$", InputEventType.Keyboard)]
        [DataRow("\x1bOA", InputEventType.Keyboard)]
        [DataRow("\x1bOa", InputEventType.Keyboard)]
        [DataRow("\x1bOU", InputEventType.Keyboard)]
        [DataRow("\x1bOu", InputEventType.Keyboard)]
        [DataRow("\x1bOj", InputEventType.Keyboard)]
        [DataRow("\x1b[64;32R", InputEventType.Position)]
        [DataRow("\u001ba", InputEventType.Keyboard)]
        [DataRow("\u001bA", InputEventType.Keyboard)]
        [DataRow("\u001b", InputEventType.Keyboard)]
        [DataRow("a", InputEventType.Keyboard)]
        [DataRow("A", InputEventType.Keyboard)]
        [DataRow("\u000b", InputEventType.Keyboard)]
        [DataRow("5", InputEventType.Keyboard)]
        [DataRow("%", InputEventType.Keyboard)]
        public void TestTokenizeBroad(string sequence, InputEventType expectedEventType)
        {
            var inputArray = sequence.ToCharArray();
            var posixTokenizer = new InputPosixTokenizer(inputArray);
            var tokenized = posixTokenizer.Parse();

            // Verify type correctness
            tokenized.ShouldNotBeNull();
            tokenized.ShouldNotBeEmpty();
            tokenized[0].EventType.ShouldBe(expectedEventType);

            // Determine how to verify specific property nullability
            switch (tokenized[0].EventType)
            {
                case InputEventType.Mouse:
                    var pointer = tokenized[0].PointerEventContext;
                    pointer.ShouldNotBeNull();
                    break;
                case InputEventType.Keyboard:
                    var cki = tokenized[0].ConsoleKeyInfo;
                    cki.ShouldNotBeNull();
                    break;
                case InputEventType.Position:
                    var pos = tokenized[0].ReportedPos;
                    pos.ShouldNotBeNull();
                    break;
                default:
                    Assert.Fail("Can't determine event type");
                    break;
            }
        }

        [TestMethod]
        [DataRow("\x1b[A", '\0', ConsoleKey.UpArrow, (ConsoleModifiers)0)]
        [DataRow("\x1b[a", '\0', ConsoleKey.UpArrow, ConsoleModifiers.Shift)]
        [DataRow("\x1b[s", '\0', ConsoleKey.PageDown, (ConsoleModifiers)0)]
        [DataRow("\x1b[y", '\0', ConsoleKey.PageUp, (ConsoleModifiers)0)]
        [DataRow("\x1b[1;5A", '\0', ConsoleKey.UpArrow, ConsoleModifiers.Control)]
        [DataRow("\x1b[17@", '\0', ConsoleKey.F7, ConsoleModifiers.Shift | ConsoleModifiers.Control)]
        [DataRow("\x1b[1@", '\0', ConsoleKey.Home, ConsoleModifiers.Shift | ConsoleModifiers.Control)]
        [DataRow("\x1b[2^", '\0', ConsoleKey.Insert, ConsoleModifiers.Control)]
        [DataRow("\x1b[3^", '\0', ConsoleKey.Delete, ConsoleModifiers.Control)]
        [DataRow("\x1b[28~", '\0', ConsoleKey.F15, (ConsoleModifiers)0)]
        [DataRow("\x1b[31$", '\0', ConsoleKey.F17, ConsoleModifiers.Shift)]
        [DataRow("\x1b[8$", '\0', ConsoleKey.End, ConsoleModifiers.Shift)]
        [DataRow("\x1bOA", '\0', ConsoleKey.UpArrow, (ConsoleModifiers)0)]
        [DataRow("\x1bOa", '\0', ConsoleKey.UpArrow, ConsoleModifiers.Shift)]
        [DataRow("\x1bOU", '\0', ConsoleKey.F6, (ConsoleModifiers)0)]
        [DataRow("\x1bOu", '\0', ConsoleKey.NumPad5, (ConsoleModifiers)0)]
        [DataRow("\x1bOj", '\0', ConsoleKey.Multiply, (ConsoleModifiers)0)]
        public void TestTokenizeCtlSeqs(string sequence, char expectedChar, ConsoleKey expectedConsoleKey, ConsoleModifiers expectedModifiers)
        {
            var inputArray = sequence.ToCharArray();
            var posixTokenizer = new InputPosixTokenizer(inputArray);
            var tokenized = posixTokenizer.Parse();

            // Verify type correctness
            tokenized.ShouldNotBeNull();
            tokenized.ShouldNotBeEmpty();
            tokenized[0].EventType.ShouldBe(InputEventType.Keyboard);

            // Verify specific property nullability
            var cki = tokenized[0].ConsoleKeyInfo;
            cki.ShouldNotBeNull();

            // Verify the ConsoleKeyInfo values
            cki.Value.KeyChar.ShouldBe(expectedChar);
            cki.Value.Key.ShouldBe(expectedConsoleKey);
            cki.Value.Modifiers.ShouldBe(expectedModifiers);
        }

        [TestMethod]
        [DataRow("\x1b[M &*", PointerButton.Left, PointerButtonPress.Clicked, 0, 5, 9, false, PointerModifiers.None)]
        [DataRow("\x1b[M#&*", PointerButton.Left, PointerButtonPress.Released, 1, 5, 9, false, PointerModifiers.None)]
        [DataRow("\x1b[M`()", PointerButton.WheelUp, PointerButtonPress.Scrolled, 0, 7, 8, false, PointerModifiers.None)]
        [DataRow("\x1b[Ma()", PointerButton.WheelDown, PointerButtonPress.Scrolled, 0, 7, 8, false, PointerModifiers.None)]
        [DataRow("\x1b[<0;10;11M", PointerButton.Left, PointerButtonPress.Clicked, 0, 9, 10, false, PointerModifiers.None)]
        [DataRow("\x1b[<0;10;11m", PointerButton.Left, PointerButtonPress.Released, 1, 9, 10, false, PointerModifiers.None)]
        [DataRow("\x1b[<64;26;9M", PointerButton.WheelUp, PointerButtonPress.Scrolled, 0, 25, 8, false, PointerModifiers.None)]
        [DataRow("\x1b[<65;26;9M", PointerButton.WheelDown, PointerButtonPress.Scrolled, 0, 25, 8, false, PointerModifiers.None)]
        public void TestTokenizeMouseSeqs(string sequence, PointerButton expectedButton, PointerButtonPress expectedButtonPress, int expectedTier, int expectedX, int expectedY, bool expectedDragging, PointerModifiers expectedModifiers)
        {
            var inputArray = sequence.ToCharArray();
            var posixTokenizer = new InputPosixTokenizer(inputArray);
            var tokenized = posixTokenizer.Parse();

            // Verify type correctness
            tokenized.ShouldNotBeNull();
            tokenized.ShouldNotBeEmpty();
            tokenized[0].EventType.ShouldBe(InputEventType.Mouse);

            // Verify specific property nullability
            var pointer = tokenized[0].PointerEventContext;
            pointer.ShouldNotBeNull();

            // Verify the pointer event context values
            pointer.Button.ShouldBe(expectedButton);
            pointer.ButtonPress.ShouldBe(expectedButtonPress);
            pointer.ClickTier.ShouldBe(expectedTier);
            pointer.Coordinates.ShouldBe((expectedX, expectedY));
            pointer.Dragging.ShouldBe(expectedDragging);
            pointer.Modifiers.ShouldBe(expectedModifiers);
        }

        [TestMethod]
        [DataRow("\x1b[64;32R", 31, 63)]
        public void TestTokenizePositionSeqs(string sequence, int expectedX, int expectedY)
        {
            var inputArray = sequence.ToCharArray();
            var posixTokenizer = new InputPosixTokenizer(inputArray);
            var tokenized = posixTokenizer.Parse();

            // Verify type correctness
            tokenized.ShouldNotBeNull();
            tokenized.ShouldNotBeEmpty();
            tokenized[0].EventType.ShouldBe(InputEventType.Position);

            // Verify specific property nullability
            var pos = tokenized[0].ReportedPos;
            pos.ShouldNotBeNull();

            // Verify the position values
            pos.Value.X.ShouldBe(expectedX);
            pos.Value.Y.ShouldBe(expectedY);
        }

        [TestMethod]
        [DataRow("\u001ba", 'a', ConsoleKey.A, ConsoleModifiers.Alt)]
        [DataRow("\u001bA", 'A', ConsoleKey.A, ConsoleModifiers.Alt | ConsoleModifiers.Shift)]
        public void TestTokenizeAltSeqs(string sequence, char expectedChar, ConsoleKey expectedConsoleKey, ConsoleModifiers expectedModifiers)
        {
            var inputArray = sequence.ToCharArray();
            var posixTokenizer = new InputPosixTokenizer(inputArray);
            var tokenized = posixTokenizer.Parse();

            // Verify type correctness
            tokenized.ShouldNotBeNull();
            tokenized.ShouldNotBeEmpty();
            tokenized[0].EventType.ShouldBe(InputEventType.Keyboard);

            // Verify specific property nullability
            var cki = tokenized[0].ConsoleKeyInfo;
            cki.ShouldNotBeNull();

            // Verify the ConsoleKeyInfo values
            cki.Value.KeyChar.ShouldBe(expectedChar);
            cki.Value.Key.ShouldBe(expectedConsoleKey);
            cki.Value.Modifiers.ShouldBe(expectedModifiers);
        }

        [TestMethod]
        public void TestTokenizeEscStandalone()
        {
            var inputArray = "\x1b".ToCharArray();
            var posixTokenizer = new InputPosixTokenizer(inputArray);
            var tokenized = posixTokenizer.Parse();

            // Verify type correctness
            tokenized.ShouldNotBeNull();
            tokenized.ShouldNotBeEmpty();
            tokenized[0].EventType.ShouldBe(InputEventType.Keyboard);

            // Verify specific property nullability
            var cki = tokenized[0].ConsoleKeyInfo;
            cki.ShouldNotBeNull();

            // Verify the ConsoleKeyInfo values
            cki.Value.KeyChar.ShouldBe('\x1b');
            cki.Value.Key.ShouldBe(ConsoleKey.Escape);
            cki.Value.Modifiers.ShouldBe((ConsoleModifiers)0);
        }

        [TestMethod]
        [DataRow("a", 'a', ConsoleKey.A, (ConsoleModifiers)0)]
        [DataRow("A", 'A', ConsoleKey.A, ConsoleModifiers.Shift)]
        [DataRow("\u000b", '\v', ConsoleKey.K, ConsoleModifiers.Control)]
        [DataRow("5", '5', ConsoleKey.D5, (ConsoleModifiers)0)]
        [DataRow("%", '%', ConsoleKey.D5, ConsoleModifiers.Shift)]
        public void TestTokenizeStandard(string sequence, char expectedChar, ConsoleKey expectedConsoleKey, ConsoleModifiers expectedModifiers)
        {
            var inputArray = sequence.ToCharArray();
            var posixTokenizer = new InputPosixTokenizer(inputArray);
            var tokenized = posixTokenizer.Parse();

            // Verify type correctness
            tokenized.ShouldNotBeNull();
            tokenized.ShouldNotBeEmpty();
            tokenized[0].EventType.ShouldBe(InputEventType.Keyboard);

            // Verify specific property nullability
            var cki = tokenized[0].ConsoleKeyInfo;
            cki.ShouldNotBeNull();

            // Verify the ConsoleKeyInfo values
            cki.Value.KeyChar.ShouldBe(expectedChar);
            cki.Value.Key.ShouldBe(expectedConsoleKey);
            cki.Value.Modifiers.ShouldBe(expectedModifiers);
        }
    }
}
