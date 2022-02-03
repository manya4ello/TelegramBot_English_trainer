using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    public abstract class Command : ICommand
    {
        public string CommandName { get; set; }

        public string CommandCode { get; set; }

        public int Id { get; set; }

        public int Father { get; set; }

       

        public ChatStatus.Status Level { get; set; }

        public Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            throw new NotImplementedException();
        }
    }
}
