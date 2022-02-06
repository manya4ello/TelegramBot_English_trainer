using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Команда инициирующая добавление слова в словарь
    /// </summary>
    internal class AddWord : Command, ICommand
    {
        
        public AddWord()
        {
            CommandName = "+ слово";
            CommandCode = "/add";
            Id = 11;
            Father = 10;
           
            Level = ChatStatus.Status.Dic;
        }

       
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
                        
            await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Введите значение на русском:");

            
            return ChatStatus.Status.AddWord;
        }
    }
}
