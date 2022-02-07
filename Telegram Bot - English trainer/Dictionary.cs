using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Telegram_Bot___English_trainer
{
    /// <summary>
    /// Определяет словарь
    /// </summary>
    public class Dictionary
    {
        public List<Word> Vocabulary;
        public static Random rnd;
        public Dictionary()
        {
            Vocabulary = new List<Word>();
            rnd = new Random();
            AddTemplate();
            try
            {
                ReadFile(5);
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message); 
            }

        }

        /// <summary>
        /// Скачивает слова из файла (проверяя нет ли дублирования)
        /// </summary>
        /// <param name="numb">Если равен 0, то добавляются все слова, если >0, то случайные слова в колличестве полученного числа</param>
        public void ReadFile(int numb =0)
        {
            Random random = new Random();   
            List<Word> FromFile =new List<Word>();
            List<Word> FromFileOnlyNew = new List<Word>();
            // string sourcefile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Dic utf8.csv";
            string sourcefile = Directory.GetCurrentDirectory() + @"\Dic utf8.csv";
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

                    Console.WriteLine($"{DateTime.Now}: Подгружаем данные из файла \t{sourcefile}");

                    bool contains = false;
                    foreach (Word wordfromfile in FromFile)
                    {
                        foreach (Word wordfromdic in Vocabulary)
                            if (wordfromfile.Russian == wordfromdic.Russian)
                                contains = true;
                        if (!contains)
                            FromFileOnlyNew.Add(wordfromfile);
                        contains = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //если параметр не введен - заливаем все данные в славарь
            if (numb==0)
            {
                bool contains = false;
                foreach (Word wordfromfile in FromFileOnlyNew)
                {
                    foreach (Word wordfromdic in Vocabulary)
                        if (wordfromfile.Russian == wordfromdic.Russian)
                            contains = true;
                    if (!contains)
                        Vocabulary.Add(wordfromfile);
                    contains = false;
                }
            }
            else
            {
                for (int i = 0; (i < numb)&&(i<FromFileOnlyNew.Count); i++)
                {
                    Vocabulary.Add((Word)FromFileOnlyNew[random.Next(FromFileOnlyNew.Count)]);
                }
            }

            
        }

        /// <summary>
        /// На случай, если с файлом проблемы - заливает 5 слов
        /// </summary>
        public void AddTemplate()
        {
            List<Word> words = new List<Word>()
            {
                new Word("кошка", "cat", "животные"),
                new Word("собака", "dog", "животные"),
                new Word("енот", "racoon", "животные"),
                new Word("жена", "wife", "семья"),
                new Word("ребенок", "child", "семья")
            };

            
            foreach (Word word in words)
                            Vocabulary.Add(word);
             
        }
       public List<string> GetRandQuestion(bool ruseng, Word target, int numanswers)
        {
            Random random = new Random();

            List<string> wrong = new List<string>();
            Word randword = new Word();

            for (int i = 0; i < numanswers; i++)
            {
                randword = Vocabulary[random.Next(Vocabulary.Count)];
                if (randword == target)
                    i--;
                else
                    if (ruseng)
                    wrong.Add(randword.English);
                    else
                    wrong.Add(randword.Russian);

            }

           
            return wrong;
        }
        
    }


}
