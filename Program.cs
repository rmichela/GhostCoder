using System;
using System.Windows.Forms;

namespace GhostCoder
{
    public class Program
    {
        
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


        public static void Main()
        {
            var deck = new Deck("deck");
            var hooker = new Hooker(deck);

            hooker.SetHook();
            Application.Run();
            hooker.ReleaseHook();
        }
    }
}
