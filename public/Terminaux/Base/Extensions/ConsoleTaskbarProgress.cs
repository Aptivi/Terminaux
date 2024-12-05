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

using SpecProbe.Software.Platform;
using System;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions.Native;
using Terminaux.Sequences.Builder;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console taskbar progress class
    /// </summary>
    public static class ConsoleTaskbarProgress
    {
        private static readonly NativeComClasses.TaskbarCom? taskbarCom;

        /// <summary>
        /// Sets the progress of the console taskbar (signed integers)
        /// </summary>
        /// <param name="state">Taskbar progress state</param>
        /// <param name="progressValue">Progress value</param>
        /// <param name="maximum">Maximum progress value</param>
        /// <remarks>
        /// Nothing happens when you try to run this function on non-Windows systems.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetProgress(ConsoleTaskbarProgressEnum state, long progressValue = 0, long maximum = 100) =>
            SetProgressUnsigned(state, (ulong)progressValue, (ulong)maximum);

        /// <summary>
        /// Sets the progress of the console taskbar (unsigned integers)
        /// </summary>
        /// <param name="state">Taskbar progress state</param>
        /// <param name="progressValue">Progress value</param>
        /// <param name="maximum">Maximum progress value</param>
        /// <remarks>
        /// Nothing happens when you try to run this function on non-Windows systems.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetProgressUnsigned(ConsoleTaskbarProgressEnum state, ulong progressValue = 0, ulong maximum = 100)
        {
            // Check to see if the taskbar COM object is initialized
            if (taskbarCom is null)
                return;

            // Check the progress value range
            if (maximum < progressValue)
                throw new ArgumentOutOfRangeException(nameof(maximum), $"Maximum value [{maximum}] should not be smaller than the progress value [{progressValue}]");
            if (progressValue < 0 || progressValue > maximum)
                throw new ArgumentOutOfRangeException(nameof(progressValue), $"The progress value [{progressValue}] must be between 0 and the maximum value of {maximum}.");
            
            // Set the value and the state
            taskbarCom.SetState(state);
            taskbarCom.SetValue(progressValue, maximum);

            // Now, try to write a VT sequence for Windows Terminal
            uint hundredths = (uint)((double)progressValue * 100 / maximum);
            string sequence = GenerateVtSequence(state, hundredths);
            TextWriterRaw.WriteRaw(sequence);
        }

        private static string GenerateVtSequence(ConsoleTaskbarProgressEnum state, uint progressValue)
        {
            // Generate the Windows Terminal specific sequence according to the official docs at
            // https://learn.microsoft.com/en-us/windows/terminal/tutorials/progress-bar-sequences#progress-bar-sequence-format
            var stateNum = state switch
            {
                ConsoleTaskbarProgressEnum.Normal           => 1,
                ConsoleTaskbarProgressEnum.Error            => 2,
                ConsoleTaskbarProgressEnum.Indeterminate    => 3,
                ConsoleTaskbarProgressEnum.Paused           => 4,
                _ => 0,
            };
            string sequence = VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.OscOperatingSystemCommand, $"9;4;{stateNum};{progressValue}");
            return sequence;
        }

        static ConsoleTaskbarProgress()
        {
            bool supported = PlatformHelper.IsOnWindows() && Environment.OSVersion.Version >= new Version(6, 1);
            if (supported)
            {
                if (!ConsoleChecker.busy)
                    ConsoleChecker.CheckConsole();
                taskbarCom = NativeComClasses.TaskbarCom.Create(ConsoleTaskbarProgressEnum.NoProgress);
            }
        }
    }
}
