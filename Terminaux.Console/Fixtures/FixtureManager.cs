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
using Terminaux.ConsoleDemo.Fixtures.Cases;

namespace Terminaux.ConsoleDemo.Fixtures
{
    internal static class FixtureManager
    {
        internal static IFixture[] fixtures =
        {
            new Prompt(),
            new PromptInterruptible(),
            new PromptWithDefault(),
            new PromptWithPlaceholder(),
            new PromptWithOneLineWrap(),
            new PromptPassword(),
            new PromptPasswordHidden(),
            new PromptPasswordWithPlaceholder(),
            new PromptPasswordWithPlaceholderHidden(),
            new PromptLooped(),
            new PromptLoopedMargin(),
            new PromptLoopedManualWrite(),
            new PromptLoopedCompletion(),
            new PromptLoopedCompletionOneLineWrap(),
            new PromptLoopedCtrlCAsInput(),
        };

        internal static IFixture GetFixtureFromName(string name)
        {
            if (DoesFixtureExist(name))
            {
                var detectedFixtures = fixtures.Where((fixture) => fixture.FixtureID == name).ToArray();
                return detectedFixtures[0];
            }
            else
                throw new Exception("Fixture doesn't exist.");
        }

        internal static bool DoesFixtureExist(string name)
        {
            var detectedFixtures = fixtures.Where((fixture) => fixture.FixtureID == name);
            return detectedFixtures.Any();
        }
    }
}
