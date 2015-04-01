using System;
using System.Collections.Generic;
using System.IO;

namespace GhostCoder
{
    public class Deck : List<string>
    {
        public Deck(string directory)
        {
            try
            {
                var files = Directory.GetFiles(directory);
                foreach (string file in files)
                {
                    var script = File.ReadAllText(file);
                    script = Sanitize(script);
                    Add(script);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        } 

        private string Sanitize(string script)
        {
            return script.Replace("\n", String.Empty);
        }
    }
}
