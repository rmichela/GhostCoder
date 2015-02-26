using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GhostCoder
{
    class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int INJECTED = 16;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static List<string> _scripts = new List<string>
        {
            "Art party listicle umami cliche, tilde master cleanse normcore artisan mlkshk tattooed sriracha Tumblr next level mixtape.",
            "Street art before they sold out Pinterest, sartorial Marfa Schlitz four loko organic cronut PBR&B squid taxidermy.",
            "Sustainable Tumblr Tonx salvia, cray direct trade cronut wolf."
        };

        private static string _scriptText = "Art party listicle umami cliche, tilde master cleanse normcore artisan mlkshk tattooed sriracha Tumblr next level mixtape.";

        private static int _script = 0;
        private static int _offset = 0;


        public static void Main()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Console.WriteLine(wParam);
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                var hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkKey = (Keys)hookStruct.vkCode;

                if (KeyCharMap.IsPrintibaleKey(vkKey))
                {                  
                    if (_offset < _scriptText.Length)
                    {
                        char offsetChar = _scriptText[_offset];
                        Keys offsetKey = KeyCharMap.GetKey(offsetChar);

                        if ((hookStruct.flags & INJECTED) != 0)
                        {
                            // Pass the key through                       
                            _offset++;
                            Console.WriteLine("{0}:Inject", _offset);
                            return CallNextHookEx(_hookID, nCode, wParam, lParam);
                        }
                        else
                        {
                            Console.WriteLine("{0}:Send", _offset);
                            SendKeys.Send(offsetChar.ToString());
                            return new IntPtr(1);
                        }
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/ms644967%28v=vs.85%29.aspx
        [StructLayout(LayoutKind.Sequential)] 
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode; 
            public int scanCode; 
            public int flags; 
            public int time; 
            public int dwExtraInfo; 

        };
    }
}
