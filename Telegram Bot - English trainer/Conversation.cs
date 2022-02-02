using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    public class Conversation
    {
        private Chat telegramChat;

        public List<Message> telegramMessages;

        public Dictionary<long, ICommand> actualCommands;
        private Commands.CommandControl Commands; //надо ли?

        ChatStatus.Status chatStatus;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
            actualCommands = new Dictionary <long, ICommand>(); //сразу надо закидывать команды уровня 0
            chatStatus = ChatStatus.Status.Root;
            Commands = new Commands.CommandControl();
                foreach (var command in Commands.CommandsRange)
                    if (command.Father == 0)
                    {
                        Console.WriteLine($"в команды уровня {chatStatus} добавлена команда: {command.CommandName}");
                        actualCommands.Add(command.Id, command);
                    }
           
        }

        public long GetId() => telegramChat.Id;
        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach (var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public string GetLastMessage() => telegramMessages[^1].Text;

        public bool IfCommand (string message, out ICommand command)
        {
            foreach (var commandline in actualCommands)
                if (actualCommands[commandline.Key].CommandCode.Equals(message??""))
                {
                    command = actualCommands[commandline.Key];
                    return true;
                }
            command = null;
            return false;
        }

        private async Task ShowCommands(ITelegramBotClient botClient, Conversation chat)
        {
            string text = "Доступные команды:";
            var buttonList = new List<InlineKeyboardButton>();

            ICommand command;
            foreach (var commandline in actualCommands)
            {
                command = commandline.Value;
                
                buttonList.Add(new InlineKeyboardButton (command.CommandName)
                    {
                        Text = command.CommandName,
                        CallbackData = command.CommandCode
                    }
                      );
            }

                        

                var keyboard = new InlineKeyboardMarkup(buttonList);

               
            

            await botClient.SendTextMessageAsync(
            chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }

    }
}
