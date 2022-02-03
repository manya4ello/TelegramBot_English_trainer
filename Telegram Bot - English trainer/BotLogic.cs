using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_Bot___English_trainer
{
    internal class BotLogic
    {
        private ITelegramBotClient botClient;

        private Dictionary<long, Conversation> chatList;
        public static Dictionary dictionary;
        public static Word wordtoadd;
        public static ChatStatus.Status chatstatus;

        public BotLogic(ITelegramBotClient botClientRes)
        {

            chatList = new Dictionary<long, Conversation>();
            dictionary = new Dictionary();
            dictionary.ReadFile();
            botClient = botClientRes;
            wordtoadd = new Word();
            chatstatus = ChatStatus.Status.Root;
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {

            Console.WriteLine("Я ничего не понял. Давайте по сценарию, как договаривались");

        }

        public async Task CheckIFTXTCommand(ITelegramBotClient botClient, Message message)
        {
            var chatID = message.Chat.Id;

            if (!chatList.ContainsKey(chatID))
            {
                var newchat = new Conversation(message.Chat);
                               

                chatList.Add(chatID, newchat);

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
                    chatId: chatID,
                    text: "Выберите нужную опцию из меню",
                    replyMarkup: replyKeyboardMarkup
                     );

            }
            if (chatstatus==ChatStatus.Status.AddWord || chatstatus == ChatStatus.Status.AddedRus || chatstatus == ChatStatus.Status.AddedEng 
                || chatstatus == ChatStatus.Status.AddedTopic)
                await WorkWithCommand(botClient, chatID, new Commands.AddWord());

            ICommand curCommand;


            if (chatList[chatID].IfCommand(message.Text, out curCommand))
            {
                Console.WriteLine($"Принята команда {curCommand.CommandName}");
                await WorkWithCommand(botClient, chatID, curCommand);
            }

        }

        public async Task CheckCallBackQuerry(ITelegramBotClient botClient, CallbackQuery query)
        {
            var chatID = query.Message.Chat.Id;

            if (!chatList.ContainsKey(chatID))
            {
                var newchat = new Conversation(query.Message.Chat);

                chatList.Add(chatID, newchat);

                Commands.Show show = new Commands.Show();
                show.Execute(botClient, newchat);

            }

            ICommand curCommand;


            if (chatList[chatID].IfCommand(query.Data, out curCommand))
            {
                Console.WriteLine($"Нажата кнопка {query.Data}");
                await WorkWithCommand(botClient, chatID, curCommand);
                await botClient.AnswerCallbackQueryAsync(query.Id);
            }

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => CheckIFTXTCommand(botClient, update.Message),
                UpdateType.EditedMessage => CheckIFTXTCommand(botClient, update.EditedMessage),
                UpdateType.CallbackQuery => CheckCallBackQuerry(botClient, update.CallbackQuery),
                //UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                //UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }


        private async Task WorkWithCommand(ITelegramBotClient botClient, long chatid, ICommand command)
        {
            Console.WriteLine($"Выполняется команда {command.CommandName}");

            var chat = chatList[chatid];
            switch (command)
            {
                case (Commands.Dic):
                    {
                        chat.actualCommands.Clear();
                        chat.actualCommands = chat.Commands.GetChildren(command.Id);
                        break;
                    }
                case (Commands.Test):
                    {
                        chat.actualCommands.Clear();
                        chat.actualCommands = chat.Commands.GetChildren(command.Id);
                        break;
                    }
                case (Commands.Root):
                    {
                        chat.actualCommands.Clear();
                        chat.actualCommands = chat.Commands.GetChildren(1);
                        chatstatus = ChatStatus.Status.Root;
                        break;
                    }
                case (Commands.AddWord):
                    {
                        chat.actualCommands.Clear();
                        chat.actualCommands = chat.Commands.GetChildren(1);
                        chatstatus = ChatStatus.Status.AddWord;
                        break;
                    }
            }


            command.Execute(botClient, chat);

        }

    }
}
