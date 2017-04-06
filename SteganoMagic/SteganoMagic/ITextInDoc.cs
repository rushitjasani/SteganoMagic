using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public interface ITextInDoc
    {
        string Embedd(string text, string msg);
        string Extract(string text);
    }
}
