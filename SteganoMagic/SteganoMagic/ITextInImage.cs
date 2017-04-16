using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public interface ITextInImage
    {
        Bitmap Embedd(string path, string msg);
        string Extract(string path);
    }
}
