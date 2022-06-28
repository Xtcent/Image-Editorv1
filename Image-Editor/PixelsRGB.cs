using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
namespace Image_editor

{
    public class PixelsRGB
    {
        public byte R;
        public byte G;
        public byte B;
        public byte I;

        public PixelsRGB()
        {
            R = 0;
            G = 0;
            B = 0;
            I = 0;
        }



        public PixelsRGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            I = (byte)Math.Round(0.0722f * b + 0.715f * g + 0.212f * r);
        }

        public PixelsRGB hsvToRGB(int h, byte s, byte v)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            int hi = Convert.ToInt32(h / 60);
            byte Vmin = Convert.ToByte((255 - s) * v / 255);
            int a = Convert.ToInt32((v - Vmin) * (h % 60) / 60);
            byte Vinc = Convert.ToByte(Vmin + a);
            byte Vdec = Convert.ToByte(v - a);

            switch (hi)
            {

                case 0: {   r = v; g = Vinc; b = Vmin; break; }
                case 1: { r = Vdec; g = v; b = Vmin; break; }
                case 2: { r = Vmin; g = v; b = Vinc; break; }
                case 3: { r = Vmin; g = Vdec; b = v; break; }
                case 4: { r = Vinc; g = Vmin; b = v; break; }
                case 5: { r = v; g = Vmin; b = Vdec; break; }



            }
            PixelsRGB rgbPix = new PixelsRGB(r, g, b);
            return rgbPix;

        }
        /*
        public PixelsRGB cmykToRGB(double C, double M, double Y, double K)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;


            //Calculate R value
            //R = 255 × (1-C) × (1-K)
            r = Convert.ToByte(255 * (1 - C) * (1 - K));

            //Calculate G value
            //G = 255 × (1-M) × (1-K)
            g = Convert.ToByte(255 * (1 - M) * (1 - K));

            //Calculate B value
            //B = 255 × (1-Y) × (1-K)
            b = Convert.ToByte(255 * (1 - Y) * (1 - K));

            PixelsRGB rgbPix = new PixelsRGB(r, g, b);
            return rgbPix;

        }
        */

    }


}

