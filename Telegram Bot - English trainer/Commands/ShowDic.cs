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
            
        }
        public new async Task Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            
            Dictionary.ShowAll(botClient, conversation.GetId());
                        

        }
    }
}
