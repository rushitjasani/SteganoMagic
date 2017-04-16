using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public class ImageFile : IImageFile
    {
        public int getWidth(string path)
        {
            Bitmap bmp = new Bitmap(Image.FromFile(path));
            return bmp.Width;
        }
        public int getHeight(string path)
        {
            Bitmap bmp = new Bitmap(Image.FromFile(path));
            return bmp.Height;
        }

        public Bitmap getBitMap(string path)
        {
            return new Bitmap(Image.FromFile(path));
        }
    }
}
