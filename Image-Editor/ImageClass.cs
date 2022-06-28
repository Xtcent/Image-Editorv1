using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Image_editor
{
    public class ImageClass
    {   

        //Multi dimensional arrays of PixelsRGB
        //original image
        public PixelsRGB[,] img1;
        //copy of original
        public PixelsRGB[,] img2;

        //Original image ontouched
        public PixelsRGB[,] ogImg;
        //test
        public PixelsRGB[,] img3;

        public PixelsRGB[,] emptytest;

        //Array of image in HSV system
        public PixelsHSV[,] imghsv;
        public PixelsHSV[,] imghsv2;
        //Array of image in CMYK system
        public PixelsCMYK[,] imgcmyk;
        public PixelsCMYK[,] imgcmyk2;
        //Array of image in YUV system
        public PixelsYUV[,] imgyuv;
        public PixelsYUV[,] imgyuv2;

        //Histogram
        public HistogramClass hst1;
        public HistogramClass hst2;

        public int pixelComponents;
        public int imagewidth=0;
        public int imageheight=0;


        public void ReadImage(Bitmap bmp)
        {
            this.imagewidth = bmp.Width;
            this.imageheight = bmp.Height;
            //Two dimensional arrays with a fixed size of Width and Height
            img1 = new PixelsRGB[bmp.Width, bmp.Height];
            img2 = new PixelsRGB[bmp.Width, bmp.Height];
            img3 = new PixelsRGB[bmp.Width, bmp.Height];
            ogImg = new PixelsRGB[bmp.Width, bmp.Height];


            imghsv = new PixelsHSV[bmp.Width, bmp.Height];
            imghsv2 = new PixelsHSV[bmp.Width, bmp.Height];
            imgcmyk = new PixelsCMYK[bmp.Width, bmp.Height];
            imgcmyk2 = new PixelsCMYK[bmp.Width, bmp.Height];
            imgyuv = new PixelsYUV[bmp.Width, bmp.Height];
            imgyuv2 = new PixelsYUV[bmp.Width, bmp.Height];
            emptytest = new PixelsRGB[bmp.Width, bmp.Height];

            //Create an instance of histogramclass
            hst1 = new HistogramClass();
            hst2 = new HistogramClass();


            //Implicitly declared variable
            // System.Drawing.Imaging.BitmapData

            //Use the LockBits method to lock an existing bitmap in system memory so that
            //it can be changed programmatically. You can change the color of an image with
            //the SetPixel method, although the LockBits method offers 
            //better performance for large-scale changes.

            //public System.Drawing.Imaging.BitmapData LockBits 
            //(System.Drawing.Rectangle rect,
            //System.Drawing.Imaging.ImageLockMode flags, 
            //System.Drawing.Imaging.PixelFormat format);

            //Creates a System.Drawing.Imaging.BitmapData
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            //Just a pointer
            IntPtr ptr = IntPtr.Zero;
           


            if (bmpData.PixelFormat == PixelFormat.Format24bppRgb)
            { //Format24bpprgb has red, green, and blue components
                pixelComponents = 3;
            } else if(bmpData.PixelFormat == PixelFormat.Format32bppRgb)
            { //Format32bpprgb has alpha, red, green, and blue components
                pixelComponents = 4;
            } else
            {
                pixelComponents = 0;
            }

            //Implicit array containing bytes of size (width*pixelcomponents)
            var row = new byte[bmp.Width * pixelComponents];
            //uninitialized


            //Scanning image
            //Loops go through pixels in this order from top left of the image
            // Firstline
            // ---------->
            // Secondline
            // ---------->
            //nLine
            // ---------->
            for(int y = 0; y < bmp.Height; y++)
            {   // Set pointer to data
                ptr = bmpData.Scan0 + y * bmpData.Stride;
                // Copy image to bitmap
                Marshal.Copy(ptr, row, 0, row.Length);
                //populate img1 array with pixel values
                for(int x = 0; x < bmp.Width; x++)
                {                               //r                                    //g                        //b
                    img1[x, y] = new PixelsRGB(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    img2[x, y] = new PixelsRGB(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    img3[x, y] = new PixelsRGB(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    ogImg[x,y] = new PixelsRGB(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);

                    imghsv[x, y] = new PixelsHSV(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    imghsv2[x, y] = new PixelsHSV(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    imgcmyk[x, y] = new PixelsCMYK(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    imgcmyk2[x, y] = new PixelsCMYK(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    imgyuv[x, y] = new PixelsYUV(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    imgyuv2[x, y] = new PixelsYUV(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                }
                
            }
            //Unlock memory
            bmp.UnlockBits(bmpData);
            

            

            //Erase histogram before reading new one
            hst2.eraseHistogram();

            //Fill histogram with data
           hst1.readHistogram(img1,null);
           hst2.readHistogram(img2,null);
           hst2.readHistogram(imghsv);
           hst2.readHistogram(imgyuv);
           hst2.readHistogram(imgcmyk);      
        }

        //New methods, because i can't draw the image with just passing the object type
        public Bitmap DrawImageHSV(PixelsHSV[,] img, string mode)
        {
           // PixelsRGB[,] returnimg = new PixelsRGB[img.GetLength(0), img.GetLength(1)];


            if (img != null)
            {
                // pointer
                IntPtr ptr = IntPtr.Zero;
                //creates new bitmap
                var bmp = new Bitmap(img.GetLength(0), img.GetLength(1), PixelFormat.Format24bppRgb);
                //Creates a System.Drawing.Imaging.BitmapData
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                //Creates new array of Width*3 because there are 3 values for every pixel
                var row = new byte[bmp.Width * 3];


                for (int y = 0; y < img.GetLength(1); y++)
                {
                    for (int x = 0; x < img.GetLength(0); x++)
                    {
                        switch (mode) {
                            case "HSV":
                        row[3 * x + 2] = img[x, y].hsvToRGB(img[x, y].H, img[x, y].S, img[x, y].V).R;
                        row[3 * x + 1] = img[x, y].hsvToRGB(img[x, y].H, img[x, y].S, img[x, y].V).G;
                        row[3 * x] = img[x, y].hsvToRGB(img[x, y].H, img[x, y].S, img[x, y].V).B;
                                break;

                            case "H":
                                row[3 * x + 2] = img[x, y].hsvToRGB(img[x, y].H, 255, 255).R;
                                row[3 * x + 1] = img[x, y].hsvToRGB(img[x, y].H, 255, 255).G;
                                row[3 * x] = img[x, y].hsvToRGB(img[x, y].H, 255, 255).B;
                                break;

                                
                            case "S":
                                row[3 * x + 2] = img[x, y].S;
                                row[3 * x + 1] = img[x, y].S;
                                row[3 * x] = img[x, y].S;
                                break;
                               
                            case "V":
                                row[3 * x + 2] = img[x, y].V;
                                row[3 * x + 1] = img[x, y].V;
                                row[3 * x] = img[x, y].V;
                                break;
                                
                       
                    }
                    }
                    // Set pointer to bmpdata
                    ptr = bmpData.Scan0 + y * bmpData.Stride;
                    //Copy data into new bitmap
                    Marshal.Copy(row, 0, ptr, row.Length);
                }
                //Unlock memory
                bmp.UnlockBits(bmpData);
                return bmp;
            }
            else { return null; }

        }

        public Bitmap DrawImageCMYK(PixelsCMYK[,] img, string mode)
        {
            // PixelsRGB[,] returnimg = new PixelsRGB[img.GetLength(0), img.GetLength(1)];


            if (img != null)
            {
                // pointer
                IntPtr ptr = IntPtr.Zero;
                //creates new bitmap
                var bmp = new Bitmap(img.GetLength(0), img.GetLength(1), PixelFormat.Format24bppRgb);
                //Creates a System.Drawing.Imaging.BitmapData
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                //Creates new array of Width*3 because there are 3 values for every pixel
                var row = new byte[bmp.Width * 3];


                for (int y = 0; y < img.GetLength(1); y++)
                {
                    for (int x = 0; x < img.GetLength(0); x++)
                    {
                        switch (mode)
                        {
                            case "CMYK":
                                row[3 * x + 2] = img[x, y].cmykToRGB(img[x, y].C, img[x, y].M, img[x, y].Y, img[x, y].K).R;
                                row[3 * x + 1] = img[x, y].cmykToRGB(img[x, y].C, img[x, y].M, img[x, y].Y, img[x, y].K).G;
                                row[3 * x] = img[x, y].cmykToRGB(img[x, y].C, img[x, y].M, img[x, y].Y, img[x, y].K).B;


                                break;
                            case "C":
                                row[3 * x + 2] = img[x, y].cmykToRGB(img[x, y].C, 0.0, 0.0, 0.0).R;
                                row[3 * x + 1] = 255;
                                row[3 * x] = 255;
                                break;
                            case "M":
                                row[3 * x + 2] = 255;
                                row[3 * x + 1] = img[x, y].cmykToRGB(0.0, img[x, y].M, 0.0, 0.0).G;
                                row[3 * x] = 255;
                                break;
                            case "Y":
                                row[3 * x + 2] = 255;
                                row[3 * x + 1] = 255;
                                row[3 * x] = img[x, y].cmykToRGB(0.0, 0.0, img[x, y].Y, 0.0).B;
                                break;
                            case "K":
                                row[3 * x + 2] = img[x, y].cmykToRGB(0.0, 0.0, 0.0, img[x, y].K).R;
                                row[3 * x + 1] = img[x, y].cmykToRGB(0.0, 0.0, 0.0, img[x, y].K).G;
                                row[3 * x] = img[x, y].cmykToRGB(0.0, 0.0, 0.0, img[x, y].K).B;
                                break;


                        }
                    }
                    // Set pointer to bmpdata
                    ptr = bmpData.Scan0 + y * bmpData.Stride;
                    //Copy data into new bitmap
                    Marshal.Copy(row, 0, ptr, row.Length);
                }
                //Unlock memory
                bmp.UnlockBits(bmpData);
                return bmp;
            }
            else { return null; }

        }

        public Bitmap DrawImageYUV(PixelsYUV[,] img, string mode)
        {
            // PixelsRGB[,] returnimg = new PixelsRGB[img.GetLength(0), img.GetLength(1)];


            if (img != null)
            {
                // pointer
                IntPtr ptr = IntPtr.Zero;
                //creates new bitmap
                
                var bmp = new Bitmap(img.GetLength(0), img.GetLength(1), PixelFormat.Format24bppRgb);
                //Creates a System.Drawing.Imaging.BitmapData
                //Lock memory
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                //Creates new array of Width*3 because there are 3 values for every pixel
                var row = new byte[bmp.Width * 3];


                for (int y = 0; y < img.GetLength(1); y++)
                {
                    for (int x = 0; x < img.GetLength(0); x++)
                    {
                        switch (mode)
                        {
                            case "YUV":
                                row[3 * x + 2] = img[x, y].yuvToRGB(img[x, y].Y, img[x, y].U, img[x, y].V).R;
                                row[3 * x + 1] = img[x, y].yuvToRGB(img[x, y].Y, img[x, y].U, img[x, y].V).G;
                                row[3 * x] = img[x, y].yuvToRGB(img[x, y].Y, img[x, y].U, img[x, y].V).B;
                                break;
                            case "Y1":
                                row[3 * x + 2] = (byte)img[x, y].Y; //imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, 0, 0).R;
                                row[3 * x + 1] = (byte)img[x, y].Y; //imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, 0, 0).G;
                                row[3 * x] = (byte)img[x, y].Y;    //imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, 0, 0).B;
                                break;

                                //fix this
                            case "U":
                                row[3 * x + 2] = (byte)img[x, y].yuvToRGB(0, img[x, y].U, 0).R; //imgyuv[x, y].U;
                                row[3 * x + 1] = (byte)img[x, y].U;//imgyuv[x, y].yuvToRGB(0, imgyuv[x, y].U, 0).G;  // //
                                row[3 * x] = (byte)img[x, y].U; //imgyuv[x, y].yuvToRGB(255, imgyuv[x, y].U, 255).B; //////
                                break;

                                //fix this
                            case "V1":
                                row[3 * x + 2] = img[x, y].yuvToRGB(0, 0, img[x, y].V).R; //imgyuv[x, y].V;
                                row[3 * x + 1] = img[x, y].yuvToRGB(0, 0, img[x, y].V).G; //imgyuv[x, y].V;
                                row[3 * x] = img[x, y].yuvToRGB(0, 0, img[x, y].V).B; //imgyuv[x, y].V;
                                break;


                        }
                    }
                    // Set pointer to bmpdata
                    ptr = bmpData.Scan0 + y * bmpData.Stride;
                    //Copy data into new bitmap
                    Marshal.Copy(row, 0, ptr, row.Length);
                }
                //Unlock memory
                bmp.UnlockBits(bmpData);
                return bmp;
            }
            else { return null; }
        }


        public Bitmap DrawImage(PixelsRGB[,] img, String mode)
        {

           


            if (img != null) { 
                // pointer
                IntPtr ptr = IntPtr.Zero;
            //creates new bitmap
            var bmp = new Bitmap(img.GetLength(0), img.GetLength(1), PixelFormat.Format24bppRgb);
                //Creates a System.Drawing.Imaging.BitmapData
                //Lock memory
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            //Creates new array of Width*3 because there are 3 values for every pixel
            var row = new byte[bmp.Width * 3];

            for(int y = 0; y< bmp.Height; y++)
            {
                for(int x =0; x < bmp.Width; x++)
                {
                    switch (mode)
                    {
                        case "RGB":
                            {

                                row[3 * x + 2] = img[x, y].R;
                                row[3 * x + 1] = img[x, y].G;
                                row[3 * x] = img[x, y].B;
                                break;

                            }
                        case "R":
                            {
                                row[3 * x + 2] = img[x, y].R;
                                row[3 * x + 1] = 0;
                                row[3 * x] = 0;
                                break;
                            }
                        case "G":
                            {
                                row[3 * x + 2] = 0;
                                row[3 * x + 1] = img[x, y].G;
                                row[3 * x] = 0;
                                break;
                            }
                        case "B":
                            {
                                row[3 * x + 2] = 0;
                                row[3 * x + 1] = 0;
                                row[3 * x] = img[x, y].B;
                                break;
                            }
                        case "I":
                            {
                                row[3 * x + 2] = img[x, y].I;
                                row[3 * x + 1] = img[x, y].I;
                                row[3 * x] = img[x, y].I;
                                break;
                            }
                        case "HSV":
                            {
                                row[3 * x + 2] = img[x, y].hsvToRGB(imghsv[x,y].H, imghsv[x, y].S, imghsv[x, y].V).R;
                                row[3 * x + 1] = img[x, y].hsvToRGB(imghsv[x, y].H, imghsv[x, y].S, imghsv[x, y].V).G;
                                row[3 * x] = img[x, y].hsvToRGB(imghsv[x, y].H, imghsv[x, y].S, imghsv[x, y].V).B;
                                break;
                            }
                        case "H":
                            {
                                row[3 * x + 2] = img[x, y].hsvToRGB(imghsv[x, y].H, 255, 255).R;
                                row[3 * x + 1] = img[x, y].hsvToRGB(imghsv[x, y].H, 255, 255).G;
                                row[3 * x] = img[x, y].hsvToRGB(imghsv[x, y].H, 255, 255).B;
                                break;
                            }
                        case "S":
                            {
                                row[3 * x + 2] = imghsv[x,y].S;
                                row[3 * x + 1] = imghsv[x, y].S;
                                row[3 * x] = imghsv[x, y].S;
                                break;
                            }
                        case "V":
                            {
                                row[3 * x + 2] = imghsv[x, y].V;
                                row[3 * x + 1] = imghsv[x, y].V;
                                row[3 * x] = imghsv[x, y].V;
                                break;
                            }
                        case "CMYK":
                            {
                                row[3 * x + 2] = imgcmyk[x, y].cmykToRGB(imgcmyk[x,y].C , imgcmyk[x,y].M , imgcmyk[x,y].Y , imgcmyk[x,y].K).R;
                                row[3 * x + 1] = imgcmyk[x, y].cmykToRGB(imgcmyk[x, y].C, imgcmyk[x, y].M, imgcmyk[x, y].Y, imgcmyk[x, y].K).G;
                                row[3 * x] = imgcmyk[x, y].cmykToRGB(imgcmyk[x, y].C, imgcmyk[x, y].M, imgcmyk[x, y].Y, imgcmyk[x, y].K).B;
                                    //    System.Diagnostics.Debug.WriteLine("Drawing CMYK");
                                   // System.Diagnostics.Debug.WriteLine("R :" +imgcmyk[x, y].cmykToRGB(imgcmyk[x, y].C, imgcmyk[x, y].M, imgcmyk[x, y].Y, imgcmyk[x, y].K).R);
                                  //  System.Diagnostics.Debug.WriteLine("G :" +imgcmyk[x, y].cmykToRGB(imgcmyk[x, y].C, imgcmyk[x, y].M, imgcmyk[x, y].Y, imgcmyk[x, y].K).G);
                                   // System.Diagnostics.Debug.WriteLine("B :" +imgcmyk[x, y].cmykToRGB(imgcmyk[x, y].C, imgcmyk[x, y].M, imgcmyk[x, y].Y, imgcmyk[x, y].K).B);
                                    break;
                            }
                        case "C":
                            {
                                row[3 * x + 2] = imgcmyk[x, y].cmykToRGB(imgcmyk[x, y].C, 0.0, 0.0, 0.0).R;
                                    row[3 * x + 1] = 255;
                                    row[3 * x] = 255; 
                                break;
                            }
                         case "M":
                            {
                                row[3 * x + 2] = 255;
                                row[3 * x + 1] = imgcmyk[x, y].cmykToRGB(0.0, imgcmyk[x, y].M, 0.0, 0.0).G;
                                row[3 * x] = 255;
                                break;
                            }
                         case "Y":
                            {
                                row[3 * x + 2] = 255;
                                row[3 * x + 1] = 255;
                                row[3 * x] = imgcmyk[x, y].cmykToRGB(0.0, 0.0, imgcmyk[x, y].Y, 0.0).B;
                                break;
                            }
                         case "K":
                            {
                                row[3 * x + 2] = imgcmyk[x, y].cmykToRGB(0.0, 0.0, 0.0, imgcmyk[x, y].K).R;
                                row[3 * x + 1] = imgcmyk[x, y].cmykToRGB(0.0, 0.0, 0.0, imgcmyk[x, y].K).G;
                                row[3 * x] = imgcmyk[x, y].cmykToRGB(0.0, 0.0, 0.0, imgcmyk[x, y].K).B;
                                break;
                            }
                        case "YUV":
                            {   //Sometimes has artifacts on images...
                                //Removed artifacts by storing y u v as integers...

                                row[3 * x + 2] = imgyuv[x,y].yuvToRGB(imgyuv[x,y].Y,imgyuv[x,y].U,imgyuv[x,y].V).R;
                                row[3 * x + 1] = imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, imgyuv[x, y].U, imgyuv[x, y].V).G;
                                row[3 * x] = imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, imgyuv[x, y].U, imgyuv[x, y].V).B;
                                break;
                            }
                        case "Y1":
                            {
                                    //Idk why it works
                                row[3 * x + 2] = (byte)imgyuv[x, y].Y; //imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, 0, 0).R;
                                row[3 * x + 1] = (byte)imgyuv[x, y].Y; //imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, 0, 0).G;
                                row[3 * x] = (byte)imgyuv[x, y].Y;    //imgyuv[x, y].yuvToRGB(imgyuv[x, y].Y, 0, 0).B;
                                break;
                            }
                        case "U":

                                {   //Es nezinu kā īsti pareizi attēlot U un V kānālus RGB krāsu sistēmā
                                    //Gribu piebilst ka demonstrācijas programma arī tas nebija pareizi izdarīts
                                    //Esmu daudz ko mēģinājis, bet nekādīgi nesanāca
                                    //Idk need to fix it
                                    row[3 * x + 2] = (byte)imgyuv[x, y].yuvToRGB(0, imgyuv[x, y].U, 0).R; //imgyuv[x, y].U;
                                row[3 * x + 1] = (byte)imgyuv[x, y].U;//imgyuv[x, y].yuvToRGB(0, imgyuv[x, y].U, 0).G;  // //
                                    row[3 * x] = (byte)imgyuv[x, y].U; //imgyuv[x, y].yuvToRGB(255, imgyuv[x, y].U, 255).B; //////
                                    break;
                            }
                        case "V1":
                            {       //idk need to fix it
                                    row[3 * x + 2] = imgyuv[x, y].yuvToRGB(0, 0, imgyuv[x, y].V).R; //imgyuv[x, y].V;
                                row[3 * x + 1] =  imgyuv[x, y].yuvToRGB(0, 0, imgyuv[x, y].V).G; //imgyuv[x, y].V;
                                row[3 * x] = imgyuv[x, y].yuvToRGB(0, 0, imgyuv[x, y].V).B; //imgyuv[x, y].V;
                                    break;
                            }
                    }


                    /*
                    row[3 * x + 2] = img[x, y].R;
                    row[3 * x + 1] = img[x, y].G;
                    row[3 * x] = img[x, y].B;
                    */
                }
                    // Set pointer to bmpdata
                    ptr = bmpData.Scan0 + y * bmpData.Stride;
                    //Copy data into new bitmap
                    Marshal.Copy(row, 0, ptr, row.Length);
                
               
            }
                //Unlock memory
                bmp.UnlockBits(bmpData);

                

                return bmp;
            }
            else { return null; }
        }

        //Custom GetPixel
        public Color GetPixel(int x, int y)
        {
            Color clr = Color.Empty;
            //Sets pixel values according to pixel format
            //PixelFormat24bppRGB
            if (pixelComponents == 3)
            { 
                PixelsRGB one = img1[x, y];
                byte r = one.R;
                byte g = one.G;
                byte b = one.B;
                clr = Color.FromArgb( r, g, b);
            }
            ////PixelFormat32bppRGB
            else if (pixelComponents == 4)
            {
                PixelsRGB one = img1[x, y];
                byte r = one.R;
                byte g = one.G;
                byte b = one.B;
                byte a = one.I;
                clr = Color.FromArgb(a, r, g, b);
            }
                return clr;
        }


        //Custom SetPixel
        public void SetPixel(int x, int y, Color color)
        {

            //PixelFormat24bppRGB
            if (pixelComponents == 3)
            {
                img2[x, y] = new PixelsRGB(color.R, color.G, color.B);


            }//PixelFormat32bppRGB
            else if (pixelComponents == 4)
            {

                img2[x, y] = new PixelsRGB(color.R, color.G, color.B);

            }



        }

        //Update method for updating images with new input image
        //Input image -img
        public void updateImages(PixelsRGB[,] imgupdate)
        {
            for (int y = 0; y < imgupdate.GetLength(1); y++)
            {
                for (int x = 0; x < imgupdate.GetLength(0); x++)
                {
                    imghsv[x, y] = new PixelsHSV(imgupdate[x, y].R, imgupdate[x, y].G, imgupdate[x, y].B);
                    imgcmyk[x, y] = new PixelsCMYK(imgupdate[x, y].R, imgupdate[x, y].G, imgupdate[x, y].B);
                    imgyuv[x, y] = new PixelsYUV(imgupdate[x, y].R, imgupdate[x, y].G, imgupdate[x, y].B);

                }


            }
        }



        public void filterImage(string strfilter)
        {
            //3x3 Low frequency filters
            int[,] filter = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int[,] filter2 = new int[,] { { 1, 1, 1 }, { 1, 10, 1 }, { 1, 1, 1 } };
            int[,] filter3 = new int[,] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
            //3x3 High frequency filters
            int[,] filterhq = new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            int[,] filterhq2 = new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
            int[,] filterhq3 = new int[,] { { -2, -2, -2 }, { -2, 17, -2 }, { -2, -2, -2 } };

            int k = 0;

            
           

            //3x3
            for (int fi = 0; fi < 3; fi++)
            {
                for (int fj = 0; fj < 3; fj++)
                {
                    switch (strfilter) {

                        case "filter1":
                        k += filter[fi, fj];
                            
                            break;

                        case "filter2":
                            k += filter2[fi, fj];
                            break;

                        case "filter3":
                            k += filter3[fi, fj];
                            break;
                        case "filterhq":
                            k += filterhq[fi, fj];
                            break;
                        case "filterhq2":
                            k += filterhq2[fi, fj];
                            break;
                        case "filterhq3":
                            k += filterhq3[fi, fj];
                            break;
                    }
                }
            }
            
            //Apply filter to image and get new data
            for (int x =1; x < img1.GetLength(0) - 1; x++)
            {
                for(int y=1; y< img1.GetLength(1) - 1; y++)
                {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int i = 0;

                    for (int fi = 0; fi < 3; fi++)
                    {
                        for (int fj = 0; fj < 3; fj++)
                        {
                            switch (strfilter) {
                                case "filter1":
                                // Take only values from original image
                                r += img1[x + fi - 1, y + fj - 1].R * filter[fi, fj];
                                g += img1[x + fi - 1, y + fj - 1].G * filter[fi, fj];
                                b += img1[x + fi - 1, y + fj - 1].B * filter[fi, fj];
                                i += img1[x + fi - 1, y + fj - 1].I * filter[fi, fj];
                                break;
                                case "filter2":
                                    r += img1[x + fi - 1, y + fj - 1].R * filter2[fi, fj];
                                    g += img1[x + fi - 1, y + fj - 1].G * filter2[fi, fj];
                                    b += img1[x + fi - 1, y + fj - 1].B * filter2[fi, fj];
                                    i += img1[x + fi - 1, y + fj - 1].I * filter2[fi, fj];
                                    break;
                                case "filter3":
                                    r += img1[x + fi - 1, y + fj - 1].R * filter3[fi, fj];
                                    
                                    g += img1[x + fi - 1, y + fj - 1].G * filter3[fi, fj];
                                  
                                    b += img1[x + fi - 1, y + fj - 1].B * filter3[fi, fj];
                                    
                                    i += img1[x + fi - 1, y + fj - 1].I * filter3[fi, fj];
                                   
                                    break;
                                case "filterhq":
                                    r += img1[x + fi - 1, y + fj - 1].R * filterhq[fi, fj];
                                   
                                    g += img1[x + fi - 1, y + fj - 1].G * filterhq[fi, fj];
                                    
                                    b += img1[x + fi - 1, y + fj - 1].B * filterhq[fi, fj];
                                  
                                    i += img1[x + fi - 1, y + fj - 1].I * filterhq[fi, fj];
                                   
                                    break;
                                case "filterhq2":
                                    r += img1[x + fi - 1, y + fj - 1].R * filterhq2[fi, fj];
                                    g += img1[x + fi - 1, y + fj - 1].G * filterhq2[fi, fj];
                                    b += img1[x + fi - 1, y + fj - 1].B * filterhq2[fi, fj];
                                    i += img1[x + fi - 1, y + fj - 1].I * filterhq2[fi, fj];
                                    break;
                                case "filterhq3":
                                    r += img1[x + fi - 1, y + fj - 1].R * filterhq3[fi, fj];
                                    g += img1[x + fi - 1, y + fj - 1].G * filterhq3[fi, fj];
                                    b += img1[x + fi - 1, y + fj - 1].B * filterhq3[fi, fj];
                                    i += img1[x + fi - 1, y + fj - 1].I * filterhq3[fi, fj];
                                    break;



                            }
                            


                        }


                        }
                    

                    r = Math.Max(0, Math.Min(255, r /= k));
                 
                    g = Math.Max(0, Math.Min(255, g /= k));
                  
                    b = Math.Max(0, Math.Min(255, b /= k));
                
                    i = Math.Max(0, Math.Min(255, i /= k));
                  

                    //changed array
                    img2[x, y].R = (byte)r;
                    img2[x, y].G = (byte)g;
                    img2[x, y].B = (byte)b;
                    img2[x, y].I = (byte)i;
                   

                }
            }
            //Update image here
            updateImages(img2);
            hst2.eraseHistogram();
            hst2.readHistogram(img2,null);
            hst2.readHistogram(imghsv);
          
            hst2.readHistogram(imgyuv);
            hst2.readHistogram(imgcmyk);



        }


        //Median filter method 3x3
        public void filterImageMedian3x3()
        {
            //Loop through the image
            for (int x = 1; x < img1.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < img1.GetLength(1) - 1; y++)
                {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int i = 0;
                    //Doesn't work if i set it to 10?
                    //Create arrays to store new median values
                    int[] mdarrayR = new int[10];
                    int[] mdarrayG = new int[10];
                    int[] mdarrayB = new int[10];
                    int[] mdarrayI = new int[10];
                    int count = 0;

                    for (int fi = 0; fi < 3; fi++)
                    {
                        for (int fj = 0; fj < 3; fj++)
                        {
                            //Add image data to median arrays
                            mdarrayR[count] = img1[x + fi - 1, y + fj - 1].R;
                            mdarrayG[count] = img1[x + fi - 1, y + fj - 1].G;
                            mdarrayB[count] = img1[x + fi - 1, y + fj - 1].B;
                            mdarrayI[count] = img1[x + fi - 1, y + fj - 1].I;
                            count++;
                        }
                    }
                    //Calculate median
                    r = Math.Max(0, Math.Min(255, getMedian(mdarrayR)));

                    g = Math.Max(0, Math.Min(255, getMedian(mdarrayG)));

                    b = Math.Max(0, Math.Min(255, getMedian(mdarrayB)));

                    i = Math.Max(0, Math.Min(255, getMedian(mdarrayI)));


                    //changed array
                    img2[x, y].R = (byte)r;
                    img2[x, y].G = (byte)g;
                    img2[x, y].B = (byte)b;
                    img2[x, y].I = (byte)i;


                }
            }


            //Update image here
            updateImages(img2);
            hst2.eraseHistogram();
            hst2.readHistogram(img2, null);
            hst2.readHistogram(imghsv);
                
            hst2.readHistogram(imgyuv);
            hst2.readHistogram(imgcmyk);


        }

        //Median filter method 5x5
        public void filterImageMedian5x5()
        {
            //Loop through the image
            //Loop length changed for 5x5 array
            for (int x = 2; x < img1.GetLength(0) - 2; x++)
            {
                for (int y = 2; y < img1.GetLength(1) - 2; y++)
                {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int i = 0;
                    //Doesn't work if i set it to 10?
                    //Create arrays to store new median values
                    int[] mdarrayR = new int[25];
                    int[] mdarrayG = new int[25];
                    int[] mdarrayB = new int[25];
                    int[] mdarrayI = new int[25];
                    int count = 0;

                    for (int fi = 0; fi < 5; fi++)
                    {
                        for (int fj = 0; fj < 5; fj++)
                        {
                            //Add image data to median arrays
                            mdarrayR[count] = img1[x + fi - 2, y + fj - 2].R;
                            mdarrayG[count] = img1[x + fi - 2, y + fj - 2].G;
                            mdarrayB[count] = img1[x + fi - 2, y + fj - 2].B;
                            mdarrayI[count] = img1[x + fi - 2, y + fj - 2].I;
                            count++;
                        }
                    }
                    //Calculate median
                    r = Math.Max(0, Math.Min(255, getMedian(mdarrayR)));

                    g = Math.Max(0, Math.Min(255, getMedian(mdarrayG)));

                    b = Math.Max(0, Math.Min(255, getMedian(mdarrayB)));

                    i = Math.Max(0, Math.Min(255, getMedian(mdarrayI)));


                    //changed array
                    img2[x, y].R = (byte)r;
                    img2[x, y].G = (byte)g;
                    img2[x, y].B = (byte)b;
                    img2[x, y].I = (byte)i;


                }
            }


            //Update image here
            updateImages(img2);
            hst2.eraseHistogram();
            hst2.readHistogram(img2, null);
            hst2.readHistogram(imghsv);
            hst2.readHistogram(imgyuv);
            hst2.readHistogram(imgcmyk);


        }

        //Median filter method 7x7
        public void filterImageMedian7x7()
        {
            //Loop through the image
            //Loop length changed for 7x7 array
            
            for (int x = 3; x < img1.GetLength(0) - 3; x++)
            {
                for (int y = 3; y < img1.GetLength(1) - 3; y++)
                {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int i = 0;
                    //Doesn't work if i set it to 10?
                    //Create arrays to store new median values
                    int[] mdarrayR = new int[49];
                    int[] mdarrayG = new int[49];
                    int[] mdarrayB = new int[49];
                    int[] mdarrayI = new int[49];
                    int count = 0;

                    for (int fi = 0; fi < 7; fi++)
                    {
                        for (int fj = 0; fj < 7; fj++)
                        {
                            //Add image data to median arrays
                            mdarrayR[count] = img1[x + fi - 3, y + fj - 3].R;
                            mdarrayG[count] = img1[x + fi - 3, y + fj - 3].G;
                            mdarrayB[count] = img1[x + fi - 3, y + fj - 3].B;
                            mdarrayI[count] = img1[x + fi - 3, y + fj - 3].I;
                            count++;
                        }
                    }

                    //Calculate median
                    r = Math.Max(0, Math.Min(255, getMedian(mdarrayR)));

                    g = Math.Max(0, Math.Min(255, getMedian(mdarrayG)));

                    b = Math.Max(0, Math.Min(255, getMedian(mdarrayB)));

                    i = Math.Max(0, Math.Min(255, getMedian(mdarrayI)));


                    //changed array
                    img2[x, y].R = (byte)r;
                    img2[x, y].G = (byte)g;
                    img2[x, y].B = (byte)b;
                    img2[x, y].I = (byte)i;


                }
            }


            //Update image here
            updateImages(img2);
            hst2.eraseHistogram();
            hst2.readHistogram(img2, null);
            hst2.readHistogram(imghsv);
            
             hst2.readHistogram(imgyuv);
            hst2.readHistogram(imgcmyk);


        }



        
        //Method to calculate median value from an array input
        //Input -- int[] arr
        public int getMedian(int[] mdarray)
        {
            
            int n = mdarray.GetLength(0);
            int median;
            int temp;

            temp = 0;
            //sort the array
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (mdarray[j] < mdarray[j + 1])
                    {
                        temp = mdarray[j];
                        mdarray[j] = mdarray[j + 1];
                        mdarray[j + 1] = temp;
                    }
                }
            }

            //calculate median
            if (n % 2 == 0)
            {
                median = Convert.ToInt32((mdarray[(n / 2) - 1] + mdarray[(n / 2)]) / 2.0F);
                return median;
            }
            else
            {
                median = mdarray[(n / 2)];
                return median;
            }
            
        }


        





        /*
        
       //Takes in the original image and modifies its contrast and returns
       //the modified image.
       public PixelsRGB[,] stretchHistogram(int[] a, PixelsRGB[,] img1)
       {
           if (img1==null)
           {
               System.Diagnostics.Debug.WriteLine("HistogramClass, stretchHistogram, NullObject img");
           }
           else { System.Diagnostics.Debug.WriteLine("HistogramClass, stretchHistogram, img is not null"); }

           int Ddesired = 255;
           int Dbegin = 0;
           int Dend = 0;
           int Doriginal = 0;
           int k = 0;
           //int Inew = 0;
           //int Iold = img.;
           PixelsRGB[,] img2 = new PixelsRGB[img1.GetLength(0), img1.GetLength(1)];


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

           for (int y=0; y < img1.GetLength(1); y++)
           {
               for (int x=0; x < img1.GetLength(0); x++)
               {
                   int ff = (img1[x, y].I - Dbegin);
                   img2[x, y].I =(byte) (k * ff);
               }
           }

           return img2;
       }
       */







    }
}
