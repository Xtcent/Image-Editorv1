using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_editor
{
    public class PixelsHSV
    {
        public int H;
        public byte S;
        public byte V;

        public PixelsHSV()
        {
            H = 0;
            S = 0;
            V = 0;
        }

        //RGB to HSV conversion
        public PixelsHSV(byte r, byte g, byte b)
        {
            int MAX = Math.Max(r, Math.Max(g, b));
            int MIN = Math.Min(r, Math.Min(g, b));

            if (MAX == MIN) { H = 0; }
            else if ((MAX == r) && (g >= b)) { H = 60 * (g - b) / (MAX - MIN); }
            else if ((MAX == r) && (g < b)) { H = 60 * (g - b) / (MAX - MIN) + 360; }
            else if (MAX == g) { H = 60 * (b - r) / (MAX - MIN) + 120; }
            else { H = 60 * (r - g) / (MAX - MIN) + 240; }

            if (H == 360) { H = 0; }

            if (MAX == 0) { S = 0; }
            else { S = Convert.ToByte(255 * (1 - ((float)MIN / MAX))); }

            V = (byte)MAX;
        }

        //HSV to RGB conversion
        public PixelsRGB hsvToRGB(int h, byte s, byte v)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            int Hi = Convert.ToInt32(h / 60);
            byte Vmin = Convert.ToByte((255 - s) * v / 255);
            int a = Convert.ToInt32((v - Vmin) * (h % 60) / 60);
            byte Vinc = Convert.ToByte(Vmin + a);
            byte Vdec = Convert.ToByte(v - a);

            switch (Hi)
            {
                case 0: { r = v; g = Vinc; b = Vmin; break; }
                case 1: { r = Vdec; g = v; b = Vmin; break; }
                case 2: { r = Vmin; g = v; b = Vinc; break; }
                case 3: { r = Vmin; g = Vdec; b = v; break; }
                case 4: { r = Vinc; g = Vmin; b = v; break; }
                case 5: { r = v; g = Vmin; b = Vdec; break; }
            }

            PixelsRGB rgbPix = new PixelsRGB(r, g, b);
            return rgbPix;

        }


    }
}
