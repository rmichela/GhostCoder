using System;
using System.Windows.Forms;
using GhostCoder.Properties;

namespace GhostCoder.Menu
{
    public class GcTrayIcon : IDisposable
    {
        private readonly NotifyIcon _icon;
        private GcTrayMenu _deckMenu;
        private bool _enabled = false;

        public event EventHandler<DeckEnablementEventArgs> DeckEnablementChanged;

        public GcTrayIcon(GcTrayMenu deckMenu)
        {
            _icon = new NotifyIcon();
            _deckMenu = deckMenu;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                UpdateIcon();
                OnDeckEnablementChanged();
            }
        }

        public void Display()
        {
            UpdateIcon();
            _icon.Text = "Ghost Coder";
            _icon.MouseClick += (o, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    _enabled = !_enabled;
                    UpdateIcon();
                    OnDeckEnablementChanged();
                }
            };

            _icon.ContextMenuStrip = _deckMenu;
            _icon.Visible = true;
        }

        private void UpdateIcon()
        {
            _icon.Icon = _enabled ? Resources.GhostIcon : Resources.GhostIconX;
        }

        private void OnDeckEnablementChanged()
        {
            var e = DeckEnablementChanged;
            if (e != null)
            {
                e(this, new DeckEnablementEventArgs(_enabled));
            }
        }

        public void Dispose()
        {
            _icon.Dispose();
        }
    }
}
