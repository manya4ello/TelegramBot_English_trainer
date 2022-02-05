using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Команда останавливает тест на любом вопросе
    /// </summary>
    internal class TestStop : Command, ICommand
    {
        
        public TestStop()
        {
            CommandName = "Останавливает тест";
            CommandCode = "/stop";
            Id = 300;
            Father = 20;
           
            Level = ChatStatus.Status.TestInProcess;
        }
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            await botClient.SendTextMessageAsync(
           chatId: conversation.GetId(), text: "Тест прерван");


            return ChatStatus.Status.Root;
        }
    }
}
