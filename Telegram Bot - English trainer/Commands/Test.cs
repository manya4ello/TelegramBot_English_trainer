﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class Test : Command, ICommand
    {
        
        public Test()
        {
            CommandName = "Тест";
            CommandCode = "/test";
            Id = 20;
            Father = 1;
            Level = ChatStatus.Status.Root;
        }
        public new async Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {

            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: "Для начала теста выберите направление перевода", parseMode: ParseMode.Markdown);

            ICommand confirm = new Commands.TestDir();
            confirm.Execute(botClient, conversation);

            return ChatStatus.Status.Test;
        }
    }
}
