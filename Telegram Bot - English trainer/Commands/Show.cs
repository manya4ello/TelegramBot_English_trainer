﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Показывает все доступные команды
    /// </summary>
    internal class Show : Command, ICommand
    {
        
        public Show()
        {
            CommandName = "Показать команды";
            CommandCode = "/show";
            Id = 3;
            Father = 0;

            Console.WriteLine($"Создана команда {CommandName}");
        }
        public new async  Task Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            string text = "Доступные команды:";
            var buttonList = new List<InlineKeyboardButton>();

            ICommand command;
            foreach (var commandline in conversation.actualCommands)
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
    }
}