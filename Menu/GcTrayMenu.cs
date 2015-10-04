using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace GhostCoder.Menu
{
    public class GcTrayMenu : ContextMenuStrip
    {
        public Deck SelectedDeck { get; private set; }
        private DeckManager _deckManager;

        public event EventHandler<DeckSelectedEventArgs> DeckSelected;

        public GcTrayMenu(DeckManager deckManager)
        {
            _deckManager = deckManager;
            SelectedDeck = _deckManager.Decks.FirstOrDefault();
            RefreshMenu();
        }

        private void RefreshMenu()
        {
            Items.Clear();
            ToolStripMenuItem item;

            foreach (var deck in _deckManager.Decks)
            {
                item = new ToolStripMenuItem
                {
                    Text = deck.Name,
                    Checked = deck == SelectedDeck
                };
                item.Click += (o, e) =>
                {
                    SelectedDeck = deck;
                    RefreshMenu();
                    OnDeckSelected();
                };
                Items.Add(item);
            }

            Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem
            {
                Text = "Refresh decks"
            };
            Click += (o, e) =>
            {
                _deckManager.Refresh();
                SelectedDeck = _deckManager.Decks.FirstOrDefault(d => d.Name == SelectedDeck.Name);
                RefreshMenu();
                OnDeckSelected();
            };
            Items.Add(item);

            item = new ToolStripMenuItem
            {
                Text = "Edit decks..."
            };
            item.Click += (o, e) => Process.Start("Explorer.exe", _deckManager.BaseDirectory.FullName);
            Items.Add(item);

            item = new ToolStripMenuItem
            {
                Text = "Exit"
            };
            item.Click += (o, e) => Application.Exit();
            Items.Add(item);
        }

        private void OnDeckSelected()
        {
            var e = DeckSelected;
            if (e != null)
            {
                e(this, new DeckSelectedEventArgs(SelectedDeck));
            }
        }
    }
}
