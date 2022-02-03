using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class WordConfirm : Command, ICommand
    {
        public static readonly string Yes = "Да";
        public static readonly string No = "Нет";

        public WordConfirm()
        {
            CommandName = "Подтвердить";
            CommandCode = "/conf";
            Id = 110;
            Father = 11;
            
        }

        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                    new KeyboardButton[] { Yes, No },
                })
            {
                ResizeKeyboard = true
            };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: conversation.GetId(),
                text: "Подтверждаете?",
                replyMarkup: replyKeyboardMarkup
                );

            return ChatStatus.Status.Dic;
        }



    }
    
}
