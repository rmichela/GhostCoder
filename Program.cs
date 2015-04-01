using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GhostCoder.Keyboard;
using GhostCoder.Menu;

namespace GhostCoder
{
    public class Program
    {

        private static readonly List<string> ScriptText = new List<string>
            {
                "This is my first string\r",
                "This is my second string\r",
                @"private static Dictionary<char, Keys> _keyMap = new Dictionary<char, Keys>
{
    // Alphabet keys
    {'A', Keys.A}, {'a', Keys.A}
};".Replace("\n", String.Empty)
            };


        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
//            var deck = new Deck("deck");
            using (var icon = new ProcessIcon())
            using (var hooker = new Hooker(ScriptText))
            {
                icon.Display();
                hooker.SetHook();

                Application.Run();
            }
        }
    }
}
