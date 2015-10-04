using GhostCoder.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCoder
{
    public class DeckManager
    {
        public DirectoryInfo BaseDirectory { get; private set; }

        public List<Deck> Decks { get; private set; }

        public DeckManager(DirectoryInfo baseDirectory)
        {
            BaseDirectory = baseDirectory;
            if (!BaseDirectory.Exists)
            {
                BaseDirectory.Create();
                File.WriteAllText(Path.Combine(BaseDirectory.FullName, "README.txt"), Resources.README);
            }

            Refresh();
        }

        public void Refresh()
        {
            Decks = new List<Deck>();
            foreach (var deckDir in BaseDirectory.GetDirectories().OrderBy(fi => fi.Name))
            {
                Decks.Add(new Deck(deckDir));
            }
        }
    }
}
