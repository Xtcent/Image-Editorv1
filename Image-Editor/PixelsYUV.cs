using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_editor

{
    public class PixelsYUV
    {
        public int Y;
        public int U;
        public int V;

        

        public PixelsYUV()
        {
            Y = 0;
            U = 0;
            V = 0;
            
        }

        //RGB to YUV conversion
        public PixelsYUV(byte r, byte g, byte b)
        {
        

            float Yc;
            float Uc;
            float Vc;

            int Yi;
            int Ui;
            int Vi;


            Yc = ((0.299f * r) + (0.587f * g) + (0.114f * b));
            Uc = ((-0.14713f * r) - (0.28886f * g) + (0.436f * b) + 128);
            Vc = ((0.615f * r) - (0.51499f * g) - (0.10001f * b) + 128);
            Yi = Convert.ToInt32(Yc);
            Ui = Convert.ToInt32(Uc);
            Vi = Convert.ToInt32(Vc);
            Y = Yi;
            U = Ui;
            V = Vi;
        }

        //Yuv to RGB conversion
        public PixelsRGB yuvToRGB(int y, int u, int v)
        {
            byte R;
            byte G;
            byte B;
            double rd;
            double gd;
            double bd;


            rd = Convert.ToDouble((y + 1.13983f * (v - 128)));
            gd = Convert.ToDouble((y - 0.39465f * (u - 128) - (0.58060f * (v - 128))));
            //double test = (y + 2.03211f * (u - 128));
            //System.Diagnostics.Debug.WriteLine("yuvToRGb, test: "+ test);
            bd = Convert.ToDouble((y + 2.03211f * (u - 128)));

            R = (byte)Math.Max(0, Math.Min(255,(rd)));
            G = (byte)Math.Max(0, Math.Min(255,(gd)));
            B = (byte)Math.Max(0, Math.Min(255,(bd)));

            PixelsRGB rgbPix = new PixelsRGB(R, G, B);
            return rgbPix;
        }

    }
}
