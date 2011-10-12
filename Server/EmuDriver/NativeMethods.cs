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
using System.Drawing;
using System.Runtime.InteropServices;

namespace WindowsPhoneTestFramework.EmuDriver
{
    public class NativeMethods
    {
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

        public static bool MakeWindowTopMost(string lpClassName, string lpWindowName)
        {
            var hWnd = FindWindow(lpClassName, lpWindowName);
            if (hWnd == IntPtr.Zero)
                return false;

            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            return true;
        }

        public static void RevokeWindowTopMost(string lpClassName, string lpWindowName)
        {
            var hWnd = FindWindow(lpClassName, lpWindowName);
            if (hWnd == IntPtr.Zero)
                return;

            SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        public static Rectangle GetDesktopRectangle()
        {
            return GetWindowRectangle(GetDesktopWindow());
        }

        public static Rectangle GetWindowRectangle(string lpClassName, string lpWindowName)
        {
            var hWnd = FindWindow(lpClassName, lpWindowName);
            if (hWnd == IntPtr.Zero)
                return Rectangle.Empty;

            return GetWindowRectangle(hWnd);
        }

        public static Rectangle GetWindowRectangle(IntPtr hWnd)
        {
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

        public static bool EnsureWindowIsInForeground(string lpClassName, string lpWindowName)
        {
            var hWnd = FindWindow(lpClassName, lpWindowName);
            if (hWnd == IntPtr.Zero)
                return false;

            var result = SetForegroundWindow(hWnd);
            return result;
        }

    }
}