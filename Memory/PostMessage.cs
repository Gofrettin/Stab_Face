using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Stab_Face
{
    /// <summary>
    /// Sends keystrokes to the specified window
    /// </summary>
    public static class PostMessage
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(int hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        // Or use System.Drawing.Point (Forms only)
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        private static extern int _PostMessage(int hWnd, int msg, int wParam, uint lParam);
        [DllImport("user32.dll", EntryPoint = "MapVirtualKey")]
        private static extern int _MapVirtualKey(int uCode, int uMapType);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        // constants for the mouse_input() API function
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;


        private static Object thisLock = new Object();

        /// <summary>
        /// Sends keystrokes to the specified window
        /// </summary>
        /// <param name="hWnd">Window's hWnd</param>
        /// <param name="keys">String of keys to send</param>
        /// <returns>Returns number of keystrokes sent, -1 if an error occurs</returns>
        public static int SendKeys(int hWnd, string keys)
        {
            //lock (thisLock)
            //{
            if (hWnd <= 0 || keys.Length == 0)
                return -1;

            int ret = 0, i = 0;

            System.Text.StringBuilder str = new System.Text.StringBuilder(keys.ToUpper());

            str.Replace(Convert.ToChar("`"), Convert.ToChar(0xC0));
            str.Replace(Convert.ToChar("~"), Convert.ToChar(0xC0));
            str.Replace(Convert.ToChar("-"), Convert.ToChar(0xBD));
            str.Replace(Convert.ToChar("="), Convert.ToChar(0xBB));
            str.Replace(Convert.ToChar("/"), Convert.ToChar(0xBF));
            str.Replace("{BKSPC}", Convert.ToChar(0x8).ToString());
            str.Replace("{TAB}", Convert.ToChar(0x9).ToString());
            str.Replace("{ENTER}", Convert.ToChar(0xD).ToString());
            str.Replace("{SPACE}", Convert.ToChar(0x20).ToString());
            str.Replace("{CTRL}", Convert.ToChar(0x11).ToString());
            str.Replace("{ESC}", Convert.ToChar(0x1B).ToString());
            str.Replace("{F5}", Convert.ToChar(0x74).ToString());
            str.Replace("{F12}", Convert.ToChar(0x7B).ToString());
            str.Replace("{SHIFTD}", Convert.ToChar(0xC1).ToString());
            str.Replace("{SHIFTU}", Convert.ToChar(0xC2).ToString());
            str.Replace("{RIGHT}", Convert.ToChar(0x27).ToString());
            str.Replace("{LEFT}", Convert.ToChar(0x25).ToString());

            for (int ix = 1; ix <= str.Length; ++ix)
            {
                char chr = str[i];

                if (Convert.ToInt32(chr) == 0xC1)
                {
                    _PostMessage(hWnd, 0x100, 0x10, 0x002A0001);
                    _PostMessage(hWnd, 0x100, 0x10, 0x402A0001);
                    Thread.Sleep(1);
                }
                else if (Convert.ToInt32(chr) == 0xC2)
                {
                    _PostMessage(hWnd, 0x101, 0x10, 0xC02A0001);
                    Thread.Sleep(1);
                }
                else
                {
                    //int tries = 0;
                    //int r = -1;

                    //while (r < 0 && tries < 3)
                    //{
                    //   r = 0;
                    try
                    {
                        ret = _MapVirtualKey(Convert.ToInt32(chr), 0);
                        if (_PostMessage(hWnd, 0x100, Convert.ToInt32(chr), MakeLong(1, ret)) == 0)
                        {
                            try
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }
                            catch (Exception ex)
                            {
                                Debug.Print("Error on post message press: " + ex.Message);
                                return -1;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("Error on post message main press: " + ex.Message);
                        return -1;
                    }

                    Thread.Sleep(1);
                    try
                    {
                        if (_PostMessage(hWnd, 0x101, Convert.ToInt32(chr), (MakeLong(1, ret) + 0xC0000000)) == 0)
                        {
                            try
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }
                            catch (Exception ex)
                            {
                                Debug.Print("Error on post message up: " + ex.Message);
                                return -1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("Error on post message main up: " + ex.Message);
                        return -1;
                    }
                    //    tries++;
                    //}
                    //if (r < 0)
                    //    return r;
                }
                i++;
            }
            return i;
            //}
        }

        public static int SendKey(int hWnd, Int32 key)
        {
            //lock (thisLock)
            //{
            int ret = 0, i = 0;

            if (key == 0xC1)
            {
                _PostMessage(hWnd, 0x100, 0x10, 0x002A0001);
                _PostMessage(hWnd, 0x100, 0x10, 0x402A0001);
                Thread.Sleep(1);
            }
            else if (key == 0xC2)
            {
                _PostMessage(hWnd, 0x101, 0x10, 0xC02A0001);
                Thread.Sleep(1);
            }
            else
            {
                //int tries = 0;
                //int r = -1;

                //while (r < 0 && tries < 3)
                //{
                //   r = 0;
                ret = _MapVirtualKey(key, 0);
                if (_PostMessage(hWnd, 0x100, key, MakeLong(1, ret)) == 0)
                    return -1;

                Thread.Sleep(1);

                if (_PostMessage(hWnd, 0x101, key, (MakeLong(1, ret) + 0xC0000000)) == 0)
                    return -1;
                //    tries++;
                //}
                //if (r < 0)
                //    return r;
            }
            i++;
            return i;
            //} 
        }


        /// <summary>
        /// Taps the specified arrow key
        /// </summary>
        /// <param name="hWnd">The window's hWnd</param>
        /// <param name="key">The arrow key to be tapped ("left", "up", "right", or "down")</param>
        /// <returns>Returns true if successful, false if not</returns>
        public static bool ArrowKey(int hWnd, string key)
        {
            //lock (thisLock)
            //{
            //If hWnd is 0 or parameter two is incorrect, return false
            if (hWnd <= 0 ||
                (key.ToLower() != "left" &&
                key.ToLower() != "right" &&
                key.ToLower() != "down" &&
                key.ToLower() != "up"))
                return false;

            int wParam;
            uint lParam;

            //Set up wParam and lParam based upon which button needs pressing
            switch (key.ToLower())
            {
                case "left":
                    wParam = 0x41;
                    lParam = 0x11E0001;
                    //wParam = 0x25;
                    //lParam = 0x14B0001;
                    break;

                case "up":
                    wParam = 0x57;
                    lParam = 0x1110001;
                    //wParam = 0x26;
                    //lParam = 0x1480001;
                    break;

                case "right":
                    wParam = 0x44;
                    lParam = 0x1200001;
                    //wParam = 0x27;
                    //lParam = 0x14D0001;
                    break;

                case "down":
                    wParam = 0x53;
                    lParam = 0x11F0001;
                    //wParam = 0x28;
                    //lParam = 0x1500001;
                    break;

                default:
                    return false;
            }

            //Post the WM_KEYDOWN message, return false if unsuccessful
            if (_PostMessage(hWnd, 0x100, wParam, lParam) == 0)
                return false;

            //Sleep to let the window process the message
            Thread.Sleep(5);

            //Post the WM_KEYUP message, return false if unsuccessful
            if (_PostMessage(hWnd, 0x101, wParam, (lParam + 0xC0000000)) == 0)
                return false;

            return true;
            //}
        }

        /// <summary>
        /// Holds down an arrow key for the specified time
        /// </summary>
        /// <param name="hWnd">The window's hWnd</param>
        /// <param name="key">The arrow key to be tapped ("left", "up", "right", or "down")</param>
        /// <param name="holdDelay">Number of milliseconds to hold down key</param>
        /// <returns>Returns true if successful, false if not</returns>
        public static bool ArrowKey(int hWnd, string key, int holdDelay)
        {
            //lock (thisLock)
            //{
            //If hWnd is 0 or parameter two is incorrect, return false
            if (hWnd <= 0 ||
                (key.ToLower() != "left" &&
                key.ToLower() != "right" &&
                key.ToLower() != "down" &&
                key.ToLower() != "up"))
                return false;

            int wParam;
            uint lParam;

            //Set up wParam and lParam based upon which button needs pressing
            switch (key.ToLower())
            {
                case "left":
                    //wParam = 0x41;
                    //lParam = 0x11E0001;
                    wParam = 0x25;
                    lParam = 0x14B0001;
                    break;

                case "up":
                    wParam = 0x57;
                    lParam = 0x1110001;
                    //wParam = 0x26;
                    //lParam = 0x1480001;
                    break;

                case "right":
                    //wParam = 0x44;
                    //lParam = 0x1200001;
                    wParam = 0x27;
                    lParam = 0x14D0001;
                    break;

                case "down":
                    wParam = 0x53;
                    lParam = 0x11F0001;
                    //wParam = 0x28;
                    //lParam = 0x1500001;
                    break;

                default:
                    return false;
            }

            //Post the WM_KEYDOWN message, return false if unsuccessful
            if (_PostMessage(hWnd, 0x100, wParam, lParam) == 0)
                return false;

            //Sleep for half a second to emulate the delay you get when you hold a key down on your keyboard
            Thread.Sleep(500);

            //Loop until i >= delay specified in parameter 3
            for (int i = 0; i < holdDelay; i += 50)
            {
                //Post the WM_KEYDOWN message with the repeat flag turned on, return false if unsuccessful
                if (_PostMessage(hWnd, 0x100, wParam, (lParam + 0x40000000)) == 0)
                    return false;

                //Sleep for 1/20th of a second between posting the message
                Thread.Sleep(50);
            }

            //Post the WM_KEYUP message, return false if unsuccessful
            if (_PostMessage(hWnd, 0x101, wParam, (lParam + 0xC0000000)) == 0)
                return false;

            return true;
            //}
        }

        public static bool ArrowKey(int hWnd, string key, bool press)
        {
            //lock (thisLock)
            //{
            //If hWnd is 0 or parameter two is incorrect, return false
            if (hWnd <= 0 ||
                (key.ToLower() != "left" &&
                key.ToLower() != "right" &&
                key.ToLower() != "down" &&
                key.ToLower() != "up" &&
                key.ToLower() != "enter"))
                return false;

            int wParam;
            uint lParam;

            //Set up wParam and lParam based upon which button needs pressing
            switch (key.ToLower())
            {
                case "left":
                    //wParam = 0x41;
                    //lParam = 0x11E0001;
                    wParam = 0x25;
                    lParam = 0x14B0001;
                    break;

                case "up":
                    //wParam = 0x57;
                    //lParam = 0x1110001;
                    wParam = 0x26;
                    lParam = 0x1480001;
                    break;

                case "right":
                    //wParam = 0x44;
                    //lParam = 0x1200001;
                    wParam = 0x27;
                    lParam = 0x14D0001;
                    break;

                case "down":
                    //wParam = 0x53;
                    //lParam = 0x11F0001;
                    wParam = 0x28;
                    lParam = 0x1500001;
                    break;

                case "enter":
                    wParam = 0x0D;
                    lParam = 0x11C0001;
                    break;

                default:
                    return false;
            }

            int tries = 0;
            if (press)
            {
                //Post the WM_KEYDOWN message, return false if unsuccessful
                while (tries < 5)
                {
                    try
                    {
                        if (_PostMessage(hWnd, 0x100, wParam, lParam) == 0)
                            try
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }
                            catch (Exception ex)
                            {
                                Debug.Print("Error nest on post message press and hold: " + ex.Message);
                                return false;
                            }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("Error on post message press and hold: " + ex.Message);
                        return false;
                    }
                    Thread.Sleep(30);
                    tries++;
                }
            }

            //Post the WM_KEYDOWN message with the repeat flag turned on, return false if unsuccessful
            //if (_PostMessage(hWnd, 0x100, wParam, (lParam + 0x40000000)) == 0)
            //   return false;

            if (!press)
            {
                while (tries < 5)
                {
                    //Post the WM_KEYUP message, return false if unsuccessful
                    try
                    {
                        if (_PostMessage(hWnd, 0x101, wParam, (lParam + 0xC0000000)) == 0)
                            try
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }
                            catch (Exception ex)
                            {
                                Debug.Print("Error on post message hold release: " + ex.Message);
                                return false;
                            }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("Error on post message press and hold: " + ex.Message);
                        return false;
                    }
                    Thread.Sleep(30);
                    tries++;
                }
            }

            return true;
            //}
        }

        public static void LinearSmoothMove(float distance, int steps, string direction)
        {
            if (direction.ToLower() == "left")
                distance *= -1;
            
            POINT start; // = GetCursorPosition();
            GetCursorPos(out start);
            Debug.WriteLine(start.X);

            // Find the slope of the line segment defined by start and newPosition
            // Divide by the number of steps

            // Move the mouse to each iterative point.
            for (int i = 0; i < steps; i++)
            {
                SetCursorPos(start.X + (int)((distance / steps) * i), start.Y);
                //SetCursorPosition(Point.Round(iterPoint));
                Thread.Sleep(5);
            }
        }

        public static bool mouseHold(int hWnd, string button, int x, int y, bool depress)
        {
            RECT rec;
            GetWindowRect(hWnd, out rec);
            x += rec.Left;
            y -= rec.Top;

            Debug.WriteLine(x);
            Debug.WriteLine(y);

            if (button.ToLower() == "left")
            {
                if (!depress)
                {
                    //Post the WM_LBUTTONDOWN message
                    if (_PostMessage(hWnd, 0x201, 1, MakeLong(x, y)) == 0)
                        return false;
                }
                else
                {
                    //Post the WM_LBUTTONUP message
                    if (_PostMessage(hWnd, 0x202, 0, MakeLong(x, y)) == 0)
                        return false;
                }
            }
            else
            {
                if (!depress)
                {
                    //Post the WM_RBUTTONDOWN message
                    if (_PostMessage(hWnd, 0x204, 2, MakeLong(x, y)) == 0)
                        return false;
                }
                else
                {
                    //Post the WM_RBUTTONUP message
                    if (_PostMessage(hWnd, 0x205, 0, MakeLong(x, y)) == 0)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Send one mouse click to specified window
        /// </summary>
        /// <param name="hWnd">Window's hWnd</param>
        /// <param name="button">Button that will be clicked (left or right)</param>
        /// <param name="x">X Coordinate on which to click</param>
        /// <param name="y">Y Coordinate on which to click</param>
        /// <returns>Returns true if successful, false if otherwise</returns>
        public static bool MouseClick(int hWnd, string button, int x, int y)
        {
            //lock (thisLock)
            //{
            //If hWnd is 0, return false
            if (hWnd <= 0)
                return false;

            //If string in parameter 2 isn't either "left" or "right"
            if (button.ToLower() != "left" && button.ToLower() != "right")
                return false;

            //Post the WM_MOUSEMOVE message
            if (_PostMessage(hWnd, 0x200, 0, MakeLong(x, y)) == 0)
                return false;

            //Figure out which button to click
            if (button.ToLower() == "left")
            {
                //Post the WM_LBUTTONDOWN message
                if (_PostMessage(hWnd, 0x201, 1, MakeLong(x, y)) == 0)
                    return false;

                //Post the WM_LBUTTONUP message
                if (_PostMessage(hWnd, 0x202, 0, MakeLong(x, y)) == 0)
                    return false;
            }
            else
            {
                //Post the WM_RBUTTONDOWN message
                if (_PostMessage(hWnd, 0x204, 2, MakeLong(x, y)) == 0)
                    return false;

                //Post the WM_RBUTTONUP message
                if (_PostMessage(hWnd, 0x205, 0, MakeLong(x, y)) == 0)
                    return false;
            }

            return true;
            //}
        }

        public static void setCursor(int x, int y) {
            SetCursorPos(x, y);
        }

        // simulates movement of the mouse.  parameters specify an
        // absolute location, with the top left corner being the
        // origin
        public static void MoveTo(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x, y, 0, 0);
        }


        // simulates a click-and-release action of the left mouse
        // button at its current position
        public static void RightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        /// <summary>
        /// Sends a mouse click to specified window
        /// </summary>
        /// <param name="hWnd">Window's hWnd</param>
        /// <param name="button">String signifying which button to click (left or right)</param>
        /// <param name="x">X Coordinate on which to click</param>
        /// <param name="y">Y Coordinate on which to click</param>
        /// <param name="times">Number of times to click</param>
        /// <param name="delay">Delay in milliseconds between clicks</param>
        /// <returns>Returns true if successful, false if otherwise</returns>
        public static bool MouseClick(int hWnd, string button, int x, int y, int times, int delay)
        {
            //lock (thisLock)
            //{
            //If hWnd is 0, return false
            if (hWnd <= 0)
                return false;

            //If string in parameter 2 isn't either "left" or "right"
            if (button.ToLower() != "left" && button.ToLower() != "right")
                return false;

            //Post the WM_MOUSEMOVE message
            if (_PostMessage(hWnd, 0x200, 0, MakeLong(x, y)) == 0)
                return false;

            if (button.ToLower() == "left")
            {
                //Click number of time specified in parameter 5
                for (int i = 0; i < times; ++i)
                {
                    //Post WM_LBUTTONDOWN message and return false if it fails
                    if (_PostMessage(hWnd, 0x201, 1, MakeLong(x, y)) == 0)
                        return false;

                    //Post WM_LBUTTONUP message and return false if it fails;
                    if (_PostMessage(hWnd, 0x202, 0, MakeLong(x, y)) == 0)
                        return false;

                    //Sleep as long as specified in parameter 6
                    Thread.Sleep(delay);
                }
            }
            else
            {
                //Click number of time specified in parameter 5
                for (int i = 0; i < times; ++i)
                {
                    //Post WM_RBUTTONDOWN message and return false if it fails
                    if (_PostMessage(hWnd, 0x201, 2, MakeLong(x, y)) == 0)
                        return false;

                    //Post WM_RBUTTONUP message and return false if it fails;
                    if (_PostMessage(hWnd, 0x202, 0, MakeLong(x, y)) == 0)
                        return false;

                    //Sleep as long as specified in parameter 6
                    Thread.Sleep(delay);
                }
            }
            //If it's gotten this far, it must have succeeded
            return true;
            //}
        }

        /// <summary>
        /// Create the lParam for PostMessage
        /// </summary>
        /// <param name="a">HiWord</param>
        /// <param name="b">LoWord</param>
        /// <returns>Returns the long value</returns>
        private static uint MakeLong(int a, int b)
        {
            return (uint)((uint)((ushort)(a)) | ((uint)((ushort)(b) << 16)));
        }
    }

    /// <summary>
    /// Gather information about windows using IsWindowVisible and GetWindowText API
    /// </summary>
    public static class WindowInfo
    {
        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        private static extern bool _IsWindowVisible(int hWnd);
        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        private static extern int _GetWindowText(int hWnd, StringBuilder buf, int nMaxCount);

        /// <summary>
        /// Returns the window title of the specified window
        /// </summary>
        /// <param name="hWnd">A handle to the window</param>
        /// <param name="length">Length of the string to be returned</param>
        /// <returns></returns>
        public static string GetWindowTitle(int hWnd, int length)
        {
            StringBuilder str = new StringBuilder(length);
            _GetWindowText(hWnd, str, length);
            return str.ToString();
        }

        /// <summary>
        /// Returns true if the window is visible, false if not
        /// </summary>
        /// <param name="hWnd">A handle to the window</param>
        /// <returns></returns>
        public static bool IsVisible(int hWnd)
        {
            return _IsWindowVisible(hWnd);
        }
    }
    /// <summary>
    /// Enumerate open windows
    /// </summary>
    public class WindowArray : ArrayList
    {
        private delegate bool EnumWindowsCB(int handle, IntPtr param);

        [DllImport("user32")]
        private static extern int EnumWindows(EnumWindowsCB cb,
            IntPtr param);

        private static bool MyEnumWindowsCB(int hwnd, IntPtr param)
        {
            GCHandle gch = (GCHandle)param;
            WindowArray itw = (WindowArray)gch.Target;
            itw.Add(hwnd);
            return true;
        }

        /// <summary>
        /// Returns an array of all open windows and their hWnds
        /// </summary>
        public WindowArray()
        {
            GCHandle gch = GCHandle.Alloc(this);
            EnumWindowsCB ewcb = new EnumWindowsCB(MyEnumWindowsCB);
            EnumWindows(ewcb, (IntPtr)gch);
            gch.Free();
        }
    }
}