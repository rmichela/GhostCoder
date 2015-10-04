using System;

namespace GhostCoder.Menu
{
    public class DeckEnablementEventArgs : EventArgs
    {
        public bool Enabled { get; private set; }
        public DeckEnablementEventArgs(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
