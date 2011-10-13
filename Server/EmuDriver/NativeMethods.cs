// ----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public class NativeMethods
    {
        #region Native Imports

        // Get a handle to an application window - http://msdn.microsoft.com/en-us/library/ms633499(v=VS.85).aspx
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName,
                                                string lpWindowName);

        /*
        // Get a handle to a child window - http://msdn.microsoft.com/en-us/library/ms633500(v=VS.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        */

        // Activate an application window - http://msdn.microsoft.com/en-us/library/ms633539(v=VS.85).aspx
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        // Get a Window position - http://msdn.microsoft.com/en-us/library/ms633519%28VS.85%29.aspx
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;

        #endregion // Native Imports

        public static IntPtr GetProcessMainWindow(string processName)
        {
            var pro = Process.GetProcesses();
            var process = Process.GetProcessesByName(processName).FirstOrDefault();

            if (process == null)
                return IntPtr.Zero;

            return process.MainWindowHandle;
        }

        public static bool ChangeWindowTopMost(string processName, bool topMost = true)
        {
            var hWnd = GetProcessMainWindow(processName);
            return ChangeWindowTopMost(hWnd, topMost);
        }

        public static bool ChangeWindowTopMost(string lpClassName, string lpWindowName, bool topMost = true)
        {
            var hWnd = FindWindow(lpClassName, lpWindowName);
            return ChangeWindowTopMost(hWnd, topMost);
        }

        public static bool ChangeWindowTopMost(IntPtr hWnd, bool topMost = true)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            SetWindowPos(hWnd, topMost ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            return true;
        }

        public static bool MakeWindowTopMost(string processName)
        {
            return ChangeWindowTopMost(processName, true);
        }

        public static bool MakeWindowTopMost(string lpClassName, string lpWindowName)
        {
            return ChangeWindowTopMost(lpClassName, lpWindowName, true);
        }

        public static bool MakeWindowTopMost(IntPtr hWnd)
        {
            return ChangeWindowTopMost(hWnd, true);
        }

        public static bool RevokeWindowTopMost(string processName)
        {
            return ChangeWindowTopMost(processName, false);
        }

        public static bool RevokeWindowTopMost(string lpClassName, string lpWindowName)
        {
            return ChangeWindowTopMost(lpClassName, lpWindowName, false);
        }

        public static bool RevokeWindowTopMost(IntPtr hWnd)
        {
            return ChangeWindowTopMost(hWnd, false);
        }

        public static Rectangle GetDesktopRectangle()
        {
            return GetWindowRectangle(GetDesktopWindow());
        }

        public static Rectangle GetWindowRectangle(string processName)
        {
            var hWnd = GetProcessMainWindow(processName);
            return GetWindowRectangle(hWnd);
        }

        public static Rectangle GetWindowRectangle(string lpClassName, string lpWindowName)
        {
            var hWnd = FindWindow(lpClassName, lpWindowName);
            return GetWindowRectangle(hWnd);
        }

        public static Rectangle GetWindowRectangle(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return Rectangle.Empty;

            RECT rect;
            var result = GetWindowRect(hWnd, out rect);
            if (!result)
                return Rectangle.Empty;

            return new Rectangle()
                       {
                           X = rect.Left,
                           Y = rect.Top,
                           Width = rect.Right - rect.Left,
                           Height = rect.Bottom - rect.Top
                       };
        }

        public static bool EnsureWindowIsInForeground(string processName)
        {
            return ChangeWindowIsInForeground(processName);
        }

        public static bool EnsureWindowIsInForeground(string lpClassName, string lpWindowName)
        {
            return ChangeWindowIsInForeground(lpClassName, lpWindowName, true);
        }

        public static bool EnsureWindowIsInForeground(IntPtr hWnd)
        {
            return ChangeWindowIsInForeground(hWnd, true);
        }

        public static bool ChangeWindowIsInForeground(string processName, bool inForeground = true)
        {
            var hWnd = GetProcessMainWindow(processName);
            return ChangeWindowIsInForeground(hWnd, inForeground);
        }

        public static bool ChangeWindowIsInForeground(string lpClassName, string lpWindowName, bool inForeground = true)
        {
            var hWnd = FindWindow(lpClassName, lpWindowName);
            return ChangeWindowIsInForeground(hWnd, inForeground);
        }

        public static bool ChangeWindowIsInForeground(IntPtr hWnd, bool inForeground = true)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            var result = SetForegroundWindow(hWnd);
            return result;
        }
    }
}