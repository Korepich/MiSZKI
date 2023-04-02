using System;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using static lab2.DES1;
using static System.Net.Mime.MediaTypeNames;


namespace lab2
{
    class Program
    {
        static void Main()
        {
            var ob = new DES1();

            while (true)
            {
                Console.WriteLine("press 1 to encrypt");
                Console.WriteLine("press 2 to decrypt");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            ob.Start(1);
                            Console.WriteLine("encrypt text in encrypt.txt");
                            break;
                        }

                    case "2":
                        {
                            ob.Start(2);
                            Console.WriteLine("decrypt file in decrypt.txt");
                            break;
                        }
                }
            }
        }
    }
}