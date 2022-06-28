using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
namespace Image_editor

{
   public class HistogramClass
    {   
        //Arrays for R G B I
        public int[] hR;
        public int[] hI;
        public int[] hG;
        public int[] hB;

        //Arrays for H S V
        public int[] hH;
        public int[] hS;
        public int[] hV;

        //Arrays for C M Y K
        public double[] hC;
        public double[] hM;
        public double[] hY;
        public double[] hK;

        //Arrays for Y U V
        public int[] hY1;
        public int[] hU;
        public int[] hV1;


        public HistogramClass()
        {   
            //Array init for R I G B
            hR = new int[256];
            hI = new int[256];
            hG = new int[256];
            hB = new int[256];

            //Array init for H S V
            hH = new int[1000];
            hS = new int[1000];
            hV = new int[1000];

            //Array init for C M Y K
            hC = new double[256];
            hM = new double[256];
            hY = new double[256];
            hK = new double[256];

            //Array init for Y U V
            hY1 = new int[256];
            hU = new int[256];
            hV1 = new int[256];


        }


        //Erase values from all histogram arrays
       
        public void eraseHistogram()
        {
            for(int i=0; i < 256; i++)
            {


                //R G B I
                hR[i] = 0;
                hI[i] = 0;
                hG[i] = 0;
                hB[i] = 0;

               

                // C M Y K
                hC[i] = 0;
                hM[i] = 0;
                hY[i] = 0;
                hK[i] = 0;

                // Y U V
                hY1[i] = 0;
                hU[i] = 0;
                hV1[i] = 0;



            }

            for (int i = 0; i < 1000; i++)
            {
                // H S V
                hH[i] = 0;
                hS[i] = 0;
                hV[i] = 0;

               
            }


        }

        //Overloaded histogram read methods for each type of histogram
        public void readHistogram(PixelsRGB[,] img, string context)
        {

           // eraseHistogram();
            //is this the right order?
            //Well order probably doesn't matter
            for(int x =0; x < img.GetLength(0); x++)
            {
                for(int y = 0; y<img.GetLength(1); y++)
                {
                      //Read value and add to histogram array;
                    if (context == "R") { hR[img[x, y].R]++; }
                    if (context == "I") { hI[img[x, y].I]++; }
                    if (context == "G") { hG[img[x, y].G]++; }
                    if (context == "B") { hB[img[x, y].B]++; }
                    if (context == null)
                    {
                        hR[img[x, y].R]++;
                        hI[img[x, y].I]++;
                        hG[img[x, y].G]++;
                        hB[img[x, y].B]++;
                    }
                }
            }
        }

        public void readHistogram(PixelsHSV[,] imgHSV)
        {

            //eraseHistogram();
            //is this the right order?
            //Well order probably doesn't matter
            for (int x = 0; x < imgHSV.GetLength(0); x++)
            {
                for (int y = 0; y < imgHSV.GetLength(1); y++)
                {
                    //System.Diagnostics.Debug.WriteLine(imgHSV.GetLength(0));
                    //680
                    //System.Diagnostics.Debug.WriteLine(imgHSV.GetLength(1));
                    //350
                    //238000
                    // System.Diagnostics.Debug.WriteLine("HistogramClass, readHSV, loop ,X:" + x);
                    //System.Diagnostics.Debug.WriteLine("HistogramClass, readHSV, loop ,Y:" + y);

                    //ask about this??
                    //Read value and add histogram to array
                    hH[imgHSV[x, y].H]++;
                    //hH[imgHSV[x, y].hsvToRGB(imgHSV[x, y].H, 255, 255).R]++;
                    hS[imgHSV[x, y].S]++;
                    hV[imgHSV[x, y].V]++;
                }
            }
        }

