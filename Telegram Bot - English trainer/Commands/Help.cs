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
            Id = 9;
            Father = 0;
          
            Level = ChatStatus.Status.Root;
            
        }
        public new async  Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            string text = "*В помощь пользователю*" +
                "\nДля Вашего удобства навигация осуществляется кнопками" +
                "\nУ данного робота есть два основных блока:" +
                "\n1.*Словарь:* в нем вы можете добавлять новые слова и удалять их, а также подгрузить небольшой, подготовленный заранее, словарь" +
                "\n2.*Тест:* вы можете проверить свои знания, вводя значения слов на английском или русском. Для остановки теста в любой момент введите команду /stop";
                        
            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: text, parseMode: ParseMode.Markdown);

            return conversation.chatStatus; ;
        }
    }
}
