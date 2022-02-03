﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class Mainmenu : Command, ICommand
    {
        
        public Mainmenu()
        {
            CommandName = "Главное меню";
            CommandCode = "/mm";
            Id = 999;
            Father = 0;
            
        }

        public new async Task Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            BotLogic.chatstatus = ChatStatus.Status.Root;

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
                text: "Выберите нужную опцию из меню",
                replyMarkup: replyKeyboardMarkup
                 );

        }



    }
    
}
