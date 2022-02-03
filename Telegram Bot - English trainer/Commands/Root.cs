using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Возвращает в корневой каталог из любого места
    /// </summary>
    internal class Root : Command, ICommand
    {
        
        public Root()
        {
            CommandName = "Главное меню";
            CommandCode = "/Главное меню";
            Id = 3;
            Father = 0;
                        
        }
        public new async  Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            string text = "*ГЛАВНОЕ МЕНЮ*";
                        
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: text, parseMode: ParseMode.Markdown);

            ICommand mm = new Mainmenu();
            await mm.Execute(botClient, conversation);
            return ChatStatus.Status.Root;
        }
    }
}