        //How to histogramm for doubles??
        //Not working correctly
        //TODO Fix this
        public void readHistogram(PixelsCMYK[,] imgCMYK)
        {

            // eraseHistogram();
            //is this the right order?
            //Well order probably doesn't matter
            //Read value and add histogram to array
            for (int x = 0; x < imgCMYK.GetLength(0); x++)
            {
                for (int y = 0; y < imgCMYK.GetLength(1); y++)
                {
                    hC[imgCMYK[x, y].cmykToRGB(imgCMYK[x, y].C, 0.0, 0.0, 0.0).R]++;
                    hM[imgCMYK[x, y].cmykToRGB(0.0, imgCMYK[x, y].M, 0.0, 0.0).G]++;
                    hY[imgCMYK[x, y].cmykToRGB(0.0, 0.0, imgCMYK[x, y].Y, 0.0).B]++;


                    //Probably need a combined histogramm?
                   // hK = ((0.299 * (imgCMYK[x, y].cmykToRGB(0.0, 0.0, 0.0, imgCMYK[x, y].K).R)) + (0.587 * (imgCMYK[x, y].cmykToRGB(0.0, 0.0, 0.0, imgCMYK[x, y].K).G)) + (0.114 *(imgCMYK[x, y].cmykToRGB(0.0, 0.0, 0.0, imgCMYK[x, y].K).B))));
                    hK[imgCMYK[x, y].cmykToRGB(0.0, 0.0, 0.0, imgCMYK[x, y].K).B]++;
                }
            }
        }
        
        public void readHistogram(PixelsYUV[,] imgYUV)
        {

            //eraseHistogram();

            //  int count = 0;
            //Read value and add histogram to array
            for (int x = 0; x < imgYUV.GetLength(0); x++)
            {
                for (int y = 0; y < imgYUV.GetLength(1); y++)
                {
                   
                    //castfix
                    hY1[(byte)imgYUV[x, y].Y]++;
                    hU[(byte)imgYUV[x, y].U]++;
                    hV1[(byte)imgYUV[x, y].V]++;
                
                }
            }
        }

     



        public void drawHistogram(Chart chart)
        {
            //Add things to chart
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add("ChartArea");
            chart.Series.Add("R");
            chart.Series["R"].Color = Color.Red;

            chart.Series.Add("G");
            chart.Series["G"].Color = Color.Green;

            chart.Series.Add("B");
            chart.Series["B"].Color = Color.Blue;

            chart.Series.Add("I");
            chart.Series["I"].Color = Color.Black;

            for(int i=0; i < 256; i++)
            {
                //populate chart with values
                chart.Series["R"].Points.AddXY(i, hR[i]);
                chart.Series["I"].Points.AddXY(i, hI[i]);
                chart.Series["G"].Points.AddXY(i, hG[i]);
                chart.Series["B"].Points.AddXY(i, hB[i]);
            }
                    


        }

