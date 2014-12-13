using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GhostCoder
{
    public static class KeyCharMap
    {
        private static Dictionary<char, Keys> _keyMap = new Dictionary<char, Keys>
        {
            // Alphabet keys
            {'A', Keys.A}, {'a', Keys.A},
            {'B', Keys.B}, {'b', Keys.B},
            {'C', Keys.C}, {'c', Keys.C},
            {'D', Keys.D}, {'e', Keys.E},
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
            {'"', Keys.OemQuotes},
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
        };

        public static Keys GetKey(char c)
        {
            Keys k;
            if (_keyMap.TryGetValue(c, out k))
            {
                return k;
            }
            throw new KeyNotFoundException(string.Format("Could not locate a keymap for '{0}'", c));
        }

        public static bool IsPrintibaleKey(Keys k)
        {
            
        }
    }
}
