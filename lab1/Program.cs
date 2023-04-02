using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class Program
    {
        public static char[,] main_matrix = new char[7, 5]
        {
        {'ж', 'щ', 'н', 'ю', 'р'},
        {'и', 'т', 'ь', 'ц', 'б'},
        {'я', 'м', 'е', '.', 'с'},
        {'в', 'ы', 'п', 'ч', ' '},
        {'й', 'д', 'у', 'о', 'к'},
        {'з', 'э', 'ф', 'г', 'ш'},
        {'х', 'а', ',', 'л', 'ъ'}
        };

        public static char[,] first_matrix = new char[7, 5]
        {
        {'ж', 'щ', 'н', 'ю', 'р'},
        {'и', 'т', 'ь', 'ц', 'б'},
        {'я', 'м', 'е', '.', 'с'},
        {'в', 'ы', 'п', 'ч', ' '},
        {'й', 'д', 'у', 'о', 'к'},
        {'з', 'э', 'ф', 'г', 'ш'},
        {'х', 'а', ',', 'л', 'ъ'}
        };

        public static char[,] second_matrix = new char[7, 5]
        {
        {'и', 'ч', 'г', 'я', 'т'},
        {',', 'ж', 'м', 'ь', 'о'},
        {'з', 'ю', 'р', 'в', 'щ'},
        {'ц', 'й', 'п', 'е', 'л'},
        {'ъ', 'а', 'н', '.', 'х'},
        {'э', 'к', 'с', 'ш', 'д'},
        {'б', 'ф', 'у', 'ы', ' '}
        };
        
        static void Main()
        {
            string path = "Text.txt";
            string fileText = File.ReadAllText(path);
            fileText.ToLower();


            Random random = new Random();

            while (true)
            {

                Console.WriteLine("Выберите действие");
                Console.WriteLine("1 - Зашифровать файл");
                Console.WriteLine("2 - Расшифровать файл");
                Console.WriteLine("3 - Зашифровать файл cо своими квадратами");
                Console.WriteLine("4 - Расшифровать файл cо своими квадратами");



                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            var mc = new Code();
                            string EncodeForFile = mc.Encrypt(fileText);
                            System.IO.File.WriteAllText("EncryptText.txt", EncodeForFile);
                            break;
                        }
                    case "2":
                        {
                            var mc = new Code();
                            string DecodeForFile = mc.Decrypt(File.ReadAllText("EncryptText.txt"));
                            System.IO.File.WriteAllText("DecryptText.txt", DecodeForFile);
                            break;
                        }

                    case "3":
                        {

                            Code.MatrixReading();
                            var mc = new Code();
                            string EncodeForFile = mc.Encrypt(fileText);
                            System.IO.File.WriteAllText("EncryptText.txt", EncodeForFile);
                            break;

                        }

                    case "4":
                        {

                            Code.MatrixReading();
                            var mc = new Code();
                            string DecodeForFile = mc.Decrypt(File.ReadAllText("EncryptText.txt"));
                            System.IO.File.WriteAllText("DecryptText.txt", DecodeForFile);
                            break;
                        };

                }
            }
        }
    }
    public class Code
    {
        public static void MatrixReading()
        {
            string path1 = "1matrix.txt";
            string path2 = "2matrix.txt";

            using (StreamReader reader = new StreamReader(path1))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    for (int i = 0; i < 7; i++)
                    {

                        for (int j = 0; j < 5; j++)
                        {
                            Program.first_matrix[i, j] = line[j];

                        }
                        line = reader.ReadLine();
                    }
                }
            }

            using (StreamReader reader = new StreamReader(path2))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Program.second_matrix[i, j] = line[j];

                        }
                        line = reader.ReadLine();
                    }
                }
            }


        }

        public string Encrypt(string text)
        {
            text = text.ToLower();

            string result_text = "";

            if (text.Length % 2 != 0) //делим текст на биграммы
            {
                text += ' '; //добавляем пробел последней биграмме
            }
            int length = text.Length / 2; //длина текста в биграммах
            int k = 0;
            char[,] bigram = new char[length, 2]; //биграмма
            char[,] kripto_bigram = new char[length, 2]; //зашифрованная биграмма

            for (int i = 0; i < length; i++) //заполняем биграмму
            {
                for (int j = 0; j < 2; j++)
                {
                    bigram[i, j] = text[k];
                    k++;
                }
            }

            int step = 0;
            while (step < length-1) //перемешивание букв в биграмме
            {

                Cortege cortege1 = FindIndexes(bigram[step, 0], Program.first_matrix);
                Cortege cortege2 = FindIndexes(bigram[step, 1], Program.second_matrix);

                kripto_bigram[step, 0] = Program.second_matrix[cortege1.I, cortege2.J];
                kripto_bigram[step, 1] = Program.first_matrix[cortege2.I, cortege1.J];

                step++;
            }


            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    result_text += kripto_bigram[i, j].ToString();
                }
            }

            return result_text;
        }


        public string Decrypt(string text)
        {
            string result_text = "";
            text = text + "\t";

            int length = text.Length / 2;
            int k = 0;

            char[,] bigram = new char[length, 2];
            char[,] kripto_bigram = new char[length, 2];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    bigram[i, j] = text[k];
                    k++;
                }
            }

            int step = 0;
            while (step < length-1)
            {
                Cortege cortege1 = FindIndexes(bigram[step, 0], Program.second_matrix);
                Cortege cortege2 = FindIndexes(bigram[step, 1], Program.first_matrix);

                kripto_bigram[step, 0] = Program.first_matrix[cortege1.I, cortege2.J];
                kripto_bigram[step, 1] = Program.second_matrix[cortege2.I, cortege1.J];

                step++;
            }


            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    result_text += kripto_bigram[i, j].ToString();
                }
            }


            return result_text;
        }

        Cortege FindIndexes(char symbol, char[,]
        matrix)
        {
            Cortege cortege = new Cortege();

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (symbol == matrix[i, j])
                    {
                        cortege.I = i;
                        cortege.J = j;
                        return cortege;
                    }
                }
            }

            return null;
        }
    }
}

public class Cortege
{
    public int I { get; set; }
    public int J { get; set; }
    public Cortege() { }
}




