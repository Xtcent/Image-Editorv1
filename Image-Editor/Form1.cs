using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_editor

{
    public partial class Form1 : Form
    {

        ImageClass imgcls = new ImageClass();
        //public ImageClass imgClass = new ImageClass();
        public Form1()
        {
            InitializeComponent();
            


        }
        //Global vars for current X and Y coordinates
        public int CurrentX = 0;
        public int CurrentY = 0;
       // public Boolean SetColor = false;
        public Color clrDialoguecolor;
       // public Boolean SetRed = false;
        public Color ColorToDraw;
        //Button logic bools
        public bool ContrastStretch = false;
        public bool ContrastNormalize = false;
        public bool ContrastEqualize = false;

        private void PictureBox1_Click(object sender, EventArgs e)
        {
           

        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //radioButton1.Checked = true;
                this.pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                Bitmap bmp = (Bitmap)pictureBox1.Image.Clone();
                //reading image with Readimage() method
                imgcls.ReadImage(bmp);
                //draws image in picturebox2
                pictureBox2.Image = imgcls.DrawImage(imgcls.img1,"RGB");
                
                //Draws chart1 histogram
                imgcls.hst1.drawHistogram(chart1);
                //Draws default init histogram for chart2
                
                //draw histogram
                imgcls.hst2.drawHistogram(chart2);
                RGB.Checked = true;

            }
        }

        //Method for converting coordinates.
        private Point ConvertXY(int x, int y, PictureBox pb)
        {
            Point p = new Point();
            //Image dimensions
            int imgWidth = pb.Image.Width;
            int imgHeight = pb.Image.Height;
            int boxWidth = pb.Width;
            int boxHeight = pb.Height;

            //Calculate ratio
            double kx = (double)imgWidth / boxWidth;
            double ky = (double)imgHeight / boxHeight;

            double k = Math.Max(kx, ky);
            //Calculate coordinate offset
            double nobidex = (boxWidth * k - imgWidth) / 2f;
            double nobidey = (boxHeight * k - imgHeight) / 2f;
            //Calculate coordinates
            p.X = Convert.ToInt32(Math.Max(Math.Min(Math.Round(x * k - nobidex), imgWidth - 1), 0));
            p.Y = Convert.ToInt32(Math.Max(Math.Min(Math.Round(y * k - nobidey), imgHeight - 1), 0));

            return p;

        }









        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void InvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int r, g, b;
                //Looping through the image data
                for (int i = 0; i < pictureBox2.Image.Width; i++)
                {
                    for (int j = 0; j < pictureBox2.Image.Height; j++)
                    {
                        // Gets the existing pixels and modifies them.
                        r = 255 - imgcls.GetPixel(i, j).R;
                        g = 255 - imgcls.GetPixel(i, j).G;
                        b = 255 - imgcls.GetPixel(i, j).B;
                        // Sets the specified pixels to a specified color
                        imgcls.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }
              //  pictureBox1.Image = imgClass.DrawImage(imgClass.img1);
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2,"RGB");
               // radioButton1.Checked = true;
                pictureBox2.Refresh();
            }
        }
     

    private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Point p = new Point();
                p = ConvertXY(e.X, e.Y, pictureBox1);

                //Fetch X and Y coordinates for use in other methods.
                CurrentX = p.X;
                CurrentY = p.Y;


                label1.Text = "x, y : " + Convert.ToString(p.X) + ", " + Convert.ToString(p.Y);
               //Display color
                byte r = imgcls.GetPixel(p.X, p.Y).R;
                byte g = imgcls.GetPixel(p.X, p.Y).G;
                byte b = imgcls.GetPixel(p.X, p.Y).B;
                //Set color for picturebox draw from current pixel we are hovering over
                ColorToDraw = imgcls.GetPixel(p.X, p.Y);


                //Create new picturebox do display color in 
                //and draw the color of current pixel
                Bitmap bmp = new Bitmap(pictureBox3.Width, pictureBox3.Height);
                using (Graphics graph = Graphics.FromImage(bmp))
                {
                    SolidBrush myBrush = new SolidBrush(ColorToDraw);
                    Rectangle ImageSize = new Rectangle(0, 0, pictureBox3.Width, pictureBox3.Height);
                    graph.FillRectangle(myBrush, ImageSize);
                }
                pictureBox3.Image = bmp;




                //Display color in Hex
                label2.Text = "Color: #"+r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
                //Display image R G B values for current pixel
                label3.Text = "RGB: R:"+imgcls.GetPixel(p.X, p.Y).R+" G:"+imgcls.GetPixel(p.X, p.Y).G+" B:"+ imgcls.GetPixel(p.X, p.Y).B;
                //Display H S V values for current pixel
                label4.Text = "HSV: H:"+imgcls.imghsv[CurrentX, CurrentY].H+ " S:"+ imgcls.imghsv[CurrentX, CurrentY].S+" V:"+ imgcls.imghsv[CurrentX, CurrentY].V;
                //Display C M Y K values for current pixel
                label5.Text = "CMYK: C:" + string.Format("{0:0 %}", imgcls.imgcmyk[CurrentX, CurrentY].C) + " M:" + string.Format("{0:0 %}", imgcls.imgcmyk[CurrentX, CurrentY].M) + " Y:" + string.Format("{0:0 %}", imgcls.imgcmyk[CurrentX, CurrentY].Y) + " K:" + string.Format("{0:0 %}", imgcls.imgcmyk[CurrentX, CurrentY].K);
                //Display Y U V values for current pixel
                label6.Text = "YUV: Y:" + imgcls.imgyuv[CurrentX, CurrentY].Y + " U:" + imgcls.imgyuv[CurrentX, CurrentY].U + " V:" + imgcls.imgyuv[CurrentX, CurrentY].V;


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {

        }


        //Get set and store the currently selected radio button
        protected string context;
       
        public string getrdbContext()
        {
            return this.context;
        }

        public void setrdbContext(string ct)
        {
            this.context = ct;
        }



        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdbtn = (RadioButton)sender;
            //Radio button logic for color system display
            //Radio button logic for histogram display

            string[] senderText = new string[5];
            senderText[0] = rdbtn.Text;
            setrdbContext(rdbtn.Text);
            //TODO check this
            if (sender.Equals(radioButton17))
            {
                senderText[0] = "V1";
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, "V1");
                setrdbContext("V1");
            }
            else if (sender.Equals(radioButton15))
            {
                senderText[0] = "Y1";
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, "Y1");
                setrdbContext("Y1");
            }
            else
            {
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, rdbtn.Text);
            }
            
            //Check if Image array is initialized
            if (imgcls.img1 != null)
            {
                //Draw histogram of type specified by radiobutton
               
                    imgcls.hst2.drawSpecificHistogram(chart2, senderText);
                  

                
            }

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set default filename
            saveFileDialog1.FileName = "default.jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {   
                //Set user selected filename when saving image
                string path = saveFileDialog1.FileName;
                //Save image as jpeg
                pictureBox2.Image.Save(path , ImageFormat.Jpeg);
            }
        }

        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RadioButton11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void RadioButton17_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Chart1_Click(object sender, EventArgs e)
        {

        }


        //Normalize
        //Enhance Contrast Normalize radio button
        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ContrastNormalize = true;
            ContrastStretch = false;
            ContrastEqualize = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {   //Takes the original image and modifies its contrast 
            //For each color channel

            //Temp variables to pass on methods
            PixelsRGB[,] imgTemp = imgcls.emptytest;
            PixelsHSV[,] imgTemphsv = imgcls.imghsv;
            PixelsCMYK[,] imgTempcmyk = imgcls.imgcmyk;
            PixelsYUV[,] imgTempyuv = imgcls.imgyuv;
            if (ContrastStretch){
                string[] ddtemp = new string[2];
                ddtemp[0] = getrdbContext();
                switch (getrdbContext())
                {


                    /// RGBI
                    case "RGB":

                                                                    //R               //G           //B                //I
                        imgTemp = imgcls.hst2.stretchHistogram(imgcls.hst2.hR, imgcls.hst2.hG, imgcls.hst2.hB,imgcls.hst2.hI, imgcls.img1, imgcls.img2);

                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemp, ddtemp[0]);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImage(imgTemp,getrdbContext());
                        

                        break;
                    case "R":
                        imgTemp = imgcls.hst2.stretchHistogram(imgcls.hst2.hR, imgcls.img1, imgcls.img2,getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemp, ddtemp[0]);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);

                        pictureBox2.Image = imgcls.DrawImage(imgTemp, "R"/*getrdbContext()*/);
                        break;
                    case "G":

                        imgTemp = imgcls.hst2.stretchHistogram(imgcls.hst2.hG, imgcls.img1, imgcls.img2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemp, ddtemp[0]);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImage(imgTemp, getrdbContext());

                        break;
                    case "B":
                        imgTemp = imgcls.hst2.stretchHistogram(imgcls.hst2.hB, imgcls.img1, imgcls.img2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemp, ddtemp[0]);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImage(imgTemp, getrdbContext());


                        break;
                    case "I":
                        imgTemp = imgcls.hst2.stretchHistogram(imgcls.hst2.hI, imgcls.img1, imgcls.img2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemp, ddtemp[0]);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImage(imgTemp, getrdbContext());
                        break;





                        //HSV
                    case "HSV":
                        imgTemphsv = imgcls.hst2.stretchHistogramHSV(imgcls.hst2.hH, imgcls.hst2.hS, imgcls.hst2.hV, imgcls.imghsv, imgcls.imghsv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemphsv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);

                        pictureBox2.Image = imgcls.DrawImageHSV(imgTemphsv, getrdbContext());
                        break;



                    case "H":
                        
                        imgTemphsv = imgcls.hst2.stretchHistogramHSV(imgcls.hst2.hH, imgcls.imghsv, imgcls.imghsv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemphsv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        
                        pictureBox2.Image = imgcls.DrawImageHSV(imgTemphsv, getrdbContext());
                        
                        break;
                        
                    case "S":
                        
                        imgTemphsv = imgcls.hst2.stretchHistogramHSV(imgcls.hst2.hS, imgcls.imghsv, imgcls.imghsv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemphsv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageHSV(imgTemphsv, getrdbContext());
                         
                        break;
                       
                    case "V":
                        
                        imgTemphsv = imgcls.hst2.stretchHistogramHSV(imgcls.hst2.hI, imgcls.imghsv, imgcls.imghsv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTemphsv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageHSV(imgTemphsv, getrdbContext());
                        
                        break;
                        




                        //CMYK
                    case "CMYK":

                        
                        imgTempcmyk= imgcls.hst2.stretchHistogramCMYK(imgcls.hst2.hC, imgcls.hst2.hM, imgcls.hst2.hY, imgcls.hst2.hK, imgcls.imgcmyk, imgcls.imgcmyk, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempcmyk);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageCMYK(imgTempcmyk, getrdbContext());
                        

                        break;


                    case "C":
                        imgTempcmyk = imgcls.hst2.stretchHistogramCMYK(imgcls.hst2.hC, imgcls.imgcmyk, imgcls.imgcmyk, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempcmyk);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageCMYK(imgTempcmyk, getrdbContext());


                        break;
                    case "M":
                        imgTempcmyk = imgcls.hst2.stretchHistogramCMYK(imgcls.hst2.hM, imgcls.imgcmyk, imgcls.imgcmyk, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempcmyk);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageCMYK(imgTempcmyk, getrdbContext());
                        break;
                    case "Y":
                        imgTempcmyk = imgcls.hst2.stretchHistogramCMYK(imgcls.hst2.hY, imgcls.imgcmyk, imgcls.imgcmyk, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempcmyk);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageCMYK(imgTempcmyk, getrdbContext());
                        break;
                    case "K":
                        imgTempcmyk = imgcls.hst2.stretchHistogramCMYK(imgcls.hst2.hK, imgcls.imgcmyk, imgcls.imgcmyk, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempcmyk);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageCMYK(imgTempcmyk, getrdbContext());
                        break;




                        //YUV
                    case "YUV":
                        imgTempyuv = imgcls.hst2.stretchHistogramYUV(imgcls.hst2.hY1, imgcls.hst2.hU, imgcls.hst2.hV1, imgcls.imgyuv, imgcls.imgyuv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempyuv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageYUV(imgTempyuv, getrdbContext());
                        break;

                    case "Y1":
                        imgTempyuv = imgcls.hst2.stretchHistogramYUV(imgcls.hst2.hY1, imgcls.imgyuv, imgcls.imgyuv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempyuv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageYUV(imgTempyuv, getrdbContext());
                        break;
                    case "U":
                        imgTempyuv = imgcls.hst2.stretchHistogramYUV(imgcls.hst2.hU, imgcls.imgyuv, imgcls.imgyuv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempyuv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageYUV(imgTempyuv, getrdbContext());
                        break;
                    case "V1":
                        imgTempyuv = imgcls.hst2.stretchHistogramYUV(imgcls.hst2.hV, imgcls.imgyuv, imgcls.imgyuv2, getrdbContext());
                        imgcls.hst2.eraseHistogram();
                        imgcls.hst2.readHistogram(imgTempyuv);
                        imgcls.hst2.drawSpecificHistogram(chart2, ddtemp);
                        pictureBox2.Image = imgcls.DrawImageYUV(imgTempyuv, getrdbContext());
                        break;
                }
                
                //Draws the modified images to picturebox2
                //imgcls.hst2.eraseHistogram();
                // imgcls.hst2.readHistogram(imgTemp);
                


            }
            else if (ContrastNormalize){

                switch (getrdbContext())
                {
                    case "RGB":
                        break;
                    case "R":
                        break;
                    case "G":
                        break;
                    case "B":
                        break;
                    case "I":
                        break;

                    case "H":
                        break;
                    case "S":
                        break;
                    case "V":
                        break;

                    case "C":
                        break;
                    case "M":
                        break;
                    case "Y":
                        break;
                    case "K":
                        break;

                    case "Y1":
                        break;
                    case "U":
                        break;
                    case "V1":
                        break;
                }



            }
            else if (ContrastEqualize) {

                switch (getrdbContext())
                {
                    case "RGB":
                        break;
                    case "R":
                        break;
                    case "G":
                        break;
                    case "B":
                        break;
                    case "I":
                        break;

                    case "H":
                        break;
                    case "S":
                        break;
                    case "V":
                        break;

                    case "C":
                        break;
                    case "M":
                        break;
                    case "Y":
                        break;
                    case "K":
                        break;

                    case "Y1":
                        break;
                    case "U":
                        break;
                    case "V1":
                        break;
                }




            }
        }

        //Stretch
        //Enhance Contrast Stretch radio button
        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ContrastStretch = true;
            ContrastEqualize = false;
            ContrastNormalize = false;
        }

        //Equalize
        //Enhance Contrast Equalize radio button
        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ContrastStretch = false;
            ContrastEqualize = true;
            ContrastNormalize = false;
        }


        //Blur filter button logic
        //Filter1 3x3
        private void Blur1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImage("filter1");

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Blur 1";
            }
        }
        //Blur filter button logic
        //Filter2 3x3
        private void Blur2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImage("filter2");

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Blur 2";
            }
        }
        //Blur filter button logic
        //Filter3 3x3
        private void Blur3ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (imgcls.img2 != null)
            {
                imgcls.filterImage("filter3");

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Blur 3";
            }
        }
        //Sharpen filter button logic
        //Filterhq1 3x3
        private void Sharpen1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImage("filterhq");

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Sharpen 1";
            }
        }
        //Sharpen filter button logic
        //Filterhq2 3x3
        private void Sharpen2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImage("filterhq2");

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Sharpen 2";
            }
        }

        //Sharpen filter button logic
        //Filterhq3 3x3
        private void Sharpen3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImage("filterhq3");

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Sharpen 3";
            }
        }

        //Median filter button logic
        //3x3
        private void X3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImageMedian3x3();

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Median 3x3";
            }
        }

        //Median filter button logic
        //5x5
        private void X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImageMedian5x5();

                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Median 5x5";
            }
        }

        //Median filter button logic
        //7x7
        private void X7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgcls.img2 != null)
            {
                imgcls.filterImageMedian7x7();
                
                //redraw histogram
                string[] temp = new string[2];
                temp[0] = getrdbContext();
                pictureBox2.Image = imgcls.DrawImage(imgcls.img2, getrdbContext());
                imgcls.hst2.drawSpecificHistogram(chart2, temp);
                label7.Text = "Current filter: Median 7x7";
            }
        }
    }
}
