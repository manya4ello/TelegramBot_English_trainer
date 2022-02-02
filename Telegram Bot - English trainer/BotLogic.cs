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

        public BotLogic(ITelegramBotClient botClientRes)
        {
           
            chatList = new Dictionary<long,
            Conversation>();
            botClient = botClientRes;
        }

        public  Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public  async Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
           
           Console.WriteLine("Я ничего не понял. Давайте по сценарию, как договаривались");
           
        }

        public async Task CheckIFTXTCommand (ITelegramBotClient botClient, Message message)
        {
            var chatID = message.Chat.Id;

            if (!chatList.ContainsKey(chatID))
            {
                var newchat = new Conversation(message.Chat);

                chatList.Add(chatID, newchat);

                Commands.Show show = new Commands.Show();
                show.Execute(botClient, newchat);
                               
            }
            
            ICommand curCommand;

            if (chatList[chatID].IfCommand(message.Text, out curCommand))
            {
                Console.WriteLine($"Принята команда {curCommand.CommandName}");
                curCommand.Execute(botClient, chatList[chatID]);
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

            if (chatList[chatID].IfCommand(query.Message.Text, out curCommand))
            {
                Console.WriteLine($"Принята команда {curCommand.CommandName}");
                curCommand.Execute(botClient, chatList[chatID]);
            }

           // await botClient.AnswerCallbackQueryAsync(
           //callbackQueryId: query.Id,
           //text: $"Received {query.Data}");
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
                UpdateType.Message => CheckIFTXTCommand (botClient, update.Message),
                UpdateType.EditedMessage => CheckIFTXTCommand (botClient, update.EditedMessage),
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
        private async Task SendTextMessage(ITelegramBotClient botClient, Conversation chat)
        {
            var text = "messenger.CreateTextMessage(chat)";

            await botClient.SendTextMessageAsync(
            chatId: chat.GetId(), text: text);
        }

        private async Task SendKeyBoard(ITelegramBotClient botClient, Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(
            chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }

    }
}
