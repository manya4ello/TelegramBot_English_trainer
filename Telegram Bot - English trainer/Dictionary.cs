using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Telegram_Bot___English_trainer
{
    internal class Dictionary
    {
        public static List<Word> Vocabulary;
        public static Random rnd;
        public Dictionary()
        {
            Vocabulary = new List<Word>();
            rnd = new Random();

        }

        public void ReadFile()
        {

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
                            Vocabulary.Add(word);
                        }
                    }

                    Console.WriteLine($"Читаем данные из файла \t{sourcefile}");
                    //foreach (Word item in Vocabulary)
                    //    Console.WriteLine($"{item.Russian} \t{item.English} \t{item.Topic}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        public static async void ShowAll(ITelegramBotClient botClient, long ChatId)
        {


            string text = $"Всего в словаре {Vocabulary.Count} пар слов" +
                $"\nНо, из-за ограничений по размеру, высылаю 10 случайных пар"; 
                //"\n*Русское значение\t-\tАнглийское значение \t/\tТема*";

            await botClient.SendTextMessageAsync(ChatId, text, parseMode: ParseMode.Markdown);

            text = "\n*Русское значение\t-\tАнглийское значение \t/\tТема*";
            int v;
            for (int i = 0; i < 10; i++)
            {
                v = rnd.Next(Vocabulary.Count);
                Console.WriteLine(v);
                text += $"\n{Vocabulary[v].Russian}\t-\t{Vocabulary[v].English}\t/\t({Vocabulary[v].Topic})";
            }
                        
            await botClient.SendTextMessageAsync(ChatId, text, parseMode: ParseMode.Markdown);



        }

    }


}
