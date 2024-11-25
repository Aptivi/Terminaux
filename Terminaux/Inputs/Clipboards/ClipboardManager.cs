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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Terminaux.Inputs.Clipboards
{
    /// <summary>
    /// Clipboard management tools
    /// </summary>
    public static class ClipboardManager
    {
        /// <summary>
        /// Gets the clipboard contents (text only)
        /// </summary>
        /// <returns>Clipboard contents</returns>
        public static string GetClipboardContents()
        {
            string contents = "";

            // We need to constraint to the current platform so that we run the correct platform code.
            var platform = PlatformHelper.GetPlatform();
            switch (platform)
            {
                case Platform.Windows:
                    // Open the clipboard (state variable)
                    var newOwner = Process.GetCurrentProcess().MainWindowHandle;
                    bool opened = false;

                    try
                    {
                        // Check the clipboard format before opening it
                        int finalFormat = CF_UNICODETEXT;
                        bool isValid = IsClipboardFormatAvailable(CF_UNICODETEXT);
                        if (!isValid)
                        {
                            finalFormat = CF_TEXT;
                            isValid = IsClipboardFormatAvailable(CF_TEXT);
                            if (!isValid)
                                return "";
                        }
                        opened = OpenClipboard(newOwner);
                        if (!opened)
                            return "";

                        // Get the clipboard data
                        var clipboardDataPtr = GetClipboardData(finalFormat);
                        var clipboardData = GlobalLock(clipboardDataPtr);
                        if (clipboardData != IntPtr.Zero)
                        {
                            contents =
                                finalFormat == CF_UNICODETEXT ?
                                Marshal.PtrToStringUni(clipboardData) :
                                Marshal.PtrToStringAnsi(clipboardData);
                            GlobalUnlock(clipboardData);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Clipboard has failed [{Marshal.GetLastWin32Error()}]: {ex}");
                        contents = "";
                    }
                    finally
                    {
                        if (opened)
                            CloseClipboard();
                    }
                    break;
            }

            // Return the result
            return contents;
        }

        #region Windows
        private const int CF_TEXT = 1;
        private const int CF_UNICODETEXT = 13;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsClipboardFormatAvailable(int format);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetClipboardData(int uFormat);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GlobalLock(IntPtr hMem);
        #endregion
    }
}
