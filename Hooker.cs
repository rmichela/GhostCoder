using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace GhostCoder
{
    public class Hooker : IDisposable
    {
        private readonly List<string> _deck;
        private IntPtr _hookId;
        private int _deckOffset;
        private int _scriptOffset;
        private bool _lastWasRealKey;
        private readonly InputSimulator _inputSimulator = new InputSimulator();
        private Keys _scriptAdvanceKey = Keys.Tab;
        private bool _disposed;

        public Hooker(List<string> deck)
        {
            _deck = deck;
            _deckOffset = 0;
            _scriptOffset = 0;
        }

        public int DeckOffset
        {
            get { return _deckOffset; }
            set
            {
                if (value < 0) throw new ArgumentException("DeckOffset cannot be negative");
                if (value > _deck.Count) throw new ArgumentException("DeckOffset cannot be larger than deck size");
                _deckOffset = value;
            }
        }

        public Keys ScriptAdvanceKey
        {
            get { return _scriptAdvanceKey; }
            set { _scriptAdvanceKey = value; }
        }

        public void SetHook()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _hookId = Native.SetWindowsHookEx(Native.WH_KEYBOARD_LL, HookCallback, Native.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public void ReleaseHook()
        {
            Native.UnhookWindowsHookEx(_hookId);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && wParam == (IntPtr)Native.WM_KEYDOWN)
                {
                    var hookStruct = (Native.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.KBDLLHOOKSTRUCT));
                    var vkKey = (Keys)hookStruct.vkCode;

                    // Protect against injected keys triggering more injected keys
                    if ((vkKey == Keys.Packet || (hookStruct.flags & Native.INJECTED) != 0) && _lastWasRealKey)
                    {
                        _lastWasRealKey = false;
                        return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
                    }

                    // Allow backspace to do its job
                    if (vkKey == Keys.Back)
                    {
                        _lastWasRealKey = true;
                        if (_scriptOffset > 0) _scriptOffset--;
                        Console.WriteLine("BACKSPACE");
                        return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
                    }

                    // Advance the deck offset when the end of a script is reached and TAB is pressed
                    string scriptText = _deck[_deckOffset];
                    if (_scriptOffset >= scriptText.Length)
                    {
                        if (vkKey == _scriptAdvanceKey && _deckOffset < _deck.Count-1)
                        {
                            _deckOffset++;
                            _scriptOffset = 0;
                        }
                        return new IntPtr(1);
                    }

                    if (KeyCharMap.IsPrintibaleKey(vkKey))
                    {
                        if (_scriptOffset < scriptText.Length)
                        {
                            char offsetChar = scriptText[_scriptOffset];
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
                            _scriptOffset++;
                            return new IntPtr(1);
                        }
                    }
                }
                return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _scriptOffset++;
                return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
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
                    return c.ToString(CultureInfo.InvariantCulture);
            }
        }

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return; 

            if (disposing) {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            ReleaseHook();
            _disposed = true;
        }

        ~Hooker()
        {
            Dispose(false);
        }
    }
}
