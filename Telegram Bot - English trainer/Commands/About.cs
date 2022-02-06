using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Выводит краткую информацию о проекте
    /// </summary>
    internal class About : Command, ICommand
    {
        
        public About()
        {
            CommandName = "О проекте";
            CommandCode = "/О проекте";
            Id = 1;
            Father = 1;
          
            Level = ChatStatus.Status.Any;
            
        }
        public new async  Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            string text = "*О данном проекте:*" +
                "\nРобот-тренер по английскому языку разработан в рамках курса \"Разработчик С#\"" +
                "\nНадеемся он будет вам полезен!" +
                "\n_С уважением," +
                "\nManya4ello_";
                        
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: text, parseMode: ParseMode.Markdown);


            return conversation.chatStatus; ;
        }
    }
}
