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
            CommandName = "Все слова";
            CommandCode = "/showdic";
            Id = 100;
            Father = 10;
            Console.WriteLine($"Создана команда {CommandName}");
        }
        public new async Task Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: "Добро пожаловать в словарь");

            Dictionary.ShowAll(botClient, conversation.GetId());
            
            //Show show = new Show();
            //show.Execute(botClient, conversation);  

        }
    }
}
