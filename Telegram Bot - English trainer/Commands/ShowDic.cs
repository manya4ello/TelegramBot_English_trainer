using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class ShowDic : Command, ICommand
    {
        

        public ShowDic()
            {
            CommandName = "База слов";
            CommandCode = "/showdic";
            Id = 100;
            Father = 10;
            
            Level = ChatStatus.Status.Dic;
        }
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {

            if (conversation.dictionary.Vocabulary.Count<1)
            {
                await botClient.SendTextMessageAsync(conversation.GetId(), "К сожалению, Ваш словарь пуст. Добавьте слова или подгрузите из нашей базы данных.", parseMode: ParseMode.Markdown);
                return ChatStatus.Status.Dic;
            }
                Random rnd = new Random();  
               
                string text = $"Всего в словаре {conversation.dictionary.Vocabulary.Count} пар слов" +
                    $"\nНо, из-за ограничений по размеру, высылаю 10 случайных пар";
                //"\n*Русское значение\t-\tАнглийское значение \t/\tТема*";

                await botClient.SendTextMessageAsync(conversation.GetId(), text, parseMode: ParseMode.Markdown);

                text = "\n*Русское значение\t-\tАнглийское значение \t/\tТема*";
                int v;
                for (int i = 0; i < 10; i++)
                {
                    v = rnd.Next(conversation.dictionary.Vocabulary.Count);
                    Console.WriteLine(v);
                    text += $"\n{conversation.dictionary.Vocabulary[v].Russian}\t-\t{conversation.dictionary.Vocabulary[v].English}\t/\t({conversation.dictionary.Vocabulary[v].Topic})";
                }

                await botClient.SendTextMessageAsync(conversation.GetId(), text, parseMode: ParseMode.Markdown);



            




            return ChatStatus.Status.Dic;  
        }
    }
}
