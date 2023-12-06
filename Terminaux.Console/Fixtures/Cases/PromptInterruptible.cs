
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

using System.Threading;
using System.Threading.Tasks;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class PromptInterruptible : IFixture
    {
        public string FixtureID => "PromptInterruptible";

        public void RunFixture()
        {
            var interruptTask = new Task(() =>
            {
                Thread.Sleep(5000);
                TermReaderTools.Interrupt();
            });
            interruptTask.Start();
            string input = TermReader.Read("You have five seconds to say anything: ", "", false, false, true);
            TextWriterColor.Write("You said: " + input);
        }
    }
}
