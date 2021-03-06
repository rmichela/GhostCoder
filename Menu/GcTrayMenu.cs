﻿using GhostCoder.Properties;
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
            item.Click += (o, e) =>
            {
                _deckManager.Refresh();
                SelectedDeck = FindSelectedDeck();
                RefreshMenu();
                OnDeckSelected();
            };
            item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            item.Image = Resources.Refresh;
            Items.Add(item);

            item = new ToolStripMenuItem
            {
                Text = "Edit decks..."
            };
            item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            item.Image = Resources.Edit;
            item.Click += (o, e) => Process.Start("Explorer.exe", _deckManager.BaseDirectory.FullName);
            Items.Add(item);

            item = new ToolStripMenuItem
            {
                Text = "Exit"
            };
            item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            item.Image = Resources.Exit;
            item.Click += (o, e) => Application.Exit();
            Items.Add(item);
        }

        private Deck FindSelectedDeck()
        {
            if (SelectedDeck == null)
            {
                return _deckManager.Decks.FirstOrDefault();
            }
            Deck found = _deckManager.Decks.FirstOrDefault(d => d.Name == SelectedDeck.Name);
            if (found == null)
            {
                return _deckManager.Decks.FirstOrDefault();
            }
            return found;
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
