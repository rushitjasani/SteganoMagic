using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public interface IImageInImage
    {
        string Embedd(string cbitstream, string dbitstream );
        string Extract(string sbitstream);
    }
}