        //Method that adds all the neccesary components and draws 
        // the specified histogram
        // Chart --Current chart
        // spec -- Specified color channel
        public void drawSpecificHistogram(Chart chart, string[] spec)
        {

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add("ChartArea");

            bool Rd, Gd, Bd, Id;
            bool Hd, Sd, Vd;
            bool Cd, Md, Yd, Kd;
            bool Y1d, Ud, V1d;

            Hd=Sd=Vd = false;
            Rd = Gd = Bd = Id = false;
            Cd = Md = Yd = Kd = false;
            Y1d = Ud = V1d = false;

            for (int i = 0; i < spec.Length; i++)
            {    //Add things to histogram based on string value
                switch (spec[i])
                {

                    //R G B I
                    case "R":
                        chart.Series.Add("R");
                        chart.Series["R"].Color = Color.Red;
                        Rd = true;
                        break;

                    case "G":
                        chart.Series.Add("G");
                        chart.Series["G"].Color = Color.Green;
                        Gd = true;
                        break;
                    case "B":
                        chart.Series.Add("B");
                        chart.Series["B"].Color = Color.Blue;
                        Bd = true;
                        break;
                    case "I":
                        chart.Series.Add("I");
                        chart.Series["I"].Color = Color.Black;
                        Id = true;
                        break;


                    case "RGB":

                        chart.Series.Add("R");
                        chart.Series["R"].Color = Color.Red;
                        Rd = true;

                        chart.Series.Add("G");
                        chart.Series["G"].Color = Color.Green;
                        Gd = true;

                        chart.Series.Add("B");
                        chart.Series["B"].Color = Color.Blue;
                        Bd = true;

                        chart.Series.Add("I");
                        chart.Series["I"].Color = Color.Black;
                        Id = true;
                        break;


                        // H S V
                    case "HSV":
                        chart.Series.Add("H");
                        chart.Series["H"].Color = Color.Red;
                        Hd = true;

                        chart.Series.Add("S");
                        chart.Series["S"].Color = Color.Yellow;
                        Sd = true;

                        chart.Series.Add("V");
                        chart.Series["V"].Color = Color.Black;
                        Vd = true;
                        break;


                    case "H":
                        chart.Series.Add("H");
                        chart.Series["H"].Color = Color.Red;
                        Hd = true;
                        break;
                    case "S":
                        chart.Series.Add("S");
                        chart.Series["S"].Color = Color.Yellow;
                        Sd = true;
                        break;
                    case "V":
                        chart.Series.Add("V");
                        chart.Series["V"].Color = Color.Black;
                        Vd = true;
                        break;


                        // Y U V
                    case "YUV":
                        chart.Series.Add("Y");
                        chart.Series["Y"].Color = Color.Black;
                        Y1d = true;

                        chart.Series.Add("U");
                        chart.Series["U"].Color = Color.Blue;
                        Ud = true;

                        chart.Series.Add("V");
                        chart.Series["V"].Color = Color.Red;
                        V1d = true;
                        break;


                    case "Y1":
                        chart.Series.Add("Y");
                        chart.Series["Y"].Color = Color.Black;
                        Y1d = true;
                        break;
                    case "U":
                        chart.Series.Add("U");
                        chart.Series["U"].Color = Color.Blue;
                        Ud = true;
                        break;
                    case "V1":
                        chart.Series.Add("V");
                        chart.Series["V"].Color = Color.Red;
                        V1d = true;
                        break;


                    //C M Y K
                    case "CMYK":
                        chart.Series.Add("C");
                        chart.Series["C"].Color = Color.Cyan;
                        Cd = true;

                        chart.Series.Add("M");
                        chart.Series["M"].Color = Color.Magenta;
                        Md = true;

                        chart.Series.Add("Y");
                        chart.Series["Y"].Color = Color.Yellow;
                        Yd = true;

                        chart.Series.Add("K");
                        chart.Series["K"].Color = Color.Black;
                        Kd = true;


                        break;
                    case "C":
                        chart.Series.Add("C");
                        chart.Series["C"].Color = Color.Cyan;
                        Cd = true;
                        break;
                    case "M":
                        chart.Series.Add("M");
                        chart.Series["M"].Color = Color.Magenta;
                        Md = true;
                        break;
                    case "Y":
                        chart.Series.Add("Y");
                        chart.Series["Y"].Color = Color.Yellow;
                        Yd = true;
                        break;
                    case "K":
                        chart.Series.Add("K");
                        chart.Series["K"].Color = Color.Black;
                        Kd = true;
                        break;


                }
            }
            for (int i = 0; i < 256; i++)
            {   

                //TODO fix this?
                //Populate chart R G B I
                if (Rd) { chart.Series["R"].Points.AddXY(i, hR[i]); }
                if (Gd) { chart.Series["G"].Points.AddXY(i, hG[i]); }
                if (Bd) { chart.Series["B"].Points.AddXY(i, hB[i]); }
                if (Id) { chart.Series["I"].Points.AddXY(i, hI[i]); }

                //Populate chart H S V
                if (Hd) { chart.Series["H"].Points.AddXY(i, hH[i]); }
                if (Sd) { chart.Series["S"].Points.AddXY(i, hS[i]); }
                if (Vd) { chart.Series["V"].Points.AddXY(i, hV[i]); }

                // Populate chart Y U V
                if (Y1d) { chart.Series["Y"].Points.AddXY(i, hY1[i]); }
                if (Ud) { chart.Series["U"].Points.AddXY(i, hU[i]); }
                if (V1d) { chart.Series["V"].Points.AddXY(i, hV1[i]); }

                //Populate chart C M Y K
                if (Cd) { chart.Series["C"].Points.AddXY(i, hC[i]); }
                if (Md) {  chart.Series["M"].Points.AddXY(i, hM[i]); } 
                if (Yd) { chart.Series["Y"].Points.AddXY(i, hY[i]); }
                if (Kd) { chart.Series["K"].Points.AddXY(i, hK[i]); }


            }

           


        }

