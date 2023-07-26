/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
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
 * 
 */

using System;
using System.Linq;
using TermRead.Reader;

namespace TermRead.ConsoleDemo.Fixtures.Cases
{
    internal class PromptLoopedCompletion : IFixture
    {
        public string FixtureID => "PromptLoopedCompletion";

        public void RunFixture()
        {
            Console.WriteLine("Write \"exit\" to get out of here.");
            string input = "";
            while (input != "exit")
            {
                TermReaderSettings.Suggestions = GetSuggestions;
                input = TermReader.Read(">> ");
                Console.WriteLine("You said: " + input);
            }
        }

        public string[] GetSuggestions(string text, int index, char[] delims)
        {
            text = text.Substring(0, index);
            string[] parts = text.Split(delims);
            if (parts.Length == 0)
                return Array.Empty<string>();
            else
            {
                if (parts[0] == "dotnet")
                    return new string[] { "build", "restore", "run" }
                        .Where((str) => str.StartsWith(parts[1]))
                        .Select((str) => str.Substring(parts[1].Length))
                        .ToArray();
                else if (parts[0] == "git")
                    return new string[] { "fetch", "pull", "push" }
                        .Where((str) => str.StartsWith(parts[1]))
                        .Select((str) => str.Substring(parts[1].Length))
                        .ToArray();
            }
            return Array.Empty<string>();
        }
    }
}
