﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class LoadDic : Command, ICommand
    {

        /// <summary>
        /// Команда инициирующая добавление всех слов из файла в словарь
        /// </summary>
        public LoadDic()
            {
            CommandName = "Подгрузить слова из базы";
            CommandCode = "/loaddic";
            Id = 13;
            Father = 10;
            
            Level = ChatStatus.Status.Dic;
        }
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {

            var start = conversation.dictionary.Vocabulary.Count;
            conversation.dictionary.ReadFile();

            if (start == conversation.dictionary.Vocabulary.Count)
            { 
                await botClient.SendTextMessageAsync(conversation.GetId(), $"К сожалению, мы не смогли найти новых слов. У вас в словаре по прежнему {conversation.dictionary.Vocabulary.Count} пар слов", parseMode: ParseMode.Markdown);
                return ChatStatus.Status.Dic;
            }

            string text = $"Всего в словаре было {start} пар слов";

            

            text += $"\nТеперь стало {conversation.dictionary.Vocabulary.Count} пар слов" +
                $"\nПоздравляем, загрузка прошла успешно!";


            await botClient.SendTextMessageAsync(conversation.GetId(), text, parseMode: ParseMode.Markdown);
             
                   




            return ChatStatus.Status.Dic;  
        }
    }
}
