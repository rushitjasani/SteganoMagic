using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public class SimpleObjectFactory
    {
        public ITextFile getTextFile()
        {
            return new TextFile();
        }

        public ITextInDoc getTextInDoc()
        {
            return new TextInDoc();
        }

        public ITextInImage getTextInImage()
        {
            return new TextInImage();
        }

        public IImageFile getImageFile()
        {
            return new ImageFile();
        }

        public IImageInImage getImageInImage()
        {
            return new ImageInImage();
        }
    }
}
