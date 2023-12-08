using System.Drawing;
using System.Net;
using System;
using System.Runtime.InteropServices;

namespace mouses
{
    public class SendMouse
    {
        [DllImport("mouse.dll", EntryPoint = "MoveMouseTo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MoveMouseTo(int x, int y);
        [DllImport("mouse.dll", EntryPoint = "MoveMouseBy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MoveMouseBy(int x, int y);
        [DllImport("mouse.dll", EntryPoint = "MouseMW3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseMW3(int x, int y);
        [DllImport("mouse.dll", EntryPoint = "MouseBrink", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseBrink(int x, int y);
        [DllImport("user32.dll")]
        public static extern void SetPhysicalCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCaretPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int X, int Y);
        public static string drivertype;
        public Form1 form1 = new Form1();
        public void ViewData()
        {
            if (!form1.Visible)
            {
                form1.SetVisible();
            }
        }
        public void UnLoadKM()
        {
            SetKM("kmevent", 0, 0, 0, 0, 0, 0);
            SetKM("sendinput", 0, 0, 0, 0, 0, 0);
        }
        public void SetKM(string KeyboardMouseDriverType, double MouseMoveX, double MouseMoveY, double MouseAbsX, double MouseAbsY, double MouseDesktopX, double MouseDesktopY)
        {
            drivertype = KeyboardMouseDriverType;
            if (MouseMoveX != 0f | MouseMoveY != 0f)
                mousebrink((int)(MouseMoveX), (int)(MouseMoveY));
            if (MouseAbsX != 0f | MouseAbsY != 0f)
                mousemw3((int)(MouseAbsX), (int)(MouseAbsY));
            if (MouseDesktopX != 0f | MouseDesktopY != 0f)
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)(MouseDesktopX), (int)(MouseDesktopY));
                SetPhysicalCursorPos((int)(MouseDesktopX), (int)(MouseDesktopY));
                SetCaretPos((int)(MouseDesktopX), (int)(MouseDesktopY));
                SetCursorPos((int)(MouseDesktopX), (int)(MouseDesktopY));
            }
            if (form1.Visible)
            {
                string str = "KeyboardMouseDriverType : " + KeyboardMouseDriverType + Environment.NewLine;
                str += "MouseMoveX : " + MouseMoveX + Environment.NewLine;
                str += "MouseMoveY : " + MouseMoveY + Environment.NewLine;
                str += "MouseAbsX : " + MouseAbsX + Environment.NewLine;
                str += "MouseAbsY : " + MouseAbsY + Environment.NewLine;
                str += "MouseDesktopX : " + MouseDesktopX + Environment.NewLine;
                str += "MouseDesktopY : " + MouseDesktopY + Environment.NewLine;
                str += Environment.NewLine;
                form1.SetLabel1(str);
            }
        }
        public static void mousebrink(int x, int y)
        {
            if (drivertype == "sendinput")
                MoveMouseBy(x, y);
            else
                MouseBrink(x, y);
        }
        public static void mousemw3(int x, int y)
        {
            if (drivertype == "sendinput")
                MoveMouseTo(x, y);
            else
                MouseMW3(x, y);
        }
    }
}
