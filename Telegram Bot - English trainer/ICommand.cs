using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    public interface ICommand
    {
        int Id { get; }
        string CommandName { get; }
        string CommandCode { get; }
        int Father { get; }

        public Task Execute(ITelegramBotClient botClient, Conversation conversation);
    }
}
