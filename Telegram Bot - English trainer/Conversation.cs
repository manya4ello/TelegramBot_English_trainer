using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    /// <summary>
    /// Класс чата. Создается для каждого чата и содержит всю необходимую информацию, привязанную к нему
    /// </summary>
    public class Conversation
    {
        private Chat telegramChat;

        public List<Message> telegramMessages;

        public Dictionary<long, ICommand> actualCommands;
        public Commands.CommandControl Commands; //надо ли?

        public ChatStatus.Status chatStatus;

        public Dictionary dictionary;

        public Word wordtoadd;
        public string wordtodell;

        public Test test;

        public bool firstimerun;
        
        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
            actualCommands = new Dictionary <long, ICommand>(); 
            chatStatus = ChatStatus.Status.Root;
            Commands = new Commands.CommandControl();
                foreach (var command in Commands.CommandsRange)
                    if (command.Father == 1)
                    {
                        Console.WriteLine($"в команды уровня {chatStatus} добавлена команда: {command.CommandName}");
                        actualCommands.Add(command.Id, command);
                    }
            chatStatus = ChatStatus.Status.Root;
            wordtoadd = new Word();
            wordtodell = string.Empty;
            dictionary = new Dictionary();
            test = new Test();
            firstimerun = true;
        }
        /// <summary>
        /// Получить номер чата
        /// </summary>
        /// <returns></returns>
        public long GetId() => telegramChat.Id;
               
        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }
        /// <summary>
        /// Возвращает список текстовых сообщений из чата
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Возвращает последнее сообщение в чате
        /// </summary>
        /// <returns></returns>
        public string GetLastMessage() => telegramMessages[^1].Text;
        /// <summary>
        /// Возвращает номер последнего сообщения
        /// </summary>
        /// <returns></returns>
        public int GetLastMessageID() => telegramMessages[^1].MessageId;

        public bool IfCommand (string message, out ICommand command)
        {
            Console.WriteLine($"на входе  {message}");
            
            ICommand show = new Commands.Show();
            if (message == show.CommandCode)
            {
                command = show;
                return true;
            }

            ICommand root = new Commands.Root();
            if (message == root.CommandCode)
            {
                command = root;
                return true;
            }

            ICommand about = new Commands.About();
            if (message == about.CommandCode)
            {
                command = about;
                return true;
            }

            ICommand instr = new Commands.Instructions();
            if (message == instr.CommandCode)
            {
                command = instr;
                return true;
            }

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
