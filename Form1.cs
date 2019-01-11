using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
namespace YuvCozucu
{
    public partial class Form1 : Form
    {
        Form form = new Form();
        public ListBox listBoxControl1 = new ListBox();



        private List<Bitmap> bmpFrames = new List<Bitmap>();
        private int width;
        private int height;
        private double frameCount;
        private double katsayi;
        private double frameSize;
        private int sayac = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            comboBox1.SelectedIndex = 2;
            widthUi.Text = "176";
            heightUi.Text = "144";
            */
            
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) // form 2 kapatılırsa form 1 i tekrar acılması icin
        {
            this.Show();
        }


        private void button1_Click(object sender, EventArgs e)//Dosya Okuma işlemi
        {
            
            Stopwatch watch = new Stopwatch();
            watch.Start();


            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "C:/Users/kaank/Source/Repos/YuvCozucu/";
            file.Filter = "Yuv Dosyası |*.yuv| Yuv Dosyası|*.yuv";
            file.FilterIndex = 2;
            file.Title = "Yuv Dosyası Seçiniz..";
            file.ShowDialog();
            try
            {
                width = int.Parse(widthUi.Text);
                height = int.Parse(heightUi.Text);
            
                byte[] fileBytes = File.ReadAllBytes(file.FileName);


                List<int> yuv = new List<int>();

                foreach (byte b in fileBytes)
                {
                    yuv.Add(Convert.ToInt32(b));
                }



                frameSize = width * height;

                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        katsayi = 3;
                        frameCount = yuv.Count / (3 * frameSize);
                        break;
                    case 1:
                        katsayi = 2;
                        frameCount = yuv.Count / (2 * frameSize);
                        break;
                    case 2:
                        katsayi = 1.5;
                        frameCount = yuv.Count / (1.5 * frameSize);
                        break;
                    default:
                        MessageBox.Show("format secilmedi");
                        break;
                }

                trackBar1.Minimum = 0;
                trackBar1.Maximum = Convert.ToInt32(frameCount) - 1;
                bmpFrames = yuvToBMP(yuv, width, height);

                Console.WriteLine("bmpFrames oluşturuldu.");
                MessageBox.Show("bmpframes oluşturuldu!!!!!");
            }


            catch
            {

            }


            watch.Stop();
          
            //listBoxControl1.Items.Add(watch.Elapsed.Milliseconds);
            Console.WriteLine("su kadar milisaniyede  calıstı :{0}",watch.Elapsed.Milliseconds);
        }
   

        private List<Bitmap> yuvToBMP(List<int> yuv, int width, int height)

        {
            Bitmap bitmap = new Bitmap("C:/Users/kaank/Source/Repos/YuvCozucu/indir.jpg");
            List<Bitmap> bmpFrames = new List<Bitmap>();
            
            Color r;
            int sayac = 0;
            for(int k = 0; k < frameCount; k++)
            {
                Bitmap bmp = new Bitmap(bitmap, width, height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int pix = yuv.ElementAt(sayac);
                        r = Color.FromArgb(pix, pix, pix);
                        bmp.SetPixel(j, i, r);
                        sayac++;
                    }
                }
                sayac += Convert.ToInt32(frameSize * (katsayi - 1));
                bmpFrames.Add(bmp);
             
             
                
            }
            Console.WriteLine(bmpFrames.Count);
           

            return bmpFrames;
        }

        private void show_Click(object sender, EventArgs e)
        {

        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
 
        }
        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)//Görüntü oynatma button
        {
            timer1.Start();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Console.WriteLine(Convert.ToString(trackBar1.Value));
            pictureBox1.Image = bmpFrames.ElementAt(trackBar1.Value);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            pictureBox1.Image = bmpFrames.ElementAt(sayac);
            sayac++;
            if (sayac >= frameCount)
            {
                timer1.Stop();
                sayac = 0;
            }

        }

        private void button3_Click_1(object sender, EventArgs e)//istenilen frame e gitme
        {


            int index = int.Parse(textBox1.Text);
            if (int.Parse(textBox1.Text)>bmpFrames.Count || int.Parse(textBox1.Text)<0)
            {
                MessageBox.Show("bu frame aralıkta degil!");

            }
            else
            {
                pictureBox1.Image = bmpFrames.ElementAt(index);

            }



        }
        
        /*
        private void matrisyazdir(int index) {
            for (int i = 0; i<height; i++)
            {
                for (int j = 0; j<width; j++)
                {
                    Console.WriteLine(bmpFrames.ElementAt(index).GetPixel(j, i));
                }
            }
        }
        */
        public void kaydetme()
        {
            for (int k = 0; k < frameCount; k++)
            {
                bmpFrames.ElementAt(k).Save("C:\\Users\\kaank\\Documents\\yuv\\yuv1" + k + ".bmp");
                

            }
            MessageBox.Show("fotoğraflar kaydedildi....");
        }

        private void button2_Click(object sender, EventArgs e)//.bmp olarak kaydetme
        {
            kaydetme();
            /*
            try
            {
                if (pictureBox1.Image != null)
                {
                    Image img = pictureBox1.Image;
                    Bitmap bmp = new Bitmap(img.Width, img.Height);
                    Graphics gra = Graphics.FromImage(bmp);
                    gra.DrawImageUnscaled(img, new Point(0, 0));
                    gra.Dispose();

                    string belgelerim = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    bmp.Save(belgelerim + "\\foto.bmp", ImageFormat.Jpeg);

                    MessageBox.Show("görüntü kaydedildi.");
                    bmp.Dispose();
                }
            }
            catch { }
            */
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
          
        }
    }
}