        //Histogram stretching implementation, currently not working
        //Takes in the original image and modifies its contrast and returns
        //the modified image.
        public PixelsRGB[,] stretchHistogram(int[] a ,PixelsRGB[,] img, PixelsRGB[,] img2, string context)
        {
         
            int Ddesired = 255;
            int Dbegin = 0;
            int Dend = 0;
            int Doriginal = 0;
            float k = 0;
          
            //Calculate Dbegin and Dend
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > 0) { Dbegin = a[i]; break; }
            }
            for(int i = 255; i <= 0; i++)
            {
                if (a[i] > 0) { Dend = a[i]; break; }
            }

            Doriginal = Dend - Dbegin;
            k = Ddesired / Doriginal;

            // XY or YX ??
            double result = 0;
            for (int y = 0; y < img.GetLength(1); y++)
                
                
            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                    if (context == "I")
                    {   result = Convert.ToDouble(k * (img[x, y].I - Dbegin));
                        img2[x, y].I = (byte)(result);
                        
                    }
                    if(context == "R")
                    {
                        img2[x, y].R = (byte)(k * (img[x, y].R - Dbegin));
                    }
                    if(context == "G")
                    {
                        img2[x, y].G = (byte)(k * (img[x, y].G - Dbegin));
                    }
                    if(context == "B")
                    {
                        img2[x, y].B = (byte)(k * (img[x, y].B - Dbegin));
                    }
                    
                }
            }

            return img2;
        }

        public PixelsRGB[,] stretchHistogram(int[] hR, int[] hG, int[] hB, int[] hI, PixelsRGB[,] img, PixelsRGB[,] img2)
        {
           
            int DdesiredR = 255;
            int DbeginR = 0;
            int DendR = 0;
            int DoriginalR = 0;
            int kR = 0;

            int DdesiredG = 255;
            int DbeginG = 0;
            int DendG = 0;
            int DoriginalG = 0;
            int kG = 0;

            int DdesiredB = 255;
            int DbeginB = 0;
            int DendB = 0;
            int DoriginalB = 0;
            int kB = 0;

            int DdesiredI = 255;
            int DbeginI = 0;
            int DendI = 0;
            int DoriginalI = 0;
            int kI = 0;

            //int Inew = 0;
            //int Iold = img.;



            //Calculate Dbegin and Dend
            for (int i = 0; i < hR.Length; i++)
            {
                if (hR[i] > 0) { DbeginR = hR[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hR[i] > 0) { DendR = hR[i]; break; }
            }

            DoriginalR = DendR - DbeginR;
            kR = DdesiredR / DoriginalR;


            //Calculate Dbegin and Dend
            for (int i = 0; i < hG.Length; i++)
            {
                if (hG[i] > 0) { DbeginG = hG[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hG[i] > 0) { DendG = hG[i]; break; }
            }

            DoriginalG = DendG - DbeginG;
            kG = DdesiredG / DoriginalG;



            //Calculate Dbegin and Dend
            for (int i = 0; i < hB.Length; i++)
            {
                if (hB[i] > 0) { DbeginB = hB[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hB[i] > 0) { DendB = hB[i]; break; }
            }

            DoriginalB = DendB - DbeginB;
            kB = DdesiredB / DoriginalB;



            //Calculate Dbegin and Dend
            for (int i = 0; i < hI.Length; i++)
            {
                if (hI[i] > 0) { DbeginI = hI[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hI[i] > 0) { DendI = hI[i]; break; }
            }

            DoriginalI = DendI - DbeginI;
            kI = DdesiredI / DoriginalI;



            for (int y = 0; y < img.GetLength(1); y++)
            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                  
                    
                    
                    
                        img2[x, y].R = (byte)(kR * (img[x, y].R - DbeginR));
                        img2[x, y].G = (byte)(kG * (img[x, y].G - DbeginG));
                        img2[x, y].B = (byte)(kB * (img[x, y].B - DbeginB));
                        img2[x, y].I = (byte)(kI * (img[x, y].I - DbeginI));
                    
                    
                }
            }

            return img2;
        }



        public PixelsHSV[,] stretchHistogramHSV(int[] a, PixelsHSV[,]img, PixelsHSV[,] img2, string context)
        {
            int Ddesired = 255;
            int Dbegin = 0;
            int Dend = 0;
            int Doriginal = 0;
            float k = 0;
            //int Inew = 0;
            //int Iold = img.;



            //Calculate Dbegin and Dend
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > 0) { Dbegin = a[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (a[i] > 0) { Dend = a[i]; break; }
            }

            Doriginal = Dend - Dbegin;
            k = Ddesired / Doriginal;


            // XY or YX ??
            double result = 0;
            for (int y = 0; y < img.GetLength(1); y++)


            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                    if (context == "H")
                    {
                        result = Convert.ToDouble(k * (img[x, y].H - Dbegin));
                        img2[x, y].H = (byte)(result);

                    }
                    if (context == "S")
                    {
                        img2[x, y].S = (byte)(k * (img[x, y].S - Dbegin));
                    }
                    if (context == "V")
                    {
                        img2[x, y].V = (byte)(k * (img[x, y].V - Dbegin));
                    }
                  
                }
            }



            return img2;
        }

        public PixelsHSV[,] stretchHistogramHSV(int[] hH, int[] hS, int[] hV, PixelsHSV[,] img, PixelsHSV[,] img2, string context)
        {
            int DdesiredH = 255;
            int DbeginH = 0;
            int DendH = 0;
            int DoriginalH = 0;
            float kH = 0;


            int DdesiredS = 255;
            int DbeginS = 0;
            int DendS = 0;
            int DoriginalS = 0;
            float kS = 0;

            int DdesiredV = 255;
            int DbeginV = 0;
            int DendV = 0;
            int DoriginalV = 0;
            float kV = 0;



            //Calculate Dbegin and Dend H
            for (int i = 0; i < hH.Length; i++)
            {
                if (hH[i] > 0) { DbeginH = hH[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hH[i] > 0) { DendH = hH[i]; break; }
            }

            DoriginalH = DendH - DbeginH;
            kH = DdesiredH / DoriginalH;


            ////Calculate Dbegin and Dend S
            for (int i = 0; i < hS.Length; i++)
            {
                if (hS[i] > 0) { DbeginS = hS[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hS[i] > 0) { DendS = hS[i]; break; }
            }

            DoriginalS = DendS - DbeginS;
            kS = DdesiredS / DoriginalS;

            ////Calculate Dbegin and Dend V
            for (int i = 0; i < hV.Length; i++)
            {
                if (hV[i] > 0) { DbeginV = hV[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hV[i] > 0) { DendV = hV[i]; break; }
            }

            DoriginalV = DendV - DbeginV;
            kV = DdesiredV / DoriginalV;


            // XY or YX ??
            double result = 0;
            for (int y = 0; y < img.GetLength(1); y++)


            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                    if (context == "HSV")
                    {
                        result = Convert.ToDouble(kH * (img[x, y].H - DbeginH));
                        img2[x, y].H = (byte)(result);
                        img2[x, y].S = (byte)(kS * (img[x, y].S - DbeginS));
                        img2[x, y].V = (byte)(kV * (img[x, y].V - DbeginV));
                    }
                  

                }
            }



            return img2;
        }


        //Does it work for doubles???
        public PixelsCMYK[,] stretchHistogramCMYK(double[] a, PixelsCMYK[,] img, PixelsCMYK[,] img2, string context)
        {
            double Ddesired = 1.0;
            double Dbegin = 0;
            double Dend = 0;
            double Doriginal = 0;
            float k = 0;
            //int Inew = 0;
            //int Iold = img.;



            //Calculate Dbegin and Dend
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > 0) { Dbegin = a[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (a[i] > 0) { Dend = a[i]; break; }
            }

            Doriginal = Dend - Dbegin;
            k = (float) (Ddesired / Doriginal);


            // XY or YX ??
            double result = 0;

            for (int y = 0; y < img.GetLength(1); y++)

            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                    if (context == "C")
                    {
                        result = Convert.ToDouble(k * (img[x, y].C - Dbegin));
                        img2[x, y].C = (double)(result);

                    }
                    if (context == "M")
                    {
                        img2[x, y].M = Convert.ToDouble((k * (img[x, y].M - Dbegin)));
                    }
                    if (context == "Y")
                    {
                        img2[x, y].Y = Convert.ToDouble((k * (img[x, y].Y - Dbegin)));
                    }
                    if (context == "K")
                    {
                        img2[x, y].K = Convert.ToDouble((k * (img[x, y].K - Dbegin)));
                    }
                }
            }



            return img2;
        }

        public PixelsCMYK[,] stretchHistogramCMYK(double[] hC, double[] hM, double[]hY, double[] hK, PixelsCMYK[,] img, PixelsCMYK[,] img2, string context)
        {

            double DdesiredC = 1.0;
            double DbeginC = 0;
            double DendC = 0;
            double DoriginalC = 0;
            float kC = 0;

            double DdesiredM = 1.0;
            double DbeginM = 0;
            double DendM = 0;
            double DoriginalM = 0;
            float kM = 0;

            double DdesiredY = 1.0;
            double DbeginY = 0;
            double DendY = 0;
            double DoriginalY = 0;
            float kY = 0;

            double DdesiredK = 1.0;
            double DbeginK = 0;
            double DendK = 0;
            double DoriginalK = 0;
            float kK = 0;


            //Calculate Dbegin and Dend
            //hc
            for (int i = 0; i < hC.Length; i++)
            {
                if (hC[i] > 0) { DbeginC = hC[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hC[i] > 0) { DendC = hC[i]; break; }
            }

            DoriginalC = DendC - DbeginC;
            kC = (float)(DdesiredC / DoriginalC);



            //hm
            for (int i = 0; i < hM.Length; i++)
            {
                if (hM[i] > 0) { DbeginM = hM[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hM[i] > 0) { DendM = hM[i]; break; }
            }

            DoriginalM = DendM - DbeginM;
            kM = (float)(DdesiredM / DoriginalM);



            //hy
            for (int i = 0; i < hY.Length; i++)
            {
                if (hY[i] > 0) { DbeginY = hY[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hY[i] > 0) { DendY = hY[i]; break; }
            }

            DoriginalY = DendY - DbeginY;
            kY = (float)(DdesiredY / DoriginalY);


            //hk
            for (int i = 0; i < hK.Length; i++)
            {
                if (hK[i] > 0) { DbeginK = hK[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hK[i] > 0) { DendK = hK[i]; break; }
            }

            DoriginalK = DendK - DbeginK;
            kK = (float)(DdesiredK / DoriginalK);


            // XY or YX ??
            double result = 0;

            for (int y = 0; y < img.GetLength(1); y++)

            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                    if (context == "CMYK")
                    {
                        result = Convert.ToDouble(kC * (img[x, y].C - DbeginC));
                        img2[x, y].C = (double)(result);
                        img2[x, y].M = Convert.ToDouble((kM * (img[x, y].M - DbeginM)));
                        img2[x, y].Y = Convert.ToDouble((kY * (img[x, y].Y - DbeginY)));
                        img2[x, y].K = Convert.ToDouble((kK * (img[x, y].K - DbeginK)));
                    }
                
                }
            }



            return img2;
        }

        
        public PixelsYUV[,] stretchHistogramYUV(int[] hY, int[] hU, int[] hV, PixelsYUV[,] img, PixelsYUV[,] img2, string context)
        {
            int DdesiredY = 255;
            int DbeginY = 0;
            int DendY = 0;
            int DoriginalY = 0;
            float kY = 0;
           

            int DdesiredU = 255;
            int DbeginU = 0;
            int DendU = 0;
            int DoriginalU = 0;
            float kU = 0;
            

            int DdesiredV = 255;
            int DbeginV = 0;
            int DendV = 0;
            int DoriginalV = 0;
            float kV = 0;
            


            //hy
            //Calculate Dbegin and Dend
            for (int i = 0; i < hY.Length; i++)
            {
                if (hY[i] > 0) { DbeginY = hY[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hY[i] > 0) { DendY = hY[i]; break; }
            }

            DoriginalY = DendY - DbeginY;
            kY = DdesiredY / DoriginalY;


            //hU
            for (int i = 0; i < hU.Length; i++)
            {
                if (hU[i] > 0) { DbeginU = hU[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hU[i] > 0) { DendU = hU[i]; break; }
            }

            DoriginalU = DendU - DbeginU;
            kU = DdesiredU / DoriginalU;



            //hV
            
            for (int i = 0; i < hU.Length; i++)
            {
                if (hV[i] > 0) { DbeginV = hV[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hV[i] > 0) { DendV = hV[i]; break; }
            }

            DoriginalV = DendV - DbeginV;
            kV = DdesiredV / DoriginalV;

            // XY or YX ??
            double result = 0;
            for (int y = 0; y < img.GetLength(1); y++)


            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                    if (context == "YUV")
                    {
                        result = Convert.ToDouble(kY * (img[x, y].Y - DbeginY));
                        img2[x, y].Y = (int)(result);
                        img2[x, y].U = (int)(kU * (img[x, y].U - DbeginU));
                        img2[x, y].V = (int)(kV * (img[x, y].V - DbeginV));
                    }
                  


                }
            }

            return img2;
        }

        public PixelsYUV[,] stretchHistogramYUV(int[] hY, PixelsYUV[,] img, PixelsYUV[,] img2, string context)
        {

            int Ddesired = 255;
            int Dbegin = 0;
            int Dend = 0;
            int Doriginal = 0;
            float k = 0;
            //int Inew = 0;
            //int Iold = img.;



            //Calculate Dbegin and Dend
            for (int i = 0; i < hY.Length; i++)
            {
                if (hY[i] > 0) { Dbegin = hY[i]; break; }
            }
            for (int i = 255; i <= 0; i++)
            {
                if (hY[i] > 0) { Dend = hY[i]; break; }
            }

            Doriginal = Dend - Dbegin;
            k = Ddesired / Doriginal;


            // XY or YX ??
            double result = 0;
            for (int y = 0; y < img.GetLength(1); y++)


            {
                for (int x = 0; x < img.GetLength(0); x++)
                {
                    if (context == "Y1")
                    {
                        result = Convert.ToDouble(k * (img[x, y].Y - Dbegin));
                        img2[x, y].Y = (int)(result);

                    }
                    if (context == "U")
                    {
                        img2[x, y].U = (int)(k * (img[x, y].U - Dbegin));
                    }
                    if (context == "V1")
                    {
                        img2[x, y].V = (int)(k * (img[x, y].V - Dbegin));
                    }
                  

                }
            }

            return img2;


        }




















        // Normalized Histogram(i)=(Total Number of Pixels of Intensity i)/(Total Number of Pixels)
        public PixelsRGB[,] normalizeHistogram()
        {
            return null;
        }

        public PixelsRGB[,] equalizeHistogram()
        {
            return null;
        }

    }
}
