using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Показывает все доступные команды в данный момент в виде сообщения в чате
    /// </summary>
    internal class Instructions : Command, ICommand
    {
        public string Content;
        public Instructions()
        {
            CommandName = "Активные команды";
            CommandCode = "/inst";
            Id = 998;
            Father = 0;
          
            Level = ChatStatus.Status.Any;
            Content = String.Empty;
        }
        public new async  Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            Content = "*На данном этапе вам доступны команды:*";
            foreach (var command in conversation.actualCommands)
            {
                Content += $"\n*{command.Value.CommandName}:* {command.Value.CommandCode}";
            }
            Content += "\nА также в любой момент вы можете воспользоваться:";

            foreach (var command in conversation.Commands.CommandsRange)
            {
                if (command.Level == ChatStatus.Status.Any) 
                Content += $"\n*{command.CommandName}:* {command.CommandCode}";
            }

            await botClient.SendTextMessageAsync(
            chatId: conversation.GetId(), text: Content, parseMode: ParseMode.Markdown);

            return conversation.chatStatus; ;
        }
    }
}
