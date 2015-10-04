using System;

namespace GhostCoder.Menu
{
    public class DeckSelectedEventArgs : EventArgs
    {
        public Deck SelectedDeck { get; private set; }
        public DeckSelectedEventArgs(Deck selectedDeck)
        {
            SelectedDeck = selectedDeck;
        }
    }
}
