using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class DelWord : Command, ICommand
    {


        /// <summary>
        /// Команда инициирующая удаление слова из словаря
        /// </summary>
        public DelWord()
        {
            CommandName = "- слово";
            CommandCode = "/del";
            Id = 12;
            Father = 10;
            
            Level = ChatStatus.Status.Dic;
        }
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Введите русское значение слова, которое хотите удалить:");
                                    
            return ChatStatus.Status.DelWord;
        }
        
    }
}
