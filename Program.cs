using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GhostCoder.Keyboard;
using GhostCoder.Menu;
using System.IO;

namespace GhostCoder
{
    public class Program
    {
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var deckManager = new DeckManager(new DirectoryInfo("Decks"));
            var trayMenu = new GcTrayMenu(deckManager);

            using (var trayIcon = new GcTrayIcon(trayMenu))
            using (var hooker = new Hooker())
            {
                hooker.SetDeck(trayMenu.SelectedDeck);

                trayIcon.DeckEnablementChanged += (o, e) => hooker.Enabled = e.Enabled;
                trayMenu.DeckSelected += (o, e) => hooker.SetDeck(e.SelectedDeck);

                trayIcon.Display();
                Application.Run();
            }
        }
    }
}
