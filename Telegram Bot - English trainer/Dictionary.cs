using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    internal static class Dictionary
    {
        static List <Word> Vocabulary;
        
        static async void ShowAll(ITelegramBotClient botClient, long ChatId)
        {
            Vocabulary.Sort();
            foreach (Word word in Vocabulary)
            { 
            Console.WriteLine(word);
                await botClient.SendTextMessageAsync(ChatId, $"{word.Russian}\t{word.English}\t{word.Topic}");
            }
        }
    }
}
