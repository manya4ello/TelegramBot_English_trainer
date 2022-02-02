using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Показывает все доступные команды
    /// </summary>
    internal class About : Command, ICommand
    {
        
        public About()
        {
            CommandName = "О проекте";
            CommandCode = "/О проекте";
            Id = 1;
            Father = 1;

            Console.WriteLine($"Создана команда {CommandName}");
        }
        public new async  Task Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            string text = "*О данном проекте:*" +
                "\nРобот-тренер по английскому языку разработан в рамках курса \"Разработчик С#\"" +
                "\nНадеемся он будет вам полезен!" +
                "\nС уважением," +
                "\nManya4ello";
                        
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: text, parseMode: ParseMode.Markdown);
                       

        }
    }
}
