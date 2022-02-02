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

        public Dictionary<long, Commands.Command> actualCommands;

        ChatStatus.Status chatStatus;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
            actualCommands = new Dictionary <long,Commands.Command>(); //сразу надо закидывать команды уровня 0
            chatStatus = ChatStatus.Status.Root;
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

        public bool IfCommand (string message)
        {
            foreach (var command in actualCommands)
                if command.
        }

    }
}
