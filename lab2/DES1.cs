using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace lab2
{
    public class DES1
    {
        public void Start(int temp)
        {
            try
            {
                byte[] key;
                byte[] iv;
                string original;

                /*using (DES des = DES.Create())
                {
                    key = des.Key;
                    //iv = des.IV;
                }*/


                using (FileStream fstream = File.OpenRead("key.txt"))
                {
                    byte[] buffer = new byte[8];
                    fstream.Read(buffer, 0, buffer.Length);
                    key = buffer;
                    iv = buffer;

                }

                string filename = "crypt.txt";

                if (temp == 1)
                {

                    using (FileStream fstream = File.OpenRead("text.txt"))
                    {
                        byte[] buffer = new byte[fstream.Length];
                        fstream.Read(buffer, 0, buffer.Length);
                        original = Encoding.Default.GetString(buffer);
                    }


                    EncryptTextToFile(original, filename, key, iv);

                } 
                
                else
                {
                    string decrypted = DecryptTextFromFile(filename, key, iv);
                    Console.WriteLine(decrypted);

                    using (StreamWriter writer = new StreamWriter("decrypt.txt", false, Encoding.UTF8))
                    {
                        writer.WriteLine(decrypted);
                    }

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void EncryptTextToFile(string text, string path, byte[] key, byte[] iv)
        {
            try
            {
                using (FileStream fStream = File.Open(path, FileMode.Create))
                using (DES des = DES.Create())
                using (ICryptoTransform encryptor = des.CreateEncryptor(key, iv))
                using (var cStream = new CryptoStream(fStream, encryptor, CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.UTF8.GetBytes(text);

                    cStream.Write(toEncrypt, 0, toEncrypt.Length);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                throw;
            }
        }

        public string DecryptTextFromFile(string path, byte[] key, byte[] iv)
        {
            try
            {
                using (FileStream fStream = File.OpenRead(path))
                using (DES des = DES.Create())
                using (ICryptoTransform decryptor = des.CreateDecryptor(key, iv))
                using (var cStream = new CryptoStream(fStream, decryptor, CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cStream, Encoding.UTF8))
                {

                    return reader.ReadToEnd();
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {1}", e.Message);
                throw;
            }
        }
    }
}
