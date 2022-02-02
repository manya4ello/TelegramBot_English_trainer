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
            Id = 1;
            Father = 0;
            Console.WriteLine($"Создана команда {CommandName}");
        }
        public new async Task Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: "Добро пожаловать в словарь");

            Show show = new Show();
            show.Execute(botClient, conversation);  
        }
    }
}
