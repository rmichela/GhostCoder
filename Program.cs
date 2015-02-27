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

        private static string _scriptText = @"private static Dictionary<char, Keys> _keyMap = new Dictionary<char, Keys>
        {
            // Alphabet keys
            {'A', Keys.A}, {'a', Keys.A},
            {'B', Keys.B}, {'b', Keys.B},
            {'C', Keys.C}, {'c', Keys.C},
            {'D', Keys.D}, {'d', Keys.D},
            {'E', Keys.E}, {'e', Keys.E},
            {'F', Keys.F}, {'f', Keys.F},
            {'G', Keys.G}, {'g', Keys.G},
            {'H', Keys.H}, {'h', Keys.H},
            {'I', Keys.I}, {'i', Keys.I},
            {'J', Keys.J}, {'j', Keys.J},
            {'K', Keys.K}, {'k', Keys.K},
            {'L', Keys.L}, {'l', Keys.L},
            {'M', Keys.M}, {'m', Keys.M},
            {'N', Keys.N}, {'n', Keys.N},
            {'O', Keys.O}, {'o', Keys.O},
            {'P', Keys.P}, {'p', Keys.P},
            {'Q', Keys.Q}, {'q', Keys.Q},
            {'R', Keys.R}, {'r', Keys.R},
            {'S', Keys.S}, {'s', Keys.S},
            {'T', Keys.T}, {'t', Keys.T},
            {'U', Keys.U}, {'u', Keys.U},
            {'V', Keys.V}, {'v', Keys.V},
            {'W', Keys.W}, {'w', Keys.W},
            {'X', Keys.X}, {'x', Keys.X},
            {'Y', Keys.Y}, {'y', Keys.Y},
            {'Z', Keys.Z}, {'z', Keys.Z},

            // Number Keys
            {'1', Keys.D1}, {'!', Keys.D1},
            {'2', Keys.D2}, {'@', Keys.D2},
            {'3', Keys.D3}, {'#', Keys.D3},
            {'4', Keys.D4}, {'$', Keys.D4},
            {'5', Keys.D5}, {'%', Keys.D5},
            {'6', Keys.D6}, {'^', Keys.D6},
            {'7', Keys.D7}, {'&', Keys.D7},
            {'8', Keys.D8}, {'*', Keys.D8},
            {'9', Keys.D9}, {'(', Keys.D9},
            {'0', Keys.D0}, {')', Keys.D0},

            // Stand Alone Punctation
            {',', Keys.Oemcomma}, 
            {'<', Keys.Oemcomma},
            {'.', Keys.OemPeriod},
            {'>', Keys.OemPeriod},
            {'/', Keys.OemQuestion},
            {'?', Keys.OemQuestion},
            {';', Keys.OemSemicolon},
            {':', Keys.OemSemicolon},
            {'\'', Keys.OemQuotes},
            {'[', Keys.OemOpenBrackets},
            {'{', Keys.OemOpenBrackets},
            {']', Keys.OemCloseBrackets},
            {'}', Keys.OemCloseBrackets},
            {'\\', Keys.OemPipe},
            {'|', Keys.OemPipe},
            {'-', Keys.OemMinus},
            {'_', Keys.OemMinus},
            {'=', Keys.Oemplus},
            {'+', Keys.Oemplus},
            {'`', Keys.Oemtilde},
            {'~', Keys.Oemtilde},

            // Whitespace
            {'\n', Keys.Return},
            {'\t', Keys.Tab},
            {' ', Keys.Space},
        };";

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
            try
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
                                return CallNextHookEx(_hookID, nCode, wParam, lParam);
                            }
                            else
                            {
                                SendKeys.Send(offsetChar.ToString());
                                return new IntPtr(1);
                            }
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
