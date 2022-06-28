using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_editor
{
    public class PixelsCMYK
    {
        public double C;
        public double M;
        public double Y;
        public double K;
    

    public PixelsCMYK()
    {
            C = 0.0;
            M = 0.0;
            Y = 0.0;
            K = 0.0;
    }

    public PixelsCMYK(byte r, byte g, byte b)
        {

            //Change the range from 0...255 to 0.0...1.0
            double divide = 255.0;

            double Rprim = Convert.ToDouble((r / divide));
            double Gprim = Convert.ToDouble((g / divide));
            double Bprim = Convert.ToDouble((b / divide));

           
           //Rprim, Gprim, Bprim debug
            // System.Diagnostics.Debug.WriteLine("CalcCmyk, Rprim :"+Rprim);
           //System.Diagnostics.Debug.WriteLine("CalcCmyk, Gprim :" + Gprim);
           // System.Diagnostics.Debug.WriteLine("CalcCmyk, Bprim :" + Bprim);


            //Calculate K value
            //K = 1 - max(R', G', B')

            double RGBmax = Math.Max(Rprim, Gprim);
           // System.Diagnostics.Debug.WriteLine("CalcCmyk, Rgbmax1 :" + RGBmax);
            RGBmax = Math.Max(RGBmax, Bprim);
            //System.Diagnostics.Debug.WriteLine("CalcCmyk, Rgbmax2 :" + RGBmax);
             K = 1 - RGBmax;
          //  System.Diagnostics.Debug.WriteLine("CalcCmyk, K :" + K);


            //Calculate C value
            // C = (1-R'-K) / (1-K)

             C = ((1 - Rprim - K) / (1 - K));
         //   System.Diagnostics.Debug.WriteLine("CalcCmyk, C :" + C);

            //Calculate M value
            //M = (1-G'-K) / (1-K)


             M = ((1 - Gprim - K) / (1 - K));
         //   System.Diagnostics.Debug.WriteLine("CalcCmyk, M :" + M);

            //Calculate Y value


            //Y = (1-B'-K) / (1-K)
             Y = (1 - Bprim - K) / (1 - K);
           // System.Diagnostics.Debug.WriteLine("CalcCmyk, Y :" + Y);


        }

        public PixelsRGB cmykToRGB(double C, double M, double Y, double K)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            /*
            System.Diagnostics.Debug.WriteLine("cmykToRGB, testInput, C :" + C);
            System.Diagnostics.Debug.WriteLine("cmykToRGB, testInput, M :" + M);
            System.Diagnostics.Debug.WriteLine("cmykToRGB, testInput, Y :" + Y);
            System.Diagnostics.Debug.WriteLine("cmykToRGB, testInput, K :" + K);
            */

           

            //Calculate R value
            //R = 255 × (1-C) × (1-K)
            r = (byte)( (255 * (1 - C) * (1 - K)));
            //System.Diagnostics.Debug.WriteLine("cmykToRGB, r :" + r);
         
            //Calculate G value
            //G = 255 × (1-M) × (1-K)
            g = (byte)((255.0 * (1.0 - M) * (1.0 - K)));
            //System.Diagnostics.Debug.WriteLine("cmykToRGB, g :" + g);


            //Calculate B value
            //B = 255 × (1-Y) × (1-K)
            b = (byte)((255.0 * (1.0 - Y) * (1.0 - K)));
            //System.Diagnostics.Debug.WriteLine("cmykToRGB, B :" + b);


            PixelsRGB rgbPix = new PixelsRGB(r, g, b);
            return rgbPix;

        }

    }

}
