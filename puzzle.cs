using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proje1
{
    public partial class frm_puzzle : Form
    {
        public frm_puzzle()
        {
            InitializeComponent();
        }

        Random random = new Random();

        PictureBox temp = new PictureBox();

        List<PictureBox> picboxes;
        List<Image> imageArray, mixedArray;

        Image pic;

        double score = 0;
        double maxScore = 0;

        int tick = 0;
        int counter = 0;
        int countStart;
        int index1 = 0;
        int index2 = 0;


        string path;
        string savePath = @"D:\dersler\yaz_lab2\proje1\proje1\proje1\enyuksekskor.txt";
        

        private void btn_mix_Click(object sender, EventArgs e)
        {
            tick = 0;
            pictureBox19.Visible = false;

            imageArray = new List<Image>();
            mixedArray = new List<Image>();

            int vert = pic.Height / 4;
            int hori = pic.Width / 4;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var index = i * 4 + j;

                    //yataya dikey

                    Bitmap b = new Bitmap(hori, vert);
                    imageArray.Add(b);

                    var graphics = Graphics.FromImage(imageArray[index]);

                    graphics.DrawImage(pic, new Rectangle(0, 0, hori, vert), new Rectangle(i * hori, j * vert, hori, vert), GraphicsUnit.Pixel);
                    graphics.Dispose();
                }
            }

            //birbirinden farklı sayıları ürettik.

            int var;
            var list = new List<int>();

            for (int i = 0; i < 16; i++)
            {
                var = random.Next(0, 16);
                if (!list.Contains(var))
                    list.Add(var);
                else
                    i--;
            }



            //fotoğraf parçalarımızı karıştırıyoruz.

            for (int i = 0; i < 16; i++)
                mixedArray.Add(imageArray[list[i]]);

            {
                pictureBox1.Image = mixedArray[0];
                pictureBox2.Image = mixedArray[1];
                pictureBox3.Image = mixedArray[2];
                pictureBox4.Image = mixedArray[3];
                pictureBox5.Image = mixedArray[4];
                pictureBox6.Image = mixedArray[5];
                pictureBox7.Image = mixedArray[6];
                pictureBox8.Image = mixedArray[7];
                pictureBox9.Image = mixedArray[8];
                pictureBox10.Image = mixedArray[9];
                pictureBox11.Image = mixedArray[10];
                pictureBox12.Image = mixedArray[11];
                pictureBox13.Image = mixedArray[12];
                pictureBox14.Image = mixedArray[13];
                pictureBox15.Image = mixedArray[14];
                pictureBox16.Image = mixedArray[15];
            }


            bool cont;
            countStart = 0;

            // kontrol etme 

            for (int i = 0; i < 16; i++)
            {
                Bitmap bmp1 = new Bitmap(imageArray[i]);
                Bitmap bmp2 = new Bitmap(mixedArray[i]);

                cont = compare(bmp1, bmp2);
                if (cont)
                {
                    countStart++;
                    picboxes[brkFind(bmp2)].BorderStyle = BorderStyle.FixedSingle;
                }

            }

            counter = countStart;
            if (countStart > 2)
            {
                lbl_note.Visible = true;
                lbl_note.Text = countStart + " tane fotoğrafı doğru yerleştirdiniz.";
                btn_mix.Enabled = false;
            }
            else
            {
                lbl_note.Text = "Tekrar karıştırınız.";
                btn_mix.Enabled = true;
            }

            score = countStart * 6.25;
            lbl_score.Text = "" + score;

            if (countStart == 16)
            {
                score = 100;

                lbl_score.Text = "" + score;
                pictureBox19.Visible = true;
            }
        }

        private bool compare(Bitmap bmp1, Bitmap bmp2)
        {
            bool equals = true;
            bool flag = true;  //Inner loop isn't broken

            //bmpleri karşılaştırmak için önce boyutlarının ayı olup olmadığının kontrolü yapılır
            if (bmp1.Size == bmp2.Size)
            {
                for (int x = 0; x < bmp1.Width; ++x)
                {
                    for (int y = 0; y < bmp1.Height; ++y)
                    {
                        if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                        {
                            equals = false;
                            flag = false;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
            }
            else
            {
                equals = false;
            }
            return equals;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tick++;
            lbl_note.Text = "Hamle sayısı :  " + tick;

            if (tick > 24)
                score -= 3;

            if (tick % 2 != 0)
            {
                temp = (PictureBox)sender;
            }
            else
            {
                swap(temp, (PictureBox)sender);
            }
        }

        public void swap(PictureBox p1, PictureBox p2)
        {
            Bitmap bmp1 = new Bitmap(p1.Image);
            Bitmap bmp2 = new Bitmap(p2.Image);

            index1 = brkFind(bmp1);
            index2 = brkFind(bmp2);

            PictureBox pb = new PictureBox();
            pb.Image = picboxes[index1].Image;
            picboxes[index1].Image = picboxes[index2].Image;
            picboxes[index2].Image = pb.Image;


            //fotoğraflar doğru yerinde mi kontrolü yapılıyor

            bmp1 = (Bitmap)picboxes[index1].Image;

            if (compare(bmp1, (Bitmap)imageArray[index1]))
            {
                counter++;
                p1.BorderStyle = BorderStyle.FixedSingle;
                //lbl_score.Visible = true;
                score += 6.25;
                lbl_score.Text = score.ToString();
            }

            bmp2 = (Bitmap)picboxes[index2].Image;

            if (compare(bmp2, (Bitmap)imageArray[index2]))
            {
                counter++;
                p2.BorderStyle = BorderStyle.FixedSingle;
                //lbl_score.Visible = true;
                score += 6.25;
                lbl_score.Text =score.ToString();
            }


            if (counter == 16)
            {
                lbl_score.Text =score.ToString();

                pictureBox19.Image = Image.FromFile(path);
                pictureBox19.Visible = true;

                FileStream fs = new FileStream(savePath, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(score);
                sw.Flush();
                //Veriyi tampon bölgeden dosyaya aktardık.
                sw.Close();
                fs.Close();

                if (maxScore < score)
                {
                    maxScore = score;
                    lbl_top_score.Text = maxScore.ToString();
                    MessageBox.Show("Tebrikler \n Yeni En Yüksek Skor");
                }
                else
                    MessageBox.Show("Tebrikler ");
                btn_mix.Enabled = true;
            }
        }

        private void btn_photo_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "JPG Dosyası |*.jpg| PNG Dosyası |*.png| BMP Dosyası |*.bmp";
            opf.Title = "Fotoğraf Seç";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                lbl_note.Text = "Karıştır butonuna basınız.";
                path = opf.FileName;
                this.pic = Image.FromFile(path);
                btn_mix.Enabled = true;
            }
            else
            {
                lbl_note.Text = "Lütfen önce fotoğraf seçiniz.";
            }
        }

        private void frm_puzzle_Load(object sender, EventArgs e)
        {

            {
                picboxes = new List<PictureBox>();
                picboxes.Add(pictureBox1);
                picboxes.Add(pictureBox2);
                picboxes.Add(pictureBox3);
                picboxes.Add(pictureBox4);
                picboxes.Add(pictureBox5);
                picboxes.Add(pictureBox6);
                picboxes.Add(pictureBox7);
                picboxes.Add(pictureBox8);
                picboxes.Add(pictureBox9);
                picboxes.Add(pictureBox10);
                picboxes.Add(pictureBox11);
                picboxes.Add(pictureBox12);
                picboxes.Add(pictureBox13);
                picboxes.Add(pictureBox14);
                picboxes.Add(pictureBox15);
                picboxes.Add(pictureBox16);
            }


            FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);


            //en yüksek puan bulunacak

            string top1 = sw.ReadLine();
            string top2;
            int t1;
            int t2;
            while (top1 != null)
            {
                t1 = Int16.Parse(top1);
                maxScore = t1;

                top2 = sw.ReadLine();
                if (top2 != null)
                {
                    
                    t2 = Int16.Parse(top2);
                    if (t1 > t2)
                    {
                        maxScore = t1;
                    }
                }
                top1 = sw.ReadLine();
            }

            sw.Close();
            fs.Close();

            lbl_top_score.Text = maxScore.ToString();
        }

        public int brkFind(Bitmap bmp)
        {
            //picturebox listesinde gönderdiğin bitmapi arar
            Bitmap picMap;
            for (int i = 0; i < picboxes.Count(); i++)
            {
                picMap = new Bitmap(picboxes[i].Image);
                if (compare(bmp, picMap))
                {
                    return i;
                }

            }
            MessageBox.Show("Aranılan eleman listede bulunamadı.");
            return (-1);

        }


    }


}
