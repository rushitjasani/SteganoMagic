using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    class TextInImage : ITextInImage
    {
        public Bitmap Embedd(string path,string msg)
        {
            String binMsg = "";
            for (int i = 0; i < msg.Length; i++)
            {
                binMsg += Convert.ToString(msg[i], 2).PadLeft(8, '0');
            }
            binMsg += "11111111";

            SimpleObjectFactory sof = new SimpleObjectFactory();
            IImageFile ImageFile = sof.getImageFile();
            Bitmap bmp = ImageFile.getBitMap(path);
            int width = ImageFile.getWidth(path);
            int height = ImageFile.getHeight(path);
            int binIndex = 0;
            int flag = 0;

            IImageFile sImage = sof.getImageFile();
            Bitmap eMap = sImage.getBitMap(path);
            for (int y = 0; y < height && flag == 0; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bmp.GetPixel(x, y);

                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    if (int.Parse(binMsg[binIndex].ToString()) != r % 2)
                    {
                        if (binMsg[binIndex] == '0') r--;
                        else r++;
                    }
                    eMap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    binIndex++;
                    if (binIndex == binMsg.Length) { flag = 1; break; }
                    if (int.Parse(binMsg[binIndex].ToString()) != g % 2)
                    {
                        if (binMsg[binIndex] == '0') g--;
                        else g++;
                    }
                    eMap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    binIndex++;
                    if (binIndex == binMsg.Length) { flag = 1; break; }
                    if (int.Parse(binMsg[binIndex].ToString()) != b % 2)
                    {
                        if (binMsg[binIndex] == '0') b--;
                        else b++;
                    }
                    eMap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    binIndex++;
                    if (binIndex == binMsg.Length) { flag = 1; break; }
                }
            }


            return eMap;

        }
        public string Extract(string path)
        {

            SimpleObjectFactory sof = new SimpleObjectFactory();
            IImageFile ImageFile = sof.getImageFile();
            Bitmap bmp = ImageFile.getBitMap(path);
            int width = bmp.Width;
            int height = bmp.Height;
            int flag = 0;
            String msg = "",binMsg = "";

            //IImageFile sImage = sof.getImageFile();
            //Bitmap eMap = sImage.getBitMap(path);

            for (int y = 0; y < height && flag == 0; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bmp.GetPixel(x, y);

                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    binMsg += (r % 2).ToString();
                    if (binMsg.Length > 8)
                        if (binMsg.Substring(binMsg.Length - 8, 8).Equals("11111111") && binMsg.Length % 8 == 0)
                        { flag = 1; break; }

                    binMsg += (g % 2).ToString();
                    if (binMsg.Length > 8)
                        if (binMsg.Substring(binMsg.Length - 8, 8).Equals("11111111") && binMsg.Length % 8 == 0)
                        { flag = 1; break; }

                    binMsg += (b % 2).ToString();
                    if (binMsg.Length > 8)
                        if (binMsg.Substring(binMsg.Length - 8, 8).Equals("11111111") && binMsg.Length % 8 == 0)
                        { flag = 1; break; }
                }
            }

            binMsg = binMsg.Remove(binMsg.Length - 8);
            for (int i = 0; i < binMsg.Length; i += 8)
            {
                msg += (char)(128 * int.Parse(binMsg[i].ToString()) + 64 * int.Parse(binMsg[i + 1].ToString()) + 32 * int.Parse(binMsg[i + 2].ToString()) + 16 * int.Parse(binMsg[i + 3].ToString()) + 8 * int.Parse(binMsg[i + 4].ToString()) + 4 * int.Parse(binMsg[i + 5].ToString()) + 2 * int.Parse(binMsg[i + 6].ToString()) + int.Parse(binMsg[i + 7].ToString()));
            }
                

            return msg;
        }
    }
}
