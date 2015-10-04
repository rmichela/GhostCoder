using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GhostCoder.Properties;

namespace GhostCoder.Menu
{
    public class ProcessIcon : IDisposable
    {
        private readonly NotifyIcon _icon;
        private DeckMenu _deckMenu;

        public ProcessIcon(DeckMenu deckMenu)
        {
            _icon = new NotifyIcon();
            _deckMenu = deckMenu;
        }

        public void Display()
        {
            _icon.Icon = Resources.GhostIcon;
            _icon.Text = "Ghost Coder";

            _icon.ContextMenuStrip = _deckMenu.Create();
            _icon.Visible = true;
        }

        public void Dispose()
        {
            _icon.Dispose();
        }
    }
}
