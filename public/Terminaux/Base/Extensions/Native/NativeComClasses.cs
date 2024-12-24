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

// Part of the code was taken from BenchmarkDotNet source code licensed under
// the MIT license that you can find in the README.BenchmarkDotNet file.

using System;
using System.Runtime.InteropServices;

namespace Terminaux.Base.Extensions.Native
{
    internal class NativeComClasses
    {
        internal class TaskbarCom
        {
            private readonly ITaskbarList3 taskbarInstance;
            private readonly IntPtr consoleWindowHandle;

            [ComImport]
            [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            private interface ITaskbarList3
            {
                // ITaskbarList
                [PreserveSig]
                void HrInit();
                [PreserveSig]
                void AddTab(IntPtr hwnd);
                [PreserveSig]
                void DeleteTab(IntPtr hwnd);
                [PreserveSig]
                void ActivateTab(IntPtr hwnd);
                [PreserveSig]
                void SetActiveAlt(IntPtr hwnd);

                // ITaskbarList2
                [PreserveSig]
                void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

                // ITaskbarList3
                [PreserveSig]
                void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
                [PreserveSig]
                void SetProgressState(IntPtr hwnd, ConsoleTaskbarProgressEnum state);
            }

            [ComImport]
            [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
            [ClassInterface(ClassInterfaceType.None)]
            private class TaskbarInstance
            { }

            internal static TaskbarCom? Create(ConsoleTaskbarProgressEnum initialTaskbarState)
            {
                try
                {
                    // Get the console window
                    IntPtr handle = NativeMethods.GetConsoleWindow();
                    if (handle == IntPtr.Zero)
                        return null;

                    // Now, create a COM object
                    var com = new TaskbarCom(handle);
                    com.SetState(initialTaskbarState);
                    return com;
                }
                catch
                {
                    return null;
                }
            }

            internal void SetState(ConsoleTaskbarProgressEnum taskbarState)
                => taskbarInstance.SetProgressState(consoleWindowHandle, taskbarState);

            internal void SetValue(ulong progressValue, ulong maximum)
                => taskbarInstance.SetProgressValue(consoleWindowHandle, progressValue, maximum);

            private TaskbarCom(IntPtr handle)
            {
                taskbarInstance = (ITaskbarList3)new TaskbarInstance();
                consoleWindowHandle = handle;
            }
        }
    }
}
