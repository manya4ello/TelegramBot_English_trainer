using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Telegram_Bot___English_trainer
{
    public class Dictionary
    {
        public List<Word> Vocabulary;
        public static Random rnd;
        public Dictionary()
        {
            Vocabulary = new List<Word>();
            rnd = new Random();

        }

        public void ReadFile()
        {

            List<Word> FromFile =new List<Word>();
            string sourcefile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Dic utf8.csv";
            string text;

            try
            {


                using (FileStream fstream = System.IO.File.OpenRead(sourcefile))
                {

                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    string textFromFile = System.Text.Encoding.UTF8.GetString(array);

                    string[] lines = textFromFile.Split("\n");


                    foreach (string line in lines)
                    {
                        if (line.Length > 1)
                        {
                            string[] parts = line.Split(";");
                            Word word = new Word() { Russian = parts[0], English = parts[1], Topic = parts[2] };
                            FromFile.Add(word);
                        }
                    }

                    Console.WriteLine($"Читаем данные из файла \t{sourcefile}");

                    bool contains = false;
                    foreach (Word wordfromfile in FromFile)
                    {
                        foreach (Word wordfromdic in Vocabulary)
                            if (wordfromfile.Russian == wordfromdic.Russian)
                                contains = true;
                        if (!contains)
                            Vocabulary.Add(wordfromfile);
                        contains = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

       

    }


}
