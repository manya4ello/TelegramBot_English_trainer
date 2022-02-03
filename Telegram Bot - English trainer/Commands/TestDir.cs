using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class TestDir : Command, ICommand
    {
        public static readonly string RusEng = "Рус/Англ";
        public static readonly string EngRus = "Англ/Рус";
        public static readonly string Rand = "Случайным образом";
        public TestDir()
        {
            CommandName = "Направление теста";
            CommandCode = "/testdir";
            Id = 200;
            Father = 20;
           
            Level = ChatStatus.Status.Test;
        }
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                    new KeyboardButton[] { RusEng, EngRus, Rand},
                })
            {
                ResizeKeyboard = true
            };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: conversation.GetId(),
                text: "Выберете один из вариантов",
                replyMarkup: replyKeyboardMarkup
                );

            return ChatStatus.Status.TestInProcess;
        }
    }
}
