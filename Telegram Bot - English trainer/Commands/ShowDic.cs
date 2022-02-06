using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class ShowDic : Command, ICommand
    {

        /// <summary>
        /// Команда выводит либо все слова словаря (если их 10 или меньше), либо 10 случайных слов
        /// </summary>
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
            string text = String.Empty;
            Random rnd = new Random();
            
            
            if (conversation.dictionary.Vocabulary.Count<1)
            {
                Console.WriteLine($"{DateTime.Now}: чат {conversation.GetId()}: запрос на показ слов. Словарь пуст");
                await botClient.SendTextMessageAsync(conversation.GetId(), "К сожалению, Ваш словарь пуст. Добавьте слова или подгрузите из нашей базы данных.", parseMode: ParseMode.Markdown);
                return ChatStatus.Status.Dic;
            }
                
            if (conversation.dictionary.Vocabulary.Count > 10)
            {
                Console.WriteLine($"{DateTime.Now}: чат {conversation.GetId()}: запрос на показ слов. Показано 10 случайных слов");
                text = $"Всего в словаре {conversation.dictionary.Vocabulary.Count} пар слов" +
                    $"\nНо, из-за ограничений по размеру, высылаю 10 случайных пар";
                

                await botClient.SendTextMessageAsync(conversation.GetId(), text, parseMode: ParseMode.Markdown);

                
                text = "*Тема: \tРусское значение\t-\tАнглийское значение*";
                
               
                int v;
                for (int i = 0; i < 10; i++)
                {
                    v = rnd.Next(conversation.dictionary.Vocabulary.Count);
                    text += $"\n{conversation.dictionary.Vocabulary[v].Topic}: \t{conversation.dictionary.Vocabulary[v].Russian}\t-\t{conversation.dictionary.Vocabulary[v].English}."; 
                                        
                }

                
                await botClient.SendTextMessageAsync(conversation.GetId(), text, parseMode: ParseMode.Markdown);

                return ChatStatus.Status.Dic;
            }

            if ((conversation.dictionary.Vocabulary.Count > 0)&&(conversation.dictionary.Vocabulary.Count < 11))
            {
                Console.WriteLine($"{DateTime.Now}: чат {conversation.GetId()}: запрос на показ слов. Показано {conversation.dictionary.Vocabulary.Count} слов");

                text = $"Всего в словаре {conversation.dictionary.Vocabulary.Count} пар слов";
                await botClient.SendTextMessageAsync(conversation.GetId(), text, parseMode: ParseMode.MarkdownV2);

               
                text = "*Тема: \tРусское значение\t-\tАнглийское значение*";
               

                foreach (Word word in conversation.dictionary.Vocabulary)
                {
                    text += $"\n{word.Topic}: \t{word.Russian}\t-\t{word.English}";
                                        
                }
                  

                await botClient.SendTextMessageAsync(conversation.GetId(), text, parseMode: ParseMode.Markdown);

                return ChatStatus.Status.Dic;
            }

            
            


            return ChatStatus.Status.Dic;  
        }
    }
}
