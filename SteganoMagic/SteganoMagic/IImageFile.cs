using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public interface IImageFile
    {
        int getWidth(String path);
        int getHeight(String path);
        Bitmap getBitMap(String path);
    }
}
