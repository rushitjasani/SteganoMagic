using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public interface ITextFile
    {
        string getTextFromFile(string path);
    }
}
