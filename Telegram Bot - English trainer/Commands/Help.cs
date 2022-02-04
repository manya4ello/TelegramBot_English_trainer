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
    internal class Help : Command, ICommand
    {
        
        public Help()
        {
            CommandName = "Помощь";
            CommandCode = "/help";
            Id = 1;
            Father = 1;
          
            Level = ChatStatus.Status.Any;
            
        }
        public new async  Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            string text = "*В помощь пользователю*" +
                "\nРобот-тренер по английскому языку разработан в рамках курса \"Разработчик С#\"" +
                "\nНадеемся он будет вам полезен!" +
                "\nС уважением," +
                "\nManya4ello";
                        
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: text, parseMode: ParseMode.Markdown);

            return conversation.chatStatus; ;
        }
    }
}
