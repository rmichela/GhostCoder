using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GhostCoder.Menu
{
    public class DeckMenu
    {
        private Deck _selectedDeck;
        private DeckManager _deckManager;

        public DeckMenu(DeckManager deckManager)
        {
            _deckManager = deckManager;
        }

        public ContextMenuStrip Create()
        {
            var contextMenu = new ContextMenuStrip();
            ToolStripMenuItem item;

            foreach (var deck in _deckManager.Decks)
            {
                item = new ToolStripMenuItem
                {
                    Text = deck.Name,
                    Checked = deck == _selectedDeck
                };
                contextMenu.Items.Add(item);
            }

            contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem
            {
                Text = "Refresh decks"
            };
            contextMenu.Items.Add(item);

            item = new ToolStripMenuItem
            {
                Text = "Edit decks..."
            };
            item.Click += (o, e) => Process.Start("Explorer.exe", _deckManager.BaseDirectory.FullName);
            contextMenu.Items.Add(item);

            item = new ToolStripMenuItem
            {
                Text = "Exit"
            };
            item.Click += (o, e) => Application.Exit();
            contextMenu.Items.Add(item);

            return contextMenu;
        }
    }
}
