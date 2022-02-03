using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class Dic : Command, ICommand
    {
        

        public Dic()
            {
            CommandName = "Словарь";
            CommandCode = "/dic";
            Id = 10;
            Father = 1;
           
            Level = ChatStatus.Status.Root;
            
        }
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: "Добро пожаловать в словарь");
                      

            return ChatStatus.Status.Dic;
        }
    }
}
