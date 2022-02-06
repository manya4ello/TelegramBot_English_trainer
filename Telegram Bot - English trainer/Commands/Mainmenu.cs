using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class Mainmenu : Command, ICommand
    {
        /// <summary>
        /// Команда выводит основные команды бота, доступные на любом уровне, в клавиатуру
        /// </summary>
        public Mainmenu()
        {
            CommandName = "Команды";
            CommandCode = "/mm";
            Id = 999;
            Father = 1;
           
            Level = ChatStatus.Status.Root;
        }

        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            conversation.chatStatus = ChatStatus.Status.Root;

            ICommand show = new Commands.Show();
            ICommand root = new Commands.Root();
            ICommand about = new Commands.About();
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
             {
                     new KeyboardButton[] { about.CommandCode, root.CommandCode, show.CommandCode },
                 })
            {
                ResizeKeyboard = true
            };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: conversation.GetId(),
                text: "Установленна клавиатура с возможностью выбора",
                replyMarkup: replyKeyboardMarkup
                 );

            return ChatStatus.Status.Root;
        }



    }
    
}
