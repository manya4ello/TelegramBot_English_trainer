using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Показывает все доступные команды в виде клавиатуры
    /// </summary>
    internal class Show : Command, ICommand
    {
        
        public Show()
        {
            CommandName = "Показать команды";
            CommandCode = "/Показать возможные команды";
            Id = 2;
            Father = 1;
            
            Level = ChatStatus.Status.Any;
        }
        public new async  Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            var actualCommands = conversation.actualCommands;

            if (actualCommands.Count >0)
            {
                string text = "Доступные команды:";
                var buttonList = new List<InlineKeyboardButton>();

               
                ICommand command;
                foreach (var commandline in actualCommands)
                {
                    command = commandline.Value;

                    buttonList.Add(new InlineKeyboardButton(command.CommandName)
                    {
                        Text = command.CommandName,
                        CallbackData = command.CommandCode
                    }
                          );
                }


                var keyboard = new InlineKeyboardMarkup(buttonList);

                await botClient.SendTextMessageAsync(
                chatId: conversation.GetId(), text: text, replyMarkup: keyboard);
            }
            return conversation.chatStatus;
        }
    }
}
