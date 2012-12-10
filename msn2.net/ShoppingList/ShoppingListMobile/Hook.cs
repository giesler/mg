using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace mn2.net.ShoppingList
{
    public class Hook
    {
        IntPtr _oldWndProc;
        IntPtr _hwnd;
        WndProcDelegate _newWndProc;

        public Hook()
        {
            _newWndProc = new WndProcDelegate(WndProc);
        }

        public void Attach(Control c)
        {
            Attach(c.Handle);
        }

        public void Attach(IntPtr hwnd)
        {
            _hwnd = hwnd;
            _oldWndProc = GetWindowLong(hwnd, GWL_WNDPROC);
            if (_oldWndProc == IntPtr.Zero) throw new Win32Exception();
            int r = SetWindowLong(hwnd, GWL_WNDPROC, _newWndProc);
            if (r == 0) throw new Win32Exception();
        }

        public void Detach()
        {
            int r = SetWindowLong(_hwnd, GWL_WNDPROC, _oldWndProc);
            if (r == 0) throw new Win32Exception();
        }


        protected virtual int WndProc(IntPtr hWnd, uint msg, IntPtr wparam, IntPtr lparam)
        {
            return CallWindowProc(_oldWndProc, hWnd, msg, wparam, lparam);
        }

        internal delegate int WndProcDelegate(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        #region P/Invoke declaration

        [DllImport("coredll.dll", SetLastError = true)]
        static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("coredll.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, WndProcDelegate newProc);
        [DllImport("coredll.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newProc);

        [DllImport("coredll.dll", SetLastError = true)]
        static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("coredll.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        #endregion

        const int GWL_WNDPROC = -4;
    }
}
