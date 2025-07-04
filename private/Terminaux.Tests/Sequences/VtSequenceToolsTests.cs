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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using Terminaux.Sequences;

namespace Terminaux.Tests.Sequences
{
    [TestClass]
    public class VtSequenceToolsTests
    {
        /// <summary>
        /// Tests splitting the VT sequences
        /// </summary>
        [TestMethod]
        public void TestSplitVTSequences()
        {
            char BellChar = Convert.ToChar(0x7);
            char EscapeChar = Convert.ToChar(0x1b);
            char StringTerminator = Convert.ToChar(0x9c);
            string vtSequence1 = $"{EscapeChar}[38;5;43m";
            string vtSequence2 = $"{EscapeChar}_1{StringTerminator}";
            string vtSequence3 = $"{EscapeChar}]0;Hi!{BellChar}";
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}").ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence2}lo!").ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence3}Hello!").ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence3}Hel{vtSequence2}lo!{vtSequence1}", VtSequenceType.Csi | VtSequenceType.Osc).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests splitting the APC sequences
        /// </summary>
        [TestMethod]
        public void TestSplitAPCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}_1{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Apc;
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence1}Hello!", requestedType).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests splitting the C1 sequences
        /// </summary>
        [TestMethod]
        public void TestSplitC1Sequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}\\";
            string vtSequence2 = $"{EscapeChar}M";
            VtSequenceType requestedType = VtSequenceType.C1;
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence2}Hello!", requestedType).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests splitting the CSI sequences
        /// </summary>
        [TestMethod]
        public void TestSplitCSISequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}[37m";
            VtSequenceType requestedType = VtSequenceType.Csi;
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence1}Hello!", requestedType).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests splitting the DCS sequences
        /// </summary>
        [TestMethod]
        public void TestSplitDCSSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}P3{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Dcs;
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence1}Hello!", requestedType).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests splitting the ESC sequences
        /// </summary>
        [TestMethod]
        public void TestSplitESCSequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}#4";
            string vtSequence2 = $"{EscapeChar}%G";
            VtSequenceType requestedType = VtSequenceType.Esc;
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence2}Hello!", requestedType).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests splitting the OSC sequences
        /// </summary>
        [TestMethod]
        public void TestSplitOSCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}]0;Title{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Osc;
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence1}Hello!", requestedType).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests splitting the PM sequences
        /// </summary>
        [TestMethod]
        public void TestSplitPMSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}^Kermit{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Pm;
            VtSequenceTools.SplitVTSequences($"Hello!{vtSequence1}", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldNotBeEmpty();
            VtSequenceTools.SplitVTSequences($"{vtSequence1}Hello!", requestedType).ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests filtering the VT sequences
        /// </summary>
        [TestMethod]
        public void TestFilterVTSequences()
        {
            char BellChar = Convert.ToChar(0x7);
            char EscapeChar = Convert.ToChar(0x1b);
            char StringTerminator = Convert.ToChar(0x9c);
            string vtSequence1 = $"{EscapeChar}[38;5;43m";
            string vtSequence2 = $"{EscapeChar}_1{StringTerminator}";
            string vtSequence3 = $"{EscapeChar}]0;This is the title{BellChar}";
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}").ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"Hel{vtSequence2}lo!").ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence3}Hello!").ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests filtering the APC sequences
        /// </summary>
        [TestMethod]
        public void TestFilterAPCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}_1{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Apc;
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence1}Hello!", "", requestedType).ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests filtering the C1 sequences
        /// </summary>
        [TestMethod]
        public void TestFilterC1Sequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}\\";
            string vtSequence2 = $"{EscapeChar}M";
            VtSequenceType requestedType = VtSequenceType.C1;
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"Hel{vtSequence1}lo!", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence2}Hello!", "", requestedType).ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests filtering the CSI sequences
        /// </summary>
        [TestMethod]
        public void TestFilterCSISequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}[37m";
            VtSequenceType requestedType = VtSequenceType.Csi;
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"Hel{vtSequence1}lo!", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence1}Hello!", "", requestedType).ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests filtering the DCS sequences
        /// </summary>
        [TestMethod]
        public void TestFilterDCSSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}P3{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Dcs;
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"Hel{vtSequence1}lo!", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence1}Hello!", "", requestedType).ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests filtering the ESC sequences
        /// </summary>
        [TestMethod]
        public void TestFilterESCSequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}#4";
            string vtSequence2 = $"{EscapeChar}%G";
            VtSequenceType requestedType = VtSequenceType.Esc;
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"Hel{vtSequence1}lo!", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence2}Hello!", "", requestedType).ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests filtering the OSC sequences
        /// </summary>
        [TestMethod]
        public void TestFilterOSCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}]0;Title{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Osc;
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"Hel{vtSequence1}lo!", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence1}Hello!", "", requestedType).ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests filtering the PM sequences
        /// </summary>
        [TestMethod]
        public void TestFilterPMSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}^Kermit{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Pm;
            VtSequenceTools.FilterVTSequences($"Hello!{vtSequence1}", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"Hel{vtSequence1}lo!", "", requestedType).ShouldBe("Hello!");
            VtSequenceTools.FilterVTSequences($"{vtSequence1}Hello!", "", requestedType).ShouldBe("Hello!");
        }

        /// <summary>
        /// Tests matching the VT sequences
        /// </summary>
        [TestMethod]
        public void TestMatchVTSequences()
        {
            char BellChar = Convert.ToChar(0x7);
            char EscapeChar = Convert.ToChar(0x1b);
            char StringTerminator = Convert.ToChar(0x9c);
            string vtSequence1 = $"{EscapeChar}[38;5;43m";
            string vtSequence2 = $"{EscapeChar}_1{StringTerminator}";
            string vtSequence3 = $"{EscapeChar}]0;Hi!{BellChar}";
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}");
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence2}lo!");
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence3}Hello!");
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.Count.ShouldBe(7);
            matchCollections2.Count.ShouldBe(7);
            matchCollections3.Count.ShouldBe(7);
            bool found1 = false;
            bool found2 = false;
            bool found3 = false;
            foreach (var kvp in matchCollections1)
            {
                var matches = kvp.Value;
                if (matches.Length > 0)
                {
                    matches.ShouldNotBeEmpty();
                    matches.ShouldHaveSingleItem();
                    found1 = true;
                }
            }
            foreach (var kvp in matchCollections2)
            {
                var matches = kvp.Value;
                if (matches.Length > 0)
                {
                    matches.ShouldNotBeEmpty();
                    matches.ShouldHaveSingleItem();
                    found2 = true;
                }
            }
            foreach (var kvp in matchCollections3)
            {
                var matches = kvp.Value;
                if (matches.Length > 0)
                {
                    matches.ShouldNotBeEmpty();
                    matches.ShouldHaveSingleItem();
                    found3 = true;
                }
            }
            bool found = found1 && found2 && found3;
            found.ShouldBeTrue();
        }

        /// <summary>
        /// Tests matching the APC sequences
        /// </summary>
        [TestMethod]
        public void TestMatchAPCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}_1{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Apc;
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.ShouldContainKey(requestedType);
            matchCollections2.ShouldContainKey(requestedType);
            matchCollections3.ShouldContainKey(requestedType);
            matchCollections1[requestedType].ShouldNotBeEmpty();
            matchCollections2[requestedType].ShouldNotBeEmpty();
            matchCollections3[requestedType].ShouldNotBeEmpty();
            matchCollections1[requestedType].ShouldHaveSingleItem();
            matchCollections2[requestedType].ShouldHaveSingleItem();
            matchCollections3[requestedType].ShouldHaveSingleItem();
        }

        /// <summary>
        /// Tests matching the C1 sequences
        /// </summary>
        [TestMethod]
        public void TestMatchC1Sequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}\\";
            string vtSequence2 = $"{EscapeChar}M";
            VtSequenceType requestedType = VtSequenceType.C1;
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence2}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.ShouldContainKey(requestedType);
            matchCollections2.ShouldContainKey(requestedType);
            matchCollections3.ShouldContainKey(requestedType);
            matchCollections1[requestedType].ShouldNotBeEmpty();
            matchCollections2[requestedType].ShouldNotBeEmpty();
            matchCollections3[requestedType].ShouldNotBeEmpty();
            matchCollections1[requestedType].ShouldHaveSingleItem();
            matchCollections2[requestedType].ShouldHaveSingleItem();
            matchCollections3[requestedType].ShouldHaveSingleItem();
        }

        /// <summary>
        /// Tests matching the CSI sequences
        /// </summary>
        [TestMethod]
        public void TestMatchCSISequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}[37m";
            VtSequenceType requestedType = VtSequenceType.Csi;
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.ShouldContainKey(requestedType);
            matchCollections2.ShouldContainKey(requestedType);
            matchCollections3.ShouldContainKey(requestedType);
            matchCollections1[requestedType].ShouldNotBeEmpty();
            matchCollections2[requestedType].ShouldNotBeEmpty();
            matchCollections3[requestedType].ShouldNotBeEmpty();
            matchCollections1[requestedType].ShouldHaveSingleItem();
            matchCollections2[requestedType].ShouldHaveSingleItem();
            matchCollections3[requestedType].ShouldHaveSingleItem();
        }

        /// <summary>
        /// Tests matching the DCS sequences
        /// </summary>
        [TestMethod]
        public void TestMatchDCSSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}P3{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Dcs;
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.ShouldContainKey(requestedType);
            matchCollections2.ShouldContainKey(requestedType);
            matchCollections3.ShouldContainKey(requestedType);
            matchCollections1[requestedType].ShouldNotBeEmpty();
            matchCollections2[requestedType].ShouldNotBeEmpty();
            matchCollections3[requestedType].ShouldNotBeEmpty();
            matchCollections1[requestedType].ShouldHaveSingleItem();
            matchCollections2[requestedType].ShouldHaveSingleItem();
            matchCollections3[requestedType].ShouldHaveSingleItem();
        }

        /// <summary>
        /// Tests matching the ESC sequences
        /// </summary>
        [TestMethod]
        public void TestMatchESCSequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}#4";
            string vtSequence2 = $"{EscapeChar}%G";
            VtSequenceType requestedType = VtSequenceType.Esc;
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence2}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.ShouldContainKey(requestedType);
            matchCollections2.ShouldContainKey(requestedType);
            matchCollections3.ShouldContainKey(requestedType);
            matchCollections1[requestedType].ShouldNotBeEmpty();
            matchCollections2[requestedType].ShouldNotBeEmpty();
            matchCollections3[requestedType].ShouldNotBeEmpty();
            matchCollections1[requestedType].ShouldHaveSingleItem();
            matchCollections2[requestedType].ShouldHaveSingleItem();
            matchCollections3[requestedType].ShouldHaveSingleItem();
        }

        /// <summary>
        /// Tests matching the OSC sequences
        /// </summary>
        [TestMethod]
        public void TestMatchOSCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}]0;Title{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Osc;
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.ShouldContainKey(requestedType);
            matchCollections2.ShouldContainKey(requestedType);
            matchCollections3.ShouldContainKey(requestedType);
            matchCollections1[requestedType].ShouldNotBeEmpty();
            matchCollections2[requestedType].ShouldNotBeEmpty();
            matchCollections3[requestedType].ShouldNotBeEmpty();
            matchCollections1[requestedType].ShouldHaveSingleItem();
            matchCollections2[requestedType].ShouldHaveSingleItem();
            matchCollections3[requestedType].ShouldHaveSingleItem();
        }

        /// <summary>
        /// Tests matching the PM sequences
        /// </summary>
        [TestMethod]
        public void TestMatchPMSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}^Kermit{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Pm;
            var matchCollections1 = VtSequenceTools.MatchVTSequences($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.MatchVTSequences($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.MatchVTSequences($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1.ShouldContainKey(requestedType);
            matchCollections2.ShouldContainKey(requestedType);
            matchCollections3.ShouldContainKey(requestedType);
            matchCollections1[requestedType].ShouldNotBeEmpty();
            matchCollections2[requestedType].ShouldNotBeEmpty();
            matchCollections3[requestedType].ShouldNotBeEmpty();
            matchCollections1[requestedType].ShouldHaveSingleItem();
            matchCollections2[requestedType].ShouldHaveSingleItem();
            matchCollections3[requestedType].ShouldHaveSingleItem();
        }

        /// <summary>
        /// Tests checking to see if the string contains the VT sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchVTSequences()
        {
            char BellChar = Convert.ToChar(0x7);
            char EscapeChar = Convert.ToChar(0x1b);
            char StringTerminator = Convert.ToChar(0x9c);
            string vtSequence1 = $"{EscapeChar}[38;5;43m";
            string vtSequence2 = $"{EscapeChar}_1{StringTerminator}";
            string vtSequence3 = $"{EscapeChar}]0;Hi!{BellChar}";
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}").ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence2}lo!").ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence3}Hello!").ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the APC sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchAPCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}_1{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Apc;
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence1}Hello!", requestedType).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the C1 sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchC1Sequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}\\";
            string vtSequence2 = $"{EscapeChar}M";
            VtSequenceType requestedType = VtSequenceType.C1;
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence2}Hello!", requestedType).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the CSI sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchCSISequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}[37m";
            VtSequenceType requestedType = VtSequenceType.Csi;
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence1}Hello!", requestedType).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the DCS sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchDCSSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}P3{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Dcs;
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence1}Hello!", requestedType).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the ESC sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchESCSequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}#4";
            string vtSequence2 = $"{EscapeChar}%G";
            VtSequenceType requestedType = VtSequenceType.Esc;
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence2}Hello!", requestedType).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the OSC sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchOSCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}]0;Title{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Osc;
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence1}Hello!", requestedType).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the PM sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchPMSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}^Kermit{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Pm;
            VtSequenceTools.IsMatchVTSequences($"Hello!{vtSequence1}", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"Hel{vtSequence1}lo!", requestedType).ShouldBeTrue();
            VtSequenceTools.IsMatchVTSequences($"{vtSequence1}Hello!", requestedType).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the VT sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificVTSequences()
        {
            char BellChar = Convert.ToChar(0x7);
            char EscapeChar = Convert.ToChar(0x1b);
            char StringTerminator = Convert.ToChar(0x9c);
            string vtSequence1 = $"{EscapeChar}[38;5;43m";
            string vtSequence2 = $"{EscapeChar}_1{StringTerminator}";
            string vtSequence3 = $"{EscapeChar}]0;Hi!{BellChar}";
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}");
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence2}lo!");
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence3}Hello!");
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.Csi].ShouldBeTrue();
            matchCollections2[VtSequenceType.Apc].ShouldBeTrue();
            matchCollections3[VtSequenceType.Osc].ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the APC sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificAPCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}_1{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Apc;
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.Apc].ShouldBeTrue();
            matchCollections2[VtSequenceType.Apc].ShouldBeTrue();
            matchCollections3[VtSequenceType.Apc].ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the C1 sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificC1Sequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}\\";
            string vtSequence2 = $"{EscapeChar}M";
            VtSequenceType requestedType = VtSequenceType.C1;
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence2}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.C1].ShouldBeTrue();
            matchCollections2[VtSequenceType.C1].ShouldBeTrue();
            matchCollections3[VtSequenceType.C1].ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the CSI sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificCSISequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}[37m";
            VtSequenceType requestedType = VtSequenceType.Csi;
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.Csi].ShouldBeTrue();
            matchCollections2[VtSequenceType.Csi].ShouldBeTrue();
            matchCollections3[VtSequenceType.Csi].ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the DCS sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificDCSSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}P3{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Dcs;
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.Dcs].ShouldBeTrue();
            matchCollections2[VtSequenceType.Dcs].ShouldBeTrue();
            matchCollections3[VtSequenceType.Dcs].ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the ESC sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificESCSequences()
        {
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}#4";
            string vtSequence2 = $"{EscapeChar}%G";
            VtSequenceType requestedType = VtSequenceType.Esc;
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence2}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.Esc].ShouldBeTrue();
            matchCollections2[VtSequenceType.Esc].ShouldBeTrue();
            matchCollections3[VtSequenceType.Esc].ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the OSC sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificOSCSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}]0;Title{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Osc;
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.Osc].ShouldBeTrue();
            matchCollections2[VtSequenceType.Osc].ShouldBeTrue();
            matchCollections3[VtSequenceType.Osc].ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if the string contains the PM sequences
        /// </summary>
        [TestMethod]
        public void TestIsMatchSpecificPMSequences()
        {
            char StringTerminator = Convert.ToChar(0x9c);
            char EscapeChar = Convert.ToChar(0x1b);
            string vtSequence1 = $"{EscapeChar}^Kermit{StringTerminator}";
            VtSequenceType requestedType = VtSequenceType.Pm;
            var matchCollections1 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hello!{vtSequence1}", requestedType);
            var matchCollections2 = VtSequenceTools.IsMatchVTSequencesSpecific($"Hel{vtSequence1}lo!", requestedType);
            var matchCollections3 = VtSequenceTools.IsMatchVTSequencesSpecific($"{vtSequence1}Hello!", requestedType);
            matchCollections1.ShouldNotBeEmpty();
            matchCollections2.ShouldNotBeEmpty();
            matchCollections3.ShouldNotBeEmpty();
            matchCollections1[VtSequenceType.Pm].ShouldBeTrue();
            matchCollections2[VtSequenceType.Pm].ShouldBeTrue();
            matchCollections3[VtSequenceType.Pm].ShouldBeTrue();
        }

        /// <summary>
        /// Tests determining the VT sequence type from the given text
        /// </summary>
        [TestMethod]
        public void TestDetermineTypeFromText()
        {
            char BellChar = Convert.ToChar(0x7);
            char EscapeChar = Convert.ToChar(0x1b);
            char StringTerminator = Convert.ToChar(0x9c);
            string vtSequence1 = $"{EscapeChar}[38;5;43m";
            string vtSequence2 = $"{EscapeChar}_1{StringTerminator}";
            string vtSequence3 = $"{EscapeChar}]0;Hi!{BellChar}";
            VtSequenceTools.DetermineTypeFromText($"Hello!{vtSequence1}").ShouldBe(VtSequenceType.Csi);
            VtSequenceTools.DetermineTypeFromText($"Hel{vtSequence2}lo!").ShouldBe(VtSequenceType.Apc);
            VtSequenceTools.DetermineTypeFromText($"{vtSequence3}Hello!").ShouldBe(VtSequenceType.Osc);
            VtSequenceTools.DetermineTypeFromText($"Hello!").ShouldBe(VtSequenceType.None);
        }
    }
}
