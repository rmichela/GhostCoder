using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

namespace GhostCoder
{
    class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int INJECTED = 16;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static InputSimulator _inputSimulator = new InputSimulator();

        private static List<string> _scripts = new List<string>
        {
            "Art party listicle umami cliche, tilde master cleanse normcore artisan mlkshk tattooed sriracha Tumblr next level mixtape.",
            "Street art before they sold out Pinterest, sartorial Marfa Schlitz four loko organic cronut PBR&B squid taxidermy.",
            "Sustainable Tumblr Tonx salvia, cray direct trade cronut wolf."
        };

        private static string _scriptText = 
@"private static Dictionary<char, Keys> _keyMap = new Dictionary<char, Keys>
{
    // Alphabet keys
    {'A', Keys.A}, {'a', Keys.A},
    {'B', Keys.B}, {'b', Keys.B},
    {'C', Keys.C}, {'c', Keys.C},
    {'D', Keys.D}, {'d', Keys.D},
    {'E', Keys.E}, {'e', Keys.E},
};".Replace("\n", String.Empty);

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

        private static bool _lastWasRealKey;

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    var hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                    var vkKey = (Keys)hookStruct.vkCode;

                    // Protect against injected keys triggering more injected keys
                    if ((vkKey == Keys.Packet || (hookStruct.flags & INJECTED) != 0) && _lastWasRealKey)
                    {
                        _lastWasRealKey = false;
                        return CallNextHookEx(_hookID, nCode, wParam, lParam);
                    }

                    // Allow backspace to do its job
                    if (vkKey == Keys.Back)
                    {
                        _lastWasRealKey = true;
                        if (offset > 0) _offset--;
                        Console.WriteLine("BACKSPACE");
                        return CallNextHookEx(_hookID, nCode, wParam, lParam);
                    }

                    if (KeyCharMap.IsPrintibaleKey(vkKey))
                    {                  
                        if (_offset < _scriptText.Length)
                        {
                            char offsetChar = _scriptText[_offset];
                            Console.WriteLine(PrintChar(offsetChar));
                            switch (offsetChar)
                            {
                                case '\n':
                                    break;
                                case '\r':
                                    _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                                    break;
                                default:
                                    _inputSimulator.Keyboard.TextEntry(offsetChar);
                                    break;
                            }

                            _lastWasRealKey = true;
                            _offset++;
                            return new IntPtr(1);
                        }
                    }
                }
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _offset++;
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
        }

        private static string PrintChar(char c)
        {
            switch (c)
            {
                case '\n':
                    return "NEWLINE";
                case '\r':
                    return "CARIAGE_RETURN";
                case '\t':
                    return "TAB";
                default:
                    return c.ToString();
            }
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
