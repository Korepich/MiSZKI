using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public static byte[] img;
        public static byte[] buffer1;
        public static byte[] AllByte;
        public static byte[] key;

        public static BigInteger LRS1 = new BigInteger(18437701690082721791);
        public static BigInteger LRS2 = BigInteger.Parse("147501613520661774335");
        public static BigInteger LRS3 = BigInteger.Parse("492216959415791509455859");

        public Form1()
        {
            InitializeComponent();
            Bitmap btmp = (Bitmap)Image.FromFile("sanya.bmp");
            pictureBox1.Image = btmp;
        }

        private void Encode_Click(object sender, EventArgs e)
        {
            Bitmap imge = new Bitmap("sanya.bmp");
            byte[] img =  ConvertBitMapToByte(imge);
            pictureBox1.Image = ConvertByteToBitMap(img);
            using (FileStream fstream = File.OpenRead("text.txt"))
            {
                // выделяем массив для считывания данных из файла
                key = new byte[fstream.Length];
                // считываем данные
                fstream.Read(key, 0, key.Length);
            }
            byte[] data = crypt.Encrypt(img);
            Bitmap b = new Bitmap(ConvertByteToBitMap(data));
            b.Save("Encode.bmp", ImageFormat.Bmp);
            pictureBox1.Image = b;
        }

        private byte[] ConvertBitMapToByte(Bitmap img)
        {
            byte[] Result = null;
            BitmapData bData = img.LockBits(new Rectangle(new Point(), img.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int ByteCount = bData.Stride * img.Height;
            Result = new byte[ByteCount];
            Marshal.Copy(bData.Scan0, Result, 0, ByteCount);
            img.UnlockBits(bData);
            return Result;
        }

        private Bitmap ConvertByteToBitMap(byte[] Ishod)
        {
            Bitmap img = new Bitmap(446, 513);
            BitmapData bData = img.LockBits(new Rectangle(new Point(), img.Size), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(Ishod, 0, bData.Scan0, Ishod.Length);
            img.UnlockBits(bData);
            return img;
        }

        private void Decode_Click(object sender, EventArgs e)
        {
            LRS1 = new BigInteger(18437701690082721791);
            LRS2 = BigInteger.Parse("147501613520661774335");
            LRS3 = BigInteger.Parse("492216959415791509455859");

            Bitmap imge = (Bitmap)Image.FromFile("Encode.bmp");
            ImageConverter imgCon = new ImageConverter();
            img = ConvertBitMapToByte(imge);
            using (FileStream fstream = File.OpenRead("text.txt"))
            {
                // выделяем массив для считывания данных из файла
                key = new byte[fstream.Length];
                // считываем данные
                fstream.Read(key, 0, key.Length);
            }
            byte[] data = crypt.Encrypt(img);
            Bitmap b = new Bitmap(ConvertByteToBitMap(data));
            b.Save("Decode.bmp", ImageFormat.Bmp);
            pictureBox1.Image = b;

        }
    }

    internal class crypt
    {
        public static byte[] Encrypt(byte[] data)
        {
            int a, i, j, k, tmp;
            int[] codeKey, box;
            byte[] cipher;

            cipher = new byte[data.Length];

            for (i = 0; i < data.Length; i++)
            {
                if (i > 255)
                {
                    cipher[i] = (byte)(data[i] ^ gamma());

                }
                else
                {
                    cipher[i] = data[i];
                }

            }


            return cipher;
        }


        public static byte gamma()
        {
            BigInteger back;
            byte y = 0;

            for (int i = 0; i < 8; i++)
            {
                byte bit1 = (byte)(Form1.LRS1 % 2);

                back = ((Form1.LRS1 >> 63) % 2) ^ ((Form1.LRS1 >> 3) % 2) ^ ((Form1.LRS1 >> 2) % 2) ^ (Form1.LRS1 % 2);
                Form1.LRS1 = (back << 64) | (Form1.LRS1 >> 1);


                byte bit2 = (byte)(Form1.LRS2 % 2);

                back = ((Form1.LRS2 >> 66) % 2) ^ ((Form1.LRS2 >> 4) % 2) ^ ((Form1.LRS2 >> 1) % 2) ^ (Form1.LRS2 % 2);
                Form1.LRS2 = (back << 64) | (Form1.LRS2 >> 1);

                byte bit3 = (byte)(Form1.LRS3 % 2);

                back = ((Form1.LRS3 >> 78) % 2) ^ ((Form1.LRS3 >> 3) % 2) ^ ((Form1.LRS3 >> 2) % 2) ^ ((Form1.LRS3 >> 1) % 2);
                Form1.LRS3 = (back << 64) | (Form1.LRS3 >> 1);

                //if ((bit1 & bit2) || (bit1 & bit3) || (bit3 & bit2)) == 1)
                //bool kolya = bit1 & bit2 == 1;
                if (bit1+bit2+bit3 >= 2)  y = (byte)((y << 1) | 1);
                

                else 
                    y = (byte)((y << 1) | 0);


            }

            return y;
        }

        public static byte[] Decrypt(byte[] data)
        {
            return Encrypt(data);
        }
    }
}
