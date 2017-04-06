using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    class TextFile : ITextFile
    {
        string text;
        public string getTextFromFile(string path)
        {
            text = File.ReadAllText(path, Encoding.UTF8);
            return text;
        }
    }
}
