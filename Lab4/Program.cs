using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Lab4
{
    class Program
    {
        public static byte[] buffer;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите действие");
                Console.WriteLine("1 - Зашифровать файл");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            using (FileStream fstream = File.OpenRead("text.txt"))
                            {
                                buffer = new byte[fstream.Length];
                                fstream.Read(buffer, 0, buffer.Length);
                            }

                            SHA512 shaM = new SHA512Managed();
                            byte[] data = shaM.ComputeHash(buffer);

                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < data.Length; i++)
                            {
                                sb.Append(data[i].ToString("X2"));
                            }
                            string hash = sb.ToString().ToLower();
                            System.IO.File.WriteAllText("encode.txt", hash);

                            break;
                        }
                }
            }
        }
    }
}