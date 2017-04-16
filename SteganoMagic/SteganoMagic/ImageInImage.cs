using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganoMagic
{
    public class ImageInImage : IImageInImage
    {
        public Bitmap Embedd(String cPath, String sPath)
        {

            SimpleObjectFactory sof = new SimpleObjectFactory();

            IImageFile imgs = sof.getImageFile();
            Bitmap bmps = imgs.getBitMap(sPath);
            int widths = imgs.getWidth(sPath);
            int heights = imgs.getHeight(sPath);

            IImageFile imgc = sof.getImageFile();
            Bitmap bmpc = imgs.getBitMap(cPath);
            int widthc = imgs.getWidth(cPath);
            int heightc = imgs.getHeight(cPath);

            Bitmap eMap = imgc.getBitMap(cPath);

            int cx = 0, cy = 0;

            long len = widths;
            string blen = Convert.ToString(len, 2);
            while (blen.Length != 15)
                blen = "0" + blen;

            len = heights;
            string stemp = Convert.ToString(len, 2);
            while (stemp.Length != 15)
                stemp = "0" + stemp;

            blen = blen + stemp;

            for (int r = 0; r < 10; r++)
            {
                Color lp = eMap.GetPixel(cx, cy);
                int ca = lp.A;
                int cr = lp.R;
                int cg = lp.G;
                int cb = lp.B;

                if (int.Parse(blen[3 * r].ToString()) != cr % 2)
                {
                    if (blen[3 * r] == '0') cr--;
                    else cr++;
                }
                if (int.Parse(blen[(3 * r) + 1].ToString()) != cg % 2)
                {
                    if (blen[(3 * r) + 1] == '0') cg--;
                    else cg++;
                }
                if (int.Parse(blen[(3 * r) + 2].ToString()) != cb % 2)
                {
                    if (blen[(3 * r) + 2] == '0') cb--;
                    else cb++;
                }
                eMap.SetPixel(cx, cy, Color.FromArgb(ca, cr, cg, cb));
                cx++;
            }


            for (int y = 0; y < heights; y++)
            {
                for (int x = 0; x < widths; x++)
                {
                    Color p = bmps.GetPixel(x, y);

                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    string temp = null;
                    string sa = Convert.ToString(a, 2);
                    while (sa.Length != 8)
                        sa = "0" + sa;
                    string sr = Convert.ToString(r, 2);
                    while (sr.Length != 8)
                        sr = "0" + sr;
                    string sg = Convert.ToString(g, 2);
                    while (sg.Length != 8)
                        sg = "0" + sg;
                    string sb = Convert.ToString(b, 2);
                    while (sb.Length != 8)
                        sb = "0" + sb;
                    //int cflag = 0;
                    temp = sa + sr + sg + sb + "0";
                    for (int m = 0; m < 11; m++)
                    {
                        Color cp = eMap.GetPixel(cx, cy);
                        int ca = cp.A;
                        int cr = cp.R;
                        int cg = cp.G;
                        int cb = cp.B;

                        if (int.Parse(temp[3 * m].ToString()) != cr % 2)
                        {
                            if (temp[3 * m] == '0') cr--;
                            else cr++;
                        }
                        if (int.Parse(temp[(3 * m) + 1].ToString()) != cg % 2)
                        {
                            if (temp[(3 * m) + 1] == '0') cg--;
                            else cg++;
                        }
                        if (int.Parse(temp[(3 * m) + 2].ToString()) != cb % 2)
                        {
                            if (temp[(3 * m) + 2] == '0') cb--;
                            else cb++;
                        }
                        eMap.SetPixel(cx, cy, Color.FromArgb(ca, cr, cg, cb));
                        cx++;
                        if (cx == widthc) { cx = 0; cy++; }
                    }
                }
            }

            return eMap;
        }

        public new Bitmap Extract(String sPath)
        {
            SimpleObjectFactory sof = new SimpleObjectFactory();

            IImageFile cimg = sof.getImageFile();
            Bitmap cbmp = cimg.getBitMap(sPath);
            int cwidth = cimg.getWidth(sPath);
            int cheight = cimg.getHeight(sPath);

            int cx = 0, cy = 0;
            string swidth = null, sheight = null;

            for (int i = 0; i < 5; i++)
            {
                Color lp = cbmp.GetPixel(cx, cy);
                int ca = lp.A;
                int cr = lp.R;
                int cg = lp.G;
                int cb = lp.B;
                swidth += (cr % 2).ToString();
                swidth += (cg % 2).ToString();
                swidth += (cb % 2).ToString();
                cx++;
            }


            for (int i = 0; i < 5; i++)
            {
                Color lp = cbmp.GetPixel(cx, cy);
                int ca = lp.A;
                int cr = lp.R;
                int cg = lp.G;
                int cb = lp.B;
                sheight += (cr % 2).ToString();
                sheight += (cg % 2).ToString();
                sheight += (cb % 2).ToString();
                cx++;
            }


            int sw = Convert.ToInt32(swidth, 2);
            int sh = Convert.ToInt32(sheight, 2);

            Bitmap s = new Bitmap(sw, sh);

            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    string all = null;
                    for (int k = 0; k < 11; k++)
                    {
                        Color lp = cbmp.GetPixel(cx, cy);
                        int ca = lp.A;
                        int cr = lp.R;
                        int cg = lp.G;
                        int cb = lp.B;

                        all += (cr % 2).ToString();
                        all += (cg % 2).ToString();
                        all += (cb % 2).ToString();

                        cx++;
                        if (cx == cwidth) { cx = 0; cy++; }
                    }
                    string alpha = all.Substring(0, 8);
                    int sa = Convert.ToInt32(alpha, 2);
                    string red = all.Substring(8, 8);
                    int sr = Convert.ToInt32(red, 2);
                    string green = all.Substring(16, 8);
                    int sg = Convert.ToInt32(green, 2);
                    string blue = all.Substring(24, 8);
                    int sb = Convert.ToInt32(blue, 2);

                    s.SetPixel(j, i, Color.FromArgb(sa, sr, sg, sb));
                }
            }

            return s;
        }

    }
}
