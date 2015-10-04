using System;
using System.Collections.Generic;
using System.IO;

namespace GhostCoder
{
    public class Deck : List<string>
    {
        public string Name { get; private set; }

        public Deck(DirectoryInfo directory)
        {
            try
            {
                Name = directory.Name;

                var files = directory.GetFiles("*.txt");
                foreach (FileInfo file in files)
                {
                    using (var fs = file.OpenText())
                    {
                        var script = fs.ReadToEnd();
                        script = Sanitize(script);
                        Add(script);
                    }                       
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
